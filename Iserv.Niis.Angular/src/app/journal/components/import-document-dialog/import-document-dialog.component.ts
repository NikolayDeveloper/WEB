import { Component, OnInit, Inject, OnDestroy } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { RequestService } from 'app/requests/request.service';
import { Subject } from 'rxjs';
import { Router } from '@angular/router';



@Component({
  selector: 'app-import-document-dialog',
  templateUrl: './import-document-dialog.component.html',
  styleUrls: ['./import-document-dialog.component.scss']
})
export class ImportDocumentDialogComponent implements OnInit, OnDestroy {
  documentNumber: string;

  private onDestroy = new Subject();

  constructor(
    public dialogRef: MatDialogRef<ImportDocumentDialogComponent>,
    private router: Router,
    private requestService: RequestService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  ngOnInit(): void {}


  onCancel(event): void {
    this.dialogRef.close(null);
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  onImport(event): void {
    this.requestService
      .importRequest(this.documentNumber)
      .takeUntil(this.onDestroy)
      .subscribe((requestId: number) => {
        this.router.navigate(['requests', requestId]);
        this.dialogRef.close(null);
      });
  }
}
