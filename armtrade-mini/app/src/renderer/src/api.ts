const BASE =
  (window as any).appInfo?.apiBase ?? 'http://127.0.0.1:3917/api';

async function req<T>(path: string, init?: RequestInit): Promise<T> {
  const res = await fetch(`${BASE}${path}`, {
    headers: { 'Content-Type': 'application/json' },
    ...init,
  });
  if (!res.ok) {
    let message = `${res.status} ${res.statusText}`;
    try {
      const body = await res.json();
      if (body?.message) {
        message = Array.isArray(body.message)
          ? body.message.join(', ')
          : body.message;
      }
    } catch {
      /* ignore non-JSON errors */
    }
    throw new Error(message);
  }
  if (res.status === 204) return undefined as T;
  return res.json() as Promise<T>;
}

// ---- types ---------------------------------------------------------------
export interface Barcode {
  id: string;
  barcode: string;
  productId: string;
}
export interface Product {
  id: string;
  code: string;
  name: string;
  unit: string;
  salePrice: number;
  cost: number;
  vatRate: number;
  isWeight: number;
  barcodes: Barcode[];
}
export interface ProductInput {
  code: string;
  name: string;
  unit?: string;
  salePrice?: number;
  cost?: number;
  vatRate?: number;
  isWeight?: boolean;
  barcodes?: string[];
}
export interface StockRow {
  productId: string;
  code: string;
  name: string;
  unit: string;
  salePrice: number;
  cost: number;
  stock: number;
}
export interface SaleItem {
  id: string;
  name: string;
  qty: number;
  price: number;
  lineTotal: number;
}
export interface Sale {
  id: string;
  number: number;
  total: number;
  paymentType: 'cash' | 'card';
  createdAt: string;
  items: SaleItem[];
}
export interface ImportSummary {
  created: number;
  updated: number;
  barcodesAdded: number;
  pricesUpdated: number;
  stockPosted: number;
  rows: number;
  errors: string[];
}
export type StockMode = 'ignore' | 'set' | 'add';

export type DeviceType = 'fiscal' | 'scale' | 'terminal';
export interface Device {
  id: string;
  type: DeviceType;
  name: string;
  model?: string | null;
  config: Record<string, any>;
  active: number;
}
export interface DeviceInput {
  type: DeviceType;
  name: string;
  model?: string | null;
  config?: Record<string, any>;
  active?: boolean;
}
export interface FiscalResult {
  rseq: number;
  crn: string;
  sn: string;
  fiscal: string;
  total: number;
  change: number;
  qr: string;
}
export interface TerminalResult {
  approved: boolean;
  responseCode?: string;
  responseText?: string;
  rrn?: string;
  crn?: number;
  merchantReceipt?: string;
  cardholderReceipt?: string;
}
export interface ScaleExportResult {
  model: string;
  verified: boolean;
  total: number;
  exported: number;
  failed: number;
  lines: { plu: number; name: string; exported: boolean; error?: string }[];
  fileContent?: string;
  fileName?: string;
  note?: string;
}

// ---- api surface ---------------------------------------------------------
export const api = {
  products: {
    list: (query?: string) =>
      req<Product[]>(
        `/products${query ? `?query=${encodeURIComponent(query)}` : ''}`,
      ),
    byBarcode: (barcode: string) =>
      req<Product>(`/products/by-barcode/${encodeURIComponent(barcode)}`),
    create: (input: ProductInput) =>
      req<Product>('/products', {
        method: 'POST',
        body: JSON.stringify(input),
      }),
    update: (id: string, input: Partial<ProductInput>) =>
      req<Product>(`/products/${id}`, {
        method: 'PUT',
        body: JSON.stringify(input),
      }),
    remove: (id: string) =>
      req<{ ok: boolean }>(`/products/${id}`, { method: 'DELETE' }),
  },
  inventory: {
    levels: (query?: string) =>
      req<StockRow[]>(
        `/inventory${query ? `?query=${encodeURIComponent(query)}` : ''}`,
      ),
    adjust: (productId: string, qty: number, note?: string) =>
      req('/inventory/adjust', {
        method: 'POST',
        body: JSON.stringify({ productId, qty, note }),
      }),
    stocktake: (productId: string, countedQty: number, note?: string) =>
      req('/inventory/stocktake', {
        method: 'POST',
        body: JSON.stringify({ productId, countedQty, note }),
      }),
  },
  pos: {
    checkout: (
      items: { productId: string; qty: number; price: number }[],
      paymentType: 'cash' | 'card',
    ) =>
      req<Sale>('/pos/checkout', {
        method: 'POST',
        body: JSON.stringify({ items, paymentType }),
      }),
    sales: (limit = 50) => req<Sale[]>(`/pos/sales?limit=${limit}`),
  },
  import: {
    xml: (content: string, stockMode: StockMode) =>
      req<ImportSummary>('/import/xml', {
        method: 'POST',
        body: JSON.stringify({ content, stockMode }),
      }),
    excel: (base64: string, stockMode: StockMode) =>
      req<ImportSummary>('/import/excel', {
        method: 'POST',
        body: JSON.stringify({ base64, stockMode }),
      }),
    prices: (base64: string) =>
      req<ImportSummary>('/import/prices', {
        method: 'POST',
        body: JSON.stringify({ base64 }),
      }),
  },
  devices: {
    list: (type?: DeviceType) =>
      req<Device[]>(`/devices${type ? `?type=${type}` : ''}`),
    create: (input: DeviceInput) =>
      req<Device>('/devices', { method: 'POST', body: JSON.stringify(input) }),
    update: (id: string, input: Partial<DeviceInput>) =>
      req<Device>(`/devices/${id}`, { method: 'PUT', body: JSON.stringify(input) }),
    remove: (id: string) =>
      req<{ ok: boolean }>(`/devices/${id}`, { method: 'DELETE' }),
    scaleSupported: () =>
      req<{ model: string; verified: boolean; transport: string }[]>(
        '/devices/scale/supported',
      ),
    fiscalTest: (deviceId: string) =>
      req<{ ok: true }>('/devices/fiscal/test', {
        method: 'POST',
        body: JSON.stringify({ deviceId }),
      }),
    fiscalPrint: (deviceId: string, receipt: any) =>
      req<FiscalResult>('/devices/fiscal/print', {
        method: 'POST',
        body: JSON.stringify({ deviceId, receipt }),
      }),
    fiscalReport: (deviceId: string, type: 1 | 2) =>
      req('/devices/fiscal/report', {
        method: 'POST',
        body: JSON.stringify({ deviceId, type }),
      }),
    terminalTest: (deviceId: string) =>
      req<TerminalResult>('/devices/terminal/test', {
        method: 'POST',
        body: JSON.stringify({ deviceId }),
      }),
    terminalCharge: (deviceId: string, amount: number) =>
      req<TerminalResult>('/devices/terminal/charge', {
        method: 'POST',
        body: JSON.stringify({ deviceId, amount }),
      }),
    scaleExport: (deviceId: string, products: any[]) =>
      req<ScaleExportResult>('/devices/scale/export', {
        method: 'POST',
        body: JSON.stringify({ deviceId, products }),
      }),
  },
};

export function money(n: number): string {
  return new Intl.NumberFormat('en-US', {
    minimumFractionDigits: 0,
    maximumFractionDigits: 2,
  }).format(n ?? 0);
}
