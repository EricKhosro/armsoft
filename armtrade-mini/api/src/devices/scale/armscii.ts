/**
 * CAS/DIGI device codepage (ARMSCII-8 style), ported verbatim from
 * ArmSoft.ElScaleConnector Cas.cs `_armUTF` + `ReCodeName`.
 *
 * Rule: ASCII (<128) passes through; Armenian U+0531–U+0587 maps via the table;
 * Cyrillic U+0410–U+044F → byte(code-848); the ligature և (U+0587) emits byte
 * 187 and then continues as if 'ւ' followed. Unknown chars are truncated to a
 * byte. Output is zero-padded/truncated to `length` bytes.
 */

const ARM_UTF: Record<number, number> = {
  1329: 178, 1330: 180, 1331: 182, 1332: 184, 1333: 186, 1334: 188, 1335: 190,
  1336: 192, 1337: 194, 1338: 196, 1339: 198, 1340: 200, 1341: 202, 1342: 204,
  1343: 206, 1344: 208, 1345: 210, 1346: 212, 1347: 214, 1348: 216, 1349: 218,
  1350: 220, 1351: 222, 1352: 224, 1353: 226, 1354: 228, 1355: 230, 1356: 232,
  1357: 234, 1358: 236, 1359: 238, 1360: 240, 1361: 242, 1362: 244, 1363: 246,
  1364: 248, 1365: 250, 1366: 252,
  1377: 179, 1378: 181, 1379: 183, 1380: 185, 1381: 187, 1382: 189, 1383: 191,
  1384: 193, 1385: 195, 1386: 197, 1387: 199, 1388: 201, 1389: 203, 1390: 205,
  1391: 207, 1392: 209, 1393: 211, 1394: 213, 1395: 215, 1396: 217, 1397: 219,
  1398: 221, 1399: 223, 1400: 225, 1401: 227, 1402: 229, 1403: 231, 1404: 233,
  1405: 235, 1406: 237, 1407: 239, 1408: 241, 1409: 243, 1410: 245, 1411: 247,
  1412: 249, 1413: 251, 1414: 253, 1415: 187,
};

/** Recode a name into a fixed-length byte field (zero padded/truncated). */
export function reCodeName(name: string | null | undefined, length: number): Buffer {
  const out = Buffer.alloc(length);
  if (!name) return out;
  // Expand the և ligature first (it emits byte 187 then behaves like 'ւ').
  let s = name;
  for (let i = 0; i < s.length && i < length; i++) {
    const code = s.charCodeAt(i);
    if (code < 128) {
      out[i] = code & 0xff;
    } else if (ARM_UTF[code] !== undefined) {
      out[i] = ARM_UTF[code];
      if (code === 1415) s = s.slice(0, i + 1) + 'ւ' + s.slice(i + 1);
    } else if (code >= 1040 && code <= 1103) {
      out[i] = (code - 848) & 0xff; // Cyrillic
    } else {
      out[i] = code & 0xff;
    }
  }
  return out;
}
