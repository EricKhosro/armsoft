import { BadRequestException, Injectable, Logger } from '@nestjs/common';
import WebSocket from 'ws';

/**
 * PossoftPay card-terminal client, ported from ArmSoft.PossoftPayClient.
 *
 * Protocol (faithful):
 *  - Transport: one `ws://<address>` connection per transaction (address must
 *    include the port, e.g. "192.168.1.50:8080"). Request is a single UTF-8
 *    JSON text frame; the terminal may send interim {operation:"wait",timeout}
 *    messages that reset the inactivity timer; the final message echoes the
 *    request `operation`.
 *  - Amounts are integer minor units: floor(amount * 100). Currency 51 = AMD.
 *  - `CRN` is a client reference: MMddHHmmss + "0000".
 *  - Success is the boolean `result`; on false we surface responseCode/Text.
 *
 * Cleaner than the original: no secrets involved, structured results, explicit
 * connect timeout (the original armed its timer only after the first "wait").
 */

const AMD = 51;
const MIN_TIMEOUT_MS = 10_000;

export type TerminalOp =
  | 'sale'
  | 'refund'
  | 'void'
  | 'cash'
  | 'saleCash'
  | 'settlement'
  | 'getMerchantList'
  | 'getRecord'
  | 'report'
  | 'test'
  | 'ping';

export interface TerminalRequest {
  operation: TerminalOp;
  amount?: number; // minor units
  amountCashback?: number;
  currency?: number;
  CRN?: number;
  functionCode?: number;
  rrn?: string;
}

export interface TerminalResult {
  approved: boolean;
  operation: string;
  responseCode?: string;
  responseText?: string;
  rrn?: string;
  crn?: number;
  merchantReceipt?: string;
  cardholderReceipt?: string;
  headerImageString?: string;
  receiptLineLength?: number;
  raw: Record<string, any>;
}

@Injectable()
export class TerminalService {
  private readonly log = new Logger('TerminalService');

  /** Charge a card. `amount` is a normal decimal (e.g. 1234.56 AMD). */
  sale(address: string, amount: number): Promise<TerminalResult> {
    return this.exec(address, {
      operation: 'sale',
      amount: toMinor(amount),
      currency: AMD,
      CRN: newCRN(),
    });
  }

  refund(address: string, amount: number, rrn: string): Promise<TerminalResult> {
    return this.exec(address, {
      operation: 'refund',
      amount: toMinor(amount),
      currency: AMD,
      CRN: newCRN(),
      rrn,
    });
  }

  /** Cancel the last operation, identified by its CRN. */
  voidLast(address: string, crn: number): Promise<TerminalResult> {
    return this.exec(address, { operation: 'void', CRN: crn });
  }

  settlement(address: string): Promise<TerminalResult> {
    return this.exec(address, { operation: 'settlement', CRN: newCRN() });
  }

  ping(address: string): Promise<TerminalResult> {
    return this.exec(address, { operation: 'ping' });
  }

  test(address: string): Promise<TerminalResult> {
    return this.exec(address, { operation: 'test' });
  }

  // ---- core request/response -------------------------------------------
  private exec(address: string, req: TerminalRequest): Promise<TerminalResult> {
    if (!address) throw new BadRequestException('Terminal address is required');
    const url = address.startsWith('ws://') ? address : `ws://${address}`;
    const payload = serialize(req);

    return new Promise<TerminalResult>((resolve, reject) => {
      const ws = new WebSocket(url);
      let timer: NodeJS.Timeout;
      let settled = false;

      const arm = (ms: number) => {
        clearTimeout(timer);
        timer = setTimeout(() => {
          if (settled) return;
          settled = true;
          try {
            ws.terminate();
          } catch {
            /* ignore */
          }
          reject(new Error(`Terminal timed out after ${ms} ms`));
        }, ms);
      };

      const finish = (fn: () => void) => {
        if (settled) return;
        settled = true;
        clearTimeout(timer);
        try {
          ws.close();
        } catch {
          /* ignore */
        }
        fn();
      };

      arm(MIN_TIMEOUT_MS);

      ws.on('open', () => {
        this.log.log(`${req.operation} → ${url}`);
        ws.send(payload);
      });

      ws.on('message', (data) => {
        let msg: Record<string, any>;
        try {
          msg = JSON.parse(data.toString());
        } catch {
          return finish(() =>
            reject(new Error('Terminal sent a non-JSON message')),
          );
        }
        // Interim keep-alive: reset the timer and keep waiting.
        if (msg.operation === 'wait') {
          const secs = Number(msg.timeout) || 10;
          arm(Math.max(secs * 1000, MIN_TIMEOUT_MS));
          return;
        }
        if (msg.operation !== req.operation) {
          return finish(() =>
            reject(
              new Error(
                `Operation mismatch: sent ${req.operation}, got ${msg.operation}`,
              ),
            ),
          );
        }
        finish(() => resolve(toResult(msg)));
      });

      ws.on('error', (err) =>
        finish(() => reject(new Error(`Terminal connection error: ${err.message}`))),
      );
      ws.on('close', () => {
        if (!settled) {
          settled = true;
          clearTimeout(timer);
          reject(new Error('Terminal closed the connection before responding'));
        }
      });
    });
  }
}

// ---- helpers -------------------------------------------------------------
function toMinor(amount: number): number {
  return Math.floor(Number(amount) * 100);
}

function newCRN(): number {
  const d = new Date();
  const p = (n: number) => String(n).padStart(2, '0');
  const stamp = `${p(d.getMonth() + 1)}${p(d.getDate())}${p(d.getHours())}${p(
    d.getMinutes(),
  )}${p(d.getSeconds())}0000`;
  return Number(stamp);
}

/** Match the C# client: omit zero/empty/undefined fields. */
function serialize(req: TerminalRequest): string {
  const out: Record<string, any> = { operation: req.operation };
  if (req.amount) out.amount = req.amount;
  if (req.amountCashback) out.amountCashback = req.amountCashback;
  if (req.currency) out.currency = req.currency;
  if (req.CRN) out.CRN = req.CRN;
  if (req.functionCode) out.functionCode = req.functionCode;
  if (req.rrn) out.rrn = req.rrn;
  return JSON.stringify(out);
}

function toResult(msg: Record<string, any>): TerminalResult {
  return {
    approved: msg.result === true,
    operation: msg.operation,
    responseCode: msg.responseCode,
    responseText: msg.responseText,
    rrn: msg.rrn,
    crn: msg.CRN,
    merchantReceipt: msg.merchantReceipt,
    cardholderReceipt: msg.cardholderReceipt,
    headerImageString: msg.headerImageString,
    receiptLineLength: msg.receiptLineLength,
    raw: msg,
  };
}
