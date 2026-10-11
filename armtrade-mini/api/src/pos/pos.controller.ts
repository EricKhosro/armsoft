import { Body, Controller, Get, Post, Query } from '@nestjs/common';
import { PosService, CheckoutInput } from './pos.service';

@Controller('pos')
export class PosController {
  constructor(private readonly pos: PosService) {}

  @Post('checkout')
  checkout(@Body() body: CheckoutInput) {
    return this.pos.checkout(body);
  }

  @Get('sales')
  sales(@Query('limit') limit?: string) {
    return this.pos.recentSales(limit ? Number(limit) : 50);
  }
}
