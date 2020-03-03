import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ModalButton } from 'app/shared/services/models/modal-button.enum';

@Component({
  selector: 'app-yes-no-cancel-dialog',
  templateUrl: './yes-no-cancel-dialog.component.html',
  styleUrls: ['./yes-no-cancel-dialog.component.scss']
})
export class YesNoCancelDialogComponent implements OnInit {
  public modalButton = ModalButton;

  constructor(
    public dialogRef: MatDialogRef<YesNoCancelDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  get title(): string {
    return this.data.title;
  }

  get message(): string {
    return this.data.message;
  }

  isAvailable(modalButton: ModalButton): boolean {
    if (this.data.buttons) {
      return this.data.buttons.includes(modalButton);
    } else {
      return true;
    }
  }

  getLabel(modalButton: ModalButton): string | null {
    if (this.data.labels) {
        return this.data.labels[modalButton];
    } else {
        return null;
    }
}

  ngOnInit() {
  }

  onCancelClick() {
    this.dialogRef.close(ModalButton.Cancel);
  }

  onNoClick() {
    this.dialogRef.close(ModalButton.No);
  }

  onYesClick() {
    this.dialogRef.close(ModalButton.Yes);
  }
}
