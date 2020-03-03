import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { PaymentsComponent } from './components/payments/payments.component';
import { InvoiceDialogComponent } from './components/invoice-dialog/invoice-dialog.component';
import { PaymentService } from './payment.service';
import { PaymentUseDialogComponent } from './components/payment-use-dialog/payment-use-dialog.component';
import { PaymentSearchFormComponent } from './components/payment-search-form/payment-search-form.component';
import { PaymentUseListComponent } from './components/payment-use-list/payment-use-list.component';
import { LinkedPaymentListComponent } from './components/linked-payment-list/linked-payment-list.component';
import { SystemService } from 'app/shared/services/system.service';

@NgModule({
  imports: [
    SharedModule,
  ],
  exports: [
    PaymentUseDialogComponent,
    PaymentsComponent,
  ],
  declarations: [
    PaymentsComponent,
    InvoiceDialogComponent,
    PaymentUseDialogComponent,
    PaymentSearchFormComponent,
    PaymentUseListComponent,
    LinkedPaymentListComponent,
  ],
  bootstrap: [
    PaymentUseDialogComponent
  ],
  providers: [
    PaymentService,
    SystemService
  ],
  entryComponents: [
    InvoiceDialogComponent,
    PaymentUseDialogComponent,
    LinkedPaymentListComponent,
  ]
})
export class PaymentsModule { }
