import 'rxjs/add/operator/takeUntil';
import { Component, Input, SimpleChanges, SimpleChange, OnChanges, OnInit } from '@angular/core';
import { PaymentUsesService } from '../../services/payment-uses.service';
import { Config } from '../../../shared/components/table/config.model';
import { toShortDateString, FromUTCDateToFullDateString } from '../../helpers/date-helpers';
import { PaymentUseListDto } from '../../models/payment-use-list.dto';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { DeletePaymentUseDialogComponent } from '../delete-payment-use-dialog/delete-payment-use-dialog.component';
import { EditPaymentUseDialogComponent } from '../edit-payment-use-dialog/edit-payment-use-dialog.component';
import { forkJoin } from 'rxjs/observable/forkJoin';
import { ModalService } from 'app/shared/services/modal.service';
import { Subject } from 'rxjs';
import { PaymentsSharedService } from 'app/payments-journal/helpers/payments-shared.service';
import { ColumnConfig, ColumnConfigDialogComponent, ColumnConfigService } from '../../../modules/column-config';
import { PaymentsService } from 'app/payments-journal/services/payments.service';
import { saveAs } from 'file-saver';

const COLUMNS_CONFIG_KEY = 'payments_journal_payment_uses_columns_config';

@Component({
  selector: 'app-payment-uses-list',
  templateUrl: './payment-uses-list.component.html'
})
export class PaymentUsesListComponent implements OnInit, OnChanges {
  @Input()
  public paymentId: number;

  public data: PaymentUseListDto[] = [];
  private onDestroy = new Subject();

  public displayedColumns: string[];

  public readonly columnsConfigs: ColumnConfig[] = [
    { field: 'id', name: 'ID', enabled: true },
    { field: 'payerName', name: 'Наименование плательщика', enabled: true },
    { field: 'payerXin', name: 'ИИН\\БИН плательщика', enabled: true },
    { field: 'payerRnn', name: 'РНН плательщика', enabled: true },
    { field: 'serviceCode', name: 'Код услуги', enabled: true },
    { field: 'serviceName', name: 'Наименование услуги', enabled: true },
    { field: 'protectionDocNumber', name: 'Номер ОД', enabled: true },
    { field: 'requestNumber', name: 'Номер заявки', enabled: true },
    { field: 'protectionDocTypeName', name: 'Вид ОПС', enabled: true },
    { field: 'protectionDocName', name: 'Наименование ОПС', enabled: true },
    { field: 'contractNumber', name: 'Номер Договора', enabled: true },
    { field: 'contractTypeName', name: 'Вид заявления на регистрацию договора', enabled: true },
    { field: 'amount', name: 'Сумма', enabled: true },
    { field: 'description', name: 'Описание', enabled: true },
    { field: 'paymentUseDate', name: 'Дата зачтения оплаты', enabled: true },
    { field: 'employeeCheckoutPaymentName', name: 'Пользователь (ЗО)', enabled: true },
    { field: 'issuingPaymentDate', name: 'Дата выставления оплаты услуги', enabled: true },
    { field: 'dateOfPayment', name: 'Дата списания оплаты', enabled: true },
    { field: 'employeeWriteOffPaymentName', name: 'Пользователь (СО)', enabled: true },
    { field: 'editClearedPaymentReason', name: 'Причина изменения зачтённой суммы,', enabled: true },
    { field: 'editClearedPaymentDate', name: 'Дата изменения зачтённой суммы', enabled: true },
    { field: 'editClearedPaymentEmployeeName', name: 'Сотрудник изменивший зачтённую сумму', enabled: true },
    { field: 'deletionClearedPaymentDate', name: 'Дата и время удаления зачтённой оплаты', enabled: true },
    { field: 'deletionClearedPaymentEmployeeName', name: 'Сотрудник, выполнивший удаление зачтённой оплаты', enabled: true },
    { field: 'deletionClearedPaymentReason', name: 'Причина удаления зачтённой оплаты', enabled: true },
    { field: 'editPaymentUse', name: 'Редактировать зачтённую оплату', enabled: true },
    { field: 'deletePaymentUse', name: 'Удалить зачтённую оплату', enabled: true },
    { field: 'loadPaymentUse', name: 'Загрузить зачтённую оплату', enabled: true }
  ];

  public readonly columns: Config[] = [
    new Config({ columnDef: 'id', header: 'ID', class: 'width-50' }),
    new Config({ columnDef: 'payerName', header: 'Наименование плательщика', class: 'width-300' }),
    new Config({ columnDef: 'payerXin', header: 'ИИН\\БИН плательщика', class: 'width-125' }),
    new Config({ columnDef: 'payerRnn', header: 'РНН плательщика', class: 'width-125' }),
    new Config({ columnDef: 'serviceCode', header: 'Код услуги', class: 'width-125' }),
    new Config({ columnDef: 'serviceName', header: 'Наименование услуги', class: 'width-300' }),
    new Config({ columnDef: 'protectionDocNumber', header: 'Номер ОД', class: 'width-125' }),
    new Config({ columnDef: 'requestNumber', header: 'Номер заявки', class: 'width-125' }),
    new Config({ columnDef: 'protectionDocTypeName', header: 'Вид ОПС', class: 'width-300' }),
    new Config({ columnDef: 'protectionDocName', header: 'Наименование ОПС', class: 'width-300' }),
    new Config({ columnDef: 'contractNumber', header: 'Номер Договора', class: 'width-125' }),
    new Config({ columnDef: 'contractTypeName', header: 'Вид заявления на регистрацию договора', class: 'width-300' }),
    new Config({ columnDef: 'amount', header: 'Сумма', class: 'width-100' }),
    new Config({ columnDef: 'description', header: 'Описание', class: 'width-300' }),
    new Config({
      columnDef: 'paymentUseDate',
      header: 'Дата зачтения оплаты',
      class: 'width-125',
      format: (row: PaymentUseListDto) => FromUTCDateToFullDateString(row.paymentUseDate)
    }),

    new Config({ columnDef: 'employeeCheckoutPaymentName', header: 'Пользователь (ЗО)', class: 'width-300' }),
    new Config({
      columnDef: 'issuingPaymentDate',
      header: 'Дата выставления оплаты услуги',
      class: 'width-125',
      format: (row: PaymentUseListDto) => FromUTCDateToFullDateString(row.issuingPaymentDate)
    }),
    new Config({
      columnDef: 'dateOfPayment',
      header: 'Дата списания оплаты',
      class: 'width-125',
      format: (row: PaymentUseListDto) => toShortDateString(row.dateOfPayment)
    }),
    new Config({ columnDef: 'employeeWriteOffPaymentName', header: 'Пользователь (СО)', class: 'width-300' }),
    new Config({ columnDef: 'editClearedPaymentReason', header: 'Причина изменения зачтённой суммы,', class: 'width-300' }),
    new Config({
      columnDef: 'editClearedPaymentDate',
      header: 'Дата изменения зачтённой суммы',
      class: 'width-125',
      format: (row: PaymentUseListDto) => FromUTCDateToFullDateString(row.editClearedPaymentDate)
    }),
    new Config({
      columnDef: 'editClearedPaymentEmployeeName',
      header: 'Сотрудник изменивший зачтённую сумму',
      class: 'width-300'
    }),
    new Config({
      columnDef: 'deletionClearedPaymentDate',
      header: 'Дата и время удаления зачтённой оплаты',
      class: 'width-150',
      format: (row: PaymentUseListDto) => FromUTCDateToFullDateString(row.deletionClearedPaymentDate)
    }),
    new Config({
      columnDef: 'deletionClearedPaymentEmployeeName',
      header: 'Сотрудник, выполнивший удаление зачтённой оплаты',
      class: 'width-300'
    }),
    new Config({
      columnDef: 'deletionClearedPaymentReason',
      header: 'Причина удаления зачтённой оплаты',
      class: 'width-300'
    }),
    new Config({
      columnDef: 'editPaymentUse',
      header: 'Редактировать зачтённую оплату',
      class: 'width-25 sticky',
      icon: 'edit',
      click: row => this.onEditPaymentUseClick.call(this, row),
      disable: (row: PaymentUseListDto) => row.isDeleted
    }),
    new Config({
      columnDef: 'deletePaymentUse',
      header: 'Удалить зачтённую оплату',
      class: 'width-25 sticky',
      icon: 'delete',
      click: row => this.onDeletePaymentUseClick.call(this, row),
      disable: (row: PaymentUseListDto) => row.isDeleted
    }),
    new Config({
      columnDef: 'loadPaymentUse',
      header: 'Загрузить зачтённую оплату',
      class: 'width-25 sticky',
      icon: 'get_app',
      click: row => {
        this.download(row.id);
      },
      disable: (row: PaymentUseListDto) => row.isDeleted
    })
  ];

  constructor(
    public dialog: MatDialog,
    public paymentUsesService: PaymentUsesService,
    private modalService: ModalService,
    private paymentsSharedService: PaymentsSharedService,
    private columnConfigService: ColumnConfigService,
    private paymentsService: PaymentsService) {
  }

  public ngOnInit(): void {
    this.initializeColumns();
  }

  public ngOnChanges(changes: SimpleChanges) {
    this.loadData();
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
    this.paymentUsesService.getByPaymentId(this.paymentId)
      .subscribe((result: PaymentUseListDto[]) => this.data = result);
  }

  private onEditPaymentUseClick(row: PaymentUseListDto): void {
    const isInvoiceCharged = this.paymentUsesService.isInvoiceCharged(row.id);
    const getPaymentUseForEdit = this.paymentUsesService.getPaymentUseForEdit(row.id);

    const requests = forkJoin([isInvoiceCharged, getPaymentUseForEdit]);
    requests.takeUntil(this.onDestroy)
      .subscribe(([isInvoiceCharged, paymentUseForEditResponseDto]) => {
        if (isInvoiceCharged) {
          this.modalService.ok('Ошибка редактирования оплаты.', 'Оплата списана. Редактирование суммы оплаты невозможно.');
        } else {
          const config = new MatDialogConfig();
          config.width = '350px';
          config.disableClose = true;
          config.data = {
            PaymentUseForEdit: paymentUseForEditResponseDto
          };

          const dialogRef = this.dialog.open(EditPaymentUseDialogComponent, config);
          dialogRef.componentInstance.paymentUseEdited.subscribe(() => {
            this.paymentsSharedService.paymentUseEdited();
            dialogRef.componentInstance.paymentUseEdited.unsubscribe();
          });
        }
      });
  }

  private onDeletePaymentUseClick(row: PaymentUseListDto): void {
    this.paymentUsesService.isInvoiceCharged(row.id)
      .takeUntil(this.onDestroy)
      .subscribe((isInvoiceCharged: boolean) => {
        if (isInvoiceCharged) {
          this.modalService.ok('Ошибка удаления оплаты.', 'Оплата списана. Удаление оплаты невозможно.');
        } else {
          const config = new MatDialogConfig();
          config.width = '350px';
          config.disableClose = true;
          config.data = {
            PaymentUseId: row.id
          };

          const dialogRef = this.dialog.open(DeletePaymentUseDialogComponent, config);
          dialogRef.componentInstance.paymentUseDeleted.subscribe(() => {
            this.paymentsSharedService.paymentUseDeleted();
            dialogRef.componentInstance.paymentUseDeleted.unsubscribe();
          });
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

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  onExport(columns: string[]): void {
    const queryParams = [
      {
        key: 'excelFields',
        value: (columns && columns.length) ? columns.join() : this.displayedColumns.join()
      }
    ];

    this.paymentsService
      .getExcel(queryParams, this.paymentId)
      .subscribe((data) => {
        saveAs(data.body, `Журнал распределения платежа № ${this.paymentId}.xlsx`);
      });
  }

  download(id) {
    this.paymentUsesService
      .getStatementFromBank(id)
      .subscribe((data) => {
        saveAs(data.body, `Выписка из банка.pdf`);
      });
  }
}
