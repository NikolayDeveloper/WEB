import { FocusMonitor } from '@angular/cdk/a11y';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import {
  Component,
  ElementRef,
  EventEmitter,
  forwardRef,
  HostBinding,
  Input,
  OnDestroy,
  OnInit,
  Output,
  Renderer2,
  ViewChild,
  OnChanges,
  SimpleChanges
} from '@angular/core';
import {
  AbstractControl,
  ControlValueAccessor,
  FormBuilder,
  FormGroup,
  NG_VALIDATORS,
  NG_VALUE_ACCESSOR,
  ValidationErrors,
  Validator
} from '@angular/forms';
import { MatFormFieldControl } from '@angular/material';
import { BiblioField } from 'app/bibliographic-data/models/field-config';
import { IntellectualPropertyDetails } from 'app/shared/models/intellectual-property-details';
import { Subject } from 'rxjs/Subject';
import { Subscription } from 'rxjs/Subscription';
import { DictionaryService } from 'app/shared/services/dictionary.service';
import { BaseDictionary } from 'app/shared/services/models/base-dictionary';
import { DictionaryType } from 'app/shared/services/models/dictionary-type.enum';
import { isStageFormationAppData } from '../description/description.component';

@Component({
  selector: 'app-media-file-fields',
  templateUrl: './media-file-fields.component.html',
  styleUrls: ['./media-file-fields.component.scss'],
  // Провайдеры для контрола
  providers: [
    {
      // Интерфейс, переопределяющий поведение компонента как контрол Angular Material
      provide: MatFormFieldControl,
      useExisting: MediaFileFieldsComponent
    },
    {
      provide: BiblioField,
      useExisting: MediaFileFieldsComponent
    },
    {
      // Дает возможность обращения к компоненту через formControlName
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => MediaFileFieldsComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => MediaFileFieldsComponent),
      multi: true
    }
  ]
})
export class MediaFileFieldsComponent extends BiblioField
  implements
    OnInit,
    OnChanges,
    OnDestroy,
    MatFormFieldControl<MediaFileFields>,
    ControlValueAccessor,
    Validator {
  static nextId = 0;
  // Событие, поднимающееся при изменении значения
  @Output()
  changed = new EventEmitter<any>();
  @Input()
  isInvalidInput = false;
  // Свойство-label
  @Input()
  get placeholder() {
    return this._placeholder;
  }
  set placeholder(plh) {
    this._placeholder = plh;
    this.stateChanges.next();
  }
  // Является ли обязательным
  @Input()
  get required() {
    return this._required;
  }
  set required(value) {
    this._required = coerceBooleanProperty(value);
    this.stateChanges.next();
  }
  // Является ли отключенным
  @Input()
  get disabled() {
    return this._disabled;
  }
  set disabled(value) {
    this._disabled = coerceBooleanProperty(value);
    this.editMode = !value;
    this.stateChanges.next();
  }
  // Занчение контрола
  @Input()
  get value() {
    if (this.formGroup) {
      const result = new MediaFileFields();
      return result;
    }
    return null;
  }
  set value(value: MediaFileFields) {
    this.writeValue(value);
    this.stateChanges.next();
  }
  // уникальный идентификатор контрола, селектор компонента + айдишник
  @HostBinding()
  id = `${this.controlType}-${MediaFileFieldsComponent.nextId++}`;
  // Идентификаторы вложенных контролов
  @HostBinding('attr.aria-describedby')
  describedBy = '';
  // Определяет, как контрол отображается, т.е. выходит ли он на передний план при фокусе
  @HostBinding('class.floating')
  get shouldLabelFloat() {
    return this.focused || !this.empty;
  }
  // Пусто ли значение контрола
  get empty() {
    const n = this.formGroup.getRawValue();
    return !n.genus || !n.breedCountryId || !n.breedingNumber;
  }
  // Здесь ведем учет изменениям состояния контрола
  stateChanges = new Subject<void>();
  ngControl = null;
  focused = false;
  shouldPlaceholderFloat?: boolean;
  errorState = false;
  // Тип контрола, должен совпадать с селектором компонента
  controlType = 'app-media-file-fields-form';

  @Input()
  owner: IntellectualPropertyDetails;
  @Input()
  editMode: boolean;
  @Input()
  changeMode: boolean;
  @Input()
  trademarkTypeId: number;

  @ViewChild('inputFile')
  nativeInputFile: ElementRef;

  get fileCount(): number {
    return this.nativeInputFile &&
      this.nativeInputFile.nativeElement.files.length
      ? 1
      : 0;
  }

  formGroup: FormGroup;
  fileSource: any;

  private onDestroy = new Subject();
  private _required = false;
  private _placeholder: string;
  private _disabled = false;
  private subs: Subscription[] = [];

  private trademarkType: BaseDictionary;

  constructor(
    private fm: FocusMonitor,
    private elRef: ElementRef,
    private renderer: Renderer2,
    private fb: FormBuilder,
    private dictionaryService: DictionaryService
  ) {
    super();
    this.buildForm();
    // подписываемся на событие фокуса
    this.subs.push(
      fm.monitor(elRef.nativeElement, true).subscribe(origin => {
        this.focused = !!origin;
        this.stateChanges.next();
      })
    );
    // подписываемся на событие изменения значения
    this.subs.push(
      this.formGroup.valueChanges.subscribe((value: number) => {
        this.propagateChange(value);
      })
    );
  }

  ngOnInit() {
    if (this.owner) {
      this.dictionaryService
        .getBaseDictionaryById(
          DictionaryType.DicTypeTrademark,
          this.owner.typeTrademarkId
        )
        .takeUntil(this.onDestroy)
        .subscribe(data => (this.trademarkType = data));
    }
  }

  ngOnDestroy(): void {
    this.formGroup.reset();
    this.stateChanges.complete();
    this.subs.forEach(s => s.unsubscribe());
    this.onDestroy.next();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.trademarkTypeId && changes.trademarkTypeId.currentValue) {
      this.dictionaryService
        .getBaseDictionaryById(
          DictionaryType.DicTypeTrademark,
          this.trademarkTypeId
        )
        .takeUntil(this.onDestroy)
        .subscribe(data => (this.trademarkType = data));
    }
  }

  setDescribedByIds(ids: string[]): void {
    this.describedBy = ids.join(' ');
  }

  onContainerClick(event: MouseEvent): void {
    if ((event.target as Element).tagName.toLowerCase() !== 'input') {
      this.elRef.nativeElement.querySelector('input').focus();
    }
  }

  writeValue(obj: MediaFileFields): void {
    if (obj) {
      if (this.formGroup) {
        this.changed.emit(obj);
        this.formGroup.get('mediaFile').setValue(obj.mediaFile);
        this.fileSource = obj.mediaUrl;
        this.propagateChange(obj);
      }
    } else {
      if (this.formGroup) {
        this.formGroup.reset();
        this.changed.emit(obj);
        this.propagateChange(obj);
      }
    }
  }

  registerOnChange(fn: any): void {
    this.propagateChange = fn;
  }

  registerOnTouched(fn: any): void {
    return;
  }

  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  validate(c: AbstractControl): ValidationErrors {
    const invalid = {
      invalid: {
        valid: false
      }
    };
    if (!this.formGroup) {
      return invalid;
    }
    return this.formGroup.valid ? null : invalid;
  }

  registerOnValidatorChange?(fn: () => void): void {
    return;
  }

  private propagateChange = (_: any) => {};

  private buildForm() {
    this.formGroup = this.fb.group({
      mediaFile: [{ value: '' }]
    });
  }

  getValue() {
    return this.value;
  }

  onFileSelect($event) {
    this.formGroup.markAsDirty();
    const reader = new FileReader();
    reader.onloadend = () => {
      this.fileSource = reader.result;
    };
    this.owner.mediaFile = !this.nativeInputFile
      ? null
      : this.nativeInputFile.nativeElement.files[0];
    reader.readAsDataURL(this.owner.mediaFile);
    this.formGroup.get('mediaFile').setValue(this.owner.mediaFile);
    this.changed.emit(this.owner);
  }

  isVideo(): boolean {
    return (
      !!this.trademarkType && ['02', '11'].includes(this.trademarkType.code)
    );
  }

  isAudio(): boolean {
    return !!this.trademarkType && this.trademarkType.code === '03';
  }

  isDisabledOnStageFormationAppData(): boolean {
    // if (this.changeMode) {
    //   return false;
    // }
    // return !this.editMode || !isStageFormationAppData(this.owner);
    return false;
  }

  getAccept(): string {
    if (this.isAudio()) {
      return 'audio/*';
    }
    if (this.isVideo()) {
      return 'video/*';
    }
    return '';
  }
}

export class MediaFileFields {
  mediaFile: File;
  mediaUrl: string;
}
