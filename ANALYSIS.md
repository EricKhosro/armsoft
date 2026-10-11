# AS Trade 9.2.37: reverse-engineering report

- **Target:** `C:\Program Files\Armenian Software\AS Trade`
- **Method:** REA (`reverse-engineer-anything` skill, rea-agents 6.3.0) for managed-artifact and native-boundary evidence, then ILSpy 11.1 (`ilspycmd`) to reconstruct C#.
- **Date:** 2026-10-10.
- **Evidence:** REA evidence JSON is in `decompiled/_rea-evidence/`.
- **Source:** decompiled source is in `decompiled/`. Open `decompiled/AS Trade (decompiled).sln`.

Labels used below:
- **OBSERVED**: read directly in the decompiled code.
- **INFERRED**: a conclusion drawn from the code, not seen directly.

All findings are static. Nothing was executed.

---

## 1. What the application is

| | |
|---|---|
| Type | .NET 10 WPF desktop client, DevExpress 25.2 UI, MVVM (CommunityToolkit + DevExpress.Mvvm) |
| Code form | 100% IL. No ReadyToRun, NativeAOT or obfuscation (REA `inspect_managed_artifact`: `implementation: cil`, `ready_to_run_signature: false`, no obfuscator attributes). |
| Decompiled output | 25 projects, 4,652 `.cs` files (~631k lines), 108 `.xaml` views recovered from BAML. Zero decompiler failures. |
| Entry point | `AS Trade/ArmSoft.SME.Trade.Host/App.xaml.cs`, which calls `TradeSettings.Init`, then shows the splash screen and `MainWindow` |
| Shared platform | "ArmSoft SME". The same Core.UI, Client and Common code also powers **AS Accountant** and **AS Sales Management** (`SystemTypes`) |

Key REA evidence IDs (managed inspection):

| Assembly | Evidence ID |
|---|---|
| AS Trade.dll | `ev_60c83d9f…dfa72c` |
| ArmSoft.SME.Core.UI | `ev_224f601c…a82e80d` |
| ArmSoft.SME.Trade.UI | `ev_20dfe6d3…b269a610d` |
| ArmSoft.SME.Client | `ev_bcf82504…aaedc3f68` |
| ArmSoft.SME.Common | `ev_09a0da44…0119f0096` |

### Assembly layers

```
AS Trade.exe / AS Trade.dll        host: App, MainWindow, splash
 └─ ArmSoft.SME.Trade.UI           Trade screens, POS, device helpers, offline SQLite
     └─ ArmSoft.SME.Core.UI        shared shell: login, DI, logging, updates, scripting (Roslyn), reports
         ├─ ArmSoft.SME.Trade.Client / ArmSoft.SME.Client   REST client for the per-database business API
         ├─ ArmSoft.SME.Trade.Common / ArmSoft.SME.Common   domain model: documents, directories, enums
         └─ ArmSoft.Cloud.Client   control plane: cloud.armsoft.am (keys, licences, DB list)
 Integrations: TaxService, ECRConnector, ElScaleConnector, EMarkReceiptClient,
               PossoftPayClient, Arcus2Client, InternetBank/OnlineBanking/OpenBanking, b2bClient.V2
 Excel: ArmSoft.SME.Core.AddIn / ArmSoft.SME.Trade.AddIn (VSTO, .NET Framework) + AsTradeCloudAddIn.XLA
```

Native boundary (REA `inspect_managed_native_boundaries`):
- The only real native dependency is `arccom.dll` (Arcus card terminal SDK). AS Trade does **not** ship it.
- Everything else uses only small Win32 calls: `ProcessIdToSessionId`, `SetCurrentProcessExplicitAppUserModelID`, `EnumDisplayDevices`.
- `AnyDesk.exe` and `AsRemoteSupport.exe` are third-party remote-support tools that ship in the folder.

---

## 2. Architecture: a thin client over ArmSoft's cloud

1. **Sign-in:**
   - **Azure AD B2C**, tenant `armsoftb2c`, through MSAL. It uses an embedded web view or the system browser with a `http://localhost` redirect (`Core.UI/ADHelper.cs`).
   - The token cache is `tradearmsoft.bc.cache` in the MSAL user root.
2. **Control plane:**
   - The B2C token is sent to `https://cloud.armsoft.am/api/7X/CloudLogin/V2`.
   - The reply lists the user's licence keys and databases. Each database has a `Service` URL (`CloudLoginWindow.xaml.cs:429-438`, `:828-833`).
3. **Business API login:**
   - `POST {Service}/api/Login` exchanges the B2C token for an API bearer token (`ApplicationServiceConfig.cs:74-97`).
   - It also sends machine name, process ID, MachineGuid, OS and .NET version, install folder and screen resolution.
4. **All work goes through REST:**
   - Each request carries `Authorization: Bearer`, `X-Version: 1.0` and `Accept-Language` (hy/ka/ru/en) (`SME.Client/ApiClientBase.cs`).
   - About 130 route templates exist, for example `/api/TradeDocument/Store`, `/api/Data/{provider}`, `/api/DPR/*` and `/api/UserAccess/*`.
5. **Server selection:**
   - The cloud chooses it, unless `appsettings.json` contains `BaseURL`.
   - The shipped `appsettings.json` is empty (`{}`).
   - The last database choice is stored in `HKCU\Software\Armenian Software\AS Trade 9`.

**Business logic is on the server (INFERRED, strong evidence):**
- The client never builds accounting entries. It only sends flags such as `RegenerateEntries`.
- Stock levels, costing, discounts, batch distribution, numbering and all reports are server calls (`Trade.Client/CommonServicesRoutes.cs`, `GoodsEngineServicesRoutes.cs`).
- The server code is **not** in this install. What you have is the client.

### Domain model (`SME.Common`, `SME.Trade.Common`)

- **Base type:** everything inherits `PersistableObject` (`ISN` Guid key, `TS` concurrency stamp). It splits into:
  - **Documents:** `DocDate`, `DocNum`, `State` (Draft=1, Processed=2, Deleted=99).
  - **Directories:** reference data with `Code`, `Name` and `IsClosed`.
- **About 40 document types** (`DocumentTypeSE.cs`):
  - cash and bank: CashIn/Out, BankIn/Out;
  - goods in and out: MTIncome, MTInvoice, MTSaleRetail, **MTBill (POS receipt)**;
  - returns, moves, complectation, stock-taking and revaluation;
  - partner balance corrections;
  - tax documents and EMarkReceipt.
- **Directories** (`DirectoryTypeSE.cs`): Goods, Partner, Storage, Cash (register), CashierShift, Price, PriceType, Currency, Bank and more.
- **Document lifecycle:** `Create/{type}` (the server pre-fills the document), `GetNextDocNum`, then `Store` (the server validates and returns warnings or exceptions), then `ChangeDocState`. Delete goes through `ValidateDelete` and `DELETE`.
- **Reports and lists:** `DataProvider` (about 154 providers):
  1. Enqueues a server job.
  2. Polls its progress.
  3. Streams rows in a custom binary format (`DataProviderRowsBinaryReader`).
- **DPR jobs:** long-running server tasks that can ask the user questions mid-run (`/api/DPR/uiRequestResult`).
- **Permissions:**
  - Units are subsystems (Purchases, Sales, Storage, CashBank, Analysis, B2B, POS, EInvoicing, AdminTools), about 280 access IDs, and Add/Edit/View/Delete rights per document or directory type.
  - INFERRED: enforcement is on the server.

### Local data (`%APPDATA%\Armenian Software\Trade\Local Storage\`)

- **`{db}_metadata.db3`** (SQLite): a cache of reference data, refreshed incrementally by timestamp.
- **`{db}_data.db3`** (SQLite): the **offline POS** database.
  - Only receipts and returns (MTBill, MTBillReturn, MTReturnFromCustomer) can be created offline. They are pushed to `/Store` when the app is back online.
  - The same file holds login tokens and the offline PIN hash. There is **no app-level encryption** (`LocalDBService.cs:65-99`).

---

## 3. External integrations: all called directly from the desktop

| Integration | Protocol / endpoint | Auth | Notes |
|---|---|---|---|
| **SRC e-invoicing** (`ArmSoft.TaxService`) | SOAP login `http://ews.taxservice.am:80`, REST `https://e-invoicing.taxservice.am` | TIN + login + password → JWT cookie | **Login is plain HTTP.** |
| **Fiscal register / HDM** (`ECRConnector`) | Raw TCP to register IP:port, JSON encrypted with 3DES-ECB | ECR password + cashier PIN | Every request is logged **in plaintext** to `%TEMP%\ECR logs\` |
| **Label scales** (`ElScaleConnector`) | TCP, plus UDP discovery; Massa-K through its COM driver | Mertech: device password | CAS, DIGI, Mertech, TM-A, Massa-K |
| **Weighing scales** | Serial COM port (Trade.UI `ScaleConnector`) | none | CAS PR-II, Shtrih |
| **e-mark.am virtual receipts** | HTTPS `ecr.e-mark.am/ext_app/api/v1` | DataMatrix activation → Bearer and refresh tokens | |
| **PossoftPay card terminal** | `ws://<terminal>` (unencrypted WebSocket) | none | |
| **Arcus card terminal** | `arccom.dll` P/Invoke | none | DLL not shipped |
| **Internet banking** (8 Armenian banks) | HTTPS REST | Embedded per-bank app credential + user password / 2FA | Ameria, Ararat, Amio, ArmSwiss, Byblos, Converse, Evoca, Demo |
| **ArmSoft Open Banking** | HTTPS `aob.armsoft.am`, OAuth + PKCE in WebView2 | licence key as `client_id` | PKCE verifier is only 6 characters (the standard requires 43–128) |
| **B2B marketplace** | HTTPS `services.armsoft.am:5443`, `cloud.armsoft.am` | `apiToken` from `/api/ApplicationToken` (MD5 hardware fingerprint) | |
| **CBA exchange rates** | SOAP 1.2 `https://api.cba.am/exchangerates.asmx` | none | |
| **Dropbox** | Dropbox API | OAuth; app keys embedded | backups |
| **Excel add-in** | COM-visible `TradeExcelConnector` → `/api/TradeExchange/*`, `/api/FormulaEngine/T*` | same B2C + bearer flow | about 150 worksheet `T*` functions; `Import\*.xlsm` templates |

The ArmSoft server is involved for the tax service and banks only to store mapping data. It does not proxy those calls.

---

## 4. Behaviour you probably want to know about

**Telemetry and phone-home:**
- **Remote logging (OBSERVED):** the shipped config has no `Serilog` section. The client therefore sends Warning-and-above logs and unhandled exceptions to **`http://93.94.223.178:5341` (Seq) over plain HTTP**, using an embedded API key.
  - Each log event carries machine name, process ID and version.
  - After login it also carries **UserId, UserName, ServiceAddress and Database** (`Core.UI/Settings.cs:1256-1275`).
- **Usage statistics (OBSERVED):** every view open, ribbon click and menu action is counted and timed. The data goes to `/api/Statistics/StoreCallCount` and `/StoreCallDuration` 10 minutes after login, then hourly, and at logout.
- **Licensing (OBSERVED):**
  - An online licence is checked out at login (`api/7X/OnlineLicenses/V2`) and renewed every 10 minutes.
  - If renewal fails repeatedly, the app logs out.
  - Demo and contract expiry are also checked on the client. Enforcement is mainly server-side.
- **Auto-update (OBSERVED):** AppLife updater, source `https://update.armsoft.am/tradeUI/`, signed with a pinned RSA public key and run with UAC elevation. The server can force an update with error 20006.

**Remote code execution by design (OBSERVED):**
- **What it is:** C# "extensions" stored on the server are downloaded, compiled with Roslyn and loaded into the client process with no sandbox.
  - Extension types: `UserScript`, `DVD` (data-view definitions), `DVS` (data-view settings) and `CF` (context functions).
  - Code: `Core.UI/MetaData/CSharpCompilerHelper.cs:27-59` uses `AssemblyLoadContext.Default.LoadFromStream`, with every trusted platform assembly as a reference.
  - The scripts have access to all of .NET: files, processes, network and reflection.
- **When it runs:** some scripts run without a click. Building the electronic-scale type list compiles **every** UserScript (`ElScaleAndTerminalHelper.cs:66-73`), and DVS scripts run when a data-view group opens.
- **Implication:** anyone who can write extension records (a tenant admin with access ID 421, a stolen token, ArmSoft, or a server compromise) can run code on every PC that opens AS Trade.

**Secrets and at-rest weaknesses (OBSERVED; values intentionally not reproduced):**
- `SME.Common/Helpers/EncryptHelper.cs` uses **hard-coded keys**: a string key plus a 32-byte AES-GCM key. It encrypts saved e-invoicing credentials and other parameters, so anyone with the DLL can decrypt them.
- **Embedded credentials:**
  - per-bank Internet-bank app credentials (`InternetBank.APIClient/Config.cs`, `IBDemoSettings.cs`);
  - Seq API keys;
  - Dropbox app keys (`Core.UI/Settings.cs:390-396`).
  - B2C client IDs are public identifiers, so they are not secrets.
- The offline SQLite database stores tokens without encryption.
- The SRC login password is sent over HTTP.
- The ECR password and cashier PIN are written to `%TEMP%` logs. No cleanup code was found.
- No TLS certificate-validation overrides were found in the decompiled code (searched for `ServerCertificate*ValidationCallback` and `RemoteCertificateValidationCallback`).

---

## 5. Where to look in the source

| Question | Start here |
|---|---|
| Startup and DI | `ArmSoft.SME.Core.UI/ArmSoft.SME.Core.UI/Settings.cs` (`Init`, ~line 458) |
| Login | `ArmSoft.SME.Core.UI/.../CloudLoginWindow.xaml.cs`, `ADHelper.cs`, `Services/ApplicationServiceConfig.cs` |
| REST client and routes | `ArmSoft.SME.Client/ArmSoft.SME.Client/ApiClientBase.cs`, `*Routes.cs`; `ArmSoft.SME.Trade.Client/.../Routes/` |
| Domain enums | `ArmSoft.SME.Trade.Common/.../SmartEnums/DocumentTypeSE.cs`, `DirectoryTypeSE.cs`, `UserAccessIdSE.cs` |
| Document window and save flow | `ArmSoft.SME.Core.UI/.../Documents/DocumentWindow.cs`, `Services/DocumentService.cs` |
| POS | `ArmSoft.SME.Trade.UI/.../Documents.Goods/SaleWindow.xaml.cs`, `Navigation/NavigationBarsController.cs` |
| Offline mode | `ArmSoft.SME.Trade.UI/.../Services/LocalDBService.cs`, `LocalDBDocumentService.cs` |
| Scripting | `ArmSoft.SME.Core.UI/.../MetaData/CSharpCompilerHelper.cs`, `Services/DefinitionMappingsService.cs` |
| Fiscal register | `ArmSoft.ECRConnector/ArmSoft.ECRConnector/ECR.cs`, `Trade.UI/.../Helpers/ECRHelpers.cs` |
| Tax service | `ArmSoft.TaxService/ArmSoft.TaxService/TaxServiceClient.cs`, `Core.UI/.../HelpClasses/TaxServiceHelper.cs` |

---

## 6. Limits and open questions

- **Rebuilding:** the decompiled solution is for reading. Rebuilding it would need licensed DevExpress 25.2 packages, plus fixes to some ILSpy artefacts.
- **Server not covered:** the server side (business rules, accounting, permission enforcement, script handling on the server) is not in this install. It cannot be confirmed from client code.
- **Not determined:**
  - whether the server restricts or validates UserScript/DVD/CF uploads;
  - whether it re-checks permissions when the client sends `WithoutAccessCheck=true` (offline sync does this);
  - where parameter values such as the encrypted tax password are stored (INFERRED: the server database).
- **Not examined:** macros inside `AsTradeCloudAddIn.XLA` and the `Import\*.xlsm` templates.
- **ECR protocol:** only the client side is visible. The register's side of the protocol is not.
- **REA MCP tools:** these get registered in `~/.claude.json`. They only become available to Claude Code after a restart. This analysis used the REA CLI.
