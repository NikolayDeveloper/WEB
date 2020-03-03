import { AfterViewInit, ChangeDetectorRef, Component, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatExpansionPanel } from '@angular/material';
import { Observable, Subject } from 'rxjs/Rx';
import { Operators } from '../../../../shared/filter/operators';
import { DictionaryService } from '../../../../shared/services/dictionary.service';
import { DicRouteStage, DicProtectionDocType } from '../../../../shared/services/models/base-dictionary';
import { DictionaryType } from '../../../../shared/services/models/dictionary-type.enum';
import { SelectOption } from '../../../../shared/services/models/select-option';
import { moment, MY_FORMATS } from '../../../../shared/shared.module';
import { numberMask } from '../../.././../shared/services/validator/custom-validators';
import { OperatorFor } from '../../../models/protectiondoc-search-dto.model';
import { SearchType, SearchTypeSelectOptions } from '../../../models/search-type.enum';
import { selectedSearchTypeKey } from '../advanced-search.component';
import { SearchMode } from '../../../../shared/components/search-mode-toggle/search-mode-toggle.component';
import { startWith, map } from 'rxjs/operators';

@Component({
  selector: 'app-protectiondoc-search-form',
  templateUrl: './protectiondoc-search-form.component.html',
  styleUrls: ['./protectiondoc-search-form.component.scss']
})
export class ProtectionDocSearchFormComponent implements OnInit, OnDestroy, AfterViewInit {
  formGroup: FormGroup;
  dicProtectionDocStatuses: SelectOption[];
  dicProtectionDocTypes: DicProtectionDocType[];
  dicRouteStages: DicRouteStage[];
  availableRouteStages: DicRouteStage[];
  filteredRouteStages: Observable<SelectOption[]>;
  dicCountries: SelectOption[] = [];
  filteredDicCountries: Observable<SelectOption[]>;
  searchTypeSelectOptions = SearchTypeSelectOptions;
  searchType: SearchType;
  private onFetch = new Subject();
  private onDestroy = new Subject();

  numberMask = numberMask;

  protectionDocTypeCodes = [
    'B_PD',
    'U_PD',
    'S2_PD',
    'TM_PD',
    'PN_PD',
    'SA_PD',
  ];

  @Input() resultsLength: Observable<number>;
  @Output() search = new EventEmitter<any>();
  @Output() reset = new EventEmitter<any>();
  @Output() selectedSearchType = new EventEmitter<SearchType>();
  @ViewChild(MatExpansionPanel) accordionItem: MatExpansionPanel;

  constructor(private fb: FormBuilder,
    private dictionaryService: DictionaryService,
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
      this.dictionaryService.getSelectOptions(DictionaryType.DicProtectionDocStatus),
      this.dictionaryService.getSelectOptions(DictionaryType.DicProtectionDocType),
      this.dictionaryService.getBaseDictionary(DictionaryType.DicRouteStage),
      this.dictionaryService.getSelectOptions(DictionaryType.DicCountry),
    )
      .takeUntil(this.onDestroy)
      .subscribe(([protectionDocStatuses, protectionDocTypes, routeStages, countries]) => {
        this.dicProtectionDocStatuses = protectionDocStatuses;
        this.dicProtectionDocTypes = protectionDocTypes.filter((protectionDocType) => {
          return this.protectionDocTypeCodes.some((protectionDocTypeCode) => {
            return protectionDocTypeCode === protectionDocType.code;
          });
        }) as DicProtectionDocType[];
        this.dicRouteStages = routeStages as DicRouteStage[];
        this.dicCountries = countries.filter(c => c.nameRu);
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
    if (!formValues) {
      this.formGroup.reset();
      return;
    }

    const queryParams = this.buildQueryParamsFrom(formValues);
    this.search.emit(queryParams);
  }

  onReset() {
    this.formGroup.controls.workflowDateFrom.disable();
    this.formGroup.controls.workflowDateTo.disable();
    this.formGroup.controls.customerCountryId.setValue('');
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

  private getFormValues(): Object {
    const formValues = this.formGroup.getRawValue();
    const currentStage = this.formGroup.get('currentStageId').value;
    if (currentStage) {
      const typeId = this.formGroup.get('typeId').value;
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
    const hasValue = Object.keys(formValues).some(key => formValues[key] ? true : false);

    return hasValue ? formValues : null;
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      statusId: [''],
      typeId: [''],
      currentStageId: [{ value: '', disabled: true }],
      workflowDateFrom: [{ value: '', disabled: true }],
      workflowDateTo: [{ value: '', disabled: true }],
      publicDateFrom: [''],
      publicDateTo: [''],
      gosNumber: [''],
      gosDateFrom: [''],
      gosDateTo: [''],
      name: [''],
      validDateFrom: [''],
      validDateTo: [''],
      customerXin: ['', [Validators.maxLength(12)]],
      customerNameRu: [''],
      customerAddress: [''],
      customerCountryId: [''],
      gosNumber_mode: [''],
      name_mode: [''],
      customerXin_mode: [''],
      customerNameRu_mode: [''],
      customerAddress_mode: [''],
    });
  }

  private buildQueryParamsFrom(formValues: Object): any[] {
    const queryParams = [];
    const suffix = '_mode';
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

    return this.applySort(queryParams);
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

    if (key === 'gosDateFrom' || key === 'gosDateTo') {
      return 'gosDate';
    }

    if (key === 'publicDateFrom' || key === 'publicDateTo') {
      return 'publicDate';
    }

    if (key === 'workflowDateFrom' || key === 'workflowDateTo') {
      return 'workflowDate';
    }

    return key;
  }

  private prepare(key: string, value: any): any {
    if (moment.isMoment(value)) {
      value = new Date(value.toDate().toDateString());
      if (key === 'gosDateTo' || key === 'publicDateTo' || key === 'workflowDateTo') {
        value.setDate(value.getDate() + 1);
      }
      return value.toJSON();
    }

    return value;
  }
}
