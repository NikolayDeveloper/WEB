import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-confirm-form-dialog',
  templateUrl: './confirm-form-dialog.component.html',
  styleUrls: ['./confirm-form-dialog.component.scss']
})
export class ConfirmFormDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<ConfirmFormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
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
