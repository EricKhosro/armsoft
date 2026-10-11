import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { Device } from '../entities/device.entity';
import { DevicesService } from './devices.service';
import { DevicesController } from './devices.controller';
import { FiscalService } from './fiscal/fiscal.service';
import { TerminalService } from './terminal/terminal.service';
import { ScaleService } from './scale/scale.service';

@Module({
  imports: [TypeOrmModule.forFeature([Device])],
  providers: [DevicesService, FiscalService, TerminalService, ScaleService],
  controllers: [DevicesController],
  exports: [DevicesService, FiscalService, TerminalService, ScaleService],
})
export class DevicesModule {}
