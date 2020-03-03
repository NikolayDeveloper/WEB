import { coerceBooleanProperty } from '@angular/cdk/coercion';
import {
  Component,
  ElementRef,
  forwardRef,
  HostBinding,
  Input,
  OnDestroy,
  OnInit,
  Output,
  EventEmitter
} from '@angular/core';
import {
  ControlValueAccessor,
  FormBuilder,
  FormControl,
  FormGroup,
  NG_VALIDATORS,
  NG_VALUE_ACCESSOR,
  Validator,
  Validators
} from '@angular/forms';

import { Router, ActivatedRoute } from '@angular/router';
import { MatDialog, MatFormFieldControl } from '@angular/material';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { Subscription } from 'rxjs/Subscription';
import { ContractService } from '../../../contracts/contract.service';
import { ContractDetails } from '../../../contracts/models/contract-details';
import { ProtectionDocDetails } from '../../../protection-docs/models/protection-doc-details';
import { ProtectionDocsService } from '../../../protection-docs/protection-docs.service';
import { RequestDetails } from '../../../requests/models/request-details';
import { RequestService } from '../../../requests/request.service';
import { IntellectualPropertySearchDto } from '../../../search/models/intellectual-property-search-dto';
import { OwnerType } from '../../../shared/services/models/owner-type.enum';
import { DocumentType, MaterialOwnerDto, getDocumentTypeRoute } from '../../models/materials.model';
import { SearchDialogComponent} from '../search-dialog/search-dialog.component';
import { FocusMonitor } from '@angular/cdk/a11y';

@Component({
  selector: 'app-attach-to-owner-form',
  templateUrl: './attach-to-owner-form.component.html',
  styleUrls: ['./attach-to-owner-form.component.scss'],
  providers: [
    {
      provide: MatFormFieldControl,
      useExisting: AttachToOwnerFormComponent
    },
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => AttachToOwnerFormComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => AttachToOwnerFormComponent),
      multi: true
    }
  ]
})
export class AttachToOwnerFormComponent
  implements
    OnInit,
    OnDestroy,
    MatFormFieldControl<MaterialOwnerDto[]>,
    ControlValueAccessor,
    Validator {
  static nextId = 0;

  private _placeholder: string;
  private _disabled = false;
  private _required = false;
  private _isSimple = false;
  private subs: Subscription[] = [];
  owners: MaterialOwnerDto[] = [];
  private onDestroy = new Subject();
  private hasOwnerId = false;

  controlType = 'app-attach-to-owner-form';
  formGroup: FormGroup;
  ngControl = null;
  focused = false;
  shouldPlaceholderFloat?: boolean;
  errorState = false;
  requestNumbers = [];
  protectioNumbers = [];
  contractNumbers = [];

  @Input() code: string;
  @Input() ignoreOwnerTypes: OwnerType[];
  @Input() ownerType: any;
  @Input() protectionDocTypeId: any;
  @Output() selectProtectionDocTypeId = new EventEmitter<number>();
  @Output() attach: EventEmitter<any> = new EventEmitter();

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
  get value(): MaterialOwnerDto[] | null {
    return this.owners;
  }

  set value(value: MaterialOwnerDto[] | null) {
    this.writeValue(value);
    this.stateChanges.next();
  }

  @Input()
  get isSimple() {
    return this._isSimple;
  }
  set isSimple(value) {
    this._isSimple = coerceBooleanProperty(value);
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
  id = `${this.controlType}-${AttachToOwnerFormComponent.nextId++}`;

  @HostBinding('attr.aria-describedby') describedBy = '';

  @HostBinding('class.floating')
  get shouldLabelFloat() {
    return this.focused || !this.empty;
  }

  get empty() {
    return !this.owners || this.owners.length === 0;
  }

  constructor(
    private dialog: MatDialog,
    private fb: FormBuilder,
    private fm: FocusMonitor,
    private elRef: ElementRef,
    private requestService: RequestService,
    private contractService: ContractService,
    private protectionDocService: ProtectionDocsService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.buildForm();
    this.subs.push(
      fm.monitor(elRef.nativeElement, true).subscribe(origin => {
        this.focused = !!origin;
        this.stateChanges.next();
      })
    );
    this.subs.push(
      this.formGroup.valueChanges.subscribe((value: MaterialOwnerDto[]) => {
        this.propagateChange(value);
      })
    );


  }

  private propagateChange = (_: any) => {};

  private buildForm() {
    this.formGroup = this.fb.group({
      numbers: [{ value: '', disabled: true }],
      requestNumbers: [{ value: [], disabled: true }],
      pdNumbers: [{ value: '', disabled: true }],
      contractNumbers: [{ value: '', disabled: true }],
      owners: ['']
    });
  }

  ngOnDestroy(): void {
    this.formGroup.reset();
    this.stateChanges.complete();
    this.subs.forEach(s => s.unsubscribe());
    this.onDestroy.next();
  }

  setDescribedByIds(ids: string[]): void {
    this.describedBy = ids.join(' ');
  }

  onContainerClick(event: MouseEvent): void {
    if ((event.target as Element).tagName.toLowerCase() !== 'input') {
      this.elRef.nativeElement.querySelector('input').focus();
    }
  }

  writeValue(value: MaterialOwnerDto[]): void {
    if (!!value) {
      this.owners = value;
      this.formGroup.get('owners').setValue(this.owners);
    } else {
      this.owners = [];
      this.formGroup.reset();
    }
    this.setNumbersValue();
  }

  registerOnChange(fn: (_: any) => void): void {
    this.propagateChange = fn;
  }

  registerOnTouched(fn: (_: any) => void): void {
    return;
  }

  // setDisabledState?(isDisabled: boolean): void {
  //   console.log('setDisabledState', isDisabled);
  //   this.disabled = isDisabled;
  // }

  registerOnValidatorChange?(fn: () => void): void {
    return;
  }

  validate(c: FormControl): { [key: string]: any } {
    if (this.hasOwnerId) {
      const invalid = {
        invalid: {
          valid: false
        }
      };
      if (!this.owners) {
        return invalid;
      }
      return this.owners.length > 0 ? null : invalid;
    } else {
      return null;
    }
  }

  ngOnInit() {
    if (this.ownerType === OwnerType.Request) {
      this.placeholder = 'Прикрепить к заявке';
    } else if (this.ownerType === OwnerType.ProtectionDoc) {
      this.placeholder = 'Прикрепить к ОД';
    } else {
      this.placeholder = 'Прикрепить к ...';
    }

    this.route.paramMap.subscribe((params) => {
      this.hasOwnerId = params.has('ownerId');

      if (this.hasOwnerId) {
        this.formGroup.get('numbers').setValidators(Validators.required);
      }
    });
  }

  onSearchClick() {
    const dialogRef = this.dialog.open(SearchDialogComponent, {
      data: {
        code: this.code,
        isSimple: this.isSimple,
        selectedIntellectualProperties: !!this.owners
          ? this.owners.map(o => {
              const property = new IntellectualPropertySearchDto();
              property.id = o.ownerId;
              property.type = o.ownerType;
              property.protectionDocTypeId = o.protectionDocTypeId;
              return property;
            })
          : '',
          ownerType: this.ownerType,
          protectionDocTypeId: this.protectionDocTypeId
      },
      width: '1000px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.owners = result.owners;
        this.writeValue(result.owners);
        this.selectProtectionDocTypeId.emit(result.protectionDoсTypeId);

        const entries = this.owners.map((entry) => {
          return entry.addressee;
        });
        this.attach.emit(entries);
      }
    });
  }

  hasOwners(): boolean {
    return !!this.owners && this.owners.length > 0;
  }

  areOwnersRequest(): boolean {
    return (
      this.hasOwners() &&
      this.owners.some(o => o.ownerType === OwnerType.Request)
    );
  }

  areOwnersProtectionDoc(): boolean {
    return (
      this.hasOwners() &&
      this.owners.some(o => o.ownerType === OwnerType.ProtectionDoc)
    );
  }

  areOwnersContract(): boolean {
    return (
      this.hasOwners() &&
      this.owners.some(o => o.ownerType === OwnerType.Contract)
    );
  }

  onClearClick() {
    this.owners = [];
    this.writeValue([]);

    this.attach.emit(null);
  }

  remove(id: number) {
    const newOwners = [...this.owners.filter(d => d.ownerId !== id)];
    this.owners = newOwners;
    this.writeValue(newOwners);
  }

  requestOnClick(id: number) {
    this.router.navigate([
      getDocumentTypeRoute(DocumentType.Request),
      id
    ]);
  }

  protectioOnClick(id: number) {
    this.router.navigate([
      getDocumentTypeRoute(DocumentType.ProtectionDoc),
      id
    ]);
  }

  contractOnClick(id: number) {
    this.router.navigate([
      getDocumentTypeRoute(DocumentType.Contract),
      id
    ]);
  }

  setNumbersValue() {
    if (this.areOwnersRequest()) {
      const requests$ = this.owners
        .filter(o => o.ownerType === OwnerType.Request)
        .map(owner => this.requestService.getRequestById(owner.ownerId));
      Observable.combineLatest(requests$)
        .takeUntil(this.onDestroy)
        .subscribe((requests: RequestDetails[]) => {
          const requestnumbers = requests
            .map(r => ({ id: r.id, value: `${r.nameRu} ${r.requestNum || '(без номера)'} ${r.barcode}` }));
          // numbers = requests.map(r => r.requestNum ? r.requestNum : '(без номера) ' + r.barcode).join('; ');
          this.requestNumbers = requestnumbers;
          this.formGroup.get('requestNumbers').setValue(requestnumbers);
          this.formGroup.get('numbers').setValue(true);
        });
    }
    if (this.areOwnersProtectionDoc()) {
      const protectionDocs$ = this.owners
        .filter(o => o.ownerType === OwnerType.ProtectionDoc)
        .map(owner => this.protectionDocService.get(owner.ownerId));
      Observable.combineLatest(protectionDocs$)
        .takeUntil(this.onDestroy)
        .subscribe((protectionDocs: ProtectionDocDetails[]) => {
          const protectionumbers = protectionDocs.map(r => ({ id: r.id, value: `${r.nameRu} ${r.gosNumber || '(без номера)'} ${r.barcode}` }));
          // numbers = protectionDocs.map(r => r.gosNumber ? r.gosNumber : '(без номера) ' + r.barcode).join('; ');
          this.protectioNumbers = protectionumbers;
          this.formGroup.get('pdNumbers').setValue(protectionumbers);
          this.formGroup.get('numbers').setValue(true);
        });
    }
    if (this.areOwnersContract()) {
      const contracts$ = this.owners
        .filter(o => o.ownerType === OwnerType.Contract)
        .map(owner => this.contractService.getById(owner.ownerId));
      Observable.combineLatest(contracts$)
        .takeUntil(this.onDestroy)
        .subscribe((contractDocs: ContractDetails[]) => {
          const contrNumbers = contractDocs.map(r => ({ id: r.id, value: `${r.nameRu} ${r.contractNum || '(без номера)'} ${r.barcode}` }));
          this.contractNumbers = contrNumbers;
          // numbers = protectionDocs.map(r => r.contractNum ? r.contractNum : '(без номера) ' + r.barcode).join('; ');
          this.formGroup.get('contractNumbers').setValue(contrNumbers);
          this.formGroup.get('numbers').setValue(true);
        });
    } else {
      this.formGroup.get('numbers').setValue('');
    }
  }
}
