import { getDocumentTypeRoute } from '../../../../materials/models/materials.model';
import { AfterViewInit, Component, ElementRef, EventEmitter, OnDestroy, OnInit, Output, ViewChild, ChangeDetectorRef } from '@angular/core';
import { MatDialog, MatPaginator, MatSort } from '@angular/material';
import { Router, RouterEvent, NavigationStart } from '@angular/router';
import { ColumnConfig, ColumnConfigDialogComponent, ColumnConfigService } from 'app/modules/column-config';
import { saveAs } from 'file-saver';
import { Subject, Subscription } from 'rxjs/Rx';

import { ConfigService } from '../../../../core/index';
import { BaseDataSource } from '../../../../shared/base-data-source';
import { DocumentSearchDto } from '../../../models/document-search-dto.model';
import { ResetableComponent } from '../../../models/resetable-component';
import { SearchType } from '../../../models/search-type.enum';
import { DocumentSearchService } from '../../../services/document-search.service';
import { QueryParamsStorageService } from '../../../services/query-params-storage.service';
import { Config } from '../../../../shared/components/table/config.model';

const columnsConfigKey = 'document_search_columns_config';
const defaultColumnsConfig: ColumnConfig[] = [
  { field: 'id', name: 'ID', enabled: true },
  { field: 'barcode', name: 'Штрих код', enabled: true },
  { field: 'description', name: 'Описание', enabled: true },
  { field: 'typeNameRu', name: 'Класс', enabled: true },
  { field: 'departmentNameRu', name: 'Штат', enabled: true },
  { field: 'userNameRu', name: 'Пользователь', enabled: true },
  { field: 'documentNum', name: '№ документа', enabled: true },
  { field: 'documentDate', name: 'Дата документа', enabled: true },
  { field: 'receiveTypeNameRu', name: 'Тип подачи', enabled: true },
  { field: 'customerXin', name: 'ИИН/БИН контрагента', enabled: true },
  { field: 'customerNameRu', name: 'Адресат', enabled: true },
  { field: 'customerAddress', name: 'Адрес контрагента', enabled: true },
  { field: 'customerCountryNameRu', name: 'Страна контрагента', enabled: true },
  { field: 'outgoingNumber', name: 'Исх. номер (контрагента)', enabled: true },
  { field: 'sendingDate', name: 'Дата документа (контрагента)', enabled: true },
];

@Component({
  selector: 'app-document-search-list',
  templateUrl: './document-search-list.component.html',
  styleUrls: ['./document-search-list.component.scss']
})
export class DocumentSearchListComponent implements OnInit, OnDestroy, AfterViewInit, ResetableComponent {
  displayedColumns: string[];
  dataSource: BaseDataSource<DocumentSearchDto> | null;
  queryParams: Subject<any[]>;

  get source() { return this.documentSearchService; }

  columns: Config[] = [
    new Config({ columnDef: 'id', header: 'ID', class: 'width-75' }),
    new Config({ columnDef: 'barcode', header: 'Штрих код', class: 'width-100' }),
    new Config({ columnDef: 'typeNameRu', header: 'Класс', class: 'width-250' }),
    new Config({ columnDef: 'departmentNameRu', header: 'Штат', class: 'width-200' }),
    new Config({ columnDef: 'userNameRu', header: 'Пользователь', class: 'width-100' }),
    new Config({ columnDef: 'documentNum', header: '№ документа', class: 'width-100' }),
    new Config({ columnDef: 'documentDate', header: 'Дата документа', class: 'width-200' }),
    new Config({ columnDef: 'description', header: 'Описание', class: 'width-200' }),
    new Config({ columnDef: 'receiveTypeNameRu', header: 'Тип подачи', class: 'width-150' }),
    new Config({ columnDef: 'customerXin', header: 'ИИН/БИН контрагента', class: 'width-150' }),
    new Config({ columnDef: 'customerNameRu', header: 'Контрагент', class: 'width-150' }),
    new Config({ columnDef: 'customerAddress', header: 'Адрес контрагента', class: 'width-150' }),
    new Config({ columnDef: 'customerCountryNameRu', header: 'Страна контрагента', class: 'width-150' }),
    new Config({ columnDef: 'outgoingNumber', header: 'Исх. номер (контрагента)', class: 'width-150' }),
    new Config({ columnDef: 'sendingDate', header: 'Дата документа (контрагента)', class: 'width-200' }),
    new Config({ columnDef: 'documentType', header: 'Тип документа', class: 'width-100' }),
  ];

  private onDestroy = new Subject();
  private subscriptions: Subscription[] = [];

  @Output() resultsLength = new EventEmitter<number>();

  constructor(
    private router: Router,
    private documentSearchService: DocumentSearchService,
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

  onSelect(record: DocumentSearchDto) {
    this.router.navigate([getDocumentTypeRoute(record.documentType), record.id]);
  }

  reset(queryParams?: any[]) {
    this.queryParams.next(queryParams);
  }

  onExport() {
    const queryParams = this.queryParamsStorageService.get(SearchType.AdvancedDocument);
    queryParams.push({ key: 'excelFields', value: this.displayedColumns.join() });

    this.documentSearchService
      .getExcel(queryParams)
      .takeUntil(this.onDestroy)
      .subscribe(response => {
        saveAs(response.body, `Результаты поиска по материалам ${new Date().toLocaleString()}.xlsx`);
      });
  }
}
