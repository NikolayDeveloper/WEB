import { AfterViewInit, Component, ElementRef, EventEmitter, OnDestroy, OnInit, Output, ViewChild, ChangeDetectorRef } from '@angular/core';
import { MatDialog, MatPaginator, MatSort } from '@angular/material';
import { Router, RouterEvent, NavigationStart } from '@angular/router';
import { saveAs } from 'file-saver';
import { Subject, Subscription } from 'rxjs/Rx';
import { SearchType } from '../../../models/search-type.enum';

import { ColumnConfig, ColumnConfigDialogComponent, ColumnConfigService } from 'app/modules/column-config';
import { ConfigService } from '../../../../core/index';
import { BaseDataSource } from '../../../../shared/base-data-source';
import { ContractSearchDto } from '../../../models/contract-search-dto.model';
import { ResetableComponent } from '../../../models/resetable-component';
import { ContractSearchService } from '../../../services/contract-search.service';
import { QueryParamsStorageService } from '../../../services/query-params-storage.service';
import { Config } from '../../../../shared/components/table/config.model';

const columnsConfigKey = 'contract_search_columns_config';
const defaultColumnsConfig: ColumnConfig[] = [
  { field: 'statusNameRu', name: 'Статус договора', enabled: true },
  { field: 'contractTypeNameRu', name: 'Вид договора', enabled: true },
  { field: 'categoryNameRu', name: 'Категория', enabled: true },
  { field: 'currentStageNameRu', name: 'Этап', enabled: true },
  { field: 'workflowDate', name: 'Дата этапа', enabled: true },
  { field: 'departmentNameRu', name: 'Штат', enabled: true },
  { field: 'userNameRu', name: 'Пользователь', enabled: true },
  { field: 'applicationNum', name: 'Рег. номер заявления', enabled: true },
  { field: 'dateCreate', name: 'Дата подачи заявления', enabled: true },
  { field: 'contractNum', name: 'Рег. номер договора', enabled: true },
  { field: 'regDate', name: 'Дата регистрации', enabled: true },
  { field: 'protectionDocTypeNameRu', name: 'Тип ОД', enabled: true },
  { field: 'name', name: 'Предмет договора', enabled: true },
  { field: 'customerXin', name: 'ИИН/БИН контрагента', enabled: true },
  { field: 'customerNameRu', name: 'Адресат', enabled: true },
  { field: 'customerAddress', name: 'Адрес контрагента', enabled: true },
  { field: 'customerCountryNameRu', name: 'Страна контрагента', enabled: true },
  { field: 'registrationPlace', name: 'Место регистрации', enabled: true },
  { field: 'validDate', name: 'Срок действия договора', enabled: true },
];

@Component({
  selector: 'app-contract-search-list',
  templateUrl: './contract-search-list.component.html',
  styleUrls: ['./contract-search-list.component.scss']
})
export class ContractSearchListComponent implements OnInit, OnDestroy, AfterViewInit, ResetableComponent {
  displayedColumns: string[];
  dataSource: BaseDataSource<ContractSearchDto> | null;
  queryParams: Subject<any[]>;

  get source() { return this.contractSearchService; }

  columns: Config[] = [
    new Config({ columnDef: 'statusNameRu', header: 'Статус договора', class: 'width-200' }),
    new Config({ columnDef: 'contractTypeNameRu', header: 'Вид договора', class: 'width-200' }),
    new Config({ columnDef: 'categoryNameRu', header: 'Категория', class: 'width-300' }),
    new Config({ columnDef: 'currentStageNameRu', header: 'Этап', class: 'width-200' }),
    new Config({ columnDef: 'workflowDate', header: 'Дата этапа', class: 'width-200' }),
    new Config({ columnDef: 'departmentNameRu', header: 'Штат', class: 'width-200' }),
    new Config({ columnDef: 'userNameRu', header: 'Пользователь', class: 'width-100' }),
    new Config({ columnDef: 'applicationNum', header: 'Рег. номер заявления', class: 'width-200' }),
    new Config({ columnDef: 'dateCreate', header: 'Дата подачи заявления', class: 'width-200' }),
    new Config({ columnDef: 'contractNum', header: 'Рег. номер договора', class: 'width-200' }),
    new Config({ columnDef: 'regDate', header: 'Дата регистрации', class: 'width-200' }),
    new Config({ columnDef: 'protectionDocTypeNameRu', header: 'Тип ОД', class: 'width-150' }),
    new Config({ columnDef: 'name', header: 'Предмет договора', class: 'width-200' }),
    new Config({ columnDef: 'customerXin', header: 'ИИН/БИН контрагента', class: 'width-150' }),
    new Config({ columnDef: 'customerNameRu', header: 'Контрагент', class: 'width-150' }),
    new Config({ columnDef: 'customerAddress', header: 'Адрес контрагента', class: 'width-150' }),
    new Config({ columnDef: 'customerCountryNameRu', header: 'Страна контрагента', class: 'width-150' }),
    new Config({ columnDef: 'registrationPlace', header: 'Место регистрации', class: 'width-200' }),
    new Config({ columnDef: 'validDate', header: 'Срок действия договора', class: 'width-200' }),
    new Config({ columnDef: 'id', header: 'ID', class: 'width-50' }),
  ];

  private onDestroy = new Subject();
  private subscriptions: Subscription[] = [];

  @Output() resultsLength = new EventEmitter<number>();

  constructor(
    private router: Router,
    private contractSearchService: ContractSearchService,
    private queryParamsStorageService: QueryParamsStorageService,
    private columnConfigService: ColumnConfigService,
    public dialog: MatDialog,
    private configService: ConfigService,
    private changeDetector: ChangeDetectorRef) {
    this.subscriptions.push(
      this.router.events.subscribe((event: RouterEvent) => {
        if (event instanceof NavigationStart) {
          if (!event.url.includes('search')) {
            this.columnConfigService.save(columnsConfigKey, defaultColumnsConfig);
          }
        }
      }));
    this.queryParams = new Subject<any[]>();
  }

  ngOnInit() {
    this.displayedColumns = this.columnConfigService.get(columnsConfigKey, defaultColumnsConfig)
      .filter(cc => cc.enabled)
      .map(cc => cc.field);
  }

  ngAfterViewInit(): void {
    this.changeDetector.detectChanges();
  }

  onColumnsChange() {
    const dialogRef = this.dialog.open(ColumnConfigDialogComponent, {
      data: {
        configKey: columnsConfigKey,
        defaultConfig: defaultColumnsConfig
      },
      disableClose: true
    });

    dialogRef.afterClosed()
      .takeUntil(this.onDestroy)
      .subscribe(result => {
        this.displayedColumns = result;
      });
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => {
      sub.unsubscribe();
    });
    this.onDestroy.next();
  }

  onSelect(record: ContractSearchDto) {
    this.router.navigate(['contracts', record.id]);
  }

  reset(queryParams?: any[]) {
    this.queryParams.next(queryParams);
  }

  onExport() {
    const queryParams = this.queryParamsStorageService.get(SearchType.AdvancedContract);
    queryParams.push({ key: 'excelFields', value: this.displayedColumns.join() });

    this.contractSearchService
      .getExcel(queryParams)
      .takeUntil(this.onDestroy)
      .subscribe(response => {
        saveAs(response.body, `Результаты поиска по договорам ${new Date().toLocaleString()}.xlsx`);
      });
  }
}
