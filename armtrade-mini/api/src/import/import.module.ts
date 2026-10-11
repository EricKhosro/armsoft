import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { Product } from '../entities/product.entity';
import { Barcode } from '../entities/barcode.entity';
import { ProductsModule } from '../products/products.module';
import { InventoryModule } from '../inventory/inventory.module';
import { ImportService } from './import.service';
import { ImportController } from './import.controller';

@Module({
  imports: [
    TypeOrmModule.forFeature([Product, Barcode]),
    ProductsModule,
    InventoryModule,
  ],
  providers: [ImportService],
  controllers: [ImportController],
})
export class ImportModule {}
