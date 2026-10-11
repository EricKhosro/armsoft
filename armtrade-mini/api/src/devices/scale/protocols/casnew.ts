import { ScalePlu, ScaleExportResult } from '../types';

/**
 * CASNew export, ported from ArmSoft.SME.Trade.UI ElScaleExportCASNewHandler.
 * This family has NO network connector in the original — it writes an XML file
 * that CAS's own PC software then imports. So we produce the same XML content.
 */
export function buildCasNewXml(plus: ScalePlu[]): string {
  const esc = (s: string) =>
    String(s).replace(/[<>&'"]/g, (c) =>
      ({ '<': '&lt;', '>': '&gt;', '&': '&amp;', "'": '&apos;', '"': '&quot;' })[c]!,
    );
  const rows = plus
    .map(
      (p) =>
        `  <Report>` +
        `<CodeSort>${p.barcode}</CodeSort>` +
        `<Code>${p.plu}</Code>` +
        `<GoodName>${esc(p.name.trim())}</GoodName>` +
        `<PriceOut2>${p.price}</PriceOut2>` +
        `<IsPiece>${p.isWeight ? 0 : 1}</IsPiece>` +
        `</Report>`,
    )
    .join('\n');
  return `<?xml version="1.0" encoding="utf-8"?>\n<NewDataSet>\n${rows}\n</NewDataSet>`;
}

export function exportFile(plus: ScalePlu[]): ScaleExportResult {
  return {
    model: 'casnew',
    verified: true,
    total: plus.length,
    exported: plus.length,
    failed: 0,
    lines: plus.map((p) => ({ plu: p.plu, name: p.name, exported: true })),
    fileContent: buildCasNewXml(plus),
    fileName: 'casnew-plu.xml',
    note: 'CASNew is a file import: save this XML and import it with the CAS PC software.',
  };
}
