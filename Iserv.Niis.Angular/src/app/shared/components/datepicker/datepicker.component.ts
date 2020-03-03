import {
  Component,
  OnInit,
  Input,
  ElementRef,
  OnDestroy,
  forwardRef,
  HostBinding,
  Renderer2,
  HostListener,
  ViewEncapsulation,
  Output,
  EventEmitter
} from '@angular/core';
import { MatFormFieldControl } from '@angular/material';
import { Observable, Subject, Subscription } from 'rxjs';
import {
  NgControl,
  ControlValueAccessor,
  NG_VALUE_ACCESSOR,
  FormBuilder,
  FormGroup,
  NG_VALIDATORS
} from '@angular/forms';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import { FocusMonitor } from '@angular/cdk/a11y';

@Component({
  selector: 'app-datepicker',
  templateUrl: './datepicker.component.html',
  styleUrls: ['./datepicker.component.scss'],
  encapsulation: ViewEncapsulation.None,
  providers: [
    {
      provide: MatFormFieldControl,
      useExisting: DatepickerComponent
    },
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DatepickerComponent),
      multi: true
    }
  ]
})
export class DatepickerComponent
  implements
    OnInit,
    OnDestroy,
    MatFormFieldControl<Date>,
    ControlValueAccessor {
  static nextId = 0;
  stateChanges = new Subject<void>();
  ngControl = null;
  focused = false;
  shouldPlaceholderFloat?: boolean;
  errorState = false;
  controlType = 'app-datepicker';
  formGroup: FormGroup;

  private _value: Date;
  private _placeholder: string;
  private _disabled = false;
  private _required = false;
  private subs: Subscription[] = [];
  private onDestroy = new Subject();

  @Input()
  get placeholder() {
    return this._placeholder;
  }
  set placeholder(plh) {
    this._placeholder = plh;
    this.stateChanges.next();
  }

  @HostBinding() id = `${this.controlType}-${DatepickerComponent.nextId++}`;
  @HostBinding('attr.aria-describedby') describedBy = '';
  @HostBinding('class.floating')
  get shouldLabelFloat() {
    return this.focused || !this.empty;
  }
  @HostListener('(blur)') _onTouched: Function;

  @Input()
  get value(): Date | null {
    return this.formGroup.controls.inputVal.value;
  }

  set value(value: Date | null) {
    this.writeValue(value);
    this.stateChanges.next();
  }

  get empty() {
    return !this.formGroup.controls.inputVal.value;
  }

  @Input()
  get required() {
    return this._required;
  }
  set required(value) {
    this._required = coerceBooleanProperty(value);
    this.stateChanges.next();
  }

  @Input()
  get disabled() {
    return this._disabled;
  }
  set disabled(value) {
    this._disabled = coerceBooleanProperty(value);
    this.stateChanges.next();
  }

  @Output() changed = new EventEmitter<Date>();

  constructor(
    private fb: FormBuilder,
    private fm: FocusMonitor,
    private elRef: ElementRef,
    private renderer: Renderer2
  ) {
    this.buildForm();
    this.subs.push(
      fm.monitor(elRef.nativeElement, true).subscribe(origin => {
        this.focused = !!origin;
        this.stateChanges.next();
      })
    );
    this.subs.push(
      this.formGroup.controls.inputVal.valueChanges.subscribe((value: Date) => {
        this.propagateChange(value);
      })
    );
  }

  ngOnInit() {
    this.formGroup
      .get('inputVal')
      .valueChanges.takeUntil(this.onDestroy)
      .subscribe(value => this.changed.emit(value));
  }

  private propagateChange = (_: any) => {};

  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
    isDisabled
      ? this.formGroup.controls.inputVal.disable()
      : this.formGroup.controls.inputVal.enable();
  }

  setDescribedByIds(ids: string[]): void {
    this.describedBy = ids.join(' ');
  }

  onContainerClick(event: MouseEvent): void {
    if ((event.target as Element).tagName.toLowerCase() !== 'input') {
      this.elRef.nativeElement.querySelector('input').focus();
    }
  }

  writeValue(val: Date): void {
    if (val) {
      this.formGroup.controls.inputVal.setValue(val);
      this.changed.emit(val);
    } else {
      this.formGroup.reset();
    }
  }

  registerOnChange(fn: (_: any) => void): void {
    this.propagateChange = fn;
  }
  registerOnTouched(fn: (_: any) => void): void {
    this._onTouched = fn;
  }
  ngOnDestroy(): void {
    this.formGroup.reset();
    this.stateChanges.complete();
    this.subs.forEach(s => s.unsubscribe());
    this.onDestroy.next();
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      inputVal: ['']
    });
  }
}
