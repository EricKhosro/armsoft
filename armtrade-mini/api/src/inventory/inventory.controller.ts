import { Body, Controller, Get, Param, Post, Query } from '@nestjs/common';
import { InventoryService } from './inventory.service';

@Controller('inventory')
export class InventoryController {
  constructor(private readonly inventory: InventoryService) {}

  @Get()
  levels(@Query('query') query?: string) {
    return this.inventory.levels(query);
  }

  @Get(':productId/history')
  history(@Param('productId') productId: string) {
    return this.inventory.history(productId);
  }

  @Post('adjust')
  adjust(
    @Body() body: { productId: string; qty: number; note?: string },
  ) {
    return this.inventory.adjust(body.productId, Number(body.qty), body.note);
  }

  @Post('stocktake')
  stocktake(
    @Body() body: { productId: string; countedQty: number; note?: string },
  ) {
    return this.inventory.stocktake(
      body.productId,
      Number(body.countedQty),
      body.note,
    );
  }
}
