import * as net from 'net';

/** Parse an "ip:port" string into host/port. */
export function parseAddress(addr: string): { host: string; port: number } {
  const [host, portStr] = String(addr).split(':');
  const port = Number(portStr);
  if (!host || !Number.isFinite(port)) {
    throw new Error(`Invalid device address "${addr}" (expected ip:port)`);
  }
  return { host, port };
}

export function connect(
  host: string,
  port: number,
  timeoutMs = 10_000,
): Promise<net.Socket> {
  return new Promise((resolve, reject) => {
    const socket = new net.Socket();
    socket.setTimeout(timeoutMs);
    const onErr = (e: Error) => {
      socket.destroy();
      reject(new Error(`Connect to ${host}:${port} failed: ${e.message}`));
    };
    socket.once('error', onErr);
    socket.once('timeout', () => onErr(new Error('timeout')));
    socket.connect(port, host, () => {
      socket.off('error', onErr);
      resolve(socket);
    });
  });
}

/** Write a buffer, then resolve with the next inbound chunk (one reply). */
export function writeAndReadOnce(
  socket: net.Socket,
  data: Buffer,
  timeoutMs = 120_000,
): Promise<Buffer> {
  return new Promise((resolve, reject) => {
    const onData = (chunk: Buffer) => {
      cleanup();
      resolve(chunk);
    };
    const onErr = (e: Error) => {
      cleanup();
      reject(e);
    };
    const timer = setTimeout(() => {
      cleanup();
      reject(new Error('Read timed out'));
    }, timeoutMs);
    const cleanup = () => {
      clearTimeout(timer);
      socket.off('data', onData);
      socket.off('error', onErr);
    };
    socket.on('data', onData);
    socket.on('error', onErr);
    socket.write(data);
  });
}

export function closeSocket(socket: net.Socket): void {
  try {
    socket.end();
    socket.destroy();
  } catch {
    /* ignore */
  }
}
