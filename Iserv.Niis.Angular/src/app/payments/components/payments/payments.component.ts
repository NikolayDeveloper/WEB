import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  SimpleChanges
} from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { ColumnConfigDialogComponent } from 'app/modules/column-config/column-config-dialog/column-config-dialog.component';
import { ColumnConfig } from 'app/modules/column-config/column-config.model';
import { ColumnConfigService } from 'app/modules/column-config/column-config.service';
import 'rxjs/add/operator/takeUntil';
import { Subject } from 'rxjs/Subject';
import { SnackBarHelper } from '../../../core/snack-bar-helper.service';
import { Config } from '../../../shared/components/table/config.model';
import { OwnerType } from '../../../shared/services/models/owner-type.enum';
import { SubjectDto } from '../../../subjects/models/subject.model';
import { PaymentInvoice } from '../../models/payment.model';
import { PaymentService } from '../../payment.service';
import { InvoiceDialogComponent } from '../invoice-dialog/invoice-dialog.component';
import { PaymentUseDialogComponent } from '../payment-use-dialog/payment-use-dialog.component';
import { DeleteChargedPaymentInvoiceDialogComponent } from 'app/payments-journal/components/delete-charged-payment-invoice-dialog/delete-charged-payment-invoice-dialog.component';
import { ChargePaymentInvoiceComponent } from 'app/payments-journal/components/charge-payment-invoice/charge-payment-invoice.component';
import { EditChargedPaymentInvoiceDialogComponent } from 'app/payments-journal/components/edit-charged-payment-invoice-dialog/edit-charged-payment-invoice-dialog.component';
import { DeletePaymentInvoiceDto } from 'app/payments-journal/models/delete-payment-invoice-dto';
import { DeletePaymentInvoiceResponseDto } from 'app/payments-journal/models/delete-payment-invoice-response-dto';
import { ModalService } from 'app/shared/services/modal.service';
import { FromUTCDateToFullDateString, toFullDateString } from 'app/payments-journal/helpers/date-helpers';
import { LinkedPaymentListComponent } from '../linked-payment-list/linked-payment-list.component';
import { YesNoCancelDialogComponent } from 'app/shared/components/yes-no-cancel-dialog/yes-no-cancel-dialog.component';
import { ModalButton } from 'app/shared/services/models/modal-button.enum';

const columnsConfigKey = 'payment_invoices_columns_config';
const defaultColumnsConfig: ColumnConfig[] = [
  { field: 'tariffCode', name: 'Код услуги', enabled: true },
  { field: 'tariffNameRu', name: 'Наименование услуги', enabled: true },
  { field: 'tariffPrice', name: 'Тариф', enabled: true },
  { field: 'coefficient', name: 'Коэффициент', enabled: true },
  { field: 'tariffCount', name: 'Количество', enabled: true },
  { field: 'penaltyPercent', name: 'Штраф', enabled: true },
  { field: 'nds', name: 'НДС', enabled: true },
  { field: 'totalAmount', name: 'Сумма выплаты', enabled: true },
  { field: 'totalAmountNds', name: 'Сумма с НДС', enabled: true },
  { field: 'amountUseSum', name: 'Фактическая сумма', enabled: true },
  { field: 'statusNameRu', name: 'Статус оплаты', enabled: true },
  { field: 'remainder', name: 'Не погашенная сумма', enabled: true },
  { field: 'createDate', name: 'Дата выставления счёта на оплату услуги', enabled: true },
  { field: 'createUser', name: 'Исполнитель (кто добавил счёт)', enabled: true },
  { field: 'creditDate', name: 'Дата зачтения оплаты', enabled: true },
  { field: 'creditUser', name: 'Исполнитель (кто зачёл)', enabled: true },
  { field: 'writeOffDate', name: 'Дата списания', enabled: true },
  { field: 'writeOffUser', name: 'Исполнитель (кто списал)', enabled: true },
  { field: 'deleteDate', name: 'Дата удаления выставленной оплаты', enabled: true },
  { field: 'deleteUser', name: 'Исполнитель (кто удалил выставленную оплату)', enabled: true },
  { field: 'deletionReason', name: 'Причина удаления выставленной оплаты', enabled: true },
  { field: 'linkPayment', name: 'Связать оплату', enabled: true },
  { field: 'linkedPayments', name: 'Связанные оплаты', enabled: true },
  { field: 'chargePaymentInvoice', name: 'Списать оплату', enabled: true },
  { field: 'editChargedPaymentInvoice', name: 'Изменить списанную оплату', enabled: true },
  { field: 'deleteChargedPaymentInvoice', name: 'Удалить списанную оплату', enabled: true },
  { field: 'deletePaymentInvoice', name: 'Удалить выставленную оплату', enabled: true }
];

@Component({
  selector: 'app-payments',
  templateUrl: './payments.component.html',
  styleUrls: ['./payments.component.scss']
})
export class PaymentsComponent implements OnInit, OnChanges, OnDestroy {
  columns: Config[] = [];
  formGroup: FormGroup;
  invoiceDtos: PaymentInvoice[];
  displayedColumns: string[];

  @Input() disabled: boolean;
  @Input() disabledCharge: boolean;
  @Input() flagChange: boolean;
  @Input() ownerId: number;
  @Input() ownerType: OwnerType;
  @Input() payingSubject: SubjectDto;
  @Input() protectionDocTypeId: number;
  @Output() changed = new EventEmitter();

  private onDestroy = new Subject();
  private defaultColumns: Config[] = [
    new Config({
      columnDef: 'tariffCode',
      header: 'Код услуги',
      class: 'width-100'
    }),
    new Config({
      columnDef: 'tariffNameRu',
      header: 'Наименование услуги',
      class: 'width-300'
    }),
    new Config({
      columnDef: 'tariffPrice',
      header: 'Тариф',
      class: 'width-100'
    }),
    new Config({
      columnDef: 'coefficient',
      header: 'Коэффициент',
      class: 'width-100'
    }),
    new Config({
      columnDef: 'tariffCount',
      header: 'Количество',
      class: 'width-100'
    }),
    new Config({
      columnDef: 'penaltyPercent',
      header: 'Штраф',
      class: 'width-50'
    }),
    new Config({ columnDef: 'nds', header: 'НДС', class: 'width-50' }),
    new Config({
      columnDef: 'totalAmount',
      header: 'Сумма выплаты',
      class: 'width-175'
    }),
    new Config({
      columnDef: 'totalAmountNds',
      header: 'Сумма с НДС',
      class: 'width-175'
    }),
    new Config({
      columnDef: 'amountUseSum',
      header: 'Фактическая сумма',
      class: 'width-175'
    }),
    new Config({
      columnDef: 'statusNameRu',
      header: 'Статус оплаты',
      class: 'width-100'
    }),
    new Config({
      columnDef: 'remainder',
      header: 'Не погашенная сумма',
      class: 'width-175'
    }),
    new Config({
      columnDef: 'createDate',
      header: 'Дата выставления счёта на оплату услуги',
      class: 'width-200',
      format: row => toFullDateString(row.createDate)
    }),
    new Config({
      columnDef: 'createUser',
      header: 'Исполнитель (кто добавил счёт)',
      class: 'width-150'
    }),
    new Config({
      columnDef: 'creditDate',
      header: 'Дата зачтения оплаты',
      class: 'width-200',
      format: row => toFullDateString(row.creditDate)
    }),
    new Config({
      columnDef: 'creditUser',
      header: 'Исполнитель (кто зачёл)',
      class: 'width-150'
    }),
    new Config({
      columnDef: 'writeOffDate',
      header: 'Дата списания',
      class: 'width-200',
      format: row => toFullDateString(row.writeOffDate)
    }),
    new Config({
      columnDef: 'writeOffUser',
      header: 'Исполнитель (кто списал)',
      class: 'width-150'
    }),
    new Config({
      columnDef: 'deleteDate',
      header: 'Дата удаления выставленной оплаты',
      class: 'width-200',
      format: row => toFullDateString(row.deleteDate)
    }),
    new Config({
      columnDef: 'deleteUser',
      header: 'Исполнитель (кто удалил выставленную оплату)',
      class: 'width-150'
    }),
    new Config({
      columnDef: 'deletionReason',
      header: 'Причина удаления выставленной оплаты',
      class: 'width-150'
    }),
    new Config({
      columnDef: 'linkPayment',
      header: 'Связать оплату',
      icon: 'payment',
      class: 'width-25 sticky',
      click: row => this.onLinkPayment.call(this, row),
      disable: row => row.statusCode !== 'notpaid' || this.disabled,
      isSticky: true
    }),
    new Config({
      columnDef: 'linkedPayments',
      header: 'Связанные оплаты',
      icon: 'storage',
      class: 'width-25 sticky',
      click: row => this.onLinkedPayments.call(this, row),
      disable: row => row.amountUseSum === 0 || this.disabled,
      isSticky: true
    }),
    new Config({
      columnDef: 'chargePaymentInvoice',
      header: 'Списать оплату',
      icon: 'account_balance',
      class: 'width-25 sticky',
      click: row => this.onChargePaymentInvoice.call(this, row),
      disable: row => (row.statusCode !== 'credited' && row.statusCode !== 'notpaid' ) || this.disabledCharge || this.disabled,
      isSticky: true
    }),
    new Config({
      columnDef: 'editChargedPaymentInvoice',
      header: 'Редактировать списанную оплату',
      class: 'width-25 sticky',
      icon: 'edit',
      click: row => this.onEditChargedPaymentInvoiceClick.call(this, row),
      disable: row => (row.statusCode !== 'charged'  || row.isDeleted) || this.disabled,
      isSticky: true
    }),
    new Config({
      columnDef: 'deleteChargedPaymentInvoice',
      header: 'Удалить списанную оплату',
      class: 'width-25 sticky',
      icon: 'delete_forever',
      click: row => this.onDeleteChargedPaymentInvoiceClick.call(this, row),
      disable: row => (row.statusCode !== 'charged'  || row.isDeleted) || this.disabled,
      isSticky: true
    }),
    new Config({
      columnDef: 'deletePaymentInvoice',
      header: 'Удалить выставленную оплату',
      class: 'width-25 sticky',
      icon: 'delete',
      click: row => this.onDeletePaymentInvoiceClick.call(this, row),
      disable: row => (row.statusCode === 'charged' || row.isDeleted) || this.disabled,
      isSticky: true
    })
  ];

  constructor(
    private paymentService: PaymentService,
    private snackBarHelper: SnackBarHelper,
    private columnConfigService: ColumnConfigService,
    public dialog: MatDialog,
    private modalService: ModalService
  ) {}

  ngOnInit() {
    this.displayedColumns = this.columnConfigService
      .get(columnsConfigKey, defaultColumnsConfig)
      .filter(cc => cc.enabled)
      .map(cc => cc.field);
    this.generateColumns(this.displayedColumns);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.ownerId && this.ownerType) {
      this.paymentService
        .getInvoices(this.ownerId, this.ownerType)
        .takeUntil(this.onDestroy)
        .subscribe((invoices: any) => {
          this.invoiceDtos = invoices;
        });
    }
  }

  onAddService() {
    if (this.isPayingSubjectMissing()) {
      return;
    }

    const dialogRef = this.dialog.open(InvoiceDialogComponent, {
      data: {
        ownerId: this.ownerId,
        ownerType: this.ownerType,
        protectionDocTypeId: this.protectionDocTypeId
      },
      width: '1000px'
    });

    dialogRef
      .afterClosed()
      .takeUntil(this.onDestroy)
      .filter(invoice => !!invoice)
      .subscribe(invoice => {
        this.invoiceDtos = [invoice, ...this.invoiceDtos];
      });
  }

  onLinkPayment(selectedInvoice: PaymentInvoice) {
    if (this.isPayingSubjectMissing()) {
      return;
    }

    if (Math.round(selectedInvoice.remainder) < 0) {
      const errorMessage =
        'Непогашенная сумма отрицательная, применение оплаты невозможно!\nОбратитесь пожалуйста к администратору!';
      console.error(`${errorMessage} Details: {ownerType:${
        this.ownerType
      }, ownerId:${this.ownerId},
       paymentInvoice: ${JSON.stringify(selectedInvoice)}}`);
      this.snackBarHelper.error(errorMessage);
      return;
    }

    if (Math.round(selectedInvoice.remainder) === 0) {
      const errorMessage =
        'Услуга полностью зачтена, применение оплаты невозможно!\nОбратитесь пожалуйста к администратору!';
      console.error(`${errorMessage} Details: {ownerType:${
        this.ownerType
      }, ownerId:${this.ownerId},
       paymentInvoice: ${JSON.stringify(selectedInvoice)}}`);
      this.snackBarHelper.error(errorMessage);
      return;
    }


    const dialogRef = this.dialog.open(PaymentUseDialogComponent, {
      data: {
        ownerType: this.ownerType,
        payingSubject: this.payingSubject,
        paymentInvoice: selectedInvoice
      },
      width: '800px',
      height: '80vh'
    });

    dialogRef
      .afterClosed()
      .takeUntil(this.onDestroy)
      .subscribe(paymentUse => {
        if (paymentUse) {
          this.paymentService
            .getInvoices(this.ownerId, this.ownerType)
            .takeUntil(this.onDestroy)
            .subscribe(invoices => {
              this.invoiceDtos = invoices;
              this.changed.emit(this.invoiceDtos);
            }, console.log);
        }
      });
  }

  onLinkedPayments(selectedInvoice: PaymentInvoice) {
    const dialogRef = this.dialog.open(LinkedPaymentListComponent, {
      data: {
        paymentInvoice: selectedInvoice
      },
      width: '1200px',
      height: '300px'
    });

  }

  onChargePaymentInvoice(selectedInvoice: PaymentInvoice) {
    if (this.isPayingSubjectMissing()) {
      return;
    }

    const dialogRef = this.dialog.open(ChargePaymentInvoiceComponent, {
      data: {
        ownerType: this.ownerType,
        paymentInvoice: selectedInvoice
      },
      width: '300px',
      height: '270px'
    });

    dialogRef
      .afterClosed()
      .takeUntil(this.onDestroy)
      .subscribe(closed => {
        if (closed) {
          this.paymentService
            .getInvoices(this.ownerId, this.ownerType)
            .takeUntil(this.onDestroy)
            .subscribe(invoices => {
              this.invoiceDtos = invoices;
              this.changed.emit(this.invoiceDtos);
            }, console.log);
          }
      });
  }

  private onEditChargedPaymentInvoiceClick(row: PaymentInvoice): void {

    const config = new MatDialogConfig();
    config.disableClose = true;
    config.data = {
      ownerType: this.ownerType,
      PaymentInvoiceId: row.id
    };

    const dialogRef = this.dialog.open(EditChargedPaymentInvoiceDialogComponent, config);

          dialogRef.componentInstance.сhargedPaymentInvoiceEdited.subscribe(() => {
            this.paymentService
            .getInvoices(this.ownerId, this.ownerType)
            .takeUntil(this.onDestroy)
            .subscribe(invoices => {
              this.invoiceDtos = invoices;
              this.changed.emit(this.invoiceDtos);
            }, console.log);

            dialogRef.componentInstance.сhargedPaymentInvoiceEdited.unsubscribe();
          });
  }

  private onDeleteChargedPaymentInvoiceClick(row: PaymentInvoice): void {

    const config = new MatDialogConfig();
    config.disableClose = true;
    config.data = {
      PaymentInvoiceId: row.id,
      ownerType: this.ownerType
    };

    const dialogRef = this.dialog.open(DeleteChargedPaymentInvoiceDialogComponent, config);

          dialogRef.componentInstance.сhargedPaymentInvoiceDeleted.subscribe(() => {
            this.paymentService
            .getInvoices(this.ownerId, this.ownerType)
            .takeUntil(this.onDestroy)
            .subscribe(invoices => {
              this.invoiceDtos = invoices;
              this.changed.emit(this.invoiceDtos);
            }, console.log);

            dialogRef.componentInstance.сhargedPaymentInvoiceDeleted.unsubscribe();
          });
  }

  private onDeletePaymentInvoiceClick(row: PaymentInvoice): void {

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
                this.deletePaymentInvoice(row);
              }
          });
  }

  private deletePaymentInvoice(row: PaymentInvoice) {
    const requestDto = new DeletePaymentInvoiceDto();

    requestDto.OwnerType = this.ownerType;

    this.paymentService.deletePaymentInvoice(row, requestDto)
      .takeUntil(this.onDestroy)
      .subscribe((resposeDto: DeletePaymentInvoiceResponseDto) => {
        this.modalService
        .ok('OK', resposeDto.message)
        .subscribe(() => {
          this.paymentService
          .getInvoices(this.ownerId, this.ownerType)
          .takeUntil(this.onDestroy)
          .subscribe(invoices => {
            this.invoiceDtos = invoices;
            this.changed.emit(this.invoiceDtos);
          }, console.log);
        });
      });
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  onColumnsChange() {
    const dialogRef = this.dialog.open(ColumnConfigDialogComponent, {
      data: {
        configKey: columnsConfigKey,
        defaultConfig: defaultColumnsConfig
      },
      disableClose: true
    });

    dialogRef
      .afterClosed()
      .takeUntil(this.onDestroy)
      .subscribe(result => {
        this.generateColumns(result);
      });
  }

  private isPayingSubjectMissing(): boolean {
    const missing = !this.payingSubject;
    if (missing) {
      const errorMessage =
        'Невозможно выполнить действие, плательщик (контрагент) неизвестен!';
      console.error(
        `${errorMessage} Details: {ownerType:${this.ownerType}, ownerId:${
          this.ownerId
        }}`
      );
      this.snackBarHelper.error(errorMessage);
    }

    return missing;
  }

  private generateColumns(displayedColumns: string[]) {
    const newColumns: Config[] = [];
    displayedColumns.forEach(d => {
      const newColumn = this.defaultColumns.find(dc => dc.columnDef === d);
      if (!!newColumn) {
        newColumns.push(newColumn);
      }
    });
    this.columns = newColumns;
  }
}
