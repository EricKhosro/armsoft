import * as crypto from 'crypto';

/**
 * HDM (Armenian fiscal register) wire protocol, ported from ArmSoft.ECRConnector.
 *
 * Request frame (12-byte header + body):
 *   [0..5]  magic  D5 80 D4 B4 D5 84  ("ՀԴՄ")
 *   [6..7]  version 00 07
 *   [8]     op code
 *   [9]     reserved 0x00
 *   [10..11] body length, uint16 big-endian
 *   [12..]  3DES-ECB(PKCS7) of UTF-8 JSON
 *
 * Response frame:
 *   [0..4]  opaque header bytes
 *   [5..6]  status, uint16 big-endian (200 = OK)
 *   [7..8]  body length, uint16 big-endian
 *   [9..10] opaque trailing bytes
 *   [11..]  3DES-ECB(PKCS7) of UTF-8 JSON (only when length > 0)
 *
 * Fixes over the original: reads loop until the full frame arrives (the C#
 * code assumed one TCP chunk), and nothing is ever written to a plaintext log.
 */

export const REQUEST_HEADER = Buffer.from([213, 128, 212, 180, 213, 132, 0, 7]);
export const RESPONSE_MAGIC = Buffer.from([213, 128, 212, 180, 213, 132]);

export const OP = {
  LIST_OPS_AND_DEPS: 1,
  OPS_LOGIN: 2,
  OPS_LOGOUT: 3,
  PRINT_RECEIPT: 4,
  PRINT_LAST_RECEIPT: 5,
  PRINT_RETURN_RECEIPT: 6,
  PRINT_FISCAL_REPORT: 9,
  GET_RECEIPT: 10,
  PRINT_CASH_IN_OUT: 11,
  GET_DATE_AND_TIME: 12,
  SYNC: 14,
  GET_PAYMENT_SYSTEMS: 15,
  CHECK_EMARK: 16,
} as const;

export const STATUS_OK = 200;

export class HdmError extends Error {
  constructor(
    public readonly code: number,
    message?: string,
  ) {
    super(message || `HDM error ${code}`);
    this.name = 'HdmError';
  }
}

// Subset of the device's status codes (English). Unknown codes fall back to the number.
const ERROR_TEXT: Record<number, string> = {
  400: 'Bad request',
  402: 'Unsupported protocol version',
  403: 'Unauthorized',
  404: 'Unknown operation code',
  500: 'Device internal error',
  501: 'Tax (SRC) server error',
  502: 'No internet connection on the device',
  101: 'Wrong password or encoding',
  102: 'Session error',
  103: 'Header error',
  104: 'Receipt sequence error',
  105: 'JSON format error',
  111: 'Wrong user',
  112: 'User not found',
  113: 'User is inactive',
  121: 'Login error',
  141: 'No last receipt',
  143: 'Printer error',
  152: 'Paid amount is less than the total',
  155: 'Device needs synchronization',
  157: 'Invalid return ticket id',
  162: 'Invalid product name',
  191: 'Cash in/out amount error',
  193: 'Invalid partner TIN',
  195: 'Invalid e-mark format',
};

export function errorText(code: number): string {
  return ERROR_TEXT[code] ? `${ERROR_TEXT[code]} (${code})` : `HDM error ${code}`;
}

/** passwordKey = first 24 bytes of SHA-256(password); used for login & listing. */
export function derivePasswordKey(password: string): Buffer {
  return crypto.createHash('sha256').update(password, 'utf8').digest().subarray(0, 24);
}

export function encrypt(key: Buffer, json: string): Buffer {
  const cipher = crypto.createCipheriv('des-ede3', key, null); // 3DES-ECB, no IV
  cipher.setAutoPadding(true); // PKCS7
  return Buffer.concat([cipher.update(Buffer.from(json, 'utf8')), cipher.final()]);
}

export function decrypt(key: Buffer, body: Buffer): string {
  const decipher = crypto.createDecipheriv('des-ede3', key, null);
  decipher.setAutoPadding(true);
  return Buffer.concat([decipher.update(body), decipher.final()]).toString('utf8');
}

/** Build a full request packet for an op, given its (already-chosen) key. */
export function buildRequest(op: number, key: Buffer, payload: object): Buffer {
  const json = JSON.stringify(payload);
  const body = encrypt(key, json);
  const head = Buffer.concat([REQUEST_HEADER, Buffer.from([op, 0x00])]);
  const len = Buffer.alloc(2);
  len.writeUInt16BE(body.length, 0);
  return Buffer.concat([head, len, body]);
}

export interface ParsedResponse {
  status: number;
  body: Buffer; // still ciphered; empty if none
}

/**
 * Try to parse a response from an accumulated buffer. Returns null if more
 * bytes are needed, otherwise the status and (ciphered) body plus how many
 * bytes were consumed.
 */
export function tryParseResponse(
  buf: Buffer,
): { status: number; body: Buffer; consumed: number } | null {
  if (buf.length < 11) return null;
  const status = buf.readUInt16BE(5);
  const bodyLen = buf.readUInt16BE(7);
  if (status !== STATUS_OK) {
    // The device sends no body on error.
    return { status, body: Buffer.alloc(0), consumed: 11 };
  }
  if (buf.length < 11 + bodyLen) return null;
  return { status, body: buf.subarray(11, 11 + bodyLen), consumed: 11 + bodyLen };
}
