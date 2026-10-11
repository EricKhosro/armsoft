/** A product prepared for export to a label-printing scale. */
export interface ScalePlu {
  /** PLU code stored on the scale (numeric). */
  plu: number;
  /** Numeric item/barcode code embedded in the printed barcode. */
  barcode: number;
  /** Barcode prefix / group. */
  prefix: number;
  name: string;
  /** Ingredients / direct message (optional). */
  description?: string;
  /** Decimal unit price (e.g. 450.00). Each protocol scales as it requires. */
  price: number;
  /** true = sold by weight, false = sold by piece. */
  isWeight: boolean;
  /** Label format id configured on the scale. */
  labelN: number;
  /** Barcode format id configured on the scale. */
  barCodeN: number;
}

export interface ScaleLineResult {
  plu: number;
  name: string;
  exported: boolean;
  error?: string;
}

export interface ScaleExportResult {
  model: string;
  /** false when the protocol is ported but not yet verified against a device. */
  verified: boolean;
  total: number;
  exported: number;
  failed: number;
  lines: ScaleLineResult[];
  /** For file-based models (CASNew): the produced file content. */
  fileContent?: string;
  fileName?: string;
  note?: string;
}

export type ScaleModel =
  | 'cas'
  | 'casnew'
  | 'digi'
  | 'tma'
  | 'mertech'
  | 'massak';
