import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { ModalButton } from 'app/shared/services/models/modal-button.enum';

@Component({
  selector: 'app-ok-dialog',
  templateUrl: './ok-dialog.component.html',
  styleUrls: ['./ok-dialog.component.scss']
})
export class OkDialogComponent implements OnInit {
  public title: string;
  public message: string;

  constructor(
    public dialogRef: MatDialogRef<OkDialogComponent>
  ) {
  }

  ngOnInit() {
  }

  onOkClick() {
    this.dialogRef.close(ModalButton.Ok);
  }
}
