import {
  Body,
  Controller,
  Delete,
  Get,
  Param,
  Post,
  Put,
  Query,
} from '@nestjs/common';
import { ProductsService, ProductInput } from './products.service';

@Controller('products')
export class ProductsController {
  constructor(private readonly products: ProductsService) {}

  @Get()
  findAll(@Query('query') query?: string) {
    return this.products.findAll(query);
  }

  @Get('by-barcode/:barcode')
  findByBarcode(@Param('barcode') barcode: string) {
    return this.products.findByBarcode(barcode);
  }

  @Get(':id')
  findOne(@Param('id') id: string) {
    return this.products.findOne(id);
  }

  @Post()
  create(@Body() body: ProductInput) {
    return this.products.create(body);
  }

  @Put(':id')
  update(@Param('id') id: string, @Body() body: Partial<ProductInput>) {
    return this.products.update(id, body);
  }

  @Delete(':id')
  async remove(@Param('id') id: string) {
    await this.products.remove(id);
    return { ok: true };
  }
}
