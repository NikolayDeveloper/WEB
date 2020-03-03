import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { Config } from '../../../shared/components/table/config.model';
import { LinkedPaymentDto } from '../../models/linked-payment.dto';
import { FromUTCDateToFullDateString, FromUTCDateToShortDateString } from '../../helpers/date-helpers';
import { PaymentInvoicesService } from '../../services/payment-invoices.service';

interface DialogData {
  paymentInvoiceId: number;
}

@Component({
  selector: 'app-linked-payments-dialog',
  templateUrl: './linked-payments-dialog.component.html',
  styleUrls: ['./linked-payments-dialog.component.scss']
})
export class LinkedPaymentsDialogComponent {
  public readonly columns: Config[] = [
    new Config({ columnDef: 'payerName', header: 'Плательщик', class: 'width-300' }),
    new Config({ columnDef: 'amount', header: 'Сумма', class: 'width-100' }),
    new Config({ columnDef: 'remainder', header: 'Остаток', class: 'width-100' }),
    new Config({ columnDef: 'distributed', header: 'Распределено', class: 'width-100' }),
    new Config({ columnDef: 'paymentUseAmount', header: 'Зачтено по услуге', class: 'width-100' }),
    new Config({
      columnDef: 'paymentUseDate',
      header: 'Дата зачтения',
      class: 'width-125',
      format: (row: LinkedPaymentDto) => FromUTCDateToFullDateString(row.paymentUseDate)
    }),
    new Config({ columnDef: 'paymentUseEmployeeName', header: 'Исполнитель (кто зачёл)', class: 'width-300' }),
    new Config({ columnDef: 'paymentStatusName', header: 'Статус платежа', class: 'width-150' }),
    new Config({ columnDef: 'paymentPurpose', header: 'Назначение', class: 'width-300' }),
    new Config({
      columnDef: 'paymentDate',
      header: 'Дата платежа',
      class: 'width-125',
      format: (row: LinkedPaymentDto) => FromUTCDateToShortDateString(row.paymentDate)
    }),
    new Config({ columnDef: 'paymentNumber', header: 'Номер платежа', class: 'width-150' }),
    new Config({ columnDef: 'payerXin', header: 'ИИН\\БИН плательщика', class: 'width-125' }),
    new Config({
      columnDef: 'paymentDateCreate',
      header: 'Дата создания записи',
      class: 'width-125',
      format: (row: LinkedPaymentDto) => FromUTCDateToFullDateString(row.paymentDateCreate)
    }),
    new Config({ columnDef: 'paymentId', header: 'ID', class: 'width-50' }),
    new Config({ columnDef: 'paymentDocumentNumber', header: 'Номер документа 1С', class: 'width-150' }),
    new Config({ columnDef: 'isAdvancePayment', header: 'Авансовый', class: 'width-100' }),
    new Config({ columnDef: 'isForeignCurrency', header: 'Платёж в иностранной валюте', class: 'width-100' }),
    new Config({ columnDef: 'currencyCode', header: 'Код валюты', class: 'width-125' }),
    new Config({
      columnDef: 'deletionClearedPaymentDate',
      header: 'Дата удаления зачтённой суммы оплаты',
      class: 'width-125',
      format: (row: LinkedPaymentDto) => FromUTCDateToFullDateString(row.deletionClearedPaymentDate)
    }),
    new Config({
      columnDef: 'deletionClearedPaymentEmployeeName',
      header: 'Исполнитель (кто удалил зачтённую оплату)',
      class: 'width-300'
    }),
    new Config({ columnDef: 'deletionClearedPaymentReason', header: 'Причина удаления зачтённой оплаты', class: 'width-300' })
  ];

  public listData: LinkedPaymentDto[] = [];

  constructor(
    public dialogRef: MatDialogRef<LinkedPaymentsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private paymentInvoicesService: PaymentInvoicesService) {

    paymentInvoicesService.getLinkedPayments(data.paymentInvoiceId)
      .subscribe((result: LinkedPaymentDto[]) => this.listData = result);
  }

  public onCloseClick(): void {
    this.dialogRef.close();
  }
}
