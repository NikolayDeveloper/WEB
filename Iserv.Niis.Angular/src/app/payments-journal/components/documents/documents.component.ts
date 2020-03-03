import { Component, EventEmitter, Input, Output } from '@angular/core';
import { PaymentsSearchParametersDto } from '../../models/payments-search-parameters.dto';
import { DocumentDto } from '../../models/document.dto';

@Component({
  selector: 'app-documents',
  templateUrl: './documents.component.html',
  styleUrls: ['./documents.component.scss']
})
export class DocumentsComponent {
  @Input()
  public paymentId: number;

  @Output()
  public readonly returnToPayments = new EventEmitter<void>();

  public searchParams: PaymentsSearchParametersDto;

  public selectedDocument: DocumentDto;

  public onApplySearch(searchParams: PaymentsSearchParametersDto): void {
    this.searchParams = searchParams;
  }

  public onSelectedDocumentChanged(selectedDocument: DocumentDto) {
    this.selectedDocument = selectedDocument;
  }

  public onReturnBack(): void {
    this.returnToPayments.emit();
  }
}
