import {
  Component,
  ElementRef,
  forwardRef,
  HostBinding,
  Input,
  OnDestroy,
  OnInit,
  Renderer2,
  ViewEncapsulation
} from '@angular/core';
import { FocusMonitor } from '../../../../../node_modules/@angular/cdk/a11y';
import { coerceBooleanProperty } from '../../../../../node_modules/@angular/cdk/coercion';
import {
  ControlValueAccessor,
  NG_VALUE_ACCESSOR
} from '../../../../../node_modules/@angular/forms';
import {
  MatButtonToggleChange,
  MatFormFieldControl,
  MatIconRegistry
} from '../../../../../node_modules/@angular/material';
import { DomSanitizer } from '../../../../../node_modules/@angular/platform-browser';
import { Subject } from '../../../../../node_modules/rxjs/Subject';
import { Subscription } from '../../../../../node_modules/rxjs/Subscription';

@Component({
  selector: 'app-search-mode-toggle',
  templateUrl: './search-mode-toggle.component.html',
  styleUrls: ['./search-mode-toggle.component.scss'],
  encapsulation: ViewEncapsulation.None,
  providers: [
    {
      // Интерфейс, переопределяющий поведение компонента как контрол Angular Material
      provide: MatFormFieldControl,
      useExisting: SearchModeToggleComponent
    },
    {
      // Дает возможность обращения к компоненту через formControlName
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SearchModeToggleComponent),
      multi: true
    }
  ]
})
export class SearchModeToggleComponent
  implements
    OnInit,
    OnDestroy,
    ControlValueAccessor,
    MatFormFieldControl<SearchMode> {
  static nextId = 0;
  _mode = defaultValue;

  private _placeholder: string;
  private _disabled = false;
  private _required = false;
  private subs: Subscription[] = [];
  private onDestroy = new Subject();

  controlType = 'app-search-mode-toggle';
  ngControl = null;
  focused = false;
  shouldPlaceholderFloat?: boolean;
  errorState = false;

  stateChanges = new Subject<void>();
  @Input()
  get placeholder() {
    return this._placeholder;
  }
  set placeholder(plh) {
    this._placeholder = plh;
    this.stateChanges.next();
  }
  @Input()
  get value(): SearchMode | null {
    return this._mode;
  }

  set value(value: SearchMode | null) {
    this.writeValue(value);
    this.stateChanges.next();
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

  @HostBinding()
  id = `${this.controlType}-${SearchModeToggleComponent.nextId++}`;

  @HostBinding('attr.aria-describedby') describedBy = '';

  @HostBinding('class.floating')
  get shouldLabelFloat() {
    return this.focused || !this.empty;
  }

  get empty() {
    return !this._mode;
  }

  constructor(
    private fm: FocusMonitor,
    private elRef: ElementRef,
    private renderer: Renderer2,
    private iconRegistry: MatIconRegistry,
    private sanitizer: DomSanitizer
  ) {
    iconRegistry.addSvgIcon(
      'equals',
      sanitizer.bypassSecurityTrustResourceUrl('./assets/equals.svg')
    );
    iconRegistry.addSvgIcon(
      'contains',
      sanitizer.bypassSecurityTrustResourceUrl('./assets/contains.svg')
    );
    this.subs.push(
      fm.monitor(elRef.nativeElement, true).subscribe(origin => {
        this.focused = !!origin;
        this.stateChanges.next();
      })
    );
  }

  private propagateChange = (_: any) => {};

  ngOnInit() {}

  ngOnDestroy(): void {
    this.stateChanges.complete();
    this.subs.forEach(s => s.unsubscribe());
    this.onDestroy.next();
  }

  onSearchModeChanged(e: MatButtonToggleChange) {
    if (e.source.checked) {
      this.writeValue(e.source.value);
    } else {
      this.writeValue(defaultValue.toString());
    }
  }

  writeValue(value: any): void {
    if (!!value) {
      this._mode = value;
    } else {
      this._mode = defaultValue;
    }
    this.propagateChange(value);
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

  setDescribedByIds(ids: string[]): void {
    this.describedBy = ids.join(' ');
  }

  onContainerClick(event: MouseEvent): void {
    if ((event.target as Element).tagName.toLowerCase() !== 'input') {
      this.elRef.nativeElement.querySelector('input').focus();
    }
  }
}

export enum SearchMode {
  Equals = 0,
  Contains = 1
}

const defaultValue = SearchMode.Contains;
const selectedValue = SearchMode.Equals;
