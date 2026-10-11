import { ScalePlu, ScaleExportResult, ScaleLineResult } from '../types';
import { connect, closeSocket, parseAddress } from '../net-util';

/**
 * TM-A label scale, ported from ArmSoft.ElScaleConnector/TM_A.cs.
 * Tab-delimited UTF-8 lines. A batch is: blank line, "DWL\tPLU\t", one
 * "PLU\t…" line per item (58 fields), then "END\tPLU\t". Price is sent as
 * "<price*100>,2" (the ",2" marks two decimals). Names are truncated to 32.
 * The list upload is fire-and-forget (the device sends no per-item ack).
 */

const CRLF = '\r\n';

function clip(name: string): string {
  return (name ?? '').length > 32 ? name.slice(0, 32) : name ?? '';
}

/** Build the 58-field PLU line (matches the list-upload overload in TM_A.cs). */
export function buildTmaLine(item: ScalePlu): string {
  const price = `${Math.round(item.price * 100)},2`;
  const type = item.isWeight ? '1' : '2';
  const f = [
    'PLU',
    String(item.plu), // 1
    String(item.barcode), // 2
    '0', // 3
    type, // 4
    price, // 5
    '0,00', // 6
    '0,000', // 7
    String(item.labelN), // 8
    String(item.barCodeN), // 9
    String(item.prefix), // 10
    '0', // 11
    '0', // 12
    '0', // 13
    '', // 14
    clip(item.name), // 15
    clip(item.description ?? ''), // 16
    '', '', '', '', '', '', // 17-22
    '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', // 23-34
    '0,000', '0,000', '0', '0', // 35-38
    '0,000', '0,000', '0,00', '0', '0', // 39-43
    '0,000', '0,000', '0,00', '0', '0', // 44-48
    '0,000', '0,000', '0,00', '0', '0', // 49-53
    '0,000', '0,000', '0,00', '0', '0', // 54-58
  ];
  return f.join('\t') + '\t';
}

export async function exportPlus(
  address: string,
  plus: ScalePlu[],
  opts: { eraseFirst?: boolean } = {},
): Promise<ScaleExportResult> {
  const { host, port } = parseAddress(address);
  const socket = await connect(host, port, 10_000);
  const lines: ScaleLineResult[] = [];
  try {
    const chunks: string[] = [];
    if (opts.eraseFirst) {
      chunks.push('', 'CLR\tPLU\t', 'CLR\tEND\t', 'DWL\tPLU\t');
    }
    // Batches of 1000, matching the original.
    for (let i = 0; i < plus.length; i += 1000) {
      chunks.push('', 'DWL\tPLU\t');
      for (const item of plus.slice(i, i + 1000)) {
        chunks.push(buildTmaLine(item));
        lines.push({ plu: item.plu, name: item.name, exported: true });
      }
      chunks.push('END\tPLU\t');
    }
    await new Promise<void>((resolve, reject) => {
      socket.write(chunks.join(CRLF) + CRLF, (err) =>
        err ? reject(err) : resolve(),
      );
    });
  } finally {
    closeSocket(socket);
  }

  return {
    model: 'tma',
    verified: true,
    total: plus.length,
    exported: lines.length,
    failed: 0,
    lines,
    note: 'TM-A list upload is fire-and-forget; the device returns no per-item acknowledgement.',
  };
}
