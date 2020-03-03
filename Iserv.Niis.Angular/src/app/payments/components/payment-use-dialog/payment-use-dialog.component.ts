import 'rxjs/add/operator/takeUntil';

import { Component, Inject, OnDestroy, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { PaymentUse } from '../../models/payment.model';
import { PaymentUseListComponent } from '../payment-use-list/payment-use-list.component';

@Component({
  selector: 'app-payment-use-dialog',
  templateUrl: './payment-use-dialog.component.html',
  styleUrls: ['./payment-use-dialog.component.scss']
})
export class PaymentUseDialogComponent {
  @ViewChild(PaymentUseListComponent) paymentUseListComponent: PaymentUseListComponent;

  constructor(private dialogRef: MatDialogRef<PaymentUseDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  onSearch(queryParams: any[]) {
    this.paymentUseListComponent.reset.next(queryParams);
  }

  onUseCompleted(use?: PaymentUse) {
    this.dialogRef.close(use);
  }
}
