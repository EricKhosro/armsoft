import { useEffect, useState } from 'react';
import { api, Device, DeviceInput, DeviceType } from '../api';

type ScaleInfo = { model: string; verified: boolean; transport: string };

const BLANK: Record<DeviceType, DeviceInput> = {
  fiscal: {
    type: 'fiscal',
    name: 'HDM register',
    config: { host: '127.0.0.1', port: 3930, password: '1234', cashierId: 3, pin: '0000', dep: 1 },
  },
  terminal: {
    type: 'terminal',
    name: 'Card terminal',
    config: { address: '127.0.0.1:3933' },
  },
  scale: {
    type: 'scale',
    name: 'Label scale',
    model: 'cas',
    config: { address: '127.0.0.1:3931', defaultLabelN: 1, defaultBarCodeN: 1, defaultPrefix: 22, eraseFirst: false },
  },
};

export default function Devices() {
  const [devices, setDevices] = useState<Device[]>([]);
  const [scales, setScales] = useState<ScaleInfo[]>([]);
  const [open, setOpen] = useState(false);
  const [editing, setEditing] = useState<Device | null>(null);
  const [form, setForm] = useState<DeviceInput>(BLANK.fiscal);
  const [msg, setMsg] = useState<{ kind: 'ok' | 'error'; text: string } | null>(null);
  const [testing, setTesting] = useState<string | null>(null);

  async function load() {
    setDevices(await api.devices.list());
  }
  useEffect(() => {
    load();
    api.devices.scaleSupported().then(setScales).catch(() => undefined);
  }, []);

  function openNew(type: DeviceType) {
    setEditing(null);
    setForm(JSON.parse(JSON.stringify(BLANK[type])));
    setMsg(null);
    setOpen(true);
  }
  function openEdit(d: Device) {
    setEditing(d);
    setForm({ type: d.type, name: d.name, model: d.model, config: { ...d.config }, active: !!d.active });
    setMsg(null);
    setOpen(true);
  }
  function setCfg(key: string, value: any) {
    setForm((f) => ({ ...f, config: { ...f.config, [key]: value } }));
  }

  async function save() {
    try {
      if (editing) await api.devices.update(editing.id, form);
      else await api.devices.create(form);
      setOpen(false);
      await load();
    } catch (e: any) {
      setMsg({ kind: 'error', text: e.message });
    }
  }
  async function remove(d: Device) {
    if (!confirm(`Delete "${d.name}"?`)) return;
    await api.devices.remove(d.id);
    await load();
  }

  async function test(d: Device) {
    setTesting(d.id);
    setMsg(null);
    try {
      if (d.type === 'fiscal') {
        await api.devices.fiscalTest(d.id);
        setMsg({ kind: 'ok', text: `${d.name}: fiscal login OK` });
      } else if (d.type === 'terminal') {
        const r = await api.devices.terminalTest(d.id);
        setMsg({ kind: r.approved ? 'ok' : 'error', text: `${d.name}: ping ${r.approved ? 'OK' : 'failed'}` });
      } else {
        setMsg({ kind: 'ok', text: `${d.name}: scales test by exporting from the Products page` });
      }
    } catch (e: any) {
      setMsg({ kind: 'error', text: `${d.name}: ${e.message}` });
    } finally {
      setTesting(null);
    }
  }

  const byType = (t: DeviceType) => devices.filter((d) => d.type === t);

  return (
    <div className="page">
      <header className="page-head">
        <h1>Devices</h1>
      </header>
      {msg && <div className={`alert ${msg.kind}`}>{msg.text}</div>}

      {(['fiscal', 'terminal', 'scale'] as DeviceType[]).map((type) => (
        <section className="card device-section" key={type}>
          <div className="device-head">
            <h2>
              {type === 'fiscal' ? 'Fiscal registers (HDM)' : type === 'terminal' ? 'Card terminals' : 'Label scales'}
            </h2>
            <button className="btn" onClick={() => openNew(type)}>+ Add</button>
          </div>
          <table className="grid">
            <thead>
              <tr>
                <th>Name</th>
                <th>{type === 'scale' ? 'Model' : 'Address'}</th>
                <th>Details</th>
                <th>Active</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {byType(type).map((d) => (
                <tr key={d.id}>
                  <td>{d.name}</td>
                  <td>{type === 'scale' ? (d.model || '—') : d.config.address || `${d.config.host}:${d.config.port}`}</td>
                  <td className="muted">
                    {type === 'fiscal' && `cashier ${d.config.cashierId}`}
                    {type === 'scale' && (d.config.address || '')}
                  </td>
                  <td>{d.active ? '✓' : ''}</td>
                  <td className="row-actions">
                    <button className="link" disabled={testing === d.id} onClick={() => test(d)}>
                      {testing === d.id ? 'Testing…' : 'Test'}
                    </button>
                    <button className="link" onClick={() => openEdit(d)}>Edit</button>
                    <button className="link danger" onClick={() => remove(d)}>Delete</button>
                  </td>
                </tr>
              ))}
              {byType(type).length === 0 && (
                <tr><td colSpan={5} className="empty">None configured.</td></tr>
              )}
            </tbody>
          </table>
          {type === 'scale' && scales.length > 0 && (
            <p className="muted scale-support">
              Supported models:{' '}
              {scales.map((s) => (
                <span key={s.model} className={`chip ${s.verified ? 'ok' : 'warn'}`}>
                  {s.model} {s.verified ? '✓' : '(experimental)'}
                </span>
              ))}
            </p>
          )}
        </section>
      ))}

      {open && (
        <div className="modal-backdrop" onClick={() => setOpen(false)}>
          <div className="modal" onClick={(e) => e.stopPropagation()}>
            <h2>{editing ? 'Edit device' : `New ${form.type} device`}</h2>
            <div className="form-grid">
              <label className="wide">
                Name
                <input value={form.name} onChange={(e) => setForm({ ...form, name: e.target.value })} />
              </label>

              {form.type === 'fiscal' && (
                <>
                  <label>Host<input value={form.config.host} onChange={(e) => setCfg('host', e.target.value)} /></label>
                  <label>Port<input type="number" value={form.config.port} onChange={(e) => setCfg('port', Number(e.target.value))} /></label>
                  <label>Password<input value={form.config.password} onChange={(e) => setCfg('password', e.target.value)} /></label>
                  <label>Cashier id<input type="number" value={form.config.cashierId} onChange={(e) => setCfg('cashierId', Number(e.target.value))} /></label>
                  <label>PIN<input value={form.config.pin} onChange={(e) => setCfg('pin', e.target.value)} /></label>
                  <label>Department<input type="number" value={form.config.dep} onChange={(e) => setCfg('dep', Number(e.target.value))} /></label>
                </>
              )}

              {form.type === 'terminal' && (
                <label className="wide">Address (ip:port)<input value={form.config.address} onChange={(e) => setCfg('address', e.target.value)} /></label>
              )}

              {form.type === 'scale' && (
                <>
                  <label>
                    Model
                    <select value={form.model || 'cas'} onChange={(e) => setForm({ ...form, model: e.target.value })}>
                      {scales.map((s) => (
                        <option key={s.model} value={s.model}>
                          {s.model} {s.verified ? '' : '(experimental)'}
                        </option>
                      ))}
                    </select>
                  </label>
                  <label>Address (ip:port)<input value={form.config.address} onChange={(e) => setCfg('address', e.target.value)} /></label>
                  <label>Default label #<input type="number" value={form.config.defaultLabelN} onChange={(e) => setCfg('defaultLabelN', Number(e.target.value))} /></label>
                  <label>Default barcode #<input type="number" value={form.config.defaultBarCodeN} onChange={(e) => setCfg('defaultBarCodeN', Number(e.target.value))} /></label>
                  <label>Barcode prefix<input type="number" value={form.config.defaultPrefix} onChange={(e) => setCfg('defaultPrefix', Number(e.target.value))} /></label>
                  <label className="check">
                    <input type="checkbox" checked={!!form.config.eraseFirst} onChange={(e) => setCfg('eraseFirst', e.target.checked)} />
                    Erase scale before export
                  </label>
                </>
              )}

              <label className="check wide">
                <input type="checkbox" checked={form.active !== false} onChange={(e) => setForm({ ...form, active: e.target.checked })} />
                Active (used by the POS / export)
              </label>
            </div>
            {msg && msg.kind === 'error' && <div className="alert error">{msg.text}</div>}
            <div className="modal-actions">
              <button className="btn ghost" onClick={() => setOpen(false)}>Cancel</button>
              <button className="btn" onClick={save}>Save</button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
