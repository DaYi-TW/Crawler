namespace Crawler
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlHeader    = new Panel();
            pnlAccent    = new Panel();
            pnlButtons   = new Panel();
            pnlToolbar   = new Panel();
            lblTitle     = new Label();
            lblSubtitle  = new Label();
            lblTargetLbl = new Label();
            cboTarget    = new ComboBox();
            lblStatus    = new Label();
            btnCrawl     = new Button();
            btnCrawlAll  = new Button();
            btnSave      = new Button();
            pbProgress   = new ProgressBar();
            flowPanel    = new FlowLayoutPanel();

            pnlHeader.SuspendLayout();
            pnlButtons.SuspendLayout();
            pnlToolbar.SuspendLayout();
            SuspendLayout();

            // ── pnlAccent：左側藍色裝飾條 ──────────────────────────────────────
            pnlAccent.BackColor = Color.FromArgb(0, 120, 212);
            pnlAccent.Dock      = DockStyle.Left;
            pnlAccent.Width     = 4;

            // ── btnCrawl ────────────────────────────────────────────────────────
            btnCrawl.BackColor           = Color.FromArgb(0, 120, 212);
            btnCrawl.FlatStyle           = FlatStyle.Flat;
            btnCrawl.FlatAppearance.BorderSize = 0;
            btnCrawl.ForeColor           = Color.White;
            btnCrawl.Font                = new Font("Microsoft JhengHei UI", 10F, FontStyle.Bold);
            btnCrawl.Location            = new System.Drawing.Point(8, 24);
            btnCrawl.Size                = new System.Drawing.Size(128, 40);
            btnCrawl.Text                = "▶  開始爬取";
            btnCrawl.UseVisualStyleBackColor = false;
            btnCrawl.Cursor              = Cursors.Hand;
            btnCrawl.Click              += btnCrawl_Click;

            // ── btnCrawlAll ──────────────────────────────────────────────────────
            btnCrawlAll.BackColor           = Color.FromArgb(95, 60, 180);
            btnCrawlAll.FlatStyle           = FlatStyle.Flat;
            btnCrawlAll.FlatAppearance.BorderSize = 0;
            btnCrawlAll.ForeColor           = Color.White;
            btnCrawlAll.Font                = new Font("Microsoft JhengHei UI", 10F, FontStyle.Bold);
            btnCrawlAll.Location            = new System.Drawing.Point(148, 24);
            btnCrawlAll.Size                = new System.Drawing.Size(128, 40);
            btnCrawlAll.Text                = "⏩  全部抓取";
            btnCrawlAll.UseVisualStyleBackColor = false;
            btnCrawlAll.Cursor              = Cursors.Hand;
            btnCrawlAll.Click              += btnCrawlAll_Click;

            // ── btnSave ─────────────────────────────────────────────────────────
            btnSave.BackColor            = Color.FromArgb(16, 124, 16);
            btnSave.FlatStyle            = FlatStyle.Flat;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.ForeColor            = Color.White;
            btnSave.Font                 = new Font("Microsoft JhengHei UI", 10F, FontStyle.Bold);
            btnSave.Location             = new System.Drawing.Point(288, 24);
            btnSave.Size                 = new System.Drawing.Size(128, 40);
            btnSave.Text                 = "💾  一鍵保存";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Cursor               = Cursors.Hand;
            btnSave.Enabled              = false;
            btnSave.Click               += btnSave_Click;

            // ── pnlButtons：按鈕容器，靠右停靠 ─────────────────────────────────
            pnlButtons.BackColor = Color.FromArgb(30, 30, 30);
            pnlButtons.Controls.Add(btnCrawl);
            pnlButtons.Controls.Add(btnCrawlAll);
            pnlButtons.Controls.Add(btnSave);
            pnlButtons.Dock     = DockStyle.Right;
            pnlButtons.Width    = 428;

            // ── lblTitle ────────────────────────────────────────────────────────
            lblTitle.AutoSize  = true;
            lblTitle.ForeColor = Color.White;
            lblTitle.Font      = new Font("Microsoft JhengHei UI", 13F, FontStyle.Bold);
            lblTitle.Location  = new System.Drawing.Point(18, 12);
            lblTitle.Text      = "DailyView 圖片擷取工具";

            // ── lblSubtitle ─────────────────────────────────────────────────────
            lblSubtitle.AutoSize  = true;
            lblSubtitle.ForeColor = Color.FromArgb(115, 115, 140);
            lblSubtitle.Font      = new Font("Microsoft JhengHei UI", 8.5F);
            lblSubtitle.Location  = new System.Drawing.Point(20, 44);
            lblSubtitle.Text      = "dailyview.tw  ·  口碑聲量排行  ·  元素截圖模式";

            // ── lblTargetLbl ────────────────────────────────────────────────────
            lblTargetLbl.AutoSize  = true;
            lblTargetLbl.ForeColor = Color.FromArgb(120, 120, 145);
            lblTargetLbl.Font      = new Font("Microsoft JhengHei UI", 9F);
            lblTargetLbl.Location  = new System.Drawing.Point(16, 11);
            lblTargetLbl.Text      = "目標頁面：";

            // ── cboTarget ───────────────────────────────────────────────────────
            cboTarget.BackColor     = Color.FromArgb(45, 45, 52);
            cboTarget.ForeColor     = Color.White;
            cboTarget.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTarget.Font          = new Font("Microsoft JhengHei UI", 9F);
            cboTarget.FlatStyle     = FlatStyle.Flat;
            cboTarget.Location      = new System.Drawing.Point(78, 7);
            cboTarget.Size          = new System.Drawing.Size(380, 26);

            // ── lblStatus ───────────────────────────────────────────────────────
            lblStatus.AutoSize     = false;
            lblStatus.ForeColor    = Color.FromArgb(140, 140, 160);
            lblStatus.Font         = new Font("Microsoft JhengHei UI", 9F);
            lblStatus.Location     = new System.Drawing.Point(470, 11);
            lblStatus.Size         = new System.Drawing.Size(620, 20);
            lblStatus.Text         = "按下「開始爬取」以擷取圖片";
            lblStatus.TextAlign    = ContentAlignment.MiddleLeft;

            // ── pbProgress：標題列底部跑馬燈（爬取中才顯示）───────────────────
            pbProgress.Dock                   = DockStyle.Bottom;
            pbProgress.Height                 = 3;
            pbProgress.Style                  = ProgressBarStyle.Marquee;
            pbProgress.MarqueeAnimationSpeed  = 30;
            pbProgress.Visible                = false;

            // ── pnlHeader ───────────────────────────────────────────────────────
            // 注意加入順序：Dock 控制項先加，確保佈局正確
            pnlHeader.BackColor = Color.FromArgb(30, 30, 30);
            pnlHeader.Controls.Add(pnlButtons);   // Dock=Right
            pnlHeader.Controls.Add(pnlAccent);    // Dock=Left
            pnlHeader.Controls.Add(pbProgress);   // Dock=Bottom
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);
            pnlHeader.Dock   = DockStyle.Top;
            pnlHeader.Height = 82;

            // ── pnlToolbar ──────────────────────────────────────────────────────
            pnlToolbar.BackColor = Color.FromArgb(24, 24, 32);
            pnlToolbar.Controls.Add(lblTargetLbl);
            pnlToolbar.Controls.Add(cboTarget);
            pnlToolbar.Controls.Add(lblStatus);
            pnlToolbar.Dock   = DockStyle.Top;
            pnlToolbar.Height = 40;

            // ── flowPanel ───────────────────────────────────────────────────────
            flowPanel.AutoScroll  = true;
            flowPanel.BackColor   = Color.FromArgb(28, 28, 28);
            flowPanel.Dock        = DockStyle.Fill;
            flowPanel.Padding     = new Padding(10);
            flowPanel.WrapContents = true;

            // ── Form1 ───────────────────────────────────────────────────────────
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode       = AutoScaleMode.Font;
            BackColor           = Color.FromArgb(28, 28, 28);
            ClientSize          = new System.Drawing.Size(1200, 720);
            Controls.Add(flowPanel);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlHeader);
            Font        = new Font("Microsoft JhengHei UI", 9F);
            MinimumSize = new System.Drawing.Size(900, 500);
            Name        = "Form1";
            Text        = "DailyView 圖片擷取工具";

            pnlToolbar.ResumeLayout(false);
            pnlButtons.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel           pnlHeader;
        private Panel           pnlAccent;
        private Panel           pnlButtons;
        private Panel           pnlToolbar;
        private Label           lblTitle;
        private Label           lblSubtitle;
        private Label           lblTargetLbl;
        private ComboBox        cboTarget;
        private Label           lblStatus;
        private Button          btnCrawl;
        private Button          btnCrawlAll;
        private Button          btnSave;
        private ProgressBar     pbProgress;
        private FlowLayoutPanel flowPanel;
    }
}

