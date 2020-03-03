import 'rxjs/add/operator/takeUntil';
import {
  AfterViewInit,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
  OnDestroy,
  ElementRef,
  ViewChild, OnInit
} from '@angular/core';
import { PaymentsService } from '../../services/payments.service';
import { QueryParam } from '../../helpers/query-param';
import { PaymentsSearchParametersDto } from '../../models/payments-search-parameters.dto';
import { Config } from '../../../shared/components/table/config.model';
import { Subject } from 'rxjs/Subject';
import { PaymentListDto } from '../../models/payment-list.dto';
import { toFullDateString, toShortDateString } from '../../helpers/date-helpers';
import { Subscription } from 'rxjs';
import { PaymentsSharedService } from 'app/payments-journal/helpers/payments-shared.service';
import { TableComponent } from 'app/shared/components/table/table.component';
import { ColumnConfig, ColumnConfigDialogComponent, ColumnConfigService } from '../../../modules/column-config';
import { MatDialog } from '@angular/material';
import { saveAs } from 'file-saver';

const COLUMNS_CONFIG_KEY = 'payments_journal_payments_columns_config';

@Component({
  selector: 'app-payments-list',
  templateUrl: './payments-list.component.html'
})
export class PaymentsListComponent implements OnInit, AfterViewInit, OnChanges, OnDestroy {
  @Input()
  public searchParams: PaymentsSearchParametersDto;

  @Output()
  public selectedPaymentIdChanged: EventEmitter<number> = new EventEmitter();

  private paymentUseEditedSubscription: Subscription;
  private paymentUseDeletedSubscription: Subscription;

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

  public readonly reset = new Subject();
  private selectedPaymentItem: PaymentListDto;
  @ViewChild(TableComponent) tableReq: TableComponent;

  constructor(
    public paymentsService: PaymentsService,
    private paymentsSharedService: PaymentsSharedService,
    public dialog: MatDialog,
    private columnConfigService: ColumnConfigService) {
    this.paymentUseEditedSubscription = paymentsSharedService.paymentUseEdited$
      .subscribe(() => {
        this.loadData();
      });
    this.paymentUseEditedSubscription = paymentsSharedService.paymentUseDeleted$
      .subscribe(() => {
        this.loadData();
      });
  }

  public ngOnInit(): void {
    this.initializeColumns();
  }

  public ngAfterViewInit(): void {
    this.selectedPaymentItem = null;
    this.loadData();
  }

  public ngOnChanges(changes: SimpleChanges): void {
    this.selectedPaymentItem = null;
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

  public onSelect(item: PaymentListDto): void {
    this.selectedPaymentItem = item;

    setTimeout(() => {
      this.selectedPaymentIdChanged.emit(item != null ? item.id : null);
    });
  }

  public onResultsLength(): void {
    this.actualizeSelectedItem();
  }

  public loadData(): void {
    this.reset.next(this.getSearchQueryParams());
  }

  private actualizeSelectedItem(): void {
    let selectedItem: PaymentListDto;

    if (this.selectedPaymentItem) {
      selectedItem = (this.tableReq.calculatedDataSource.data as PaymentListDto[])
        .filter(x => x.id === this.selectedPaymentItem.id)[0];
    } else {
      selectedItem = null;
    }

    this.tableReq.onSelect(selectedItem);
  }

  private getSearchQueryParams(): QueryParam[] {
    if (!this.searchParams) {
      this.searchParams = new PaymentsSearchParametersDto();
    }

    return this.searchParams.getQueryParams();
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
    //this.paymentUseDeletedSubscription.unsubscribe();
    //this.paymentUseEditedSubscription.unsubscribe();

  }

  onExport(columns: string[]): void {
    const queryParams = [
      {
        key: 'excelFields',
        value: (columns && columns.length) ? columns.join() : this.displayedColumns.join()
      },
      ...this.searchParams.getQueryParams()
    ];

    this.paymentsService
      .getExcel(queryParams)
      .subscribe((data) => {
        saveAs(data.body, 'Журнал платежей.xlsx');
      });
  }
}
