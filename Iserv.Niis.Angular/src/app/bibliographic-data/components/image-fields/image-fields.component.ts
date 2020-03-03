import {
  Component,
  OnChanges,
  OnInit,
  Input,
  ViewChild,
  ElementRef,
  Renderer2,
  forwardRef,
  OnDestroy,
  Output,
  EventEmitter,
  HostBinding
} from '@angular/core';
import { IntellectualPropertyDetails } from 'app/shared/models/intellectual-property-details';
import {
  FormGroup,
  FormBuilder,
  NG_VALUE_ACCESSOR,
  NG_VALIDATORS,
  ControlValueAccessor,
  Validator,
  AbstractControl,
  ValidationErrors
} from '@angular/forms';
import { MatCheckboxChange, MatFormFieldControl } from '@angular/material';
import { RouteStageCodes } from 'app/shared/models/route-stage-codes';
import { FocusMonitor } from '@angular/cdk/a11y';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import { Subject } from 'rxjs/Subject';
import { Subscription } from 'rxjs/Subscription';
import { BiblioField } from 'app/bibliographic-data/models/field-config';
import { isStageFormationAppData, isStageMakingChanges } from '../description/description.component';

@Component({
  selector: 'app-image-fields',
  templateUrl: './image-fields.component.html',
  styleUrls: ['./image-fields.component.scss'],
  // Провайдеры для контрола
  providers: [
    {
      // Интерфейс, переопределяющий поведение компонента как контрол Angular Material
      provide: MatFormFieldControl,
      useExisting: ImageFieldsComponent
    },
    {
      provide: BiblioField,
      useExisting: ImageFieldsComponent
    },
    {
      // Дает возможность обращения к компоненту через formControlName
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ImageFieldsComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => ImageFieldsComponent),
      multi: true
    }
  ]
})
export class ImageFieldsComponent extends BiblioField
  implements
    OnInit,
    OnChanges,
    OnDestroy,
    MatFormFieldControl<ImageFields>,
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
      const result = new ImageFields();
      result.imageFile = this.formGroup.get('imageFile').value;
      result.isImageFromName = this.formGroup.get('isImageFromName').value;
      return result;
    }
    return null;
  }
  set value(value: ImageFields) {
    this.writeValue(value);
    this.stateChanges.next();
  }
  // уникальный идентификатор контрола, селектор компонента + айдишник
  @HostBinding()
  id = `${this.controlType}-${ImageFieldsComponent.nextId++}`;
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
  controlType = 'app-image-fields-form';

  @Input()
  owner: IntellectualPropertyDetails;
  @Input()
  editMode: boolean;
  @Input()
  changeMode: boolean;

  @ViewChild('inputFile')
  nativeInputFile: ElementRef;

  get fileCount(): number {
    return this.nativeInputFile &&
      this.nativeInputFile.nativeElement.files.length
      ? 1
      : 0;
  }

  formGroup: FormGroup;
  defaultImageSource = '/assets/no-image.png';
  imageSource = null;

  private onDestroy = new Subject();
  private _required = false;
  private _placeholder: string;
  private _disabled = false;
  private subs: Subscription[] = [];

  constructor(
    private fm: FocusMonitor,
    private elRef: ElementRef,
    private renderer: Renderer2,
    private fb: FormBuilder
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

  ngOnInit() {}

  ngOnDestroy(): void {
    this.formGroup.reset();
    this.stateChanges.complete();
    this.subs.forEach(s => s.unsubscribe());
    this.onDestroy.next();
  }

  onFileSelect($event) {
    this.formGroup.markAsDirty();
    const reader = new FileReader();
    reader.onloadend = () => {
      this.imageSource = reader.result;
    };
    this.owner.imageFile = !this.nativeInputFile
      ? null
      : this.nativeInputFile.nativeElement.files[0];
    reader.readAsDataURL(this.owner.imageFile);
    this.formGroup.get('imageFile').setValue(this.owner.imageFile);
    this.changed.emit(this.owner);
  }

  onImageFromNameChecked(value: MatCheckboxChange) {
    if (value.checked) {
      this.formGroup.get('imageFile').reset();
    }
  }

  isChecked(): boolean {
    // return this.formGroup.get('isImageFromName').value;
    return false;
  }

  isRequired(): boolean {
    if (this.isChecked()) {
      return false;
    }

    return !this.imageSource;
  }

  isDisabled(): boolean {
    // if (this.changeMode) {
    //   return false;
    // }
    // return !this.editMode || (!isStageFormationAppData(this.owner) && !isStageMakingChanges(this.owner));
    return false;
  }

  isGenerateButtonHidden() {
    if (this.owner) {
      return RouteStageCodes.pdTypeIndustrialdesignsCodes.includes(
        this.owner.protectionDocTypeCode
      );
    }
    return false;
  }

  setDescribedByIds(ids: string[]): void {
    this.describedBy = ids.join(' ');
  }

  onContainerClick(event: MouseEvent): void {
    if ((event.target as Element).tagName.toLowerCase() !== 'input') {
      this.elRef.nativeElement.querySelector('input').focus();
    }
  }

  writeValue(obj: ImageFields): void {
    if (obj) {
      if (this.formGroup) {
        this.formGroup.get('imageFile').setValue(obj.imageFile);
        this.formGroup.get('isImageFromName').setValue(obj.isImageFromName);
        this.imageSource = obj.imageUrl || this.imageSource;
        this.changed.emit(obj);
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

  ngOnChanges(changes) {
    if (changes.disabled && !changes.disabled.currentValue) {
      this.formGroup.controls.isImageFromName.enable();
    }
    if (changes.disabled && changes.disabled.currentValue) {
      this.formGroup.controls.isImageFromName.disable();
    }
  }
  private buildForm() {
    this.formGroup = this.fb.group({
      isImageFromName: [{ value: '', disabled: !this.editMode }],
      imageFile: [{ value: '' }]
    });
  }

  getValue() {
    return this.value;
  }
}

export class ImageFields {
  isImageFromName: boolean;
  imageFile: File;
  imageUrl: string;
}
