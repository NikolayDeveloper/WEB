import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { DocumentsInfoForCompare } from 'app/requests/models/documents-compare.model';

@Component({
  selector: 'app-documents-compare',
  templateUrl: './documents-compare.component.html',
  styleUrls: ['./documents-compare.component.scss']
})
export class DocumentsCompareComponent implements OnInit {
  @Input() requestId: number;
  @Input() inputDocumentsInfoForCompare: DocumentsInfoForCompare;
  documentsInfoForCompare: DocumentsInfoForCompare = new DocumentsInfoForCompare();
  constructor() { }

  ngOnInit() {
    this.initializeDocsInfoForCompare();
  }
  initializeDocsInfoForCompare() {
    this.dummyObservableMethod()
      .subscribe(() => {
        this.documentsInfoForCompare = this.inputDocumentsInfoForCompare;
      });
  }

  dummyObservableMethod(): Observable<number> {
    return new Observable<number>(observer => {
      setTimeout(() => {
        observer.next(1);
      }, 0);
    });
  }
  isVisibleEssay() {
    return (this.inputDocumentsInfoForCompare.essay && this.inputDocumentsInfoForCompare.changedEssay);
  }
  isVisibleDescription() {
    return (this.inputDocumentsInfoForCompare.description && this.inputDocumentsInfoForCompare.changedDescription);
  }
  isVisibleFormula() {
    return (this.inputDocumentsInfoForCompare.formula && this.inputDocumentsInfoForCompare.changedFormula);
  }
}
