import {
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
  ViewChild,
  ChangeDetectorRef,
  AfterViewInit
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatExpansionPanel } from '@angular/material';
import { Observable, Subject } from 'rxjs/Rx';

import { UsersService } from '../../../../administration/users.service';
import { Operators } from '../../../../shared/filter/operators';
import { DictionaryService } from '../../../../shared/services/dictionary.service';
import {
  DicDepartment,
  DicProtectionDocSubType,
  DicRouteStage,
  ExtendedTreeNode,
  toUserTreeNodes,
  DicProtectionDocType
} from '../../../../shared/services/models/base-dictionary';
import { DictionaryType } from '../../../../shared/services/models/dictionary-type.enum';
import { SelectOption } from '../../../../shared/services/models/select-option';
import { TreeNodeService } from '../../../../shared/services/tree-node.service';
import { moment, MY_FORMATS } from '../../../../shared/shared.module';
import { OperatorFor } from '../../../models/contract-search-dto.model';
import {
  SearchType,
  SearchTypeSelectOptions
} from '../../../models/search-type.enum';
import { selectedSearchTypeKey } from '../advanced-search.component';
import { numberMask } from '../../.././../shared/services/validator/custom-validators';
import { SearchMode } from '../../../../shared/components/search-mode-toggle/search-mode-toggle.component';
import { startWith, map } from 'rxjs/operators';

// TODO: Перейти на публичный код
const contractRouteId = 162;
const contractPDTypeId = 72;

@Component({
  selector: 'app-contract-search-form',
  templateUrl: './contract-search-form.component.html',
  styleUrls: ['./contract-search-form.component.scss']
})
export class ContractSearchFormComponent
  implements OnInit, OnDestroy, AfterViewInit {
  formGroup: FormGroup;
  dicContractStatuses: SelectOption[];
  dicContractTypes: SelectOption[];
  dicProtectionDocTypes: DicProtectionDocType[];
  dicContractCategories: SelectOption[];
  dicRouteStages: DicRouteStage[];
  availableRouteStages: DicRouteStage[];
  filteredRouteStages: Observable<SelectOption[]>;
  dicCountries: SelectOption[] = [];
  filteredDicCountries: Observable<SelectOption[]>;
  searchTypeSelectOptions = SearchTypeSelectOptions;
  searchType: SearchType;
  originUserNodes: ExtendedTreeNode[];
  viewUserNodes: ExtendedTreeNode[];
  selectedNodes: ExtendedTreeNode[] = [];
  numberMask = numberMask;

  private onFetch = new Subject();
  private onDestroy = new Subject();

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
      this.dictionaryService.getSelectOptions(DictionaryType.DicContractStatus),
      this.dictionaryService.getBaseDictionary(
        DictionaryType.DicContractType
      ),
      this.dictionaryService.getSelectOptions(
        DictionaryType.DicProtectionDocType
      ),
      this.dictionaryService.getBaseDictionary(DictionaryType.DicRouteStage),
      this.dictionaryService.getSelectOptions(DictionaryType.DicCountry),
      this.dictionaryService.getSelectOptions(
        DictionaryType.DicContractCategory
      ),
      this.dictionaryService.getSelectOptions(DictionaryType.DicDivision),
      this.dictionaryService.getBaseDictionary(DictionaryType.DicDepartment),
      this.usersService.get()
    )
      .takeUntil(this.onDestroy)
      .subscribe(
        ([
          contractStatuses,
          contractTypes,
          pdTypes,
          routeStages,
          countries,
          contractCategories,
          divisions,
          departments,
          users
        ]) => {
          this.dicContractStatuses = contractStatuses;
          this.dicContractTypes = contractTypes;
          this.dicProtectionDocTypes = pdTypes.filter(t => t.code === 'DK');
          this.dicRouteStages = routeStages as DicRouteStage[];
          this.dicCountries = countries.filter(c => c.nameRu);
          this.dicContractCategories = contractCategories;
          this.originUserNodes = toUserTreeNodes(
            divisions,
            departments as DicDepartment[],
            users as any
          );
          this.viewUserNodes = this.originUserNodes;
          this.onFetch.next();
        }
      );

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
    if (this.formGroup.invalid) {
      return;
    }
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
    this.formGroup.controls.workflowDateFrom.disable();
    this.formGroup.controls.workflowDateTo.disable();
    this.formGroup.controls.customerCountryId.setValue('');
    this.formGroup.controls.currentStageId.setValue('');
    this.viewUserNodes = this.originUserNodes;
    this.formGroup.reset();
    this.reset.emit();
  }

  onProtectionDocTypeChange(typeId: number) {
    const routeId = this.dicProtectionDocTypes.find((protectionDocType) => {
      return protectionDocType.id === typeId;
    }).routeId;
    this.availableRouteStages = this.dicRouteStages.filter(s => s.routeId === routeId);
    this.formGroup.get('currentStageId').reset();
    this.formGroup.get('currentStageId').enable();
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

  private buildForm() {
    this.formGroup = this.fb.group({
      statusId: [''],
      contractTypeId: [''],
      categoryId: [''],
      currentStageId: [{ value: '', disabled: true }],
      workflowDateFrom: [{ value: '', disabled: true }],
      workflowDateTo: [{ value: '', disabled: true }],
      applicationNum: [''],
      dateCreateFrom: [''],
      dateCreateTo: [''],
      contractNum: [''],
      regDateFrom: [''],
      regDateTo: [''],
      protectionDocTypeId: [''],
      name: [''],
      customerXin: ['', [Validators.maxLength(12)]],
      customerNameRu: [''],
      customerAddress: [''],
      customerCountryId: [''],
      registrationPlace: [''],
      validDateFrom: [''],
      validDateTo: [''],
      searchText: [''],
      applicationNum_mode: [''],
      contractNum_mode: [''],
      name_mode: [''],
      customerXin_mode: [''],
      customerNameRu_mode: [''],
      customerAddress_mode: [''],
      registrationPlace_mode: ['']
    });
  }

  private getFormValues(): Object {
    const formValues = this.formGroup.getRawValue();
    delete formValues.searchText;
    const currentStage = this.formGroup.get('currentStageId').value;
    if (currentStage) {
      const typeId = this.formGroup.get('protectionDocTypeId').value;
      const routeId = this.dicProtectionDocTypes.find((protectionDocType) => {
        return protectionDocType.id === typeId;
      }).routeId;
      const stage = this.dicRouteStages.find(routeStage => (routeStage.nameRu === currentStage && routeStage.routeId === routeId));
      if (stage) {
        formValues.currentStageId = stage.id;
      }
    }
    const customerCountry = this.formGroup.get('customerCountryId').value;
    if (customerCountry) {
      formValues.customerCountryId = customerCountry.id;
    }
    const hasValue = Object.keys(formValues).some(
      key => (formValues[key] ? true : false)
    );

    return hasValue ? formValues : null;
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
      queryParams.push({
        key: userIdKey + OperatorFor[userIdKey],
        value: userIds.join()
      });
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
    if (key === 'validDateFrom' || key === 'validDateTo') {
      return 'validDate';
    }

    if (key === 'regDateFrom' || key === 'regDateTo') {
      return 'regDate';
    }

    if (key === 'dateCreateFrom' || key === 'dateCreateTo') {
      return 'dateCreate';
    }

    if (key === 'workflowDateFrom' || key === 'workflowDateTo') {
      return 'workflowDate';
    }

    return key;
  }

  private prepare(key: string, value: any): any {
    if (moment.isMoment(value)) {
      value = new Date(value.toDate().toDateString());
      if (
        key === 'regDateTo' ||
        key === 'dateCreateTo' ||
        key === 'workflowDateTo'
      ) {
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
    this.viewUserNodes = this.treeNodeService.searchByTree(
      searchText,
      this.originUserNodes
    ) as ExtendedTreeNode[];
  }
}
