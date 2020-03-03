import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material';
import { Subject } from 'rxjs';

import { UndoDialogComponent } from 'app/shared/components/undo-dialog/undo-dialog.component';
import { IntellectualPropertyDetails } from 'app/shared/models/intellectual-property-details';

import { isStagesForIpc, hasSelectiveAchievementType, isStageFormationAppData, isStageFullExpertise, isStageFormalExam, isStagePatentExam } from '../description/description.component';

import { Combination } from './combination';
import { Priority } from './priority.enum';
import { ConventionType } from './convention-type.enum';

import { PCTConfiguration, EurasionApplicationConfiguration, PriorityDataConfiguration } from './configuration';
import { DictionaryService } from 'app/shared/services/dictionary.service';
import { DictionaryType } from 'app/shared/services/models/dictionary-type.enum';
import { ProtectionDocTypes } from 'app/shared/enums/protection-doc-types.enum';

@Component({
  selector: 'app-priority',
  templateUrl: './priority.component.html',
  styleUrls: ['./priority.component.scss'],
})
export class PriorityComponent implements OnInit {
  @Input() owner: IntellectualPropertyDetails;
  @Input() isChangeMode: boolean;
  @Input() disabled: boolean;
  @Output() edit: EventEmitter<boolean> = new EventEmitter();
  @Output() submitData: EventEmitter<any> = new EventEmitter();

  pctConfigurations = PCTConfiguration;
  eurasianApplicationConfigurations = EurasionApplicationConfiguration;
  priorityDataConfigurations = PriorityDataConfiguration;
  storage = {};
  statusTypes = {
    [ConventionType.NoPriority]: null,
    [ConventionType.Convention]: [
      ProtectionDocTypes.Invention,
      ProtectionDocTypes.UsefulModel,
      ProtectionDocTypes.SelectionAchievement,
      ProtectionDocTypes.IndustrialDesign,
      ProtectionDocTypes.Trademark
    ],
    [ConventionType.PCT]: [
      ProtectionDocTypes.Invention,
      ProtectionDocTypes.UsefulModel,
      ProtectionDocTypes.SelectionAchievement,
    ],
    [ConventionType.PCTConvention]: [
      ProtectionDocTypes.Invention,
      ProtectionDocTypes.UsefulModel,
      ProtectionDocTypes.SelectionAchievement
    ],
    [ConventionType.PCTReceivingOffice]: [],
    [ConventionType.EurasianApplication]: []
  };
  statuses = {
    [ConventionType.NoPriority]: new Combination()
      .empty(),
    [ConventionType.Convention]: new Combination()
      .equals(
        Priority.FirstApplicationParisConvention,
        Priority.EqualsParagraph3Article20,
        Priority.EqualsParagraph4Article20,
        Priority.EqualsParagraph5Article20,
        Priority.PriorityData,
        Priority.ExhibitionPriority,
        Priority.ClaimedDesignationHasEarlierRegistration,
        Priority.ReceiptApplicationAdmissionStateCommission,
        Priority.FilingApplicationCountryInternationalConvention,
        Priority.WhetherOfferedRepublic,
        Priority.WhetherOfferedOther
      )
      .notEquals(
        Priority.InternationalApplication,
        Priority.PublicationInternationalApplication
      ),
    [ConventionType.PCT]: new Combination()
      .equals(
        Priority.InternationalApplication,
        Priority.PublicationInternationalApplication
      )
      .notEquals(
        Priority.FirstApplicationParisConvention,
        Priority.EqualsParagraph3Article20,
        Priority.EqualsParagraph4Article20,
        Priority.EqualsParagraph5Article20,
        Priority.PriorityData,
        Priority.ExhibitionPriority,
        Priority.ClaimedDesignationHasEarlierRegistration,
        Priority.ReceiptApplicationAdmissionStateCommission,
        Priority.FilingApplicationCountryInternationalConvention,
        Priority.WhetherOfferedRepublic,
        Priority.WhetherOfferedOther
      ),
    [ConventionType.PCTConvention]: (...values) => {
      const combinations = [
        (values) => new Combination()
          .equals(
            Priority.InternationalApplication,
            Priority.PublicationInternationalApplication
          )
          .some(...values),
        (values) => new Combination()
          .equals(
            Priority.FirstApplicationParisConvention,
            Priority.EqualsParagraph3Article20,
            Priority.EqualsParagraph4Article20,
            Priority.EqualsParagraph5Article20,
            Priority.ReceiptApplicationAdmissionStateCommission,
            Priority.FilingApplicationCountryInternationalConvention,
            Priority.WhetherOfferedRepublic,
            Priority.WhetherOfferedOther
          )
          .some(...values)
      ];
      const result = combinations.map((combination) => combination(values)).every((value) => value);

      return result;
    },
    [ConventionType.PCTReceivingOffice]: new Combination(),
    [ConventionType.EurasianApplication]: new Combination()
      .equals(
        Priority.EurasianApplication,
        Priority.PublicationEurasianApplication
      )
  };
  conventionType = ConventionType;
  protectionDocTypes = ProtectionDocTypes;
  applicationType: ProtectionDocTypes = null;
  formGroup: FormGroup;
  editMode = false;
  canSubmit = {};
  earlyRegTypes: any[];
  needSubmit: EventEmitter<any> = new EventEmitter();

  private onDestroy = new Subject();

  constructor(
    private dialog: MatDialog,
    private dictionaryService: DictionaryService
  ) {
    this.fillData();
    this.buildForm();
  }

  ngOnInit(): void {
    this.dictionaryService.getBaseDictionary(DictionaryType.DicCountry)
      .subscribe((data: any[]) => {
        const countries = data.map((entry) => ({
          id: entry.id,
          value: entry.code
        }));

        for (let entry of this.pctConfigurations) {
          entry.setValues({
            regCountryId: countries
          });
        }

        for (let entry of this.eurasianApplicationConfigurations) {
          entry.setValues({
            regCountryId: countries
          });
        }

        for (let entry of this.priorityDataConfigurations) {
          entry.setValues({
            regCountryId: countries
          });
        }
      });

    this.applicationType = this.owner.protectionDocTypeCode as ProtectionDocTypes;
  }

  checkCategories(): void {
    const keys = Object.values(Priority);

    for (let key of keys) {
      if (this.storage[key].data.length === 0) {
        this.storage[key].active = false;
      }
    }
  }

  fillData(): void {
    const keys = Object.values(Priority);

    for (let key of keys) {
      this.storage[key] = {
        active: false,
        data: []
      };
    }

    if (this.earlyRegTypes) {
      const requestEarlyRegs = this.owner.requestEarlyRegDtos;
      for (let requestEarlyReg of requestEarlyRegs) {
        const earlyRegType = this.earlyRegTypes.find((entry) => (entry.id === requestEarlyReg.earlyRegTypeId));

        if (earlyRegType && this.storage.hasOwnProperty(earlyRegType.code)) {
          this.storage[earlyRegType.code].active = true;
          this.storage[earlyRegType.code].data.push(requestEarlyReg);
        }
      }
    } else {
      this.dictionaryService.getBaseDictionary(DictionaryType.DicEarlyRegType)
        .subscribe((earlyRegTypes) => {
          const requestEarlyRegs = this.owner.requestEarlyRegDtos;
          for (let requestEarlyReg of requestEarlyRegs) {
            const earlyRegType = earlyRegTypes.find((entry) => (entry.id === requestEarlyReg.earlyRegTypeId));

            if (earlyRegType && this.storage.hasOwnProperty(earlyRegType.code)) {
              this.storage[earlyRegType.code].active = true;
              this.storage[earlyRegType.code].data.push(requestEarlyReg);
            }
          }

          this.earlyRegTypes = earlyRegTypes;
        });
    }
  }

  onToggle({ name, data }): void {
    if (this.storage.hasOwnProperty(name)) {
      this.storage[name].active = Boolean(data);

      if (!this.storage[name].active) {
        this.canSubmit[name] = true;
      }
    }
  }

  onCanSubmitUpdate(name, value): void {
    this.canSubmit[name] = value;
  }

  get status(): ConventionType {
    const activatedCategories = Object.entries(this.storage)
      .filter(([key, entry]: [any, any]) => entry.active)
      .map(([key, entry]) => key);

    const statuses = {};
    for (let key of Object.keys(this.statuses)) {
      const callback = this.statuses[key];
      const typesForStatus = this.statusTypes[key];
      const isStageForStatus = typesForStatus instanceof Array ? typesForStatus.includes(this.applicationType) : true;

      if (callback instanceof Combination) {
        statuses[key] = callback.some(...activatedCategories) && isStageForStatus;
      } else if (callback instanceof Function) {
        statuses[key] = callback(...activatedCategories) && isStageForStatus;
      }
    }
    const availableStatuses = Object.entries(statuses)
      .filter(([key, entry]: [any, any]) => entry)
      .map(([key, entry]) => key);

    return availableStatuses[0] as ConventionType;
  }

  get formData(): any {
    const data = [];
    const entries = Object.entries(this.storage)
      .filter(([key, entry]: [any, any]) => entry.active)
      .map(([key, entry]: [any, any]) => [key, entry.data]);

    for (let [key, entry] of entries) {
      for (let item of entry) {
        const earlyRegType = this.earlyRegTypes.find((earlyRegType) => {
          return earlyRegType.code === key;
        });
        const entity = {
          earlyRegTypeId: earlyRegType ? earlyRegType.id : null,
          regNumber: null,
          regDate: null,
          regCountryId: null,
          nameSD: null,
          stageSD: null,
          chapterOne: null,
          dateOfChapterOne: null,
          chapterTwo: null,
          dateOfChapterTwo: null
        };

        data.push(Object.assign(entity, item));
      }
    }

    return data;
  }

  private toggleEditMode(value: boolean): void {
    this.editMode = value;
    this.edit.emit(value);
  }

  private openDialog(): void {
    const dialogRef = this.dialog.open(UndoDialogComponent);

    dialogRef
      .afterClosed()
      .takeUntil(this.onDestroy)
      .subscribe(result => {
        if (result === 'true') {
          this.toggleEditMode(false);
          this.fillData();
        }
      });
  }

  onUndo(): void {
    this.openDialog();
  }

  onEdit(): void {
    this.toggleEditMode(true);
  }

  onSubmit(): void {
    this.needSubmit.emit(true);

    const data = this.formData;
    this.owner.requestEarlyRegDtos = data;
    this.submitData.emit(this.owner);
    this.checkCategories();
    this.toggleEditMode(false);
  }

  hasRequestNum(): boolean {
    return !!this.owner && !!this.owner.requestNum;
  }

  /**
   * Возвращает состояние кнопки редактирования данных.
   */
  isDisabledEditButton(): boolean {
    // if (!hasSelectiveAchievementType(this.owner) && !this.hasRequestNum()) {
    //   return false;
    // }

    // return !(
    //   this.hasRequestNum() &&
    //   (
    //     isStageFormationAppData(this.owner) ||
    //     isStageFormalExam(this.owner) ||
    //     isStageFullExpertise(this.owner) ||
    //     isStagePatentExam(this.owner)
    //   )
    // );

    return this.disabled;
  }

  /**
   * Возвращает состояние кнопки сохранения данных.
   */
  isDisabledSaveButton(): boolean {
    // const isEnabled = Object.entries(this.canSubmit).every(([key, value]: [string, boolean]) => value);

    // return !isEnabled;

    return false;
  }

  isDisabled(): boolean {
    // return (
    //   !this.editMode ||
    //   (
    //     (
    //       isStageFullExpertise(this.owner) ||
    //       isStagesForIpc(this.owner)
    //     ) &&
    //     !this.editMode
    //   )
    // );

    return !this.editMode;
  }

  private buildForm(): void {
    this.formGroup = new FormGroup({});
  }
}
