import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-confirm-form-icgs-dialog',
  templateUrl: './confirm-form-icgs-dialog.component.html',
  styleUrls: ['./confirm-form-icgs-dialog.component.scss']
})
export class ConfirmFormIcgsDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<ConfirmFormIcgsDialogComponent>
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
