import { Component, Inject, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { saveAs } from 'file-saver';
import { NgxBarcodeComponent } from 'ngx-barcode';

@Component({
  selector: 'app-barcode-dialog',
  templateUrl: './barcode-dialog.component.html',
  styleUrls: ['./barcode-dialog.component.scss']
})
export class BarcodeDialogComponent {
  barcodeNumber: number;
  barcodeCanvas: any;

  @ViewChild('barcode') barcodeComponent: NgxBarcodeComponent;

  constructor(
    private dialogRef: MatDialogRef<BarcodeDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private data: any) {
    this.barcodeNumber = data.barcode;
  }

  onPrint() {
    const barcodeCanvas = this.barcodeComponent.bcElement.nativeElement.firstChild;
    const imgSrc = barcodeCanvas.toDataURL();
    const printWindow = window.open('', 'PrintMap');
    printWindow.document.writeln('<img src="' + imgSrc + '">');

    setTimeout(() => {
      printWindow.document.close();
      printWindow.print();
      printWindow.close();
    }, 200);
  }

  onDownload() {
    const requestId = this.barcodeNumber;
    const barcodeCanvas = this.barcodeComponent.bcElement.nativeElement.firstChild;
    const ctx = barcodeCanvas.getContext('2d');
    barcodeCanvas.toBlob(function (blob) {
      saveAs(blob, `${requestId}.png`);
    });
  }

  onCancel() {
    this.dialogRef.close();
  }
}
