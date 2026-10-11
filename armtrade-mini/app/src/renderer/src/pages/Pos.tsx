import { useEffect, useMemo, useRef, useState } from 'react';
import {
  api,
  money,
  Product,
  Sale,
  Device,
  FiscalResult,
  TerminalResult,
} from '../api';

interface CartLine {
  productId: string;
  name: string;
  price: number;
  qty: number;
}

export default function Pos() {
  const [scan, setScan] = useState('');
  const [cart, setCart] = useState<CartLine[]>([]);
  const [results, setResults] = useState<Product[]>([]);
  const [message, setMessage] = useState<string | null>(null);
  const [lastSale, setLastSale] = useState<Sale | null>(null);
  const [lastFiscal, setLastFiscal] = useState<FiscalResult | null>(null);
  const [lastTerminal, setLastTerminal] = useState<TerminalResult | null>(null);
  const [devices, setDevices] = useState<Device[]>([]);
  const [busy, setBusy] = useState(false);
  const scanRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    scanRef.current?.focus();
  }, [lastSale]);

  useEffect(() => {
    api.devices.list().then(setDevices).catch(() => undefined);
  }, []);

  const activeFiscal = devices.find((d) => d.type === 'fiscal' && d.active);
  const activeTerminal = devices.find((d) => d.type === 'terminal' && d.active);

  const total = useMemo(
    () => cart.reduce((s, l) => s + l.qty * l.price, 0),
    [cart],
  );

  function addProduct(p: Product) {
    setCart((prev) => {
      const existing = prev.find((l) => l.productId === p.id);
      if (existing) {
        return prev.map((l) =>
          l.productId === p.id ? { ...l, qty: l.qty + 1 } : l,
        );
      }
      return [
        ...prev,
        { productId: p.id, name: p.name, price: p.salePrice, qty: 1 },
      ];
    });
    setMessage(null);
  }

  // The barcode scanner acts as a keyboard that ends with Enter.
  async function onScanEnter() {
    const code = scan.trim();
    if (!code) return;
    setScan('');
    try {
      const p = await api.products.byBarcode(code);
      addProduct(p);
    } catch {
      // Not a barcode — fall back to a name/code search.
      try {
        const found = await api.products.list(code);
        if (found.length === 1) addProduct(found[0]);
        else if (found.length === 0) setMessage(`No product for "${code}"`);
        else setResults(found);
      } catch (e: any) {
        setMessage(e.message);
      }
    }
  }

  async function search(q: string) {
    setScan(q);
    if (q.trim().length < 2) {
      setResults([]);
      return;
    }
    try {
      setResults(await api.products.list(q));
    } catch {
      setResults([]);
    }
  }

  function setQty(productId: string, qty: number) {
    setCart((prev) =>
      prev
        .map((l) => (l.productId === productId ? { ...l, qty } : l))
        .filter((l) => l.qty > 0),
    );
  }

  function removeLine(productId: string) {
    setCart((prev) => prev.filter((l) => l.productId !== productId));
  }

  async function checkout(paymentType: 'cash' | 'card') {
    if (cart.length === 0) return;
    setBusy(true);
    setMessage(null);
    setLastFiscal(null);
    setLastTerminal(null);
    try {
      // 1) For card payments, authorise on the terminal first.
      let terminal: TerminalResult | null = null;
      if (paymentType === 'card' && activeTerminal) {
        terminal = await api.devices.terminalCharge(activeTerminal.id, total);
        if (!terminal.approved) {
          setLastTerminal(terminal);
          setMessage(`Card declined: ${terminal.responseText ?? terminal.responseCode ?? ''}`);
          setBusy(false);
          return;
        }
      }

      // 2) Record the sale (decrements stock).
      const sale = await api.pos.checkout(
        cart.map((l) => ({ productId: l.productId, qty: l.qty, price: l.price })),
        paymentType,
      );

      // 3) Print the fiscal receipt if a fiscal device is active.
      let fiscal: FiscalResult | null = null;
      if (activeFiscal) {
        try {
          fiscal = await api.devices.fiscalPrint(activeFiscal.id, {
            items: cart.map((l) => ({
              productName: l.name,
              qty: l.qty,
              price: l.price,
            })),
            paidAmount: paymentType === 'cash' ? total : 0,
            paidAmountCard: paymentType === 'card' ? total : 0,
          });
        } catch (e: any) {
          setMessage(`Sale saved, but fiscal printing failed: ${e.message}`);
        }
      }

      setLastSale(sale);
      setLastFiscal(fiscal);
      setLastTerminal(terminal);
      setCart([]);
      setResults([]);
    } catch (e: any) {
      setMessage(e.message);
    } finally {
      setBusy(false);
    }
  }

  return (
    <div className="page pos">
      <header className="page-head">
        <h1>Point of Sale</h1>
      </header>

      <div className="pos-grid">
        <section className="pos-left card">
          <input
            ref={scanRef}
            className="scan"
            placeholder="Scan barcode or type to search, then Enter…"
            value={scan}
            onChange={(e) => search(e.target.value)}
            onKeyDown={(e) => {
              if (e.key === 'Enter') onScanEnter();
            }}
          />
          {results.length > 0 && (
            <ul className="search-results">
              {results.map((p) => (
                <li
                  key={p.id}
                  onClick={() => {
                    addProduct(p);
                    setResults([]);
                    setScan('');
                    scanRef.current?.focus();
                  }}
                >
                  <span>{p.name}</span>
                  <span className="muted">{p.code}</span>
                  <span className="price">{money(p.salePrice)}</span>
                </li>
              ))}
            </ul>
          )}

          <table className="cart">
            <thead>
              <tr>
                <th>Item</th>
                <th>Price</th>
                <th>Qty</th>
                <th>Total</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {cart.length === 0 && (
                <tr>
                  <td colSpan={5} className="empty">
                    Cart is empty — scan a product to begin.
                  </td>
                </tr>
              )}
              {cart.map((l) => (
                <tr key={l.productId}>
                  <td>{l.name}</td>
                  <td>{money(l.price)}</td>
                  <td>
                    <input
                      type="number"
                      min={0}
                      value={l.qty}
                      onChange={(e) =>
                        setQty(l.productId, Number(e.target.value))
                      }
                    />
                  </td>
                  <td>{money(l.qty * l.price)}</td>
                  <td>
                    <button
                      className="link danger"
                      onClick={() => removeLine(l.productId)}
                    >
                      ✕
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </section>

        <aside className="pos-right card">
          <div className="total-box">
            <div className="total-label">Total</div>
            <div className="total-value">{money(total)}</div>
            <div className="muted">
              {cart.reduce((s, l) => s + l.qty, 0)} items
            </div>
          </div>
          <button
            className="btn big"
            disabled={busy || cart.length === 0}
            onClick={() => checkout('cash')}
          >
            Pay cash
          </button>
          <button
            className="btn big secondary"
            disabled={busy || cart.length === 0}
            onClick={() => checkout('card')}
          >
            Pay card{activeTerminal ? '' : ' (no terminal)'}
          </button>

          <div className="device-badges">
            <span className={activeFiscal ? 'badge on' : 'badge off'}>
              {activeFiscal ? `Fiscal: ${activeFiscal.name}` : 'No fiscal device'}
            </span>
            <span className={activeTerminal ? 'badge on' : 'badge off'}>
              {activeTerminal ? `Terminal: ${activeTerminal.name}` : 'No terminal'}
            </span>
          </div>

          {message && <div className="alert error">{message}</div>}

          {lastTerminal && lastTerminal.approved && (
            <div className="alert ok">
              Card approved · RRN {lastTerminal.rrn}
            </div>
          )}
          {lastFiscal && (
            <div className="alert ok">
              Fiscal #{lastFiscal.rseq} · {lastFiscal.fiscal}
              {lastFiscal.change > 0 && <> · change {money(lastFiscal.change)}</>}
            </div>
          )}
          {lastSale && (
            <div className="receipt">
              <div className="receipt-head">
                Receipt #{lastSale.number} · {money(lastSale.total)} ·{' '}
                {lastSale.paymentType}
              </div>
              {lastSale.items.map((i) => (
                <div key={i.id} className="receipt-line">
                  <span>
                    {i.qty} × {i.name}
                  </span>
                  <span>{money(i.lineTotal)}</span>
                </div>
              ))}
            </div>
          )}
        </aside>
      </div>
    </div>
  );
}
