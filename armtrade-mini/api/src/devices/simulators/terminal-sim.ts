import { WebSocketServer } from 'ws';

/**
 * PossoftPay card-terminal simulator. Speaks the same ws:// JSON protocol: it
 * may send an interim {operation:"wait"} and then the final result echoing the
 * request operation. Approves everything except amount 666 (to exercise the
 * decline path). Not a real payment terminal.
 */
export function startTerminalSimulator(port: number): Promise<WebSocketServer> {
  const wss = new WebSocketServer({ host: '127.0.0.1', port });

  wss.on('connection', (ws) => {
    ws.on('message', (raw) => {
      let req: any;
      try {
        req = JSON.parse(raw.toString());
      } catch {
        return;
      }
      const op = req.operation;
      const financial = ['sale', 'refund', 'cash', 'saleCash'].includes(op);

      // Exercise the client's "wait" handling for financial ops.
      if (financial) ws.send(JSON.stringify({ operation: 'wait', timeout: 5 }));

      const declined = req.amount === 666;
      const base: any = {
        operation: op,
        result: !declined,
        responseCode: declined ? '05' : '00',
        responseText: declined ? 'DECLINED' : 'APPROVED',
        CRN: req.CRN,
        receiptLineLength: 42,
      };
      if (financial && !declined) {
        base.rrn = String(Math.floor(Math.random() * 1e12)).padStart(12, '0');
        base.merchantReceipt = `*** MERCHANT COPY ***\n${op.toUpperCase()} ${(
          req.amount / 100
        ).toFixed(2)} AMD\nAPPROVED`;
        base.cardholderReceipt = `*** CUSTOMER COPY ***\n${op.toUpperCase()} ${(
          req.amount / 100
        ).toFixed(2)} AMD\nAPPROVED`;
      }
      // Small delay so the interim "wait" is observed first.
      setTimeout(() => ws.send(JSON.stringify(base)), financial ? 50 : 0);
    });
  });

  return new Promise((resolve) => wss.on('listening', () => resolve(wss)));
}

if (require.main === module) {
  const port = Number(process.argv[2]) || 3933;
  startTerminalSimulator(port).then(() =>
    // eslint-disable-next-line no-console
    console.log(`[terminal-sim] ws://127.0.0.1:${port} (decline on amount 666)`),
  );
}
