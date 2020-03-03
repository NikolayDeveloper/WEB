import {
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
  ViewChild,
  AfterViewInit,
  ChangeDetectorRef
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatExpansionPanel } from '@angular/material';
import { Observable, Subject } from 'rxjs/Rx';

import { UsersService } from '../../../../administration/users.service';
import { Operators } from '../../../../shared/filter/operators';
import { DictionaryService } from '../../../../shared/services/dictionary.service';
import {
  Classification,
  DicDepartment,
  DicDocumentType,
  ExtendedTreeNode,
  toDocumentTypeTreeNodes,
  toUserTreeNodes
} from '../../../../shared/services/models/base-dictionary';
import { DictionaryType } from '../../../../shared/services/models/dictionary-type.enum';
import { SelectOption } from '../../../../shared/services/models/select-option';
import { TreeNodeService } from '../../../../shared/services/tree-node.service';
import { moment, MY_FORMATS } from '../../../../shared/shared.module';
import { numberMask } from '../../.././../shared/services/validator/custom-validators';
import { OperatorFor } from '../../../models/document-search-dto.model';
import {
  SearchType,
  SearchTypeSelectOptions
} from '../../../models/search-type.enum';
import { selectedSearchTypeKey } from '../advanced-search.component';
import { SearchMode } from '../../../../shared/components/search-mode-toggle/search-mode-toggle.component';

@Component({
  selector: 'app-document-search-form',
  templateUrl: './document-search-form.component.html',
  styleUrls: ['./document-search-form.component.scss']
})
export class DocumentSearchFormComponent
  implements OnInit, OnDestroy, AfterViewInit {
  formGroup: FormGroup;
  dicCountries: SelectOption[] = [];
  filteredDicCountries: Observable<SelectOption[]>;
  dicReceiveTypes: SelectOption[];
  searchTypeSelectOptions = SearchTypeSelectOptions;
  searchType: SearchType;
  originUserNodes: ExtendedTreeNode[];
  viewUserNodes: ExtendedTreeNode[];
  selectedNodes: ExtendedTreeNode[] = [];
  originDocumentTypeNodes: ExtendedTreeNode[];
  viewDocumentTypeNodes: ExtendedTreeNode[];
  selectedDocTypeNodes: ExtendedTreeNode[] = [];

  private onFetch = new Subject();
  private onDestroy = new Subject();

  numberMask = numberMask;

  @Input() resultsLength: Observable<number>;
  @Output() search = new EventEmitter<any>();
  @Output() reset = new EventEmitter<any>();
  @Output() selectedSearchType = new EventEmitter<SearchType>();
  @ViewChild(MatExpansionPanel) accordionItem: MatExpansionPanel;

  constructor(
    private fb: FormBuilder,
    private dictionaryService: DictionaryService,
    private usersService: UsersService,
    private treeNodeService: TreeNodeService,
    private changeDetector: ChangeDetectorRef
  ) {
    this.buildForm();
    this.searchType = JSON.parse(localStorage.getItem(selectedSearchTypeKey));
  }

  onChange(value: any) {}

  ngOnInit() {
    this.resultsLength.takeUntil(this.onDestroy).subscribe(length => {
      length > 9 ? this.accordionItem.close() : this.accordionItem.open();
    });

    Observable.combineLatest(
      this.dictionaryService.getSelectOptions(DictionaryType.DicCountry),
      this.dictionaryService.getSelectOptions(DictionaryType.DicReceiveType),
      this.dictionaryService.getBaseDictionary(
        DictionaryType.DicDocumentClassification
      ),
      this.dictionaryService.getBaseDictionary(DictionaryType.DicDocumentType),
      this.dictionaryService.getSelectOptions(DictionaryType.DicDivision),
      this.dictionaryService.getBaseDictionary(DictionaryType.DicDepartment),
      this.usersService.get()
    )
      .takeUntil(this.onDestroy)
      .subscribe(
        ([
          countries,
          receiveTypes,
          docClassifications,
          documentTypes,
          divisions,
          departments,
          users
        ]) => {
          this.dicCountries = countries.filter(c => c.nameRu);
          this.dicReceiveTypes = receiveTypes;
          this.originDocumentTypeNodes = toDocumentTypeTreeNodes(
            docClassifications as Classification[],
            documentTypes as DicDocumentType[]
          );
          this.viewDocumentTypeNodes = this.originDocumentTypeNodes;
          this.originUserNodes = toUserTreeNodes(
            divisions,
            departments as DicDepartment[],
            users
          );
          this.viewUserNodes = this.originUserNodes;
          this.onFetch.next();
        }
      );
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  ngAfterViewInit(): void {
    this.changeDetector.detectChanges();
  }

  onSubmit() {
    if (this.formGroup.invalid) {
      return;
    }
    this.formGroup.markAsPristine();

    const formValues = this.getFormValues();
    const userIds = this.selectedNodes.filter(n => n.isFinal).map(n => n.data);
    const documentTypeIds = this.selectedDocTypeNodes
      .filter(n => n.isFinal)
      .map(n => n.data);

    if (!formValues && userIds.length === 0 && documentTypeIds.length === 0) {
      this.formGroup.reset();
      return;
    }

    const queryParams = this.buildQueryParamsFrom(
      formValues,
      userIds,
      documentTypeIds
    );
    this.search.emit(queryParams);
  }

  onReset() {
    this.selectedNodes = [];
    this.selectedDocTypeNodes = [];
    this.formGroup.controls.customerCountryId.setValue('');
    this.formGroup.reset();
    this.reset.emit();
  }

  /**
  /* Запрет на выбор больше одной корневой ветки (не используется)
   *
   *
   * @memberof DocumentSearchFormComponent
   */
  onDocumentTypeSelect(value: any) {
    if (value.node.parentId === null) {
      this.selectedDocTypeNodes = this.toFlat([value.node]);
    }

    this.formGroup.markAsDirty();
  }

  onSearchTypeChange(type: SearchType) {
    this.selectedSearchType.emit(type);
  }

  toDate(value: any) {
    return moment(value, MY_FORMATS.parse.dateInput).toDate();
  }

  private getFormValues(): Object {
    const formValues = this.formGroup.getRawValue();
    const customerCountry = this.formGroup.get('customerCountryId').value;
    if (customerCountry) {
      formValues.customerCountryId = customerCountry.id;
    }
    delete formValues.searchTextUserNode;
    delete formValues.searchTextDocumentType;
    const hasValue = Object.keys(formValues).some(
      key => (formValues[key] ? true : false)
    );

    return hasValue ? formValues : null;
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      documentNum: [''],
      documentDateFrom: [''],
      documentDateTo: [''],
      description: [''],
      customerXin: ['', [Validators.maxLength(12)]],
      customerNameRu: [''],
      customerAddress: [''],
      customerCountryId: [''],
      barcode: [''],
      receiveTypeId: [''],
      outgoingNumber: [''],
      sendingDateFrom: [''],
      sendingDateTo: [''],
      searchTextUserNode: [''],
      searchTextDocumentType: [''],
      documentNum_mode: [''],
      description_mode: [''],
      customerXin_mode: [''],
      customerNameRu_mode: [''],
      customerAddress_mode: [''],
      outgoingNumber_mode: ['']
    });
  }

  private buildQueryParamsFrom(
    formValues: Object,
    userIds: number[],
    documentTypeIds: number[]
  ): any[] {
    const queryParams = [];
    const suffix = '_mode';
    if (formValues) {
      Object.keys(formValues).forEach(key => {
        const value = formValues[key];
        const modeValue = formValues[key + suffix];
        if (value) {
          let operator;
          if (modeValue) {
            switch (modeValue) {
              case SearchMode.Equals.toString():
                operator = Operators.equal;
                break;
              case SearchMode.Contains.toString():
                operator = OperatorFor[key];
                break;
            }
          } else {
            operator = OperatorFor[key];
          }
          if (operator) {
            queryParams.push({
              key: this.simplify(key) + operator,
              value: this.prepare(key, value)
            });
          }
        }
      });
    }

    if (userIds.length > 0) {
      const userIdKey = 'userId';
      queryParams.push({
        key: userIdKey + OperatorFor[userIdKey],
        value: userIds.join()
      });
    }

    if (documentTypeIds.length > 0) {
      const typeIdKey = 'typeId';
      queryParams.push({
        key: typeIdKey + OperatorFor[typeIdKey],
        value: documentTypeIds.join()
      });
    }

    this.applySort(queryParams);
    return queryParams;
  }

  private applySort(queryParams: any[]): void {
    queryParams.push(
      { key: Operators.sort, value: 'id' },
      { key: Operators.order, value: 'desc' }
    );
  }

  private simplify(key: string) {
    if (key === 'documentDateFrom' || key === 'documentDateTo') {
      return 'documentDate';
    }

    if (key === 'sendingDateFrom' || key === 'sendingDateTo') {
      return 'sendingDate';
    }

    return key;
  }

  private prepare(key: string, value: any): any {
    if (moment.isMoment(value)) {
      value = new Date(value.toDate().toDateString());
      if (key === 'documentDateTo' || key === 'sendingDateTo') {
        value.setDate(value.getDate() + 1);
      }
      return value.toJSON();
    }

    return value;
  }
  private toFlat(nodes: ExtendedTreeNode[]): ExtendedTreeNode[] {
    let flatNodes = [];
    nodes.forEach(node => {
      flatNodes.push(node);
      const childNodes = this.toFlat(node.children as ExtendedTreeNode[]);
      flatNodes = flatNodes.concat(childNodes);
    });
    return flatNodes;
  }
  resetSearchUserNode(): void {
    this.selectedNodes = [];
    this.formGroup.controls.searchTextUserNode.setValue('');
    this.viewUserNodes = this.originUserNodes;
  }
  resetSearchDocumentType(): void {
    this.selectedNodes = [];
    this.formGroup.controls.searchTextDocumentType.setValue('');
    this.viewDocumentTypeNodes = this.originDocumentTypeNodes;
  }
  searchByUserNodesTree(): void {
    const searchTextUserNode = this.formGroup.controls.searchTextUserNode.value;
    if (!searchTextUserNode) {
      return;
    }
    this.viewUserNodes = this.treeNodeService.searchByTree(
      searchTextUserNode,
      this.originUserNodes
    ) as ExtendedTreeNode[];
  }
  searchByDocumentTypesTree(): void {
    const searchTextDocumentType = this.formGroup.controls
      .searchTextDocumentType.value;
    if (!searchTextDocumentType) {
      return;
    }
    this.viewDocumentTypeNodes = this.treeNodeService.searchByTree(
      searchTextDocumentType,
      this.originDocumentTypeNodes
    ) as ExtendedTreeNode[];
  }
}
