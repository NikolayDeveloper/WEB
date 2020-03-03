import {
    AfterViewInit,
    Component,
    Inject,
    OnDestroy,
    OnInit,
    ChangeDetectorRef
} from '@angular/core';
import {
    FormBuilder,
    FormControl,
    FormGroup,
    Validators
} from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { SnackBarHelper } from 'app/core/snack-bar-helper.service';
import { AddressResponse, PostKzService } from 'app/modules/postkz';
import { DictionaryService } from 'app/shared/services/dictionary.service';
import { BaseDictionary } from 'app/shared/services/models/base-dictionary';
import { SubjectsSearchDialogComponent } from 'app/subjects/components/subjects-search-dialog/subjects-search-dialog.component';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { Subscription } from 'rxjs/Subscription';
import { ConfigService } from '../../../core';
import { RouteStageCodes } from '../../../shared/models/route-stage-codes';
import { CustomerService } from '../../../shared/services/customer.service';
import { CustomerShortInfo } from '../../../shared/services/models/customer-short-info';
import { DictionaryType } from '../../../shared/services/models/dictionary-type.enum';
import { OwnerType } from '../../../shared/services/models/owner-type.enum';
import { SelectOption } from '../../../shared/services/models/select-option';
import {
    phoneMask,
    xinValidator
} from '../../../shared/services/validator/custom-validators';
import {
    concatAddresseeAddress,
    concatFullAddress,
    ContactInfoDto,
    getPart,
    SubjectConstants,
    SubjectDto
} from '../../models/subject.model';
import { SubjectFormMode } from '../../enums/subject-form-mode.enum';
import { SubjectSpecialFields } from '../../enums/subject-special-fields.enum';
import { SubjectFieldsConfig } from '../../models/subject-fields-config.model';
import { CustomerRole } from '../../enums/customer-role.enum';

const availableCustomerTypeCodes = ['1', '2'];
const authorlessRequestTypeCodes = [
    '001.001A',
    '001.001A.1',
    '001.001A_1',
    '001.001A_2'
];

@Component({
    selector: 'app-subject-form-dialog',
    templateUrl: './subject-form-dialog.component.html',
    styleUrls: ['./subject-form-dialog.component.scss']
})
export class SubjectFormDialogComponent implements OnInit, OnDestroy, AfterViewInit {
    formGroup: FormGroup;
    dicCountryKZCode = 'KZ'; // Республика Казахстан
    customerTypeNonResidentCode = 'NR'; // Нерезидент

    dicCustomerRoles: SelectOption[];
    dicCustomerTypes: SelectOption[];
    dicCountries: SelectOption[];
    filteredDicCountries: Observable<SelectOption[]>;
    dicContactInfotypes: BaseDictionary[];
    dicBeneficiaryTypes: BaseDictionary[] = [];

    filteredPostAddresses: any[];
    selectedAddress: string;

    phoneMask = phoneMask;
    phones: ContactInfoDto[];
    mobilePhones: ContactInfoDto[];
    faxes: ContactInfoDto[];
    emails: ContactInfoDto[];

    subject: SubjectDto;
    protectionDocTypeCode: string;
    roleCodes: string[];

    mode: SubjectFormMode;
    editableControls: string[] = [];

    hasProxy: boolean;

    ownerType: OwnerType;
    ownerId: number;

    isResident = false;

    private onDestroy = new Subject();

    isCountryRequired = false;

    get pdTypeTMCodes() {
        return RouteStageCodes.pdTypeTMCodes;
    }
    get pdTypeCOOCodes() {
        return RouteStageCodes.pdTypeCOOCodes;
    }
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
                CustomerRole.Confidant,
                CustomerRole.Author
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
                CustomerRole.CorrespondingRecipient,
                CustomerRole.Author
            ],
            specialField: SubjectSpecialFields.Contacts
        },
        {
            roleCodes: [
                CustomerRole.Declarant,
                CustomerRole.Author
            ],
            specialField: SubjectSpecialFields.Country
        },
        {
            roleCodes: [
                CustomerRole.Confidant,
                CustomerRole.PatentAttorney
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

    constructor(
        private fb: FormBuilder,
        private customerService: CustomerService,
        private configService: ConfigService,
        private postKzService: PostKzService,
        private dialogRef: MatDialogRef<SubjectFormDialogComponent>,
        private snackbarHelper: SnackBarHelper,
        private dictionaryService: DictionaryService,
        private dialog: MatDialog,
        private detectorRef: ChangeDetectorRef,
        @Inject(MAT_DIALOG_DATA) private data: any
    ) {
        this.subject = data.subject;
        this.protectionDocTypeCode = data.protectionDocTypeCode;
        this.hasProxy = data.hasProxy;
        this.roleCodes = data.roleCodes || [];
        this.mode = data.mode;
        this.ownerType = data.ownerType;
        this.ownerId = data.ownerId;
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
            'shortAddress',
            'address',
            'addressKz',
            'addressEn',
            'beneficiaryTypeId'
        ];
        this.toggleEditMode(true);
    }

    ngOnInit() {
        this.initSelectOptions();
        this.subscribeXinInput();
        this.subscribeAddressInput();
    }

    ngAfterViewInit(): void {
        this.formGroup.reset(this.data.subject);
        this.detectorRef.detectChanges();
    }

    public ngOnDestroy(): void {
        this.faxes = [];
        this.mobilePhones = [];
        this.emails = [];
        this.phones = [];
        this.onDestroy.next();
    }

    onSubmit() {
        if (this.formGroup.invalid) {
            return;
        }
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
        value.mode = value.customerId ? this.mode : SubjectFormMode.InsertOnAttach;
        value.ownerId = this.subject.ownerId;
        value.contactInfos = [];
        this.setContactInfoData(this.faxes, this.faxesGroup, 'fax');
        this.setContactInfoData(this.phones, this.phonesGroup, 'phone');
        this.setContactInfoData(
            this.mobilePhones,
            this.mobilePhonesGroup,
            'mobilePhone'
        );
        this.setContactInfoData(this.emails, this.emailsGroup, 'email');
        this.faxes.forEach(f => value.contactInfos.push(f));
        this.emails.forEach(e => value.contactInfos.push(e));
        this.mobilePhones.forEach(mp => value.contactInfos.push(mp));
        this.phones.forEach(p => value.contactInfos.push(p));
        this.dialogRef.close(value);
    }

    onCancel() {
        this.formGroup.reset();
        this.dialogRef.close();
    }

    onAddressSelect(address: any) {
        if (address === this.postKzService.unreachableText) {
            return;
        }

        const apartment = this.formGroup.get('apartment').value;
        this.formGroup
            .get('shortAddress')
            .setValue(concatAddresseeAddress(address, apartment));
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

    isPatentAttorney(): boolean {
        this.isFound();
        if (
            this.mode !== SubjectFormMode.Insert &&
            this.formGroup.get('roleId').value
        ) {
            const result = this.formGroup.get('roleId').value === 4;
            return result;
        }
        return false;
    }

    isTrusted() {
        if (
            this.mode !== SubjectFormMode.Insert &&
            this.formGroup.get('roleId').value
        ) {
            return this.formGroup.get('roleId').value === 6;
        }
        return false;
    }

    isJur(): boolean {
        if (
            (this.mode === SubjectFormMode.Insert ||
                this.mode === SubjectFormMode.Edit) &&
            this.formGroup.get('typeId').value
        ) {
            return [775, 19].includes(this.formGroup.get('typeId').value);
        }
        return false;
    }

    isFound() {
        if (this.formGroup.get('customerId').value) {
            this.formGroup.get('nameRu').disable();
        } else {
            this.formGroup.get('nameRu').enable();
        }
    }

    onSearchCustomersClick() {
        this.openDialogSearchCustomer();
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

    onCountryChange(value: any) {
        this.formGroup.get('countryCode').setValue(value.code);
        if (value.code === 'KZ') {
            this.isResident = true;
        } else {
            this.isResident = false;
        }
    }

    // workaround для изменения валидатора automoplete
    isCountryValid(): boolean {
        if (!this.formGroup) {
            return false;
        }
        return !this.isCountryRequired || !!this.formGroup.get('countryCode').value;
    }

    getResidencyValue(): string {
        return this.isNotResident()
            ? this.residencyStatuses.find(rs => rs.code === 'nrs').code
            : this.residencyStatuses.find(rs => rs.code === 'res').code;
    }

    private addNewEmail() {
        const emailType = this.dicContactInfotypes.filter(t => t.code === 'email');
        const email = new ContactInfoDto();
        if (emailType.length > 0) {
            email.typeId = emailType[0].id;
            this.emails.push(email);
        }
    }

    isNotResident(): boolean {
        return this.formGroup.get('countryCode').value !== 'KZ';
    }

    onAddressKeyPress(event: KeyboardEvent) {
        if (event.code === 'Space') {
            event.stopPropagation();
        }
    }

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

    private setContactInfoData(
        infos: ContactInfoDto[],
        formGroup: FormGroup,
        name: string
    ) {
        let i = 0;
        Object.values(formGroup.getRawValue())
            .filter(value => !!value && infos.length > 0)
            .forEach(value => {
                infos[i].info = value.toString();
                i++;
            });
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
                        this.mode = SubjectFormMode.InsertOnAttach;
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
                            shortAddress: '',
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
                            region: '',
                            city: '',
                            street: ''
                        });
                        this.toggleEditMode(true);
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
                                shortAddress: result.shortAddress,
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
                                region: result.region,
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
                                shortAddress: result.shortAddress,
                                jurRegNumber: result.jurRegNumber,
                                powerAttorneyFullNum: result.powerAttorneyFullNum,
                                dateBegin: result.dateBegin,
                                dateEnd: result.dateEnd,
                                isNotResident: result.isNotResident,
                                isNotMention: result.isNotMention,
                                beneficiaryTypeId: result.beneficiaryTypeId,
                                republic: result.republic,
                                oblast: result.oblast,
                                region: result.region,
                                city: result.city,
                                street: result.street
                            });
                        }

                        this.initializeContactInfos(result);
                    }
                }
            });
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
            shortAddress: [{ value: '', disabled: true }],
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
            region: [{ value: '', disabled: true }],
            city: [{ value: '', disabled: true }],
            street: [{ value: '', disabled: true }]
        });
    }

    /**
     * Инициализация полей из справочников.
     *
     * @private
     * @memberof SubjectsFormComponent
     */
    private initSelectOptions() {
        Observable.combineLatest(
            this.dictionaryService.getSelectOptions(DictionaryType.DicCustomerRole),
            this.dictionaryService.getSelectOptions(DictionaryType.DicCustomerType),
            this.dictionaryService.getBaseDictionary(DictionaryType.DicContactInfoType),
            this.dictionaryService.getSelectOptions(DictionaryType.DicCountry),
            this.dictionaryService.getBaseDictionary(DictionaryType.DicBeneficiaryType)
        )
            .takeUntil(this.onDestroy)
            .subscribe(
                ([roles, types, contactInfoTypes, countries, beneficiaries]) => {

                    this.dicCustomerRoles = roles
                        .filter(role => this.roleCodes.length === 0 || this.roleCodes.includes(role.code))
                        .filter(role => authorlessRequestTypeCodes.includes(this.protectionDocTypeCode) ? role.code !== '2'  : true )
                        .filter(role => role.code !== CustomerRole.Owner);

                    if (this.dicCustomerRoles.length === 1) {
                        this.formGroup
                            .get('roleId')
                            .patchValue(this.dicCustomerRoles[0].id);
                    }

                    this.dicCustomerTypes = types.filter(ct => availableCustomerTypeCodes.includes(ct.code));

                    this.dicContactInfotypes = contactInfoTypes;
                    this.initializeContactInfos(this.subject);

                    this.dicCountries = countries;
                    this.dicCountries.forEach(c => c.nameRu = c.code);

                    this.dicBeneficiaryTypes = beneficiaries;
                    this.dicBeneficiaryTypes.unshift(this.dictionaryService.emptyBaseDictionary);

                    const country = this.dicCountries.find(c => c.id === this.subject.countryId);
                    this.setCountryControls(country);
                }
            );
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
                                if (
                                    this.mode === SubjectFormMode.Attach &&
                                    !this.isPatentAttorney()
                                ) {
                                    this.formGroup.get('xin').setErrors({ xin: false });
                                    this.formGroup.get('xin').updateValueAndValidity();
                                    this.mode = SubjectFormMode.InsertOnAttach;
                                    this.snackbarHelper.success(
                                        err.message +
                                            '\nПри сохранении будет создан новый контрагент'
                                    );
                                }
                                if (
                                    this.mode === SubjectFormMode.Attach &&
                                    this.isPatentAttorney()
                                ) {
                                    this.snackbarHelper.error(
                                        'Данного пользователя нет в справочнике патентных поверенных. Просьба обратиться в Администратору системы.'
                                    );
                                }
                            } else {
                                if (
                                    this.mode !== SubjectFormMode.Insert &&
                                    this.mode !== SubjectFormMode.Edit
                                ) {
                                    this.mode = SubjectFormMode.Attach;
                                }
                                this.snackbarHelper.error(err.message);
                            }
                        }
                    );
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
        this.formGroup.get('region').setValue(customerInfo.region);
        this.formGroup.get('city').setValue(customerInfo.city);
        this.formGroup.get('street').setValue(customerInfo.street);
        this.selectedAddress = customerInfo.address;
        this.formGroup.get('typeId').setValue(customerInfo.typeId);
        this.formGroup.get('isBeneficiary').setValue(customerInfo.isBeneficiary);
        this.formGroup.get('jurRegNumber').setValue(customerInfo.jurRegNumber);
        this.formGroup
            .get('powerAttorneyFullNum')
            .setValue(customerInfo.powerAttorneyFullNum);
        this.formGroup.get('isNotResident').setValue(customerInfo.isNotResident);
        this.formGroup.get('isNotMention').setValue(customerInfo.isNotMention);
        this.formGroup.get('apartment').setValue(customerInfo.apartment);
        this.formGroup.get('shortAddress').setValue(customerInfo.address);
        this.formGroup
            .get('beneficiaryTypeId')
            .setValue(customerInfo.beneficiaryTypeId);
        this.formGroup.markAsDirty();
        if (
            this.mode !== SubjectFormMode.Insert &&
            this.mode !== SubjectFormMode.Edit
        ) {
            this.mode = SubjectFormMode.Attach;
        }
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
        this.formGroup.get('region').setValue('');
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
        this.formGroup.get('shortAddress').setValue('');
        this.formGroup.get('beneficiaryTypeId').setValue('');
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

        if (this.mode === SubjectFormMode.Edit) {
            this.formGroup.get('roleId').disable();
        }
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

    isVisibleBeneficiary(): boolean {
        return (
            (this.mode === 1 || this.mode === 2 || this.mode === 3) &&
            !(
                this.pdTypeTMCodes.includes(this.protectionDocTypeCode) ||
                this.pdTypeCOOCodes.includes(this.protectionDocTypeCode)
            ) &&
            this.isSelectedCustomerRole(CustomerRole.Declarant)
        );
    }
    isDisabledDictionaryBeneficiary(): boolean {
        return !this.formGroup.controls.isBeneficiary.value;
    }
    isVisibleResident(): boolean {
        return (
            this.isSelectedCustomerRole(CustomerRole.Declarant) ||
            this.isSelectedCustomerRole(CustomerRole.Author)
        );
    }

    isVisibleNotMention(): boolean {
        return this.isSelectedCustomerRole(CustomerRole.Author);
    }

    isDisabledButtonSave() {
        const isInvalid = this.formGroup.invalid;
        const isPristine = this.formGroup.pristine;
        const isCountryInvalid = !this.isCountryValid();
        const isNotHasAnyAddress = !this.isHasAnyAddress();
        const isNotHasAnyName = !(
            this.formGroup.controls.nameRu.value ||
            this.formGroup.controls.nameKz.value ||
            this.formGroup.controls.nameEn.value
        );
        const isAllowedEmptyXinWithNotResident = (
            !this.formGroup.controls.roleId.value &&
            this.mode === SubjectFormMode.Insert &&
            this.isNotResident &&
            !this.formGroup.controls.xin.value
        );

        if (isAllowedEmptyXinWithNotResident) {
            return (
                isInvalid ||
                isPristine ||
                isCountryInvalid ||
                isNotHasAnyAddress ||
                isNotHasAnyName
            );
        } else {
            return (
                isInvalid ||
                isPristine ||
                isCountryInvalid ||
                isNotHasAnyAddress ||
                isNotHasAnyName ||
                (
                    (
                        !this.formGroup.controls.roleId.value &&
                        (
                            this.mode !== SubjectFormMode.Insert ||
                            (
                                this.isNotResident() &&
                                !this.formGroup.controls.xin.value
                            )
                        )
                    ) ||
                    !this.formGroup.controls.typeId.value ||
                    (
                        this.isSelectedCustomerRole(CustomerRole.PatentAttorney) &&
                        !this.formGroup.controls.powerAttorneyFullNum
                    )
                )
            );
        }
    }

    selectionRoleChange(roleId: number) {
        if (
            this.isSelectedCustomerRole(CustomerRole.CorrespondingRecipient)
        ) {
            if (this.ownerType && this.ownerId) {
                this.customerService
                    .getAddresseeByOwnerId(this.ownerType, this.ownerId)
                    .takeUntil(this.onDestroy)
                    .subscribe((data: CustomerShortInfo) => {
                        if (data) {
                            this.initializeCustomerInfo(data);
                        }
                    });
            }
        }
        let defaultCustomerTypeId: number;
        if (
            this.isSelectedCustomerRole(CustomerRole.Declarant)
        ) {
            const country = this.dicCountries.find(
                c => c.code === this.dicCountryKZCode
            );
            this.setCountryControls(country);
            const jur = this.dicCustomerTypes.find(c => c.code === '1');
            if (jur) {
                defaultCustomerTypeId = jur.id;
            }
            this.isCountryRequired = true;
        } else {
            this.isCountryRequired = false;
            const fiz = this.dicCustomerTypes.find(c => c.code === '2');
            if (fiz) {
                defaultCustomerTypeId = fiz.id;
            }
        }
        this.formGroup.get('typeId').setValue(defaultCustomerTypeId);
        if (
            this.isSelectedCustomerRole(CustomerRole.PatentAttorney)
        ) {
            this.formGroup.get('customerId').setValidators(Validators.required);
            this.formGroup.get('customerId').updateValueAndValidity();
            this.formGroup.disable();
            this.formGroup.get('roleId').enable();
            this.formGroup.get('address').enable();
            this.formGroup.get('addressKz').enable();
            this.formGroup.get('addressEn').enable();
        } else {
            this.formGroup.get('customerId').clearValidators();
            this.formGroup.get('customerId').updateValueAndValidity();
            this.toggleEditMode(true);
        }

        if (
            this.isSelectedCustomerRole(CustomerRole.PatentAttorney) ||
            this.isSelectedCustomerRole(CustomerRole.Confidant)
        ) {
            const country = this.dicCountries.find(
                c => c.code === this.dicCountryKZCode
            );
            this.setCountryControls(country);
            this.formGroup.controls.countryId.disable();
            this.formGroup.get('dateBegin').enable();
            this.formGroup.get('dateEnd').enable();
            if (this.hasProxy) {
                this.formGroup.get('dateBegin').setValidators(Validators.required);
                this.formGroup.get('dateEnd').setValidators(Validators.required);
                this.formGroup.get('dateBegin').updateValueAndValidity();
                this.formGroup.get('dateEnd').updateValueAndValidity();
            }
        } else {
            this.formGroup.get('dateBegin').clearValidators();
            this.formGroup.get('dateEnd').clearValidators();
            this.formGroup.get('dateBegin').updateValueAndValidity();
            this.formGroup.get('dateEnd').updateValueAndValidity();
        }
    }
    isHasAnyAddress() {
        return (
            this.formGroup.controls.address.value ||
            this.formGroup.controls.addressKz.value ||
            this.formGroup.controls.addressEn.value
        );
    }

    isFieldActive(field: SubjectSpecialFields): boolean {
        const currentRoleCode = this.getSelectedRole();
        return this.fieldsConfig.some(
            fc => fc.roleCodes.includes(<CustomerRole>currentRoleCode) && fc.specialField === field
        );
    }

    private getSelectedRole(): string {
        const roleId = this.formGroup.get('roleId').value;
        if (isNaN(roleId) === false && roleId > 0 && this.dicCustomerRoles) {
            return this.dicCustomerRoles.find(r => r.id === roleId).code;
        }
        return '';
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

    private doesAddresseeHavePhone(): boolean {
        if (
            this.isSelectedCustomerRole(CustomerRole.CorrespondingRecipient)
        ) {
            return this.phones.length > 0;
        }
        return true;
    }
}
