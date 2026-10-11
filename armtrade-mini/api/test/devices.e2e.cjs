/* End-to-end device tests: each connector against its simulator. No hardware. */
const assert = require('assert');
const path = require('path');
const D = (p) => require(path.join(__dirname, '..', 'dist', 'devices', p));

const { startHdmSimulator } = D('simulators/hdm-sim.js');
const { FiscalService } = D('fiscal/fiscal.service.js');
const { startScaleSimulator } = D('simulators/scale-sim.js');
const cas = D('scale/protocols/cas.js');
const tma = D('scale/protocols/tma.js');
const { startTerminalSimulator } = D('simulators/terminal-sim.js');
const { TerminalService } = D('terminal/terminal.service.js');

const delay = (ms) => new Promise((r) => setTimeout(r, ms));
let passed = 0;
const ok = (msg) => {
  passed++;
  console.log('  ✓', msg);
};

async function testFiscal() {
  console.log('HDM fiscal:');
  const server = await startHdmSimulator(3940, { password: '1234', cashierId: 3, pin: '0000' });
  try {
    const svc = new FiscalService();
    const cfg = { host: '127.0.0.1', port: 3940, password: '1234', cashierId: 3, pin: '0000' };
    const res = await svc.printReceipt(cfg, {
      items: [
        { productName: 'Milk 1L', productCode: 'A001', qty: 2, price: 450 },
        { productName: 'Bread', qty: 1, price: 250 },
      ],
      paidAmount: 1200,
    });
    assert.strictEqual(res.total, 1150, 'fiscal total');
    assert.strictEqual(res.change, 50, 'fiscal change');
    assert.ok(res.crn && res.fiscal && res.qr, 'fiscal ids present');
    ok(`printed receipt #${res.rseq}, fiscal ${res.fiscal}, total ${res.total}, change ${res.change}`);

    // Wrong password must be rejected (status 101).
    let threw = false;
    try {
      await svc.printReceipt({ ...cfg, password: 'wrong' }, {
        items: [{ productName: 'X', qty: 1, price: 1 }],
      });
    } catch (e) {
      threw = true;
      assert.ok(/101|password/i.test(e.message), 'login error surfaced');
    }
    assert.ok(threw, 'wrong password rejected');
    ok('wrong password rejected with login error');

    await svc.report(cfg, 2); // Z report
    ok('Z report accepted');
    await svc.cashInOut(cfg, 5000, true, 'float');
    ok('cash-in accepted');
  } finally {
    server.close();
  }
}

async function testCas() {
  console.log('CAS label scale:');
  // checksum sanity on the built record
  const rec = cas.buildCasRecord({
    plu: 42, barcode: 20042, prefix: 22, name: 'Կաթ 1Լ', price: 450,
    isWeight: true, labelN: 1, barCodeN: 1,
  });
  let sum = 0;
  for (let i = 2; i < rec.length - 2; i++) sum += rec[i];
  assert.strictEqual(rec[rec.length - 2], sum & 0xff, 'CAS checksum');
  assert.strictEqual(rec[rec.length - 1], 13, 'CAS trailing CR');
  assert.strictEqual(rec.readUInt32LE(12), 42, 'CAS PLU field');
  ok('record checksum + PLU field correct (Armenian name encoded)');

  const server = await startScaleSimulator(3941, 'cas');
  try {
    const res = await cas.exportPlus('127.0.0.1:3941', [
      { plu: 42, barcode: 20042, prefix: 22, name: 'Կաթ 1Լ', price: 450, isWeight: true, labelN: 1, barCodeN: 1 },
      { plu: 43, barcode: 20043, prefix: 22, name: 'Hats', price: 250, isWeight: false, labelN: 1, barCodeN: 1 },
    ], { eraseFirst: true });
    assert.strictEqual(res.exported, 2, 'CAS exported count');
    assert.strictEqual(res.failed, 0, 'CAS failed count');
    assert.deepStrictEqual(server.records.map((r) => r.plu).sort(), [42, 43], 'CAS received PLUs');
    ok(`exported ${res.exported} PLUs, scale received ${server.records.length}`);
  } finally {
    server.close();
  }
}

async function testTma() {
  console.log('TM-A label scale:');
  const server = await startScaleSimulator(3942, 'tma');
  try {
    const res = await tma.exportPlus('127.0.0.1:3942', [
      { plu: 7, barcode: 10007, prefix: 23, name: 'Apple', price: 390, isWeight: true, labelN: 2, barCodeN: 3 },
    ]);
    assert.strictEqual(res.exported, 1, 'TM-A exported count');
    await delay(150); // fire-and-forget: let the sim drain
    assert.strictEqual(server.records.length, 1, 'TM-A received');
    assert.ok(server.records[0].raw.includes('39000,2'), 'TM-A price ×100 with ,2');
    assert.strictEqual(server.records[0].plu, 7, 'TM-A PLU');
    ok('PLU line received with price "39000,2" (=390.00)');
  } finally {
    server.close();
  }
}

async function testTerminal() {
  console.log('PossoftPay terminal:');
  const server = await startTerminalSimulator(3943);
  try {
    const svc = new TerminalService();
    const sale = await svc.sale('127.0.0.1:3943', 12.34);
    assert.strictEqual(sale.approved, true, 'sale approved');
    assert.ok(sale.rrn, 'sale rrn present');
    assert.ok(sale.cardholderReceipt, 'cardholder receipt present');
    ok(`sale approved, rrn ${sale.rrn} (wait-frame handled)`);

    const refund = await svc.refund('127.0.0.1:3943', 5, sale.rrn);
    assert.strictEqual(refund.approved, true, 'refund approved');
    ok('refund approved');

    const declined = await svc.sale('127.0.0.1:3943', 6.66); // amount 666 → decline
    assert.strictEqual(declined.approved, false, 'decline path');
    assert.strictEqual(declined.responseCode, '05', 'decline code');
    ok('decline path returns approved=false (code 05)');

    const ping = await svc.ping('127.0.0.1:3943');
    assert.strictEqual(ping.approved, true, 'ping ok');
    ok('ping ok');
  } finally {
    server.close();
  }
}

(async () => {
  try {
    await testFiscal();
    await testCas();
    await testTma();
    await testTerminal();
    console.log(`\nAll device tests passed (${passed} assertions).`);
    process.exit(0);
  } catch (e) {
    console.error('\nTEST FAILED:', e && e.stack ? e.stack : e);
    process.exit(1);
  }
})();
