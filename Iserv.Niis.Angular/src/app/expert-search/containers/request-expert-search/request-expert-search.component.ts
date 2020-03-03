import { Component, Input, OnInit } from '@angular/core';
import { RequestDetails } from '../../../requests/models/request-details';
import { DocumentsCompareService } from 'app/requests/services/documents-compare.service';
import { DocumentsInfoForCompare } from 'app/requests/models/documents-compare.model';

@Component({
  selector: 'app-request-expert-search',
  templateUrl: './request-expert-search.component.html',
  styleUrls: ['./request-expert-search.component.scss']
})
export class RequestExpertSearchComponent implements OnInit {

  @Input() pdTypeCode: string;
  @Input() requestId: number;
  @Input() requestDetails: RequestDetails;
  @Input() protectionDocsId: number[];
  documentsInfoForCompare: DocumentsInfoForCompare = new DocumentsInfoForCompare();
  constructor(
    private docCompareSevice: DocumentsCompareService
  ) { }

  ngOnInit() {
    this.initializeDocsInfoForCompare(this.requestDetails.id);
  }

  initializeDocsInfoForCompare(requestId: number) {
    this.docCompareSevice.getDocumentInfo(requestId)
      .subscribe((data: DocumentsInfoForCompare) => {
        this.setDocumentsInfoForCompare(data);
      });
  }
  setDocumentsInfoForCompare(docsInfo: DocumentsInfoForCompare) {
    if (docsInfo != null) {
      this.documentsInfoForCompare = docsInfo;
    }
  }
  isExistData(): boolean {
    const data = this.documentsInfoForCompare;
    if (data === null) {
      return false;
    }
    if (data.description && data.changedDescription) {
      return true;
    }
    if (data.essay && data.changedEssay) {
      return true;
    }
    if (data.formula && data.changedFormula) {
      return true;
    }
    return false;
  }
}
