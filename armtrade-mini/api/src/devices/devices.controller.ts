import {
  BadRequestException,
  Body,
  Controller,
  Delete,
  Get,
  Param,
  Post,
  Put,
  Query,
} from '@nestjs/common';
import { DevicesService, DeviceInput } from './devices.service';
import { FiscalService, FiscalConfig, FiscalReceiptInput } from './fiscal/fiscal.service';
import { TerminalService } from './terminal/terminal.service';
import { ScaleService, ScaleConfig, ScaleProductInput } from './scale/scale.service';
import { DeviceType } from '../entities/device.entity';
import { ScaleModel } from './scale/types';

@Controller('devices')
export class DevicesController {
  constructor(
    private readonly devices: DevicesService,
    private readonly fiscal: FiscalService,
    private readonly terminal: TerminalService,
    private readonly scale: ScaleService,
  ) {}

  // ---- device configuration CRUD ---------------------------------------
  @Get()
  list(@Query('type') type?: DeviceType) {
    return this.devices.findAll(type);
  }

  @Get('scale/supported')
  scaleSupported() {
    return this.scale.supported();
  }

  @Get(':id')
  get(@Param('id') id: string) {
    return this.devices.findOne(id);
  }

  @Post()
  create(@Body() body: DeviceInput) {
    return this.devices.create(body);
  }

  @Put(':id')
  update(@Param('id') id: string, @Body() body: Partial<DeviceInput>) {
    return this.devices.update(id, body);
  }

  @Delete(':id')
  async remove(@Param('id') id: string) {
    await this.devices.remove(id);
    return { ok: true };
  }

  // ---- fiscal (HDM) operations -----------------------------------------
  @Post('fiscal/test')
  async fiscalTest(@Body() body: DeviceRef) {
    return this.fiscal.test(await this.fiscalCfg(body));
  }

  @Post('fiscal/print')
  async fiscalPrint(@Body() body: DeviceRef & { receipt: FiscalReceiptInput }) {
    return this.fiscal.printReceipt(await this.fiscalCfg(body), body.receipt);
  }

  @Post('fiscal/report')
  async fiscalReport(@Body() body: DeviceRef & { type: 1 | 2 }) {
    return this.fiscal.report(await this.fiscalCfg(body), body.type === 2 ? 2 : 1);
  }

  @Post('fiscal/cash')
  async fiscalCash(
    @Body() body: DeviceRef & { amount: number; isCashIn: boolean; description?: string },
  ) {
    return this.fiscal.cashInOut(
      await this.fiscalCfg(body),
      Number(body.amount),
      !!body.isCashIn,
      body.description,
    );
  }

  // ---- card terminal (PossoftPay) --------------------------------------
  @Post('terminal/test')
  async terminalTest(@Body() body: DeviceRef) {
    return this.terminal.ping(await this.terminalAddress(body));
  }

  @Post('terminal/charge')
  async terminalCharge(@Body() body: DeviceRef & { amount: number }) {
    return this.terminal.sale(await this.terminalAddress(body), Number(body.amount));
  }

  @Post('terminal/refund')
  async terminalRefund(@Body() body: DeviceRef & { amount: number; rrn: string }) {
    return this.terminal.refund(
      await this.terminalAddress(body),
      Number(body.amount),
      body.rrn,
    );
  }

  // ---- label-scale export ----------------------------------------------
  @Post('scale/export')
  async scaleExport(
    @Body()
    body: DeviceRef & {
      model?: ScaleModel;
      config?: ScaleConfig;
      products: ScaleProductInput[];
    },
  ) {
    const { model, cfg } = await this.scaleCfg(body);
    return this.scale.export(model, cfg, body.products);
  }

  // ---- config resolution ------------------------------------------------
  private async fiscalCfg(body: DeviceRef): Promise<FiscalConfig> {
    const cfg = await this.resolveConfig(body, 'fiscal');
    return cfg as FiscalConfig;
  }

  private async terminalAddress(body: DeviceRef): Promise<string> {
    const cfg = await this.resolveConfig(body, 'terminal');
    const address = cfg.address || cfg.POSTerminalAddress;
    if (!address) throw new BadRequestException('Terminal address is not configured');
    return address;
  }

  private async scaleCfg(
    body: DeviceRef & { model?: ScaleModel; config?: ScaleConfig },
  ): Promise<{ model: ScaleModel; cfg: ScaleConfig }> {
    if (body.deviceId) {
      const d = await this.devices.findOne(body.deviceId);
      return {
        model: (body.model || (d.model as ScaleModel)) as ScaleModel,
        cfg: { ...(d.config as ScaleConfig), ...(body.config || {}) },
      };
    }
    if (!body.model) throw new BadRequestException('Scale model is required');
    return { model: body.model, cfg: body.config || {} };
  }

  /** Load a device's config by id, merge an inline override, or use the active one. */
  private async resolveConfig(
    body: DeviceRef,
    type: DeviceType,
  ): Promise<Record<string, any>> {
    if (body.deviceId) {
      const d = await this.devices.findOne(body.deviceId);
      return { ...d.config, ...(body.config || {}) };
    }
    if (body.config) return body.config;
    const active = await this.devices.active(type);
    if (!active) {
      throw new BadRequestException(`No active ${type} device configured`);
    }
    return active.config;
  }
}

interface DeviceRef {
  deviceId?: string;
  config?: Record<string, any>;
}
