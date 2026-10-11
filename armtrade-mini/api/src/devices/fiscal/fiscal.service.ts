import { BadRequestException, Injectable, Logger } from '@nestjs/common';
import * as net from 'net';
import {
  OP,
  HdmError,
  STATUS_OK,
  buildRequest,
  decrypt,
  derivePasswordKey,
  errorText,
  tryParseResponse,
} from './hdm-protocol';

export interface FiscalConfig {
  host: string;
  port: number;
  password: string;
  cashierId: number;
  pin: string;
  /** Default department/section code for receipt lines. */
  dep?: number;
}

export interface FiscalItem {
  productName: string;
  productCode?: string;
  qty: number;
  price: number;
  dep?: number;
  adgCode?: string;
  unit?: string;
  discount?: number;
  discountType?: number;
}

export interface FiscalReceiptInput {
  items: FiscalItem[];
  paidAmount?: number;
  paidAmountCard?: number;
  partnerTin?: string;
}

export interface FiscalReceiptResult {
  rseq: number;
  crn: string;
  sn: string;
  tin: string;
  taxpayer: string;
  fiscal: string;
  total: number;
  change: number;
  qr: string;
  raw: Record<string, any>;
}

const CONNECT_TIMEOUT = 15_000;
const IO_TIMEOUT = 180_000; // matches the device's 180s read/write timeout

/**
 * High-level HDM fiscal client: opens a TCP session, logs the cashier in,
 * performs one operation, logs out. Login/listing use a key derived from the
 * device password; all other operations use the Base64 session key returned by
 * login.
 */
@Injectable()
export class FiscalService {
  private readonly log = new Logger('FiscalService');

  /** Connect, log in, list operators — a lightweight connectivity/credentials test. */
  async test(cfg: FiscalConfig): Promise<{ ok: true; cashierCount?: number }> {
    const session = await this.open(cfg);
    try {
      return { ok: true };
    } finally {
      await session.close();
    }
  }

  async printReceipt(
    cfg: FiscalConfig,
    receipt: FiscalReceiptInput,
  ): Promise<FiscalReceiptResult> {
    if (!receipt.items?.length) {
      throw new BadRequestException('Fiscal receipt has no items');
    }
    const session = await this.open(cfg);
    try {
      const payload = {
        seq: session.nextSeq(),
        paidAmount: round2(receipt.paidAmount ?? 0),
        paidAmountCard: round2(receipt.paidAmountCard ?? 0),
        mode: 2, // Product receipt
        useExtPOS: false,
        ...(receipt.partnerTin ? { partnerTin: receipt.partnerTin } : {}),
        items: receipt.items.map((i) => ({
          dep: i.dep ?? cfg.dep ?? 1,
          qty: i.qty,
          price: i.price,
          ...(i.productCode ? { productCode: i.productCode } : {}),
          productName: String(i.productName).slice(0, 50),
          ...(i.adgCode ? { adgCode: i.adgCode } : {}),
          ...(i.unit ? { unit: i.unit } : {}),
          ...(i.discount ? { discount: i.discount } : {}),
          ...(i.discountType ? { discountType: i.discountType } : {}),
        })),
      };
      const res = await session.op(OP.PRINT_RECEIPT, payload);
      return {
        rseq: res.rseq,
        crn: res.crn,
        sn: res.sn,
        tin: res.tin,
        taxpayer: res.taxpayer,
        fiscal: res.fiscal,
        total: res.total,
        change: res.change,
        qr: res.qr,
        raw: res,
      };
    } finally {
      await session.close();
    }
  }

  /** X (1) or Z (2) fiscal report. */
  async report(cfg: FiscalConfig, type: 1 | 2): Promise<Record<string, any>> {
    const session = await this.open(cfg);
    try {
      return await session.op(OP.PRINT_FISCAL_REPORT, {
        seq: session.nextSeq(),
        reportType: type,
      });
    } finally {
      await session.close();
    }
  }

  async cashInOut(
    cfg: FiscalConfig,
    amount: number,
    isCashIn: boolean,
    description = '',
  ): Promise<Record<string, any>> {
    const session = await this.open(cfg);
    try {
      return await session.op(OP.PRINT_CASH_IN_OUT, {
        seq: session.nextSeq(),
        amount: round2(amount),
        isCashIn,
        cashierId: cfg.cashierId,
        description,
      });
    } finally {
      await session.close();
    }
  }

  // ---- session management ------------------------------------------------
  private async open(cfg: FiscalConfig): Promise<HdmSession> {
    if (!cfg?.host || !cfg?.port) {
      throw new BadRequestException('Fiscal device host and port are required');
    }
    const session = new HdmSession(cfg, this.log);
    await session.connect();
    await session.login();
    return session;
  }
}

/** One TCP conversation with the device. */
class HdmSession {
  private socket!: net.Socket;
  private passwordKey: Buffer;
  private sessionKey: Buffer | null = null;
  private seq = 0;

  constructor(
    private readonly cfg: FiscalConfig,
    private readonly log: Logger,
  ) {
    this.passwordKey = derivePasswordKey(cfg.password);
  }

  nextSeq(): number {
    return ++this.seq;
  }

  connect(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.socket = new net.Socket();
      this.socket.setTimeout(CONNECT_TIMEOUT);
      this.socket.once('timeout', () => {
        this.socket.destroy();
        reject(new Error(`Fiscal device connect timed out (${this.cfg.host}:${this.cfg.port})`));
      });
      this.socket.once('error', (e) =>
        reject(new Error(`Fiscal device connection error: ${e.message}`)),
      );
      this.socket.connect(this.cfg.port, this.cfg.host, () => {
        this.socket.setTimeout(IO_TIMEOUT);
        this.log.log(`connected ${this.cfg.host}:${this.cfg.port}`);
        resolve();
      });
    });
  }

  async login(): Promise<void> {
    const res = await this.op(
      OP.OPS_LOGIN,
      { password: this.cfg.password, cashier: this.cfg.cashierId, pin: this.cfg.pin },
      this.passwordKey,
    );
    if (!res.key) throw new HdmError(121, 'Login returned no session key');
    this.sessionKey = Buffer.from(res.key, 'base64');
  }

  /** Send one operation and return the decrypted response payload. */
  op(op: number, payload: object, keyOverride?: Buffer): Promise<any> {
    const key =
      keyOverride ??
      (op === OP.LIST_OPS_AND_DEPS || op === OP.OPS_LOGIN
        ? this.passwordKey
        : this.sessionKey);
    if (!key) throw new HdmError(102, 'No key for operation (not logged in)');

    const packet = buildRequest(op, key, payload);
    return new Promise((resolve, reject) => {
      let buf = Buffer.alloc(0);
      const onData = (chunk: Buffer) => {
        buf = Buffer.concat([buf, chunk]);
        const parsed = tryParseResponse(buf);
        if (!parsed) return; // need more bytes
        cleanup();
        if (parsed.status !== STATUS_OK) {
          return reject(new HdmError(parsed.status, errorText(parsed.status)));
        }
        if (parsed.body.length === 0) return resolve({});
        try {
          resolve(JSON.parse(decrypt(key, parsed.body)));
        } catch (e: any) {
          reject(new Error(`Could not decrypt/parse response: ${e.message}`));
        }
      };
      const onErr = (e: Error) => {
        cleanup();
        reject(new Error(`Fiscal I/O error: ${e.message}`));
      };
      const onTimeout = () => {
        cleanup();
        this.socket.destroy();
        reject(new Error('Fiscal device response timed out'));
      };
      const cleanup = () => {
        this.socket.off('data', onData);
        this.socket.off('error', onErr);
        this.socket.off('timeout', onTimeout);
      };
      this.socket.on('data', onData);
      this.socket.on('error', onErr);
      this.socket.on('timeout', onTimeout);
      this.socket.write(packet);
    });
  }

  async close(): Promise<void> {
    try {
      if (this.sessionKey) {
        await this.op(OP.OPS_LOGOUT, { seq: this.nextSeq() }).catch(() => undefined);
      }
    } finally {
      this.socket?.end();
      this.socket?.destroy();
    }
  }
}

function round2(n: number): number {
  return Math.round((n + Number.EPSILON) * 100) / 100;
}
