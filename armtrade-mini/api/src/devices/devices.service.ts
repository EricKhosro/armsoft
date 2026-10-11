import { Injectable, NotFoundException } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository } from 'typeorm';
import { v4 as uuid } from 'uuid';
import { Device, DeviceType } from '../entities/device.entity';

export interface DeviceInput {
  type: DeviceType;
  name: string;
  model?: string | null;
  config?: Record<string, any>;
  active?: boolean;
}

/** CRUD for device configuration rows. Protocol I/O lives in the per-type services. */
@Injectable()
export class DevicesService {
  constructor(
    @InjectRepository(Device)
    private readonly devices: Repository<Device>,
  ) {}

  findAll(type?: DeviceType): Promise<Device[]> {
    return this.devices.find({
      where: type ? { type } : {},
      order: { type: 'ASC', name: 'ASC' },
    });
  }

  async findOne(id: string): Promise<Device> {
    const d = await this.devices.findOne({ where: { id } });
    if (!d) throw new NotFoundException(`Device ${id} not found`);
    return d;
  }

  /** The active device of a type that the POS should use (first active). */
  async active(type: DeviceType): Promise<Device | null> {
    return this.devices.findOne({ where: { type, active: 1 } });
  }

  async create(input: DeviceInput): Promise<Device> {
    const d = this.devices.create({
      id: uuid(),
      type: input.type,
      name: input.name,
      model: input.model ?? null,
      config: input.config ?? {},
      active: input.active === false ? 0 : 1,
    });
    return this.devices.save(d);
  }

  async update(id: string, input: Partial<DeviceInput>): Promise<Device> {
    const d = await this.findOne(id);
    Object.assign(d, {
      name: input.name ?? d.name,
      model: input.model !== undefined ? input.model : d.model,
      config: input.config ?? d.config,
      active:
        input.active === undefined ? d.active : input.active ? 1 : 0,
    });
    return this.devices.save(d);
  }

  async remove(id: string): Promise<void> {
    await this.findOne(id);
    await this.devices.delete(id);
  }
}
