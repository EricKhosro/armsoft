import {
  Column,
  Entity,
  JoinColumn,
  ManyToOne,
  PrimaryColumn,
} from 'typeorm';
import { Sale } from './sale.entity';

@Entity('sale_items')
export class SaleItem {
  @PrimaryColumn('text')
  id!: string;

  @Column('text')
  saleId!: string;

  @Column('text')
  productId!: string;

  /** Snapshot of the product name at sale time. */
  @Column('text')
  name!: string;

  @Column('real')
  qty!: number;

  @Column('real')
  price!: number;

  @Column('real')
  lineTotal!: number;

  @ManyToOne(() => Sale, (s) => s.items, { onDelete: 'CASCADE' })
  @JoinColumn({ name: 'saleId' })
  sale!: Sale;
}
