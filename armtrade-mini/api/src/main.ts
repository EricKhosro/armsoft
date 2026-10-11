import 'reflect-metadata';
import { NestFactory } from '@nestjs/core';
import { AppModule } from './app.module';

const PORT = Number(process.env.PORT) || 3917;

async function bootstrap() {
  // Raise the JSON body limit before Nest's default parser is applied, so
  // base64 Excel/XML uploads (a few MB) are accepted.
  const app = await NestFactory.create(AppModule, {
    cors: true,
    bodyParser: false,
  });
  const express = require('express');
  const http = app.getHttpAdapter().getInstance();
  http.use(express.json({ limit: '64mb' }));
  http.use(express.urlencoded({ extended: true, limit: '64mb' }));
  app.setGlobalPrefix('api');
  await app.listen(PORT, '127.0.0.1');
  // eslint-disable-next-line no-console
  console.log(`[api] ArmTrade Mini API listening on http://127.0.0.1:${PORT}/api`);
}

bootstrap();
