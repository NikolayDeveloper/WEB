import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  SimpleChanges,
  ViewChild
} from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import {
  DocumentType,
  getDocumentTypeName,
  getDocumentTypeRoute,
  MaterialItem
} from 'app/materials/models/materials.model';
import { IntellectualPropertyDetails } from 'app/shared/models/intellectual-property-details';
import { Subject } from 'rxjs/Subject';
import { Config } from '../../../shared/components/table/config.model';
import { TableComponent } from '../../../shared/components/table/table.component';
import { MaterialsService } from '../../services/materials.service';

function filterPredicate(materialData: MaterialItem, value: string): boolean {
  const values = value.split('$');
  const isOfType =
    !values[1] ||
    values[1] === '2' ||
    materialData.documentType.toString() === values[1];
  const isFullTextSearchValid =
    !values[0] ||
    ((!!materialData.documentNum &&
      materialData.documentNum.toLowerCase().includes(values[0])) ||
      (!!materialData.typeNameRu &&
        materialData.typeNameRu.toLowerCase().includes(values[0])) ||
      (!!materialData.status &&
        materialData.status.toLowerCase().includes(values[0])) ||
      (!!materialData.executor &&
        materialData.executor.toLowerCase().includes(values[0])) ||
      (!!materialData.initiator &&
        materialData.initiator.toLowerCase().includes(values[0])) ||
      (!!materialData.id &&
        materialData.id
          .toString()
          .toLowerCase()
          .includes(values[0])) ||
      (!!materialData.dateCreate &&
        materialData.dateCreate
          .toString()
          .toLowerCase()
          .includes(values[0])));
  return isOfType && isFullTextSearchValid;
}

@Component({
  selector: 'app-materials-list',
  templateUrl: './materials-list.component.html',
  styleUrls: ['./materials-list.component.scss']
})
export class MaterialsListComponent implements OnInit, OnChanges, OnDestroy {
  private onDestroy = new Subject();
  filterPredicate = filterPredicate;
  columns: Config[] = [
    new Config({ columnDef: 'id', header: 'ID', class: 'width-50' }),
    new Config({
      columnDef: 'documentType',
      header: 'Тип документа',
      class: 'width-250',
      format: row => this.getDocumentTypeName.call(this, row.documentType)
    }),
    new Config({ columnDef: 'typeNameRu', header: 'Тип', class: 'width-300' }),
    new Config({
      columnDef: 'dateCreate',
      header: 'Дата создания',
      class: 'width-200'
    }),
    new Config({
      columnDef: 'documentNum',
      header: 'Номер документа',
      class: 'width-150'
    }),
    new Config({ columnDef: 'initiator', header: 'Пользователь', class: 'width-250' }),
    new Config({ columnDef: 'executor', header: 'Исполнитель', class: 'width-250' }),
    new Config({ columnDef: 'status', header: 'Этап', class: 'width-250' }),
    new Config({
      columnDef: 'preview',
      header: 'Предпросмотр',
      class: 'width-25 sticky',
      icon: 'zoom_in',
      click: row =>
        this.onAttachmentClick.call(
          this,
          row.id,
          row.name,
          false,
          row.hasTemplate
        ),
      disable: row => {
        // !row.canDownload
        return this.disabled;
      }
    }),
    new Config({
      columnDef: 'download',
      header: 'Загрузить',
      class: 'width-25 sticky',
      icon: 'file_download',
      click: row =>
        this.onAttachmentClick.call(
          this,
          row.id,
          row.name,
          true,
          row.hasTemplate
        ),
      disable: row => {
        // !row.canDownload
        return this.disabled;
      }
    })
  ];
  clientFilters: string;
  materials: MaterialItem[];
  formGroup: FormGroup;
  dicMaterialTypes = materialsKindConst;
  ownerKey: string;

  @Input() owner: IntellectualPropertyDetails;
  @Input() disabled: boolean;
  @Output() attachmentClick = new EventEmitter<any>();

  @ViewChild(TableComponent) tableComponent: TableComponent;

  get source() {
    return this.materialsService;
  }

  constructor(
    private router: Router,
    private materialsService: MaterialsService
  ) {}

  ngOnInit() {}

  ngOnDestroy(): void {
    this.materials = [];
    this.onDestroy.next();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.owner && changes.owner.currentValue) {
      this.materialsService
        .getByOwner(this.owner.id, this.owner.ownerType)
        .takeUntil(this.onDestroy)
        .subscribe(data => (this.materials = data));
    }
  }

  applyFilter(type: DocumentType) {
    const inputValue = this.tableComponent.filter.nativeElement.value;
    const filterValue = `${inputValue}$${type}`.toLowerCase();
    this.clientFilters = filterValue;
  }

  onAttachmentClick(id, name, isDownload, hasTemplate) {
    this.attachmentClick.emit({
      id: id,
      name: name,
      isDownload: isDownload,
      hasTemplate: hasTemplate
    });
  }

  onSelect(record: MaterialItem) {
    const link = [`${getDocumentTypeRoute(record.documentType)}/${record.id}`];
    this.router.navigate(link);
  }

  getDocumentTypeName(type: DocumentType) {
    return getDocumentTypeName(type);
  }
}

const materialsKindConst = [
  { kind: 2, nameRu: 'Все' },
  { kind: DocumentType.Incoming as number, nameRu: 'Входящие' },
  { kind: DocumentType.Outgoing as number, nameRu: 'Исходящие' },
  { kind: DocumentType.Internal as number, nameRu: 'Внутренние' },
  { kind: DocumentType.DocumentRequest as number, nameRu: 'Документ Заявки' }
];
