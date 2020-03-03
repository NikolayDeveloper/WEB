import { Component, OnInit, Input, Output, EventEmitter, SimpleChanges, OnChanges, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { Config } from 'app/shared/components/table/config.model';
import { FormGroup, FormBuilder } from '@angular/forms';
import { DocumentLinkDto, MaterialTask, DocumentType, getDocumentTypeRoute } from 'app/materials/models/materials.model';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import { MatDialog } from '@angular/material';
import { MaterialListComponent } from '../material-list/material-list.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-documen-link',
  templateUrl: './documen-link.component.html',
  styleUrls: ['./documen-link.component.scss']
})
export class DocumenLinkComponent implements OnInit, OnChanges, OnDestroy  {

  stateChanges = new Subject<void>();
  private _disabled = false;
  private onDestroy = new Subject();
  columns: Config[] = [
    new Config({
      columnDef: 'childDocumentTypeName',
      header: 'Тип документа',
      class: 'width-200'
    }),
    new Config({ columnDef: 'childDocumentNumber', header: 'Номер документа', class: 'width-200' }),
    new Config({
      columnDef: 'open',
      header: 'Открыть',
      class: 'width-25 sticky',
      icon: 'zoom_in',
      click: row => this.openLink(row),
      disable: row => false
    }),
    new Config({
      columnDef: 'delete',
      header: 'Удалить',
      class: 'width-25 sticky',
      icon: 'delete',
      click: row => this.deleteLink(row),
      disable: row => this.disabled
    }),
  ];

  parentColumns: Config[] = [
    new Config({
      columnDef: 'parentDocumentTypeName',
      header: 'Тип документа',
      class: 'width-100'
    }),
    new Config({ columnDef: 'parentDocumentNumber', header: 'Номер документа', class: 'width-100' }),
    new Config({
      columnDef: 'open',
      header: 'Открыть',
      class: 'width-25 sticky',
      icon: 'zoom_in',
      click: row => this.openParentLink(row),
      disable: row => false
    })
  ];

  formGroup: FormGroup;
  tableParentLinks: DocumentLinkDto[];
  tableLinks: DocumentLinkDto[];
  newLinks: DocumentLinkDto[];

  @Input()
  get parentLinks(): DocumentLinkDto[] {
    return this.tableParentLinks;
  }

  set parentLinks(value: DocumentLinkDto[]) {
    this.tableParentLinks = value;
    this.stateChanges.next();
  }

  @Input()
  get links(): DocumentLinkDto[] {
    return this.tableLinks;
  }

  set links(value: DocumentLinkDto[]) {
    this.tableLinks = value;
    this.stateChanges.next();
  }

  @Input() documentId: number[];
  @Output() changed = new EventEmitter<DocumentLinkDto>();
  @Output() linkRemove = new EventEmitter<DocumentLinkDto>();
  @Input()
  get disabled() {
    return this._disabled;
  }
  set disabled(value) {
    this._disabled = coerceBooleanProperty(value);
    this.stateChanges.next();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.disabled) {
      this.setDisabledState(changes.disabled.currentValue);
    }
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  constructor(
    private fb: FormBuilder,
    private dialog: MatDialog,
    private router: Router
  ) {
    this.tableParentLinks = [];
    this.tableLinks = [];
    this.newLinks = [];

    this.buildForm();
  }

  onClick() {
    const dialogRef = this.dialog.open(MaterialListComponent, {
      width: '880px'
    });

    dialogRef.afterClosed().subscribe((materialTask: MaterialTask) => {
      if (materialTask) {
        this.addLink(materialTask);
      }
    });
  }

  openParentLink(value: DocumentLinkDto) {
    this.router.navigate([
      getDocumentTypeRoute(value.parentDocumentType),
      value.parentDocumentId
    ]);
  }

  openLink(value: DocumentLinkDto) {
    this.router.navigate([
      getDocumentTypeRoute(value.childDocumentType),
      value.childDocumentId
    ]);
  }

  addLink(value: MaterialTask) {
    const link = new DocumentLinkDto();

    if (this.tableLinks.some(d => d.childDocumentId === value.id && d.needRemove !== true)) {
      return;
    }

    link.childDocumentId = value.id;
    link.childDocumentTypeName = value.typeNameRu;
    link.childDocumentType = DocumentType.Incoming;

    if (value.documentType === DocumentType.Incoming) {
      link.childDocumentNumber = value.displayNumber.toString();
    } else {
      link.childDocumentNumber = value.barcode.toString();
    }


    this.tableLinks = [...this.tableLinks, link];
    this.newLinks = [...this.newLinks, link];

    this.changed.emit(link);
  }

  deleteLink(row: any) {
    this.tableLinks = [...this.tableLinks.filter(d => d.childDocumentId === row.childDocumentId)];
    this.linkRemove.emit(row);
  }

  ngOnInit() {
    this.setDisabledState(this.disabled);
  }

  ngOnDestroy(): void {
    this.stateChanges.complete();
    this.onDestroy.next();
  }

  private buildForm() {
    this.formGroup = this.fb.group({
    });
  }
}
