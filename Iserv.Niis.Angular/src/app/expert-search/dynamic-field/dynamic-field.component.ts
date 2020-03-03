import { moment, MY_FORMATS } from '../../shared/shared.module';
import {
    AfterViewInit,
    Component,
    EventEmitter,
    Input,
    OnChanges,
    OnDestroy,
    Output,
    SimpleChanges
} from '@angular/core';
import { MatIconRegistry, MatDialog } from '@angular/material';
import { FormGroup } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { CombineOperatorEnum } from '../../shared/filter/combine-operator.enum';
import { ExpertSearchFieldEnum } from '../models/expert-search-field.enum';
import { FieldConfig } from '../models/field-config.model';
import { BaseDictionary } from 'app/shared/services/models/base-dictionary';
import { DictionaryService } from 'app/shared/services/dictionary.service';
import { DictionaryType } from 'app/shared/services/models/dictionary-type.enum';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { Operators } from 'app/shared/filter/operators';
import { TreeFormDialogComponent } from 'app/shared/components/tree-form-dialog/tree-form-dialog.component';
import { TreeNode } from 'primeng/components/common/treenode';

@Component({
    selector: 'app-dynamic-field',
    templateUrl: './dynamic-field.component.html',
    styleUrls: ['./dynamic-field.component.scss']
})
export class DynamicFieldComponent
    implements OnChanges, AfterViewInit, OnDestroy {
    CombineOperatorEnum = CombineOperatorEnum;
    ExpertSearchFieldEnum = ExpertSearchFieldEnum;
    selectedField: ExpertSearchFieldEnum;
    dictionary: BaseDictionary[] = [];
    previousDictionary: BaseDictionary[] = [];
    private onDestroy = new Subject();
    private speciesTradeMarkCodes = ['KTM', 'FTM', 'TTM'];
    get dateRangeFields() {
        return [
            ExpertSearchFieldEnum.RequestDate,
            ExpertSearchFieldEnum.GosDate,
            ExpertSearchFieldEnum.RegDate,
            ExpertSearchFieldEnum.PublishDate,
            ExpertSearchFieldEnum.PriorityDates
        ];
    }
    get selectListFields() {
        return [
            ExpertSearchFieldEnum.RequestStatusIds,
            ExpertSearchFieldEnum.ProtectionDocStatusIds,
            ExpertSearchFieldEnum.IPCCodes,
            ExpertSearchFieldEnum.IPCDescriptions,
            ExpertSearchFieldEnum.IcgsIds,
            ExpertSearchFieldEnum.IcgsDescriptions,
            ExpertSearchFieldEnum.Icis,
            ExpertSearchFieldEnum.OwnerCountryId,
            ExpertSearchFieldEnum.DeclarantCountryId,
            ExpertSearchFieldEnum.PriorityRegCountryNames,
            ExpertSearchFieldEnum.RequestTypeNameRu,
        ];
    }
    get selectFields() {
        return [
            ExpertSearchFieldEnum.TrademarkTypeId,
            ExpertSearchFieldEnum.TrademarkKindId
        ];
    }
    get dateFields() {
        return [];
    }
    get textFields() {
        return [
            ExpertSearchFieldEnum.Name,
            ExpertSearchFieldEnum.RequestNumber,
            ExpertSearchFieldEnum.GosNumber,
            ExpertSearchFieldEnum.Author,
            ExpertSearchFieldEnum.OwnerName,
            ExpertSearchFieldEnum.OwnerCity,
            ExpertSearchFieldEnum.OwnerOblast,
            ExpertSearchFieldEnum.DeclarantCity,
            ExpertSearchFieldEnum.DeclarantName,
            ExpertSearchFieldEnum.DeclarantOblast,
            ExpertSearchFieldEnum.PatentAttorneyName,
            ExpertSearchFieldEnum.PatentAttorneyNumber,
            ExpertSearchFieldEnum.Formula,
            ExpertSearchFieldEnum.Referat,
            ExpertSearchFieldEnum.Description,
            ExpertSearchFieldEnum.PriorityRegNumbers,
            ExpertSearchFieldEnum.PatentOwner,
            ExpertSearchFieldEnum.PatentAttorney
        ];
    }
    get dictionaryFields() {
        return [
            ExpertSearchFieldEnum.IcfemIds
        ];
    }

    dictionaries = {
        [ExpertSearchFieldEnum.IcfemIds]: TreeFormDialogComponent
    };

    @Input() reset: Observable<any>;
    @Input() hideOperator: boolean;
    @Input() formGroup: FormGroup;
    @Input() configs: FieldConfig[];
    @Input() defaultField: ExpertSearchFieldEnum;
    @Input() filterParams: any;
    @Input() canChanged = true;
    @Input() canDeleted = false;
    @Input() canSelectFirstOption = true;
    @Input() index: number;
    @Input() tree: TreeNode[] = null;
    @Output() change = new EventEmitter();
    @Output() delete = new EventEmitter();
    @Output() enter = new EventEmitter();
    @Output() done: EventEmitter<any> = new EventEmitter();

    constructor(
        private dialog: MatDialog,
        private dictionaryService: DictionaryService,
        private iconRegistry: MatIconRegistry,
        private sanitizer: DomSanitizer
    ) {
        iconRegistry.addSvgIcon(
            'times',
            sanitizer.bypassSecurityTrustResourceUrl('./assets/times.svg')
        );
    }

    ngOnChanges(changes: SimpleChanges): void {
        if (changes.defaultField) {
            this.selectedField = this.defaultField;
        }
    }

    onFieldDelete() {
        this.delete.emit(this.index);
    }

    openDictionary(dictionary) {
        if (this.dictionaries.hasOwnProperty(dictionary)) {
          const dialog = this.dialog.open(this.dictionaries[dictionary], {
              width: '80vw',
              height: '90vh',
              data: {
                  tree: this.tree,
                  ids: this.formGroup.get('value').value
              }
            });

            dialog.afterClosed()
                .subscribe((result: any) => {
                    if (result) {
                        const value = Array.from(new Set(result.map(entry => entry.id)));
                        const placeholder = Array.from(new Set(result.map(entry => entry.label)));

                        this.formGroup.get('value').setValue(value);
                        this.formGroup.get('placeholder').setValue(placeholder.join(', '));
                    }
                });
        }
    }

    onSearch(value: string): void {
        if (value && value.length >= 3) {
            const queryParams = [];
            const feedback = {
                [ExpertSearchFieldEnum.IPCCodes]: {
                    field: 'Code',
                    getValue: (entry) => {
                        return entry.code;
                    }
                },
                [ExpertSearchFieldEnum.IPCDescriptions]: {
                    field: 'NameRu',
                    getValue: (entry) => {
                        return entry.name ? entry.name.ru : entry.nameRu;
                    }
                }
            };

            if (feedback.hasOwnProperty(this.defaultField)) {
                const entry = feedback[this.defaultField];

                queryParams.push({
                    key: `${Operators[CombineOperatorEnum.AND]}${entry.field}${Operators.like}`,
                    value: value
                });

                this.dictionaryService
                    .getQueryBaseDictionary(DictionaryType.DicIPC, queryParams)
                    .takeUntil(this.onDestroy)
                    .subscribe((data) => {
                        this.dictionary = data.map((entry) => {
                            return {
                                id: entry.id,
                                value: feedback[this.defaultField].getValue(entry)
                            };
                        });

                        setTimeout(() => {
                            this.done.emit();
                        });
                    });
            }
        }
    }

    onFieldSelectorChange(value: any) {
        this.resetValueField();
        this.selectedField = value;
        this.change.emit({ index: this.index, value });
    }

    generateId(raw): string {
        return btoa(encodeURIComponent(raw));
    }

    ngAfterViewInit(): void {
        this.reset.takeUntil(this.onDestroy).subscribe(() => {
            this.resetValueField();
            this.formGroup.controls.operator.setValue(
                CombineOperatorEnum.AND.toString()
            );
            this.formGroup.controls.fieldSelector.patchValue(this.defaultField);
            this.selectedField = this.defaultField;
        });
        switch (this.defaultField) {
            case ExpertSearchFieldEnum.ProtectionDocStatusIds:
                this.dictionaryService
                    .getDicProtectionDocStatusForExpertSearch()
                    .takeUntil(this.onDestroy)
                    .subscribe(data => {
                        this.dictionary = data.map((entry) => {
                            return {
                                id: entry.id,
                                value: entry.name ? entry.name.ru : entry.nameRu
                            };
                        });
                    });
                break;
            case ExpertSearchFieldEnum.RequestStatusIds:
                this.dictionaryService
                    .getDicRequestStatusForExpertSearch()
                    .takeUntil(this.onDestroy)
                    .subscribe(data => {
                        this.dictionary = data.map((entry) => {
                            return {
                                id: entry.id,
                                value: entry.name ? entry.name.ru : entry.nameRu
                            };
                        });
                    });
                break;
            case ExpertSearchFieldEnum.IcgsIds:
                this.dictionaryService
                    .getBaseDictionary(DictionaryType.DicICGS)
                    .takeUntil(this.onDestroy)
                    .subscribe(data => {
                        this.dictionary = data.map((entry) => {
                            return {
                                id: entry.id,
                                value: entry.name ? entry.name.ru : entry.nameRu
                            };
                        });
                    });
                break;
            case ExpertSearchFieldEnum.TrademarkKindId:
                this.dictionaryService
                    .getBaseDictionary(DictionaryType.DicProtectionDocSubType)
                    .takeUntil(this.onDestroy)
                    .subscribe(data => {
                        this.dictionary = data
                            .filter((entry) => this.speciesTradeMarkCodes.includes(entry.code));
                    });
                break;
            case ExpertSearchFieldEnum.TrademarkTypeId:
                this.dictionaryService
                    .getBaseDictionary(DictionaryType.DicTypeTrademark)
                    .takeUntil(this.onDestroy)
                    .subscribe(data => {
                        this.dictionary = data;
                    });
                break;
            case ExpertSearchFieldEnum.DeclarantCountryId:
            case ExpertSearchFieldEnum.OwnerCountryId:
            case ExpertSearchFieldEnum.PriorityRegCountryNames:
                this.dictionaryService
                    .getBaseDictionary(DictionaryType.DicCountry)
                    .takeUntil(this.onDestroy)
                    .subscribe(data => {
                        this.dictionary = data.map((entry) => {
                            return {
                                id: entry.id,
                                value: entry.name ? entry.name.ru : entry.nameRu
                            };
                        });
                    });
                break;
            case ExpertSearchFieldEnum.IcgsDescriptions:
                this.dictionaryService
                    .getBaseDictionary(DictionaryType.DicICGS)
                    .takeUntil(this.onDestroy)
                    .subscribe((data) => {
                        this.dictionary = data.map((entry) => {
                            return {
                                id: entry.id,
                                value: entry.descriptionShort
                            };
                        });
                    });
                break;
            case ExpertSearchFieldEnum.RequestTypeNameRu:
                this.dictionaryService
                    .getBaseDictionary(DictionaryType.DicConventionType)
                    .takeUntil(this.onDestroy)
                    .subscribe((data) => {
                        this.dictionary = data.map((entry) => {
                            return {
                                id: entry.id,
                                value: entry.name ? entry.name.ru : entry.nameRu
                            };
                        });
                    });
                break;
            case ExpertSearchFieldEnum.Icis:
                this.dictionaryService
                    .getBaseDictionary(DictionaryType.DicICIS)
                    .takeUntil(this.onDestroy)
                    .subscribe((data) => {
                        this.dictionary = data
                            .filter((entry) => {
                                return entry.description;
                            })
                            .sort((a, b) => {
                                return a.code.localeCompare(b.code);
                            })
                            .map((entry) => {
                                return {
                                    id: entry.id,
                                    value: entry.description
                                };
                            });
                    });
                break;
        }
    }

    ngOnDestroy(): void {
        this.onDestroy.next();
    }

    toDate(value: any) {
        if (!value) {
            return;
        }

        return moment(value, MY_FORMATS.parse.dateInput).toDate();
    }

    isFieldActive(
        fieldEnum: ExpertSearchFieldEnum,
        fieldEnums: ExpertSearchFieldEnum[]
    ): boolean {
        return fieldEnums.includes(fieldEnum);
    }

    private resetValueField(): void {
        this.formGroup.controls.value.reset('');
        this.formGroup.controls.subValue.reset('');

        if (this.formGroup.controls.placeholder) {
          this.formGroup.controls.placeholder.reset('');
        }
    }
}
