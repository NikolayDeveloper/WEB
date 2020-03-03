import { Injectable } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material';
import { ConfigService } from 'app/core';

@Injectable()
export class SnackBarHelper {
  constructor(
    private configService: ConfigService,
    private snackBar: MatSnackBar
  ) {}

  success(message: string = 'Data from the server successfully loaded', config: MatSnackBarConfig = {}) {
    const _config = Object.assign(this.configService.snackConfig, { panelClass: ['snack-info'] }, config);
    this.snackBar.open(message, '', _config);
  }

  error(message: string = 'Error getting data from the server', config: MatSnackBarConfig = {}) {
    const _config = Object.assign(this.configService.snackConfig, { panelClass: ['snack-warn'] }, config);
    this.snackBar.open(message, '', _config);
  }
}
