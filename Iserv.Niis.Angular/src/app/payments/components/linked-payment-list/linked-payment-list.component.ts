import 'rxjs/add/operator/takeUntil';
import { Component, Input, SimpleChanges, SimpleChange, OnChanges, OnInit, Inject } from '@angular/core';

import { Config } from '../../../shared/components/table/config.model';

import { MatDialog, MatDialogConfig, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { Subject } from 'rxjs';
import { ColumnConfig, ColumnConfigDialogComponent, ColumnConfigService } from '../../../modules/column-config';

import { PaymentListDto } from 'app/payments-journal/models/payment-list.dto';
import { toShortDateString, toFullDateString } from 'app/payments-journal/helpers/date-helpers';
import { PaymentService } from 'app/payments/payment.service';


const COLUMNS_CONFIG_KEY = 'payments_journal_payment_uses_columns_config';

@Component({
  selector: 'app-linked-payment-list',
  templateUrl: './linked-payment-list.component.html'
})
export class LinkedPaymentListComponent implements OnInit {
  @Input()  

  public paymentList: PaymentListDto[] = [];
  private onDestroy = new Subject();

  public displayedColumns: string[];

  public readonly columnsConfigs: ColumnConfig[] = [
    { field: 'id', name: 'ID', enabled: true },
    { field: 'payerName', name: 'Плательщик', enabled: true },
    { field: 'amount', name: 'Сумма', enabled: true },
    { field: 'remainder', name: 'Остаток', enabled: true },
    { field: 'distributed', name: 'Распределено', enabled: true },
    { field: 'returnedAmount', name: 'Возвращено', enabled: true },
    { field: 'blockedAmount', name: 'Заблокировано', enabled: true },
    { field: 'paymentPurpose', name: 'Назначение', enabled: true },
    { field: 'paymentDate', name: 'Дата платежа', enabled: true },
    { field: 'paymentNumber', name: 'Номер платежа', enabled: true },
    { field: 'payerXin', name: 'ИИН\\БИН плательщика', enabled: true },
    { field: 'payerRnn', name: 'РНН плательщика', enabled: true },
    { field: 'dateCreate', name: 'Дата создания записи', enabled: true },
    { field: 'paymentDocumentNumber', name: 'Номер документа 1С', enabled: true },
    { field: 'isAdvancePayment', name: 'Авансовый', enabled: true },
    { field: 'paymentStatusName', name: 'Статус платежа', enabled: true },
    { field: 'refundedEmployeeName', name: 'Сотрудник выполнивший возврат платежа', enabled: true },
    { field: 'refundDate', name: 'Дата возврата', enabled: true },
    { field: 'returnedReason', name: 'Причина возврата', enabled: true },
    { field: 'blockedEmployeeName', name: 'Сотрудник выполнивший блокирование платежа', enabled: true },
    { field: 'blockedDate', name: 'Дата блокирования', enabled: true },
    { field: 'blockedReason', name: 'Причина блокирования', enabled: true },
    { field: 'isForeignCurrency', name: 'Платёж в иностранной валюте', enabled: true },
    { field: 'currencyCode', name: 'Код валюты', enabled: true }
  ];

  public readonly columns: Config[] = [
    new Config({ columnDef: 'id', header: 'ID', class: 'width-50' }),
    new Config({ columnDef: 'payerName', header: 'Плательщик', class: 'width-300' }),
    new Config({ columnDef: 'amount', header: 'Сумма', class: 'width-100' }),
    new Config({ columnDef: 'remainder', header: 'Остаток', class: 'width-100' }),
    new Config({ columnDef: 'distributed', header: 'Распределено', class: 'width-100' }),
    new Config({ columnDef: 'returnedAmount', header: 'Возвращено', class: 'width-100' }),
    new Config({ columnDef: 'blockedAmount', header: 'Заблокировано', class: 'width-100' }),
    new Config({ columnDef: 'paymentPurpose', header: 'Назначение', class: 'width-300' }),
    new Config({
      columnDef: 'paymentDate',
      header: 'Дата платежа',
      class: 'width-125',
      format: (row: PaymentListDto) => toShortDateString(row.paymentDate)
    }),
    new Config({ columnDef: 'paymentNumber', header: 'Номер платежа', class: 'width-150' }),
    new Config({ columnDef: 'payerXin', header: 'ИИН\\БИН плательщика', class: 'width-125' }),
    new Config({ columnDef: 'payerRnn', header: 'РНН плательщика', class: 'width-125' }),
    new Config({
      columnDef: 'dateCreate',
      header: 'Дата создания записи',
      class: 'width-125',
      format: (row: PaymentListDto) => toFullDateString(row.dateCreate)
    }),
    new Config({ columnDef: 'paymentDocumentNumber', header: 'Номер документа 1С', class: 'width-150' }),
    new Config({ columnDef: 'isAdvancePayment', header: 'Авансовый', class: 'width-100' }),
    new Config({ columnDef: 'paymentStatusName', header: 'Статус платежа', class: 'width-150' }),
    new Config({ columnDef: 'refundedEmployeeName', header: 'Сотрудник выполнивший возврат платежа', class: 'width-300' }),
    new Config({
      columnDef: 'refundDate',
      header: 'Дата возврата',
      class: 'width-125',
      format: (row: PaymentListDto) => toFullDateString(row.refundDate)
    }),
    new Config({ columnDef: 'returnedReason', header: 'Причина возврата', class: 'width-300' }),
    new Config({ columnDef: 'blockedEmployeeName', header: 'Сотрудник выполнивший блокирование платежа', class: 'width-300' }),
    new Config({
      columnDef: 'blockedDate',
      header: 'Дата блокирования',
      class: 'width-125',
      format: (row: PaymentListDto) => toFullDateString(row.blockedDate)
    }),
    new Config({ columnDef: 'blockedReason', header: 'Причина блокирования', class: 'width-300' }),
    new Config({ columnDef: 'isForeignCurrency', header: 'Платёж в иностранной валюте', class: 'width-100' }),
    new Config({ columnDef: 'currencyCode', header: 'Код валюты', class: 'width-125' }),
  ];
  

  constructor(    
    public dialog: MatDialog,
    public dialogRef: MatDialogRef<LinkedPaymentListComponent>,
    private paymentService: PaymentService,        
    private columnConfigService: ColumnConfigService,
    @Inject(MAT_DIALOG_DATA) public data: any) {
  }

  ngOnInit() {  
    this.initializeColumns();
    this.loadData();
  }

  
  onCancelClick(): void {
    this.dialogRef.close(false);
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
    this.paymentService.getPaymentsByInvoiceId(this.data.paymentInvoice.id)
      .subscribe((result: PaymentListDto[]) => this.paymentList = result);
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
}
