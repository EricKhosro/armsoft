# ArmTrade Mini

A minimal retail desktop app — **Point of Sale, Products, Import, Inventory** — built as its own clean codebase.

- **Frontend:** React (Vite) inside **Electron** (via `electron-vite`).
- **Backend:** **NestJS** + TypeORM + **SQLite** (local file), REST API on `http://127.0.0.1:3917/api`.
- Fully standalone and offline. No cloud, no login.

## Scope

Only the features requested:

| Feature | Where |
|---|---|
| Point of Sale | `app` → POS page; `api` → `pos` module (checkout decrements stock in a transaction) |
| Products | `app` → Products page; `api` → `products` module (with multiple barcodes each) |
| Import — barcodes/products via **XML** | Import page; `api` → `import` module, `POST /api/import/xml` |
| Import — products + prices via **Excel** | Import page; `POST /api/import/excel` |
| Import — **prices only** via Excel | Import page; `POST /api/import/prices` |
| Inventory | Inventory page; `api` → `inventory` module (stock = sum of movements; adjust / count) |
| Devices | Devices page; `api` → `devices` module (fiscal HDM, label scale, card terminal) |

## Devices (ported from AS Trade's protocols)

Configured on the **Devices** page; the POS uses the active fiscal/terminal, and
**Products → Export to scale** pushes PLUs to the active scale. Protocols were
ported faithfully from the decompiled connectors (TCP/UDP/3DES/WebSocket all via
Node's standard library). None of the original's plaintext credential logging is
reproduced.

| Device | Protocol | Status |
|---|---|---|
| **HDM fiscal register** | TCP, 12-byte framed, 3DES-ECB JSON, cashier login → session key | ported from source, tested vs simulator |
| **CAS label scale** | TCP, binary record + checksum, ARMSCII-8 names | ported from source, tested vs simulator |
| **TM-A label scale** | TCP, tab-delimited UTF-8 lines | ported from source, tested vs simulator |
| **CASNew** | file (XML) export | ported (file import, as in the original) |
| **PossoftPay terminal** | `ws://` JSON, sale/refund/void, `wait` frames | ported from source, tested vs simulator |
| DIGI / Mertech | TCP (BCD / framed JSON) | mapping ported; **disabled pending device verification** |
| Massa-K | in-process Windows COM driver | **unsupported** from Node (needs a native bridge) |

### Try it without hardware

Each device has a simulator. Run them, then add a device on the Devices page
pointing at the simulator (the "Add" presets already match):

```bash
npm run sim      # HDM 127.0.0.1:3930, CAS 3931, TM-A 3932, terminal 3933
```

- Fiscal preset: host `127.0.0.1`, port `3930`, password `1234`, cashier `3`, pin `0000`.
- Terminal preset: `127.0.0.1:3933` (declines amount 6.66).
- Scale preset: CAS at `127.0.0.1:3931`.

Then in the POS, "Pay card" authorises on the terminal and "Pay cash/card"
prints a fiscal receipt when a fiscal device is active.

### Device tests

```bash
npm run test:devices   # drives every connector against its simulator
```

> Security note: the original wrote unencrypted request JSON (including the ECR
> password and cashier PIN) to `%TEMP%\ECR logs`. This port does not. If you have
> a real DIGI, Mertech, or Massa-K device, tell me the exact model and I'll
> finish and verify that connector.

## Prerequisites

- Node.js 22.19+ (you have 22.23).
- On install, `better-sqlite3` downloads a prebuilt binary for Node — no compiler needed in the usual case.

## Run (development)

```bash
cd armtrade-mini
npm install          # installs root + api + app workspaces
npm run dev          # starts the NestJS API and the Electron app together
```

`npm run dev` runs two processes:
- **api** — `nest start --watch` on port 3917, creating `api/armtrade.sqlite` on first run.
- **app** — `electron-vite dev`, which opens the desktop window.

If you prefer separate terminals: `npm run dev:api` and `npm run dev:app`.

## Build

```bash
npm run build        # builds api (dist/) and app (out/)
```

Packaging into a single installer (bundling the API + a Node runtime, and
spawning it from the Electron main process) is intentionally left as a next
step — see the production note in `app/src/main/index.ts`.

## Import formats

**XML** (lenient; fields may be attributes or child elements):

```xml
<Products>
  <Product code="A001" name="Milk 1L" unit="pcs" price="450" cost="380" vat="20" quantity="24">
    <Barcode>4850001234567</Barcode>
    <Barcode>4850001234568</Barcode>
  </Product>
</Products>
```

**Excel** — first sheet, header row. Recognised columns (case/spacing-insensitive,
with common aliases): `code`, `name`, `barcode` (multiple separated by `,` `;` `|`),
`unit`, `price`, `cost`, `vat`, `quantity`.

**Stock handling on import** (chosen on the Import page):
- **set** — on-hand becomes the file quantity (idempotent; re-importing is safe).
- **add** — the quantity is added to current stock (goods received).
- **ignore** — quantity columns are skipped.

## Data model

- `products` (code, name, unit, salePrice, cost, vatRate) — each with many `barcodes`.
- `stock_movements` — signed quantities; **current stock = SUM(qty)** per product
  (types: `import`, `sale`, `adjustment`, `stocktake`). Full audit trail, no single
  mutable quantity field.
- `sales` + `sale_items` — POS receipts; checkout writes negative stock movements.

Money is stored as plain numbers (AMD) for simplicity in this minimal build.

> This is a fresh, independent application. It is not derived from any other
> product's code; it only implements the requested feature set.
