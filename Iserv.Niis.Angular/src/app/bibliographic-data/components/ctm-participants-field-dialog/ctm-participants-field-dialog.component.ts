import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-ctm-participants-field-dialog',
  templateUrl: './ctm-participants-field-dialog.component.html',
  styleUrls: ['./ctm-participants-field-dialog.component.scss']
})
export class CtmParticipantsFieldDialogComponent implements OnInit {
  formGroup: FormGroup;

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<CtmParticipantsFieldDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.buildForm();
  }

  ngOnInit() {}

  onOk(): void {
    const value = this.formGroup.get('colectiveTrademarkParticipantsInfo')
      .value;
    this.dialogRef.close(value);
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      colectiveTrademarkParticipantsInfo: [
        this.data.colectiveTrademarkParticipantsInfo,
        Validators.required
      ]
    });
  }
}
