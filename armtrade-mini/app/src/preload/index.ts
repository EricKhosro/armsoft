import { contextBridge } from 'electron';

/**
 * The renderer talks to the NestJS API directly over HTTP (CORS is enabled on
 * the server), so the preload only exposes a little static info. Keeping Node
 * out of the renderer is deliberate.
 */
contextBridge.exposeInMainWorld('appInfo', {
  name: 'ArmTrade Mini',
  apiBase: 'http://127.0.0.1:3917/api',
});
