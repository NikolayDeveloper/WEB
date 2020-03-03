import { Component, OnInit, OnDestroy, Inject } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Observable, Subject } from 'rxjs';
import { startWith, map } from 'rxjs/operators';

import { DictionaryService } from 'app/shared/services/dictionary.service';
import { CustomerService } from 'app/shared/services/customer.service';
import { ConfigService } from 'app/core';
import { DictionaryType } from 'app/shared/services/models/dictionary-type.enum';
import { SubjectFormMode } from 'app/subjects/enums/subject-form-mode.enum';
import { SnackBarHelper } from 'app/core/snack-bar-helper.service';

import { SubjectsSearchDialogComponent } from 'app/subjects/components/subjects-search-dialog/subjects-search-dialog.component';

import { Fields } from './fields.enum';
import { ProtectionDocTypes } from 'app/shared/enums/protection-doc-types.enum';

import { CustomerRole } from '../../enums/customer-role.enum';
import { CustomerType } from '../../enums/customer-type.enum';
import { CustomerShortInfo } from 'app/shared/services/models/customer-short-info';
import { OwnerType } from 'app/shared/services/models/owner-type.enum';
import { SubjectDto, ContactInfoDto } from 'app/subjects/models/subject.model';
import { BaseDictionary } from 'app/shared/services/models/base-dictionary';
import { SelectOption } from 'app/shared/services/models/select-option';

enum ResidentStatus {
  True = 'res',
  False = 'nrs'
}

class FieldState {
  public enabled: boolean;
  public validators: Function[];

  constructor() {
    this.enabled = false;
    this.validators = null;
  }

  setEnabled(enabled = true) {
    this.enabled = enabled;

    return this;
  }

  setValidators(validators = null) {
    this.validators = validators;

    return this;
  }
}

@Component({
  selector: 'app-subject-create-dialog',
  templateUrl: './subject-create-dialog.component.html',
  styleUrls: ['./subject-create-dialog.component.scss']
})
export class SubjectCreateDialogComponent implements OnInit, OnDestroy {
  formGroup: FormGroup;

  customerTypes: SelectOption[];
  customerRoles: SelectOption[];
  countryCodes: SelectOption[];
  contactInfoTypes: BaseDictionary[];
  // beneficiaryTypes: BaseDictionary[];
  filteredCountryCodes: Observable<SelectOption[]>;
  availableCategories: Object[];

  private onDestroy = new Subject();

  defaultValues = {
    [CustomerRole.Declarant]: {
      [Fields.CustomerType]: CustomerType.LegalEntity,
      [Fields.Resident]: ResidentStatus.True,
      [Fields.CountryCode]: 'KZ',
      [Fields.Country]: 'Республика Казахстан'
    },
    [CustomerRole.CorrespondingRecipient]: {
      [Fields.CustomerType]: CustomerType.Individual,
      [Fields.Resident]: ResidentStatus.True,
      [Fields.CountryCode]: 'KZ',
      [Fields.Country]: 'Республика Казахстан'
    },
    [CustomerRole.PatentAttorney]: {
      [Fields.CustomerType]: CustomerType.Individual,
      [Fields.Resident]: ResidentStatus.True,
      [Fields.CountryCode]: 'KZ',
      [Fields.Country]: 'Республика Казахстан'
    },
    [CustomerRole.Confidant]: {
      [Fields.CustomerType]: CustomerType.Individual,
      [Fields.Resident]: ResidentStatus.True,
      [Fields.CountryCode]: 'KZ',
      [Fields.Country]: 'Республика Казахстан'
    },
    [CustomerRole.Author]: {
      [Fields.CustomerType]: CustomerType.Individual,
      [Fields.Resident]: ResidentStatus.True,
      [Fields.CountryCode]: 'KZ',
      [Fields.Country]: 'Республика Казахстан'
    },
    [CustomerRole.Owner]: {
      [Fields.Resident]: ResidentStatus.True,
      [Fields.CountryCode]: 'KZ',
      [Fields.Country]: 'Республика Казахстан'
    },
    [CustomerRole.DeclarantAddress]: {
      [Fields.Resident]: ResidentStatus.True,
      [Fields.CountryCode]: 'KZ',
      [Fields.Country]: 'Республика Казахстан'
    },
    [CustomerRole.SideOne]: {
      [Fields.CustomerType]: CustomerType.LegalEntity,
      [Fields.Resident]: ResidentStatus.True,
      [Fields.CountryCode]: 'KZ',
      [Fields.Country]: 'Республика Казахстан'
    },
    [CustomerRole.SideTwo]: {
      [Fields.CustomerType]: CustomerType.LegalEntity,
      [Fields.Resident]: ResidentStatus.True,
      [Fields.CountryCode]: 'KZ',
      [Fields.Country]: 'Республика Казахстан'
    },
  };
  customerType = CustomerType;
  modes = SubjectFormMode;
  fields = Fields;
  residentStatus = ResidentStatus;
  // TODO Для электронной подачи сделать поле «Электронной почты» обязательнным.
  // TODO Для `CustomerRole.PatentAttorney` сделать логику на Backend'е.
  forms = {
    [CustomerRole.Declarant]: {
      [Fields.IIN]: new FieldState().setEnabled(),
      [Fields.NameRu]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.NameEn]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.NameKz]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.CustomerType]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Resident]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.CountryCode]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Country]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Region]: new FieldState().setEnabled(),
      [Fields.City]: new FieldState().setEnabled(), // Убран Validators.required по задаче NIIS-905
      [Fields.District]: new FieldState().setEnabled(),
      [Fields.Street]: new FieldState().setEnabled(),
      [Fields.AddressRu]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.AddressEn]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.AddressKz]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Category]: new FieldState().setEnabled(),
      [Fields.Tel]: new FieldState().setEnabled(),
      [Fields.MobileTel]: new FieldState().setEnabled(),
      [Fields.Fax]: new FieldState().setEnabled(),
      [Fields.Email]: new FieldState().setEnabled()
    },
    [CustomerRole.CorrespondingRecipient]: {
      [Fields.IIN]: new FieldState().setEnabled(),
      [Fields.NameRu]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.NameEn]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.NameKz]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.CustomerType]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Resident]: new FieldState().setEnabled(),
      [Fields.CountryCode]: new FieldState().setEnabled(),
      [Fields.Country]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Region]: new FieldState().setEnabled(),
      [Fields.City]: new FieldState().setEnabled(), // Убран Validators.required по задаче NIIS-905
      [Fields.District]: new FieldState().setEnabled(),
      [Fields.Street]: new FieldState().setEnabled(),
      [Fields.AddressRu]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.AddressEn]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.AddressKz]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Tel]: new FieldState().setEnabled(),
      [Fields.MobileTel]: new FieldState().setEnabled(),
      [Fields.Fax]: new FieldState().setEnabled(),
      [Fields.Email]: new FieldState().setEnabled(),
      [Fields.OrganizationNameRu]: new FieldState().setEnabled(),
      [Fields.OrganizationNameKz]: new FieldState().setEnabled(),
      [Fields.DirectorName]: new FieldState().setEnabled()
    },
    [CustomerRole.PatentAttorney]: {
      [Fields.IIN]: new FieldState(),
      [Fields.NameRu]: new FieldState().setValidators(Validators.required),
      [Fields.NameEn]: new FieldState().setValidators(Validators.required),
      [Fields.NameKz]: new FieldState().setValidators(Validators.required),
      [Fields.CustomerType]: new FieldState().setValidators(Validators.required),
      [Fields.RegisterNumber]: new FieldState().setValidators(Validators.required),
      [Fields.Resident]: new FieldState(),
      [Fields.Status]: new FieldState().setValidators(Validators.required),
      [Fields.BeginDate]: new FieldState().setEnabled(),
      [Fields.EndDate]: new FieldState().setEnabled(),
      [Fields.CountryCode]: new FieldState(),
      [Fields.Country]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Region]: new FieldState().setEnabled(),
      [Fields.City]: new FieldState().setEnabled(), // Убран Validators.required по задаче NIIS-905
      [Fields.District]: new FieldState().setEnabled(),
      [Fields.Street]: new FieldState().setEnabled(),
      [Fields.AddressRu]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.AddressEn]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.AddressKz]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Tel]: new FieldState(),
      [Fields.MobileTel]: new FieldState(),
      [Fields.Fax]: new FieldState(),
      [Fields.Email]: new FieldState()
    },
    [CustomerRole.Confidant]: {
      [Fields.IIN]: new FieldState().setEnabled(),
      [Fields.NameRu]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.NameEn]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.NameKz]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.CustomerType]: new FieldState().setValidators(Validators.required),
      [Fields.Resident]: new FieldState().setEnabled(),
      [Fields.BeginDate]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.EndDate]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.CountryCode]: new FieldState().setEnabled(),
      [Fields.Country]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Region]: new FieldState().setEnabled(),
      [Fields.City]: new FieldState().setEnabled(), // Убран Validators.required по задаче NIIS-905
      [Fields.District]: new FieldState().setEnabled(),
      [Fields.Street]: new FieldState().setEnabled(),
      [Fields.AddressRu]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.AddressEn]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.AddressKz]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Tel]: new FieldState().setEnabled(),
      [Fields.MobileTel]: new FieldState().setEnabled(),
      [Fields.Fax]: new FieldState().setEnabled(),
      [Fields.Email]: new FieldState().setEnabled()
    },
    [CustomerRole.Author]: {
      [Fields.IIN]: new FieldState().setEnabled(),
      [Fields.NameRu]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.NameEn]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.NameKz]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.CustomerType]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Resident]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.CountryCode]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Country]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Region]: new FieldState().setEnabled(),
      [Fields.City]: new FieldState().setEnabled(), // Убран Validators.required по задаче NIIS-905
      [Fields.District]: new FieldState().setEnabled(),
      [Fields.Street]: new FieldState().setEnabled(),
      [Fields.AddressRu]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.AddressEn]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.AddressKz]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Tel]: new FieldState().setEnabled(),
      [Fields.MobileTel]: new FieldState().setEnabled(),
      [Fields.Fax]: new FieldState().setEnabled(),
      [Fields.Email]: new FieldState().setEnabled(),
      [Fields.NotNotify]: new FieldState().setEnabled()
    },
    [CustomerRole.Owner]: {
      [Fields.IIN]: new FieldState().setEnabled(),
      [Fields.NameRu]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.NameEn]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.NameKz]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.CustomerType]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Resident]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.CountryCode]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Country]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Region]: new FieldState().setEnabled(),
      [Fields.City]: new FieldState().setEnabled(), // Убран Validators.required по задаче NIIS-905
      [Fields.District]: new FieldState().setEnabled(),
      [Fields.Street]: new FieldState().setEnabled(),
      [Fields.AddressRu]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.AddressEn]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.AddressKz]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Tel]: new FieldState().setEnabled(),
      [Fields.MobileTel]: new FieldState().setEnabled(),
      [Fields.Fax]: new FieldState().setEnabled(),
      [Fields.Email]: new FieldState().setEnabled()
    },
    [CustomerRole.DeclarantAddress]: {
      [Fields.IIN]: new FieldState().setEnabled(),
      [Fields.NameRu]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.NameEn]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.NameKz]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.CustomerType]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Resident]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.CountryCode]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Country]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Region]: new FieldState().setEnabled(),
      [Fields.City]: new FieldState().setEnabled(), // Убран Validators.required по задаче NIIS-905
      [Fields.District]: new FieldState().setEnabled(),
      [Fields.Street]: new FieldState().setEnabled(),
      [Fields.AddressRu]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.AddressEn]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.AddressKz]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Category]: new FieldState().setEnabled(),
      [Fields.Tel]: new FieldState().setEnabled(),
      [Fields.MobileTel]: new FieldState().setEnabled(),
      [Fields.Fax]: new FieldState().setEnabled(),
      [Fields.Email]: new FieldState().setEnabled()
    },
    [CustomerRole.SideOne]: {
      [Fields.IIN]: new FieldState().setEnabled(),
      [Fields.NameRu]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.NameEn]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.NameKz]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.CustomerType]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Resident]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.CountryCode]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Country]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Region]: new FieldState().setEnabled(),
      [Fields.City]: new FieldState().setEnabled(), // Убран Validators.required по задаче NIIS-905
      [Fields.District]: new FieldState().setEnabled(),
      [Fields.Street]: new FieldState().setEnabled(),
      [Fields.AddressRu]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.AddressEn]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.AddressKz]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Category]: new FieldState().setEnabled(),
      [Fields.Tel]: new FieldState().setEnabled(),
      [Fields.MobileTel]: new FieldState().setEnabled(),
      [Fields.Fax]: new FieldState().setEnabled(),
      [Fields.Email]: new FieldState().setEnabled()
    },
    [CustomerRole.SideTwo]: {
      [Fields.IIN]: new FieldState().setEnabled(),
      [Fields.NameRu]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.NameEn]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.NameKz]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.CustomerType]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Resident]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.CountryCode]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Country]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Region]: new FieldState().setEnabled(),
      [Fields.City]: new FieldState().setEnabled(), // Убран Validators.required по задаче NIIS-905
      [Fields.District]: new FieldState().setEnabled(),
      [Fields.Street]: new FieldState().setEnabled(),
      [Fields.AddressRu]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.AddressEn]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.AddressKz]: new FieldState().setEnabled().setValidators(Validators.required),
      [Fields.Category]: new FieldState().setEnabled(),
      [Fields.Tel]: new FieldState().setEnabled(),
      [Fields.MobileTel]: new FieldState().setEnabled(),
      [Fields.Fax]: new FieldState().setEnabled(),
      [Fields.Email]: new FieldState().setEnabled()
    }
  };
  additionalAvailableRules = {
    [Fields.Category]: () => {
      return [
        ProtectionDocTypes.Invention,
        ProtectionDocTypes.IndustrialDesign,
        ProtectionDocTypes.UsefulModel,
        ProtectionDocTypes.SelectionAchievement
      ].includes(this.protectionDocTypeCode);
    }
  };
  formTypes = [
    {
      name: 'Заявитель',
      code: CustomerRole.Declarant,
      available: this.data.roleCodes.includes(CustomerRole.Declarant)
    },
    {
      name: 'Адресат для переписки',
      code: CustomerRole.CorrespondingRecipient,
      available: this.data.roleCodes.includes(CustomerRole.CorrespondingRecipient)
    },
    {
      name: 'Патентный поверенный',
      code: CustomerRole.PatentAttorney,
      available: this.data.roleCodes.includes(CustomerRole.PatentAttorney)
    },
    {
      name: 'Доверенное лицо',
      code: CustomerRole.Confidant,
      available: this.data.roleCodes.includes(CustomerRole.Confidant)
    },
    {
      name: 'Автор',
      code: CustomerRole.Author,
      available: this.data.roleCodes.includes(CustomerRole.Author)
    },
    {
      name: 'Владелец',
      code: CustomerRole.Owner,
      available: this.data.roleCodes.includes(CustomerRole.Owner)
    },
    {
      name: 'Адрес заявителя',
      code: CustomerRole.DeclarantAddress,
      available: false
    },
    {
      name: 'Сторона 1',
      code: CustomerRole.SideOne,
      available: this.data.roleCodes.includes(CustomerRole.SideOne)
    },
    {
      name: 'Сторона 2',
      code: CustomerRole.SideTwo,
      available: this.data.roleCodes.includes(CustomerRole.SideTwo)
    }
  ];
  deferredTasks = [];

  subject: SubjectDto;
  hideRole: boolean = false;
  protectionDocTypeCode: ProtectionDocTypes;
  roleCodes: CustomerRole[];
  mode: SubjectFormMode;
  hasProxy: boolean;
  ownerId: number;
  ownerType: OwnerType;

  isSearchOpened: boolean = false;
  isPatchValue: boolean = false;
  forceCanSubmit: boolean = false;
  private previousValue: string = null;

  constructor(
    private formBuilder: FormBuilder,
    private dictionaryService: DictionaryService,
    private customerService: CustomerService,
    private configService: ConfigService,
    private snackbarHelper: SnackBarHelper,
    private dialog: MatDialog,
    private reference: MatDialogRef<SubjectCreateDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private data: any
  ) {
    this.buildForm();
    this.initSelectOptions();

    for (let key of Object.keys(this.data)) {
      this[key] = this.data[key];
    }
  }

  ngOnInit(): void {
    this.formGroup.get(Fields.Role).valueChanges
      .subscribe(() => {
        const role = this.formGroup.get(Fields.Role).value;
        const states = this.forms[role];
        const defaultValues = this.defaultValues[role];

        for (let key of Object.keys(this.formGroup.controls)) {
          const control = this.formGroup.get(key);

          if (states.hasOwnProperty(key)) {
            const state = states[key];

            if (state) {
              if (state.validators && state.validators instanceof Function) {
                control.setValidators(state.validators);
              } else {
                control.clearValidators();
              }

              if (state.enabled) {
                control.enable();
              } else {
                control.disable();
              }
            } else {
              control.clearValidators();
              control.disable();
            }

            if (defaultValues.hasOwnProperty(key)) {
              control.reset(defaultValues[key]);
            } else {
              control.reset();
            }
          }
        }

        this.markFields();

        if (this.mode === SubjectFormMode.Insert && !this.isSearchOpened) {
          this.openSearchCustomerDialog();

          this.isSearchOpened = true;
        }
      });

    this.formGroup.get(Fields.IIN).valueChanges
      .subscribe(() => {
        const value = this.formGroup.get(Fields.IIN).value;

        if (value) {
          const parsedValue = value.replace(/[^0-9]*/g, '');

          if (value !== parsedValue) {
            this.formGroup.get(Fields.IIN).setValue(parsedValue || null);
          }
        }
      });

    this.formGroup.get(Fields.IIN).valueChanges
      .takeUntil(this.onDestroy)
      .debounceTime(this.configService.debounceTime)
      .subscribe(() => {
        const value = this.formGroup.get(Fields.IIN).value;

        if (value && value !== this.previousValue) {
          if (value.toString().length === 12) {
            this.customerService.getByXin(value.toString(), this.isPatentAttorney())
              .takeUntil(this.onDestroy)
              .subscribe((customerShortInfo: CustomerShortInfo) => {
                if (!customerShortInfo) {
                  this.snackbarHelper.success('Пользователь с таким БИН/ИИН не найден');

                  return;
                }

                customerShortInfo.ownerId = this.subject.ownerId;
                this.initFormData(customerShortInfo);

                this.mode = customerShortInfo.id ? SubjectFormMode.Attach : SubjectFormMode.Insert;
                this.previousValue = value.toString();
              }, (error) => {
                if (error.status === 404) {
                  if (this.mode === SubjectFormMode.Attach) {
                    if (!this.isPatentAttorney()) {
                      this.mode = SubjectFormMode.InsertOnAttach;

                      this.snackbarHelper.success(`${error.message}\nПри сохранении будет создан новый контрагент`);
                    } else {
                      this.snackbarHelper.success('Данного пользователя нет в справочнике патентных поверенных');
                    }
                  }
                } else {
                  if (this.mode !== SubjectFormMode.Insert && this.mode !== SubjectFormMode.Edit) {
                    this.mode = SubjectFormMode.Attach;
                  }

                  this.snackbarHelper.error(error.message);
                }
              });
          }
        }
      });

    this.formGroup.get(Fields.CustomerType).valueChanges
      .subscribe(() => {
        const role = this.formGroup.get(Fields.Role).value;
        const customerType = this.formGroup.get(Fields.CustomerType).value;

        if (role === CustomerRole.CorrespondingRecipient) {
          for (let key of [Fields.OrganizationNameRu, Fields.OrganizationNameKz, Fields.DirectorName]) {
            const control = this.formGroup.get(key);

            if (customerType === CustomerType.LegalEntity) {
              control.setValidators(Validators.required);
            } else {
              control.clearValidators();
            }
            control.enable();
          }
        }
      });

    this.formGroup.get(Fields.Resident).valueChanges
      .subscribe(() => {
        const role = this.formGroup.get(Fields.Role).value;
        if (!role) {
          return;
        }

        if (!this.isPatchValue) {
          this.updateCountryCodeField();
          this.updateIINField();
        }
      });

    this.formGroup.get(Fields.CountryCode).valueChanges
      .subscribe(() => {
        if (!this.isPatchValue) {
          this.updateResidentField();
          this.updateIINField();

          if (this.countryCodes) {
            const countryCode = this.formGroup.get(Fields.CountryCode).value;
            const country = this.countryCodes.find(entry => entry.code === countryCode);

            if (country) {
              this.formGroup.get(Fields.Country).setValue(country.nameRu);
            }
          }
        }
      });

    const someRequired = (fields: Fields[], count: number): void => {
      for (let field of fields) {
        this.formGroup.get(field).valueChanges
          .subscribe(() => {
            const controls = {};
            for (let field of fields) {
              controls[field] = this.formGroup.get(field);
            }
            const emptyControls = Object.entries(controls)
              .filter(([key, control]) => !(control as FormControl).value)
              .map(([key, value]) => key);
            const notEmptyControlsCount = fields.length - emptyControls.length;

            const role = this.formGroup.get(Fields.Role).value;
            if (role) {
              const states = this.forms[role];

              for (let key of emptyControls) {
                if (notEmptyControlsCount >= count) {
                  controls[key].clearValidators();
                } else {
                  controls[key].setValidators(states[key].validators);
                }

                controls[key].updateValueAndValidity({
                  emitEvent: false
                });
              }
            }
          });
      }
    };

    someRequired([Fields.NameRu, Fields.NameEn, Fields.NameKz], 1);
    someRequired([Fields.AddressRu, Fields.AddressEn, Fields.AddressKz], 1);

    this.filteredCountryCodes = this.formGroup.get(Fields.CountryCode).valueChanges
      .pipe(
        startWith(''),
        map(value => value ? this.countryCodes.filter(countryCode => {
          if (countryCode.code) {
            return countryCode.code.toLowerCase().includes(value.toLowerCase());
          } else {
            return false;
          }
        }) : this.countryCodes ? this.countryCodes.slice() : [])
      );

    if (this.mode === SubjectFormMode.Edit) {
      for (let key of Object.keys(this.formGroup.controls)) {
        this.formGroup.get(key).markAsDirty();
      }
    }

    if (this.mode === SubjectFormMode.Insert) {
      if (this.subject.roleCode) {
        this.formGroup.get(Fields.Role).setValue(this.subject.roleCode);
      }
    }

    if (this.mode === SubjectFormMode.Attach || this.mode === SubjectFormMode.Edit) {
      this.initFormData(this.subject as any);
    }
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  openSearchCustomerDialog() {
    const dialog = this.dialog.open(SubjectsSearchDialogComponent, {
      data: {
        isPatentAttorney: this.isPatentAttorney()
      },
      width: '1200px'
    });

    dialog.afterClosed()
      .takeUntil(this.onDestroy)
      .subscribe((data) => {
        if (data) {
          data.ownerId = this.subject.ownerId;

          this.initFormData(data);

          this.mode = data.id ? SubjectFormMode.Attach : SubjectFormMode.Insert;
        }
      });
  }

  isAvailable(name: string): boolean {
    const role = this.formGroup.get(Fields.Role).value;
    const available = this.forms[role];

    if (available) {
      if (this.additionalAvailableRules.hasOwnProperty(name)) {
        return this.additionalAvailableRules[name]();
      } else {
        return available.hasOwnProperty(name);
      }
    }

    return false;
  }

  isRequired(name: string): boolean {
    const role = this.formGroup.get(Fields.Role).value;
    const available = this.forms[role];

    if (available && available.hasOwnProperty(name)) {
      return (available[name].validators !== null);
    }

    return false;
  }

  canSubmit(): boolean {
    if (this.forceCanSubmit) {
      return this.isRoleSelected();
    } else {
      return (
        !this.formGroup.invalid &&
        !this.formGroup.pristine &&
        this.isRoleSelected()
      );
    }
  }

  isRoleSelected(): boolean {
    return (this.formGroup.get(Fields.Role).value !== null);
  }

  isIndividual(): boolean {
    return (this.formGroup.get(Fields.CustomerType).value === CustomerType.Individual);
  }

  isLegalEntity(): boolean {
    return (this.formGroup.get(Fields.CustomerType).value === CustomerType.LegalEntity);
  }

  isPatentAttorney(): boolean {
    const role = this.formGroup.get(Fields.Role).value;

    if (this.mode !== SubjectFormMode.Insert && role) {
      return role === CustomerRole.PatentAttorney;
    }

    return false;
  }

  close(): void {
    this.reference.close();
  }

  private updateResidentField(): void {
    const residentValue = this.formGroup.get(Fields.Resident).value;
    const countryCodeValue = this.formGroup.get(Fields.CountryCode).value;

    if (countryCodeValue === 'KZ') {
      if (residentValue === ResidentStatus.False) {
        this.formGroup.get(Fields.Resident).setValue(ResidentStatus.True, {
          emitEvent: false
        });
      }
    } else {
      if (residentValue === ResidentStatus.True) {
        this.formGroup.get(Fields.Resident).setValue(ResidentStatus.False, {
          emitEvent: false
        });
      }
    }
  }

  private updateCountryCodeField(): void {
    const residentValue = this.formGroup.get(Fields.Resident).value;

    if (residentValue === ResidentStatus.True) {
      this.formGroup.get(Fields.CountryCode).setValue('KZ', {
        emitEvent: false
      });
    } else {
      this.formGroup.get(Fields.CountryCode).setValue(null, {
        emitEvent: false
      });
    }
  }

  private updateIINField(): void {
    const role = this.formGroup.get(Fields.Role).value;
    const residentValue = this.formGroup.get(Fields.Resident).value;

    if (role && residentValue) {
      if (this.forms[role][Fields.Resident].enabled) {
        if (role === CustomerRole.Declarant || role === CustomerRole.Owner || role === CustomerRole.DeclarantAddress) {
          if (residentValue === ResidentStatus.True) {
            this.formGroup.get(Fields.IIN).setValidators([Validators.minLength(12), Validators.maxLength(12), Validators.required]);
            this.formGroup.get(Fields.IIN).enable();
          } else {
            this.formGroup.get(Fields.IIN).clearValidators();
            this.formGroup.get(Fields.IIN).disable();
          }
        } else {
          this.formGroup.get(Fields.IIN).clearValidators();
          this.formGroup.get(Fields.IIN).enable();
        }
      }
    }
  }

  get formData(): Object {
    const data = Object.assign(this.subject, this.formGroup.value);

    this.outputMapper(data);

    return data;
  }

  private markFields(): void {
    for (let key of Object.keys(this.formGroup.controls)) {
      const control = this.formGroup.get(key);

      control.markAsTouched();
    }
  }

  private checkValidation(): void {
    this.forceCanSubmit = this.formGroup.valid;
  }

  private buildForm(): void {
    this.formGroup = this.formBuilder.group({
      [Fields.Role]: new FormControl(),
      [Fields.IIN]: new FormControl(),
      [Fields.NameRu]: new FormControl(),
      [Fields.NameEn]: new FormControl(),
      [Fields.NameKz]: new FormControl(),
      [Fields.CustomerType]: new FormControl(),
      [Fields.RegisterNumber]: new FormControl,
      [Fields.Resident]: new FormControl(),
      [Fields.Status]: new FormControl(),
      [Fields.BeginDate]: new FormControl(),
      [Fields.EndDate]: new FormControl(),
      [Fields.CountryCode]: new FormControl(),
      [Fields.Country]: new FormControl(),
      [Fields.Region]: new FormControl(),
      [Fields.City]: new FormControl(),
      [Fields.District]: new FormControl(),
      [Fields.Street]: new FormControl(),
      [Fields.AddressRu]: new FormControl(),
      [Fields.AddressEn]: new FormControl(),
      [Fields.AddressKz]: new FormControl(),
      [Fields.Category]: new FormControl(),
      [Fields.OrganizationNameRu]: new FormControl(),
      [Fields.OrganizationNameKz]: new FormControl(),
      [Fields.DirectorName]: new FormControl(),
      [Fields.Tel]: new FormControl(),
      [Fields.MobileTel]: new FormControl(),
      [Fields.Fax]: new FormControl(),
      [Fields.Email]: new FormControl(),
      [Fields.NotNotify]: new FormControl()
    });
  }

  private initSelectOptions(): void {
    Observable.combineLatest(
      this.dictionaryService.getSelectOptions(DictionaryType.DicCountry),
      this.dictionaryService.getSelectOptions(DictionaryType.DicCustomerRole),
      this.dictionaryService.getSelectOptions(DictionaryType.DicCustomerType),
      this.dictionaryService.getBaseDictionary(DictionaryType.DicContactInfoType),
      this.dictionaryService.getBaseDictionary(DictionaryType.DicBeneficiaryType)
    )
      .takeUntil(this.onDestroy)
      .subscribe(([countryCodes, customerRoles, customerTypes, contactInfoTypes, beneficiaryTypes]) => {
        this.countryCodes = countryCodes;
        this.customerRoles = customerRoles;
        this.customerTypes = customerTypes;
        this.contactInfoTypes = contactInfoTypes;
        // this.beneficiaryTypes = beneficiaryTypes;

        const feedback = {
          SMB: () => this.isIndividual() || this.isLegalEntity(),
          VET: () => this.isIndividual()
        };
        const entries = beneficiaryTypes
          .filter(entry => {
            return feedback.hasOwnProperty(entry.code);
          })
          .map(entry => {
            return {
              id: entry.id,
              text: entry.nameRu,
              isAvailable: feedback[entry.code]
            };
          });

        this.availableCategories = entries;

        this.deferredTasks.forEach((deferredTask, index) => {
          deferredTask();

          this.deferredTasks.splice(index, 1);
        });
      });
  }

  private initFormData(customerShortInfo: CustomerShortInfo) {
    for (let key of Object.keys(this.formGroup.controls)) {
      if (key !== Fields.Role) {
        const control = this.formGroup.get(key);

        control.reset();
      }
    }

    for (let [key, value] of Object.entries(customerShortInfo)) {
      if (value === null || value === undefined) {
        delete(customerShortInfo[key]);
      }
    }

    const data = Object.assign(this.formGroup.value, customerShortInfo);

    this.inputMapper(data);

    this.subject = data;
    this.isPatchValue = true;
    this.previousValue = data[Fields.IIN];
    this.formGroup.patchValue(data);
    this.isPatchValue = false;

    this.checkValidation();

    if (this.mode !== SubjectFormMode.Insert && this.mode !== SubjectFormMode.Edit) {
      this.mode = SubjectFormMode.Attach;
    }
  }

  private inputMapper(data: any): void {
    data[Fields.Role] = this.formGroup.get(Fields.Role).value || data[Fields.Role];
    data[Fields.Resident] = data.isNotResident ? ResidentStatus.False : ResidentStatus.True;

    this.deferredTasks.push(() => {
      if (this.countryCodes) {
        const country = this.countryCodes.find(entry => entry.id === data.countryId);

        if (country) {
          this.formGroup.get(Fields.Country).setValue(country.nameRu);
        }
      }
    });

    for (let key of [Fields.Tel, Fields.MobileTel, Fields.Fax, Fields.Email]) {
      if (data[key] && typeof data[key] === 'string') {
        data[key] = data[key].split(';').map(entry => entry.trim());

        if (!(data[key] instanceof Array)) {
          data[key] = [data[key]];
        }
      }
    }
  }

  private outputMapper(data: any): void {
    data.mode = data.customerId ? this.mode : SubjectFormMode.InsertOnAttach;
    data.ownerId = this.subject.ownerId;
    if (data.countryCode && this.countryCodes) {
      const country = this.countryCodes.find(entry => (entry.code === data.countryCode));

      data.countryId = country ? country.id : null;
    }
    if (data.roleCode && this.customerRoles) {
      const role = this.customerRoles.find(entry => (entry.code === data.roleCode));

      data.roleId = role ? role.id : null;
    }
    if (data.typeId && this.customerTypes) {
      const type = this.customerTypes.find(entry => (entry.id === data.typeId));

      data.typeNameRu = type ? type.nameRu : null;
    }
    data.contactInfos = [];

    if (this.contactInfoTypes) {
      const feedback = {
        [Fields.Tel]: 'phone',
        [Fields.MobileTel]: 'mobilePhone',
        [Fields.Fax]: 'fax',
        [Fields.Email]: 'email'
      };

      for (let key of [Fields.Tel, Fields.MobileTel, Fields.Fax, Fields.Email]) {
        if (data[key]) {
          const type = this.contactInfoTypes.find(entry => (entry.code === feedback[key]));

          if (type) {
            if (data[key] instanceof Array) {
              const contactInfos: ContactInfoDto[] = data[key].map(entry => {
                return {
                  typeId: type.id,
                  info: entry
                };
              });

              data.contactInfos.push(...contactInfos);

              data[key] = data[key].join('; ');
            } else {
              const contactInfo: ContactInfoDto = {
                id: null,
                typeId: type.id,
                info: data[key]
              };

              data.contactInfos.push(contactInfo);
            }
          }
        }
      }
    }
  }
}
