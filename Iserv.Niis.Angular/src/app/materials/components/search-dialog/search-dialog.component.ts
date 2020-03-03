import {
  Component,
  ElementRef,
  Inject,
  OnInit,
  ViewChild
} from '@angular/core';
import { OnDestroy } from '@angular/core/src/metadata/lifecycle_hooks';
import { FormBuilder, FormGroup } from '@angular/forms';
import {
  MatCheckboxChange,
  MatDialogRef,
  MatIconRegistry,
  MatPaginator,
  MatSort,
  MatTableDataSource,
  MAT_DIALOG_DATA
} from '@angular/material';
import { DomSanitizer } from '@angular/platform-browser';
import { Subject } from 'rxjs/Subject';
import { ConfigService } from '../../../core';
import { SelectionMode } from '../../../journal/components/journal-tasks/journal-tasks.component';
import { IntellectualPropertySearchDto } from '../../../search/models/intellectual-property-search-dto';
import { BaseDataSource } from '../../../shared/base-data-source';
import { OwnerType } from '../../../shared/services/models/owner-type.enum';
import { MaterialOwnerDto } from '../../models/materials.model';
import { AttachmentSearchService } from '../../services/attachment-search.service';
import { Operators } from '../../../shared/filter/operators';
import { DictionaryService } from '../../../shared/services/dictionary.service';
import { DictionaryType } from '../../../shared/services/models/dictionary-type.enum';
import { DocumentKind } from '../../../shared/models/create-document-model';
import { SelectOption } from '../../../shared/services/models/select-option';

@Component({
  selector: 'app-search-dialog',
  templateUrl: './search-dialog.component.html',
  styleUrls: ['./search-dialog.component.scss']
})
export class SearchDialogComponent implements OnDestroy, OnInit {
  formGroup: FormGroup;

  dataSource: BaseDataSource<IntellectualPropertySearchDto> | null;
  isAllSelected = false;
  isSimple = false;
  selectionMode = SelectionMode.Including;
  selectedItems: IntellectualPropertySearchDto[] = [];
  queryParams: any[];
  protectionDocTypes: SelectOption[];
  ownerType: any;
  selectedKind = DocumentKind.Request;
  protectionDocTypeId: any;
  isDisabledSelectForm = false;
  ownerTypes: OwnerTypeSearchMode[] = [
    {
      nameRu: 'Заявки',
      kind: DocumentKind.Request as number
    },
    {
      nameRu: 'ОД',
      kind: DocumentKind.ProtectionDoc as number
    },
    {
      nameRu: 'Договора',
      kind: DocumentKind.Contract as number
    }
  ];

  private onDestroy = new Subject();

  regNumbers = {
    key: 'number' + Operators.equal,
    value: ''
  };
  requestColumns: string[];
  ipDatasource: MatTableDataSource<IntellectualPropertySearchDto>;

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild('filter') filter: ElementRef;

  private readonly requestCodes = ['B', 'U', 'S2', 'TM', 'PN', 'SA'];
  private readonly protectionDocCodes = ['B_PD', 'U_PD', 'S2_PD', 'TM_PD', 'PN_PD', 'SA_PD'];

  constructor(
    private configService: ConfigService,
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<SearchDialogComponent>,
    private attachmentSearchService: AttachmentSearchService,
    private iconRegistry: MatIconRegistry,
    private sanitizer: DomSanitizer,
    private dictionaryService: DictionaryService,
    @Inject(MAT_DIALOG_DATA) private data: any
  ) {
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
    if (data.selectedIntellectualProperties) {
      this.selectedItems = data.selectedIntellectualProperties;
    }

    this.isSimple = data.isSimple;

    if (data.ownerType) {
      this.ownerType = data.ownerType;
    }

    if (data.protectionDocTypeId) {
      this.protectionDocTypeId = data.protectionDocTypeId;
      // this.isDisabledSelectForm = true;
    }

    this.buildForm();
    this.requestColumns = [
      'checkboxes',
      'barcode',
      'regNumber',
      'kind',
      'dateCreate',
      'typeName',
      'nameRu'
    ];
  }

  ngOnDestroy(): void {
    this.selectedItems = [];
    this.onDestroy.next();
  }

  ngOnInit(): void {
    this.getProtectionDocTypes();

    this.formGroup.get('kind').valueChanges
      .subscribe(() => {
        this.getProtectionDocTypes();
      });

    this.paginator.pageIndex = 0;
    this.paginator.pageSize = this.configService.pageSize;
    this.paginator.pageSizeOptions = this.configService.pageSizeOptions;
    let source;
    source = new BaseDataSource<IntellectualPropertySearchDto>(
      this.attachmentSearchService,
      this.configService,
      this.paginator,
      this.sort,
      this.filter,
      false,
      [
        {
          key: 'type' + Operators.equal,
          value: DocumentKind.Request as number
        },
        this.regNumbers
      ]
    );
    this.dataSource = source;

    this.formGroup.get('regNumbers')
      .valueChanges.takeUntil(this.onDestroy)
      .debounceTime(this.configService.debounceTime)
      .subscribe(v => this.regNumbersOnChange(v));

    this.formGroup
      .get('filter')
      .valueChanges.takeUntil(this.onDestroy)
      .debounceTime(this.configService.debounceTime)
      .filter(value => !!value)
      .subscribe(v => this.resetSelection());

    const protectionDocTypeId = !!this.protectionDocTypeId ? this.protectionDocTypeId : 1;

    if (this.ownerType) {
      if (this.ownerType === OwnerType.Request) {
        this.ownerTypes = this.ownerTypes.filter(t => t.kind === DocumentKind.Request);
        this.selectedKind = DocumentKind.Request;
        this.formGroup.get('kind').patchValue(DocumentKind.Request);
        this.formGroup.get('requestTypeId').patchValue(protectionDocTypeId);
        this.onRequestTypeChange(protectionDocTypeId);
      } else if (this.ownerType === OwnerType.ProtectionDoc) {
        this.ownerTypes = this.ownerTypes.filter(t => t.kind === DocumentKind.ProtectionDoc);
        this.selectedKind = DocumentKind.ProtectionDoc;
        this.formGroup.get('kind').patchValue(DocumentKind.ProtectionDoc);
        this.formGroup.get('protectionDocTypeId').patchValue(protectionDocTypeId);
        this.onProtectionDocTypeChange(protectionDocTypeId);
      }
    }
  }

  getProtectionDocTypes(): void {
    const selectedKind = this.formGroup.get('kind').value;

    if (selectedKind === DocumentKind.Request || selectedKind === DocumentKind.ProtectionDoc) {
      const codes =  selectedKind === DocumentKind.Request ? this.requestCodes : this.protectionDocCodes;

      this.dictionaryService
        .getBaseDictionaryByCodes(DictionaryType.DicProtectionDocType, codes)
        .takeUntil(this.onDestroy)
        .subscribe(protectionDocTypes => {
          this.protectionDocTypes = protectionDocTypes;
        });
    } else {
      this.protectionDocTypes = [];
    }
  }

  attachButtonDisabled() {
    return this.selectedItems.length === 0 && this.isAllSelected === false;
  }

  onSubmit() {
    const filterParams = this.dataSource.getQueryParams();

    const params = [
      ...filterParams,
      { key: 'isAllSelected', value: this.isAllSelected },
      { key: 'selectionMode', value: this.selectionMode }
    ];
    this.attachmentSearchService
      .getSelectedOwners(this.selectedItems, params)
      .takeUntil(this.onDestroy)
      .subscribe((selection: IntellectualPropertySearchDto[]) => {
        const owners = selection.map(sr => {
          const owner = new MaterialOwnerDto();
          owner.ownerType = sr.type;
          owner.ownerId = sr.id;
          owner.protectionDocTypeId = sr.protectionDocTypeId;
          owner.addressee = sr.addressee;
          return owner;
        });
        this.dialogRef.close(
          new Result(owners, this.selectedItems.map(r => r.number).join(', '), this.protectionDocTypeId)
        );
      });
  }

  onCancel() {
    this.dialogRef.close();
  }

  getSelectedKind(): number {
    return this.selectedKind as number;
  }

  isChecked(row: IntellectualPropertySearchDto): boolean {
    return (
      this.isAllSelected ||
      (this.selectionMode === SelectionMode.Including &&
        this.selectedItems.some(
          si => si.id === row.id && si.type === row.type
        )) ||
      (this.selectionMode === SelectionMode.Except &&
        !this.selectedItems.some(
          si => si.id === row.id && si.type === row.type
        ))
    );
  }

  onCheck(row: IntellectualPropertySearchDto) {
    const isChecked = !this.selectedItems.some(
      s => s.id === row.id && s.type === row.type
    );
    if (this.selectionMode === SelectionMode.Except) {
      this.isAllSelected = false;
    }
    if (isChecked) {
      this.isSimple ? this.selectedItems = [row]
        : this.selectedItems.push(row);
    } else { this.selectedItems.splice(
          this.selectedItems.findIndex(
            si => si.id === row.id && si.type === row.type
          ),
          1
        );
    }
  }

  isAllChecked(): boolean {
    return (
      this.isAllSelected ||
      this.dataSource.data.every(d => this.selectedItems.includes(d))
    );
  }

  checkAll(isChecked: MatCheckboxChange) {
    this.selectedItems = [];
    this.isAllSelected = isChecked.checked;
    isChecked.checked
      ? (this.isAllSelected = true)
      : (this.isAllSelected = false);
    if (!this.isAllSelected) {
      this.selectionMode = SelectionMode.Including;
    } else {
      this.selectionMode = SelectionMode.Except;
    }
  }

  isSomeChecked(): boolean {
    return (
      this.selectedItems.length > 0 &&
      this.selectedItems.length < this.dataSource.data.length
    );
  }

  regNumbersOnChange(value: string) {
    this.regNumbers = {
      key: 'number' + Operators.equal,
      value: value
    };

    this.dataSource.update(this.regNumbers);
  }

  onKindChange(value: DocumentKind) {
    if (value) {
      this.selectedKind = value;
      this.dataSource.reset([
        {
          key: 'type' + Operators.equal,
          value: value as number
        },
        this.regNumbers
      ]);
    } else {
      this.dataSource.reset();
    }
    this.resetSelection();
    this.formGroup.get('requestTypeId').patchValue(1); // изобретения
    this.formGroup.get('protectionDocTypeId').patchValue(1); // изобретения
  }

  onProtectionDocTypeChange(value: number) {
    if (value) {
      this.dataSource.reset([
        {
          key: 'typeId' + Operators.equal,
          value: value
        },
        this.regNumbers
      ]);
    } else {
      this.dataSource.reset();
    }
    this.protectionDocTypeId = value;
    this.resetSelection();
  }

  onRequestTypeChange(value: number) {
    if (value) {
      this.dataSource.reset([
        {
          key: 'requestTypeId' + Operators.equal,
          value: value
        },
        this.regNumbers
      ]);
    } else {
      this.dataSource.reset();
    }
    this.protectionDocTypeId = value;
    this.resetSelection();
  }

  resetSelection() {
    this.selectedItems = [];
    this.selectionMode = SelectionMode.Including;
    this.isAllSelected = false;
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      regNumbers: [''],
      ownerNumbers: [''],
      protectionDocTypeId: [''],
      requestTypeId: [''],
      intellectualPropertyTypeId: [''],
      filter: [''],
      kind: [OwnerType.Request]
    });
  }
}

export class Result {
  constructor(public owners: MaterialOwnerDto[],
              public numbers: string,
              public protectionDoсTypeId: number) {}
}

export class OwnerTypeSearchMode {
  nameRu: string;
  kind: DocumentKind;
}
