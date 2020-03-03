import { AfterViewInit, Component, ElementRef, EventEmitter, OnDestroy, OnInit, Output, ViewChild, ChangeDetectorRef } from '@angular/core';
import { MatDialog, MatPaginator, MatSort } from '@angular/material';
import { Router, RouterEvent, NavigationStart } from '@angular/router';
import { ColumnConfig, ColumnConfigDialogComponent, ColumnConfigService } from 'app/modules/column-config';
import { saveAs } from 'file-saver';
import { Subject, Subscription } from 'rxjs/Rx';

import { ConfigService } from '../../../../core/index';
import { BaseDataSource } from '../../../../shared/base-data-source';
import { ProtectionDocSearchDto } from '../../../models/protectiondoc-search-dto.model';
import { ResetableComponent } from '../../../models/resetable-component';
import { SearchType } from '../../../models/search-type.enum';
import { ProtectionDocSearchService } from '../../../services/protectiondoc-search.service';
import { QueryParamsStorageService } from '../../../services/query-params-storage.service';
import { Config } from '../../../../shared/components/table/config.model';

const columnsConfigKey = 'protectiondoc_search_columns_config';
const defaultColumnsConfig: ColumnConfig[] = [
  { field: 'statusNameRu', name: 'Статус ОД', enabled: true },
  { field: 'typeNameRu', name: 'Тип ОД', enabled: true },
  { field: 'currentStageNameRu', name: 'Этап', enabled: true },
  { field: 'workflowDate', name: 'Дата этапа', enabled: true },
  { field: 'publicDate', name: 'Дата публикации', enabled: true },
  { field: 'gosNumber', name: '№ ОД', enabled: true },
  { field: 'gosDate', name: 'Дата ОД', enabled: true },
  { field: 'name', name: 'Наименование', enabled: true },
  { field: 'validDate', name: 'Срок действия ОД', enabled: true },
  { field: 'customerXin', name: 'ИИН/БИН контрагента', enabled: true },
  { field: 'customerNameRu', name: 'Адесат', enabled: true },
  { field: 'customerAddress', name: 'Адрес контрагента', enabled: true },
  { field: 'customerCountryNameRu', name: 'Страна контрагента', enabled: true },
];

@Component({
  selector: 'app-protectiondoc-search-list',
  templateUrl: './protectiondoc-search-list.component.html',
  styleUrls: ['./protectiondoc-search-list.component.scss']
})
export class ProtectionDocSearchListComponent implements OnInit, OnDestroy, AfterViewInit, ResetableComponent {
  displayedColumns: string[];
  dataSource: BaseDataSource<ProtectionDocSearchDto> | null;
  queryParams: Subject<any[]>;

  get source() { return this.protectionDocSearchService; }

  columns: Config[] = [
    new Config({ columnDef: 'statusNameRu', header: 'Статус ОД', class: 'width-200' }),
    new Config({ columnDef: 'typeNameRu', header: 'Тип ОД', class: 'width-250' }),
    new Config({ columnDef: 'currentStageNameRu', header: 'Этап', class: 'width-200' }),
    new Config({ columnDef: 'workflowDate', header: 'Дата этапа', class: 'width-200' }),
    new Config({ columnDef: 'publicDate', header: 'Дата публикации', class: 'width-200' }),
    new Config({ columnDef: 'gosNumber', header: '№ ОД', class: 'width-100' }),
    new Config({ columnDef: 'gosDate', header: 'Дата ОД', class: 'width-200' }),
    new Config({ columnDef: 'name', header: 'Наименование', class: 'width-200' }),
    new Config({ columnDef: 'validDate', header: 'Срок действия ОД', class: 'width-200' }),
    new Config({ columnDef: 'customerXin', header: 'ИИН/БИН контрагента', class: 'width-150' }),
    new Config({ columnDef: 'customerNameRu', header: 'Контрагент', class: 'width-150' }),
    new Config({ columnDef: 'customerAddress', header: 'Адрес контрагента', class: 'width-150' }),
    new Config({ columnDef: 'customerCountryNameRu', header: 'Страна контрагента', class: 'width-150' }),
    new Config({ columnDef: 'id', header: 'ID', class: 'width-50' }),
  ];

  private onDestroy = new Subject();
  private subscriptions: Subscription[] = [];

  @Output() resultsLength = new EventEmitter<number>();

  constructor(
    private router: Router,
    private protectionDocSearchService: ProtectionDocSearchService,
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

  onSelect(record: ProtectionDocSearchDto) {
    this.router.navigate(['protectiondocs', record.id]);
  }

  reset(queryParams?: any[]) {
    this.queryParams.next(queryParams);
  }

  onExport() {
    const queryParams = this.queryParamsStorageService.get(SearchType.AdvancedProtectionDoc);
    queryParams.push({ key: 'excelFields', value: this.displayedColumns.join() });

    this.protectionDocSearchService
      .getExcel(queryParams)
      .takeUntil(this.onDestroy)
      .subscribe(response => {
        saveAs(response.body, `Результаты поиска по ОД ${new Date().toLocaleString()}.xlsx`);
      });
  }
}
