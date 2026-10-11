import { startHdmSimulator } from './hdm-sim';
import { startScaleSimulator } from './scale-sim';
import { startTerminalSimulator } from './terminal-sim';

/**
 * Starts all device simulators so the app can be demoed without hardware.
 * Defaults match the "Add device" presets on the Devices page:
 *   fiscal (HDM)   127.0.0.1:3930   password 1234, cashier 3, pin 0000
 *   scale  (CAS)   127.0.0.1:3931
 *   terminal       127.0.0.1:3933
 */
async function main() {
  await startHdmSimulator(3930);
  await startScaleSimulator(3931, 'cas');
  await startScaleSimulator(3932, 'tma');
  await startTerminalSimulator(3933);
  // eslint-disable-next-line no-console
  console.log(
    'Device simulators running:\n' +
      '  HDM fiscal   127.0.0.1:3930  (password 1234, cashier 3, pin 0000)\n' +
      '  CAS scale    127.0.0.1:3931\n' +
      '  TM-A scale   127.0.0.1:3932\n' +
      '  card term.   127.0.0.1:3933  (declines amount 6.66)\n' +
      'Press Ctrl+C to stop.',
  );
}

main();
