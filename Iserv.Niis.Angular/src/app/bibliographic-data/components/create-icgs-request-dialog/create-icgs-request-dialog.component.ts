import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { BaseDictionary } from 'app/shared/services/models/base-dictionary';


@Component({
  selector: 'app-create-icgs-request-dialog',
  templateUrl: './create-icgs-request-dialog.component.html',
  styleUrls: ['./create-icgs-request-dialog.component.scss']
})
export class CreateIcgsRequestDialogComponent implements OnInit {
  icgs: BaseDictionary[];
  alreadySelectedIcgs: number[];
  claimedDescription: string;
  claimedDescriptionEn: string;
  selectedIcgs: number;
  constructor(
    public dialogRef: MatDialogRef<CreateIcgsRequestDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit() {
    this.icgs = this.data.icgs;
    this.alreadySelectedIcgs = this.data.selectedIcgs;
    this.selectedIcgs = this.icgs.filter(x => !this.alreadySelectedIcgs.includes(x.id))[0].id;
  }
  onOk(): void {
    const data = {
      claimedDescription: this.claimedDescription,
      claimedDescriptionEn: this.claimedDescriptionEn,
      icgsId: this.selectedIcgs,
    };
    this.dialogRef.close(data);
  }
  onCancel(): void {
    this.dialogRef.close(null);
  }
  isIcgsAlreadySelected(id: number): boolean {
    return this.alreadySelectedIcgs.includes(id);
  }
  isDisabledButtonSave() {
    return !(this.claimedDescription || this.claimedDescriptionEn);
  }
}
