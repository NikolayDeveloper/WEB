import { FocusMonitor } from '@angular/cdk/a11y';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import {
    Component,
    ElementRef,
    forwardRef,
    HostBinding,
    Input,
    OnDestroy,
    OnInit,
    Renderer2,
    ChangeDetectorRef,
    AfterViewInit
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
import { MatDialog, MatFormFieldControl } from '@angular/material';
import { BiblioField } from 'app/bibliographic-data/models/field-config';
import { ConfigService } from 'app/core';
import { SnackBarHelper } from 'app/core/snack-bar-helper.service';
import { DocumentType } from 'app/materials/models/materials.model';
import { AddressResponse, PostKzService } from 'app/modules/postkz';
import { CustomerService } from 'app/shared/services/customer.service';
import { CustomerShortInfo } from 'app/shared/services/models/customer-short-info';
import { OwnerType } from 'app/shared/services/models/owner-type.enum';
import { xinValidator } from 'app/shared/services/validator/custom-validators';
import { SubjectFormDialogComponent } from 'app/subjects/components/subject-form-dialog/subject-form-dialog.component';
import { SubjectsSearchDialogComponent } from 'app/subjects/components/subjects-search-dialog/subjects-search-dialog.component';
import {
    concatAddresseeAddress,
    getPart,
    SubjectConstants,
    SubjectDto,
    ContactInfoDto
} from 'app/subjects/models/subject.model';
import { SubjectsService } from 'app/subjects/services/subjects.service';
import { Subject, Subscription } from 'rxjs';
import { SubjectFormMode } from 'app/subjects/enums/subject-form-mode.enum';
import { CustomerRole } from 'app/subjects/enums/customer-role.enum';
import { SubjectCreateDialogComponent } from 'app/subjects/components/subject-create-dialog/subject-create-dialog.component';

@Component({
    selector: 'app-subjects-search-form',
    templateUrl: './subjects-search-form.component.html',
    styleUrls: ['./subjects-search-form.component.scss'],
    providers: [
        {
            provide: MatFormFieldControl,
            useExisting: SubjectsSearchFormComponent
        },
        {
            provide: BiblioField,
            useExisting: SubjectsSearchFormComponent
        },
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => SubjectsSearchFormComponent),
            multi: true
        },
        {
            provide: NG_VALIDATORS,
            useExisting: forwardRef(() => SubjectsSearchFormComponent),
            multi: true
        }
    ]
})
export class SubjectsSearchFormComponent extends BiblioField
    implements
        OnInit,
        OnDestroy,
        MatFormFieldControl<AddresseeInfo>,
        ControlValueAccessor,
        Validator,
        AfterViewInit {
    static nextId = 0;

    stateChanges = new Subject<void>();
    focused = false;
    shouldPlaceholderFloat?: boolean;
    errorState = false;
    controlType = 'app-subjects-search-form';
    ngControl = null;

    formGroup: FormGroup;
    editableControls: string[];

    filteredPostAddresses: any[];
    selectedAddresseeAddress: string;

    editMode = false;

    selectedSubject: SubjectDto;

    private onDestroy = new Subject();
    private _required = false;
    private _placeholder: string;
    private _disabled = false;
    private subs: Subscription[] = [];

    @Input()
    get placeholder() {
        return this._placeholder;
    }
    set placeholder(plh) {
        this._placeholder = plh;
        this.stateChanges.next();
    }

    @Input() protectionDocTypeCode: string;
    @Input() ownerType: OwnerType;
    @Input() documentType: DocumentType;

    @HostBinding()
    id = `app-subjects-search-form-${SubjectsSearchFormComponent.nextId++}`;
    @HostBinding('attr.aria-describedby') describedBy = '';
    @HostBinding('class.floating')
    get shouldLabelFloat() {
        return this.focused || !this.empty;
    }
    @Input()
    get value(): AddresseeInfo | null {
        const result = new AddresseeInfo();
        Object.assign(result, this.formGroup.getRawValue());
        return result;
    }

    set value(value: AddresseeInfo | null) {
        this.writeValue(value);
        this.stateChanges.next();
    }

    get empty() {
        const n = this.formGroup.value;
        return (
            !n.addresseeId &&
            !n.addresseeAddress &&
            !n.apartment &&
            !n.addresseeNameRu &&
            !n.addresseeXin
        );
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
        this.toggleEditMode(!value);
        this.stateChanges.next();
    }

    constructor(
        private postKzService: PostKzService,
        private configService: ConfigService,
        private customerService: CustomerService,
        private subjectsService: SubjectsService,
        private snackbarHelper: SnackBarHelper,
        private dialog: MatDialog,
        private fb: FormBuilder,
        private fm: FocusMonitor,
        private elRef: ElementRef,
        private renderer: Renderer2,
        private detectorRef: ChangeDetectorRef
    ) {
        super();
        this.buildForm();
        this.subs.push(
            fm.monitor(elRef.nativeElement, true).subscribe(origin => {
                this.focused = !!origin;
                this.stateChanges.next();
            })
        );
        this.subs.push(
            this.formGroup.valueChanges.subscribe((value: AddresseeInfo) => {
                this.propagateChange(value);
            })
        );
    }
    private propagateChange = (_: any) => {};

    getAddresseeAddress(address: any) {
        const apartment = this.formGroup.get('apartment').value;
        return concatAddresseeAddress(address, apartment);
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

    validate(c: FormControl): { [key: string]: any } {
        const invalid = {
            invalid: {
                valid: false
            }
        };
        if (!this.formGroup) {
            return invalid;
        }
        const value = this.formGroup.getRawValue();
        return !!value.addresseeId ? null : invalid;
    }

    registerOnValidatorChange?(fn: () => void): void {
        return;
    }

    writeValue(info: AddresseeInfo): void {
        if (info) {
            this.formGroup.setValue({
                addresseeAddress: info.addresseeAddress ? info.addresseeAddress : '',
                addresseeShortAddress: info.addresseeShortAddress ? info.addresseeShortAddress : '',
                apartment: info.apartment ? info.apartment : '',
                addresseeId: info.addresseeId ? info.addresseeId : '',
                addresseeNameRu: info.addresseeNameRu ? info.addresseeNameRu : '',
                addresseeXin: info.addresseeXin ? info.addresseeXin : '',
                addresseeEmail: info.email ? info.email : '',
                republic: info.republic ? info.republic : '',
                oblast: info.oblast ? info.oblast : '',
                region: info.region ? info.region : '',
                city: info.city ? info.city : '',
                street: info.street ? info.street : '',
                contactInfos: info.contactInfos ? info.contactInfos : ''
            });
            this.selectedAddresseeAddress = info.addresseeAddress;
        }
    }

    registerOnChange(fn: (_: any) => void): void {
        this.propagateChange = fn;
    }
    registerOnTouched(fn: (_: any) => void): void {
        return;
    }

    ngOnInit() {
        this.subscribeXinInput();
        this.subscribeAddressInput();
    }

    ngOnDestroy(): void {
        this.formGroup.reset();
        this.stateChanges.complete();
        this.subs.forEach(s => s.unsubscribe());
        this.onDestroy.next();
    }

    onAddressSelect(address: any) {
        if (address === this.postKzService.unreachableText) {
            return;
        }
        this.selectedAddresseeAddress = this.getFullAdress(address);
        const apartment = this.formGroup.get('apartment').value;
        this.formGroup
            .get('addresseeShortAddress')
            .setValue(address.shortAddress);
        this.formGroup
            .get('addresseeAddress')
            .setValue(concatAddresseeAddress(address, apartment));
        this.formGroup
            .get('republic')
            .setValue(getPart(address, SubjectConstants.republicPartIds));
        this.formGroup
            .get('oblast')
            .setValue(getPart(address, SubjectConstants.oblastPartIds));
        this.formGroup
            .get('region')
            .setValue(getPart(address, SubjectConstants.regionPartIds));
        this.formGroup
            .get('city')
            .setValue(getPart(address, SubjectConstants.cityPartIds));
        this.formGroup
            .get('street')
            .setValue(getPart(address, SubjectConstants.streetPartIds));
        this.filteredPostAddresses = [];
        this.formGroup.markAsDirty();
    }

    onFindCustomerClick() {
        this.openDialogSearchCustomer();
    }

    isVisableEmail() {
        // return (
        //     this.ownerType === OwnerType.Material &&
        //     this.documentType === DocumentType.Outgoing
        // );
        return true;
    }

    getFullAdress(selectedAddress: any): string {
        return `${selectedAddress.addressRus}, индекс: ${selectedAddress.postcode}`;
    }

    onAddressKeyPress(event: KeyboardEvent) {
        if (event.keyCode === 32) {
            event.stopPropagation();
        }
    }

    private toggleEditMode(value: boolean) {
        this.editMode = value;
        this.filteredPostAddresses = [];

        this.editableControls.forEach(c => {
            value ? this.formGroup.get(c).enable() : this.formGroup.get(c).disable();
        });
    }

    private openDialogSearchCustomer() {
        const searchDialog = this.dialog.open(SubjectsSearchDialogComponent, {
            width: '1000px',
            data: {
                isPatentAttorney: false
            }
        });

        searchDialog
            .afterClosed()
            .takeUntil(this.onDestroy)
            .subscribe(result => {
                if (result) {
                    if (result === 'create') {
                        const subject = new SubjectDto();
                        subject.roleCode = CustomerRole.CorrespondingRecipient;

                        const formDialog = this.dialog.open(SubjectCreateDialogComponent, {
                            data: {
                                subject,
                                hideRole: true,
                                isSearchOpened: true,
                                protectionDocTypeCode: this.protectionDocTypeCode,
                                mode: SubjectFormMode.Insert,
                                roleCodes: [
                                    CustomerRole.Declarant,
                                    CustomerRole.Author,
                                    CustomerRole.Confidant,
                                    CustomerRole.CorrespondingRecipient
                                ]
                            },
                            width: '960px'
                        });

                        formDialog
                            .afterClosed()
                            .takeUntil(this.onDestroy)
                            .subscribe(customerResult => {
                                if (customerResult) {
                                    if (customerResult.id) {
                                        this.formGroup.reset();

                                        this.formGroup.get('addresseeXin').setValue(customerResult.xin);
                                        this.formGroup.get('addresseeId').setValue(customerResult.id);
                                        this.formGroup.get('addresseeNameRu').setValue(customerResult.nameRu);
                                        this.formGroup.get('addresseeShortAddress').setValue(customerResult.shortAddress);
                                        this.formGroup.get('addresseeAddress').setValue(customerResult.address);
                                        this.formGroup.get('apartment').setValue(customerResult.apartment);
                                        this.formGroup.get('republic').setValue(customerResult.republic);
                                        this.formGroup.get('oblast').setValue(customerResult.oblast);
                                        this.formGroup.get('region').setValue(customerResult.region);
                                        this.formGroup.get('city').setValue(customerResult.city);
                                        this.formGroup.get('street').setValue(customerResult.street);
                                        this.formGroup.get('contactInfos').setValue(customerResult.contactInfos);
                                        this.formGroup.get('addresseeEmail').setValue(customerResult.email);
                                        this.selectedAddresseeAddress = customerResult.address;
                                    } else {
                                        this.subjectsService
                                            .create(customerResult, OwnerType.None)
                                            .takeUntil(this.onDestroy)
                                            .subscribe(data => {
                                                this.formGroup.reset();

                                                this.formGroup.get('addresseeXin').setValue(data.xin);
                                                this.formGroup.get('addresseeId').setValue(data.customerId);
                                                this.formGroup.get('addresseeNameRu').setValue(data.nameRu);
                                                this.formGroup.get('addresseeShortAddress').setValue(data.shortAddress);
                                                this.formGroup.get('addresseeAddress').setValue(data.address);
                                                this.formGroup.get('apartment').setValue(data.apartment);
                                                this.formGroup.get('addresseeEmail').setValue(data.email);
                                                this.formGroup.get('republic').setValue(data.republic);
                                                this.formGroup.get('oblast').setValue(data.oblast);
                                                this.formGroup.get('region').setValue(data.region);
                                                this.formGroup.get('city').setValue(data.city);
                                                this.formGroup.get('street').setValue(data.street);
                                                this.formGroup.get('contactInfos').setValue(data.contactInfos);
                                                this.selectedAddresseeAddress = data.address;
                                            },
                                            errorMessage => {
                                                this.snackbarHelper.error(errorMessage);
                                            }
                                        );
                                    }
                                }
                            });
                    } else {
                        if (!result.roleCode) {
                          result.roleCode = CustomerRole.CorrespondingRecipient;
                        }

                        const formDialog = this.dialog.open(SubjectCreateDialogComponent, {
                            data: {
                                subject: result,
                                hideRole: true,
                                protectionDocTypeCode: this.protectionDocTypeCode,
                                mode: SubjectFormMode.Attach,
                                roleCodes: [CustomerRole.CorrespondingRecipient]
                            },
                            width: '960px'
                        });

                        formDialog
                            .afterClosed()
                            .takeUntil(this.onDestroy)
                            .subscribe(customerResult => {
                                this.formGroup.reset();

                                this.formGroup.get('addresseeXin').setValue(customerResult.xin);
                                this.formGroup.get('addresseeId').setValue(customerResult.id);
                                this.formGroup.get('addresseeNameRu').setValue(customerResult.nameRu);
                                this.formGroup.get('addresseeAddress').setValue(customerResult.address);
                                this.formGroup.get('addresseeShortAddress').setValue(customerResult.shortAddress);
                                this.formGroup.get('apartment').setValue(customerResult.apartment);
                                this.formGroup.get('addresseeEmail').setValue(customerResult.email);
                                this.formGroup.get('republic').setValue(customerResult.republic);
                                this.formGroup.get('oblast').setValue(customerResult.oblast);
                                this.formGroup.get('region').setValue(customerResult.region);
                                this.formGroup.get('city').setValue(customerResult.city);
                                this.formGroup.get('street').setValue(customerResult.street);
                                this.formGroup.get('contactInfos').setValue(customerResult.contactInfos);
                                this.selectedAddresseeAddress = customerResult.address;
                            });
                    }
                }
            });
    }

    /**
     * Парсит значение `input` элемента и отбрасывает все символы не являющиеся цифрами.
     * @param event Событие
     */
    parseToNumeric(event): void {
        const { target } = event;

        if (target instanceof HTMLInputElement) {
            const parsedValue = target.value.replace(/[^0-9]*/g, '');

            target.value = parsedValue;
        }
    }

    private buildForm() {
        this.formGroup = this.fb.group({
            addresseeId: [{ value: '', disabled: true }, Validators.required],
            addresseeXin: [
                { value: '', disabled: true },
                [
                    Validators.required,
                    Validators.minLength(12),
                    Validators.maxLength(12),
                    xinValidator
                ]
            ],
            contactInfos: [{ value: '', disabled: true }],
            addresseeNameRu: [{ value: '', disabled: true }],
            addresseeAddress: [{ value: '', disabled: true }],
            addresseeShortAddress: [{ value: '', disabled: true }],
            apartment: [{ value: '', disabled: true }],
            addresseeEmail: [{ value: '', disabled: true }],
            republic: [{ value: '', disabled: true }],
            oblast: [{ value: '', disabled: true }],
            region: [{ value: '', disabled: true }],
            city: [{ value: '', disabled: true }],
            street: [{ value: '', disabled: true }]
        });

        this.editableControls = [
            // 'addresseeId',
            // 'addresseeXin',
            // 'contactInfos',
            // 'addresseeAddress',
            // 'addresseeShortAddress',
            // 'apartment'
            'addresseeId',
            'addresseeXin',
            'addresseeNameRu',
            'addresseeAddress',
            'addresseeShortAddress',
            'addresseeEmail',
            'apartment',
            'republic',
            'oblast',
            'region',
            'city',
            'street'
        ];
    }

    /**
     * Подписывается на события изменения значения в поле XIN и запрашивает данные с бэкенда
     *
     * @private
     * @memberof RequestComponent
     */
    private subscribeXinInput() {
        const addresseeXinControl = this.formGroup.get('addresseeXin');
        addresseeXinControl.valueChanges
            .takeUntil(this.onDestroy)
            .debounceTime(this.configService.debounceTime)
            .filter(
                value =>
                    value &&
                    value.toString().length === 12 &&
                    addresseeXinControl.valid &&
                    addresseeXinControl.dirty
            )
            .distinctUntilChanged()
            .subscribe((value: number) => {
                this.getAddressee(value);
            });
    }

    private getAddressee(value: number) {
        this.customerService
            .getByXin(value.toString(), false)
            .takeUntil(this.onDestroy)
            .subscribe(
                (customerInfo: CustomerShortInfo) => {
                    if (!customerInfo) {
                        this.snackbarHelper.success(
                            'Пользователь с таким БИН/ИИН не найден'
                        );
                        return;
                    }
                    this.formGroup.get('addresseeId').setValue(customerInfo.id);
                    this.formGroup.get('addresseeNameRu').setValue(customerInfo.nameRu);
                    this.formGroup.get('addresseeShortAddress').setValue(customerInfo.address);
                    this.formGroup.get('addresseeAddress').setValue(customerInfo.address);
                    this.formGroup.get('apartment').setValue(customerInfo.apartment);
                    this.formGroup.get('addresseeEmail').setValue(customerInfo.email);
                    this.formGroup.get('republic').setValue(customerInfo.republic);
                    this.formGroup.get('oblast').setValue(customerInfo.oblast);
                    this.formGroup.get('region').setValue(customerInfo.region);
                    this.formGroup.get('city').setValue(customerInfo.city);
                    this.formGroup.get('street').setValue(customerInfo.street);
                    this.selectedAddresseeAddress = customerInfo.address;
                    this.formGroup.markAsDirty();
                },
                err => {
                    this.formGroup.get('addresseeXin').setErrors({ xin: true });
                    this.formGroup.get('addresseeId').setValue('');
                    this.formGroup.get('addresseeNameRu').setValue('');
                    this.formGroup.get('addresseeShortAddress').setValue('');
                    this.formGroup.get('addresseeAddress').setValue('');
                    this.formGroup.get('apartment').setValue('');
                    this.formGroup.get('addresseeEmail').setValue('');
                    this.formGroup.get('republic').setValue('');
                    this.formGroup.get('oblast').setValue('');
                    this.formGroup.get('region').setValue('');
                    this.formGroup.get('city').setValue('');
                    this.formGroup.get('street').setValue('');
                    this.formGroup.get('contactInfos').setValue('');
                }
            );
    }

    /**
     * Подписывается на события изменения значения в поле addresseeAddress и запрашивает данные с казпочты
     *
     * @private
     * @memberof RequestComponent
     */
    private subscribeAddressInput() {
        let postKzSubs: Subscription;
        const addresseeAddress = this.formGroup.get('addresseeAddress');
        addresseeAddress.valueChanges
            .takeUntil(this.onDestroy)
            .debounceTime(this.configService.debounceTime)
            .filter(
                value =>
                    value &&
                    value.length > 4 &&
                    addresseeAddress.dirty &&
                    addresseeAddress.valid
            )
            .distinctUntilChanged()
            .subscribe((value: string) => {
                if (postKzSubs && !postKzSubs.closed) {
                    postKzSubs.unsubscribe();
                }
                postKzSubs = this.postKzService
                    .get(value)
                    .takeUntil(this.onDestroy)
                    .subscribe((addresses: AddressResponse) => {
                        this.filteredPostAddresses = addresses ? addresses.data : [];
                    });
            });
    }

    getValue() {
        return this.value;
    }
    ngAfterViewInit(): void {
        // TODO: workaround против этого бага https://github.com/angular/material2/issues/5593
        this.detectorRef.detectChanges();
    }
}

export class AddresseeInfo {
    addresseeId: number;
    addresseeXin: string;
    addresseeNameRu: string;
    addresseeAddress: string;
    addresseeShortAddress: string;
    apartment: string;
    email: string;
    republic: string;
    oblast: string;
    region: string;
    city: string;
    street: string;
    contactInfos: ContactInfoDto[];
}
