import * as net from 'net';
import * as crypto from 'crypto';
import { OP, decrypt, derivePasswordKey, encrypt } from '../fiscal/hdm-protocol';

/**
 * A minimal HDM fiscal-register simulator that speaks the same wire protocol as
 * the real device, so the connector can be exercised without hardware.
 * It is NOT a certified fiscal device and performs no real fiscal recording.
 */
export interface HdmSimOptions {
  password?: string;
  cashierId?: number;
  pin?: string;
}

export function startHdmSimulator(
  port: number,
  opts: HdmSimOptions = {},
): Promise<net.Server> {
  const password = opts.password ?? '1234';
  const cashierId = opts.cashierId ?? 3;
  const pin = opts.pin ?? '0000';
  const passwordKey = derivePasswordKey(password);

  let receiptSeq = 100;

  const server = net.createServer((socket) => {
    const sessionKey = crypto.randomBytes(24); // handed to the client at login
    let loggedIn = false;
    let buf = Buffer.alloc(0);

    socket.on('data', (chunk) => {
      buf = Buffer.concat([buf, chunk]);
      // Request frame: [8 header][op][0][len BE(2)][body]
      while (buf.length >= 12) {
        const len = buf.readUInt16BE(10);
        if (buf.length < 12 + len) break;
        const op = buf[8];
        const body = buf.subarray(12, 12 + len);
        buf = buf.subarray(12 + len);
        handleOp(op, body);
      }
    });

    function handleOp(op: number, body: Buffer) {
      const key = op === OP.OPS_LOGIN || op === OP.LIST_OPS_AND_DEPS ? passwordKey : sessionKey;
      let req: any = {};
      try {
        req = JSON.parse(decrypt(key, body));
      } catch {
        return respondError(101);
      }

      switch (op) {
        case OP.OPS_LOGIN: {
          if (String(req.password) !== password || String(req.pin) !== pin) {
            return respondError(101);
          }
          if (Number(req.cashier) !== cashierId) return respondError(111);
          loggedIn = true;
          return respond(key, { responseCode: 0, key: sessionKey.toString('base64') });
        }
        case OP.OPS_LOGOUT:
          loggedIn = false;
          return respond(sessionKey, {});
        case OP.PRINT_RECEIPT:
        case OP.PRINT_RETURN_RECEIPT: {
          if (!loggedIn) return respondError(102);
          const total =
            (req.items || []).reduce(
              (s: number, i: any) => s + Number(i.qty) * Number(i.price),
              0,
            ) || 0;
          const paid = Number(req.paidAmount || 0) + Number(req.paidAmountCard || 0);
          return respond(sessionKey, {
            responseCode: 0,
            rseq: ++receiptSeq,
            crn: String(200000000 + receiptSeq),
            sn: 'SIM00012345',
            tin: '01234567',
            taxpayer: 'Simulated Taxpayer LLC',
            address: 'Yerevan, Simulation st. 1',
            time: Date.now(),
            fiscal: String(300000000000 + receiptSeq),
            lottery: '12345678',
            prize: 0,
            total,
            change: Math.max(0, paid - total),
            qr: `https://sim.hdm/receipt/${receiptSeq}`,
            emarksCount: '0',
            verificationNumber: '000000',
          });
        }
        case OP.PRINT_FISCAL_REPORT:
        case OP.PRINT_CASH_IN_OUT:
        case OP.SYNC:
          if (!loggedIn) return respondError(102);
          return respond(sessionKey, { responseCode: 0 });
        case OP.GET_DATE_AND_TIME:
          return respond(sessionKey, { responseCode: 0, dateTime: Date.now() });
        default:
          return respondError(404);
      }
    }

    function respond(key: Buffer, payload: object) {
      const body = encrypt(key, JSON.stringify(payload));
      socket.write(frame(200, body));
    }
    function respondError(status: number) {
      socket.write(frame(status, Buffer.alloc(0)));
    }
  });

  return new Promise((resolve) => server.listen(port, '127.0.0.1', () => resolve(server)));
}

/** Response frame: [5 opaque][status BE(2)][len BE(2)][2 opaque][body]. */
function frame(status: number, body: Buffer): Buffer {
  const head = Buffer.alloc(11);
  Buffer.from([213, 128, 212, 180, 213]).copy(head, 0);
  head.writeUInt16BE(status, 5);
  head.writeUInt16BE(body.length, 7);
  return Buffer.concat([head, body]);
}

// Allow running standalone: `node dist/devices/simulators/hdm-sim.js [port]`
if (require.main === module) {
  const port = Number(process.argv[2]) || 3930;
  startHdmSimulator(port).then(() =>
    // eslint-disable-next-line no-console
    console.log(`[hdm-sim] listening on 127.0.0.1:${port} (password 1234, cashier 3, pin 0000)`),
  );
}
