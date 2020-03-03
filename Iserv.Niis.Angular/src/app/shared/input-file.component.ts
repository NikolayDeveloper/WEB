import { Component, ElementRef, EventEmitter, Input, Output, ViewChild } from '@angular/core';

/**
 * https://github.com/angular/material2/issues/2315
 *
 * @export
 * @class InputFileComponent
 * @implements {OnInit}
 */
@Component({
  selector: 'input-file',
  template: `
  <span>
    <input [accept]="accept" type="file" (change)="onNativeInputFileSelect($event)" #inputFile hidden />
    <button type="button" mat-raised-button (click)="selectFile()">
      <mat-icon>file_upload</mat-icon>
      <ng-content *ngIf="!fileCount" select=".nofiles"></ng-content>
      <span *ngIf="fileCount">
        <span>{{fileCount}}</span>
        <ng-content select=".span-selected"></ng-content>
      </span>
    </button>
  </span>
  `,
  styles: []
})
export class InputFileComponent {
  @Input() accept: string;
  @Output() onFileSelect: EventEmitter<File[]> = new EventEmitter();

  @ViewChild('inputFile') nativeInputFile: ElementRef;

  private _files: File[];

  get fileCount(): number { return this._files && this._files.length || 0; }

  onNativeInputFileSelect($event) {
    this._files = $event.srcElement.files;
    this.onFileSelect.emit(this._files);
  }

  selectFile() {
    this.nativeInputFile.nativeElement.click();
  }

}
