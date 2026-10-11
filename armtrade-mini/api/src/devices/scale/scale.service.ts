import { BadRequestException, Injectable, Logger } from '@nestjs/common';
import { ScaleModel, ScalePlu, ScaleExportResult } from './types';
import * as cas from './protocols/cas';
import * as tma from './protocols/tma';
import * as casnew from './protocols/casnew';

export interface ScaleConfig {
  /** "ip:port" for network scales. */
  address?: string;
  /** Defaults applied when a product doesn't specify them. */
  defaultLabelN?: number;
  defaultBarCodeN?: number;
  defaultPrefix?: number;
  eraseFirst?: boolean;
}

/** A product row to export (from the Products/Inventory UI). */
export interface ScaleProductInput {
  plu: number;
  barcode: number;
  name: string;
  price: number;
  isWeight: boolean;
  prefix?: number;
  description?: string;
  labelN?: number;
  barCodeN?: number;
}

const VERIFIED: ScaleModel[] = ['cas', 'tma', 'casnew'];

/**
 * Dispatches an export to the right scale protocol.
 *
 * Status of each family (faithful to AS Trade's connectors):
 *  - cas, tma   → fully ported from source and tested against a simulator.
 *  - casnew     → file (XML) export, as in the original (no network transport).
 *  - digi, mertech → ported mapping exists but the binary framing is NOT yet
 *                    verified against a device; disabled until confirmed.
 *  - massak     → original uses an in-process Windows COM driver; cannot run
 *                 from the Node service. Use CAS/TM-A or request a bridge.
 */
@Injectable()
export class ScaleService {
  private readonly log = new Logger('ScaleService');

  supported(): { model: ScaleModel; verified: boolean; transport: string }[] {
    return [
      { model: 'cas', verified: true, transport: 'TCP (binary)' },
      { model: 'tma', verified: true, transport: 'TCP (tab text)' },
      { model: 'casnew', verified: true, transport: 'file (XML)' },
      { model: 'digi', verified: false, transport: 'TCP (BCD) — experimental' },
      { model: 'mertech', verified: false, transport: 'TCP (framed JSON) — experimental' },
      { model: 'massak', verified: false, transport: 'COM driver — unsupported in Node' },
    ];
  }

  async export(
    model: ScaleModel,
    cfg: ScaleConfig,
    products: ScaleProductInput[],
  ): Promise<ScaleExportResult> {
    if (!products?.length) {
      throw new BadRequestException('Nothing to export');
    }
    const plus = products.map((p) => this.toPlu(p, cfg));

    switch (model) {
      case 'cas':
        this.requireAddress(cfg);
        return cas.exportPlus(cfg.address!, plus, { eraseFirst: cfg.eraseFirst });
      case 'tma':
        this.requireAddress(cfg);
        return tma.exportPlus(cfg.address!, plus, { eraseFirst: cfg.eraseFirst });
      case 'casnew':
        return casnew.exportFile(plus);
      case 'digi':
      case 'mertech':
        throw new BadRequestException(
          `The ${model.toUpperCase()} protocol is ported from analysis but not yet ` +
            `verified against a device, so it is disabled to avoid writing bad data. ` +
            `Tell me your exact model and I'll finish and test it.`,
        );
      case 'massak':
        throw new BadRequestException(
          `Massa-K uses an in-process Windows COM driver, which the Node service ` +
            `cannot load. Use CAS or TM-A, or request a native bridge.`,
        );
      default:
        throw new BadRequestException(`Unknown scale model "${model}"`);
    }
  }

  isVerified(model: ScaleModel): boolean {
    return VERIFIED.includes(model);
  }

  private toPlu(p: ScaleProductInput, cfg: ScaleConfig): ScalePlu {
    return {
      plu: Number(p.plu),
      barcode: Number(p.barcode),
      prefix: Number(p.prefix ?? cfg.defaultPrefix ?? 0),
      name: p.name,
      description: p.description,
      price: Number(p.price),
      isWeight: !!p.isWeight,
      labelN: Number(p.labelN ?? cfg.defaultLabelN ?? 1),
      barCodeN: Number(p.barCodeN ?? cfg.defaultBarCodeN ?? 1),
    };
  }

  private requireAddress(cfg: ScaleConfig): void {
    if (!cfg.address) {
      throw new BadRequestException('Scale address (ip:port) is required');
    }
  }
}
