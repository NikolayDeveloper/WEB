import 'rxjs/add/operator/takeUntil';
import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { QueryParam } from '../../helpers/query-param';
import { Config } from '../../../shared/components/table/config.model';
import { Subject } from 'rxjs/Subject';
import { DocumentDto } from '../../models/document.dto';
import { DocumentsSearchParametersDto } from '../../models/documents-search-parameters.dto';
import { DocumentsService } from '../../services/documents.service';
import { toFullDateString, toShortDateString } from '../../helpers/date-helpers';
import { ColumnConfig, ColumnConfigDialogComponent, ColumnConfigService } from '../../../modules/column-config';
import { MatDialog } from '@angular/material';
import { saveAs } from 'file-saver';

const COLUMNS_CONFIG_KEY = 'payments_journal_documents_columns_config';

@Component({
  selector: 'app-documents-list',
  templateUrl: './documents-list.component.html'
})
export class DocumentsListComponent implements OnInit, OnChanges {
  @Input()
  public searchParams: DocumentsSearchParametersDto;

  @Output()
  public selectedDocumentChanged: EventEmitter<DocumentDto> = new EventEmitter();

  public displayedColumns: string[];

  public readonly columnsConfigs: ColumnConfig[] = [
    { field: 'docTypeName', name: 'Вид ОПС \\ Договора', enabled: true },
    { field: 'barcode', name: 'Штрих код', enabled: true },
    { field: 'requestSubTypeName', name: 'Подвид заявки', enabled: true },
    { field: 'requestTypeName', name: 'Тип заявки', enabled: true },
    { field: 'incomingNumber', name: 'Входящий номер Заявки \\ Договора', enabled: true },
    { field: 'dateCreate', name: 'Дата создания записи', enabled: true },
    { field: 'regNumber', name: 'Регистрационный номер Заявки \\ Договора', enabled: true },
    { field: 'receiveTypeName', name: 'Тип доставки', enabled: true },
    { field: 'nameRu', name: 'Наименование на русском', enabled: true },
    { field: 'nameKz', name: 'Наименование на казахском', enabled: true },
    { field: 'nameEn', name: 'Наименование на английском', enabled: true },
    { field: 'regDate', name: 'Дата подачи Заявки \\ Заявления на регистрацию Договора', enabled: true },
    { field: 'protectionDocNumber', name: 'Номер охранного документа', enabled: true },
    { field: 'protectionDocMaintainYear', name: 'Год поддержания ОД', enabled: true },
    { field: 'protectionDocValidDate', name: 'Срок действия ОД', enabled: true },
    { field: 'protectionDocExtensionDate', name: 'Срок продления действия ОД', enabled: true },
    { field: 'icisCodes', name: 'МПК', enabled: true },
    { field: 'requestStatusName', name: 'Статус заявки', enabled: true },
    { field: 'protectionDocStatusName', name: 'Статус ОД', enabled: true },
    { field: 'icfemCodes', name: 'МКИТЗ', enabled: true },
    { field: 'ipcCodes', name: 'МКПО', enabled: true },
    { field: 'icgsCodes', name: 'МКТУ', enabled: true },
    { field: 'selectionAchieveTypeName', name: 'Тип селекционного достижения', enabled: true },
    { field: 'breedingNumber', name: 'Селекционный номер', enabled: true },
    { field: 'protectionDocDate', name: 'Дата регистрации ОД в Госреестре', enabled: true },
    { field: 'protectionDocOutgoingDate', name: 'Дата отправки патента патентообладателю', enabled: true },
    { field: 'disclaimerRu', name: 'Дискламация (рус)', enabled: true },
    { field: 'disclaimerKz', name: 'Дискламация (каз)', enabled: true },
    { field: 'declarantNames', name: 'Заявитель', enabled: true },
    { field: 'patentOwnerNames', name: 'Патентообладатель', enabled: true },
    { field: 'authorNames', name: 'Автор', enabled: true },
    { field: 'patentAttorneyNames', name: 'Патентно-поверенный', enabled: true },
    { field: 'correspondenceNames', name: 'Адресат для переписки', enabled: true },
    { field: 'confidantNames', name: 'Доверенное лицо', enabled: true },
    { field: 'authorsAreNotMentions', name: 'Наличие отказов от упоминания при публикации', enabled: true },
    { field: 'authorsCertificateNumbers', name: '№ авторского свидетельства', enabled: true },
    { field: 'numberBulletin', name: 'Номер бюллетеня', enabled: true },
  ];

  public readonly columns: Config[] = [
    new Config({ columnDef: 'docTypeName', header: 'Вид ОПС \\ Договора', class: 'width-300' }),
    new Config({ columnDef: 'barcode', header: 'Штрих код', class: 'width-150' }),
    new Config({ columnDef: 'requestSubTypeName', header: 'Подвид заявки', class: 'width-300' }),
    new Config({ columnDef: 'requestTypeName', header: 'Тип заявки', class: 'width-300' }),
    new Config({ columnDef: 'incomingNumber', header: 'Входящий номер Заявки \\ Договора', class: 'width-150' }),
    new Config({
      columnDef: 'dateCreate',
      header: 'Дата создания записи',
      class: 'width-125',
      format: (row: DocumentDto) => toFullDateString(row.dateCreate)
    }),
    new Config({ columnDef: 'regNumber', header: 'Регистрационный номер Заявки \\ Договора', class: 'width-150' }),
    new Config({ columnDef: 'receiveTypeName', header: 'Тип доставки', class: 'width-300' }),
    new Config({ columnDef: 'nameRu', header: 'Наименование на русском', class: 'width-300' }),
    new Config({ columnDef: 'nameKz', header: 'Наименование на казахском', class: 'width-300' }),
    new Config({ columnDef: 'nameEn', header: 'Наименование на английском', class: 'width-300' }),
    new Config({
      columnDef: 'regDate',
      header: 'Дата подачи Заявки \\ Заявления на регистрацию Договора',
      class: 'width-200',
      format: (row: DocumentDto) => toShortDateString(row.regDate)
    }),
    new Config({ columnDef: 'protectionDocNumber', header: 'Номер охранного документа', class: 'width-125' }),
    new Config({ columnDef: 'protectionDocMaintainYear', header: 'Год поддержания ОД', class: 'width-125' }),
    new Config({
      columnDef: 'protectionDocValidDate',
      header: 'Срок действия ОД',
      class: 'width-125',
      format: (row: DocumentDto) => toShortDateString(row.protectionDocValidDate)
    }),
    new Config({
      columnDef: 'protectionDocExtensionDate',
      header: 'Срок продления действия ОД',
      class: 'width-125',
      format: (row: DocumentDto) => toShortDateString(row.protectionDocExtensionDate)
    }),
    new Config({ columnDef: 'icisCodes', header: 'МПК', class: 'width-300' }),
    new Config({ columnDef: 'requestStatusName', header: 'Статус заявки', class: 'width-300' }),
    new Config({ columnDef: 'protectionDocStatusName', header: 'Статус ОД', class: 'width-300' }),
    new Config({ columnDef: 'icfemCodes', header: 'МКИТЗ', class: 'width-300' }),
    new Config({ columnDef: 'ipcCodes', header: 'МКПО', class: 'width-300' }),
    new Config({ columnDef: 'icgsCodes', header: 'МКТУ', class: 'width-300' }),
    new Config({ columnDef: 'selectionAchieveTypeName', header: 'Тип селекционного достижения', class: 'width-300' }),
    new Config({ columnDef: 'breedingNumber', header: 'Селекционный номер', class: 'width-150' }),
    new Config({
      columnDef: 'protectionDocDate',
      header: 'Дата регистрации ОД в Госреестре',
      class: 'width-150',
      format: (row) => toShortDateString(row.protectionDocDate)
    }),
    new Config({
      columnDef: 'protectionDocOutgoingDate',
      header: 'Дата отправки патента патентообладателю',
      class: 'width-150',
      format: (row: DocumentDto) => toShortDateString(row.protectionDocOutgoingDate)
    }),
    new Config({ columnDef: 'disclaimerRu', header: 'Дискламация (рус)', class: 'width-300' }),
    new Config({ columnDef: 'disclaimerKz', header: 'Дискламация (каз)', class: 'width-300' }),
    new Config({ columnDef: 'declarantNames', header: 'Заявитель', class: 'width-300' }),
    new Config({ columnDef: 'patentOwnerNames', header: 'Патентообладатель', class: 'width-300' }),
    new Config({ columnDef: 'authorNames', header: 'Автор', class: 'width-300' }),
    new Config({ columnDef: 'patentAttorneyNames', header: 'Патентно-поверенный', class: 'width-300' }),
    new Config({ columnDef: 'correspondenceNames', header: 'Адресат для переписки', class: 'width-300' }),
    new Config({ columnDef: 'confidantNames', header: 'Доверенное лицо', class: 'width-300' }),
    new Config({ columnDef: 'authorsAreNotMentions', header: 'Наличие отказов от упоминания при публикации', class: 'width-125' }),
    new Config({ columnDef: 'authorsCertificateNumbers', header: '№ авторского свидетельства', class: 'width-200' }),
    new Config({ columnDef: 'numberBulletin', header: 'Номер бюллетеня', class: 'width-150' }),
  ];

  public readonly reset = new Subject();

  constructor(public documentsService: DocumentsService,
              private columnConfigService: ColumnConfigService,
              private dialog: MatDialog) {
  }

  public ngOnInit(): void {
    this.initializeColumns();
  }

  public ngOnChanges(changes: SimpleChanges): void {
    this.loadData();
  }

  public onSelect(item: DocumentDto): void {
    this.selectedDocumentChanged.emit(item);
  }

  public onResultsLength(): void {
    this.selectedDocumentChanged.emit(null);
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
    this.reset.next(this.getSearchQueryParams());
    this.selectedDocumentChanged.emit(null);
  }

  private getSearchQueryParams(): QueryParam[] {
    if (!this.searchParams) {
      this.searchParams = new DocumentsSearchParametersDto();
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

  onExport(columns: string[]): void {
    const queryParams = [
      {
        key: 'excelFields',
        value: (columns && columns.length) ? columns.join() : this.displayedColumns.join()
      },
      ...this.searchParams.getQueryParams()
    ];

    this.documentsService
      .getExcel(queryParams)
      .subscribe((data) => {
        saveAs(data.body, 'Журнал Заявок \\ ОД \\ Договоров.xlsx');
      });
  }
}
