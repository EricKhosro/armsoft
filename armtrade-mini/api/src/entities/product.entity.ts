import {
  Column,
  CreateDateColumn,
  Entity,
  Index,
  OneToMany,
  PrimaryColumn,
  UpdateDateColumn,
} from 'typeorm';
import { Barcode } from './barcode.entity';

/**
 * A sellable item. Prices are stored as plain numbers (AMD) for simplicity in
 * this minimal app. `cost` is the last purchase/import cost; `salePrice` is the
 * retail price used at the POS.
 */
@Entity('products')
export class Product {
  @PrimaryColumn('text')
  id!: string;

  /** Internal short code (unique). Often used in Excel imports as the key. */
  @Index({ unique: true })
  @Column('text')
  code!: string;

  @Column('text')
  name!: string;

  /** Unit of measure, e.g. "pcs", "kg". */
  @Column('text', { default: 'pcs' })
  unit!: string;

  @Column('real', { default: 0 })
  salePrice!: number;

  @Column('real', { default: 0 })
  cost!: number;

  /** VAT percent (0, 20, ...). Informational in this minimal build. */
  @Column('real', { default: 0 })
  vatRate!: number;

  @Column('integer', { default: 1 })
  active!: number;

  /** Sold by weight (1) rather than by piece (0). Used for scale export. */
  @Column('integer', { default: 0 })
  isWeight!: number;

  @OneToMany(() => Barcode, (b) => b.product, { cascade: true })
  barcodes!: Barcode[];

  @CreateDateColumn()
  createdAt!: Date;

  @UpdateDateColumn()
  updatedAt!: Date;
}
