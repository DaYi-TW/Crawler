# DailyView 圖片擷取工具

自動爬取 [DailyView 網路溫度計](https://dailyview.tw) 的口碑聲量圖表與排行榜，將指定頁面元素截圖並擷取圖表數據，一鍵保存為 PNG。

---
<img width="1792" height="1125" alt="圖片1" src="https://github.com/user-attachments/assets/37bf18f9-aa9f-4c89-9ef0-3134659af328" />


---

## ✨ 功能

| 功能 | 說明 |
|------|------|
| **元素截圖** | 依 XPath 定位頁面區塊，以 2× 高解析度截圖 |
| **圖表數據擷取** | 自動讀取 Highcharts 資料點（名稱 + 數值），顯示於圖片下方 |
| **單頁 / 全部抓取** | 可選擇單一目標頁面，或一次抓取所有目標 |
| **一鍵保存** | 批次將所有截圖存為 PNG 至指定資料夾 |
| **等待圖片載入** | 截圖前確認元素內所有 `<img>` 載入完成，避免截到空白 |

---

## 🖥️ 執行環境

- **作業系統**：Windows 10 / 11（x64）
- **相依套件**：無需安裝 .NET，單一 EXE 已自包含執行環境
- **網路**：需要網路連線（首次執行會自動下載 Chromium，約 200 MB）

---

## 🚀 快速開始

1. 前往 [Releases](../../releases) 下載最新版 `Crawler.exe`
2. 直接執行，首次使用會自動下載 Chromium 核心
3. 選擇目標頁面，點擊按鈕開始爬取

---

## 🎮 操作說明

```
┌─────────────────────────────────────────────┐
│  目標頁面：[下拉選單]   狀態列訊息...         │
├──────────────┬──────────────┬───────────────┤
│  ▶ 開始爬取  │  ⏩ 全部抓取  │  💾 一鍵保存  │
└──────────────┴──────────────┴───────────────┘
```

| 按鈕 | 功能 |
|------|------|
| `▶ 開始爬取` | 爬取下拉選單中選定的單一頁面 |
| `⏩ 全部抓取` | 依序爬取所有目標頁面的全部元素 |
| `💾 一鍵保存` | 將目前所有截圖批次存檔為 PNG |

---

## 🎯 爬取目標

| 名稱 | 網址 | 元素數 |
|------|------|--------|
| 口碑聲量排行 · 成真咖啡 | [dailyview.tw/top100/item/10915](https://dailyview.tw/top100/item/10915?theme=68&range=30) | 5 |
| 咖啡連鎖 TOP 排行 | [dailyview.tw/top100/topic/68](https://dailyview.tw/top100/topic/68) | 2 |

---

## 🛠️ 開發環境

| 項目 | 版本 |
|------|------|
| .NET | 10.0 |
| UI 框架 | Windows Forms |
| 爬蟲引擎 | [PuppeteerSharp](https://github.com/hardkoded/puppeteer-sharp) 21.0.2 |
| 語言 | C# 14 |

---

## 🔨 自行建置

```powershell
# 一般執行檔（需安裝 .NET 10）
dotnet build -c Release

# 單一 EXE（自包含，免安裝 .NET）
dotnet publish -p:PublishProfile=SingleFile
# 輸出位置：Crawler\bin\Release\publish\Crawler.exe
```

---

## 📄 授權

MIT License
