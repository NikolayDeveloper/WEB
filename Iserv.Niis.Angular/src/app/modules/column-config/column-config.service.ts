import { ColumnConfig } from './column-config.model';
import { Injectable } from '@angular/core';

@Injectable()
export class ColumnConfigService {

  constructor() { }

  get(key: string, defaultConfig: ColumnConfig[]): ColumnConfig[] {
    return JSON.parse(localStorage.getItem(key)) || defaultConfig;
  }

  save(key: string, columnConfig: ColumnConfig[]): void {
    localStorage.setItem(key, JSON.stringify(columnConfig));
  }
}
