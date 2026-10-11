import * as net from 'net';

/**
 * Label-scale simulators for CAS (binary) and TM-A (tab text). They validate
 * framing and reply the way the real devices do, so the connectors can be
 * tested without hardware. Parsed records are kept on `server.records`.
 */
export interface ScaleSimServer extends net.Server {
  records: Array<{ plu: number; raw?: string }>;
}

export function startScaleSimulator(
  port: number,
  model: 'cas' | 'tma',
): Promise<ScaleSimServer> {
  const server = (
    model === 'cas' ? startCas(port) : startTma(port)
  ) as unknown as ScaleSimServer;
  server.records = (server as any).records || [];
  return new Promise((resolve) => server.listen(port, '127.0.0.1', () => resolve(server)));
}

function startCas(port: number): net.Server {
  const records: Array<{ plu: number }> = [];
  const server: any = net.createServer((socket) => {
    socket.on('data', (data) => {
      if (data[0] === 67) {
        // "C43A02\n" erase → ack 'G'
        socket.write(Buffer.from([71, 0, 0]));
        return;
      }
      if (data[0] === 87 && data[1] === 76) {
        // "WL" item record: PLU is uint32 LE at offset 12.
        const plu = data.length >= 16 ? data.readUInt32LE(12) : 0;
        records.push({ plu });
        // Accept: reply[0]='G'(71), reply[14]=0 (no error).
        const reply = Buffer.alloc(20);
        reply[0] = 71;
        socket.write(reply);
      }
    });
  });
  server.records = records;
  return server;
}

function startTma(port: number): net.Server {
  const records: Array<{ plu: number; raw: string }> = [];
  const server: any = net.createServer((socket) => {
    let text = '';
    socket.on('data', (data) => {
      text += data.toString('utf8');
      let idx: number;
      while ((idx = text.indexOf('\n')) >= 0) {
        const line = text.slice(0, idx).replace(/\r$/, '');
        text = text.slice(idx + 1);
        if (line.startsWith('PLU\t')) {
          const parts = line.split('\t');
          records.push({ plu: Number(parts[1]), raw: line });
        }
      }
    });
    // The list upload is fire-and-forget; no reply needed.
  });
  server.records = records;
  return server;
}

if (require.main === module) {
  const port = Number(process.argv[3]) || 3931;
  const model = (process.argv[2] as 'cas' | 'tma') || 'cas';
  startScaleSimulator(port, model).then(() =>
    // eslint-disable-next-line no-console
    console.log(`[scale-sim:${model}] listening on 127.0.0.1:${port}`),
  );
}
