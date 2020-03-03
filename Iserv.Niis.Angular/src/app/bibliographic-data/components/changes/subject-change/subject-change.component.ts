import {
    Component,
    Input,
    OnChanges,
    OnDestroy,
    OnInit,
    SimpleChanges
} from '@angular/core';
import {
    FormBuilder,
    FormControl,
    FormGroup,
    Validators
} from '@angular/forms';
import { MatDialog } from '@angular/material';
import { SubjectChangesComponent } from 'app/bibliographic-data/models/changes-dto';
import { ConfigService } from 'app/core';
import { SnackBarHelper } from 'app/core/snack-bar-helper.service';
import { AddressResponse, PostKzService } from 'app/modules/postkz';
import { CustomerService } from 'app/shared/services/customer.service';
import { DictionaryService } from 'app/shared/services/dictionary.service';
import { BaseDictionary } from 'app/shared/services/models/base-dictionary';
import { CustomerShortInfo } from 'app/shared/services/models/customer-short-info';
import { DictionaryType } from 'app/shared/services/models/dictionary-type.enum';
import { SelectOption } from 'app/shared/services/models/select-option';
import {
    phoneMask,
    xinValidator
} from 'app/shared/services/validator/custom-validators';
import { SubjectFormDialogComponent } from 'app/subjects/components/subject-form-dialog/subject-form-dialog.component';
import { SubjectsSearchDialogComponent } from 'app/subjects/components/subjects-search-dialog/subjects-search-dialog.component';
import {
    concatAddresseeAddress,
    concatFullAddress,
    ContactInfoDto,
    getPart,
    SubjectConstants,
    SubjectDto
} from 'app/subjects/models/subject.model';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { Subscription } from 'rxjs/Subscription';
import { SubjectSpecialFields } from 'app/subjects/enums/subject-special-fields.enum';
import { SubjectFieldsConfig } from 'app/subjects/models/subject-fields-config.model';
import { CustomerRole } from 'app/subjects/enums/customer-role.enum';

const availableCustomerTypeCodes = ['1', '2'];
const authorlessRequestTypeCodes = [
    '001.001A',
    '001.001A.1',
    '001.001A_1',
    '001.001A_2'
];

@Component({
    selector: 'app-subject-change',
    templateUrl: './subject-change.component.html',
    styleUrls: ['./subject-change.component.scss']
})
export class SubjectChangeComponent
    implements OnInit, OnChanges, OnDestroy, SubjectChangesComponent {
    @Input()
    editMode: boolean;
    @Input()
    subject: SubjectDto;
    @Input()
    roleCode: string;
    @Input()
    ownerId: number;
    @Input()
    subjectId: number;

    formGroup: FormGroup;

    dicCustomerRoles: SelectOption[];
    dicCustomerTypes: SelectOption[];
    dicCountries: SelectOption[];
    filteredDicCountries: Observable<SelectOption[]>;
    dicContactInfotypes: BaseDictionary[];
    dicBeneficiaryTypes: BaseDictionary[] = [];
    filteredPostAddresses: any[];

    phones: ContactInfoDto[];
    mobilePhones: ContactInfoDto[];
    faxes: ContactInfoDto[];
    emails: ContactInfoDto[];

    isResident = false;
    phoneMask = phoneMask;

    editableControls: string[] = [];

    get AddressDetailedFieldType() {
        return SubjectSpecialFields.AddressDetailed;
    }
    get AddressSimpleFieldType() {
        return SubjectSpecialFields.AddressSimple;
    }
    get ContactsFieldType() {
        return SubjectSpecialFields.Contacts;
    }
    get CountryFieldType() {
        return SubjectSpecialFields.Country;
    }
    get LicenseDatesFieldType() {
        return SubjectSpecialFields.LicenseDates;
    }
    get RegNumberFieldType() {
        return SubjectSpecialFields.RegNumber;
    }
    get ResidencyStatusFieldType() {
        return SubjectSpecialFields.ResidencyStatus;
    }

    fieldsConfig: SubjectFieldsConfig[] = [
        {
            roleCodes: [
                CustomerRole.Declarant,
                CustomerRole.CorrespondingRecipient,
                CustomerRole.Confidant
            ],
            specialField: SubjectSpecialFields.AddressDetailed
        },
        {
            roleCodes: [
                CustomerRole.PatentAttorney
            ],
            specialField: SubjectSpecialFields.AddressSimple
        },
        {
            roleCodes: [
                CustomerRole.Declarant,
                CustomerRole.PatentAttorney,
                CustomerRole.CorrespondingRecipient
            ],
            specialField: SubjectSpecialFields.Contacts
        },
        {
            roleCodes: [
                CustomerRole.Declarant
            ],
            specialField: SubjectSpecialFields.Country
        },
        {
            roleCodes: [
                CustomerRole.Confidant
            ],
            specialField: SubjectSpecialFields.LicenseDates
        },
        {
            roleCodes: [
                CustomerRole.PatentAttorney
            ],
            specialField: SubjectSpecialFields.RegNumber
        },
        {
            roleCodes: [
                CustomerRole.Declarant
            ],
            specialField: SubjectSpecialFields.ResidencyStatus
        }
    ];

    residencyStatuses: any[] = [
        { code: 'res', nameRu: 'Резидент' },
        { code: 'nrs', nameRu: 'Не резидент' }
    ];
    roleCodes: string[] = [];
    protectionDocTypeCode: string;

    private onDestroy = new Subject();

    get faxesGroup() {
        return this.formGroup.get('faxesGroup') as FormGroup;
    }

    get phonesGroup() {
        return this.formGroup.get('phonesGroup') as FormGroup;
    }

    get mobilePhonesGroup() {
        return this.formGroup.get('mobilePhonesGroup') as FormGroup;
    }

    get emailsGroup() {
        return this.formGroup.get('emailsGroup') as FormGroup;
    }

    constructor(
        private fb: FormBuilder,
        private postKzService: PostKzService,
        private dictionaryService: DictionaryService,
        private customerService: CustomerService,
        private configService: ConfigService,
        private snackbarHelper: SnackBarHelper,
        private dialog: MatDialog
    ) {
        this.buildForm();
        this.editableControls = [
            'typeId',
            'isBeneficiary',
            'nameRu',
            'nameEn',
            'nameKz',
            'jurRegNumber',
            'powerAttorneyFullNum',
            'apartment',
            'phonesGroup',
            'mobilePhonesGroup',
            'faxesGroup',
            'emailsGroup',
            'countryId',
            'isNotMention',
            'dateBegin',
            'dateEnd',
            'address',
            'addressKz',
            'addressEn'
        ];
        this.toggleEditMode(this.editMode);
    }

    ngOnInit() {
        this.initSelectOptions();
        this.subscribeXinInput();
        this.subscribeAddressInput();
    }

    ngOnChanges(changes: SimpleChanges): void {
        if (changes.subject && changes.subject.currentValue) {
            this.formGroup.reset(this.subject);
        }
    }

    ngOnDestroy(): void {
        this.faxes = [];
        this.mobilePhones = [];
        this.emails = [];
        this.phones = [];
        this.onDestroy.next();
    }

    private initSelectOptions() {
        Observable.combineLatest(
            this.dictionaryService.getSelectOptions(DictionaryType.DicCustomerRole),
            this.dictionaryService.getSelectOptions(DictionaryType.DicCustomerType),
            this.dictionaryService.getBaseDictionary(
                DictionaryType.DicContactInfoType
            ),
            this.dictionaryService.getSelectOptions(DictionaryType.DicCountry),
            this.dictionaryService.getBaseDictionary(
                DictionaryType.DicBeneficiaryType
            )
        )
            .takeUntil(this.onDestroy)
            .subscribe(
                ([roles, types, contactInfoTypes, countries, beneficiaries]) => {
                    this.dicCustomerRoles = roles
                        .filter(
                            cr =>
                                this.roleCodes.length === 0 || this.roleCodes.includes(cr.code)
                        )
                        .filter(cr =>
                            authorlessRequestTypeCodes.includes(this.protectionDocTypeCode)
                                ? cr.code !== '2'
                                : true
                        );

                    this.dicCustomerTypes = types.filter(ct =>
                        availableCustomerTypeCodes.includes(ct.code)
                    );
                    this.dicContactInfotypes = contactInfoTypes;
                    this.initializeContactInfos(this.subject);

                    this.dicCountries = countries;

                    this.dicCountries.forEach(c => {
                        c.nameRu = c.code;
                    });

                    const country = this.dicCountries.find(
                        c => c.id === this.subject.countryId
                    );

                    this.dicBeneficiaryTypes = beneficiaries;
                    this.dicBeneficiaryTypes.unshift(
                        this.dictionaryService.emptyBaseDictionary
                    );

                    this.setCountryControls(country);
                    this.formGroup.reset(this.subject);
                }
            );
    }

    /**
     * Подписывается на события изменения значения в поле XIN и запрашивает данные с бэкенда
     *
     * @private
     * @memberof SubjectsFormComponent
     */
    private subscribeXinInput() {
        const xinControl = this.formGroup.controls.xin;
        xinControl.valueChanges
            .takeUntil(this.onDestroy)
            .debounceTime(this.configService.debounceTime)
            .filter(
                values =>
                    values === null ||
                    (values.toString().length === 12 &&
                        xinControl.valid &&
                        xinControl.dirty)
            )
            .distinctUntilChanged()
            .subscribe(values => {
                if (values === null) {
                    return;
                }
                this.customerService
                    .getByXin(values.toString(), this.isPatentAttorney())
                    .takeUntil(this.onDestroy)
                    .subscribe(
                        (customerInfo: CustomerShortInfo) => {
                            if (!customerInfo) {
                                this.snackbarHelper.success(
                                    'Пользователь с таким БИН/ИИН не найден'
                                );
                                return;
                            }
                            this.initializeCustomerInfo(customerInfo);
                        },
                        err => {
                            this.initializeCustomerInfoBydefault();

                            if (err.status === 404) {
                                if (!this.isPatentAttorney()) {
                                    this.formGroup.get('xin').setErrors({ xin: false });
                                    this.formGroup.get('xin').updateValueAndValidity();
                                    this.snackbarHelper.success(
                                        err.message +
                                            '\nПри сохранении будет создан новый контрагент'
                                    );
                                }
                                if (this.isPatentAttorney()) {
                                    this.snackbarHelper.error(
                                        'Данного пользователя нет в справочнике патентных поверенных. Просьба обратиться в Администратору системы.'
                                    );
                                }
                            } else {
                                this.snackbarHelper.error(err.message);
                            }
                        }
                    );
            });
    }

    /**
     * Подписывается на события изменения значения в поле address и запрашивает данные с казпочты
     *
     * @private
     * @memberof SubjectsFormComponent
     */
    private subscribeAddressInput() {
        let postKzSubs: Subscription;
        const address = this.formGroup.controls.address;
        address.valueChanges
            .takeUntil(this.onDestroy)
            .debounceTime(this.configService.debounceTime)
            .filter(
                value => value && value.length > 4 && address.dirty && address.valid
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

    private initializeCustomerInfo(customerInfo: CustomerShortInfo) {
        const coгntry = this.dicCountries.find(
            c => c.id === customerInfo.countryId
        );
        this.setCountryControls(coгntry);
        this.formGroup.get('customerId').setValue(customerInfo.id);
        this.formGroup.get('xin').setValue(customerInfo.xin);
        this.formGroup.get('nameRu').setValue(customerInfo.nameRu);
        this.formGroup.get('nameEn').setValue(customerInfo.nameEn);
        this.formGroup.get('nameKz').setValue(customerInfo.nameKz);
        if (customerInfo.ownerAddresseeAddress) {
            this.formGroup
                .get('address')
                .setValue(customerInfo.ownerAddresseeAddress);
        } else {
            this.formGroup.get('address').setValue(customerInfo.address);
            this.formGroup.get('addressKz').setValue(customerInfo.addressKz);
            this.formGroup.get('addressEn').setValue(customerInfo.addressEn);
        }
        this.formGroup.get('republic').setValue(customerInfo.republic);
        this.formGroup.get('oblast').setValue(customerInfo.oblast);
        this.formGroup.get('city').setValue(customerInfo.city);
        this.formGroup.get('street').setValue(customerInfo.street);
        this.formGroup.get('typeId').setValue(customerInfo.typeId);
        this.formGroup.get('isBeneficiary').setValue(customerInfo.isBeneficiary);
        this.formGroup.get('jurRegNumber').setValue(customerInfo.jurRegNumber);
        this.formGroup
            .get('powerAttorneyFullNum')
            .setValue(customerInfo.powerAttorneyFullNum);
        this.formGroup.get('isNotResident').setValue(customerInfo.isNotResident);
        this.formGroup.get('isNotMention').setValue(customerInfo.isNotMention);
        this.formGroup.get('apartment').setValue(customerInfo.apartment);
        this.formGroup
            .get('beneficiaryTypeId')
            .setValue(customerInfo.beneficiaryTypeId);
        this.formGroup.markAsDirty();
    }

    private initializeCustomerInfoBydefault() {
        this.formGroup.get('xin').setErrors({ xin: true });
        this.formGroup.get('customerId').setValue(undefined);
        this.formGroup.get('nameRu').setValue('');
        this.formGroup.get('nameEn').setValue('');
        this.formGroup.get('nameKz').setValue('');
        this.formGroup.get('address').setValue('');
        this.formGroup.get('addressKz').setValue('');
        this.formGroup.get('addressEn').setValue('');
        this.formGroup.get('republic').setValue('');
        this.formGroup.get('oblast').setValue('');
        this.formGroup.get('city').setValue('');
        this.formGroup.get('street').setValue('');
        this.formGroup.get('typeId').setValue('');
        this.formGroup.get('countryId').setValue('');
        this.formGroup.get('countryCode').setValue('');
        this.formGroup.get('isBeneficiary').setValue('');
        this.formGroup.get('jurRegNumber').setValue('');
        this.formGroup.get('powerAttorneyFullNum').setValue('');
        this.formGroup.get('isNotResident').setValue('');
        this.formGroup.get('isNotMention').setValue('');
        this.formGroup.get('apartment').setValue('');
        this.formGroup.get('beneficiaryTypeId').setValue('');
    }

    private setCountryControls(country: SelectOption) {
        if (country) {
            this.formGroup.get('countryId').setValue(country.id);
            this.formGroup.get('countryCode').setValue(country.code);
        } else {
            this.formGroup.get('countryId').setValue(null);
            this.formGroup.get('countryCode').setValue(null);
        }
    }

    onSearchCustomersClick() {
        this.openDialogSearchCustomer();
    }

    private openDialogSearchCustomer() {
        const searchDialog = this.dialog.open(SubjectsSearchDialogComponent, {
            width: '1200px',
            data: { isPatentAttorney: this.isPatentAttorney() }
        });

        searchDialog
            .afterClosed()
            .takeUntil(this.onDestroy)
            .subscribe(result => {
                if (result) {
                    if (result === 'create') {
                        this.subject.contactInfos = [];
                        this.formGroup.markAsDirty();
                        this.formGroup.patchValue({
                            id: 0,
                            xin: '',
                            customerId: '',
                            typeId: '',
                            countryId: '',
                            isBeneficiary: '',
                            nameRu: '',
                            nameEn: '',
                            nameKz: '',
                            address: '',
                            addressKz: '',
                            addressEn: '',
                            apartment: '',
                            phonesGroup: this.fb.group([]),
                            mobilePhonesGroup: this.fb.group([]),
                            faxesGroup: this.fb.group([]),
                            emailsGroup: this.fb.group([]),
                            jurRegNumber: '',
                            powerAttorneyFullNum: '',
                            dateBegin: '',
                            dateEnd: '',
                            isNotResident: '',
                            isNotMention: '',
                            beneficiaryTypeId: '',
                            republic: '',
                            oblast: '',
                            city: '',
                            street: ''
                        });
                        this.toggleEditMode(this.editMode);
                    } else {
                        this.formGroup.markAsDirty();
                        if (
                            this.isSelectedCustomerRole(CustomerRole.PatentAttorney) ||
                            this.isSelectedCustomerRole(CustomerRole.Confidant)
                        ) {
                            this.formGroup.patchValue({
                                xin: result.xin,
                                customerId: result.id,
                                typeId: result.typeId,
                                isBeneficiary: result.isBeneficiary,
                                nameRu: result.nameRu,
                                nameEn: result.nameEn,
                                nameKz: result.nameKz,
                                address: result.address,
                                addressKz: result.addressKz,
                                addressEn: result.addressEn,
                                apartment: result.apartment,
                                jurRegNumber: result.jurRegNumber,
                                powerAttorneyFullNum: result.powerAttorneyFullNum,
                                dateBegin: result.dateBegin,
                                dateEnd: result.dateEnd,
                                isNotResident: result.isNotResident,
                                isNotMention: result.isNotMention,
                                beneficiaryTypeId: result.beneficiaryTypeId,
                                republic: result.republic,
                                oblast: result.oblast,
                                city: result.city,
                                street: result.street
                            });
                        } else {
                            this.formGroup.patchValue({
                                xin: result.xin,
                                customerId: result.id,
                                typeId: result.typeId,
                                countryId: result.countryId,
                                isBeneficiary: result.isBeneficiary,
                                nameRu: result.nameRu,
                                nameEn: result.nameEn,
                                nameKz: result.nameKz,
                                address: result.address,
                                addressKz: result.addressKz,
                                addressEn: result.addressEn,
                                apartment: result.apartment,
                                jurRegNumber: result.jurRegNumber,
                                powerAttorneyFullNum: result.powerAttorneyFullNum,
                                dateBegin: result.dateBegin,
                                dateEnd: result.dateEnd,
                                isNotResident: result.isNotResident,
                                isNotMention: result.isNotMention,
                                beneficiaryTypeId: result.beneficiaryTypeId,
                                republic: result.republic,
                                oblast: result.oblast,
                                city: result.city,
                                street: result.street
                            });
                        }

                        this.initializeContactInfos(result);
                    }
                }
            });
    }

    isPatentAttorney(): boolean {
        this.isFound();
        if (this.formGroup.get('roleId').value) {
            const result = this.formGroup.get('roleId').value === 4;
            return result;
        }
        return false;
    }

    private toggleEditMode(value: boolean) {
        const customerId = this.formGroup.get('customerId');
        const roleId = this.formGroup.get('roleId');
        const xin = this.formGroup.get('xin');
        if (value) {
            customerId.clearValidators();
            roleId.clearValidators();
            xin.clearValidators();
        } else {
            customerId.setValidators(Validators.required);
            roleId.setValidators(Validators.required);
            xin.setValidators(xinValidator);
        }
        customerId.updateValueAndValidity();
        roleId.updateValueAndValidity();
        xin.updateValueAndValidity();

        this.editableControls.forEach(c => {
            value ? this.formGroup.get(c).enable() : this.formGroup.get(c).disable();
        });
    }

    isFound() {
        if (this.formGroup.get('customerId').value) {
            this.formGroup.get('nameRu').disable();
        } else {
            this.formGroup.get('nameRu').enable();
        }
    }

    private initializeContactInfos(subject: SubjectDto) {
        const emailType = this.dicContactInfotypes.filter(t => t.code === 'email');
        const faxType = this.dicContactInfotypes.filter(t => t.code === 'fax');
        const phoneType = this.dicContactInfotypes.filter(t => t.code === 'phone');
        const mobliePhoneType = this.dicContactInfotypes.filter(
            t => t.code === 'mobilePhone'
        );
        if (subject && !subject.contactInfos) {
            subject.contactInfos = [];
            this.phones = [];
            this.emails = [];
            this.mobilePhones = [];
            this.faxes = [];
        } else if (!subject) {
            this.phones = [];
            this.emails = [];
            this.mobilePhones = [];
            this.faxes = [];
        } else if (subject.contactInfos) {
            this.phones = subject.contactInfos.filter(ci =>
                phoneType.length > 0 ? ci.typeId === phoneType[0].id : false
            );
            this.mobilePhones = subject.contactInfos.filter(ci =>
                mobliePhoneType.length > 0 ? ci.typeId === mobliePhoneType[0].id : false
            );
            this.faxes = subject.contactInfos.filter(ci =>
                faxType.length > 0 ? ci.typeId === faxType[0].id : false
            );
            this.emails = subject.contactInfos.filter(ci =>
                emailType.length > 0 ? ci.typeId === emailType[0].id : false
            );
        }
        this.setContactInfoFormValue(this.phones, this.phonesGroup, 'phone', 0);
        this.setContactInfoFormValue(
            this.mobilePhones,
            this.mobilePhonesGroup,
            'mobilePhone',
            0
        );
        this.setContactInfoFormValue(this.faxes, this.faxesGroup, 'fax', 0);
        this.setContactInfoFormValue(this.emails, this.emailsGroup, 'email', 0);
    }

    isSelectedCustomerRole(roleCode: string): boolean {
        const roleId = this.formGroup.controls.roleId.value;
        if (isNaN(roleId) === false && roleId > 0 && this.dicCustomerRoles) {
            const currentRoleCode = this.dicCustomerRoles.find(r => r.id === roleId)
                .code;
            return currentRoleCode === roleCode;
        } else {
            return false;
        }
    }

    private setContactInfoFormValue(
        infos: ContactInfoDto[],
        formGroup: FormGroup,
        name: string,
        index: number
    ) {
        infos.forEach(info => {
            formGroup.addControl(name + index, new FormControl());
            formGroup.get(name + index++).patchValue(info.info);
        });
    }

    isFieldActive(field: SubjectSpecialFields): boolean {
        return this.fieldsConfig.some(
            fc => fc.roleCodes.includes(<CustomerRole>this.roleCode) && fc.specialField === field
        );
    }

    getResidencyValue(): string {
        return this.isNotResident()
            ? this.residencyStatuses.find(rs => rs.code === 'nrs').code
            : this.residencyStatuses.find(rs => rs.code === 'res').code;
    }

    isNotResident(): boolean {
        return this.formGroup.get('countryCode').value !== 'KZ';
    }

    onCountryChange(value: any) {
        this.formGroup.get('countryCode').setValue(value.code);
        if (value.code === 'KZ') {
            this.isResident = true;
        } else {
            this.isResident = false;
        }
    }

    onAddressSelect(address: any) {
        if (address === this.postKzService.unreachableText) {
            return;
        }

        const apartment = this.formGroup.get('apartment').value;
        this.formGroup
            .get('address')
            .setValue(concatAddresseeAddress(address, apartment));
        this.formGroup
            .get('republic')
            .setValue(getPart(address, SubjectConstants.republicPartIds));
        this.formGroup
            .get('oblast')
            .setValue(getPart(address, SubjectConstants.oblastPartIds));
        this.formGroup
            .get('city')
            .setValue(getPart(address, SubjectConstants.cityPartIds));
        this.formGroup
            .get('street')
            .setValue(getPart(address, SubjectConstants.streetPartIds));
        this.filteredPostAddresses = [];
        this.formGroup.markAsDirty();
    }

    getFullAdress(selectedAddress: any): string {
        return `${selectedAddress.addressRus}, индекс: ${selectedAddress.postcode}`;
    }

    getAddress(address: any) {
        const apartment = this.formGroup.get('apartment').value;
        if (
            this.isSelectedCustomerRole(CustomerRole.CorrespondingRecipient)
        ) {
            return concatAddresseeAddress(address, apartment);
        }
        return concatFullAddress(address, apartment);
    }

    onFaxDelete(index: number) {
        this.faxes.splice(index, 1);
        this.faxesGroup.removeControl('fax' + index);
    }

    onFaxAdd() {
        this.addNewFax();
        this.faxesGroup.addControl(
            'fax'.concat((this.faxes.length - 1).toString()),
            new FormControl()
        );
    }

    private addNewFax() {
        const faxType = this.dicContactInfotypes.filter(t => t.code === 'fax');
        const fax = new ContactInfoDto();
        if (faxType.length > 0) {
            fax.typeId = faxType[0].id;
            this.faxes.push(fax);
        }
    }

    onPhoneDelete(index: number) {
        this.phones.splice(index, 1);
        this.phonesGroup.removeControl('phone' + index);
    }

    onPhoneAdd() {
        this.addNewPhone();
        this.phonesGroup.addControl(
            'phone'.concat((this.phones.length - 1).toString()),
            new FormControl()
        );
    }

    private addNewPhone() {
        const phoneType = this.dicContactInfotypes.filter(t => t.code === 'phone');
        const phone = new ContactInfoDto();
        if (phoneType.length > 0) {
            phone.typeId = phoneType[0].id;
            this.phones.push(phone);
        }
    }

    onMobilePhoneDelete(index: number) {
        this.mobilePhones.splice(index, 1);
        this.mobilePhonesGroup.removeControl('mobilePhone' + index);
    }

    onMobilePhoneAdd() {
        this.addNewMobilePhone();
        this.mobilePhonesGroup.addControl(
            'mobilePhone'.concat((this.mobilePhones.length - 1).toString()),
            new FormControl()
        );
    }

    private addNewMobilePhone() {
        const mobilePhoneType = this.dicContactInfotypes.filter(
            t => t.code === 'mobilePhone'
        );
        const mobilePhone = new ContactInfoDto();
        if (mobilePhoneType.length > 0) {
            mobilePhone.typeId = mobilePhoneType[0].id;
            this.mobilePhones.push(mobilePhone);
        }
    }

    onEmailDelete(index: number) {
        this.emails.splice(index, 1);
        this.emailsGroup.removeControl('email' + index);
    }

    onEmailAdd() {
        this.addNewEmail();
        this.emailsGroup.addControl(
            'email'.concat((this.emails.length - 1).toString()),
            new FormControl()
        );
    }

    private addNewEmail() {
        const emailType = this.dicContactInfotypes.filter(t => t.code === 'email');
        const email = new ContactInfoDto();
        if (emailType.length > 0) {
            email.typeId = emailType[0].id;
            this.emails.push(email);
        }
    }

    getValue(): SubjectDto {
        this.formGroup.markAsPristine();
        const value = this.formGroup.getRawValue();
        const country = this.formGroup.get('countryId').value;
        if (country) {
            value.countryId = country.id;
        }
        delete value.emailsGroup;
        delete value.faxesGroup;
        delete value.phonesGroup;
        delete value.mobilePhonesGroup;
        const role = this.dicCustomerRoles.find(r => r.code === this.roleCode);
        value.roleId = !!role ? role.id : 0;
        value.ownerId = this.ownerId;
        value.id = this.subjectId;
        value.contactInfos = [];
        this.setContactInfoData(this.faxes, this.faxesGroup);
        this.setContactInfoData(this.phones, this.phonesGroup);
        this.setContactInfoData(this.mobilePhones, this.mobilePhonesGroup);
        this.setContactInfoData(this.emails, this.emailsGroup);
        this.faxes.forEach(f => value.contactInfos.push(f));
        this.emails.forEach(e => value.contactInfos.push(e));
        this.mobilePhones.forEach(mp => value.contactInfos.push(mp));
        this.phones.forEach(p => value.contactInfos.push(p));
        return value;
    }

    setValue(value: SubjectDto) {
        this.subject = value;
        this.formGroup.reset(value);
    }

    private buildForm() {
        this.formGroup = this.fb.group({
            id: [{ value: '', disabled: true }],
            customerId: ['', Validators.required],
            ownerId: [{ value: '', disabled: true }],
            roleId: ['', Validators.required],
            typeId: [{ value: '', disabled: true }, Validators.required],
            countryId: [{ value: '', disabled: true }],
            countryCode: [{ value: '', disabled: true }],
            isBeneficiary: [{ value: '', disabled: true }],
            xin: [
                '',
                [Validators.minLength(12), Validators.maxLength(12), xinValidator]
            ],
            nameRu: [{ value: '', disabled: true }],
            nameEn: [{ value: '', disabled: true }],
            nameKz: [{ value: '', disabled: true }],
            address: [{ value: '', disabled: true }],
            addressKz: [{ value: '', disabled: true }],
            addressEn: [{ value: '', disabled: true }],
            apartment: [{ value: '', disabled: true }],
            faxesGroup: this.fb.group([]),
            phonesGroup: this.fb.group([]),
            mobilePhonesGroup: this.fb.group([]),
            emailsGroup: this.fb.group([]),
            jurRegNumber: [{ value: '', disabled: true }],
            powerAttorneyFullNum: [{ value: '', disabled: true }],
            dateBegin: [''],
            dateEnd: [''],
            isNotResident: [{ value: '', disabled: true }],
            isNotMention: [{ value: '' }],
            beneficiaryTypeId: [{ value: '', disabled: true }],
            republic: [{ value: '', disabled: true }],
            oblast: [{ value: '', disabled: true }],
            city: [{ value: '', disabled: true }],
            street: [{ value: '', disabled: true }]
        });
    }

    private setContactInfoData(infos: ContactInfoDto[], formGroup: FormGroup) {
        let i = 0;
        Object.values(formGroup.getRawValue())
            .filter(value => !!value && infos.length > 0)
            .forEach(value => {
                infos[i].info = value.toString();
                i++;
            });
    }
}
