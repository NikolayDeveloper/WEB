import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  SimpleChanges,
  ViewChild
} from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { Subject } from 'rxjs';
import * as jsPDF from 'jspdf';
import { Observable } from 'rxjs/Observable';
import { ConfigService } from '../../../core';
import { BaseDataSource } from '../../base-data-source';
import { BaseServiceWithPagination } from '../../base-service-with-pagination';
import { Config } from './config.model';
import { MatDialog } from '@angular/material';
import {ColumnConfig, ColumnConfigDialogComponent, ColumnConfigService} from '../../../modules/column-config';
import {SelectionModel} from '@angular/cdk/collections';
import {OwnerType} from '../../services/models/owner-type.enum';
import {Operators} from '../../filter/operators';
import { DomSanitizer, SafeStyle } from '@angular/platform-browser';
import { SystemService } from '../../services/system.service';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss']
})
export class TableComponent
  implements OnInit, OnChanges, AfterViewInit, OnDestroy {
  private datePattern = new RegExp(
    /\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}[.\d]*\+\d{2}:\d{2}/
  );
  private onDestroy = new Subject();
  public simpleColumns: Config[];
  public selectAllView = false;
  calculatedDataSource: any;
  selection = new SelectionModel<any>(true, []);
  selectedRow: any;
  public protectDocs: boolean;
  public hasIpc: boolean;
  public showTick: boolean;
  public defaultColumns: ColumnConfig[] = [];
  private userId: number;
  // get simpleColumns(): Config[] {
  //   return this.columns.filter(c => !c.click);
  // }
  get buttonColumns(): Config[] {
    return this.columns.filter(column => (column.click && this.displayedColumns.includes(column.columnDef)));
  }

  @Input() displayedColumns: string[];
  @Input() headerRowClass: string;
  @Input() rowClass: string;
  @Input() isCheckBoxColumn: boolean;
  @Input() defaultConfig: ColumnConfig[];
  @Input() source: BaseServiceWithPagination<any>;
  @Input() data: any[];
  @Input() columns: Config[];
  @Input() selectable: boolean;
  @Input() sortable: boolean;
  @Input() filterable: boolean;
  @Input() filterPredicate: (row: any, value: string) => boolean;
  @Input() clientFilters: string;
  @Input() paginable: boolean;
  @Input() paginatorOnTop: boolean;
  @Input() pageSize: number;
  @Input() onlyQueriedData: boolean;
  @Input() print: boolean;
  @Input() reset: Observable<any>;
  @Input() addButton: string;
  @Input() importButton: string;
  @Input() header: string;
  @Input() sortButton: string;
  @Input() queryData: any[];
  @Input() tableMaxHeight: string;
  @Input() uniqTableName: string;
  @Input() showOnlyDate: boolean = false;
  @Input() canExport: boolean = false;
  @Input() sortBy: string;
  @Input() sortMode: 'asc' | 'desc' = 'asc';

  @Output() select = new EventEmitter<any>();
  @Output() resultsLength = new EventEmitter<any>();
  @Output() tableOpened = new EventEmitter<any>();
  @Output() pageChange = new EventEmitter<any>();
  @Output() columnSort = new EventEmitter<any>();
  @Output() onAddButton = new EventEmitter<any>();
  @Output() onImportButton = new EventEmitter<any>();
  @Output() itemClick = new EventEmitter<any>();
  @Output() checkedData = new EventEmitter<any>();
  @Output() onNumberClicked = new EventEmitter<any>();
  @Output() export: EventEmitter<any> = new EventEmitter();

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('filter') filter: ElementRef;
  @ViewChild('table') table: ElementRef;

  constructor(
    private columnConfigService: ColumnConfigService,
    public dialog: MatDialog,
    private configService: ConfigService,
    private changeDetector: ChangeDetectorRef,
    private sanitizer: DomSanitizer,
    private systemService: SystemService
  ) {
    // window.html2canvas = html2canvas;
  }

  /**
   * Возвращает CSS параметр позиции
   * @param index Индекс элемента
   * @return CSS параметр позиции
   */
  getPosition(index): SafeStyle {
    return this.sanitizer.bypassSecurityTrustStyle(`--position: ${index};`);
  }

  getColumnsForSort() {
    this.columns.forEach(col => {
      this.defaultColumns.push({
        field: col.columnDef,
        name: col.header,
        enabled: true
      });
    });
    this.getSortedCols();
  }
  displayModalForSortingCols() {
    const dialogRef = this.dialog.open(ColumnConfigDialogComponent, {
      data: {
        configKey: this.uniqTableName,
        defaultConfig: this.defaultColumns
      },
      disableClose: true
    });

    dialogRef.afterClosed()
      .takeUntil(this.onDestroy)
      .subscribe(result => {
        this.displayedColumns = result;
        this.columnsSort(this.uniqTableName, this.defaultColumns, result);
        if (this.uniqTableName) {
          this.systemService.saveUserSettings(this.uniqTableName, result).subscribe(console.log);

          localStorage.setItem(`previous_${this.uniqTableName}`, JSON.stringify({
            date: new Date(),
            columns: this.displayedColumns
          }));
        }
      });
  }
  getSortedCols() {
    if (!this.displayedColumns && this.uniqTableName) {
      this.displayedColumns = this.columnConfigService.get(this.uniqTableName, this.defaultColumns)
        .filter(cc => cc.enabled)
        .map(cc => cc.field);
    } else {
      this.columnConfigService.save(this.uniqTableName, this.defaultColumns);
    }
  }
  columnsSort(configKey, defaultCol, result) {
    defaultCol.forEach(column => {
      column.enabled = result.includes(column.field);
    });
    this.columnConfigService.save(configKey, defaultCol);
  }
  ngOnInit() {
    this.getColumns(null);
    this.getColumnsForSort();

    if (this.uniqTableName) {
      const saved = JSON.parse(localStorage.getItem(`previous_${this.uniqTableName}`));

      if (saved) {
        const delta = Date.now() - Date.parse(saved.date);

        if (delta > 12 * 60 * 60 * 1000) {
          this.loadTableSettings();
        } else {
          const defaultColumns = this.defaultColumns.map(entry => entry.field);
          this.displayedColumns = saved.columns.filter(entry => defaultColumns.includes(entry));
          this.columnsSort(this.uniqTableName, this.defaultColumns, saved.columns);
        }
      } else {
        this.loadTableSettings();
      }
    }
  }

  loadTableSettings() {
    this.systemService.loadUserSettings(this.uniqTableName)
      .subscribe((columns: string[]) => {
        if (columns) {
          const defaultColumns = this.defaultColumns.map(entry => entry.field);
          this.displayedColumns = columns.filter(entry => defaultColumns.includes(entry));
          this.columnsSort(this.uniqTableName, this.defaultColumns, columns);
        }

        localStorage.setItem(`previous_${this.uniqTableName}`, JSON.stringify({
          date: new Date(),
          columns: this.displayedColumns
        }));
      });
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (
      (changes.source && changes.source.currentValue) ||
      (changes.data && changes.data.currentValue)
    ) {
      this.initDataSource();
    }

    if (
      changes.data &&
      changes.data.currentValue &&
      this.calculatedDataSource
    ) {
      this.calculatedDataSource.data = this.data;
    }

    if (
      changes.clientFilters &&
      changes.clientFilters.currentValue &&
      this.calculatedDataSource
    ) {
      this.calculatedDataSource.filter = this.clientFilters;
    }

    if (
      changes.filterPredicate &&
      changes.filterPredicate.currentValue &&
      this.calculatedDataSource
    ) {
      this.calculatedDataSource.filterPredicate = this.filterPredicate;
    }

    if (changes.reset && changes.reset.currentValue) {
      this.reset.takeUntil(this.onDestroy).subscribe(data => {
        if (this.calculatedDataSource instanceof BaseDataSource) {
          this.calculatedDataSource.reset(data);
          this.resultsLength.emit(this.calculatedDataSource.resultsLength);
        } else {
          this.calculatedDataSource.data = this.data;
        }
      });
    }

    if (changes.columns && changes.columns.currentValue) {
      this.getColumns(changes.columns.currentValue);
      if (!this.displayedColumns && !this.uniqTableName) {
        this.displayedColumns = this.columns.map(c => c.columnDef);
      }
    }
    if (changes.queryData) {
      const query = changes.queryData && changes.queryData.currentValue;
      const dataState = query && query.find(data => data.key === 'ownerType_eq');
      if (dataState) {
        switch (dataState.value) {
          case OwnerType.Request:
            this.selectAllView = false;
            this.removeColumns('ipcCodes');
            this.protectDocs = false;
            // this.isCheckBoxColumn = true;
            this.isCheckBoxColumn = false;
            break;
          case OwnerType.Contract:
            this.selectAllView = false;
            this.removeColumns('ipcCodes');
            this.protectDocs = false;
            this.isCheckBoxColumn = false;
            break;
          case OwnerType.ProtectionDoc:
            this.protectDocs = true;
            this.isCheckBoxColumn = false;
        }
      }
      if (Array.isArray(changes.queryData.currentValue) && !changes.queryData.firstChange) {
        this.calculatedDataSource.reset(changes.queryData.currentValue);
      }
    }
  }
  removeColumns(colName) {
    const index = this.displayedColumns && this.displayedColumns.indexOf(colName);
    if (index) {
      this.displayedColumns.splice(index, 1);
    }
  }
  getColumns(columns) {
    this.simpleColumns = columns ? columns.filter(c => !c.click) : this.columns.filter(c => !c.click);
  }
  onColumnsChange() {
    this.displayModalForSortingCols()
    //this.columnSort.emit({defaultConfig: this.defaultConfig, header: this.header});
  }
  getClasses(column: Config) {
    const classObj = {};
    (column.class || '').split(' ').forEach(c => {
      classObj[c] = true;
    });

    return classObj;
  }
  getHeaderRowClass() {
    const classObj = {};
    (this.headerRowClass || '').split(' ').forEach(c => {
      classObj[c] = true;
    });

    return classObj;
  }
  onSpanClick(item, row) {
    this.itemClick.emit({item, row});
  }
  ngAfterViewInit(): void {

    this.initDataSource();
    if (!this.displayedColumns && !this.uniqTableName) {
      this.displayedColumns = this.columns.map(c => c.columnDef);
    }
    Observable.fromEvent(this.filter.nativeElement, 'keyup')
      .debounceTime(this.configService.debounceTime)
      .map((e: any) => e.target.value)
      .distinctUntilChanged()
      .filter(
        () =>
          this.calculatedDataSource instanceof MatTableDataSource &&
          this.filterable
      )
      .filter(value => value.length >= 3 || value.length === 0)
      .takeUntil(this.onDestroy)
      .subscribe(value => {
        this.calculatedDataSource.filter = value;
      });

    if (this.sortBy) {
      const sortable = this.sort.sortables.get(this.sortBy);

      if (sortable) {
        sortable.start = this.sortMode;
        this.sort.sort(sortable);
      }
    }

    this.changeDetector.detectChanges();
  }
  onSelect(row: any) {
    this.tableOpened.emit({state: true, row});
    if (this.selectable) {
      this.selectedRow = row;
      this.select.emit(row);
    } else {
      this.selectedRow = null;
    }
  }

  private initDataSource() {
    if (!this.calculatedDataSource && this.paginator && this.sort) {
      if (this.data) {
        this.calculatedDataSource = new MatTableDataSource<any>(this.data);
        this.calculatedDataSource.sort = this.sortable
          ? this.sort
          : this.calculatedDataSource.sort;
        this.calculatedDataSource.filterPredicate =
          this.filterPredicate || this.calculatedDataSource.filterPredicate;
        this.calculatedDataSource.filter =
          this.clientFilters || this.calculatedDataSource.filter;
        this.paginator.pageIndex = 0;
        this.paginator.pageSize = this.pageSize || this.configService.pageSize;
        this.paginator.pageSizeOptions = this.configService.pageSizeOptions;
        this.calculatedDataSource.paginator = this.paginable
          ? this.paginator
          : this.calculatedDataSource.paginator;
        this.resultsLength.emit(this.data.length);
      } else if (this.source) {
        this.paginator.pageSize = this.pageSize || this.configService.pageSize;
        this.paginator.pageSizeOptions = this.configService.pageSizeOptions;
        this.calculatedDataSource = new BaseDataSource<any>(
          this.source,
          this.configService,
          this.paginator,
          this.sortable ? this.sort : null,
          this.filter,
          this.onlyQueriedData,
          this.queryData || void 0
        );
        this.calculatedDataSource.resetCompleted
          .takeUntil(this.onDestroy)
          .distinctUntilChanged()
          .subscribe(() =>
            this.resultsLength.emit(this.calculatedDataSource.resultsLength)
          );
      }
    }
  }
  private  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.calculatedDataSource.data.forEach(row => this.selection.select(row));
  }
  private isAllSelected() {
    this.selection.onChange.subscribe(data => {
      this.showTick = !!data.added.length;
    })
    const numSelected = this.selection.selected.length;
    let numRows;
    if (this.calculatedDataSource) {
      numRows = this.calculatedDataSource.data.length;
    }

    return numSelected === numRows;
  }
  private showCheckBox(hasIpc?, onNumberClicked?) {
    this.selectAllView = this.protectDocs || !this.selectAllView;
    if (this.selectAllView) {
      if (!this.displayedColumns.includes('select')) {
        this.displayedColumns.unshift('select');
      }
    } else {
      const index = this.displayedColumns.indexOf('select');
      if (!this.protectDocs) {
        this.displayedColumns.splice(index, 1);
      }
    }

    const newQueries = [
      { key: 'isComplete' + Operators.equal, value: false },
      { key: 'canGenerateGosNumber' + Operators.equal, value: true },
      { key: 'IsIndustrial' + Operators.equal, value: hasIpc },
      {
        key: 'ownerType' + Operators.equal,
        value: OwnerType.ProtectionDoc
      },
      { key: '_sort', value: 'ipcCodes' },
      { key: '_order', value: 'asc' }
    ];
    if (onNumberClicked) {
      this.onNumberClicked.emit({newQueries: newQueries, hasIpc, currentDisplayColumn: this.displayedColumns});
      this.hasIpc = hasIpc;
    }
  }
  private checkboxLabel(row?): string {
    if (!row) {
      return `${this.isAllSelected() ? 'select' : 'deselect'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}`;
  }
  private dismissChecked() {
    this.selection.clear();
    this.showTick = false;
  }
  private confirmChecked() {
    this.checkedData.emit({data: this.selection.selected, isAllSelected: this.isAllSelected(), hasIpc: this.hasIpc});
  }
  private getValue(column: Config, row: any) {
    if (column.format) {
      return column.format(row);
    }

    const value = row[column.columnDef];

    if (
      this.datePattern.test(value) ||
      (value instanceof Date && !isNaN(value.valueOf()))
    ) {
      const date = new Date(value);
      if (this.showOnlyDate) {
        return `${date.toLocaleDateString()}`;
      } else {
        return `${date.toLocaleDateString()} ${date.toLocaleTimeString()}`;
      }
    }

    if (typeof value === 'number' && !Number.isInteger(value)) {
      return Math.round(value * 100) / 100;
    }

    if (value instanceof Array) {
      return value.join('; ');
    }

    return value;
  }

  private isBoolean(column: Config, row: any): boolean {
    return typeof this.getValue(column, row) === 'boolean';
  }

  public onCreateDocumentClick() {
    this.onAddButton.emit(true);
  }

  public onImportDocumentClick() {
    this.onImportButton.emit(true);
  }

  public printTable() {
    const doc = new jsPDF('p', 'pt', 'a4');
    doc.addHTML(document.getElementById('mat-table'), 10, 10,
      {pagesplit: true, margin: {top: 20, right: 20, bottom: 20, left: 20, useFor: 'page'}},
      () => {
      doc.autoPrint();
      window.open(doc.output('bloburl'), '_blank');
    });
  }

  public exportTable() {
    this.export.emit(this.displayedColumns);
  }

  trackByIndex(index, item) {
    return index;
  }
}
