import 'rxjs/add/operator/takeUntil';
import { Component, Input, SimpleChanges, SimpleChange, OnChanges, OnInit } from '@angular/core';

import { Config } from '../../../shared/components/table/config.model';
import { FromUTCDateToFullDateString } from '../../helpers/date-helpers';
import { PaymentInvoicesService } from '../../services/payment-invoices.service';
import { MatDialog } from '@angular/material';
import { BoundPaymentDialogComponent } from '../bound-payment-dialog/bound-payment-dialog.component';
import { PaymentInvoiceDto } from '../../models/payment-invoice.dto';
import { PaymentsService } from '../../services/payments.service';
import { PaymentDto } from '../../models/payment.dto';
import { DocumentDto } from '../../models/document.dto';
import { AddPaymentDialogComponent } from '../add-payment-dialog/add-payment-dialog.component';
import { ChargePaymentInvoiceComponent } from '../charge-payment-invoice/charge-payment-invoice.component';
import { DeletePaymentInvoiceDto } from 'app/payments-journal/models/delete-payment-invoice-dto';
import { PaymentService } from 'app/payments/payment.service';
import { DeletePaymentInvoiceResponseDto } from 'app/payments-journal/models/delete-payment-invoice-response-dto';
import { ModalService } from 'app/shared/services/modal.service';
import {
  EditChargedPaymentInvoiceDialogComponent
} from '../edit-charged-payment-invoice-dialog/edit-charged-payment-invoice-dialog.component';
import {
  DeleteChargedPaymentInvoiceDialogComponent
} from '../delete-charged-payment-invoice-dialog/delete-charged-payment-invoice-dialog.component';
import { LinkedPaymentsDialogComponent } from '../linked-payments-dialog/linked-payments-dialog.component';
import { ColumnConfig, ColumnConfigDialogComponent, ColumnConfigService } from '../../../modules/column-config';
import { DocumentsService } from '../../services/documents.service';
import { saveAs } from 'file-saver';
import { YesNoCancelDialogComponent } from 'app/shared/components/yes-no-cancel-dialog/yes-no-cancel-dialog.component';
import { ModalButton } from 'app/shared/services/models/modal-button.enum';

const COLUMNS_CONFIG_KEY = 'payments_journal_payment_invoices_columns_config';

@Component({
  selector: 'app-payment-invoices-list',
  templateUrl: './payment-invoices-list.component.html'
})
export class PaymentInvoicesListComponent implements OnInit, OnChanges {
  @Input()
  public paymentId: number;

  @Input()
  public document: DocumentDto;

  public data: PaymentInvoiceDto[] = [];

  public displayedColumns: string[];

  public readonly columnsConfigs: ColumnConfig[] = [
    { field: 'tariffCode', name: 'Код услуги', enabled: true },
    { field: 'tariffNameRu', name: 'Наименование услуги', enabled: true },
    { field: 'tariffPrice', name: 'Тариф', enabled: true },
    { field: 'coefficient', name: 'Коэффициент', enabled: true },
    { field: 'tariffCount', name: 'Количество', enabled: true },
    { field: 'penaltyPercent', name: 'Штраф', enabled: true },
    { field: 'nds', name: 'name', enabled: true },
    { field: 'totalAmount', name: 'Сумма выплаты', enabled: true },
    { field: 'totalAmountNds', name: 'Сумма с НДС', enabled: true },
    { field: 'amountUseSum', name: 'Фактическая сумма', enabled: true },
    { field: 'statusNameRu', name: 'Статус оплаты', enabled: true },
    { field: 'remainder', name: 'Не погашенная сумма', enabled: true },
    { field: 'createDate', name: 'Дата выставления счёта на оплату услуги', enabled: true },
    { field: 'createUser', name: 'Исполнитель (кто добавил счёт)', enabled: true },
    { field: 'creditDate', name: 'Дата зачтения оплаты', enabled: true },
    { field: 'creditUser', name: 'Исполнитель (кто зачёл)', enabled: true },
    { field: 'dateComplete', name: 'Дата списания', enabled: true },
    { field: 'writeOffUser', name: 'Исполнитель (кто списал)', enabled: true },
    { field: 'deleteDate', name: 'Дата удаления выставленной оплаты', enabled: true },
    { field: 'deleteUser', name: 'Исполнитель (кто удалил выставленную оплату)', enabled: true },
    { field: 'deletionReason', name: 'Причина удаления выставленной оплаты', enabled: true },
    { field: 'boundPaymentInvoice', name: 'Зачесть оплату', enabled: true },
    { field: 'chargePaymentInvoice', name: 'Списать оплату', enabled: true },
    { field: 'editChargedPaymentInvoice', name: 'Изменить дату списания', enabled: true  },
    { field: 'deleteChargedPaymentInvoice', name: 'Удалить списанную оплату', enabled: true  },
    { field: 'deletePaymentInvoice', name: 'Удалить выставленную оплату', enabled: true  },
    { field: 'viewPayments', name: 'Связанные платежи', enabled: true }
  ];

  public readonly columns: Config[] = [
    new Config({ columnDef: 'tariffCode', header: 'Код услуги', class: 'width-125' }),
    new Config({ columnDef: 'tariffNameRu', header: 'Наименование услуги', class: 'width-300' }),
    new Config({ columnDef: 'tariffPrice', header: 'Тариф', class: 'width-125' }),
    new Config({ columnDef: 'coefficient', header: 'Коэффициент', class: 'width-125' }),
    new Config({ columnDef: 'tariffCount', header: 'Количество', class: 'width-125' }),
    new Config({
      columnDef: 'penaltyPercent',
      header: 'Штраф',
      class: 'width-125',
      format: (row: PaymentInvoiceDto) => `${(row.penaltyPercent ? row.penaltyPercent : 0) * 100}%`
    }),
    new Config({
      columnDef: 'nds',
      header: 'НДС',
      class: 'width-125',
      format: (row: PaymentInvoiceDto) => `${(row.nds ? row.nds : 0) * 100}%`
    }),
    new Config({ columnDef: 'totalAmount', header: 'Сумма выплаты', class: 'width-125' }),
    new Config({ columnDef: 'totalAmountNds', header: 'Сумма с НДС', class: 'width-125' }),
    new Config({ columnDef: 'amountUseSum', header: 'Фактическая сумма', class: 'width-125' }),
    new Config({ columnDef: 'statusNameRu', header: 'Статус оплаты', class: 'width-300' }),
    new Config({ columnDef: 'remainder', header: 'Не погашенная сумма', class: 'width-125' }),
    new Config({
      columnDef: 'createDate',
      header: 'Дата выставления счёта на оплату услуги',
      class: 'width-125',
      format: (row: PaymentInvoiceDto) => FromUTCDateToFullDateString(row.createDate)
    }),
    new Config({ columnDef: 'createUser', header: 'Исполнитель (кто добавил счёт)', class: 'width-300' }),
    new Config({
      columnDef: 'creditDate',
      header: 'Дата зачтения оплаты',
      class: 'width-125',
      format: (row: PaymentInvoiceDto) => FromUTCDateToFullDateString(row.creditDate)
    }),
    new Config({ columnDef: 'creditUser', header: 'Исполнитель (кто зачёл)', class: 'width-300' }),
    new Config({
      columnDef: 'dateComplete',
      header: 'Дата списания',
      class: 'width-125',
      format: (row: PaymentInvoiceDto) => FromUTCDateToFullDateString(row.writeOffDate)
    }),
    new Config({ columnDef: 'writeOffUser', header: 'Исполнитель (кто списал)', class: 'width-300' }),
    new Config({
      columnDef: 'deleteDate',
      header: 'Дата удаления выставленной оплаты',
      class: 'width-125',
      format: (row: PaymentInvoiceDto) => FromUTCDateToFullDateString(row.deleteDate)
    }),
    new Config({ columnDef: 'deleteUser', header: 'Исполнитель (кто удалил выставленную оплату)', class: 'width-300' }),
    new Config({ columnDef: 'deletionReason', header: 'Причина удаления выставленной оплаты', class: 'width-300' }),
    new Config({
      columnDef: 'boundPaymentInvoice',
      header: 'Зачесть оплату',
      class: 'width-25 sticky',
      icon: 'payment',
      click: row => this.boundPayment(row),
      disable: row => (!this.isInvoiceNotPayed(row) || row.isDeleted)
    }),
    new Config({
      columnDef: 'chargePaymentInvoice',
      header: 'Списать оплату',
      icon: 'account_balance',
      class: 'width-25 sticky',
      click: row => this.chargePaymentInvoice(row),
      disable: row => (row.statusCode !== 'credited' && row.statusCode !== 'notpaid' || row.isDeleted)
    }),
    new Config({
      columnDef: 'editChargedPaymentInvoice',
      header: 'Изменить дату списания',
      class: 'width-25 sticky',
      icon: 'edit',
      click: row => this.EditChargedPaymentInvoiceClick(row),
      disable: row => (row.statusCode !== 'charged' || row.isDeleted)
    }),
    new Config({
      columnDef: 'deleteChargedPaymentInvoice',
      header: 'Удалить списанную оплату',
      class: 'width-25 sticky',
      icon: 'delete_forever',
      click: row => this.DeleteChargedPaymentInvoiceClick(row),
      disable: row => (row.statusCode !== 'charged' || row.isDeleted)
    }),
    new Config({
      columnDef: 'deletePaymentInvoice',
      header: 'Удалить выставленную оплату',
      class: 'width-25 sticky',
      icon: 'delete',
      click: row => this.deletePaymentInvoiceClick(row),
      disable: row => (row.statusCode === 'charged' || row.isDeleted)
    }),
    new Config({
      columnDef: 'viewPayments',
      header: 'Связанные платежи',
      class: 'width-25 sticky',
      icon: 'list',
      click: row => this.viewLinkedPayments(row),
      disable: () => false
    })
  ];

  constructor(public paymentInvoicesService: PaymentInvoicesService,
              private paymentsService: PaymentsService,
              private paymentService: PaymentService,
              private dialog: MatDialog,
              private modalService: ModalService,
              private columnConfigService: ColumnConfigService,
              private documentsService: DocumentsService) {
  }

  public ngOnInit(): void {
    this.initializeColumns();
  }

  public ngOnChanges(changes: SimpleChanges) {
    const document: SimpleChange = changes.document;

    if (document.previousValue !== document.currentValue) {
      this.loadData();
    }
  }

  public addPayment(): void {
    const dialogRef = this.dialog.open(AddPaymentDialogComponent, {
      data: {
        ownerId: this.document.id,
        ownerType: this.document.documentCategory,
        protectionDocTypeId: this.document.protectionDocTypeId
      },
      width: '1000px'
    });

    dialogRef
      .afterClosed()
      .filter(invoice => !!invoice)
      .subscribe(invoice => {
        this.loadData();
      });
  }

  public onColumnsChange(defaultConfig) {
    const dialogRef = this.dialog.open(ColumnConfigDialogComponent, {
      data: {
        configKey: COLUMNS_CONFIG_KEY,
        defaultConfig: defaultConfig.defaultConfig
      },
      disableClose: true
    });

    dialogRef.afterClosed()
      .subscribe(result => {
        this.displayedColumns = result;

        this.columnsConfigs.forEach(column => {
          column.enabled = result.includes(column.field);
        });
        this.columnConfigService.save(COLUMNS_CONFIG_KEY, this.columnsConfigs);
      });
  }

  private loadData(): void {
    this.paymentInvoicesService.getByDocument(this.document && this.document.id, this.document && this.document.documentCategory)
      .subscribe((result: PaymentInvoiceDto[]) => this.data = result);
  }

  private isInvoiceNotPayed(invoice: PaymentInvoiceDto): boolean {
    return invoice != null && invoice.statusCode === 'notpaid';
  }

  private boundPayment(paymentInvoice: PaymentInvoiceDto): void {
    this.paymentsService.getById(this.paymentId)
      .subscribe((payment: PaymentDto) => {
        this.dialog.open(
          BoundPaymentDialogComponent,
          {
            width: '500px',
            data: {
              payment: payment,
              documentId: this.document.id,
              documentCategory: this.document.documentCategory,
              paymentInvoice: paymentInvoice
            }
          },
        )
          .afterClosed()
          .filter((result: boolean) => result)
          .subscribe(() => this.loadData());
      });
  }

  private chargePaymentInvoice(paymentInvoice: PaymentInvoiceDto): void {
    this.paymentsService.getById(this.paymentId)
      .subscribe((payment: PaymentDto) => {
        this.dialog.open(
          ChargePaymentInvoiceComponent,
          {
            width: '500px',
            data: {
              ownerType: this.document.documentCategory,
              paymentInvoice: paymentInvoice
            }
          },
        )
          .afterClosed()
          .subscribe(() => this.loadData());
      });
  }

  private deletePaymentInvoiceClick(paymentInvoice: PaymentInvoiceDto): void {
    this.dialog
          .open(YesNoCancelDialogComponent, {
              data: {
                  title: 'Удаление платежа',
                  message: 'Платеж будет удален физически',
                  buttons: [ModalButton.Yes, ModalButton.No],
                  labels: {
                    [ModalButton.Yes]: 'ОК',
                    [ModalButton.No]: 'Отмена'
                  },
              }
          })
          .afterClosed()
          .subscribe((result: ModalButton) => {
              if (result === ModalButton.Yes) {
                this.deletePaymentInvoice(paymentInvoice);
              }
          });
  }

  private deletePaymentInvoice(paymentInvoice: PaymentInvoiceDto): void {
    this.paymentsService.getById(this.paymentId)
      .subscribe((payment: PaymentDto) => {

        const requestDto = new DeletePaymentInvoiceDto();
        requestDto.OwnerType = this.document.documentCategory;

        this.paymentService.deletePaymentInvoice(paymentInvoice, requestDto)
          .subscribe((resposeDto: DeletePaymentInvoiceResponseDto) => {
            this.modalService
              .ok('OK', resposeDto.message)
              .subscribe(() => {
                this.loadData();
              });
          });
      });
  }

  private EditChargedPaymentInvoiceClick(paymentInvoice: PaymentInvoiceDto): void {
    this.paymentsService.getById(this.paymentId)
      .subscribe((payment: PaymentDto) => {
        this.dialog.open(
          EditChargedPaymentInvoiceDialogComponent,
          {
            width: '500px',
            data: {
              ownerType: this.document.documentCategory,
              PaymentInvoiceId: paymentInvoice.id
            }
          },
        )
          .afterClosed()
          .subscribe(() => this.loadData());
      });
  }

  private DeleteChargedPaymentInvoiceClick(paymentInvoice: PaymentInvoiceDto): void {
    this.paymentsService.getById(this.paymentId)
      .subscribe((payment: PaymentDto) => {
        this.dialog.open(
          DeleteChargedPaymentInvoiceDialogComponent,
          {
            width: '500px',
            data: {
              ownerType: this.document.documentCategory,
              PaymentInvoiceId: paymentInvoice.id
            }
          },
        )
          .afterClosed()
          .subscribe(() => this.loadData());
      });
  }

  private viewLinkedPayments(paymentInvoice: PaymentInvoiceDto): void {
    this.dialog.open(
      LinkedPaymentsDialogComponent,
      {
        width: '1000px',
        data: {
          paymentInvoiceId: paymentInvoice.id
        }
      });
  }

  private initializeColumns(): void {
    if (!this.displayedColumns) {
      this.displayedColumns = this.columnConfigService.get(COLUMNS_CONFIG_KEY, this.columnsConfigs)
        .filter(cc => cc.enabled)
        .map(cc => cc.field);
    } else {
      this.columnConfigService.save(COLUMNS_CONFIG_KEY, this.columnsConfigs);
    }
  }

  onExport(columns: string[]): void {
    const queryParams = [
      {
        key: 'excelFields',
        value: (columns && columns.length) ? columns.join() : this.displayedColumns.join()
      }
    ];

    this.documentsService
      .getExcel(queryParams, this.document.documentCategory, this.document.id)
      .subscribe((data) => {
        saveAs(data.body, `Выставленные оплаты № ${this.document.barcode}.xlsx`);
      });
  }
}
