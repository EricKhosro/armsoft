import { Injectable, NotFoundException } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository, DataSource, Like } from 'typeorm';
import { v4 as uuid } from 'uuid';
import { Product } from '../entities/product.entity';
import { Barcode } from '../entities/barcode.entity';

export interface ProductInput {
  code: string;
  name: string;
  unit?: string;
  salePrice?: number;
  cost?: number;
  vatRate?: number;
  isWeight?: boolean;
  barcodes?: string[];
}

@Injectable()
export class ProductsService {
  constructor(
    @InjectRepository(Product)
    private readonly products: Repository<Product>,
    @InjectRepository(Barcode)
    private readonly barcodes: Repository<Barcode>,
    private readonly dataSource: DataSource,
  ) {}

  async findAll(query?: string): Promise<Product[]> {
    if (query && query.trim()) {
      const q = `%${query.trim()}%`;
      return this.products.find({
        where: [{ name: Like(q) }, { code: Like(q) }],
        relations: { barcodes: true },
        order: { name: 'ASC' },
        take: 200,
      });
    }
    return this.products.find({
      relations: { barcodes: true },
      order: { name: 'ASC' },
      take: 200,
    });
  }

  async findOne(id: string): Promise<Product> {
    const p = await this.products.findOne({
      where: { id },
      relations: { barcodes: true },
    });
    if (!p) throw new NotFoundException(`Product ${id} not found`);
    return p;
  }

  async findByBarcode(barcode: string): Promise<Product> {
    const b = await this.barcodes.findOne({
      where: { barcode },
      relations: { product: true },
    });
    if (!b || !b.product) {
      throw new NotFoundException(`No product for barcode ${barcode}`);
    }
    return this.findOne(b.product.id);
  }

  async create(input: ProductInput): Promise<Product> {
    const product = this.products.create({
      id: uuid(),
      code: input.code,
      name: input.name,
      unit: input.unit || 'pcs',
      salePrice: input.salePrice ?? 0,
      cost: input.cost ?? 0,
      vatRate: input.vatRate ?? 0,
      isWeight: input.isWeight ? 1 : 0,
      active: 1,
    });
    await this.products.save(product);
    await this.syncBarcodes(product.id, input.barcodes || []);
    return this.findOne(product.id);
  }

  async update(id: string, input: Partial<ProductInput>): Promise<Product> {
    const product = await this.findOne(id);
    Object.assign(product, {
      code: input.code ?? product.code,
      name: input.name ?? product.name,
      unit: input.unit ?? product.unit,
      salePrice: input.salePrice ?? product.salePrice,
      cost: input.cost ?? product.cost,
      vatRate: input.vatRate ?? product.vatRate,
      isWeight:
        input.isWeight === undefined ? product.isWeight : input.isWeight ? 1 : 0,
    });
    await this.products.save(product);
    if (input.barcodes) await this.syncBarcodes(id, input.barcodes);
    return this.findOne(id);
  }

  async remove(id: string): Promise<void> {
    await this.findOne(id);
    await this.products.delete(id);
  }

  /** Replace a product's barcode set with the given list. */
  private async syncBarcodes(productId: string, codes: string[]): Promise<void> {
    const wanted = [...new Set(codes.map((c) => String(c).trim()).filter(Boolean))];
    await this.barcodes.delete({ productId });
    for (const code of wanted) {
      await this.barcodes.save(
        this.barcodes.create({ id: uuid(), barcode: code, productId }),
      );
    }
  }
}
