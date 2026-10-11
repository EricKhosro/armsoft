import { BadRequestException, Injectable } from '@nestjs/common';
import { XMLParser } from 'fast-xml-parser';
import * as XLSX from 'xlsx';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository } from 'typeorm';
import { Product } from '../entities/product.entity';
import { Barcode } from '../entities/barcode.entity';
import { ProductsService } from '../products/products.service';
import { InventoryService } from '../inventory/inventory.service';

export type StockMode = 'ignore' | 'set' | 'add';

export interface ImportSummary {
  created: number;
  updated: number;
  barcodesAdded: number;
  pricesUpdated: number;
  stockPosted: number;
  rows: number;
  errors: string[];
}

/** Normalised row coming from any source (XML or Excel). */
interface RawRow {
  code?: string;
  name?: string;
  unit?: string;
  price?: number;
  cost?: number;
  vat?: number;
  quantity?: number;
  barcodes: string[];
}

@Injectable()
export class ImportService {
  constructor(
    @InjectRepository(Product)
    private readonly products: Repository<Product>,
    @InjectRepository(Barcode)
    private readonly barcodes: Repository<Barcode>,
    private readonly productsService: ProductsService,
    private readonly inventory: InventoryService,
  ) {}

  // ---- XML (barcodes + products) -----------------------------------------
  // Accepts a lenient shape, e.g.:
  // <Products>
  //   <Product code="A1" name="Milk" unit="pcs" price="450" cost="380" vat="20">
  //     <Barcode>4850001234567</Barcode>
  //   </Product>
  // </Products>
  // Fields may be attributes or child elements; <Barcode> may repeat.
  async importXml(content: string, stockMode: StockMode): Promise<ImportSummary> {
    if (!content || !content.trim()) {
      throw new BadRequestException('Empty XML content');
    }
    const parser = new XMLParser({
      ignoreAttributes: false,
      attributeNamePrefix: '@_',
      trimValues: true,
      isArray: (name) => /^barcode$/i.test(name) || /^product$/i.test(name),
    });
    let doc: any;
    try {
      doc = parser.parse(content);
    } catch (e: any) {
      throw new BadRequestException(`Invalid XML: ${e.message}`);
    }

    const list = findProducts(doc);
    const rows: RawRow[] = list.map((p) => ({
      code: pick(p, ['code', 'Code', '@_code', '@_Code']),
      name: pick(p, ['name', 'Name', '@_name', '@_Name']),
      unit: pick(p, ['unit', 'Unit', '@_unit', '@_Unit']),
      price: num(pick(p, ['price', 'Price', 'salePrice', '@_price', '@_Price'])),
      cost: num(pick(p, ['cost', 'Cost', '@_cost', '@_Cost'])),
      vat: num(pick(p, ['vat', 'Vat', 'vatRate', '@_vat', '@_Vat'])),
      quantity: num(
        pick(p, ['quantity', 'Quantity', 'qty', 'Qty', 'stock', '@_quantity']),
      ),
      barcodes: collectBarcodes(p),
    }));

    return this.applyRows(rows, { updatePrices: true, stockMode });
  }

  // ---- Excel (products + prices + optional quantity) ---------------------
  async importExcel(
    base64: string,
    stockMode: StockMode,
  ): Promise<ImportSummary> {
    const rows = this.readSheet(base64);
    return this.applyRows(rows, { updatePrices: true, stockMode });
  }

  // ---- Excel (prices only) ----------------------------------------------
  // Only salePrice/cost are touched; products are matched by code or barcode
  // and are NOT created.
  async importPrices(base64: string): Promise<ImportSummary> {
    const rows = this.readSheet(base64);
    const summary = blankSummary(rows.length);

    for (const [i, row] of rows.entries()) {
      try {
        const product = await this.matchProduct(row);
        if (!product) {
          summary.errors.push(`Row ${i + 2}: no product matched (code/barcode)`);
          continue;
        }
        let changed = false;
        if (row.price != null) {
          product.salePrice = row.price;
          changed = true;
        }
        if (row.cost != null) {
          product.cost = row.cost;
          changed = true;
        }
        if (changed) {
          await this.products.save(product);
          summary.pricesUpdated++;
          summary.updated++;
        }
      } catch (e: any) {
        summary.errors.push(`Row ${i + 2}: ${e.message}`);
      }
    }
    return summary;
  }

  // ---- shared row application -------------------------------------------
  private async applyRows(
    rows: RawRow[],
    opts: { updatePrices: boolean; stockMode: StockMode },
  ): Promise<ImportSummary> {
    const summary = blankSummary(rows.length);

    for (const [i, row] of rows.entries()) {
      const line = i + 2; // human-friendly (header is row 1)
      try {
        if (!row.code && row.barcodes.length === 0) {
          summary.errors.push(`Row ${line}: missing both code and barcode`);
          continue;
        }
        let product = await this.matchProduct(row);

        if (!product) {
          const created = await this.productsService.create({
            code: row.code || row.barcodes[0],
            name: row.name || row.code || row.barcodes[0],
            unit: row.unit,
            salePrice: row.price ?? 0,
            cost: row.cost ?? 0,
            vatRate: row.vat ?? 0,
            barcodes: row.barcodes,
          });
          product = await this.products.findOneOrFail({
            where: { id: created.id },
          });
          summary.created++;
          summary.barcodesAdded += row.barcodes.length;
        } else {
          if (row.name) product.name = row.name;
          if (row.unit) product.unit = row.unit;
          if (opts.updatePrices && row.price != null) {
            product.salePrice = row.price;
            summary.pricesUpdated++;
          }
          if (opts.updatePrices && row.cost != null) product.cost = row.cost;
          if (row.vat != null) product.vatRate = row.vat;
          await this.products.save(product);
          summary.updated++;
          summary.barcodesAdded += await this.addMissingBarcodes(
            product.id,
            row.barcodes,
          );
        }

        if (opts.stockMode !== 'ignore' && row.quantity != null && product) {
          if (opts.stockMode === 'set') {
            const m = await this.inventory.stocktake(
              product.id,
              row.quantity,
              'Excel/XML import (set)',
            );
            if (m) summary.stockPosted++;
          } else {
            await this.inventory.addMovement(
              product.id,
              row.quantity,
              'import',
              'Excel/XML import (add)',
            );
            summary.stockPosted++;
          }
        }
      } catch (e: any) {
        summary.errors.push(`Row ${line}: ${e.message}`);
      }
    }
    return summary;
  }

  private async matchProduct(row: RawRow): Promise<Product | null> {
    if (row.code) {
      const byCode = await this.products.findOne({ where: { code: row.code } });
      if (byCode) return byCode;
    }
    for (const bc of row.barcodes) {
      const b = await this.barcodes.findOne({
        where: { barcode: bc },
        relations: { product: true },
      });
      if (b?.product) {
        return this.products.findOneOrFail({ where: { id: b.product.id } });
      }
    }
    return null;
  }

  private async addMissingBarcodes(
    productId: string,
    codes: string[],
  ): Promise<number> {
    let added = 0;
    for (const bc of codes) {
      const exists = await this.barcodes.findOne({ where: { barcode: bc } });
      if (!exists) {
        await this.barcodes.save(
          this.barcodes.create({
            id: require('uuid').v4(),
            barcode: bc,
            productId,
          }),
        );
        added++;
      }
    }
    return added;
  }

  /** Parse an xlsx (base64) into normalised rows using header aliases. */
  private readSheet(base64: string): RawRow[] {
    if (!base64) throw new BadRequestException('Empty file');
    const clean = base64.includes(',') ? base64.split(',')[1] : base64;
    let wb: XLSX.WorkBook;
    try {
      wb = XLSX.read(Buffer.from(clean, 'base64'), { type: 'buffer' });
    } catch (e: any) {
      throw new BadRequestException(`Could not read spreadsheet: ${e.message}`);
    }
    const sheet = wb.Sheets[wb.SheetNames[0]];
    if (!sheet) throw new BadRequestException('Spreadsheet has no sheets');
    const raw: any[] = XLSX.utils.sheet_to_json(sheet, { defval: null });

    return raw.map((r) => {
      const get = (aliases: string[]) => {
        let firstMatch: any = undefined;
        for (const key of Object.keys(r)) {
          if (aliases.includes(normHeader(key))) {
            const val = r[key];
            if (val != null && String(val).trim() !== '') return val;
            if (firstMatch === undefined) firstMatch = val;
          }
        }
        return firstMatch;
      };
      const barcodeRaw = get(['barcode', 'barcodes', 'ean', 'barcod']);
      const barcodes = String(barcodeRaw ?? '')
        .split(/[;,|]/)
        .map((s) => s.trim())
        .filter(Boolean);
      return {
        code: strOrUndef(get(['code', 'productcode', 'sku', 'article'])),
        name: strOrUndef(get(['name', 'productname', 'title', 'description'])),
        unit: strOrUndef(get(['unit', 'uom', 'measure'])),
        price: num(get(['price', 'saleprice', 'retailprice', 'sellprice'])),
        cost: num(get(['cost', 'costprice', 'purchaseprice', 'buyprice'])),
        vat: num(get(['vat', 'vatrate', 'tax'])),
        quantity: num(get(['quantity', 'qty', 'stock', 'count', 'onhand'])),
        barcodes,
      } as RawRow;
    });
  }
}

// ---- helpers -------------------------------------------------------------
function blankSummary(rows: number): ImportSummary {
  return {
    created: 0,
    updated: 0,
    barcodesAdded: 0,
    pricesUpdated: 0,
    stockPosted: 0,
    rows,
    errors: [],
  };
}

function normHeader(h: string): string {
  return String(h).toLowerCase().replace(/[\s_\-.]/g, '');
}

function strOrUndef(v: any): string | undefined {
  if (v == null) return undefined;
  const s = String(v).trim();
  return s ? s : undefined;
}

function num(v: any): number | undefined {
  if (v == null || v === '') return undefined;
  const n = Number(String(v).replace(/\s/g, '').replace(',', '.'));
  return Number.isFinite(n) ? n : undefined;
}

function pick(obj: any, keys: string[]): any {
  for (const k of keys) {
    if (obj && obj[k] != null && obj[k] !== '') return obj[k];
  }
  return undefined;
}

function collectBarcodes(p: any): string[] {
  const out: string[] = [];
  const raw = p?.Barcode ?? p?.barcode ?? p?.Barcodes ?? p?.barcodes;
  const push = (v: any) => {
    if (v == null) return;
    const s = typeof v === 'object' ? v['#text'] ?? v['@_value'] : v;
    if (s != null && String(s).trim()) out.push(String(s).trim());
  };
  if (Array.isArray(raw)) raw.forEach(push);
  else push(raw);
  // also accept a comma-separated attribute
  const attr = p?.['@_barcode'] ?? p?.['@_barcodes'];
  if (attr) String(attr).split(/[;,|]/).forEach((s) => push(s));
  return [...new Set(out)];
}

/** Find <Product> nodes regardless of the wrapping element name. */
function findProducts(doc: any): any[] {
  if (!doc || typeof doc !== 'object') return [];
  for (const key of Object.keys(doc)) {
    const val = doc[key];
    if (Array.isArray(val) && /product/i.test(key)) return val;
    if (val && typeof val === 'object') {
      for (const inner of Object.keys(val)) {
        if (/product/i.test(inner)) {
          const arr = val[inner];
          return Array.isArray(arr) ? arr : [arr];
        }
      }
    }
  }
  return [];
}
