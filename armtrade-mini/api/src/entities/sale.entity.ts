import {
  Column,
  CreateDateColumn,
  Entity,
  OneToMany,
  PrimaryColumn,
} from 'typeorm';
import { SaleItem } from './sale-item.entity';

export type PaymentType = 'cash' | 'card';

/** A completed POS sale (receipt). */
@Entity('sales')
export class Sale {
  @PrimaryColumn('text')
  id!: string;

  /** Human-friendly sequential receipt number. */
  @Column('integer')
  number!: number;

  @Column('real')
  total!: number;

  @Column('text', { default: 'cash' })
  paymentType!: PaymentType;

  @OneToMany(() => SaleItem, (i) => i.sale, { cascade: true })
  items!: SaleItem[];

  @CreateDateColumn()
  createdAt!: Date;
}
