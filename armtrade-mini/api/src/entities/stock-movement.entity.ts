import {
  Column,
  CreateDateColumn,
  Entity,
  Index,
  PrimaryColumn,
} from 'typeorm';

export type StockMovementType = 'import' | 'sale' | 'adjustment' | 'stocktake';

/**
 * Inventory is derived: the current stock of a product is the SUM of the signed
 * quantities of its movements. Positive = stock in (import, positive adjust),
 * negative = stock out (sale). This keeps a full audit trail instead of a single
 * mutable "quantity" field.
 */
@Entity('stock_movements')
export class StockMovement {
  @PrimaryColumn('text')
  id!: string;

  @Index()
  @Column('text')
  productId!: string;

  /** Signed quantity. */
  @Column('real')
  qty!: number;

  @Column('text')
  type!: StockMovementType;

  @Column('text', { nullable: true })
  note?: string | null;

  /** Reference to the source record, e.g. a sale id. */
  @Column('text', { nullable: true })
  refId?: string | null;

  @CreateDateColumn()
  createdAt!: Date;
}
