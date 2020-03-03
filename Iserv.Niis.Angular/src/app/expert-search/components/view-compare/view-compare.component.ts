import { Component, Input, OnInit } from '@angular/core';
import { DocumentsCompareService } from 'app/requests/services/documents-compare.service';


@Component({
  selector: 'app-view-compare',
  templateUrl: './view-compare.component.html',
  styleUrls: ['./view-compare.component.scss']
})
export class ViewCompareComponent implements OnInit {
  @Input() originalText: string;
  @Input() changedText: string;
  @Input() documentId: number;
  constructor(private docsCompareService: DocumentsCompareService) { }

  ngOnInit() {
    this.originalText = '';
    this.changedText = '';
  }
  makeDocumentFinished() {
    this.docsCompareService.makeDocumentFinished(this.documentId)
      .subscribe(() => {
        window.location.reload();
      });
  }
}
