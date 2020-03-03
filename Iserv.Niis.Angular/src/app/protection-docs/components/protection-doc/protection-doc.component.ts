import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  SimpleChanges
} from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material';
import { Subject } from 'rxjs/Subject';
import { Config } from '../../../shared/components/table/config.model';
import { UndoDialogComponent } from '../../../shared/components/undo-dialog/undo-dialog.component';
import { DictionaryType } from '../../../shared/services/models/dictionary-type.enum';
import { OwnerType } from '../../../shared/services/models/owner-type.enum';
import { SelectOption } from '../../../shared/services/models/select-option';
import { WorkflowService } from '../../../shared/services/workflow.service';
import { AddresseeInfo } from '../../../subjects/components/subjects-search-form/subjects-search-form.component';
import { SubjectDto } from '../../../subjects/models/subject.model';
import { ProtectionDocDetails } from '../../models/protection-doc-details';
import { WorkflowBusinessService } from '../../services/workflow-business.service';
import { RequestPart } from '../../../requests/services/workflow-business.service';
import { JournalService } from 'app/journal/journal.service';

@Component({
  selector: 'app-protection-doc',
  templateUrl: './protection-doc.component.html',
  styleUrls: ['./protection-doc.component.scss']
})
export class ProtectionDocComponent implements OnInit, OnChanges, OnDestroy {
  workflows: any[];
  columns: Config[] = [
    new Config({ columnDef: 'routeNameRu', header: 'Маршрут', class: 'width-100' }),
    new Config({ columnDef: 'currentUserNameRu', header: 'Пользователь', class: 'width-100' }),
    new Config({ columnDef: 'dateCreate', header: 'Дата', class: 'width-200' }),
    new Config({ columnDef: 'currentStageNameRu', header: 'Этап', class: 'width-100' }),
    new Config({ columnDef: 'description', header: 'Описание', class: 'width-200' })
  ];
  formGroup: FormGroup;
  dicPDTypes: SelectOption[];
  dicSendTypes: SelectOption[];
  editMode = false;
  editableControlsCreate: string[];
  editableControlsDefault: string[];
  editableControlsStageInitial: string[];
  editableControlsStageFormationAppData: string[];
  availableAtStage: any;
  selectedSubject: SubjectDto;
  isDisabled = false;

  @Input() protectionDocDetails: ProtectionDocDetails;
  @Input() selectOptions: SelectOption[];
  @Output() submitData: EventEmitter<any> = new EventEmitter();
  @Output() edit: EventEmitter<boolean> = new EventEmitter();
  @Output() delete: EventEmitter<number> = new EventEmitter();

  private onDestroy = new Subject();

  constructor(
    private fb: FormBuilder,
    private dialog: MatDialog,
    private workflowService: WorkflowService,
    private workflowBusinessService: WorkflowBusinessService,
    private journalService: JournalService
  ) {
    this.buildForm();
  }

  ngOnInit() {
    this.formGroup.markAsDirty();
  }

  ngOnDestroy(): void {
    this.formGroup.reset();
    this.toggleEditMode(false);
    this.onDestroy.next();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (
      changes.protectionDocDetails &&
      changes.protectionDocDetails.currentValue
    ) {
      this.formGroup.reset(this.protectionDocDetails);
      this.setCustomerSearchValue();
      const newProtectionDoc = this.protectionDocDetails.id === 0;
      this.toggleEditMode(newProtectionDoc);
      if (!newProtectionDoc) {
        this.refreshWorkflows();
      }
      this.availableAtStage = this.workflowBusinessService.availableAtStage(
        this.protectionDocDetails,
        RequestPart.ProtectionDoc
      );
    }

    if (changes.selectOptions && changes.selectOptions.currentValue) {
      this.initSelectOptions();
    }
  }

  onUndo() {
    if (this.formGroup.dirty) {
      this.openDialog();
      return false;
    } else {
      this.formGroup.reset(this.protectionDocDetails);
      this.toggleEditMode(false);
      this.setCustomerSearchValue();
    }
  }

  onEdit() {
    this.toggleEditMode(true);
  }

  onSubmit() {
    if (this.formGroup.invalid) {
      return;
    }
    this.formGroup.markAsPristine();

    const value = this.formGroup.getRawValue();
    delete value.customerSearch;
    this.setAddresseId(value);
    this.submitData.emit(value);
  }

  refreshWorkflows() {
    this.workflowService
      .get(this.protectionDocDetails.id, OwnerType.ProtectionDoc)
      .takeUntil(this.onDestroy)
      .subscribe(workflows => (this.workflows = workflows));
  }

  formatDate(param: Date): string {
    const date = new Date(param);
    return `${date.toLocaleDateString()} ${date.toLocaleTimeString()}`;
  }

  onPreviewClick(): void {
    this.journalService
      .getAttachment(OwnerType.ProtectionDoc, this.protectionDocDetails.id)
      .takeUntil(this.onDestroy)
      .subscribe(blob => {
        window.open(window.URL.createObjectURL(blob));
      });
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      typeId: [{ value: '', disabled: true }],
      sendTypeId: [{ value: '', disabled: true }],
      customerSearch: [{ value: '', disabled: true }],
      dateCreate: [{ value: '', disabled: true }],
      pageCount: [{ value: '', disabled: true }],
      nameRu: [{ value: '', disabled: true }],
      nameKz: [{ value: '', disabled: true }],
      nameEn: [{ value: '', disabled: true }],
      description: [{ value: '', disabled: true }],
      gosNumber: [{ value: '', disabled: true }],
      barcode: [{ value: '', disabled: true }],
      gosDate: [{ value: '', disabled: true }],
      outgoingNumber: [{ value: '', disabled: true }],
      outgoingDate: [{ value: '', disabled: true }]
    });
  }

  private openDialog() {
    const dialogRef = this.dialog.open(UndoDialogComponent);

    dialogRef
      .afterClosed()
      .takeUntil(this.onDestroy)
      .subscribe(result => {
        if (result === 'true') {
          this.toggleEditMode(false);
          this.formGroup.reset(this.protectionDocDetails);
          this.setCustomerSearchValue();
        }
      });
  }

  private toggleEditMode(value: boolean) {
    this.editMode = value;
    value ? this.formGroup.get('sendTypeId').enable() : this.formGroup.get('sendTypeId').disable();
    this.edit.emit(value);
  }

  private initSelectOptions() {
    this.dicPDTypes = this.selectOptions.filter(
      so => so.dicType === DictionaryType.DicProtectionDocType
    );
    this.dicSendTypes = this.selectOptions.filter(
      so => so.dicType === DictionaryType.DicSendType
    );
  }

  private setCustomerSearchValue() {
    if (this.protectionDocDetails && this.protectionDocDetails.addressee) {
      const addresseeInfo = new AddresseeInfo();
      addresseeInfo.addresseeId = this.protectionDocDetails.addressee.id;
      addresseeInfo.addresseeShortAddress = this.protectionDocDetails.addressee.shortAddress;
      addresseeInfo.addresseeAddress = this.protectionDocDetails.addressee.address;
      addresseeInfo.addresseeNameRu = this.protectionDocDetails.addressee.nameRu;
      addresseeInfo.addresseeXin = this.protectionDocDetails.addressee.xin;
      addresseeInfo.apartment = this.protectionDocDetails.addressee.apartment;
      this.formGroup.get('customerSearch').setValue(addresseeInfo);
    }
  }

  private setAddresseId(value: any) {
    const addresseeValue = this.formGroup.get('customerSearch').value;
    value.addresseeId = addresseeValue.addresseeId;
  }
}
