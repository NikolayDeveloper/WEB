import { FocusMonitor } from '@angular/cdk/a11y';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import {
  Component,
  ElementRef,
  forwardRef,
  HostBinding,
  Input,
  OnDestroy,
  Renderer2,
  ViewEncapsulation,
  HostListener,
  Output,
} from '@angular/core';
import { ControlValueAccessor, FormBuilder, FormGroup, NG_VALUE_ACCESSOR } from '@angular/forms';
import { MatFormFieldControl } from '@angular/material';
import { Subscription } from 'rxjs';
import { Subject } from 'rxjs/Subject';
import { EventEmitter } from 'events';



// Todo!!! если инпут находится в ngIf на разметке, выходит бага и страница не реагирует на действия
@Component({
  selector: 'app-common-input-string',
  templateUrl: './common-input-string.component.html',
  styleUrls: ['./common-input-string.component.scss'],
  encapsulation: ViewEncapsulation.None,
  providers: [
    {
      provide: MatFormFieldControl,
      useExisting: CommonInputStringComponent,
    },
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CommonInputStringComponent),
      multi: true,
    },
  ],
})
export class CommonInputStringComponent implements
  OnDestroy,
  MatFormFieldControl<string>,
  ControlValueAccessor {
  static nextId = 0;

  stateChanges = new Subject<void>();
  focused = false;
  shouldPlaceholderFloat?: boolean;
  errorState = false;
  controlType = 'app-common-input-string';
  ngControl = null;

  formGroup: FormGroup;
  editableControls: string[];

  @HostListener('(blur)')
  _onTouched: Function;

  private onDestroy = new Subject();
  private _required = false;
  private _placeholder: string;
  private _disabled = false;
  private subs: Subscription[] = [];

  @Input() mask = null;
  @Input() errorText = '';
  @Input() isInvalidInput = false;

  @Input()
  get placeholder() {
    return this._placeholder;
  }
  set placeholder(plh) {
    this._placeholder = plh;
    this.stateChanges.next();
  }

  @HostBinding()
  id = `${this.controlType}-${CommonInputStringComponent.nextId++}`;
  @HostBinding('attr.aria-describedby') describedBy = '';
  @HostBinding('class.floating')
  get shouldLabelFloat() {
    return this.focused || !this.empty;
  }

  @Input()
  get value(): string | null {
    return this.formGroup.controls.inputVal.value;
  }

  set value(value: string | null) {
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
      this.formGroup.controls.inputVal.valueChanges.subscribe((value: string) => {
        this.propagateChange(value);
      })
    );
  }

  private propagateChange = (_: any) => { };

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

  writeValue(val: string): void {
    if (val) {
      this.formGroup.controls.inputVal.setValue(val);
    } else {
      this.formGroup.reset();
    }
  }

  registerOnChange(fn: (_: any) => void): void {
    this.propagateChange = fn;
    this.formGroup.get('inputVal').valueChanges.subscribe(fn);
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
      inputVal: [''],
    });
  }
}
