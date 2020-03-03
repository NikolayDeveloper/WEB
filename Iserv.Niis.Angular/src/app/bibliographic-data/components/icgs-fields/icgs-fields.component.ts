import { FocusMonitor } from '@angular/cdk/a11y';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import {
  Component,
  ElementRef,
  EventEmitter,
  forwardRef,
  HostBinding,
  Input,
  OnDestroy,
  OnInit,
  Output,
  Renderer2,
  OnChanges,
  SimpleChanges
} from '@angular/core';
import {
  AbstractControl,
  ControlValueAccessor,
  FormArray,
  FormBuilder,
  FormGroup,
  NG_VALIDATORS,
  NG_VALUE_ACCESSOR,
  ValidationErrors,
  Validator,
  Validators
} from '@angular/forms';
import {
  MatDialog,
  MatDialogConfig,
  MatFormFieldControl
} from '@angular/material';
import { BiblioField } from 'app/bibliographic-data/models/field-config';
import { ICGSRequestDto } from 'app/bibliographic-data/models/icgs-request-dto';
import { IntellectualPropertyDetails } from 'app/shared/models/intellectual-property-details';
import { DictionaryService } from 'app/shared/services/dictionary.service';
import { BaseDictionary } from 'app/shared/services/models/base-dictionary';
import { DictionaryType } from 'app/shared/services/models/dictionary-type.enum';
import { Workflow } from 'app/shared/services/models/workflow-model';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { Subscription } from 'rxjs/Subscription';
import { ConfirmFormDialogComponent } from '../confirm-form-dialog/confirm-form-dialog.component';
import { CreateIcgsRequestDialogComponent } from '../create-icgs-request-dialog/create-icgs-request-dialog.component';
import { IcgsFormDialogComponent } from '../icgs-form-dialog/icgs-form-dialog.component';
import { IcgsInfoFormDialogComponent } from '../icgs-info-form-dialog/icgs-info-form-dialog.component';
import { WorkflowService } from 'app/shared/services/workflow.service';
import { isStageFullExpertise, isStagesForIpc, isStageFormalExam, isStageMakingChanges, isStageRequestSeparation } from '../description/description.component';

@Component({
  selector: 'app-icgs-fields',
  templateUrl: './icgs-fields.component.html',
  styleUrls: ['./icgs-fields.component.scss'],

  // Провайдеры для контрола
  providers: [
    {
      // Интерфейс, переопределяющий поведение компонента как контрол Angular Material
      provide: MatFormFieldControl,
      useExisting: IcgsFieldsComponent
    },
    {
      provide: BiblioField,
      useExisting: IcgsFieldsComponent
    },
    {
      // Дает возможность обращения к компоненту через formControlName
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => IcgsFieldsComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => IcgsFieldsComponent),
      multi: true
    }
  ]
})
export class IcgsFieldsComponent extends BiblioField
  implements
    OnInit,
    OnChanges,
    OnDestroy,
    MatFormFieldControl<any>,
    ControlValueAccessor,
    Validator {
  static nextId = 0;
  // Событие, поднимающееся при изменении значения
  @Output()
  changed = new EventEmitter<any>();
  @Input()
  isInvalidInput = false;
  // Свойство-label
  @Input()
  get placeholder() {
    return this._placeholder;
  }
  set placeholder(plh) {
    this._placeholder = plh;
    this.stateChanges.next();
  }
  // Является ли обязательным
  @Input()
  get required() {
    return this._required;
  }
  set required(value) {
    this._required = coerceBooleanProperty(value);
    this.stateChanges.next();
  }
  // Является ли отключенным
  @Input()
  get disabled() {
    return this._disabled;
  }
  set disabled(value) {
    this._disabled = coerceBooleanProperty(value);
    this.editMode = !value;
    this.stateChanges.next();
  }
  // Занчение контрола
  @Input()
  get value() {
    if (this.formGroup) {
      const result = {
        icgsRequestDtos: this.getICGSRequestDtos(this.formGroup.value)
      };
      return result;
    }
    return null;
  }
  set value(value: any) {
    this.writeValue(value);
    this.stateChanges.next();
  }
  // уникальный идентификатор контрола, селектор компонента + айдишник
  @HostBinding()
  id = `${this.controlType}-${IcgsFieldsComponent.nextId++}`;
  // Идентификаторы вложенных контролов
  @HostBinding('attr.aria-describedby')
  describedBy = '';
  // Определяет, как контрол отображается, т.е. выходит ли он на передний план при фокусе
  @HostBinding('class.floating')
  get shouldLabelFloat() {
    return this.focused || !this.empty;
  }
  // Пусто ли значение контрола
  get empty() {
    const n = this.formGroup.getRawValue();
    return (
      !n.transliteration ||
      !n.productPlace ||
      !n.productSpecialProp ||
      !n.selectionFamily
    );
  }
  // Здесь ведем учет изменениям состояния контрола
  stateChanges = new Subject<void>();
  ngControl = null;
  focused = false;
  shouldPlaceholderFloat?: boolean;
  errorState = false;
  // Тип контрола, должен совпадать с селектором компонента
  controlType = 'app-icgs-fields-form';

  get icgsRequestIdsFormArray() {
    return this.formGroup.get('icgsRequestIdsFormArray') as FormArray;
  }
  formGroup: FormGroup;
  icgs: BaseDictionary[];

  @Input()
  editMode: boolean;
  @Input()
  owner: IntellectualPropertyDetails;
  @Input()
  changeMode: boolean;

  private onDestroy = new Subject();
  private _required = false;
  private _placeholder: string;
  private _disabled = false;
  private subs: Subscription[] = [];
  private workflows: Workflow[] = [];

  constructor(
    private fm: FocusMonitor,
    private elRef: ElementRef,
    private renderer: Renderer2,
    private dictionaryService: DictionaryService,
    private workflowService: WorkflowService,
    private dialog: MatDialog,
    private fb: FormBuilder
  ) {
    super();
    this.buildForm();
    // подписываемся на событие фокуса
    this.subs.push(
      fm.monitor(elRef.nativeElement, true).subscribe(origin => {
        this.focused = !!origin;
        this.stateChanges.next();
      })
    );
    // подписываемся на событие изменения значения
    this.subs.push(
      this.formGroup.valueChanges.subscribe((value: number) => {
        this.propagateChange(value);
      })
    );
  }

  ngOnInit() {
    this.initSelectOptions();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.owner && changes.owner.currentValue) {
      this.workflowService
        .get(this.owner.id, this.owner.ownerType)
        .takeUntil(this.onDestroy)
        .subscribe(data => (this.workflows = data));
    }
  }

  ngOnDestroy(): void {
    this.formGroup.reset();
    this.stateChanges.complete();
    this.subs.forEach(s => s.unsubscribe());
    this.onDestroy.next();
  }

  private setIcgs(icgsRequestDtos: ICGSRequestDto[]) {
    const icgsRequestIdFGs = icgsRequestDtos.map(icgsRequestDto =>
      this.getFromGroupIcgs(icgsRequestDto)
    );
    const icgsRequestIdFormArray = this.fb.array(icgsRequestIdFGs);
    icgsRequestIdFormArray.controls.sort(this.sortIcgsRequests);
    this.formGroup.setControl(
      'icgsRequestIdsFormArray',
      icgsRequestIdFormArray
    );
  }

  isDisabled(): boolean {
    return (
      !this.editMode ||
      (
        (isStageFullExpertise(this.owner) ||
        isStagesForIpc(this.owner) ||
        isStageMakingChanges(this.owner) ||
        isStageRequestSeparation(this.owner)
      ) &&
      !this.editMode)
    );
  }

  isIcgsCompareDisabled(): boolean {
    return !(
      this.editMode &&
      (
        isStageFormalExam(this.owner) ||
        isStageFullExpertise(this.owner) ||
        isStagesForIpc(this.owner)
      )
    );
  }

  isIcgsAlreadySelected(id: number): boolean {
    return this.icgsRequestIdsFormArray.value
      .map(i => i.icgsRequestId)
      .includes(id);
  }

  openDialogIcgsDelete(index: number) {
    const config = new MatDialogConfig();
    config.disableClose = true;
    config.width = '35vw';
    const dialogRef = this.dialog.open(ConfirmFormDialogComponent, config);

    dialogRef.afterClosed().subscribe((result: boolean) => {
      if (result) {
        this.onIcgsDelete(index);
      }
    });
  }

  onIcgsDelete(index: number) {
    this.icgsRequestIdsFormArray.removeAt(index);
    this.icgsRequestIdsFormArray.controls.sort(this.sortIcgsRequests);
    this.formGroup.markAsDirty();
  }

  openDialogCompareICGS(index: number) {
    const icgsRequest: ICGSRequestDto = this.getICGSRequestDto(index);
    const dicICGS: BaseDictionary = this.getDicICGS(icgsRequest.icgsId);

    const config = new MatDialogConfig();
    config.disableClose = true;
    config.width = '80vw';
    config.height = '90vh';
    config.data = {
      icgsRequest,
      dicICGS
    };
    const dialogRef = this.dialog.open(IcgsFormDialogComponent, config);
    dialogRef.afterClosed().subscribe((result: string) => {
      this.changeDescription(result, index);
    });
  }

  changeDescription(description: string, controlIndex: number) {
    if (!description) {
      return;
    }
    const value = this.formGroup.getRawValue();
    const icgsRequestInfo = value.icgsRequestIdsFormArray.map(obj => obj)[
      controlIndex
    ];
    if (icgsRequestInfo === undefined || icgsRequestInfo === null) {
      return;
    }
    const icgsRequestDto: ICGSRequestDto = new ICGSRequestDto();
    icgsRequestDto.id = icgsRequestInfo.id;
    icgsRequestDto.icgsId = icgsRequestInfo.icgsRequestId;
    icgsRequestDto.claimedDescription = icgsRequestInfo.icgsClaimedDescription;
    icgsRequestDto.claimedDescriptionEn =
      icgsRequestInfo.icgsClaimedDescriptionEn;
    icgsRequestDto.description = description;
    icgsRequestDto.isRefused = icgsRequestInfo.icgsIsRefused;
    icgsRequestDto.isPartialRefused = icgsRequestInfo.icgsIsPartialRefused;
    icgsRequestDto.reasonForPartialRefused =
      icgsRequestInfo.icgsReasonForPartialRefused;
    this.replaceICGSControl(controlIndex, icgsRequestDto);
  }

  icgsInsert(controlIndex: number, icgsRequestDto: ICGSRequestDto) {
    this.icgsRequestIdsFormArray.insert(
      controlIndex,
      this.getFromGroupIcgs(icgsRequestDto)
    );
    this.formGroup.markAsDirty();
  }

  getFromGroupIcgs(icgsRequestDto: ICGSRequestDto): AbstractControl {
    return this.fb.group({
      id: [icgsRequestDto.id],
      icgsRequestId: [icgsRequestDto.icgsId, Validators.required],
      icgsClaimedDescription: [icgsRequestDto.claimedDescription],
      icgsClaimedDescriptionEn: [icgsRequestDto.claimedDescriptionEn],
      icgsDescription: [icgsRequestDto.description],
      icgsIsRefused: [icgsRequestDto.isRefused],
      icgsIsPartialRefused: [icgsRequestDto.isPartialRefused],
      icgsReasonForPartialRefused: [icgsRequestDto.reasonForPartialRefused]
    });
  }

  getICGSRequestDto(controlIndex: number): ICGSRequestDto {
    const value = this.formGroup.getRawValue();

    const icgsInfo = value.icgsRequestIdsFormArray.map(obj => obj)[
      controlIndex
    ];
    const icgsRequestDto = new ICGSRequestDto();
    icgsRequestDto.id = icgsInfo && icgsInfo.id;
    icgsRequestDto.icgsId = icgsInfo && icgsInfo.icgsRequestId;
    icgsRequestDto.claimedDescription = icgsInfo && icgsInfo.icgsClaimedDescription;
    icgsRequestDto.claimedDescriptionEn = icgsInfo && icgsInfo.icgsClaimedDescriptionEn;
    icgsRequestDto.description = icgsInfo && icgsInfo.icgsDescription;
    icgsRequestDto.isRefused = icgsInfo && icgsInfo.icgsIsRefused;
    icgsRequestDto.isPartialRefused = icgsInfo && icgsInfo.icgsIsPartialRefused;
    icgsRequestDto.reasonForPartialRefused =
      icgsInfo && icgsInfo.icgsReasonForPartialRefused;
    return icgsRequestDto;
  }

  getDicICGS(id: number): BaseDictionary {
    return this.icgs.find(x => x.id === id);
  }

  openDialogViewRequestICGS(index: number, isEditing = false) {
    const icgsRequest: ICGSRequestDto = this.getICGSRequestDto(index);
    const dicICGS: BaseDictionary = this.getDicICGS(icgsRequest.icgsId);
    const config = new MatDialogConfig();
    config.disableClose = false;
    config.width = '60vw';
    config.height = '89vh';
    config.data = {
      icgsRequest,
      dicICGS,
      isStageFullExpertise: isStageFullExpertise(this.owner),
      isEdit: this.editMode,
      canEdit: isEditing // isStageFormalExam(this.owner) || this.changeMode МТКУ можно редактировать на любом этапе
    };
    const dialogRef = this.dialog.open(IcgsInfoFormDialogComponent, config);

    dialogRef.afterClosed().subscribe(result => {
      if (!result) {
        return;
      }
      if (this.editMode) {
        icgsRequest.isRefused = result.isRefused;
        icgsRequest.isPartialRefused = result.isPartialRefused;
        icgsRequest.reasonForPartialRefused = result.reasonForPartialRefused;
        this.replaceICGSControl(index, icgsRequest);
        if (isEditing) {
          this.getDescriptionForICGS();
        }
      }
    });
  }

  openDialogInputData() {
    const config = new MatDialogConfig();
    config.disableClose = true;
    config.width = '35vw';
    config.data = {
      icgs: this.icgs,
      selectedIcgs: this.icgsRequestIdsFormArray.value.map(
        i => i.icgsRequestId
      ),
      isStageFullExpertise: isStageFullExpertise(this.owner)
    };
    const dialogRef = this.dialog.open(
      CreateIcgsRequestDialogComponent,
      config
    );
    dialogRef.afterClosed().subscribe((result: any) => {
      if (!result) {
        return;
      }
      const icgsRequestDto: ICGSRequestDto = new ICGSRequestDto();
      icgsRequestDto.id = null;
      icgsRequestDto.icgsId = result.icgsId;
      icgsRequestDto.claimedDescription = result.claimedDescription;
      icgsRequestDto.description = result.claimedDescription;
      icgsRequestDto.claimedDescriptionEn = result.claimedDescriptionEn;
      this.onIcgsAdd(icgsRequestDto);
    });
  }

  onIcgsAdd(icgsRequestDto: ICGSRequestDto) {
    if (!icgsRequestDto.icgsId) {
      const usedIcgs = this.icgsRequestIdsFormArray.value.map(
        i => i.icgsRequestId
      );
      icgsRequestDto.icgsId = this.icgs.filter(
        i => !usedIcgs.includes(i.id)
      )[0].id;
    }
    this.icgsRequestIdsFormArray.push(this.getFromGroupIcgs(icgsRequestDto));
    this.icgsRequestIdsFormArray.controls.sort(this.sortIcgsRequests);
    this.formGroup.markAsDirty();
    this.getDescriptionForICGS();
  }

  getICGSRequestDtos(formGroupValues: any): ICGSRequestDto[] {
    const icgsRequestDtos: ICGSRequestDto[] = [];
    for (
      let i = 0;
      i <
      formGroupValues.icgsRequestIdsFormArray.map(obj => obj.icgsRequestId)
        .length;
      i++
    ) {
      const icgsRequestDto: ICGSRequestDto = new ICGSRequestDto();
      icgsRequestDto.id = formGroupValues.icgsRequestIdsFormArray.map(
        obj => obj.id
      )[i];
      icgsRequestDto.icgsId = formGroupValues.icgsRequestIdsFormArray.map(
        obj => obj.icgsRequestId
      )[i];
      icgsRequestDto.claimedDescription = formGroupValues.icgsRequestIdsFormArray.map(
        obj => obj.icgsClaimedDescription
      )[i];
      icgsRequestDto.claimedDescriptionEn = formGroupValues.icgsRequestIdsFormArray.map(
        obj => obj.icgsClaimedDescriptionEn
      )[i];
      icgsRequestDto.description = formGroupValues.icgsRequestIdsFormArray.map(
        obj => obj.icgsDescription
      )[i];
      icgsRequestDto.isRefused = formGroupValues.icgsRequestIdsFormArray.map(
        obj => obj.icgsIsRefused
      )[i];
      icgsRequestDto.isPartialRefused = formGroupValues.icgsRequestIdsFormArray.map(
        obj => obj.icgsIsPartialRefused
      )[i];
      icgsRequestDto.reasonForPartialRefused = formGroupValues.icgsRequestIdsFormArray.map(
        obj => obj.icgsReasonForPartialRefused
      )[i];
      icgsRequestDtos.push(icgsRequestDto);
    }
    return icgsRequestDtos;
  }

  setDescribedByIds(ids: string[]): void {
    this.describedBy = ids.join(' ');
  }

  onContainerClick(event: MouseEvent): void {
    if ((event.target as Element).tagName.toLowerCase() !== 'input') {
      this.elRef.nativeElement.querySelector('input').focus();
    }
  }

  writeValue(obj: ICGSRequestDto[]): void {
    if (obj) {
      if (this.formGroup) {
        this.setIcgs(obj);
      }
    } else {
      if (this.formGroup) {
        this.formGroup.reset();
        this.changed.emit(obj);
        this.propagateChange(obj);
      }
    }
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

  validate(c: AbstractControl): ValidationErrors {
    const invalid = {
      invalid: {
        valid: false
      }
    };
    if (!this.formGroup) {
      return invalid;
    }
    return this.formGroup.valid ? null : invalid;
  }

  registerOnValidatorChange?(fn: () => void): void {
    return;
  }

  getValue() {
    return this.value;
  }

  isRejectionChecked(): boolean {
    return this.getICGSRequestDtos(this.formGroup.value).every(
      i => i.isRefused
    );
  }

  isPartialRejectionChecked(): boolean {
    const icgs = this.getICGSRequestDtos(this.formGroup.value);
    return (
      icgs.some(i => i.isRefused || i.isPartialRefused) &&
      !icgs.every(i => i.isRefused)
    );
  }

  isRejectionStateVisible(): boolean {
    // return (
    //   this.icgsRequestIdsFormArray.controls.length > 0 &&
    //   this.workflows.some(w => w.currentStageCode === 'TM03.3.2_2')
    // );
    return true;
  }

  private sortIcgsRequests(a: any, b: any): number {
    const firstFormGroup = a as FormGroup;
    const secondFormGroup = b as FormGroup;
    const firstIcgsId = firstFormGroup.controls.icgsRequestId.value;
    const secondIcgsId = secondFormGroup.controls.icgsRequestId.value;
    if (firstIcgsId > secondIcgsId) {
      return 1;
    } else if (firstIcgsId < secondIcgsId) {
      return -1;
    } else {
      return 0;
    }
  }

  private replaceICGSControl(
    controlIndex: number,
    icgsRequestDto: ICGSRequestDto
  ) {
    this.onIcgsDelete(controlIndex);
    this.icgsInsert(controlIndex, icgsRequestDto);
  }

  private initSelectOptions() {
    Observable.combineLatest(
      this.dictionaryService.getBaseDictionary(DictionaryType.DicICGS)
    )
      .takeUntil(this.onDestroy)
      .subscribe(([icgs]) => {
        this.icgs = icgs;
        this.getDescriptionForICGS();
        if (this.owner) {
          this.setIcgs(this.owner.icgsRequestDtos);
        }
      });
  }
  private getDescriptionForICGS() {
    this.icgs.forEach((item) => {
      if (this.isIcgsAlreadySelected(item.id)) {
        const usedClasses = this.formGroup.get('icgsRequestIdsFormArray') as FormArray;
        const classIndex = usedClasses.value.findIndex(classes => classes.icgsRequestId === item.id);
        item.descriptionField = usedClasses.value[classIndex].icgsDescription;
      }
    });
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      icgsRequestIdsFormArray: this.fb.array([])
    });
  }

  private propagateChange = (_: any) => {};
}
