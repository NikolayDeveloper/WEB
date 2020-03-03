import { AfterViewInit, Component, ElementRef, EventEmitter, OnInit, OnChanges, Output, ViewChild, Input, OnDestroy, ChangeDetectorRef, SimpleChanges } from '@angular/core';
import { MatDialog, MatPaginator, MatSort } from '@angular/material';
import { Router, RouterEvent, NavigationStart } from '@angular/router';
import { ColumnConfig, ColumnConfigDialogComponent, ColumnConfigService } from 'app/modules/column-config';
import { saveAs } from 'file-saver';
import { Subject, Subscription } from 'rxjs/Rx';

import { ConfigService } from '../../../../core/index';
import { SearchType } from '../../../../search/models/search-type.enum';
import { BaseDataSource } from '../../../../shared/base-data-source';
import { getModuleName } from '../../../../shared/services/models/owner-type.enum';
import { IndustrialdesignDto } from '../../../models/industrialdesign-dto.model';
import { IndustrialdesignSearchService } from '../../../services/industrialdesign-search.service';
import { NavigateOnSelectService } from 'app/expert-search/services/navigate-on-select.service';
import { SelectionModel } from '@angular/cdk/collections';
import { ExpertSearchSimilarDto } from 'app/expert-search/models/expert-search-similar-dto';


const columnsConfigKey = 'industrialdesign_expert_search_columns_config';
const defaultColumnsConfig: ColumnConfig[] = [
  { field: 'select', name: 'Выбрать', enabled: true },
  { field: 'name', name: 'Наименование', enabled: true },
  { field: 'barcode', name: 'Штрихкод', enabled: true },
  { field: 'requestTypeNameRu', name: 'Тип заявки', enabled: true },
  { field: 'statusNameRu', name: 'Статус', enabled: true },
  { field: 'previewImage', name: 'Изображение', enabled: true },
  { field: 'gosNumber', name: '№ ОД', enabled: true },
  { field: 'gosDate', name: 'Дата ОД', enabled: true },
  { field: 'requestNum', name: 'Рег. номер заявки', enabled: true },
  { field: 'requestDate', name: 'Дата подачи заявки', enabled: true },
  { field: 'nameRu', name: 'Наименование на рус', enabled: true },
  { field: 'nameKz', name: 'Наименование на каз', enabled: true },
  { field: 'nameEn', name: 'Наименование на англ', enabled: true },
  { field: 'declarant', name: 'Заявитель', enabled: true },
  { field: 'patentOwner', name: 'Патентообладатель', enabled: true },
  { field: 'patentAttorney', name: 'Патентный поверенный', enabled: true },
  { field: 'addressForCorrespondence', name: 'Адрес для переписки', enabled: true },
  { field: 'confidant', name: 'Доверенное лицо', enabled: true },
  { field: 'receiveTypeNameRu', name: 'Тип подачи заявки', enabled: true },
  { field: 'icis', name: 'МКПО', enabled: true },
  { field: 'author', name: 'Автор', enabled: true },
  { field: 'referat', name: 'Автор', enabled: true },
  { field: 'priorityData', name: 'Приоритетные данные', enabled: true },
  { field: 'numberBulletin', name: 'Номер бюллетеня', enabled: true },
  { field: 'publicDate', name: 'Дата публикации', enabled: true },
  { field: 'validDate', name: 'Срок действия', enabled: true },
  { field: 'earlyTerminationDate', name: 'Дата досрочного прекращения', enabled: true },
];

@Component({
  selector: 'app-industrialdesign-list',
  templateUrl: './industrialdesign-list.component.html',
  styleUrls: ['./industrialdesign-list.component.scss']
})
export class IndustrialdesignListComponent implements OnInit, OnChanges, AfterViewInit, OnDestroy {
  displayedColumns: string[];
  dataSource: BaseDataSource<IndustrialdesignDto> | null;
  queryParams: any[];
  selectionModel = new SelectionModel<IndustrialdesignDto>(true, []);
  closuredCheckedDtos: IndustrialdesignDto[] = [];
  private onDestroy = new Subject();

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild('filter') filter: ElementRef;
  @Input() checkable: boolean;
  @Input() checkedIndustrialDesignDtos: IndustrialdesignDto[];
  @Output() checkChanged = new EventEmitter<ExpertSearchSimilarDto[]>();
  @Output() resultsLength = new EventEmitter<number>();

  constructor(
    private router: Router,
    private industrialdesignSearchService: IndustrialdesignSearchService,
    private columnConfigService: ColumnConfigService,
    public dialog: MatDialog,
    private configService: ConfigService,
    private navigateService: NavigateOnSelectService,
    private changeDetector: ChangeDetectorRef) {
    this.router.events.subscribe((event: RouterEvent) => {
      if (event instanceof NavigationStart) {
        if (!event.url.includes('request')) {
          this.columnConfigService.save(columnsConfigKey, defaultColumnsConfig);
        }
      }
    });
  }

  ngOnInit() {
    this.displayedColumns = this.columnConfigService.get(columnsConfigKey, defaultColumnsConfig)
      .filter(cc => cc.enabled)
      .map(cc => cc.field);
    this.paginator.pageSize = this.configService.pageSize;
    this.paginator.pageSizeOptions = this.configService.pageSizeOptions;
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.checkedIndustrialDesignDtos && changes.checkedIndustrialDesignDtos.currentValue) {
      this.selectionModel.clear();
      this.closuredCheckedDtos = [...(this.checkedIndustrialDesignDtos || [])];
    }
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

    dialogRef.afterClosed()
      .takeUntil(this.onDestroy)
      .subscribe(result => {
        this.displayedColumns = result;
      });
  }

  onSelect(record: IndustrialdesignDto) {
    const moduleName = getModuleName(record.ownerType);
    this.navigateService.openItemInNewTab(moduleName, record.id);
  }

  reset(queryParams?: any[]) {
    this.selectionModel.clear();
    this.closuredCheckedDtos = [...(this.checkedIndustrialDesignDtos || [])];
    this.queryParams = queryParams;
    this.dataSource.reset(queryParams);
  }

  ngAfterViewInit(): void {
    this.paginator.pageIndex = 0;
    this.dataSource =
      new BaseDataSource<IndustrialdesignDto>(this.industrialdesignSearchService, this.configService, this.paginator, this.sort,
        this.filter, true);

    this.dataSource.resetCompleted
      .takeUntil(this.onDestroy)
      .distinctUntilChanged()
      .subscribe(() => this.resultsLength.emit(this.dataSource.resultsLength));

    this.changeDetector.detectChanges();
  }

  onExport() {
    const queryParams = [...this.queryParams];
    queryParams.push({ key: 'excelFields', value: this.displayedColumns.filter(c => c !== 'previewImage').join() });
    queryParams.push({ key: '_limit', value: this.paginator.pageSize });
    queryParams.push({ key: '_page', value: this.paginator.pageIndex + 1 });

    this.industrialdesignSearchService
      .getExcel(queryParams)
      .takeUntil(this.onDestroy)
      .subscribe(response => {
        saveAs(response.body, `Результаты экспертного поиска по ТЗ ${new Date().toLocaleString()}.xlsx`);
      });
  }

  isAllSelected() {
    const numSelected = this.selectionModel.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected()
      ? this.selectionModel.clear()
      : this.dataSource.data.forEach(row => this.selectionModel.select(row));
  }

  isChecked(row: IndustrialdesignDto): boolean {
    if (this.closuredCheckedDtos && this.closuredCheckedDtos.length > 0
      && this.closuredCheckedDtos.some(dto => dto.ownerType === row.ownerType && dto.id === row.id)) {
      this.selectionModel.toggle(row);
      this.closuredCheckedDtos = [...this.closuredCheckedDtos.filter(dto => !(dto.ownerType === row.ownerType && dto.id === row.id))];
    }

    return this.selectionModel.isSelected(row);
  }
}
