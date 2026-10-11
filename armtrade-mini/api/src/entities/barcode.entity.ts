import {
  Column,
  Entity,
  Index,
  JoinColumn,
  ManyToOne,
  PrimaryColumn,
} from 'typeorm';
import { Product } from './product.entity';

/**
 * A product can have several barcodes (e.g. a unit barcode and a box barcode).
 * This is what the POS scanner looks up.
 */
@Entity('barcodes')
export class Barcode {
  @PrimaryColumn('text')
  id!: string;

  @Index({ unique: true })
  @Column('text')
  barcode!: string;

  @Column('text')
  productId!: string;

  @ManyToOne(() => Product, (p) => p.barcodes, { onDelete: 'CASCADE' })
  @JoinColumn({ name: 'productId' })
  product!: Product;
}
