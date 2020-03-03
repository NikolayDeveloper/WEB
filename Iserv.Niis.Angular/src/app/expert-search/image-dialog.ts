import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-image-dialog',
  template: '<img [src]="data" style="min-height: 60vh; height: 60vh; width: auto; max-width:100%; height: auto; max-height: 100%;">'
})
export class ImageDialog implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<ImageDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit() { }

}