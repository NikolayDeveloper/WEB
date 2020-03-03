import {
  Component,
  OnInit,
  Input,
  Output,
  EventEmitter,
  OnDestroy,
  SimpleChanges,
  OnChanges,
  ViewChildren,
  QueryList,
  AfterViewInit
} from '@angular/core';
import { IntellectualPropertyDetails } from 'app/shared/models/intellectual-property-details';
import {
  FormGroup,
  FormBuilder,
  Validators,
  AbstractControl
} from '@angular/forms';
import { RouteStageCodes } from 'app/shared/models/route-stage-codes';
import { Subject } from 'rxjs/Subject';
import { UndoDialogComponent } from 'app/shared/components/undo-dialog/undo-dialog.component';
import { MatDialog, MatDialogConfig } from '@angular/material';
import {
  BiblioFieldConfig,
  FieldType,
  BiblioField
} from 'app/bibliographic-data/models/field-config';
import { IcgsSplitDialogComponent } from '../icgs-split-dialog/icgs-split-dialog.component';
import { OwnerType } from 'app/shared/services/models/owner-type.enum';
import { Router } from '@angular/router';
import { RequestService } from 'app/requests/request.service';
import { ProtectionDocsService } from 'app/protection-docs/protection-docs.service';
import { BaseDictionary } from 'app/shared/services/models/base-dictionary';
import { DictionaryType } from 'app/shared/services/models/dictionary-type.enum';
import { DictionaryService } from 'app/shared/services/dictionary.service';
import { NavigateOnSelectService } from 'app/expert-search/services/navigate-on-select.service';
import { CtmParticipantsFieldDialogComponent } from '../ctm-participants-field-dialog/ctm-participants-field-dialog.component';
import {
  ConvertDto,
  ConvertResponseDto
} from 'app/bibliographic-data/models/convert-dto';
import { ChangeDialogComponent } from '../changes/change-dialog/change-dialog.component';
import { ChangeDetails } from '../changes/confirm-changes/confirm-changes.component';
import { Observable } from 'rxjs/Observable';
import { BibliographicDataService } from 'app/bibliographic-data/bibliographic-data.service';
import { SubjectsService } from 'app/subjects/services/subjects.service';
import { ChangeDetectorRef } from "@angular/core";
import { dicDocumentTypeCodes } from 'app/shared/models/dicDocumentTypeCodes';

@Component({
  selector: 'app-description',
  templateUrl: './description.component.html',
  styleUrls: ['./description.component.scss']
})
export class DescriptionComponent
  implements OnInit, AfterViewInit, OnChanges, OnDestroy {
  @Input()
  owner: IntellectualPropertyDetails;
  @Output()
  submitData: EventEmitter<any> = new EventEmitter();
  @Output()
  edit: EventEmitter<boolean> = new EventEmitter();
  @Output()
  changed = new EventEmitter<string>();
  @ViewChildren(BiblioField)
  fields!: QueryList<BiblioField>;
  @Input()
  isChangeMode: boolean;
  @Input() disabled: boolean;

  editMode = false;
  canSplit: boolean;
  formGroup: FormGroup;
  trademarkTypeId: number;

  private trademarkType: BaseDictionary;

  get pdTypeTMCodes() {
    return RouteStageCodes.pdTypeTMCodes;
  }
  get pdTypeCOOCodes() {
    return RouteStageCodes.pdTypeCOOCodes;
  }
  get pdTypeInventionsCodes() {
    return RouteStageCodes.pdTypeInventionsCodes;
  }
  get pdTypeUsefulModelsCodes() {
    return RouteStageCodes.pdTypeUsefulModelsCodes;
  }
  get pdTypeIndustrialdesignsCodes() {
    return RouteStageCodes.pdTypeIndustrialdesignsCodes;
  }
  get requestTypeSelectiveAchievementsCode() {
    return RouteStageCodes.pdTypeSelectiveAchievementsCodes;
  }
  get stagesFormationAppData() {
    return RouteStageCodes.stagesFormationAppData;
  }
  get stagesFormalExam() {
    return RouteStageCodes.stagesformalExam;
  }
  get stagesFullExpertise() {
    return RouteStageCodes.stagesFullExpertise;
  }

  get colorFieldType() {
    return FieldType.Color;
  }
  get icfemFieldType() {
    return FieldType.Icfem;
  }
  get icgsFieldType() {
    return FieldType.Icgs;
  }
  get icisFieldType() {
    return FieldType.Icis;
  }
  get imageFieldType() {
    return FieldType.Image;
  }
  get ipcFieldType() {
    return FieldType.Ipc;
  }
  // get priorityFieldType() {
  //   return FieldType.Priority;
  // }
  get productFieldType() {
    return FieldType.Product;
  }
  get referatFieldType() {
    return FieldType.Referat;
  }
  get selectionFieldType() {
    return FieldType.Selection;
  }
  get trademarkFieldType() {
    return FieldType.Trademark;
  }
  get mediaFieldType() {
    return FieldType.Media;
  }

  private onDestroy = new Subject();
  private fieldsConfig: BiblioFieldConfig[] = [
    {
      type: FieldType.Color,
      protectionDocTypeCodes: [...this.pdTypeTMCodes]
    },
    {
      type: FieldType.Icfem,
      protectionDocTypeCodes: [...this.pdTypeTMCodes]
    },
    {
      type: FieldType.Icgs,
      protectionDocTypeCodes: [...this.pdTypeTMCodes]
    },
    {
      type: FieldType.Icis,
      protectionDocTypeCodes: [...this.pdTypeIndustrialdesignsCodes]
    },
    {
      type: FieldType.Image,
      protectionDocTypeCodes: [
        ...this.pdTypeIndustrialdesignsCodes,
        ...this.pdTypeTMCodes
      ]
    },
    {
      type: FieldType.Media,
      protectionDocTypeCodes: [
        ...this.pdTypeIndustrialdesignsCodes,
        ...this.pdTypeTMCodes
      ]
    },
    {
      type: FieldType.Ipc,
      protectionDocTypeCodes: [
        ...this.pdTypeInventionsCodes,
        ...this.pdTypeUsefulModelsCodes
      ]
    },
    // {
    //   type: FieldType.Priority,
    //   protectionDocTypeCodes: [
    //     ...this.pdTypeInventionsCodes,
    //     ...this.pdTypeUsefulModelsCodes,
    //     ...this.pdTypeIndustrialdesignsCodes,
    //     ...this.pdTypeTMCodes,
    //     ...this.requestTypeSelectiveAchievementsCode
    //   ]
    // },
    {
      type: FieldType.Product,
      protectionDocTypeCodes: [...this.pdTypeCOOCodes]
    },
    {
      type: FieldType.Referat,
      protectionDocTypeCodes: [
        ...this.pdTypeIndustrialdesignsCodes,
        ...this.pdTypeInventionsCodes,
        ...this.pdTypeUsefulModelsCodes
      ]
    },
    {
      type: FieldType.Selection,
      protectionDocTypeCodes: [...this.requestTypeSelectiveAchievementsCode]
    },
    {
      type: FieldType.Trademark,
      protectionDocTypeCodes: [...this.pdTypeTMCodes]
    }
  ];

  constructor(
    private dialog: MatDialog,
    private router: Router,
    private requestService: RequestService,
    private fb: FormBuilder,
    private dictionaryService: DictionaryService,
    private bibliographicDataService: BibliographicDataService,
    private subjectService: SubjectsService,
    private navigateOnSelectService: NavigateOnSelectService,
    private changeDetectorRef: ChangeDetectorRef
  ) {
    this.buildForm();
  }

  ngOnInit() {
    this.initBibliographicData();
    this.onTrademarkTypeChanged(this.owner.typeTrademarkId);
  }

  ngAfterViewInit(): void {
    this.toggleEditMode(this.isChangeMode ? true : false);
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.owner && changes.owner.currentValue) {
      this.initBibliographicData();
    }
    this.changeDetectorRef.detectChanges();
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  onSubmit() {
    if (this.formGroup.invalid && !this.formGroup.get('commonFields').valid) {
      return;
    }
    this.formGroup.markAsPristine();
    this.fields.forEach(f => {
      this.owner = Object.assign(this.owner, f.getValue());
    });
    this.submitData.emit(this.owner);
    this.toggleEditMode(false);
  }

  /**
   * Возвращает состояние кнопки сохранения данных.
   */
  isDisabledSaveButton() {
    const control = this.formGroup.get('icgsFields');
    if(['OD04.3', 'OD01.6'].includes(this.owner.currentWorkflow.currentStageCode)){
      const specialControl = this.formGroup.get('commonFields');
      return (
        specialControl.invalid ||
        specialControl.pristine
      );
    }
    if (control.value.icgsRequestIdsFormArray) {
      return (
        this.formGroup.invalid ||
        this.formGroup.pristine ||
        control.value.icgsRequestIdsFormArray.length === 0
      );
    } else {
      return (
        this.formGroup.invalid ||
        this.formGroup.pristine
      );
    }
  }

  isDisabledEditButton() {
    if (!this.owner.currentWorkflow) {
      return true;
    }

    return false;
  //   if (['OD04.3', 'OD05','OD01.6'].includes(this.owner.currentWorkflow.currentStageCode)) {
  //     return false;
  //   }

  //   if (!hasSelectiveAchievementType(this.owner) && !this.hasRequestNum()) {
  //     return false;
  //   }

  // return !(
  //     this.hasRequestNum() &&
  //     (
  //       isStageFormationAppData(this.owner) ||
  //       isStageFormalExam(this.owner) ||
  //       isStageFullExpertise(this.owner) ||
  //       isStagePatentExam(this.owner)
  //     ) &&
  //     !this.disabled
  //   );
  }

  hasRequestNum(): boolean {
    return !!this.owner && !!this.owner.requestNum;
  }

  onUndo() {
    if (this.formGroup.dirty) {
      this.openDialog();
      return false;
    } else {
      this.initBibliographicData();
      this.toggleEditMode(false);
    }
  }

  onEdit() {
    this.toggleEditMode(true);
  }

  isFieldActive(type: FieldType): boolean {
    const config = this.fieldsConfig.find(fc => fc.type === type);
    if (config && this.owner) {
      return config.protectionDocTypeCodes.includes(
        this.owner.protectionDocTypeCode
      );
    }
    return false;
  }

  isDisabled(): boolean {
    return (
      !this.editMode ||
      ((isStageFullExpertise(this.owner) || isStagesForIpc(this.owner)) &&
        !this.editMode)
    );
  }

  onSplitClick() {
    const config = new MatDialogConfig();
    config.disableClose = true;
    config.width = '35vw';
    config.data = {
      ownerId: this.owner.id,
      ownerType: this.owner.ownerType,
      icgsRequests: this.owner.icgsRequestDtos
    };
    const dialogRef = this.dialog.open(IcgsSplitDialogComponent, config);

    dialogRef
      .afterClosed()
      .filter(value => !!value)
      .subscribe(newRequestId => {
        switch (this.owner.ownerType) {
          case OwnerType.Request:
            this.navigateOnSelectService.openItemInNewTab(
              'requests',
              newRequestId
            );
            this.router.navigate(['requests', this.owner.id]);
            break;
          case OwnerType.ProtectionDoc:
            this.navigateOnSelectService.openItemInNewTab(
              'protectiondocs',
              newRequestId
            );
            this.router.navigate(['protectiondocs', this.owner.id]);
            break;
        }
      });
  }

  setRequestNumber(result: any) {
    if (!!result.number) {
      this.owner.requestNum = result.number;
    }
  }

  onTrademarkTypeChanged(typeId: number) {
    this.trademarkTypeId = typeId;
    this.dictionaryService
      .getBaseDictionaryById(
        DictionaryType.DicTypeTrademark,
        this.trademarkTypeId
      )
      .takeUntil(this.onDestroy)
      .subscribe(data => (this.trademarkType = data));
  }
  onSpeciesTrademarkChanged(id) {
    // Переопределение обьекта для отработки OnChanges
    const owner = {...this.owner};
    owner.speciesTradeMarkId = id;
    this.owner = {...owner};
  }

  isVideo(): boolean {
    return (
      !!this.trademarkType && ['02', '11'].includes(this.trademarkType.code)
    );
  }

  isAudio(): boolean {
    return !!this.trademarkType && this.trademarkType.code === '03';
  }

  isConvertAvailable(): boolean {
    return (
      this.owner &&
      this.owner.currentWorkflow &&
      this.owner.currentWorkflow.currentStageCode === 'TMConvert'
    );
  }

  isSplitAvailable(): boolean {
    return (
      this.owner &&
      this.owner.currentWorkflow &&
      this.owner.currentWorkflow.currentStageCode === 'TM03.3.2.2'
    );
  }

  isChangeAvailable(): boolean {
    const codes = [
      'B03.9',
      'OD04.6',
      'TM06',
      'TMI06',
      'SA03.3.9.0',
      'NMPT03.2.4',
      'OD04.6.0',
      'PO03.8.4',
      'U02.2.7',
      'U02.2.7.0',
      'PD_NMPT_MakingChanges',
      'PD_TM_MakingChanges'
    ];

    return (
      this.owner &&
      this.owner.currentWorkflow &&
      codes.includes(this.owner.currentWorkflow.currentStageCode)
    );
  }

  onConvertClick() {
    const convertDto = new ConvertDto();
    convertDto.ownerId = this.owner.id;
    convertDto.ownerType = OwnerType.Request;

    if (this.owner && this.owner.speciesTrademarkCode !== 'KTM') {
      const config = new MatDialogConfig();
      config.disableClose = true;
      config.width = '35vw';
      config.data = {
        colectiveTrademarkParticipantsInfo: this.owner
          .colectiveTrademarkParticipantsInfo
      };

      const dialogRef = this.dialog.open(
        CtmParticipantsFieldDialogComponent,
        config
      );

      dialogRef
        .afterClosed()
        .filter(value => !!value)
        .subscribe(info => {
          convertDto.colectiveTrademarkParticipantsInfo = info;
          this.requestService
            .convertRequest(convertDto)
            .takeUntil(this.onDestroy)
            .subscribe((data: ConvertResponseDto) => {
              this.updateAfterConvert(data);
            });
        });
    } else {
      this.requestService
        .convertRequest(convertDto)
        .takeUntil(this.onDestroy)
        .subscribe((data: ConvertResponseDto) => {
          this.updateAfterConvert(data);
        });
    }
  }

  onChangeClick() {
    const dialogRefChange = this.dialog.open(ChangeDialogComponent, {
      data: {
        details: this.owner
      },
      width: '1680px',
      height: '680px'
    });

    dialogRefChange
      .afterClosed()
      .takeUntil(this.onDestroy)
      .subscribe((data: ChangeDetails) => {
        this.bibliographicDataService
          .change(data.changeDtos)
          .switchMap(() => {
            if (!!data.details) {
              return this.bibliographicDataService.updateBibliographicData(
                data.details
              );
            }
            return Observable.of(null);
          })
          .switchMap(() => {
            if (!!data.details) {
              return this.bibliographicDataService.updateColors(
                data.details.id,
                data.details.ownerType,
                data.details.colorTzIds
              );
            }
            return Observable.of(null);
          })
          .switchMap(() => {
            if (!!data.details) {
              return this.bibliographicDataService.updateEarlyRegs(
                data.details.id,
                data.details.ownerType,
                data.details.requestEarlyRegDtos
              );
            }
            return Observable.of(null);
          })
          .switchMap(() => {
            if (!!data.details) {
              return this.bibliographicDataService.updateIcfems(
                data.details.id,
                data.details.ownerType,
                data.details.icfemIds
              );
            }
            return Observable.of(null);
          })
          .switchMap(() => {
            if (!!data.details) {
              return this.bibliographicDataService.updateIcgs(
                data.details.id,
                data.details.ownerType,
                data.details.icgsRequestDtos
              );
            }
            return Observable.of(null);
          })
          .switchMap(() => {
            if (!!data.details) {
              return this.subjectService.attachSeveral(
                data.subjectsToAttach,
                data.details.ownerType
              );
            }
            return Observable.of(null);
          })
          .switchMap(() => {
            if (!!data.details) {
              return this.subjectService.deleteSeveral(
                data.subjectsToDelete.map(s => s.id),
                data.details.ownerType
              );
            }
            return Observable.of(null);
          })
          .switchMap(() => {
            if (!!data.details) {
              return this.subjectService.updateSeveral(
                data.subjectsToEdit,
                data.details.ownerType
              );
            }
            return Observable.of(null);
          })
          .switchMap(() => {
            if (!!data.details.imageFile) {
              return this.requestService.uploadImage(
                data.details.imageFile,
                data.details.id
              );
            }
            return Observable.of(null);
          })
          .switchMap(() => {
            if (!!data.details) {
              return this.requestService.generateNotification(
                [dicDocumentTypeCodes.NOT4],
                data.details.id
              );
            }
            return Observable.of(null);
          })
          .switchMap(() => {
            return this.requestService.getRequestById(data.details.id);
          })
          .takeUntil(this.onDestroy)
          .subscribe(details => {
            this.owner = details;
            this.submitData.emit(this.owner);
            this.initBibliographicData();
          });
      });
  }

  private updateAfterConvert(data: ConvertResponseDto) {
    this.owner.colectiveTrademarkParticipantsInfo =
      data.colectiveTrademarkParticipantsInfo;
    this.owner.speciesTradeMarkId = data.speciesTradeMarkId;
    this.owner.speciesTrademarkCode = data.speciesTrademarkCode;
    this.initBibliographicData();
  }

  private toggleEditMode(value: boolean) {
    this.editMode = value;
    this.edit.emit(value);
  }

  private openDialog() {
    const dialogRef = this.dialog.open(UndoDialogComponent);

    dialogRef
      .afterClosed()
      .takeUntil(this.onDestroy)
      .subscribe(result => {
        if (result === 'true') {
          this.initBibliographicData();
          this.toggleEditMode(false);
        }
      });
  }

  private initBibliographicData() {
    if (this.owner && this.formGroup) {
      this.formGroup.get('commonFields').setValue(this.owner);
      this.formGroup.get('referatFields').setValue(this.owner.referat);
      this.formGroup.get('imageFields').setValue(this.owner);
      this.formGroup.get('trademarkFields').setValue(this.owner);
      this.formGroup.get('productFields').setValue(this.owner);
      this.formGroup.get('selectionFields').setValue(this.owner);
      this.formGroup.get('icgsFields').setValue(this.owner.icgsRequestDtos);
      this.formGroup.get('colorFields').setValue(this.owner.colorTzIds);
      this.formGroup.get('icfemFields').setValue(this.owner.icfemIds);
      this.formGroup.get('ipcFields').setValue(this.owner);
      // this.formGroup.get('priorityFields').setValue(this.owner);
      this.formGroup.get('mediaFields').setValue(this.owner);
      this.formGroup.get('icisFields').setValue(this.owner.icisRequestIds);

    }
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      commonFields: [{ value: '' }],
      referatFields: [{ value: '' }],
      imageFields: [{ value: '' }],
      trademarkFields: [{ value: '' }],
      productFields: [{ value: '' }],
      selectionFields: [{ value: '' }],
      icgsFields: [{ value: '' }],
      colorFields: [{ value: '' }],
      icfemFields: [{ value: '' }],
      ipcFields: [{ value: '' }],
      // priorityFields: [{ value: '' }],
      icisFields: [{ value: '' }],
      mediaFields: [{ value: '' }]
    });
  }

  public getValue(): IntellectualPropertyDetails {
    this.fields.forEach(f => {
      this.owner = Object.assign(this.owner, f.getValue());
    });
    return this.owner;
  }
}

export function isStageFullExpertise(
  owner: IntellectualPropertyDetails
): boolean {
  if (!!owner && owner.currentWorkflow) {
    return RouteStageCodes.stagesFullExpertise.includes(
      owner.currentWorkflow.currentStageCode
    );
  } else {
    return false;
  }
}

export function isStageFormalExam(owner: IntellectualPropertyDetails): boolean {
  if (!!owner && owner.currentWorkflow) {
    return RouteStageCodes.stagesformalExam.includes(
      owner.currentWorkflow.currentStageCode
    );
  } else {
    return false;
  }
}

export function isStageFormationAppData(
  owner: IntellectualPropertyDetails
): boolean {
  if (!!owner && owner.currentWorkflow) {
    return RouteStageCodes.stagesFormationAppData.includes(
      owner.currentWorkflow.currentStageCode
    );
  } else {
    return false;
  }
}

export function isStageMakingChanges(owner: IntellectualPropertyDetails): boolean {
  if (!!owner && owner.currentWorkflow) {
    return RouteStageCodes.stagesMakingChanges.includes(
      owner.currentWorkflow.currentStageCode
    );
  } else {
    return false;
  }
}

export function isStageRequestSeparation(owner: IntellectualPropertyDetails): boolean {
  if (!!owner && owner.currentWorkflow) {
    return RouteStageCodes.stagesRequestSeparation.includes(
      owner.currentWorkflow.currentStageCode
    );
  } else {
    return false;
  }
}

export function isStagePatentExam(owner: IntellectualPropertyDetails): boolean {
  if (!!owner && owner.currentWorkflow) {
    return RouteStageCodes.stagesPatentExam.includes(
      owner.currentWorkflow.currentStageCode
    );
  } else {
    return false;
  }
}

export function isStagesForIpc(owner: IntellectualPropertyDetails): boolean {
  const routeStagesForIpc = [
    'B02.1',
    'B03.2.3',
    'B03.2.1',
    'B03.2.2.1',
    'B03.2.2.0',
    'B03.2.3.0',
    'B03.2.4',
    'U02.1',
    'U03.1',
    'OD04.6',
    ...RouteStageCodes.stagesPatentExam
  ];
  if (!!owner && owner.currentWorkflow) {
    return routeStagesForIpc.includes(owner.currentWorkflow.currentStageCode);
  } else {
    return false;
  }
}

export function hasSelectiveAchievementType(
  owner: IntellectualPropertyDetails
): boolean {
  if (
    !!owner &&
    RouteStageCodes.pdTypeSelectiveAchievementsCodes.includes(
      owner.protectionDocTypeCode
    )
  ) {
    return !!owner.selectionAchieveTypeId;
  }
  return true;
}

export function isStageCreation(owner: IntellectualPropertyDetails) {
  if (!!owner && owner.currentWorkflow) {
    return RouteStageCodes.stagesinitial.includes(
      owner.currentWorkflow.currentStageCode
    );
  } else {
    return false;
  }
}

export function isStagePayment(owner: IntellectualPropertyDetails) {
  if (!!owner && owner.currentWorkflow) {
    return RouteStageCodes.stagesPayment.includes(
      owner.currentWorkflow.currentStageCode
    );
  } else {
    return false;
  }
}
