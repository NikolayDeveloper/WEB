import { AfterViewInit, Component, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatExpansionPanel } from '@angular/material';
import 'rxjs/add/operator/takeUntil';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { SearchMode } from '../../../../shared/components/search-mode-toggle/search-mode-toggle.component';
import { CombineOperatorEnum } from '../../../../shared/filter/combine-operator.enum';
import { Operators } from '../../../../shared/filter/operators';
import { ExpertSearchFieldEnum } from '../../../models/expert-search-field.enum';
import { FieldConfig } from '../../../models/field-config.model';
import { CurrentFields } from '../../../models/current-fields.model';
import { OperatorFor } from '../../../models/invention-dto.model';
import { fromDateToJsonString } from 'app/payments-journal/helpers/date-helpers';
import { isMoment } from 'moment';

const searchStatusConfigs = [
  {
    formControlName: 'searchUsefulModels',
    equalsTo: true,
    checked: false
  }
];

const InventionFieldConfigs: Map<ExpertSearchFieldEnum, FieldConfig> = new Map([
  [ExpertSearchFieldEnum.None, {
    enum: ExpertSearchFieldEnum.None,
    name: 'none',
    label: 'Выберите значение'
  }],
  [ExpertSearchFieldEnum.Name, {
    enum: ExpertSearchFieldEnum.Name,
    name: 'name',
    label: 'Название'
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
  [ExpertSearchFieldEnum.Author, {
    enum: ExpertSearchFieldEnum.Author,
    name: 'author',
    label: 'Автор'
  }],
  [ExpertSearchFieldEnum.DeclarantName, {
    enum: ExpertSearchFieldEnum.DeclarantName,
    name: 'declarantName',
    label: 'Заявитель'
  }],
  [ExpertSearchFieldEnum.DeclarantCountryId, {
    enum: ExpertSearchFieldEnum.DeclarantCountryId,
    name: 'declarantCountryId',
    label: 'Страна заявителя'
  }],
  [ExpertSearchFieldEnum.DeclarantOblast, {
    enum: ExpertSearchFieldEnum.DeclarantOblast,
    name: 'declarantOblast',
    label: 'Область заявителя'
  }],
  [ExpertSearchFieldEnum.DeclarantCity, {
    enum: ExpertSearchFieldEnum.DeclarantCity,
    name: 'declarantCity',
    label: 'Город заявителя'
  }],
  [ExpertSearchFieldEnum.RequestStatusIds, {
    enum: ExpertSearchFieldEnum.RequestStatusIds,
    name: 'requestStatusIds',
    label: 'Статус заявки'
  }],
  [ExpertSearchFieldEnum.ProtectionDocStatusIds, {
    enum: ExpertSearchFieldEnum.ProtectionDocStatusIds,
    name: 'protectionDocStatusIds',
    label: 'Статус ОД'
  }],
  [ExpertSearchFieldEnum.RequestDate, {
    enum: ExpertSearchFieldEnum.RequestDate,
    name: 'requestDate',
    label: 'Дата подачи заявки'
  }],
  [ExpertSearchFieldEnum.GosNumber, {
    enum: ExpertSearchFieldEnum.GosNumber,
    name: 'gosNumber',
    label: 'Номер регистрации ОД'
  }],
  [ExpertSearchFieldEnum.GosDate, {
    enum: ExpertSearchFieldEnum.GosDate,
    name: 'gosDate',
    label: 'Дата регистрации ОД'
  }],
  [ExpertSearchFieldEnum.RegDate, {
    enum: ExpertSearchFieldEnum.RegDate,
    name: 'registerDate',
    label: 'Дата регистрации'
  }],
  [ExpertSearchFieldEnum.PublishDate, {
    enum: ExpertSearchFieldEnum.PublishDate,
    name: 'publishDate',
    label: 'Дата публикации'
  }],
  [ExpertSearchFieldEnum.OwnerName, {
    enum: ExpertSearchFieldEnum.OwnerName,
    name: 'ownerName',
    label: 'Патентообладатель'
  }]
]);

@Component({
  selector: 'app-invention-form',
  templateUrl: './invention-form.component.html',
  styleUrls: ['./invention-form.component.scss']
})
export class InventionFormComponent implements OnInit, OnDestroy, AfterViewInit {
  formGroup: FormGroup;
  CombineOperatorEnum = CombineOperatorEnum;
  ExpertSearchFieldEnum = ExpertSearchFieldEnum;
  inventionFieldConfigs = InventionFieldConfigs;
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
        enum: ExpertSearchFieldEnum.RequestStatusIds,
        canChanged: false,
        canDeleted: false,
        canSelectFirstOption: false
      },
      {
        enum: ExpertSearchFieldEnum.ProtectionDocStatusIds,
        canChanged: false,
        canDeleted: false,
        canSelectFirstOption: false
      }
    ];
    this.configs = Array.from(this.inventionFieldConfigs.values());
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

  ngAfterViewInit(): void {
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
      searchUsefulModels: this.formGroup.controls.searchUsefulModels.value
    };

    if (Object.keys(params).every(key => (params[key] === false))) {
      params = {
        searchUsefulModels: false
      };
    }

    return params;
  }

  onReset() {
    this.formGroup.reset();
    this.formGroup.controls['searchUsefulModels'].reset(true);
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
        formValues.items
            .filter(item => item.value || item.subValue)
            .forEach(item => {
                const combineOperatorEnum = item.operator as CombineOperatorEnum;
                const fieldSelector = item.fieldSelector as ExpertSearchFieldEnum;
                const fieldName = InventionFieldConfigs.get(fieldSelector).name;
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
                let value = this.prepare(item.value);

                if (operator === Operators.containsDateRange) {
                    value = [value, this.prepare(item.subValue)];
                }

                if (value) {
                    queryParams.push({
                        key: Operators[combineOperatorEnum] + fieldName + operator,
                        value: value
                    });
                }
            });
    }

    this.applySearchStatuses(formValues, queryParams);
    this.applySort(queryParams);

    return queryParams;
}

  private applySearchStatuses(formValues: any, queryParams: any[]) {
    const updatedConfigs = searchStatusConfigs.map(config => {
      config.checked = !!formValues[config.formControlName];
      return config;
    });

    if (updatedConfigs.some(config => config.checked)) {
      updatedConfigs.forEach(config => {
        if (config.checked) {
          queryParams.push({
            key:
              Operators[CombineOperatorEnum.AND] +
              config.formControlName +
              Operators.equal,
            value: config.equalsTo
          });
        }
      });
    }
  }

  private prepare(value: any): any {
    if (value instanceof Date || isMoment(value)) {
      value = fromDateToJsonString(value);
      return value;
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
      const config = this.inventionFieldConfigs.get(currentField.enum);
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
      searchUsefulModels: [true],
      items: this.fb.array(formGroups)
    });
  }
}
