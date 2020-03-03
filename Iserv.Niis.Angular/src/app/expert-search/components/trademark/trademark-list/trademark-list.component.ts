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
  ViewChild,
} from '@angular/core';
import { OnChanges, SimpleChanges } from '@angular/core/src/metadata/lifecycle_hooks';
import { MatDialog, MatPaginator, MatSort, MatIconRegistry } from '@angular/material';
import { NavigationStart, Router, RouterEvent } from '@angular/router';
import { ColumnConfig, ColumnConfigDialogComponent, ColumnConfigService } from 'app/modules/column-config';
import { DetailsPopupComponent } from '../../details-popup/details-popup.component';
import { saveAs } from 'file-saver';
import { Subject } from 'rxjs/Rx';

import { ConfigService } from '../../../../core';
import { TrademarkSearchService } from '../../../../expert-search/services/trademark-search.service';
import { RequestService } from '../../../../requests/request.service';
import { BaseDataSource } from '../../../../shared/base-data-source';
import { getModuleName } from '../../../../shared/services/models/owner-type.enum';
import { NavigateOnSelectService } from '../../../services/navigate-on-select.service';
import { TrademarkSearchDto } from '../../../models/trademark-dto.model';
import { SelectionModel } from '@angular/cdk/collections';
import { DomSanitizer } from '@angular/platform-browser';

const columnsConfigKey = 'trademark_expert_search_columns_config';
const defaultColumnsConfig: ColumnConfig[] = [
  { field: 'select', name: 'Выбрать', enabled: true },
  { field: 'previewImage', name: 'Изображение', enabled: true },
  { field: 'ownerType', name: 'Тип', enabled: true },
  { field: 'name', name: 'Наименование', enabled: true },
  { field: 'statusNameRu', name: 'Статус', enabled: true },
  { field: 'gosNumber', name: '№ ОД', enabled: true },
  { field: 'gosDate', name: 'Дата ОД', enabled: true },
  { field: 'regNumber', name: 'Рег. номер заявки', enabled: true },
  { field: 'regDate', name: 'Дата подачи заявки', enabled: true },
  { field: 'declarantName', name: 'Заявитель', enabled: true },
  { field: 'ownerName', name: 'Владелец', enabled: true },
  { field: 'priorityData', name: 'Приоритетные данные', enabled: true },
  { field: 'icgs', name: 'МКТУ', enabled: true },
  { field: 'icfem', name: 'МКИЭТЗ', enabled: true },
  { field: 'colors', name: 'Цвета', enabled: true },
  { field: 'validDate', name: 'Срок действия', enabled: true },
  { field: 'disclamation', name: 'Дискламация', enabled: true },
];

@Component({
  selector: 'app-trademark-list',
  templateUrl: './trademark-list.component.html',
  styleUrls: ['./trademark-list.component.scss']
})
export class TrademarkListComponent implements OnInit, OnChanges, AfterViewInit, OnDestroy {
  private onDestroy = new Subject();
  displayedColumns: string[];
  dataSource: BaseDataSource<TrademarkSearchDto> | null;
  queryParams: any[];
  selectionModel = new SelectionModel<TrademarkSearchDto>(true, []);
  closuredCheckedDtos: TrademarkSearchDto[] = [];

  @Input() checkable: boolean;
  @Input() checkedTrademarkDtos: TrademarkSearchDto[] = [];
  @Output() checkChanged = new EventEmitter<TrademarkSearchDto[]>();
  @Output() resultsLength = new EventEmitter<number>();
  @Output() showImage = new EventEmitter<any>();

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild('filter') filter: ElementRef;

  constructor(
    private router: Router,
    private trademarkSearchService: TrademarkSearchService,
    private columnConfigService: ColumnConfigService,
    public dialog: MatDialog,
    private configService: ConfigService,
    private iconRegistry: MatIconRegistry,
    private sanitizer: DomSanitizer,
    private requestService: RequestService,
    private navigateService: NavigateOnSelectService,
    private changeDetector: ChangeDetectorRef) {
    iconRegistry.addSvgIcon(
      'request',
      sanitizer.bypassSecurityTrustResourceUrl('./assets/request.svg')
    );
    iconRegistry.addSvgIcon(
      'protection-doc',
      sanitizer.bypassSecurityTrustResourceUrl('./assets/protectionDoc.svg')
    );
    iconRegistry.addSvgIcon(
      'contract',
      sanitizer.bypassSecurityTrustResourceUrl('./assets/contract.svg')
    );
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
    if (changes.checkedTrademarkDtos && changes.checkedTrademarkDtos.currentValue) {
      this.selectionModel.clear();
      this.closuredCheckedDtos = [...(this.checkedTrademarkDtos || [])];
    }
  }

  imageClick(event: any, row: any) {
    event.stopPropagation();
    if (row.previewImage) {
      this.showImage.emit({
        ownerId: row.id,
        ownerType: row.ownerType
      });
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

  onSelect(record: TrademarkSearchDto) {
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

  stringifyICGS(array: string[]): any {
    if (array) {
      const sortedArray = array.sort((a, b) => a.localeCompare(b));

      return sortedArray.join('\n');
    } else {
      return array;
    }
  }

  reset(queryParams?: any[]) {
    this.selectionModel.clear();
    this.closuredCheckedDtos = [...(this.checkedTrademarkDtos || [])];
    this.queryParams = queryParams;
    this.dataSource.reset(queryParams);
  }

  ngAfterViewInit(): void {
    this.paginator.pageIndex = 0;
    this.dataSource = new BaseDataSource<TrademarkSearchDto>(this.trademarkSearchService, this.configService,
      this.paginator, this.sort, this.filter, true);

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

    this.trademarkSearchService
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

  isChecked(row: TrademarkSearchDto): boolean {
    if (this.closuredCheckedDtos && this.closuredCheckedDtos.length > 0
      && this.closuredCheckedDtos.some(dto => dto.ownerType === row.ownerType && dto.id === row.id)) {
      this.selectionModel.toggle(row);
      this.closuredCheckedDtos = [...this.closuredCheckedDtos.filter(dto => !(dto.ownerType === row.ownerType && dto.id === row.id))];
    }

    return this.selectionModel.isSelected(row);
  }
}
