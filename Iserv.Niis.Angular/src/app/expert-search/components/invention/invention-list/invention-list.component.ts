import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
  ViewChild
} from '@angular/core';
import { OnChanges, SimpleChanges } from '@angular/core/src/metadata/lifecycle_hooks';
import { MatDialog, MatPaginator, MatSort, MatTable } from '@angular/material';
import { NavigationStart, Router, RouterEvent } from '@angular/router';
import { ColumnConfig, ColumnConfigDialogComponent, ColumnConfigService } from 'app/modules/column-config';
import { DetailsPopupComponent } from '../../details-popup/details-popup.component';
import { saveAs } from 'file-saver';
import { Subject } from 'rxjs/Rx';

import { ConfigService } from '../../../../core';
import { InventionSearchService } from '../../../services/invention-search.service';
import { BaseDataSource } from '../../../../shared/base-data-source';
import { NavigateOnSelectService } from 'app/expert-search/services/navigate-on-select.service';
import { getModuleName } from '../../../../shared/services/models/owner-type.enum';
import { InventionSearchDto } from '../../../models/invention-dto.model';
import { SelectionModel } from '@angular/cdk/collections';

const columnsConfigKey = 'invention_expert_search_columns_config';
const defaultColumnsConfig: ColumnConfig[] = [
  { field: 'select', name: 'Выбрать', enabled: true },
  { field: 'name', name: 'Название', enabled: true },
  { field: 'regNumber', name: 'Номер заявки', enabled: true },
  { field: 'regDate', name: 'Дата подачи заявки', enabled: true },
  { field: 'status', name: 'Статус(заявки/ОД)', enabled: true },
  { field: 'gosNumber', name: 'Номер ОД', enabled: true },
  { field: 'publishDate', name: 'Дата публикации', enabled: true },
  { field: 'declarant', name: 'Патентообладатель/Заявитель', enabled: true },
  { field: 'priorityData', name: 'Приоритетные данные', enabled: true },
  { field: 'ipc', name: 'МПК (индекс)', enabled: true },
  { field: 'referat', name: 'Реферат', enabled: true }
];

@Component({
  selector: 'app-invention-list',
  templateUrl: './invention-list.component.html',
  styleUrls: ['./invention-list.component.scss'],
})
export class InventionListComponent implements OnInit, OnChanges, AfterViewInit, OnDestroy {
  private onDestroy = new Subject();
  displayedColumns: string[];
  dataSource: BaseDataSource<InventionSearchDto> | null;
  queryParams: any[];
  selectionModel = new SelectionModel<InventionSearchDto>(true, []);
  closuredCheckedDtos: InventionSearchDto[] = [];

  @Input() checkable: boolean;
  @Input() checkedInventionDtos: InventionSearchDto[] = [];
  @Output() checkChanged = new EventEmitter<InventionSearchDto[]>();
  @Output() resultsLength = new EventEmitter<number>();

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild('filter') filter: ElementRef;
  @ViewChild(MatTable) table: MatTable<any>;

  constructor(
    private router: Router,
    private inventionSearchService: InventionSearchService,
    private columnConfigService: ColumnConfigService,
    public dialog: MatDialog,
    private configService: ConfigService,
    private navigateService: NavigateOnSelectService,
    private changeDetector: ChangeDetectorRef
  ) {
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
    if (changes.checkedInventionDtos && changes.checkedInventionDtos.currentValue) {
      this.selectionModel.clear();
      this.closuredCheckedDtos = [...(this.checkedInventionDtos || [])];
    }
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  toDate(dateString) {
    if (dateString) {
      return new Date(Date.parse(dateString)).toLocaleDateString();
    } else {
      return dateString;
    }
  }

  onColumnsChange() {
    const dialogRef = this.dialog.open(ColumnConfigDialogComponent, {
      data: {
        configKey: columnsConfigKey,
        defaultConfig: defaultColumnsConfig,
      },
      disableClose: true,
    });

    dialogRef
      .afterClosed()
      .takeUntil(this.onDestroy)
      .subscribe(result => {
        this.displayedColumns = result;
      });
  }

  onSelect(record: InventionSearchDto) {
    const moduleName = getModuleName(record.ownerType);
    this.navigateService.openItemInNewTab(moduleName, record.id);
  }

  openDetailsPopup(title, message) {
    this.dialog.open(DetailsPopupComponent, {
      data: {
        title,
        message
      }
    });
  }

  reset(queryParams?: any[]) {
    this.selectionModel.clear();
    this.closuredCheckedDtos = [...(this.checkedInventionDtos || [])];
    this.queryParams = queryParams;
    this.dataSource.reset(queryParams);
  }

  ngAfterViewInit(): void {
    this.paginator.pageIndex = 0;
    this.dataSource = new BaseDataSource<InventionSearchDto>(
      this.inventionSearchService,
      this.configService,
      this.paginator,
      this.sort,
      this.filter,
      true
    );

    this.dataSource.resetCompleted
      .takeUntil(this.onDestroy)
      .distinctUntilChanged()
      .subscribe(() => {
        this.resultsLength.emit(this.dataSource.resultsLength);
      });

    this.changeDetector.detectChanges();
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

  isChecked(row: InventionSearchDto): boolean {
    if (this.closuredCheckedDtos && this.closuredCheckedDtos.length > 0
      && this.closuredCheckedDtos.some(dto => dto.ownerType === row.ownerType && dto.id === row.id)) {
      this.selectionModel.toggle(row);
      this.closuredCheckedDtos = [...this.closuredCheckedDtos.filter(dto => !(dto.ownerType === row.ownerType && dto.id === row.id))];
    }

    return this.selectionModel.isSelected(row);
  }

  onExport() {
    const queryParams = [...this.queryParams];
    queryParams.push({
      key: 'excelFields',
      value: this.displayedColumns.join(),
    });
    queryParams.push({ key: '_limit', value: this.paginator.pageSize });
    queryParams.push({ key: '_page', value: this.paginator.pageIndex + 1 });

    this.inventionSearchService
      .getExcel(queryParams)
      .takeUntil(this.onDestroy)
      .subscribe(response => {
        saveAs(
          response.body,
          `Результаты экспертного поиска по ТЗ ${new Date().toLocaleString()}.xlsx`
        );
      });
  }
}
