import { useState } from 'react';
import { api, ImportSummary, StockMode } from '../api';

function readAsText(file: File): Promise<string> {
  return new Promise((resolve, reject) => {
    const r = new FileReader();
    r.onload = () => resolve(String(r.result));
    r.onerror = () => reject(r.error);
    r.readAsText(file);
  });
}

function readAsBase64(file: File): Promise<string> {
  return new Promise((resolve, reject) => {
    const r = new FileReader();
    r.onload = () => resolve(String(r.result)); // data:...;base64,xxxx
    r.onerror = () => reject(r.error);
    r.readAsDataURL(file);
  });
}

function Summary({ s }: { s: ImportSummary }) {
  return (
    <div className="alert ok">
      <strong>Done.</strong> {s.rows} rows · {s.created} created · {s.updated}{' '}
      updated · {s.barcodesAdded} barcodes · {s.pricesUpdated} prices ·{' '}
      {s.stockPosted} stock entries
      {s.errors.length > 0 && (
        <details>
          <summary>{s.errors.length} warnings</summary>
          <ul>
            {s.errors.slice(0, 50).map((e, i) => (
              <li key={i}>{e}</li>
            ))}
          </ul>
        </details>
      )}
    </div>
  );
}

export default function Import() {
  const [stockMode, setStockMode] = useState<StockMode>('set');
  const [busy, setBusy] = useState<string | null>(null);
  const [result, setResult] = useState<Record<string, ImportSummary>>({});
  const [error, setError] = useState<string | null>(null);

  async function run(
    key: string,
    file: File | null | undefined,
    fn: () => Promise<ImportSummary>,
  ) {
    if (!file) return;
    setBusy(key);
    setError(null);
    try {
      const summary = await fn();
      setResult((r) => ({ ...r, [key]: summary }));
    } catch (e: any) {
      setError(e.message);
    } finally {
      setBusy(null);
    }
  }

  return (
    <div className="page">
      <header className="page-head">
        <h1>Import</h1>
      </header>

      <div className="card stock-mode">
        <strong>Stock quantity handling</strong>
        <label>
          <input
            type="radio"
            checked={stockMode === 'set'}
            onChange={() => setStockMode('set')}
          />
          Set on-hand to the quantity in the file (idempotent)
        </label>
        <label>
          <input
            type="radio"
            checked={stockMode === 'add'}
            onChange={() => setStockMode('add')}
          />
          Add the quantity to current stock (goods received)
        </label>
        <label>
          <input
            type="radio"
            checked={stockMode === 'ignore'}
            onChange={() => setStockMode('ignore')}
          />
          Ignore quantity columns
        </label>
      </div>

      {error && <div className="alert error">{error}</div>}

      <div className="import-grid">
        <section className="card import-card">
          <h2>Barcodes / products (XML)</h2>
          <p className="muted">
            Imports products and barcodes from an XML file. Creates missing
            products and adds new barcodes. Updates prices when present.
          </p>
          <input
            type="file"
            accept=".xml,text/xml"
            disabled={busy !== null}
            onChange={async (e) => {
              const f = e.target.files?.[0];
              if (f)
                await run('xml', f, async () =>
                  api.import.xml(await readAsText(f), stockMode),
                );
              e.target.value = '';
            }}
          />
          {busy === 'xml' && <div className="muted">Importing…</div>}
          {result.xml && <Summary s={result.xml} />}
          <details className="hint">
            <summary>Expected XML shape</summary>
            <pre>{`<Products>
  <Product code="A001" name="Milk 1L" unit="pcs"
           price="450" cost="380" vat="20" quantity="24">
    <Barcode>4850001234567</Barcode>
    <Barcode>4850001234568</Barcode>
  </Product>
</Products>`}</pre>
          </details>
        </section>

        <section className="card import-card">
          <h2>Products + prices (Excel)</h2>
          <p className="muted">
            Imports an .xlsx with a header row. Recognised columns: code, name,
            barcode, unit, price, cost, vat, quantity. Creates or updates
            products and prices.
          </p>
          <input
            type="file"
            accept=".xlsx,.xls"
            disabled={busy !== null}
            onChange={async (e) => {
              const f = e.target.files?.[0];
              if (f)
                await run('excel', f, async () =>
                  api.import.excel(await readAsBase64(f), stockMode),
                );
              e.target.value = '';
            }}
          />
          {busy === 'excel' && <div className="muted">Importing…</div>}
          {result.excel && <Summary s={result.excel} />}
        </section>

        <section className="card import-card">
          <h2>Prices only (Excel)</h2>
          <p className="muted">
            Updates sale price (and cost) for existing products, matched by code
            or barcode. Does not create products. Columns: code or barcode, plus
            price and optionally cost.
          </p>
          <input
            type="file"
            accept=".xlsx,.xls"
            disabled={busy !== null}
            onChange={async (e) => {
              const f = e.target.files?.[0];
              if (f)
                await run('prices', f, async () =>
                  api.import.prices(await readAsBase64(f)),
                );
              e.target.value = '';
            }}
          />
          {busy === 'prices' && <div className="muted">Importing…</div>}
          {result.prices && <Summary s={result.prices} />}
        </section>
      </div>
    </div>
  );
}
