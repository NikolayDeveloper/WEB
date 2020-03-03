import { Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { PaymentsSearchParametersDto } from '../../models/payments-search-parameters.dto';
import { MatDialogConfig, MatDialog } from '@angular/material';
import { ImportPaymentsDialogComponent } from '../import-payments-dialog/import-payments-dialog.component';
import { ModalService } from 'app/shared/services/modal.service';
import { PaymentService } from 'app/payments/payment.service';
import { Subject } from 'rxjs';
import { GetPaymentForReturnAmountResponseDto } from 'app/shared/models/get-payment-for-return-amount-response-dto';
import { PaymentReturnAmountDialogComponent } from '../payment-return-amount-dialog/payment-return-amount-dialog.component';
import { PaymentBlockAmountDialogComponent } from '../payment-block-amount-dialog/payment-block-amount-dialog.component';
import { GetPaymentForBlockAmountResponseDto } from 'app/shared/models/get-payment-for-block-amount-response-dto';
import { PaymentsListComponent } from '../payments-list/payments-list.component';

@Component({
  selector: 'app-payments',
  templateUrl: './payments.component.html',
  styleUrls: ['./payments.component.scss']
})
export class PaymentsComponent {
  @ViewChild(PaymentsListComponent)
  private paymentsListComponent: PaymentsListComponent;

  @Output()
  public readonly boundPayment = new EventEmitter<number>();

  public searchParams: PaymentsSearchParametersDto;

  public selectedPaymentId: number;
  public isPaymentInForiegnCurrency: boolean = false;

  private onDestroy = new Subject();

  constructor(
    public dialog: MatDialog,
    private modalService: ModalService,
    private paymentService: PaymentService) {
  }

  public onApplySearch(searchParams: PaymentsSearchParametersDto): void {
    this.searchParams = searchParams;
    this.isPaymentInForiegnCurrency = searchParams.isForeignCurrency;
  }

  public onSelectedPaymentIdChanged(selectedPaymentId: number): void {
    if (this.selectedPaymentId === selectedPaymentId) {
      setTimeout(() => {
        this.selectedPaymentId = null;
      });
    }

    setTimeout(() => {
      this.selectedPaymentId = selectedPaymentId;
    });
  }

  public onImportPaymentsClick() {
    const config = new MatDialogConfig();
    config.disableClose = true;

    const dialogRef = this.dialog.open(ImportPaymentsDialogComponent, config);
    dialogRef.componentInstance.paymentsImported.subscribe(() => {
      dialogRef.componentInstance.paymentsImported.unsubscribe();
      this.paymentsListComponent.loadData();
    });
  }

  public onPaymentReturnAmountClick() {
    this.paymentService.getPaymentForReturnAmount(this.selectedPaymentId)
      .takeUntil(this.onDestroy)
      .subscribe((responseDto: GetPaymentForReturnAmountResponseDto) => {
        const config = new MatDialogConfig();
        config.width = '350px';
        config.disableClose = true;
        config.data = {
          PaymentForReturnAmount: responseDto
        };

        const dialogRef = this.dialog.open(PaymentReturnAmountDialogComponent, config);
        dialogRef.componentInstance.paymentAmountReturned.subscribe(() => {
          dialogRef.componentInstance.paymentAmountReturned.unsubscribe();
          this.paymentsListComponent.loadData();
        });
      });
  }

  public onPaymentBlockAmountClick() {
    this.paymentService.getPaymentForBlockAmount(this.selectedPaymentId)
      .takeUntil(this.onDestroy)
      .subscribe((responseDto: GetPaymentForBlockAmountResponseDto) => {
        const config = new MatDialogConfig();
        config.width = '400px';
        config.height = '450px';
        config.disableClose = true;
        config.data = {
          PaymentForBlockAmount: responseDto
        };

        const dialogRef = this.dialog.open(PaymentBlockAmountDialogComponent, config);
        dialogRef.componentInstance.paymentAmountBlocked.subscribe(() => {
          dialogRef.componentInstance.paymentAmountBlocked.unsubscribe();
          this.paymentsListComponent.loadData();
        });
      });
  }

  public onBoundPayment(): void {
    this.boundPayment.emit(this.selectedPaymentId);
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

}
