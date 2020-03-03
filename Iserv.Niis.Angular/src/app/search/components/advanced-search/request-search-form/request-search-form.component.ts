import { Component, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatExpansionPanel } from '@angular/material';
import { Observable, Subject } from 'rxjs/Rx';

import { UsersService } from '../../../../administration/users.service';
import { Operators } from '../../../../shared/filter/operators';
import { DictionaryService } from '../../../../shared/services/dictionary.service';
import {
  DicDepartment,
  DicProtectionDocSubType,
  DicProtectionDocType,
  DicRouteStage,
  ExtendedTreeNode,
  toUserTreeNodes,
} from '../../../../shared/services/models/base-dictionary';
import { DictionaryType } from '../../../../shared/services/models/dictionary-type.enum';
import { SelectOption } from '../../../../shared/services/models/select-option';
import { TreeNodeService } from '../../../../shared/services/tree-node.service';
import { moment, MY_FORMATS } from '../../../../shared/shared.module';
import { numberMask } from '../../.././../shared/services/validator/custom-validators';
import { OperatorFor } from '../../../models/request-search-dto.model';
import { SearchType, SearchTypeSelectOptions } from '../../../models/search-type.enum';
import { selectedSearchTypeKey } from '../advanced-search.component';
import { SearchMode } from '../../../../shared/components/search-mode-toggle/search-mode-toggle.component';
import { startWith, map } from 'rxjs/operators';

@Component({
  selector: 'app-request-search-form',
  templateUrl: './request-search-form.component.html',
  styleUrls: ['./request-search-form.component.scss']
})
export class RequestSearchFormComponent implements OnInit, OnDestroy, AfterViewInit {
  formGroup: FormGroup;
  dicRequestStatuses: SelectOption[];
  dicProtectionDocTypes: DicProtectionDocType[];
  dicPDSubTypes: DicProtectionDocSubType[];
  filteredPDSubTypes: SelectOption[];
  dicRouteStages: DicRouteStage[];
  availableRouteStages: DicRouteStage[];
  filteredRouteStages: Observable<SelectOption[]>;
  filteredObservableRouteStages: Observable<DicRouteStage[]>;
  dicCountries: SelectOption[] = [];
  filteredDicCountries: Observable<SelectOption[]>;
  dicReceiveTypes: SelectOption[];
  searchTypeSelectOptions = SearchTypeSelectOptions;
  searchType: SearchType;
  originUserNodes: ExtendedTreeNode[];
  viewUserNodes: ExtendedTreeNode[];
  selectedNodes: ExtendedTreeNode[] = [];

  requestTypeCodes = [
    'B',
    'U',
    'S2',
    'TM',
    'PN',
    'SA',
    'ITM',
  ];

  private onFetch = new Subject();
  private onDestroy = new Subject();

  numberMask = numberMask;

  @Input() resultsLength: Observable<number>;
  @Output() search = new EventEmitter<any>();
  @Output() reset = new EventEmitter<any>();
  @Output() selectedSearchType = new EventEmitter<SearchType>();
  @ViewChild(MatExpansionPanel) accordionItem: MatExpansionPanel;

  constructor(private fb: FormBuilder,
    private dictionaryService: DictionaryService,
    private usersService: UsersService,
    private treeNodeService: TreeNodeService,
    private changeDetector: ChangeDetectorRef) {
    this.buildForm();
    this.searchType = JSON.parse(localStorage.getItem(selectedSearchTypeKey));
  }

  onChange(value: any) {}

  ngOnInit() {
    this.resultsLength
      .takeUntil(this.onDestroy)
      .subscribe(length => {
        length > 9
          ? this.accordionItem.close()
          : this.accordionItem.open();
      });

    Observable.combineLatest(
      this.dictionaryService.getBaseDictionary(DictionaryType.DicRequestStatus),
      this.dictionaryService.getBaseDictionary(DictionaryType.DicProtectionDocType),
      this.dictionaryService.getBaseDictionary(DictionaryType.DicProtectionDocSubType),
      this.dictionaryService.getBaseDictionary(DictionaryType.DicRouteStage),
      this.dictionaryService.getSelectOptions(DictionaryType.DicCountry),
      this.dictionaryService.getSelectOptions(DictionaryType.DicReceiveType),
      this.dictionaryService.getSelectOptions(DictionaryType.DicDivision),
      this.dictionaryService.getBaseDictionary(DictionaryType.DicDepartment),
      this.usersService.get()
    )
      .takeUntil(this.onDestroy)
      .subscribe(([requestStatuses, pdTypes, pdSubTypes, routeStages, countries, receiveTypes, divisions, departments, users]) => {
        this.dicRequestStatuses = requestStatuses;
        this.dicProtectionDocTypes = pdTypes.filter(t => this.requestTypeCodes.some(d => d === t.code)) as DicProtectionDocType[];
        this.dicPDSubTypes = pdSubTypes as DicProtectionDocSubType[];
        this.dicRouteStages = routeStages as DicRouteStage[];
        this.dicCountries = countries.filter(c => c.nameRu);
        this.dicReceiveTypes = receiveTypes;
        this.originUserNodes = toUserTreeNodes(divisions, departments as DicDepartment[], users as any);
        this.viewUserNodes = this.originUserNodes;
        this.onFetch.next();
      });

    this.filteredRouteStages = this.formGroup.controls['currentStageId'].valueChanges
      .pipe(
          startWith(''),
          map(value => typeof value === 'string' ? value : value ? (value as SelectOption).nameRu : null),
          map(name => name ? this._filter(name) : this.availableRouteStages ? this.availableRouteStages.slice() : [])
      );
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  ngAfterViewInit(): void {
    this.changeDetector.detectChanges();
  }

  /**
   * Фильтрует массив `availableRouteStages` по полю `nameRu`
   * @param name Что искать в поле `nameRu`
   * @return Отфильтрованный массив
   */
  private _filter(name: string): SelectOption[] {
    const filterValue = name.toLowerCase();

    return this.availableRouteStages.filter(option => option.nameRu.toLowerCase().includes(filterValue));
  }

  onSubmit() {
    if (this.formGroup.invalid) { return; }
    this.formGroup.markAsPristine();

    const formValues = this.getFormValues();
    const userIds = this.selectedNodes.filter(n => n.isFinal).map(n => n.data);

    if (!formValues && userIds.length === 0) {
      this.formGroup.reset();
      return;
    }

    const queryParams = this.buildQueryParamsFrom(formValues, userIds);
    this.search.emit(queryParams);
  }

  onReset() {
    this.selectedNodes = [];
    this.formGroup.controls.currentStageId.disable();
    this.formGroup.controls.requestTypeId.disable();
    this.formGroup.controls.workflowDateFrom.disable();
    this.formGroup.controls.workflowDateTo.disable();
    this.formGroup.controls.customerCountryId.setValue('');
    this.viewUserNodes = this.originUserNodes;
    this.formGroup.reset();
    this.reset.emit();
  }

  onProtectionDocTypeChange(typeId: number) {
    const routeId = this.dicProtectionDocTypes.filter(r => r.id === typeId)[0].routeId;
    this.availableRouteStages = this.dicRouteStages.filter(s => s.routeId === routeId);
    this.formGroup.controls.currentStageId.reset();
    this.formGroup.controls.currentStageId.enable();

    this.filteredPDSubTypes = this.dicPDSubTypes.filter(st => st.typeId === typeId);
    this.formGroup.controls.requestTypeId.enable();
  }

  onSearchTypeChange(type: SearchType) {
    this.selectedSearchType.emit(type);
  }

  onStageChange(value: any) {
    this.formGroup.controls.workflowDateFrom.enable();
    this.formGroup.controls.workflowDateTo.enable();
  }

  toDate(value: any) {
    return moment(value, MY_FORMATS.parse.dateInput).toDate();
  }

  private getFormValues(): Object {
    const formValues = this.formGroup.getRawValue();
    delete formValues.searchText;
    const currentStage = this.formGroup.get('currentStageId').value;
    if (currentStage) {
      const typeId = this.formGroup.get('protectionDocTypeId').value;
      const routeId = this.dicProtectionDocTypes.filter(r => r.id === typeId)[0].routeId;
      const stage = this.dicRouteStages.find(routeStage => (routeStage.nameRu === currentStage && routeStage.routeId === routeId));
      if (stage) {
        formValues.currentStageId = stage.id;
      }
    }
    const customerCountry = this.formGroup.get('customerCountryId').value;
    if (customerCountry) {
      formValues.customerCountryId = customerCountry.id;
    }
    const hasValue = Object.keys(formValues).some(key => formValues[key] ? true : false);

    return hasValue ? formValues : null;
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      statusId: [''],
      protectionDocTypeId: [''],
      requestTypeId: [{ value: '', disabled: true }],
      currentStageId: [{ value: '', disabled: true }],
      workflowDateFrom: [{ value: '', disabled: true }],
      workflowDateTo: [{ value: '', disabled: true }],
      requestNum: [''],
      barcode: [''],
      incomingNumber: [''],
      requestDateFrom: [''],
      requestDateTo: [''],
      name: [''],
      customerXin: ['', [Validators.maxLength(12)]],
      customerNameRu: [''],
      customerAddress: [''],
      customerCountryId: [''],
      receiveTypeId: [''],
      searchText: [''],
      requestNum_mode: [''],
      name_mode: [''],
      customerXin_mode: [''],
      customerNameRu_mode: [''],
      customerAddress_mode: [''],
    });
  }

  private buildQueryParamsFrom(formValues: Object, userIds: number[]): any[] {
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
      queryParams.push({ key: userIdKey + OperatorFor[userIdKey], value: userIds.join() });
    }

    this.applySort(queryParams);
    return queryParams;
  }

  private applySort(queryParams: any[]) {
    queryParams.push(
      { key: Operators.sort, value: 'id' },
      { key: Operators.order, value: 'desc' }
    );
    return queryParams;
  }

  private simplify(key: string) {
    if (key === 'requestDateFrom' || key === 'requestDateTo') {
      return 'requestDate';
    }

    if (key === 'workflowDateFrom' || key === 'workflowDateTo') {
      return 'workflowDate';
    }

    return key;
  }

  private prepare(key: string, value: any): any {
    if (moment.isMoment(value)) {
      value = new Date(value.toDate().toDateString());
      if (key === 'requestDateTo' || key === 'workflowDateTo') {
        value.setDate(value.getDate() + 1);
      }
      return value.toJSON();
    }

    return value;
  }
  resetSearch(): void {
    this.selectedNodes = [];
    this.formGroup.controls.searchText.setValue('');
    this.viewUserNodes = this.originUserNodes;
  }
  searchByTree(): void {
    const searchText = this.formGroup.controls.searchText.value;
    if (!searchText) {
      return;
    }
    this.viewUserNodes = this.treeNodeService.searchByTree(searchText, this.originUserNodes) as ExtendedTreeNode[];
  }
}
