# 排程範例專案 (Scheduler Sample)

本專案示範如何建立與部署 UOF X 排程。

## 專案結構
```
SchedulerSample/
├── appsettings.json # 自訂設定檔
├── SyncService.cs # 排程服務邏輯（範例）
├── TaskEngine.config # 排程設定
├── TaskEngine.exe # 排程執行檔
└── SchedulerSample.csproj # 專案檔
```

**主要檔案說明：**
- `appsettings.json`：自訂設定檔，可自訂參數。
- `SyncService.cs`：排程邏輯實作範例，包含 `Run()` 方法，定期讀取 appsettings.json 設定檔，並將設定內容輸出至主控台。
- `TaskEngine.config`：排程設定，定義 DLL 檔案、命名空間（Namespace）及執行頻率（Cron 表達式）。
- `TaskEngine.exe`：排程執行檔，依據排程設定執行排程任務。

## 使用說明（以 Visual Studio 為例）

### 1. 選擇整合方式

#### 方法一：加入專案參考

當您的主要專案需整合排程功能時，建議使用此方法。
<br>
則建置主要專案時，排程專案的 DLL 及其相依檔案將自動複製到主要專案的輸出目錄，無需手動放置檔案。

- 於主要專案的方案中，加入 `SchedulerSample` 專案：
    1. 右鍵方案 → 加入 → 現有專案
    2. 選擇 `SchedulerSample.csproj` 加入

- 新增專案參考：
    1. 右鍵主要專案 → 加入 → 專案參考
    2. 勾選 `SchedulerSample` 專案 → 確定 

#### 方法二：獨立部署

直接於排程專案中開發並部署，不依賴其他專案時，建議使用此方法。

### 2. 設定 TaskEngine.config

編輯 `TaskEngine.config` 檔案，設定排程任務的 DLL 名稱、Namespace 以及執行任務的時間頻率：
- DllName：執行的 DLL 檔案名稱
- NamespaceName：執行的 Namespace 名稱
- CronExpression：執行任務的時間頻率，Cron 表達式可參考[官方文件](https://github.com/HangfireIO/Cronos#cron-format)說明

### 3. 設定檔永遠複製

如有新增自訂設定檔，須將其設定為永遠複製：
- 右鍵設定檔 → 屬性 → 複製到輸出目錄 → 永遠複製

### 4. 設定專案檔

如專案有相依套件，請確保這些套件被複製到輸出目錄：
1. 右鍵專案 → 編輯專案檔
2. 於 `<PropertyGroup>` 中加入以下設定：
```xml
<!-- 強制複製所有相依套件到輸出目錄 -->
<CopyLocalLockFileAssemblies>true</CopyLocalLockFileAssemblies>
```

### 5. 執行與測試

1. 建置專案
2. 至建置後資料夾（`bin`），確保相關 DLL 檔案與執行檔位於同一目錄
3. 執行 `TaskEngine.exe`，確認排程任務是否依設定執行

### 6. 註冊為 Windows 服務

1. 確認可執行後，將專案發佈至目標環境
2. 將 `TaskEngine.exe` 註冊為 Windows 服務在背景執行，詳細步驟請參考下方[服務管理指令](#服務管理指令)

## 服務管理指令

以下指令請以**系統管理員身分**執行命令提示字元 (CMD) 或 PowerShell。

### 新增服務
建立名為 "UOFX TaskEngine" 的服務，並設定為自動啟動。
```bash
sc create "UOFX TaskEngine" binpath= "D:\SchedulerSample\TaskEngine.exe" type= own start= auto
```

### 啟動服務
```bash
sc start "UOFX TaskEngine"
```

### 停止與刪除服務
若要移除服務，請先停止服務後再刪除。
```bash
sc stop "UOFX TaskEngine"
sc delete "UOFX TaskEngine"
```

### 變更執行檔路徑
若移動了資料夾，可使用此指令更新服務的執行檔路徑。
```bash
sc config "UOFX TaskEngine" binPath= "D:\SchedulerSample\TaskEngine.exe"
```

### 查看服務狀態
1. 在 Windows 搜尋列輸入 `services.msc` 並開啟。
2. 在服務清單中尋找 "UOFX TaskEngine" 即可查看目前狀態。

