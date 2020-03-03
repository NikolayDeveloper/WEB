import { Component, OnInit, AfterViewInit, Input, Output, EventEmitter, ViewChild, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { MatAutocompleteTrigger } from '@angular/material';
import { Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';

@Component({
    selector: 'app-advanced-select',
    templateUrl: './advanced-select.component.html',
    styleUrls: ['./advanced-select.component.scss'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: AdvancedSelectComponent,
            multi: true
        }
    ]
})
export class AdvancedSelectComponent implements ControlValueAccessor, OnInit, AfterViewInit {
    formGroup: FormGroup;
    filteredEntries: Observable<Object>;
    selectedEntries: number[] = [];
    selectedValues: any[] = [];
    isFocused = false;
    usedRequest = false;

    @Input() entries: any[];
    @Input() placeholder: string;
    @Input() id: string;
    @Input() done: EventEmitter<any>;
    @Output() search: EventEmitter<any> = new EventEmitter();
    @ViewChild(MatAutocompleteTrigger) trigger: MatAutocompleteTrigger;

    constructor(
        private formBuilder: FormBuilder
    ) {
        this.buildForm();
    }

    ngOnInit() {
        if (this.done) {
            this.done
                .subscribe(() => {
                    this.usedRequest = true;
                });
        }

        this.formGroup.get('input').valueChanges
            .debounceTime(1000)
            .subscribe(() => {
                if (this.isFocused) {
                    this.search.emit(this.formGroup.get('input').value.trim());
                }
            });

        this.formGroup.get('input').valueChanges
            .subscribe(() => {
                this.usedRequest = false;
            });

        this.filteredEntries = this.formGroup.get('input').valueChanges
            .pipe(
                startWith(''),
                map((entry: any) => typeof entry === 'string' ? entry : entry.value),
                map(filter => filter ? this.filter(filter) : this.entries ? this.entries.slice() : [])
            );
    }

    ngAfterViewInit() {
        this.trigger.panelClosingActions
            .subscribe(() => {
                this.isFocused = false;
                this.showSelected();
            });
    }

    onChange(value: any): void {}

    onTouched(value: any): void {}

    writeValue(value: any): void {
        if (!(value instanceof Array)) {
            this.selectedEntries = [];

            setTimeout(() => {
                this.showSelected();
            });
        }
        if (this.selectedEntries.length === 0 && value instanceof Array) {
            this.selectedEntries.push(...value);

            setTimeout(() => {
                const temp = sessionStorage.getItem(`${this.id}_selectedValues`);
                if (temp) {
                    const data = JSON.parse(temp);
                    if (data && data.length) {
                        this.selectedValues = data;
                    }
                }

                this.showSelected();
            });
        }
        this.onChange(value);
    }

    registerOnChange(callback: any): void {
        this.onChange = callback;
    }

    registerOnTouched(callback: any): void {
        this.onTouched = callback;
    }

    setDisabledState(isDisabled: boolean): void {
        if (isDisabled) {
            this.formGroup.get('input').disable();
        } else {
            this.formGroup.get('input').enable();
        }
    }

    /**
     * Фильтурет значения выпадающего списка.
     * @param filter Значение поиска
     */
    private filter(filter: string): Object[] {
        if (filter === '.') {
            return this.entries.filter((entry: any) => {
                return this.selectedEntries.includes(entry.id);
            });
        } else {
            return this.entries.filter((entry: any) => {
                if (!entry.value) {
                    return false;
                }

                return entry.value.toLocaleLowerCase().includes(filter.toLocaleLowerCase());
            });
        }
    }

    /**
     * Скрывает выбранные элементы.
     */
    hideSelected(): void {
        this.isFocused = true;
        this.formGroup.get('input').setValue('');
    }

    /**
     * Показывает выбранные элементы.
     */
    showSelected(): void {
        if (this.isFocused) {
            return;
        }
        const values = this.selectedValues
            .filter((entry) => this.selectedEntries.includes(entry.id))
            .map((entry) => entry.value);
        this.formGroup.get('input').setValue(values.join(', '));
        this.writeValue(this.selectedEntries);
    }

    /**
     * Проверяет выбран ли элемент.
     * @param entry Элемент
     */
    isChecked(entry: any) {
        return this.selectedEntries.includes(entry.id);
    }

    /**
     * Событие нажатия на опцию
     * @param event Событие
     * @param entry Элемент
     */
    optionClicked(event: Event, entry: any): void {
        event.stopPropagation();

        this.toggleSelection(entry);
    }

    /**
     * Переключает состояние опции
     * @param entry Элемент
     */
    toggleSelection(entry: any): void {
        if (this.selectedEntries.includes(entry.id)) {
            const index = this.selectedEntries.indexOf(entry.id);
            this.selectedEntries.splice(index, 1);
        } else {
            this.selectedEntries.push(entry.id);

            const dump = this.selectedValues.map((entry) => entry.id);
            if (!dump.includes(entry.id)) {
                this.selectedValues.push({
                    id: entry.id,
                    value: entry.value
                });
            }
        }

        sessionStorage.setItem(`${this.id}_selectedValues`, JSON.stringify(this.selectedValues));

        this.writeValue(this.selectedEntries);
    }

    /**
     * Собирает форму
     */
    private buildForm(): void {
        this.formGroup = this.formBuilder.group({
            input: new FormControl()
        });
    }
}
