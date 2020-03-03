import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-confirm-form-request-convention-dialog',
  templateUrl: './confirm-form-request-convention-dialog.component.html',
  styleUrls: ['./confirm-form-request-convention-dialog.component.scss']
})
export class ConfirmFormRequestConventionDialogComponent implements OnInit {
  constructor(
    public dialogRef: MatDialogRef<ConfirmFormRequestConventionDialogComponent>
  ) { }

  ngOnInit() {
  }
  onOk(): void {
    this.dialogRef.close(true);
  }
  onCancel(): void {
    this.dialogRef.close(false);
  }
}
