import { Body, Controller, Post } from '@nestjs/common';
import { ImportService, StockMode } from './import.service';

@Controller('import')
export class ImportController {
  constructor(private readonly importer: ImportService) {}

  @Post('xml')
  xml(@Body() body: { content: string; stockMode?: StockMode }) {
    return this.importer.importXml(body.content, body.stockMode || 'set');
  }

  @Post('excel')
  excel(@Body() body: { base64: string; stockMode?: StockMode }) {
    return this.importer.importExcel(body.base64, body.stockMode || 'set');
  }

  @Post('prices')
  prices(@Body() body: { base64: string }) {
    return this.importer.importPrices(body.base64);
  }
}
