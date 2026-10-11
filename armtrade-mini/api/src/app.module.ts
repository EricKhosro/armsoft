import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { join } from 'path';
import { Product } from './entities/product.entity';
import { Barcode } from './entities/barcode.entity';
import { StockMovement } from './entities/stock-movement.entity';
import { Sale } from './entities/sale.entity';
import { SaleItem } from './entities/sale-item.entity';
import { Device } from './entities/device.entity';
import { ProductsModule } from './products/products.module';
import { InventoryModule } from './inventory/inventory.module';
import { PosModule } from './pos/pos.module';
import { ImportModule } from './import/import.module';
import { DevicesModule } from './devices/devices.module';

/**
 * The SQLite file lives next to the API by default, or wherever ARMTRADE_DB_PATH
 * points (Electron sets this to the user's app-data folder in production).
 */
const dbPath =
  process.env.ARMTRADE_DB_PATH || join(process.cwd(), 'armtrade.sqlite');

@Module({
  imports: [
    TypeOrmModule.forRoot({
      type: 'better-sqlite3',
      database: dbPath,
      entities: [Product, Barcode, StockMovement, Sale, SaleItem, Device],
      synchronize: true, // auto-create schema; fine for this minimal app
    }),
    ProductsModule,
    InventoryModule,
    PosModule,
    ImportModule,
    DevicesModule,
  ],
})
export class AppModule {}
