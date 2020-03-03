import { SelectionModel } from '@angular/cdk/collections';
import {
  AfterViewInit,
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { OnChanges } from '@angular/core/src/metadata/lifecycle_hooks';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog, MatPaginator, MatSort } from '@angular/material';
import {
  ColumnConfig,
  ColumnConfigDialogComponent,
  ColumnConfigService,
} from 'app/modules/column-config';
import { RequestDetails } from 'app/requests/models/request-details';
import { Subject } from 'rxjs/Rx';
import { RequestService } from '../../../requests/request.service';
import { ImageDataSource } from '../../../shared/image-data-source';
import { ImageDialog } from '../../image-dialog';
import { SimilarProtectionDocDto } from 'app/expert-search/models/similar-protection-doc-dto';

const columnsConfigKey = 'expert_search_similarities_columns_config';
const defaultColumnsConfig: ColumnConfig[] = [
  { field: 'select', name: 'Выбрать', enabled: true },
  //{ field: 'barcode', name: 'Штрихкод', enabled: false },
  //{ field: 'requestTypeNameRu', name: 'Тип заявки', enabled: false },
  { field: 'statusNameRu', name: 'Статус', enabled: true },
  { field: 'previewImage', name: 'Изображение', enabled: true },
  { field: 'gosNumber', name: '№ ОД', enabled: true },
  { field: 'gosDate', name: 'Дата ОД', enabled: true },
  { field: 'regNumber', name: 'Рег. номер заявки', enabled: true },
  { field: 'regDate', name: 'Дата подачи заявки', enabled: true },
  { field: 'nameRu', name: 'Наименование на рус. яз.', enabled: true },
  { field: 'nameKz', name: 'Наименование на каз. яз.', enabled: true },
  { field: 'nameEn', name: 'Наименование на англ. яз.', enabled: true },
  { field: 'declarant', name: 'Заявитель', enabled: true },
  { field: 'ownerName', name: 'Владелец', enabled: true },
  //{ field: 'patentAttorney', name: 'Патентный поверенный', enabled: true },
  //{ field: 'addressForCorrespondence', name: 'Адрес для переписки', enabled: true },
  //{ field: 'confidant', name: 'Доверенное лицо', enabled: true },
  //{ field: 'receiveTypeNameRu', name: 'Тип подачи заявки', enabled: true },
  { field: 'icgs', name: 'МКТУ', enabled: true },
  { field: 'icfems', name: 'МКИЭТЗ', enabled: true },
  { field: 'colors', name: 'Цвета', enabled: true },
  //{ field: 'transliteration', name: 'Транслитерация', enabled: true },
  { field: 'priorityData', name: 'Приоритетные данные', enabled: true },
  //{ field: 'numberBulletin', name: 'Номер бюллетеня', enabled: true },
  //{ field: 'publicDate', name: 'Дата публикации', enabled: true },
  { field: 'validDate', name: 'Срок действия', enabled: true },
  { field: 'extensionDateTz', name: 'Дата продления', enabled: true },
  { field: 'disclaimerRu', name: 'Дискламация', enabled: true },
  { field: 'disclaimerKz', name: 'Дискламация каз', enabled: true },
  { field: 'gosreestr', name: 'Делопроизводство', enabled: true },
  { field: 'protectionDocCategory', name: 'Примечание', enabled: true }
];

@Component({
  selector: 'app-similar-protection-docs',
  templateUrl: './similar-protection-docs.component.html',
  styleUrls: ['./similar-protection-docs.component.scss'],
})
export class SimilarProtectionDocsComponent
  implements OnInit, AfterViewInit, OnChanges {
  displayedColumns: string[];
  imageDataSource: ImageDataSource<SimilarProtectionDocDto>;
  selectionModel = new SelectionModel<SimilarProtectionDocDto>(true, []);
  formGroup: FormGroup;
  expertSearchKeywords: string;

  @Input() requestId: number;
  @Input() checkedSearchDtos: SimilarProtectionDocDto[];
  @Input() resultsLength: number;
  @Input() protectionDocTypeCode: string;
  @Output() modifiedData = new EventEmitter<RequestDetails>();
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @Output() delete = new EventEmitter<SimilarProtectionDocDto[]>();
  @Output() save = new EventEmitter<{ keywords: string, checkedSearchDtos: SimilarProtectionDocDto[] }>();

  private onDestroy = new Subject();

  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog,
    private columnConfigService: ColumnConfigService,
    private requestService: RequestService
  ) {
    this.buildForm();
  }

  ngOnInit() {
    this.displayedColumns = this.columnConfigService.get(columnsConfigKey, defaultColumnsConfig)
      .filter(cc => cc.enabled)
      .map(cc => cc.field);
    this.requestService
      .getRequestById(this.requestId)
      .takeUntil(this.onDestroy)
      .subscribe(request => {
        this.formGroup.get('query').setValue(request.expertSearchKeywords);
      });
    this.initSearchTable();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.imageDataSource && this.checkedSearchDtos) {
      this.selectionModel.clear();
      this.imageDataSource.push(this.checkedSearchDtos);
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

  ngAfterViewInit(): void {
    this.paginator.pageIndex = 0;
  }

  openImageDialog(url: string): void {
    this.dialog.open(ImageDialog, {
      data: url,
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

  isAllSelected() {
    const numSelected = this.selectionModel.selected.length;
    const numRows = this.imageDataSource.data.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected()
      ? this.selectionModel.clear()
      : this.imageDataSource.data.forEach(row =>
        this.selectionModel.select(row)
      );
  }

  onSave() {
    const expertSearchKeywords = this.formGroup.get('query').value;
    this.save.emit({ keywords: expertSearchKeywords, checkedSearchDtos: this.imageDataSource.data });
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      query: [''],
    });
  }

  private initSearchTable() {
    this.imageDataSource = new ImageDataSource<SimilarProtectionDocDto>(
      this.sort,
      this.paginator
    );
  }
}
