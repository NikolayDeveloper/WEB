import { Component } from '@angular/core';

import { MatDialog, MatDialogConfig } from '@angular/material';
import { ImportPaymentsDialogComponent } from '../import-payments-dialog/import-payments-dialog.component';
import { PaymentsSearchParametersDto } from 'app/payments-journal/models/payments-search-parameters.dto';

@Component({
  selector: 'app-payment-journal',
  templateUrl: './payment-journal.component.html',
  styleUrls: ['./payment-journal.component.scss']
})
export class PaymentJournalComponent {
  isPaymentsMode: boolean;
  isDocumentsMode: boolean;
  public searchParams: PaymentsSearchParametersDto;

  public selectedPaymentId: number;
  
  constructor(
    public dialog: MatDialog) {
  }

  public onApplySearch(searchParams: PaymentsSearchParametersDto): void {
    this.searchParams = searchParams;
  }

  public onSelectedPaymentIdChanged(selectedPaymentId: number) {
    this.selectedPaymentId = selectedPaymentId;
  }

  public onImportPaymentsClick() {
    const config = new MatDialogConfig();
    config.disableClose = true;
    const dialogRef = this.dialog.open(ImportPaymentsDialogComponent, config);
  }
}
