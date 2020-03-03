import {
    AfterViewInit,
    ChangeDetectorRef,
    Component,
    EventEmitter,
    Input,
    OnDestroy,
    OnInit,
    Output,
    ViewChild
} from '@angular/core';
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
import { OperatorFor } from '../../../models/trademark-dto.model';
import { fromDateToJsonString } from 'app/payments-journal/helpers/date-helpers';
import { isMoment } from 'moment';
import { DictionaryType } from 'app/shared/services/models/dictionary-type.enum';
import { DictionaryService } from 'app/shared/services/dictionary.service';
import { TreeNode } from 'primeng/components/common/treenode';

const searchStatusConfigs = [
    {
        formControlName: 'isAppellationOfOrigin',
        equalsTo: true,
        checked: false
    },
    {
        formControlName: 'isTradeMark',
        equalsTo: true,
        checked: false
    },
    {
        formControlName: 'isIndustrialDesigns',
        equalsTo: true,
        checked: false
    }
];

const TrademarkFieldConfigs: Map<ExpertSearchFieldEnum, FieldConfig> = new Map([
    [ExpertSearchFieldEnum.None, {
        enum: ExpertSearchFieldEnum.None,
        name: 'none',
        label: 'Выберите значение'
    }],
    [ExpertSearchFieldEnum.Name, {
        enum: ExpertSearchFieldEnum.Name,
        name: 'name',
        label: 'Наименование'
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
    [ExpertSearchFieldEnum.IcgsIds, {
        enum: ExpertSearchFieldEnum.IcgsIds,
        name: 'icgsIds',
        label: 'МКТУ (номер класса)'
    }],
    [ExpertSearchFieldEnum.IcgsDescriptions, {
        enum: ExpertSearchFieldEnum.IcgsDescriptions,
        name: 'icgsDescriptions',
        label: 'МКТУ (перечень товаров и услуг)'
    }],
    [ExpertSearchFieldEnum.IcfemIds, {
        enum: ExpertSearchFieldEnum.IcfemIds,
        name: 'icfemIds',
        label: 'Венский классификатор (МКИЭТЗ)'
    }],
    [ExpertSearchFieldEnum.TrademarkTypeId, {
        enum: ExpertSearchFieldEnum.TrademarkTypeId,
        name: 'trademarkTypeId',
        label: 'Тип товарного знака'
    }],
    [ExpertSearchFieldEnum.TrademarkKindId, {
        enum: ExpertSearchFieldEnum.TrademarkKindId,
        name: 'trademarkKindId',
        label: 'Вид товарного знака'
    }],
    [ExpertSearchFieldEnum.RequestNumber, {
        enum: ExpertSearchFieldEnum.RequestNumber,
        name: 'requestNumber',
        label: 'Номер заявки'
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
    [ExpertSearchFieldEnum.PublishDate, {
        enum: ExpertSearchFieldEnum.PublishDate,
        name: 'publishDate',
        label: 'Дата публикации'
    }],
    [ExpertSearchFieldEnum.OwnerName, {
        enum: ExpertSearchFieldEnum.OwnerName,
        name: 'ownerName',
        label: 'Наименование владельца'
    }],
    [ExpertSearchFieldEnum.OwnerCity, {
        enum: ExpertSearchFieldEnum.OwnerCity,
        name: 'ownerCity',
        label: 'Город владельца'
    }],
    [ExpertSearchFieldEnum.OwnerCountryId, {
        enum: ExpertSearchFieldEnum.OwnerCountryId,
        name: 'ownerCountryId',
        label: 'Страна владельца'
    }],
    [ExpertSearchFieldEnum.OwnerOblast, {
        enum: ExpertSearchFieldEnum.OwnerOblast,
        name: 'ownerOblast',
        label: 'Область владельца'
    }],
    [ExpertSearchFieldEnum.DeclarantName, {
        enum: ExpertSearchFieldEnum.DeclarantName,
        name: 'declarantName',
        label: 'Наименование заявителя'
    }],
    [ExpertSearchFieldEnum.DeclarantCity, {
        enum: ExpertSearchFieldEnum.DeclarantCity,
        name: 'declarantCity',
        label: 'Город заявителя'
    }],
    [ExpertSearchFieldEnum.DeclarantOblast, {
        enum: ExpertSearchFieldEnum.DeclarantOblast,
        name: 'declarantOblast',
        label: 'Область заявителя'
    }],
    [ExpertSearchFieldEnum.DeclarantCountryId, {
        enum: ExpertSearchFieldEnum.DeclarantCountryId,
        name: 'declarantCountryId',
        label: 'Страна заявителя'
    }],
    [ExpertSearchFieldEnum.PatentAttorneyName, {
        enum: ExpertSearchFieldEnum.PatentAttorneyName,
        name: 'patentAttorneyName',
        label: 'Наименование патентного поверенного'
    }],
    [ExpertSearchFieldEnum.PatentAttorneyNumber, {
        enum: ExpertSearchFieldEnum.PatentAttorneyNumber,
        name: 'patentAttorneyNumber',
        label: 'Номер регистрации патентного поверенного'
    }]
]);

@Component({
    selector: 'app-trademark-form',
    templateUrl: './trademark-form.component.html',
    styleUrls: ['./trademark-form.component.scss']
})
export class TrademarkFormComponent
    implements OnInit, OnDestroy, AfterViewInit {
    formGroup: FormGroup;
    CombineOperatorEnum = CombineOperatorEnum;
    ExpertSearchFieldEnum = ExpertSearchFieldEnum;
    trademarkFieldConfigs = TrademarkFieldConfigs;
    configs: FieldConfig[];

    currentFields: CurrentFields[];

    dictionaries = {
      [ExpertSearchFieldEnum.IcfemIds]: {
        tree: null,
        type: DictionaryType.DicICFEM
      }
    };

    private onDestroy = new Subject();

    @Input() resultsLength: Observable<number>;
    @Output() search = new EventEmitter<any>();
    @Output() reset = new EventEmitter<any>();
    @ViewChild(MatExpansionPanel) accordionItem: MatExpansionPanel;

    constructor(
        private fb: FormBuilder,
        private dictionaryService: DictionaryService,
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
        this.configs = Array.from(this.trademarkFieldConfigs.values());
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
        const values = this.formGroup.controls.items.value;

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

            const values = this.formGroup.controls.items.value;
            this.buildForm(values);

            if (this.dictionaries.hasOwnProperty(value)) {
              if (this.dictionaries[value].tree === null) {
                this.dictionaryService.getBaseDictionary(this.dictionaries[value].type)
                .subscribe(data => {
                    this.dictionaries[value].tree = this.buildTree(data);
                  });
              }
            }
        }
    }

    getTree(type) {
      if (this.dictionaries.hasOwnProperty(type)) {
        return this.dictionaries[type].tree;
      } else {
        return [];
      }
    }

    getFilterParams() {
        let params = {
            isAppellationOfOrigin: this.formGroup.controls.isAppellationOfOrigin.value,
            isTradeMark: this.formGroup.controls.isTradeMark.value,
            isIndustrialDesigns: this.formGroup.controls.isIndustrialDesigns.value
        };

        if (Object.keys(params).every(key => (params[key] === false))) {
            params = {
                isAppellationOfOrigin: false,
                isTradeMark: true,
                isIndustrialDesigns: false
            };
        }

        return params;
    }

    onReset() {
        this.formGroup.reset();

        this.formGroup.controls['isAppellationOfOrigin'].reset(true);
        this.formGroup.controls['isTradeMark'].reset(true);
        this.formGroup.controls['isIndustrialDesigns'].reset(true);

        this.reset.emit();
    }

    onSubmit() {
        if (this.formGroup.invalid) {
            return;
        }
        this.formGroup.markAsPristine();

        const formValues = this.getFormValues();

        if (!formValues) {
            this.formGroup.reset();
            return;
        }

        const queryParams = this.buildQueryParamsFrom(formValues);
        this.search.emit(queryParams);
    }

    private sortTree(tree: any): void {
        const root = tree instanceof Array ? tree : tree.children;

        root.sort((a, b) => a.label.localeCompare(b.label));

        for (let child of root) {
            this.sortTree(child);
        }
    }

    private buildTree(data: any): TreeNode[] {
        const cache: Map<number, any[]> = new Map();
        const tree = [];

        data.sort(entry => {
            if (entry.parentId === null) {
                return -1;
            } else {
                return 0;
            }
        });

        for (let entry of data) {
            const root = entry.parentId === null ? tree : cache.get(entry.parentId);
            const node = {
                label: entry.nameRu ? `${entry.code} - ${entry.nameRu}` : entry.code,
                data: entry.id,
                children: []
            };

            root.push(node);
            cache.set(entry.id, node.children);
        }

        this.sortTree(tree);

        return tree;
    }

    private buildQueryParamsFrom(formValues: any): any[] {
        const queryParams = [];
        if (formValues) {
            formValues.items
                .filter(item => item.value || item.subValue)
                .forEach(item => {
                    const combineOperatorEnum = item.operator as CombineOperatorEnum;
                    const fieldSelector = item.fieldSelector as ExpertSearchFieldEnum;
                    const fieldName = TrademarkFieldConfigs.get(fieldSelector).name;
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

        const searchStatusKeys = {
            isAppellationOfOrigin: 'isNmpt',
            isTradeMark: 'isWellKnown',
            isIndustrialDesigns: 'isIndustrialSample'
        };

        if (updatedConfigs.some(config => config.checked)) {
            updatedConfigs.forEach(config => {
                if (config.checked) {
                    queryParams.push({
                        key:
                            Operators[CombineOperatorEnum.AND] +
                            searchStatusKeys[config.formControlName] +
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
        const hasValue = Object.keys(formValues).some(key =>
            formValues[key] ? true : false
        );

        return hasValue ? formValues : null;
    }

    private buildForm(values = []) {
        const formGroups = this.currentFields.map((currentField, index) => {
            const config = this.trademarkFieldConfigs.get(currentField.enum);
            const configGroup = {
                operator: [CombineOperatorEnum.AND.toString(), Validators.required],
                fieldSelector: [config.enum, Validators.required],
                value: values[index] ? [values[index].value] : [''],
                subValue: values[index] ? [values[index].subValue] : [''],
                placeholder: values[index] ? [values[index].placeholder] : [''],
                value_mode: values[index] ? [values[index].value_mode] : ['']
            };
            return this.fb.group(configGroup);
        });
        this.formGroup = this.fb.group({
            isAppellationOfOrigin : [true],
            isTradeMark: [true],
            isIndustrialDesigns: [true],
            items: this.fb.array(formGroups)
        });
    }
}
