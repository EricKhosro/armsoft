import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { Sale } from '../entities/sale.entity';
import { SaleItem } from '../entities/sale-item.entity';
import { StockMovement } from '../entities/stock-movement.entity';
import { Product } from '../entities/product.entity';
import { PosService } from './pos.service';
import { PosController } from './pos.controller';

@Module({
  imports: [
    TypeOrmModule.forFeature([Sale, SaleItem, StockMovement, Product]),
  ],
  providers: [PosService],
  controllers: [PosController],
})
export class PosModule {}
