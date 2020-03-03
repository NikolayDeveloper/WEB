import { NgModule } from '@angular/core';
import { SharedModule } from 'app/shared/shared.module';
import { PaymentsListComponent } from './components/payments-list/payments-list.component';
import { PaymentsService } from './services/payments.service';
import { PaymentUsesListComponent } from './components/payment-uses-list/payment-uses-list.component';
import { PaymentUsesService } from './services/payment-uses.service';
import { PaymentsSearchComponent } from './components/payments-search/payments-search.component';
import { PaymentsComponent } from './components/payments/payments.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { DateHttpInterceptor } from './helpers/date-http-interceptor';
import { DocumentsComponent } from './components/documents/documents.component';
import { DocumentDetailsComponent } from './components/document-details/document-details.component';
import { DocumentsListComponent } from './components/documents-list/documents-list.component';
import { DocumentsService } from './services/documents.service';
import { DocumentsSearchComponent } from './components/documents-search/documents-search.component';
import { PaymentInvoicesListComponent } from './components/payment-invoices-list.component/payment-invoices-list.component';
import { PaymentInvoicesService } from './services/payment-invoices.service';
import { PaymentsJournalComponent } from './components/payments-journal/payments-journal.component';
import { BoundPaymentDialogComponent } from './components/bound-payment-dialog/bound-payment-dialog.component';
import { PaymentService } from '../payments/payment.service';
import { AddPaymentDialogComponent } from './components/add-payment-dialog/add-payment-dialog.component';
import { ImportPaymentsDialogComponent } from './components/import-payments-dialog/import-payments-dialog.component';
import { IntegrationWith1cService } from 'app/shared/services/integration-with-1c.service';
import { DeletePaymentUseDialogComponent } from './components/delete-payment-use-dialog/delete-payment-use-dialog.component';
import { EditPaymentUseDialogComponent } from './components/edit-payment-use-dialog/edit-payment-use-dialog.component';
import { ChargePaymentInvoiceComponent } from './components/charge-payment-invoice/charge-payment-invoice.component';
import { DeleteChargedPaymentInvoiceDialogComponent } from './components/delete-charged-payment-invoice-dialog/delete-charged-payment-invoice-dialog.component';
import { EditChargedPaymentInvoiceDialogComponent } from './components/edit-charged-payment-invoice-dialog/edit-charged-payment-invoice-dialog.component';
import { ModalService } from 'app/shared/services/modal.service';
import { OkDialogComponent } from 'app/shared/components/ok-dialog/ok-dialog.component';
import { YesNoCancelDialogComponent } from 'app/shared/components/yes-no-cancel-dialog/yes-no-cancel-dialog.component';
import { PaymentReturnAmountDialogComponent } from './components/payment-return-amount-dialog/payment-return-amount-dialog.component';
import { PaymentJournalComponent } from './components/payment-journal/payment-journal.component';
import { PaymentBlockAmountDialogComponent } from './components/payment-block-amount-dialog/payment-block-amount-dialog.component';
import { PaymentsSharedService } from './helpers/payments-shared.service';
import { LinkedPaymentsDialogComponent } from './components/linked-payments-dialog/linked-payments-dialog.component';
import { SystemService } from 'app/shared/services/system.service';

@NgModule({
  imports: [
    SharedModule,
  ],
  declarations: [
    PaymentsJournalComponent,
    PaymentJournalComponent,
    PaymentsComponent,
    PaymentsListComponent,
    PaymentUsesListComponent,
    PaymentsSearchComponent,
    DocumentsComponent,
    DocumentsSearchComponent,
    DocumentsListComponent,
    DocumentDetailsComponent,
    PaymentInvoicesListComponent,
    BoundPaymentDialogComponent,
    AddPaymentDialogComponent,
    ImportPaymentsDialogComponent,
    DeletePaymentUseDialogComponent,
    EditPaymentUseDialogComponent,
    ChargePaymentInvoiceComponent,
    DeleteChargedPaymentInvoiceDialogComponent,
    EditChargedPaymentInvoiceDialogComponent,
    PaymentReturnAmountDialogComponent,
    PaymentBlockAmountDialogComponent,
    LinkedPaymentsDialogComponent
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: DateHttpInterceptor,
      multi: true
    },
    PaymentsService,
    PaymentUsesService,
    DocumentsService,
    PaymentInvoicesService,
    IntegrationWith1cService,
    ModalService,
    PaymentService,
    PaymentsSharedService,
    SystemService
  ],
  entryComponents: [
    BoundPaymentDialogComponent,
    AddPaymentDialogComponent,
    ImportPaymentsDialogComponent,
    DeletePaymentUseDialogComponent,
    EditPaymentUseDialogComponent,
    ChargePaymentInvoiceComponent,
    DeleteChargedPaymentInvoiceDialogComponent,
    EditChargedPaymentInvoiceDialogComponent,
    OkDialogComponent,
    YesNoCancelDialogComponent,
    PaymentReturnAmountDialogComponent,
    PaymentBlockAmountDialogComponent,
    LinkedPaymentsDialogComponent
  ]
})
export class PaymentsJournalModule {
}
