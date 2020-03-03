import { Injectable } from '@angular/core';
import { MatSnackBarConfig } from '@angular/material';

export class ConfigService {
  apiUrl = '';
  pageSize = 50;
  pageSizeOptions = [10, 15, 20, 25, 50, 75, 100];
  httpCacheDuration: 600000;

  snackConfig = new MatSnackBarConfig();
  debounceTime = 500;

  constructor() {
    this.snackConfig.duration = 5000;
  }
}
