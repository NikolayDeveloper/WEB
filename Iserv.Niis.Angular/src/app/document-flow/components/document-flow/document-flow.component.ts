import {AfterViewInit, ChangeDetectorRef, Component, ElementRef, HostListener, OnDestroy, OnInit, ViewChild} from '@angular/core';

import {Config} from '../../../shared/components/table/config.model';
import {DocumentFlowService} from 'app/document-flow/services/document-flow.service';
import {Operators} from 'app/shared/filter/operators';
import {OwnerType} from 'app/shared/services/models/owner-type.enum';
import {RequestService} from '../../../requests/request.service';
import {Subject} from 'rxjs/Subject';
import {moment} from '../../../shared/shared.module';
import {MaterialsService} from '../../../materials/services/materials.service';
import {ProtectionDocsService} from '../../../protection-docs/protection-docs.service';
import {DocumentsService} from '../../../shared/services/documents.service';
import {DictionaryService} from '../../../shared/services/dictionary.service';
import {SubjectsService} from '../../../subjects/services/subjects.service';
import {BibliographicDataService} from '../../../bibliographic-data/bibliographic-data.service';
import {ColumnConfig, ColumnConfigDialogComponent, ColumnConfigService} from '../../../modules/column-config';
import {Router} from '@angular/router';
import {MatDialog} from '@angular/material';
import {CreateDocumentDialogComponent} from '../../../journal/components/create-document-dialog/create-document-dialog.component';
import { ImportDocumentDialogComponent } from '../../../journal/components/import-document-dialog/import-document-dialog.component';
import { IncomingMaterialsService } from '../../../materials/incoming/incoming-materials.service';
import { getDocumentTypeName } from '../../../materials/models/materials.model';
import { ContractService } from '../../../contracts/contract.service';
import { RequestSearchService} from '../../../search/services/request-search.service';
import { WorkflowService } from '../../../shared/services/workflow.service';
import {SelectUserDialogComponent} from '../../../journal/components/select-user-dialog/select-user-dialog.component';
import { JournalService } from '../../../journal/journal.service';

const defaultMaterialColumns: ColumnConfig[] = [
  { field: 'barcode', name: 'Штрихкод', enabled: true},
  { field: 'documentType', name: 'Тип корреспонденции', enabled: true},
  { field: 'incomingNumber', name: 'Входящий номер', enabled: true},
  { field: 'outgoingNumber', name: 'Исходящий номер', enabled: true},
  { field: 'typeNameRu', name: 'Тип документа', enabled: true},
  { field: 'dateCreate', name: 'Дата Создания', enabled: true},
  { field: 'currentStageUser', name: 'Пользователь', enabled: true},
  { field: 'commentDtos', name: 'Комментарии', enabled: true},
  { field: 'statusValue', name: 'Статус', enabled: true},
  { field: 'currentStageValue', name: 'Этап', enabled: true},

];
const defaultColumns: ColumnConfig[] = [
  { field: 'protectionDocTypeValue', name: 'Вид ОПС', enabled: true},
  { field: 'barcode', name: 'Штрихкод', enabled: true},
  { field: 'incomingNumber', name: 'Входящий номер Заявки ОД', enabled: true},
  { field: 'dateCreate', name: 'Дата Создания', enabled: true},
  { field: 'currentStageDate', name: 'Дата перехода на этап', enabled: true},
  { field: 'currentStageValue', name: 'Этап', enabled: true},
  { field: 'receiveTypeNameRu', name: 'Тип подачи', enabled: true},
  { field: 'reviewDaysAll', name: 'Срок рассмотрения (общий)', enabled: true},
  { field: 'reviewDaysStage', name: 'Срок рассмотрения (этап)', enabled: true},
  { field: 'regNumber', name: 'Регистрационный номер', enabled: true},
  { field: 'statusValue', name: 'Статус', enabled: true},
  { field: 'description', name: 'Описание', enabled: true},
  { field: 'documentOwner', name: 'Владелец', enabled: true},
  { field: 'addressee', name: 'Заявитель', enabled: true},
  { field: 'declarant', name: 'Заявитель', enabled: true},
  { field: 'patentAttorney', name: 'Патентный поверенный', enabled: true},
  { field: 'confidant', name: 'Доверенное лицо', enabled: true},
  { field: 'correspondence', name: 'Адресат для переписки', enabled: true},
  { field: 'author', name: 'Автор', enabled: true},
  { field: 'nameRu', name: 'Наименование (на 3х языках)', enabled: true},
  { field: 'numberBulletin', name: 'МКТУ (номер класса)', enabled: true},
  { field: 'icgs', name: 'МКИЭТЗ (графический код)', enabled: true},
  { field: 'icis', name: 'МПК Международная Патентная Классификация', enabled: true},
  { field: 'icfem', name: 'МКПО Международная Классификация Промышленных Образцов', enabled: true},
  { field: 'transliteration', name: 'Транслитерация', enabled: true},
  { field: 'user', name: 'Пользователь', enabled: true},
  { field: 'executor', name: 'Исполнитель', enabled: true},
];

export enum SelectionMode {
  Including = 0,
  Except = 1
}

@Component({
  selector: 'app-document-flow',
  templateUrl: './document-flow.component.html',
  styleUrls: ['./document-flow.component.scss'],
  providers: [
    SubjectsService,
    BibliographicDataService,
    RequestSearchService
  ]
})
export class DocumentFlowComponent implements OnInit, AfterViewInit, OnDestroy {
  private onDestroy = new Subject();
  public materialsDataSource: any[];
  public docPrev;
  public selectionMode = SelectionMode.Including;
  public resetParams = new Subject<any[]>();
  public columns: Config[] = [
    new Config({ columnDef: 'protectionDocTypeValue', header: 'Вид ОПС', class: 'width-250'}),
    new Config({ columnDef: 'barcode', header: 'Штрихкод', class: 'width-150'}),
    new Config({ columnDef: 'incomingNumber', header: 'Входящий номер Заявки ОД', class: 'width-150'}),
    new Config({ columnDef: 'dateCreate', header: 'Дата Создания', format: (row) => moment(row.dateCreate).utc().format('DD-MM-YYYY'), class: 'width-150' }),
    new Config({ columnDef: 'currentStageDate', header: 'Дата перехода на этап', format: (row) => moment(row.currentStageDate).utc().format('DD-MM-YYYY'), class: 'width-150' }),
    new Config({ columnDef: 'currentStageValue', header: 'Этап', class: 'width-250'}),
    new Config({ columnDef: 'receiveTypeNameRu', header: 'Тип подачи', class: 'width-150'}),
    new Config({ columnDef: 'statusValue', header: 'Статус', class: 'width-150'}),
    new Config({ columnDef: 'description', header: 'Описание', class: 'width-250' }),
    new Config({ columnDef: 'reviewDaysAll', header: 'Срок рассмотрения (общий)', class: 'width-150' }),
    new Config({ columnDef: 'reviewDaysStage', header: 'Срок рассмотрения (этап)', class: 'width-150' }),
    new Config({ columnDef: 'regNumber', header: 'Регистрационный номер', class: 'width-150'}),
    new Config({ columnDef: 'addressee', header: 'Адресат', class: 'width-200' }),
    new Config({ columnDef: 'declarant', header: 'Заявитель', class: 'width-150'}),
    new Config({ columnDef: 'documentOwner', header: 'Владелец', class: 'width-150'}),
    new Config({ columnDef: 'patentAttorney', header: 'Патентный поверенный', class: 'width-150'}),
    new Config({ columnDef: 'confidant', header: 'Доверенное лицо', class: 'width-150'}),
    new Config({ columnDef: 'correspondence', header: 'Адресат для переписки', class: 'width-150'}),
    new Config({ columnDef: 'author', header: 'Автор', class: 'width-150'}),
    new Config({ columnDef: 'nameRu', header: 'Наименование (на 3х языках)', class: 'width-250',
      format: (row) => `${row.nameRu || ''} ${row.nameKz || ''}  ${row.nameEn || ''}`}),
    new Config({ columnDef: 'numberBulletin', header: 'МКТУ (номер класса)', class: 'width-250'}),
    new Config({ columnDef: 'icgs', header: 'МКИЭТЗ (графический код)', class: 'width-250'}),
    new Config({ columnDef: 'icis', header: 'МПК Международная Патентная Классификация', class: 'width-350'}),
    new Config({ columnDef: 'icfem', header: 'МКПО Международная Классификация Промышленных Образцов', class: 'width-350'}),
    new Config({ columnDef: 'transliteration', header: 'Транслитерация', class: 'width-150'}),
    new Config({ columnDef: 'user', header: 'Пользователь', class: 'width-150'}),
    new Config({ columnDef: 'executor', header: 'Исполнитель', class: 'width-150'}),
  ];
  public materialColumns: Config[] = [
    new Config({ columnDef: 'barcode', header: 'Штрихкод', class: 'width-100'}),
    new Config({ columnDef: 'documentType', header: 'Тип корреспонденции', format: (row) => getDocumentTypeName(row.documentType), class: 'width-150'}),
    new Config({ columnDef: 'incomingNumber', header: 'Входящий номер', class: 'width-100'}),
    new Config({ columnDef: 'outgoingNumber', header: 'Исходящий номер', class: 'width-100'}),
    new Config({ columnDef: 'typeNameRu', header: 'Тип документа', class: 'width-200'}),
    new Config({ columnDef: 'dateCreate', header: 'Дата Создания', format: (row) => moment(row.dateCreate).utc().format('DD-MM-YYYY'),class: 'width-150'}),
    new Config({ columnDef: 'currentStageUser', header: 'Пользователь', class: 'width-150'}),
    new Config({ columnDef: 'commentDtos', header: 'Комментарии', format: (row) => {
        return row.commentDtos.map(comment => comment.comment).join(',');
     }, class: 'width-250'
    }),
    new Config({ columnDef: 'statusValue', header: 'Статус', class: 'width-150'}),
    new Config({ columnDef: 'creator', header: 'Инициатор', class: 'width-150'}),
    new Config({ columnDef: 'currentStageValue', header: 'Этап', class: 'width-150'}),
  ];
  public itemDetailsState;
  public ordersConfigKey = 'orders_columns_config';
  public materialsConfigKey = 'materials_columns_config';
  public defaultColumns: ColumnConfig[] = defaultColumns;
  public defaultMaterialColumns: ColumnConfig[] = defaultMaterialColumns;
  public id: string;
  public dataSource;
  public matService;
  public matQueries;
  public tableState = false;
  public willClose;
  public displayedColumns: string[];
  public displayedMaterialsColumns: string[];
  public displayTable: any = new Object(null);
  public opened: boolean;
  public headers = 'Заявки';
  public materialTableHeader = 'Материалы';
  public ownerHeader = [
    {key: OwnerType.Request, name: 'Заявки'},
    {key: OwnerType.ProtectionDoc, name: 'Охранные документы'},
    {key: OwnerType.Contract, name: 'Договора'}
    ];
  requestParams: any = [];
  contractParams: any = [];
  protectionDocParams: any = [];
  materialParams: any = [];
  cloned = {
    requestParams: null,
    contractParams: null,
    protectionDocParams: null
  };
  tabKeys = Object.keys(this.cloned);
  visible = [
    true,
    false,
    false,
    false
  ];
  currentTab = 0;

  constructor(
    private documentFlowService: DocumentFlowService,
    private changeDetector: ChangeDetectorRef,
    private requestService: RequestService,
    public materialsService: MaterialsService,
    private documentsService: DocumentsService,
    private dictionaryService: DictionaryService,
    private subjectsService: SubjectsService,
    private router: Router,
    public dialog: MatDialog,
    private columnConfigService: ColumnConfigService,
    private protectionDocsService: ProtectionDocsService,
    private incomingMaterialsService: IncomingMaterialsService,
    private contractService: ContractService,
    private workflowService: WorkflowService,
    private journalService: JournalService
  ) {}

  ngOnInit(): void {
    if (!this.displayedColumns) {
      this.displayedColumns = this.columnConfigService.get(this.ordersConfigKey, defaultColumns)
        .filter(columnConfig => columnConfig.enabled)
        .map(columnConfig => columnConfig.field);
    } else {
      this.columnConfigService.save(this.ordersConfigKey, defaultColumns);
    }

    if (!this.displayedMaterialsColumns) {
      this.displayedMaterialsColumns = this.columnConfigService.get(this.materialsConfigKey, defaultMaterialColumns)
        .filter(columnConfig => columnConfig.enabled)
        .map(columnConfig => columnConfig.field);
    } else {
      this.columnConfigService.save(this.materialsConfigKey, defaultMaterialColumns);
    }

    this.documentFlowService.searchFields
      .takeUntil(this.onDestroy)
      .subscribe(searchObject => {
        this.orderQueryParamsFiltered(searchObject, false);
      });

    this.documentFlowService.searchMatFields
      .takeUntil(this.onDestroy)
      .subscribe(searchObject => {
        this.orderQueryParamsFiltered(searchObject, true);
      });

    this.getMaterialsData();
    this.id = 'active';
    this.requestParams = [
      { key: `isComplete${Operators.equal}`, value: this.id !== 'active' },
      { key: `isActiveProtectionDocument${Operators.equal}`, value: false },
      { key: `ownerType${Operators.equal}`, value: OwnerType.Request }
    ];
    this.contractParams = [
      { key: `isComplete${Operators.equal}`, value: this.id !== 'active' },
      { key: `isActiveProtectionDocument${Operators.equal}`, value: false },
      { key: `ownerType${Operators.equal}`, value: OwnerType.Contract }
    ];
    this.protectionDocParams = [
      { key: `isComplete${Operators.equal}`, value: this.id !== 'active' },
      { key: `isActiveProtectionDocument${Operators.equal}`, value: false },
      { key: `ownerType${Operators.equal}`, value: OwnerType.ProtectionDoc }
    ];
    this.materialParams = [];
    this.cloned = {
      requestParams: JSON.parse(JSON.stringify(this.requestParams)),
      contractParams: JSON.parse(JSON.stringify(this.contractParams)),
      protectionDocParams: JSON.parse(JSON.stringify(this.protectionDocParams))
    };
    this.dataSource = this.documentFlowService;
    this.matService = this.incomingMaterialsService;
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  ngAfterViewInit(): void {
    this.changeDetector.detectChanges();
  }

  onSelectedTabChange(index: number): void {
    this.visible[index] = true;
    this.currentTab = index;
  }

  tableOpened(state) {
    this.itemDetailsState = state;
  }

  detailsSidebarState(event) {
    this.tableState = event;
    this.changeDetector.detectChanges();
  }

  closed(state): void {
    this.willClose = !state === this.willClose;
  }

  getMaterialsData(params?) {
    this.materialsService.getAllMaterials()
      .takeUntil(this.onDestroy)
      .subscribe(data => {
        this.materialsDataSource = data;
      });
    if (params) {
      this.materialsService.getFilteredMaterials(params)
        .takeUntil(this.onDestroy)
        .subscribe(data => {
          this.materialsDataSource = data;
        });
    }
  }

  onColumnsChange(defaultConfig) {
    const tableDiff = Symbol.for(this.materialTableHeader);
    const columnsType = defaultConfig.header === Symbol.keyFor(tableDiff) ? OwnerType.Material : OwnerType.None;
    const configKey = columnsType ? this.materialsConfigKey : this.ordersConfigKey;
    const defaultCols = columnsType ? defaultMaterialColumns : defaultColumns;
    const dialogRef = this.dialog.open(ColumnConfigDialogComponent, {
      data: {
        configKey,
        defaultConfig: defaultConfig.defaultConfig
      },
      disableClose: true
    });

    dialogRef.afterClosed()
      .takeUntil(this.onDestroy)
      .subscribe(result => {
        if (columnsType) {
          this.displayedMaterialsColumns = result;
        } else {
          this.displayedColumns = result;
        }

        this.columnSort(configKey, defaultCols, result);
      });
  }

  columnSort(configKey, defaultCol, result) {
    defaultCol.forEach(column => {
      column.enabled = result.includes(column.field);
    });

    this.columnConfigService.save(configKey, defaultCol);
  }

  onAddButton(event): void {
    this.dialog.open(CreateDocumentDialogComponent, {
      width: '500px',
      data: {},
    });
  }

  onImportButton(event): void {
    this.dialog.open(ImportDocumentDialogComponent, {
      width: '500px',
      data: {}
    });
  }

  orderQueryParamsFiltered(filter, defaultQueries) {
    const filteredQuery = this.buildQueries(filter);
    if (defaultQueries) {
      if (this.matQueries.length > 0) {
        this.matQueries = filteredQuery.filter(query => this.matQueries.some(newQuery => {
          if (newQuery.key === query.key && query.value !== null) {
            return true;
          } else if (newQuery.key !== query.key && query.value !== null) {
            return true;
          } else {
            return false;
          }
        }));
      } else {
        this.matQueries = filteredQuery.filter(query => query.value !== null);
      }
    } else {
      const key = this.tabKeys[this.currentTab];
      if (key) {
        const result = filteredQuery.filter(query => this[key].some(newQuery => {
          if (newQuery.key === query.key && query.value !== null) {
            return true;
          } else if (newQuery.key !== query.key && query.value !== null) {
            return true;
          } else {
            return false;
          }
        }));
        this[key] = result.concat(this.cloned[key]);
      }
    }
  }

  buildQueries(filter): any {
    const queries = Object.keys(filter).map(key => {
      let queryArray = {};
      if (key === 'createDate_from' || key === 'createDate_to' || key === 'currentStageDate_from' || key === 'currentStageDate_to') {
        queryArray = {
          key: key,
          value: filter[key]
        };
      } else {
        queryArray = {
          key: key + Operators.equal,
          value: filter[key]
        };
      }
      return queryArray;
    });
    return queries;
  }

  onNumberClicked(params) {
    this[this.tabKeys[this.currentTab]] = params.newQueries;
    const columns = this.columns.concat();
    const defaultCol = this.defaultColumns.concat();
    let newSortColumn, newColumn;
    if (params.hasIpc) {
      newSortColumn = { field: 'ipcCodes', name: 'Сортировка по МПК', enabled: true};
      newColumn = new Config({ columnDef: 'ipcCodes', header: 'Сортировка по МПК', class: 'width-150'});

    } else {
      newColumn = new Config({ columnDef: 'ipcCodes', header: 'Сортировка по номеру заявки', class: 'width-150'});
      newSortColumn = { field: 'ipcCodes', name: 'Сортировка по номеру заявки', enabled: true};
    }
    this.resetSelection();
    if (columns.find(col => col.columnDef === 'ipcCodes')) {
      columns.splice(3, 1, newColumn);
      defaultCol.splice(3, 1, newSortColumn);
      params.currentDisplayColumn.splice(4, 1, 'ipcCodes');
    } else {
      columns.splice(3, 0, newColumn);
      defaultCol.splice(3, 0, newSortColumn);
      params.currentDisplayColumn.splice(4, 0, 'ipcCodes');
    }
    this.columns = columns.concat();
    this.defaultColumns = defaultCol.concat();
    this.displayedColumns = params.currentDisplayColumn.concat();
  }

  checkedData(data) {
    if (data.isAllSelected) {
      this.selectionMode = SelectionMode.Except;
    } else {
      this.selectionMode = SelectionMode.Including;
    }
    if (this.displayTable.mainTables !==  OwnerType.Request) {
      this.generateGosNumber(data.hasIpc, data);
    } else {
      this.workflowService
        .getNextStagesByOwnerId(data.data[0].id, OwnerType.Request)
        .takeUntil(this.onDestroy)
        .subscribe(value => {
          const dialogRef = this.dialog.open(SelectUserDialogComponent, {
            width: '700px',
            data: {
              areSelectedRequests: true,
              nextStageCode: value.length > 0 ? value[0].code : ''
            }
          });

          dialogRef.afterClosed().subscribe(userId => {
            if (userId) {
              const params = [
                { key: 'isAllSelected', value: data.isAllSelected },
                { key: 'selectionMode', value: this.selectionMode }
              ];
              this.journalService
                .sendRequestsToStage(
                  userId.currentUserId,
                  data.data.map(s => s.id),
                  params
                )
                .takeUntil(this.onDestroy)
                .subscribe(() => {
                  this.resetSelection();
                });
            }
          });
        });
    }
  }

  private generateGosNumber(hasIpc: boolean, data) {
    const dialogRef = this.dialog.open(SelectUserDialogComponent, {
      width: '700px',
      data: {
        areAllSelectedTrademarks: data.data.every(s =>
          [
            'Заявка на Товарный Знак',
            'Заявка на Наименование Мест Происхождения Товаров',
            'Свидетельство на Товарный Знак',
            'Свидетельство на Наименование Мест Происхождения Товаров'
          ].includes(s.protectionDocTypeValue)
        ),
        nextStageCode: 'OD01.2.2'
      }
    });

    dialogRef.afterClosed().subscribe(userId => {
      if (userId) {
        const params = [
          { key: 'hasIpc', value: hasIpc },
          { key: 'isAllSelected', value: data.isAllSelected },
          { key: 'selectionMode', value: this.selectionMode }
        ];
        const bulletinId = !!userId.bulletinId.id
          ? userId.bulletinId.id
          : userId.bulletinId.bulletinId;

        this.journalService
          .sendProtectionDocsToStage(
            userId.currentUserId,
            !!userId.nextUserForPrintId ? userId.nextUserForPrintId : 0,
            !!userId.nextUserForDescriptionsId ? userId.nextUserForDescriptionsId : 0,
            !!userId.nextUserForMaintenanceId ? userId.nextUserForMaintenanceId : 0,
            !!userId.bulletinUserId ? userId.bulletinId : 0,
            bulletinId,
            !!userId.supportUserId ? userId.supportUserId : 0,
            data.data.map(s => s.id),
            params
          )
          .takeUntil(this.onDestroy)
          .subscribe(() => {
            this.resetSelection();
          });
      }
    });
  }

  private resetSelection() {
    if (this.tabKeys[this.currentTab]) {
      this.resetParams.next(this[this.tabKeys[this.currentTab]]);
    }
  }
}
