import { OperatorFor } from '../../../models/usefulmodel-dto.model';
import 'rxjs/add/operator/takeUntil';

import { AfterViewInit, Component, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatExpansionPanel } from '@angular/material';
import { Observable, Subject, Subscription } from 'rxjs/Rx';

import { CombineOperatorEnum } from '../../../../shared/filter/combine-operator.enum';
import { Operators } from '../../../../shared/filter/operators';
import { ExpertSearchFieldEnum } from '../../../models/expert-search-field.enum';
import { FieldConfig } from '../../../models/field-config.model';
import { ProtectionDocSearchStatus } from '../../../models/protectiondoc-search-status.enum';
import { SearchMode } from '../../../../shared/components/search-mode-toggle/search-mode-toggle.component';
import { CurrentFields } from 'app/expert-search/models/current-fields.model';

const searchStatusConfigs = [
  {
    formControlName: 'active',
    status: ProtectionDocSearchStatus.Active,
    checked: false
  },
  {
    formControlName: 'inProgress',
    status: ProtectionDocSearchStatus.InProgress,
    checked: false
  },
  {
    formControlName: 'expired',
    status: ProtectionDocSearchStatus.Expired,
    checked: false
  }
];

const UsefulmodelFieldConfigs: Map<ExpertSearchFieldEnum, FieldConfig>= new Map([
  [ExpertSearchFieldEnum.None, {
    enum: ExpertSearchFieldEnum.None,
    name: 'none',
    label: 'Выберите значение'
  }],
  [ExpertSearchFieldEnum.RequestDate, {
    enum: ExpertSearchFieldEnum.RequestDate,
    name: 'priorityRegDate',
    label: 'До даты приоритета'
  }],
  [ExpertSearchFieldEnum.Name, {
    enum: ExpertSearchFieldEnum.Name,
    name: 'name',
    label: 'Название'
  }],
  [ExpertSearchFieldEnum.RequestTypeNameRu, {
    enum: ExpertSearchFieldEnum.RequestTypeNameRu,
    name: 'requestTypeNameRu',
    label: 'Тип заявки'
  }],
  [ExpertSearchFieldEnum.IPCCodes, {
    enum: ExpertSearchFieldEnum.IPCCodes,
    name: 'ipcCodes',
    label: 'Индекс МПК'
  }],
  [ExpertSearchFieldEnum.IPCDescriptions, {
    enum: ExpertSearchFieldEnum.IPCDescriptions,
    name: 'ipcDescriptions',
    label: 'Текст МПК'
  }],
  [ExpertSearchFieldEnum.Formula, {
    enum: ExpertSearchFieldEnum.Formula,
    name: 'formula',
    label: 'Формула'
  }],
  [ExpertSearchFieldEnum.Referat, {
    enum: ExpertSearchFieldEnum.Referat,
    name: 'referat',
    label: 'Реферат'
  }],
  [ExpertSearchFieldEnum.Description, {
    enum: ExpertSearchFieldEnum.Description,
    name: 'description',
    label: 'Описание'
  }],
  [ExpertSearchFieldEnum.PriorityRegNumbers, {
    enum: ExpertSearchFieldEnum.PriorityRegNumbers,
    name: 'priorityRegNumbers',
    label: '№ приоритетной заявки'
  }],
  [ExpertSearchFieldEnum.PriorityDates, {
    enum: ExpertSearchFieldEnum.PriorityDates,
    name: 'priorityRegDate',
    label: 'Дата приоритета с'
  }],
  [ExpertSearchFieldEnum.PriorityRegCountryNames, {
    enum: ExpertSearchFieldEnum.PriorityRegCountryNames,
    name: 'priorityRegCountryNames',
    label: 'Страна приоритета'
  }],
  [ExpertSearchFieldEnum.DeclarantName, {
    enum: ExpertSearchFieldEnum.DeclarantName,
    name: 'declarantName',
    label: 'Заявитель'
  }],
  [ExpertSearchFieldEnum.PatentOwner, {
    enum: ExpertSearchFieldEnum.PatentOwner,
    name: 'patentOwner',
    label: 'Патентообладатель'
  }],
  [ExpertSearchFieldEnum.Author, {
    enum: ExpertSearchFieldEnum.Author,
    name: 'author',
    label: 'Автор'
  }],
  [ExpertSearchFieldEnum.PatentAttorney, {
    enum: ExpertSearchFieldEnum.PatentAttorney,
    name: 'patentAttorney',
    label: 'Патентный поверенный'
  }],
  [ExpertSearchFieldEnum.GosNumber, {
    enum: ExpertSearchFieldEnum.GosNumber,
    name: 'gosNumber',
    label: '№ патента'
  }],
  [ExpertSearchFieldEnum.PublishDate, {
    enum: ExpertSearchFieldEnum.PublishDate,
    name: 'publishDate',
    label: 'Дата публикации с'
  }]
]);

@Component({
  selector: 'app-usefulmodel-form',
  templateUrl: './usefulmodel-form.component.html',
  styleUrls: ['./usefulmodel-form.component.scss']
})
export class UsefulmodelFormComponent implements OnInit, OnDestroy, AfterViewInit {
  formGroup: FormGroup;
  CombineOperatorEnum = CombineOperatorEnum;
  ExpertSearchFieldEnum = ExpertSearchFieldEnum;
  usefulmodelFieldConfigs = UsefulmodelFieldConfigs;
  configs: FieldConfig[];

  currentFields: CurrentFields[];

  private onDestroy = new Subject();

  @Input() resultsLength: Observable<number>;
  @Output() search = new EventEmitter<any>();
  @Output() reset = new EventEmitter<any>();
  @ViewChild(MatExpansionPanel) accordionItem: MatExpansionPanel;

  constructor(
    private fb: FormBuilder,
    private changeDetector: ChangeDetectorRef
  ) {
    this.currentFields = [
      {
        enum: ExpertSearchFieldEnum.Name,
        canChanged: false,
        canDeleted: false,
        canSelectFirstOption: false
      },
      {
        enum: ExpertSearchFieldEnum.RequestDate,
        canChanged: false,
        canDeleted: false,
        canSelectFirstOption: false
      },
      {
        enum: ExpertSearchFieldEnum.RequestTypeNameRu,
        canChanged: false,
        canDeleted: false,
        canSelectFirstOption: false
      }
    ];
    this.configs = Array.from(this.usefulmodelFieldConfigs.values());
    this.buildForm();
  }

  ngOnInit() {
    this.resultsLength.takeUntil(this.onDestroy).subscribe(length => {
      length > 1 ? this.accordionItem.close() : this.accordionItem.open();
    });
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  ngAfterViewInit() {
    this.changeDetector.detectChanges();
  }

  onAddAdditional() {
    const values = this.formGroup.controls.items.value.map((control) => {
      return control.value;
    });

    this.currentFields.push({
      enum: ExpertSearchFieldEnum.None,
      canChanged: true,
      canDeleted: true,
      canSelectFirstOption: true
    });
    this.buildForm(values);
  }

  onFieldDelete(index: number) {
    this.currentFields.splice(index, 1);
    this.buildForm();
  }

  onFieldTypeChanged({ index, value }) {
    if (value) {
      this.currentFields[index].enum = value;

      const values = this.formGroup.controls.items.value.map((control) => {
        return control.value;
      });
      this.buildForm(values);
    }
  }

  getFilterParams() {
    let params = {
      active: this.formGroup.controls.active.value,
      inProgress: this.formGroup.controls.inProgress.value,
      expired: this.formGroup.controls.expired.value,
    };

    if (Object.keys(params).every(key => (params[key] === false))) {
      params = {
        active: false,
        inProgress: false,
        expired: false
      };
    }

    return params;
  }

  onReset() {
    this.formGroup.reset();
    this.formGroup.controls['active'].reset(true);
    this.formGroup.controls['inProgress'].reset(true);
    this.formGroup.controls['expired'].reset(true);
    this.reset.emit();
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

  private buildQueryParamsFrom(formValues: any): any[] {
    const queryParams = [];
    if (formValues) {
      formValues.items.filter(item => item.value || item.subValue).forEach(item => {
        const combineOperatorEnum = item.operator as CombineOperatorEnum;
        const fieldSelector = item.fieldSelector as ExpertSearchFieldEnum;
        const fieldName = UsefulmodelFieldConfigs.get(fieldSelector).name;
        let operator;
        if (item.value_mode) {
          switch (item.value_mode) {
            case SearchMode.Equals.toString():
              operator = Operators.equal;
              break;
            case SearchMode.Contains.toString():
              operator = OperatorFor[fieldName];
              break;
          }
        } else {
          operator = OperatorFor[fieldName];
        }
        let value = this.prepare(operator, item.value);

        if (operator === Operators.containsDateRange) {
          let subValue = item.subValue;
          if (subValue && subValue instanceof Date) {
            subValue = new Date(subValue.toDateString());
            subValue.setDate(subValue.getDate() + 1);
          }

          value = [value, this.prepare(operator, subValue)]
        }

        if (value) {
          queryParams.push({
            key: Operators[combineOperatorEnum] + fieldName + operator,
            value: value
          });
        }
      })
    }

    this.applySearchStatuses(formValues, queryParams);
    this.applySort(queryParams);

    return queryParams;
  }

  private applySearchStatuses(formValues: any, queryParams: any[]) {
    const statusKey = 'searchStatus';
    const updatedConfigs = searchStatusConfigs.map(config => {
      config.checked = !!formValues[config.formControlName];
      return config;
    });

    if (updatedConfigs.some(config => config.checked)) {
      queryParams.push({
        key: Operators.commonAnd + statusKey + OperatorFor[statusKey],
        value: updatedConfigs.filter(config => config.checked).map(config => config.status).join(),
      });
    } else {
      queryParams.push({
        key: Operators.commonAnd + statusKey + OperatorFor[statusKey],
        value: [ProtectionDocSearchStatus.None],
      });
    }
  }

  private prepare(operator: string, value: any): any {
    if (operator === Operators.lessThan && value instanceof Date) {
      value.setDate(value.getDate() + 1);
    }

    if (value instanceof Date) {
      value = new Date(value.toDateString());
      return value.toJSON();
    }

    return encodeURIComponent(value);
  }

  private applySort(queryParams: any[]) {
    queryParams.push(
      { key: Operators.sort, value: 'barcode' },
      { key: Operators.order, value: 'desc' }
    );
    return queryParams;
  }

  private getFormValues(): Object {
    const formValues = this.formGroup.getRawValue();
    const hasValue = Object.keys(formValues).some(key => formValues[key] ? true : false);

    return hasValue ? formValues : null;
  }

  private buildForm(values = []) {
    const formGroups = this.currentFields.map((currentField, index) => {
      const config = this.usefulmodelFieldConfigs.get(currentField.enum);
      const configGroup = {
        operator: [CombineOperatorEnum.AND.toString(), Validators.required],
        fieldSelector: [config.enum, Validators.required],
        value: values[index] ? [values[index]] : [''],
        subValue: [''],
        value_mode: ['']
      };
      return this.fb.group(configGroup);
    });
    this.formGroup = this.fb.group({
      active: [true],
      inProgress: [true],
      expired: [true],
      items: this.fb.array(formGroups)
    });
  }
}
