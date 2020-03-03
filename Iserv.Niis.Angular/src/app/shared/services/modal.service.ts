import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { MatDialogRef, MatDialog, MatDialogConfig } from '@angular/material';
import { OkDialogComponent } from '../components/ok-dialog/ok-dialog.component';
import { ModalButton } from './models/modal-button.enum';
import { YesNoCancelDialogComponent } from '../components/yes-no-cancel-dialog/yes-no-cancel-dialog.component';

@Injectable()
export class ModalService {

  constructor(
    private dialog: MatDialog) {
  }

  public ok(title: string, message: string): Observable<ModalButton> {
    let dialogRef: MatDialogRef<OkDialogComponent>;

    const config = new MatDialogConfig();
    config.disableClose = true;

    dialogRef = this.dialog.open(OkDialogComponent, config);
    dialogRef.componentInstance.title = title;
    dialogRef.componentInstance.message = message;

    return dialogRef.afterClosed();
  }а

  public yesNoCancel(title: string, message: string): Observable<ModalButton> {
    let dialogRef: MatDialogRef<YesNoCancelDialogComponent>;

    const config = new MatDialogConfig();
    config.data = {
      title,
      message,
      disableClose: true
    };

    dialogRef = this.dialog.open(YesNoCancelDialogComponent, config);

    return dialogRef.afterClosed();
  }
}
