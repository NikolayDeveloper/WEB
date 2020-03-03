import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

import { ICGSRequestDto } from '../../models/icgs-request-dto';
import { BaseDictionary } from '../../../shared/services/models/base-dictionary';
import { isStageFormalExam } from '../description/description.component';
import { IntellectualPropertyDetails } from 'app/shared/models/intellectual-property-details';

@Component({
  selector: 'app-icgs-info-form-dialog',
  templateUrl: './icgs-info-form-dialog.component.html',
  styleUrls: ['./icgs-info-form-dialog.component.scss']
})
export class IcgsInfoFormDialogComponent implements OnInit {
  icgsRequest: ICGSRequestDto = this.data.icgsRequest;
  dicICGS: BaseDictionary = this.data.dicICGS;
  isRefused: boolean = this.icgsRequest.isRefused;
  isPartialRefused: boolean = this.icgsRequest.isPartialRefused;
  reasonForPartialRefused: string = this.icgsRequest.reasonForPartialRefused;
  canEdit: boolean = this.data.canEdit;
  constructor(
    public dialogRef: MatDialogRef<IcgsInfoFormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit() {
  }
  onCancel(): void {
    this.dialogRef.close();
  }
  onOk(): void {
    const data = {
      isRefused: this.isRefused,
      isPartialRefused: this.isPartialRefused,
      reasonForPartialRefused: this.reasonForPartialRefused,
    };
    this.dialogRef.close(data);
  }
  changeRefuse() {
    if (this.isRefused) {
      this.isPartialRefused = false;
    }
  }
  changePartialRefused() {
    if (this.isPartialRefused) {
      this.isRefused = false;
    }
  }
}
