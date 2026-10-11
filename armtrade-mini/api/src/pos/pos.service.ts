import { BadRequestException, Injectable } from '@nestjs/common';
import { DataSource } from 'typeorm';
import { v4 as uuid } from 'uuid';
import { Sale, PaymentType } from '../entities/sale.entity';
import { SaleItem } from '../entities/sale-item.entity';
import { StockMovement } from '../entities/stock-movement.entity';
import { Product } from '../entities/product.entity';

export interface CheckoutItem {
  productId: string;
  qty: number;
  price: number;
}

export interface CheckoutInput {
  items: CheckoutItem[];
  paymentType?: PaymentType;
}

@Injectable()
export class PosService {
  constructor(private readonly dataSource: DataSource) {}

  async checkout(input: CheckoutInput): Promise<Sale> {
    if (!input.items || input.items.length === 0) {
      throw new BadRequestException('Cannot check out an empty cart');
    }

    return this.dataSource.transaction(async (manager) => {
      // Next receipt number.
      const [{ maxNo }] = await manager.query(
        `SELECT COALESCE(MAX(number), 0) AS maxNo FROM sales`,
      );
      const number = Number(maxNo) + 1;

      const saleId = uuid();
      let total = 0;
      const items: SaleItem[] = [];
      const moves: StockMovement[] = [];

      for (const line of input.items) {
        const qty = Number(line.qty);
        const price = Number(line.price);
        if (!(qty > 0)) {
          throw new BadRequestException('Line quantity must be positive');
        }
        const product = await manager.findOne(Product, {
          where: { id: line.productId },
        });
        if (!product) {
          throw new BadRequestException(`Unknown product ${line.productId}`);
        }
        const lineTotal = round2(qty * price);
        total += lineTotal;

        items.push(
          manager.create(SaleItem, {
            id: uuid(),
            saleId,
            productId: product.id,
            name: product.name,
            qty,
            price,
            lineTotal,
          }),
        );
        // Sale removes stock → negative movement.
        moves.push(
          manager.create(StockMovement, {
            id: uuid(),
            productId: product.id,
            qty: -qty,
            type: 'sale',
            refId: saleId,
          }),
        );
      }

      const sale = manager.create(Sale, {
        id: saleId,
        number,
        total: round2(total),
        paymentType: input.paymentType || 'cash',
      });

      await manager.save(sale);
      await manager.save(items);
      await manager.save(moves);

      return manager.findOneOrFail(Sale, {
        where: { id: saleId },
        relations: { items: true },
      });
    });
  }

  recentSales(limit = 50) {
    return this.dataSource.getRepository(Sale).find({
      order: { createdAt: 'DESC' },
      take: limit,
      relations: { items: true },
    });
  }
}

function round2(n: number): number {
  return Math.round((n + Number.EPSILON) * 100) / 100;
}
