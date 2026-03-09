using PuppeteerSharp;
using Point = System.Drawing.Point;
using Size  = System.Drawing.Size;

namespace Crawler
{
    public partial class Form1 : Form
    {
        private record CrawlTarget(string Name, string Url, string[] XPaths);

        private static readonly CrawlTarget[] Targets =
        [
            new("口碑聲量排行 · 成真咖啡",
                "https://dailyview.tw/top100/item/10915?theme=68&range=30",
                [
                    "/html/body/div[2]/main/div[1]/div",
                    "/html/body/div[2]/main/div[2]/div/div[1]/div[1]",
                    "/html/body/div[2]/main/div[2]/div/div[1]/div[2]",
                    "/html/body/div[2]/main/div[2]/div/div[1]/div[3]",
                    "/html/body/div[2]/main/div[2]/div/div[1]/div[4]",
                ]),
            new("咖啡連鎖 TOP 排行",
                "https://dailyview.tw/top100/topic/68",
                [
                    "/html/body/div[2]/div[4]/main/div[2]",
                    "/html/body/div[2]/div[4]/main/div[6]",
                ]),
        ];

        public Form1()
        {
            InitializeComponent();
            foreach (var t in Targets)
                cboTarget.Items.Add(t.Name);
            cboTarget.SelectedIndex = 0;
        }

        private async void btnCrawl_Click(object sender, EventArgs e)
            => await RunCrawlAsync([Targets[cboTarget.SelectedIndex]]);

        private async void btnCrawlAll_Click(object sender, EventArgs e)
            => await RunCrawlAsync(Targets);

        private async Task RunCrawlAsync(CrawlTarget[] targets)
        {
            btnCrawl.Enabled    = false;
            btnCrawlAll.Enabled = false;
            btnSave.Enabled     = false;
            pbProgress.Visible  = true;
            ClearImagePanel();

            try
            {
                // ── 1. 下載 Chromium（首次執行約 200MB，之後會快取）──────────────
                SetStatus("⏳ 正在準備瀏覽器核心（首次執行需下載，請稍候）...");
                await new BrowserFetcher().DownloadAsync();

                // ── 2. 啟動無頭瀏覽器 ─────────────────────────────────────────────
                SetStatus("🌐 啟動瀏覽器...");
                await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = true,
                    Args = ["--no-sandbox", "--disable-setuid-sandbox", "--disable-gpu"]
                });

                await using var page = await browser.NewPageAsync();
                await page.SetViewportAsync(new ViewPortOptions { Width = 1440, Height = 900, DeviceScaleFactor = 2 });

                int totalXPaths = targets.Sum(t => t.XPaths.Length);
                int done = 0;

                // ── 3. 依序處理每個目標頁面 ───────────────────────────────────────
                foreach (var target in targets)
                {
                    SetStatus($"🌐 載入頁面：{target.Name}...");
                    await page.GoToAsync(target.Url, new NavigationOptions
                    {
                        WaitUntil = [WaitUntilNavigation.Networkidle0],
                        Timeout = 30_000
                    });

                    // 移除 /html/body/header，避免它遮蓋或出現在截圖中
                    await page.EvaluateExpressionAsync(
                        "document.evaluate('/html/body/header', document, null, " +
                        "XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue?.remove()");

                    for (int i = 0; i < target.XPaths.Length; i++)
                    {
                        done++;
                        SetStatus($"📸 [{target.Name}] 第 {i + 1}/{target.XPaths.Length} 個元素（總進度 {done}/{totalXPaths}）");
                        await CaptureElementAsync(page, target.XPaths[i], done);
                    }
                }

                int found = flowPanel.Controls.Cast<Panel>()
                    .Count(p => p.Controls.OfType<PictureBox>().Any());
                btnSave.Enabled = found > 0;
                SetStatus($"✅ 完成！共擷取 {found} / {totalXPaths} 個元素。");
            }
            catch (Exception ex)
            {
                SetStatus($"❌ 錯誤：{ex.Message}");
            }
            finally
            {
                btnCrawl.Enabled    = true;
                btnCrawlAll.Enabled = true;
                pbProgress.Visible  = false;
            }
        }

        /// <summary>
        /// 用 JS evaluate 定位絕對 XPath 元素，然後直接截圖。
        /// </summary>
        private async Task CaptureElementAsync(IPage page, string xpath, int no)
        {
            try
            {
                // 透過原生 document.evaluate 定位元素，絕對 XPath 必須用這種方式
                var jsHandle = await page.EvaluateExpressionHandleAsync(
                    $"document.evaluate('{EscapeJs(xpath)}', document, null, " +
                    "XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue");

                var element = jsHandle as IElementHandle;
                if (element is null)
                {
                    AddErrorCard(no, $"找不到節點：\n{xpath}");
                    return;
                }

                await element.ScrollIntoViewAsync();

                // 等待元素內所有 <img> 載入完成（load / error 皆視為完成）
                await element.EvaluateFunctionAsync(@"el => {
                    const imgs = [...el.querySelectorAll('img')];
                    return Promise.all(imgs.map(img =>
                        img.complete
                            ? Promise.resolve()
                            : new Promise(resolve => { img.onload = img.onerror = resolve; })
                    ));
                }");

                // ScreenshotDataAsync 直接回傳 PNG byte[]，不需要存檔
                byte[] bytes = await element.ScreenshotDataAsync();
                string? chartText = await ExtractChartTextAsync(element);
                using var ms = new MemoryStream(bytes);
                AddImageCard(no, xpath, new Bitmap(Image.FromStream(ms)), chartText);
            }
            catch (Exception ex)
            {
                AddErrorCard(no, ex.Message);
            }
        }

        private static string EscapeJs(string s) => s.Replace("'", "\\'");

        /// <summary>
        /// 嘗試從 Highcharts 圖表元素擷取所有資料點文字；
        /// 若非圖表元素則回傳 null。
        /// </summary>
        private static async Task<string?> ExtractChartTextAsync(IElementHandle element)
        {
            var result = await element.EvaluateFunctionAsync<string?>(@"el => {
                // ── 優先用 Highcharts API 取完整資料 ──
                if (typeof Highcharts !== 'undefined' && Highcharts.charts) {
                    const chart = Highcharts.charts.find(
                        c => c && c.container &&
                             (c.container === el || c.container.contains(el) || el.contains(c.container))
                    );
                    if (chart) {
                        const lines = [];
                        chart.series
                            .filter(s => s.visible && s.data.length > 0)
                            .forEach(s => {
                                s.data.forEach(p => {
                                    const cats = chart.xAxis[0] && chart.xAxis[0].categories;
                                    const name = p.name || (cats && cats[p.index]) || p.index;
                                    lines.push(`${name}: ${p.y}`);
                                });
                            });
                        if (lines.length > 0) return lines.join('\n');
                    }
                }
                // ── Fallback：收集 SVG tspan 去重文字 ──
                const seen = new Set();
                const texts = [...el.querySelectorAll('tspan')]
                    .map(t => t.textContent.trim())
                    .filter(t => t && !seen.has(t) && seen.add(t));
                return texts.length > 0 ? texts.join('  ·  ') : null;
            }");
            return string.IsNullOrWhiteSpace(result) ? null : result;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using var dlg = new FolderBrowserDialog
            {
                Description = "選擇圖片儲存資料夾",
                UseDescriptionForTitle = true,
            };

            if (dlg.ShowDialog() != DialogResult.OK) return;

            int saved = 0;
            foreach (Control ctrl in flowPanel.Controls)
            {
                var pb = ctrl.Controls.OfType<PictureBox>().FirstOrDefault();
                if (pb?.Image is null) continue;

                string path = Path.Combine(dlg.SelectedPath, $"{Guid.NewGuid():N}.png");
                pb.Image.Save(path, System.Drawing.Imaging.ImageFormat.Png);
            }

            if (saved > 0)
                MessageBox.Show($"✅ 已儲存 {saved} 張圖片至：\n{dlg.SelectedPath}",
                    "儲存完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ── UI helpers

        private void SetStatus(string text) => lblStatus.Text = text;

        private void ClearImagePanel()
        {
            foreach (Control c in flowPanel.Controls)
                c.Dispose();
            flowPanel.Controls.Clear();
        }

        private void AddImageCard(int no, string label, Image image, string? chartText = null)
        {
            const int cardW = 400;
            const int topH  = 32;
            const int imgH  = 312;
            const int txtH  = 100;

            bool hasText = !string.IsNullOrWhiteSpace(chartText);

            var card = new Panel
            {
                Width = cardW,
                Height = topH + imgH + (hasText ? txtH : 0),
                Margin = new Padding(8),
                BackColor = Color.FromArgb(49, 49, 49),
            };

            var topBar = new Panel
            {
                BackColor = Color.FromArgb(37, 37, 37),
                Location = new Point(0, 0),
                Size = new Size(cardW, topH),
            };

            var badge = new Label
            {
                Text = $"#{no}",
                Location = new Point(0, 0),
                Size = new Size(38, topH),
                BackColor = Color.FromArgb(0, 120, 212),
                ForeColor = Color.White,
                Font = new Font("Microsoft JhengHei UI", 9F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
            };

            var xpathLbl = new Label
            {
                Text = label,
                Location = new Point(42, 0),
                Size = new Size(cardW - 42, topH),
                ForeColor = Color.FromArgb(140, 140, 155),
                Font = new Font("Microsoft JhengHei UI", 7.5F),
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true,
                Padding = new Padding(2, 0, 4, 0),
            };

            topBar.Controls.Add(badge);
            topBar.Controls.Add(xpathLbl);

            var pb = new PictureBox
            {
                Image = image,
                SizeMode = PictureBoxSizeMode.Zoom,
                Location = new Point(0, topH),
                Size = new Size(cardW, imgH),
                BackColor = Color.FromArgb(44, 44, 44),
            };

            card.Controls.Add(topBar);
            card.Controls.Add(pb);

            if (hasText)
            {
                var txtBox = new RichTextBox
                {
                    Text      = chartText,
                    ReadOnly  = true,
                    Location  = new Point(0, topH + imgH),
                    Size      = new Size(cardW, txtH),
                    BackColor = Color.FromArgb(30, 30, 36),
                    ForeColor = Color.FromArgb(200, 200, 215),
                    Font      = new Font("Microsoft JhengHei UI", 8.5F),
                    ScrollBars     = RichTextBoxScrollBars.Vertical,
                    BorderStyle    = BorderStyle.None,
                    Padding        = new Padding(6),
                    WordWrap       = true,
                };
                card.Controls.Add(txtBox);
            }

            flowPanel.Controls.Add(card);
        }

        private void AddErrorCard(int no, string message)
        {
            const int cardW = 400;
            const int topH  = 32;
            const int imgH  = 312;

            var card = new Panel
            {
                Width = cardW,
                Height = topH + imgH,
                Margin = new Padding(8),
                BackColor = Color.FromArgb(58, 22, 22),
            };

            var topBar = new Panel
            {
                BackColor = Color.FromArgb(74, 18, 18),
                Location = new Point(0, 0),
                Size = new Size(cardW, topH),
            };

            var badge = new Label
            {
                Text = $"#{no}",
                Location = new Point(0, 0),
                Size = new Size(38, topH),
                BackColor = Color.FromArgb(196, 43, 28),
                ForeColor = Color.White,
                Font = new Font("Microsoft JhengHei UI", 9F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
            };

            var titleLbl = new Label
            {
                Text = "擷取失敗",
                Location = new Point(42, 0),
                Size = new Size(cardW - 42, topH),
                ForeColor = Color.FromArgb(255, 120, 100),
                Font = new Font("Microsoft JhengHei UI", 8.5F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(2, 0, 4, 0),
            };

            topBar.Controls.Add(badge);
            topBar.Controls.Add(titleLbl);

            var msgLbl = new Label
            {
                Text = message,
                Location = new Point(0, topH),
                Size = new Size(cardW, imgH),
                ForeColor = Color.FromArgb(220, 100, 80),
                Font = new Font("Microsoft JhengHei UI", 9F),
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(16),
            };

            card.Controls.Add(topBar);
            card.Controls.Add(msgLbl);
            flowPanel.Controls.Add(card);
        }
    }
}

