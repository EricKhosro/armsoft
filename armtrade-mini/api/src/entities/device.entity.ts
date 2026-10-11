import {
  Column,
  CreateDateColumn,
  Entity,
  PrimaryColumn,
  UpdateDateColumn,
} from 'typeorm';

export type DeviceType = 'fiscal' | 'scale' | 'terminal';

/**
 * A configured external device. `config` holds type-specific settings as JSON
 * (host, port, model, cashier id, …). Secrets such as the fiscal password are
 * kept here for a local single-user app; see SECURITY notes in the devices
 * module — unlike the original, nothing is ever written to plaintext logs.
 */
@Entity('devices')
export class Device {
  @PrimaryColumn('text')
  id!: string;

  @Column('text')
  type!: DeviceType;

  @Column('text')
  name!: string;

  /** For scales: the protocol family (cas | casnew | digi | tma | mertech | massak). */
  @Column('text', { nullable: true })
  model?: string | null;

  @Column('simple-json', { default: '{}' })
  config!: Record<string, any>;

  @Column('integer', { default: 1 })
  active!: number;

  @CreateDateColumn()
  createdAt!: Date;

  @UpdateDateColumn()
  updatedAt!: Date;
}
