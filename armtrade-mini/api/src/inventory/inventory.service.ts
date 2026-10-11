import { Injectable } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository, DataSource } from 'typeorm';
import { v4 as uuid } from 'uuid';
import {
  StockMovement,
  StockMovementType,
} from '../entities/stock-movement.entity';

export interface StockRow {
  productId: string;
  code: string;
  name: string;
  unit: string;
  salePrice: number;
  cost: number;
  stock: number;
}

@Injectable()
export class InventoryService {
  constructor(
    @InjectRepository(StockMovement)
    private readonly movements: Repository<StockMovement>,
    private readonly dataSource: DataSource,
  ) {}

  /** Current stock for every product, joined with product info. */
  async levels(query?: string): Promise<StockRow[]> {
    const params: any[] = [];
    let where = '';
    if (query && query.trim()) {
      where = 'WHERE p.name LIKE ? OR p.code LIKE ?';
      params.push(`%${query.trim()}%`, `%${query.trim()}%`);
    }
    const rows = await this.dataSource.query(
      `SELECT p.id AS productId, p.code AS code, p.name AS name,
              p.unit AS unit, p.salePrice AS salePrice, p.cost AS cost,
              COALESCE(SUM(m.qty), 0) AS stock
       FROM products p
       LEFT JOIN stock_movements m ON m.productId = p.id
       ${where}
       GROUP BY p.id
       ORDER BY p.name ASC`,
      params,
    );
    return rows.map((r: any) => ({ ...r, stock: Number(r.stock) }));
  }

  async stockFor(productId: string): Promise<number> {
    const [row] = await this.dataSource.query(
      `SELECT COALESCE(SUM(qty), 0) AS stock FROM stock_movements WHERE productId = ?`,
      [productId],
    );
    return Number(row?.stock ?? 0);
  }

  async addMovement(
    productId: string,
    qty: number,
    type: StockMovementType,
    note?: string,
    refId?: string,
  ): Promise<StockMovement> {
    const m = this.movements.create({
      id: uuid(),
      productId,
      qty,
      type,
      note: note ?? null,
      refId: refId ?? null,
    });
    return this.movements.save(m);
  }

  /** Manual stock adjustment (+/-). */
  adjust(productId: string, qty: number, note?: string) {
    return this.addMovement(productId, qty, 'adjustment', note);
  }

  /**
   * Stocktake: set the on-hand to an exact count by posting the difference
   * against current stock.
   */
  async stocktake(productId: string, countedQty: number, note?: string) {
    const current = await this.stockFor(productId);
    const diff = countedQty - current;
    if (diff === 0) return null;
    return this.addMovement(productId, diff, 'stocktake', note);
  }

  history(productId: string) {
    return this.movements.find({
      where: { productId },
      order: { createdAt: 'DESC' },
      take: 100,
    });
  }
}
