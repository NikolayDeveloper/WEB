import { AfterViewInit, Component, ElementRef, EventEmitter, OnDestroy, OnInit, Output, ViewChild, ChangeDetectorRef } from '@angular/core';
import { MatDialog, MatPaginator, MatSort } from '@angular/material';
import { Router, NavigationStart, RouterEvent } from '@angular/router';
import { ColumnConfig, ColumnConfigDialogComponent, ColumnConfigService } from 'app/modules/column-config';
import { saveAs } from 'file-saver';
import { Subject, Subscription } from 'rxjs/Rx';

import { ConfigService } from '../../../../core/index';
import { BaseDataSource } from '../../../../shared/base-data-source';
import { RequestSearchDto } from '../../../models/request-search-dto.model';
import { ResetableComponent } from '../../../models/resetable-component';
import { SearchType } from '../../../models/search-type.enum';
import { QueryParamsStorageService } from '../../../services/query-params-storage.service';
import { RequestSearchService } from '../../../services/request-search.service';
import { Config } from '../../../../shared/components/table/config.model';

const columnsConfigKey = 'request_search_columns_config';
const defaultColumnsConfig: ColumnConfig[] = [
  { field: 'statusNameRu', name: 'Статус заявки', enabled: true },
  { field: 'barcode', name: 'Штрихкод', enabled: true },
  { field: 'incomingNumber', name: 'Входящий номер', enabled: true },
  { field: 'protectionDocTypeNameRu', name: 'Тип ОД', enabled: true },
  { field: 'requestTypeNameRu', name: 'Тип заявки', enabled: true },
  { field: 'currentStageNameRu', name: 'Этап', enabled: true },
  { field: 'workflowDate', name: 'Дата этапа', enabled: true },
  { field: 'departmentNameRu', name: 'Штат', enabled: true },
  { field: 'userNameRu', name: 'Пользователь', enabled: true },
  { field: 'requestNum', name: 'Рег. номер', enabled: true },
  { field: 'requestDate', name: 'Дата подачи', enabled: true },
  { field: 'name', name: 'Наименование', enabled: true },
  { field: 'customerXin', name: 'ИИН/БИН контрагента', enabled: true },
  { field: 'customerNameRu', name: 'Контрагент', enabled: true },
  { field: 'customerAddress', name: 'Адрес контрагента', enabled: true },
  { field: 'customerCountryNameRu', name: 'Страна контрагента', enabled: true },
  { field: 'receiveTypeNameRu', name: 'Тип подачи', enabled: true },
];

@Component({
  selector: 'app-request-search-list',
  templateUrl: './request-search-list.component.html',
  styleUrls: ['./request-search-list.component.scss']
})
export class RequestSearchListComponent implements OnInit, OnDestroy, AfterViewInit, ResetableComponent {
  displayedColumns: string[];
  dataSource: BaseDataSource<RequestSearchDto> | null;
  queryParams: Subject<any[]>;

  get source() { return this.requestSearchService; }

  columns: Config[] = [
    new Config({ columnDef: 'statusNameRu', header: 'Статус заявки', class: 'width-200' }),
    new Config({ columnDef: 'barcode', header: 'Штрихкод', class: 'width-100' }),
    new Config({ columnDef: 'incomingNumber', header: 'Входящий номер', class: 'width-150' }),
    new Config({ columnDef: 'protectionDocTypeNameRu', header: 'Тип ОД', class: 'width-150' }),
    new Config({ columnDef: 'requestTypeNameRu', header: 'Тип заявки', class: 'width-150' }),
    new Config({ columnDef: 'currentStageNameRu', header: 'Этап', class: 'width-200' }),
    new Config({ columnDef: 'workflowDate', header: 'Дата этапа', class: 'width-200' }),
    new Config({ columnDef: 'departmentNameRu', header: 'Штат', class: 'width-200' }),
    new Config({ columnDef: 'userNameRu', header: 'Пользователь', class: 'width-100' }),
    new Config({ columnDef: 'requestNum', header: 'Рег. номер', class: 'width-200' }),
    new Config({ columnDef: 'requestDate', header: 'Дата подачи', class: 'width-200' }),
    new Config({ columnDef: 'name', header: 'Наименование', class: 'width-200' }),
    new Config({ columnDef: 'customerXin', header: 'ИИН/БИН контрагента', class: 'width-150' }),
    new Config({ columnDef: 'customerNameRu', header: 'Адресат', class: 'width-150' }),
    new Config({ columnDef: 'customerAddress', header: 'Адрес контрагента', class: 'width-150' }),
    new Config({ columnDef: 'customerCountryNameRu', header: 'Страна контрагента', class: 'width-150' }),
    new Config({ columnDef: 'receiveTypeNameRu', header: 'Тип подачи', class: 'width-150' }),
    new Config({ columnDef: 'id', header: 'ID', class: 'width-50' }),
  ];

  private onDestroy = new Subject();
  private subscriptions: Subscription[] = [];

  @Output() resultsLength = new EventEmitter<number>();

  constructor(
    private router: Router,
    private requestSearchService: RequestSearchService,
    private queryParamsStorageService: QueryParamsStorageService,
    private configService: ConfigService,
    private columnConfigService: ColumnConfigService,
    public dialog: MatDialog,
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

  ngOnInit(): void {
    this.displayedColumns = this.columnConfigService.get(columnsConfigKey, defaultColumnsConfig)
      .filter(cc => cc.enabled)
      .map(cc => cc.field);
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => {
      sub.unsubscribe();
    });
    this.onDestroy.next();
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

  onSelect(record: RequestSearchDto) {
    this.router.navigate(['requests', record.id]);
  }

  onResultsLengthChange(resultsLength: any) {
    this.resultsLength.emit(resultsLength);
  }

  reset(queryParams?: any[]) {
    this.queryParams.next(queryParams);
  }

  onExport() {
    const queryParams = this.queryParamsStorageService.get(SearchType.AdvancedRequest);
    queryParams.push({ key: 'excelFields', value: this.displayedColumns.join() });

    this.requestSearchService
      .getExcel(queryParams)
      .takeUntil(this.onDestroy)
      .subscribe(response => {
        saveAs(response.body, `Результаты поиска по заявкам ${new Date().toLocaleString()}.xlsx`);
      });
  }
}
