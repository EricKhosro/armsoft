import { useEffect, useState } from 'react';
import { api, money, Product, ProductInput, ScaleExportResult } from '../api';

const empty: ProductInput = {
  code: '',
  name: '',
  unit: 'pcs',
  salePrice: 0,
  cost: 0,
  vatRate: 0,
  isWeight: false,
  barcodes: [],
};

export default function Products() {
  const [list, setList] = useState<Product[]>([]);
  const [query, setQuery] = useState('');
  const [editing, setEditing] = useState<Product | null>(null);
  const [form, setForm] = useState<ProductInput>(empty);
  const [barcodeText, setBarcodeText] = useState('');
  const [error, setError] = useState<string | null>(null);
  const [open, setOpen] = useState(false);
  const [scaleBusy, setScaleBusy] = useState(false);
  const [scaleResult, setScaleResult] = useState<ScaleExportResult | null>(null);

  async function load() {
    try {
      setList(await api.products.list(query));
    } catch (e: any) {
      setError(e.message);
    }
  }

  useEffect(() => {
    const t = setTimeout(load, 200);
    return () => clearTimeout(t);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [query]);

  function openNew() {
    setEditing(null);
    setForm(empty);
    setBarcodeText('');
    setError(null);
    setOpen(true);
  }

  function openEdit(p: Product) {
    setEditing(p);
    setForm({
      code: p.code,
      name: p.name,
      unit: p.unit,
      salePrice: p.salePrice,
      cost: p.cost,
      vatRate: p.vatRate,
      isWeight: !!p.isWeight,
    });
    setBarcodeText(p.barcodes.map((b) => b.barcode).join(', '));
    setError(null);
    setOpen(true);
  }

  async function save() {
    const payload: ProductInput = {
      ...form,
      salePrice: Number(form.salePrice) || 0,
      cost: Number(form.cost) || 0,
      vatRate: Number(form.vatRate) || 0,
      barcodes: barcodeText
        .split(/[;,|\s]+/)
        .map((s) => s.trim())
        .filter(Boolean),
    };
    if (!payload.code || !payload.name) {
      setError('Code and name are required');
      return;
    }
    try {
      if (editing) await api.products.update(editing.id, payload);
      else await api.products.create(payload);
      setOpen(false);
      await load();
    } catch (e: any) {
      setError(e.message);
    }
  }

  async function remove(p: Product) {
    if (!confirm(`Delete "${p.name}"?`)) return;
    try {
      await api.products.remove(p.id);
      await load();
    } catch (e: any) {
      setError(e.message);
    }
  }

  // Export the currently-listed products to the active label scale. Products
  // need a numeric code (used as the PLU) and a numeric barcode.
  async function exportToScale() {
    setScaleResult(null);
    setError(null);
    const scales = await api.devices.list('scale');
    const scale = scales.find((d) => d.active) || scales[0];
    if (!scale) {
      setError('No scale configured. Add one on the Devices page first.');
      return;
    }
    const products = list
      .map((p) => {
        const plu = parseInt(p.code, 10);
        const bc = p.barcodes.map((b) => parseInt(b.barcode, 10)).find((n) => !isNaN(n));
        if (isNaN(plu) || bc === undefined) return null;
        return {
          plu,
          barcode: bc,
          name: p.name,
          price: p.salePrice,
          isWeight: !!p.isWeight,
        };
      })
      .filter(Boolean);
    if (products.length === 0) {
      setError('No products with a numeric code and barcode to export.');
      return;
    }
    setScaleBusy(true);
    try {
      const res = await api.devices.scaleExport(scale.id, products as any[]);
      setScaleResult(res);
      if (res.fileContent && res.fileName) {
        const blob = new Blob([res.fileContent], { type: 'application/xml' });
        const url = URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = res.fileName;
        a.click();
        URL.revokeObjectURL(url);
      }
    } catch (e: any) {
      setError(e.message);
    } finally {
      setScaleBusy(false);
    }
  }

  return (
    <div className="page">
      <header className="page-head">
        <h1>Products</h1>
        <div className="actions">
          <input
            className="search"
            placeholder="Search name or code…"
            value={query}
            onChange={(e) => setQuery(e.target.value)}
          />
          <button className="btn ghost" disabled={scaleBusy} onClick={exportToScale}>
            {scaleBusy ? 'Exporting…' : 'Export to scale'}
          </button>
          <button className="btn" onClick={openNew}>
            + New product
          </button>
        </div>
      </header>

      {error && !open && <div className="alert error">{error}</div>}
      {scaleResult && (
        <div className={`alert ${scaleResult.failed ? 'error' : 'ok'}`}>
          Scale export ({scaleResult.model}
          {scaleResult.verified ? '' : ', experimental'}): {scaleResult.exported}/
          {scaleResult.total} PLUs sent
          {scaleResult.failed ? `, ${scaleResult.failed} failed` : ''}.
          {scaleResult.note && <> {scaleResult.note}</>}
        </div>
      )}

      <div className="card">
        <table className="grid">
          <thead>
            <tr>
              <th>Code</th>
              <th>Name</th>
              <th>Unit</th>
              <th className="r">Sale price</th>
              <th className="r">Cost</th>
              <th>Barcodes</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {list.map((p) => (
              <tr key={p.id}>
                <td>{p.code}</td>
                <td>{p.name}</td>
                <td>{p.unit}</td>
                <td className="r">{money(p.salePrice)}</td>
                <td className="r">{money(p.cost)}</td>
                <td className="muted">
                  {p.barcodes.map((b) => b.barcode).join(', ')}
                </td>
                <td className="row-actions">
                  <button className="link" onClick={() => openEdit(p)}>
                    Edit
                  </button>
                  <button className="link danger" onClick={() => remove(p)}>
                    Delete
                  </button>
                </td>
              </tr>
            ))}
            {list.length === 0 && (
              <tr>
                <td colSpan={7} className="empty">
                  No products. Create one or use the Import page.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>

      {open && (
        <div className="modal-backdrop" onClick={() => setOpen(false)}>
          <div className="modal" onClick={(e) => e.stopPropagation()}>
            <h2>{editing ? 'Edit product' : 'New product'}</h2>
            <div className="form-grid">
              <label>
                Code
                <input
                  value={form.code}
                  onChange={(e) => setForm({ ...form, code: e.target.value })}
                />
              </label>
              <label>
                Name
                <input
                  value={form.name}
                  onChange={(e) => setForm({ ...form, name: e.target.value })}
                />
              </label>
              <label>
                Unit
                <input
                  value={form.unit}
                  onChange={(e) => setForm({ ...form, unit: e.target.value })}
                />
              </label>
              <label>
                Sale price
                <input
                  type="number"
                  value={form.salePrice}
                  onChange={(e) =>
                    setForm({ ...form, salePrice: Number(e.target.value) })
                  }
                />
              </label>
              <label>
                Cost
                <input
                  type="number"
                  value={form.cost}
                  onChange={(e) =>
                    setForm({ ...form, cost: Number(e.target.value) })
                  }
                />
              </label>
              <label>
                VAT %
                <input
                  type="number"
                  value={form.vatRate}
                  onChange={(e) =>
                    setForm({ ...form, vatRate: Number(e.target.value) })
                  }
                />
              </label>
              <label className="check">
                <input
                  type="checkbox"
                  checked={!!form.isWeight}
                  onChange={(e) => setForm({ ...form, isWeight: e.target.checked })}
                />
                Sold by weight (for scale export)
              </label>
              <label className="wide">
                Barcodes (comma or space separated)
                <input
                  value={barcodeText}
                  onChange={(e) => setBarcodeText(e.target.value)}
                />
              </label>
            </div>
            {error && <div className="alert error">{error}</div>}
            <div className="modal-actions">
              <button className="btn ghost" onClick={() => setOpen(false)}>
                Cancel
              </button>
              <button className="btn" onClick={save}>
                Save
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
