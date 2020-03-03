import { Component, Input } from '@angular/core';
import { DocumentDto } from '../../models/document.dto';
import { toFullDateString, toShortDateString } from '../../helpers/date-helpers';
import { DomSanitizer } from '@angular/platform-browser';
import { DocumentCategory } from '../../models/document-category';

@Component({
  selector: 'app-document-details',
  templateUrl: './document-details.component.html',
  styleUrls: ['./document-details.component.scss']
})
export class DocumentDetailsComponent {
  @Input()
  public document: DocumentDto;

  constructor(public domSanitizer: DomSanitizer) {
  }

  public getFullDateString(value: Date): string {
    return toFullDateString(value);
  }

  public getShortDateString(value: Date): string {
    return toShortDateString(value);
  }

  public getRequestRegNumber(): string {
    return this.document && this.document.documentCategory === DocumentCategory.Request ? this.document.regNumber : '';
  }

  public getProtectionDocRegNumber(): string {
    return this.document && this.document.documentCategory === DocumentCategory.ProtectionDoc ? this.document.regNumber : '' ;
  }

  public getContractRegNumber(): string {
    return this.document && this.document.documentCategory === DocumentCategory.Contract ? this.document.regNumber : '';
  }

  public getCardLink(): string {
    if (!this.document) {
      return '';
    }

    switch (this.document.documentCategory) {
      case DocumentCategory.Request:
        return `/requests/${this.document.id}`;
      case DocumentCategory.ProtectionDoc:
        return `/protectiondocs/${this.document.id}`;
      case DocumentCategory.Contract:
        return `/contracts/${this.document.id}`;
    }

    return '';
  }
}
