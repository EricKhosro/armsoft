import { useEffect, useState } from 'react';
import { api, money, StockRow } from '../api';

export default function Inventory() {
  const [rows, setRows] = useState<StockRow[]>([]);
  const [query, setQuery] = useState('');
  const [error, setError] = useState<string | null>(null);
  const [edit, setEdit] = useState<StockRow | null>(null);
  const [mode, setMode] = useState<'adjust' | 'stocktake'>('stocktake');
  const [value, setValue] = useState('0');
  const [note, setNote] = useState('');

  async function load() {
    try {
      setRows(await api.inventory.levels(query));
      setError(null);
    } catch (e: any) {
      setError(e.message);
    }
  }

  useEffect(() => {
    const t = setTimeout(load, 200);
    return () => clearTimeout(t);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [query]);

  function open(row: StockRow, m: 'adjust' | 'stocktake') {
    setEdit(row);
    setMode(m);
    setValue(m === 'stocktake' ? String(row.stock) : '0');
    setNote('');
    setError(null);
  }

  async function submit() {
    if (!edit) return;
    const v = Number(value);
    if (!Number.isFinite(v)) {
      setError('Enter a number');
      return;
    }
    try {
      if (mode === 'adjust') await api.inventory.adjust(edit.productId, v, note);
      else await api.inventory.stocktake(edit.productId, v, note);
      setEdit(null);
      await load();
    } catch (e: any) {
      setError(e.message);
    }
  }

  const totalValue = rows.reduce((s, r) => s + r.stock * r.cost, 0);

  return (
    <div className="page">
      <header className="page-head">
        <h1>Inventory</h1>
        <div className="actions">
          <input
            className="search"
            placeholder="Search name or code…"
            value={query}
            onChange={(e) => setQuery(e.target.value)}
          />
        </div>
      </header>

      {error && !edit && <div className="alert error">{error}</div>}

      <div className="stat-row">
        <div className="stat">
          <div className="stat-num">{rows.length}</div>
          <div className="stat-label">products</div>
        </div>
        <div className="stat">
          <div className="stat-num">
            {rows.reduce((s, r) => s + r.stock, 0)}
          </div>
          <div className="stat-label">units on hand</div>
        </div>
        <div className="stat">
          <div className="stat-num">{money(totalValue)}</div>
          <div className="stat-label">stock value (at cost)</div>
        </div>
      </div>

      <div className="card">
        <table className="grid">
          <thead>
            <tr>
              <th>Code</th>
              <th>Name</th>
              <th>Unit</th>
              <th className="r">On hand</th>
              <th className="r">Sale price</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {rows.map((r) => (
              <tr key={r.productId} className={r.stock <= 0 ? 'low' : ''}>
                <td>{r.code}</td>
                <td>{r.name}</td>
                <td>{r.unit}</td>
                <td className="r strong">{r.stock}</td>
                <td className="r">{money(r.salePrice)}</td>
                <td className="row-actions">
                  <button className="link" onClick={() => open(r, 'stocktake')}>
                    Count
                  </button>
                  <button className="link" onClick={() => open(r, 'adjust')}>
                    Adjust
                  </button>
                </td>
              </tr>
            ))}
            {rows.length === 0 && (
              <tr>
                <td colSpan={6} className="empty">
                  No products yet.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>

      {edit && (
        <div className="modal-backdrop" onClick={() => setEdit(null)}>
          <div className="modal small" onClick={(e) => e.stopPropagation()}>
            <h2>
              {mode === 'stocktake' ? 'Stock count' : 'Adjust stock'} ·{' '}
              {edit.name}
            </h2>
            <p className="muted">Current on hand: {edit.stock}</p>
            <label>
              {mode === 'stocktake' ? 'Counted quantity' : 'Change (+/-)'}
              <input
                type="number"
                value={value}
                onChange={(e) => setValue(e.target.value)}
                autoFocus
              />
            </label>
            <label>
              Note
              <input value={note} onChange={(e) => setNote(e.target.value)} />
            </label>
            {error && <div className="alert error">{error}</div>}
            <div className="modal-actions">
              <button className="btn ghost" onClick={() => setEdit(null)}>
                Cancel
              </button>
              <button className="btn" onClick={submit}>
                Apply
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
