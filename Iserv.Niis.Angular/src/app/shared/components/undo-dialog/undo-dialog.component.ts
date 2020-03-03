import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-undo-dialog',
  templateUrl: './undo-dialog.component.html',
  styleUrls: ['./undo-dialog.component.scss']
})
export class UndoDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<UndoDialogComponent>) { }

  ngOnInit() {
  }

}
