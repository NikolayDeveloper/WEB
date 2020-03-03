import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-data-input-form-dialog',
  templateUrl: './data-input-form-dialog.component.html',
  styleUrls: ['./data-input-form-dialog.component.scss']
})
export class DataInputFormDialogComponent implements OnInit {
  text: string;
  constructor(
    public dialogRef: MatDialogRef<DataInputFormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit() {
    if (this.data && this.data.text) {
      this.text = this.data.text;
    }
  }
  onOk(): void {
    this.dialogRef.close(this.text);
  }
  onCancel(): void {
    this.dialogRef.close(null);
  }
}
