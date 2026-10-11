import { reCodeName } from '../armscii';
import { ScalePlu, ScaleExportResult, ScaleLineResult } from '../types';
import { connect, closeSocket, parseAddress, writeAndReadOnce } from '../net-util';

/**
 * CAS label scale, ported from ArmSoft.ElScaleConnector/Cas.cs.
 * Binary record (little-endian) + trailing checksum (sum of bytes[2..] % 256)
 * + CR. One item per TCP write; reply byte[0] === 'G'(71) means accepted,
 * otherwise reply byte[14] is a CAS error code.
 */

const CAS_ERRORS: Record<number, string> = {
  99: 'Data end',
  98: "Data isn't exist",
  97: 'Data struct fail',
  95: 'Unknown data type',
  89: 'Memory full',
  88: 'Direct message full',
  84: 'Header, tail or CK fail',
  82: 'Number is over',
};

function le16(n: number): Buffer {
  const b = Buffer.alloc(2);
  b.writeUInt16LE(n & 0xffff, 0);
  return b;
}
function le32(n: number): Buffer {
  const b = Buffer.alloc(4);
  b.writeUInt32LE(n >>> 0, 0);
  return b;
}

/** Build the exact CAS item byte array (matches Cas.cs GetByteDataArray). */
export function buildCasRecord(item: ScalePlu): Buffer {
  const head = Buffer.from([87, 76, 0, 0, 0, 0, 44, 147, 0, 58]); // "WL"...
  const msg = item.description?.trim() || null;
  if (msg) head[7] = (msg.length + 147) & 0xff;

  const parts: Buffer[] = [
    head,
    le16(1), // Departament (constant 1)
    le32(item.plu),
    Buffer.from([item.isWeight ? 1 : 2]), // PLUType
    reCodeName(item.name, 40), // Name1
    reCodeName('', 40), // Name2
    reCodeName('', 5), // Name3
    le16(item.prefix), // Group
    le16(item.labelN), // Label
    le16(0), // AuxLabel
    le16(0), // Origin
    Buffer.from([0]), // UnitWeight
    le32(0), // FixedWeight
    le32(item.barcode), // ItemCode
    le16(0), // PCS
    Buffer.from([0]), // PCSSymbol
    Buffer.from([0]), // UseFixedPriceType
    le32(Math.round(item.price * 100)), // UnitPrice (×100)
    le32(0), // SpecialPrice
    le32(0), // TareWeight
    Buffer.from([0]), // TareNumber
    le16(item.barCodeN), // Barcode (format id)
    le16(0), // AuxBarcode
    le16(0), // ProducedDate
    le16(0), // PackedDate
    Buffer.from([0]), // PackedTime
    le32(0), // SellByDate
    Buffer.from([0]), // SellByTime
    le16(0), // MessageNumber
    le16(0), // TraceNumber
    le16(0), // NutritionNumber
    Buffer.from([0]), // SaleMessageNumber
  ];
  if (msg) parts.push(reCodeName(msg, msg.length)); // DirectMessage

  let body = Buffer.concat(parts);
  body = Buffer.concat([body, Buffer.from([58])]); // Last ':'
  // Checksum over bytes from index 2 to end-of-body-so-far.
  let sum = 0;
  for (let i = 2; i < body.length; i++) sum += body[i];
  return Buffer.concat([body, Buffer.from([sum & 0xff, 13])]); // checksum + CR
}

export async function erase(address: string): Promise<void> {
  const { host, port } = parseAddress(address);
  const socket = await connect(host, port, 4000);
  try {
    const reply = await writeAndReadOnce(
      socket,
      Buffer.from([67, 52, 51, 65, 48, 50, 10]), // "C43A02\n"
      120_000,
    );
    const b = reply[0];
    if (!(b === 71 || b === 67)) throw new Error('Scale refused erase');
  } finally {
    closeSocket(socket);
  }
}

export async function exportPlus(
  address: string,
  plus: ScalePlu[],
  opts: { eraseFirst?: boolean } = {},
): Promise<ScaleExportResult> {
  const { host, port } = parseAddress(address);
  if (opts.eraseFirst) await erase(address);

  const socket = await connect(host, port, 10_000);
  const lines: ScaleLineResult[] = [];
  try {
    for (const item of plus) {
      const packet = buildCasRecord(item);
      try {
        const reply = await writeAndReadOnce(socket, packet, 120_000);
        const ok = reply[0] === 71; // 'G'
        lines.push({
          plu: item.plu,
          name: item.name,
          exported: ok,
          error: ok ? undefined : CAS_ERRORS[reply[14]] ?? 'Error inside scale',
        });
      } catch (e: any) {
        lines.push({ plu: item.plu, name: item.name, exported: false, error: e.message });
      }
    }
  } finally {
    closeSocket(socket);
  }

  const exported = lines.filter((l) => l.exported).length;
  return {
    model: 'cas',
    verified: true,
    total: plus.length,
    exported,
    failed: plus.length - exported,
    lines,
  };
}
