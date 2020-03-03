import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-confirm-dialog',
  template: `<h1 mat-dialog-title i18n="@@warning">Warning!</h1>
    <div mat-dialog-content i18n="@@dialogDeleteDocText">Delete document! Are you sure?</div>
    <div mat-dialog-actions align="end">
      <button type="button" mat-button mat-dialog-close="false" color="warn" i18n="@@dialogCancel">Cancel</button>
      <button type="button" mat-button mat-dialog-close="true" color="primary" i18n="@@dialogOk">Ok</button>
  </div>`,
})
export class DeleteDialogComponent {
  constructor(public dialogRef: MatDialogRef<DeleteDialogComponent>) {}
}
