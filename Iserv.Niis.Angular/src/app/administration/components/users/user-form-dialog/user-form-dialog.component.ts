import { Component, Inject, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import {
  MAT_DIALOG_DATA,
  MatDialog,
  MatDialogConfig,
  MatDialogRef,
  MatSelect,
  MatSelectionListChange,
} from '@angular/material';
import { TreeNode } from 'primeng/components/common/treenode';
import { Subject } from 'rxjs/Rx';

import { ConfigService } from '../../../../core';
import { SnackBarHelper } from '../../../../core/snack-bar-helper.service';
import { CustomerService } from '../../../../shared/services/customer.service';
import { DictionaryService } from '../../../../shared/services/dictionary.service';
import { BaseDictionary, DicDepartment, DicPosition } from '../../../../shared/services/models/base-dictionary';
import { CustomerShortInfo } from '../../../../shared/services/models/customer-short-info';
import { DictionaryType } from '../../../../shared/services/models/dictionary-type.enum';
import { SelectOption } from '../../../../shared/services/models/select-option';
import { RolesService } from '../../../roles.service';
import { TreeFormDialogIpcComponent } from '../tree-form-dialog-ipc/tree-form-dialog-ipc.component';

@Component({
  selector: 'app-user-form-dialog',
  templateUrl: './user-form-dialog.component.html',
  styles: [`
    .mat-dialog-content { max-height: 100%}

    .mat-selection-list: { overflow-y: auto;}

    h5 {
      margin-bottom: 0;
      padding-left: 16px;
    }
  `]
})
export class UserFormDialogComponent implements OnInit, OnDestroy {
  formGroup: FormGroup;
  divisionOptions: SelectOption[];
  departmentOptions: SelectOption[];
  positionOptions: SelectOption[];
  roleOptions: SelectOption[] = [];
  icgsOptions: BaseDictionary[];
  selectedRoleIds: number[];
  selectedIcgsIds: number[];
  showMktu = false;

  ipcTree: TreeNode[] = [];
  labelsIpc: string[] = [];

  @ViewChild('divisionId') divisionSelect: MatSelect;
  @ViewChild('departmentId') departmentSelect: MatSelect;
  private onDestroy = new Subject();

  constructor(
    private dialogRef: MatDialogRef<UserFormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private data: any,
    private fb: FormBuilder,
    private configService: ConfigService,
    private customerService: CustomerService,
    private rolesService: RolesService,
    private dictionaryService: DictionaryService,
    private dialog: MatDialog,
    private snackbarHelper: SnackBarHelper
  ) {
    this.initForm();
  }

  ngOnInit() {
    if (this.data.model) {
      this.formGroup.patchValue(this.data.model);
      this.onDivisionChange(this.data.model.divisionId);
      this.onDepartmentChange(this.data.model.departmentId);
      // TODO сейчас в системе ИИН имеет некорректный формат. Временно разрешено редактирование для существующих пользователей
      // this.formGroup.get('xin').disable();
    } else {
      this.formGroup.get('password').setValidators(Validators.required);
    }
    this.setSubscriptions();
    this.initialize();
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  onCancel(): void {
    this.dialogRef.close(null);
  }

  onSubmit(): void {
    if (this.formGroup.invalid) {
      return;
    }
    this.dialogRef.close(this.formGroup.getRawValue());
  }

  onIcgsSelectionChanged(event: MatSelectionListChange): void {
    const icgsIds = event.source
      .selectedOptions
      .selected
      .map(s => s.value);

    this.formGroup.patchValue({ icgsIds });
  }

  onRolesSelectionChanged(event: MatSelectionListChange): void {
    const roleIds = event.source
      .selectedOptions
      .selected
      .map(s => s.value);
    this.formGroup.patchValue({ roleIds });

    const selectedRoles = this.roleOptions.filter(r => roleIds.includes(r.id));
    this.checkMktu(selectedRoles)
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
   * Инициализация формы
   *
   * @private
   * @memberof UserFormComponent
   */
  private initForm() {
    this.formGroup = this.fb.group({
      id: [''],
      // TODO сейчас в системе ИИН имеет некорректный формат. Временно разрешено редактирование для существующих пользователей
      xin: ['', [Validators.required, Validators.minLength(12), Validators.maxLength(12)]], // , xinValidator
      nameRu: [{ value: '', disabled: true }, [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.minLength(6)]],
      customerId: [''],
      divisionId: ['', [Validators.required]],
      departmentId: ['', [Validators.required]],
      positionId: ['', [Validators.required]],
      roleIds: ['', [Validators.required]],
      icgsIds: [''],
      isLocked: [''],
      ipcIds: [''],
      certPassword: [''],
      certStoragePath: [''],
      positionTypeNameRu: [''],
      positionTypeCode: ['']
    });
  }

  private setSubscriptions(): void {

    this.divisionSelect.change
      .subscribe((component) => {
        this.departmentOptions = [];
        this.positionOptions = [];
        this.formGroup.get('departmentId').setValue('');
        this.formGroup.get('positionId').setValue('');
        this.onDivisionChange(component.value);
      });
    this.departmentSelect.change
      .subscribe((component) => {
        this.positionOptions = [];
        this.formGroup.get('positionId').setValue('');
        this.onDepartmentChange(component.value);
      });
    this.subscribeXinInput();
  }

  private checkMktu(options: any): void {
    const mktuRoles = options.filter(r =>
      r.code === 'EXPERT_OF_PRELIMINARY_/_FORMAL_EXPERTISE'
      || r.code === 'EXPERT_OF_FULL_/_IN_ESSENCE_EXPERTISE');
    this.showMktu = mktuRoles.length > 0
  }

  /**
   * Подписывается на события изменения значения в поле XIN и запрашивает данные с бэкенда
   *
   * @private
   * @memberof UserFormComponent
   */
  private subscribeXinInput() {
    const xinControl = this.formGroup.controls.xin;
    xinControl.valueChanges
      .takeUntil(this.onDestroy)
      .debounceTime(this.configService.debounceTime)
      .filter(value => value && value.toString().length === 12 && xinControl.valid && xinControl.dirty)
      .distinctUntilChanged()
      .subscribe((value: number) => {
        this.customerService.getByXin(value.toString())
          .takeUntil(this.onDestroy)
          .subscribe((customerInfo: CustomerShortInfo) => {
            if (!customerInfo) {
              this.snackbarHelper.success('Пользователь с таким БИН/ИИН не найден');
              return;
            }
            this.formGroup.controls['customerId'].setValue(customerInfo.id);
            this.formGroup.controls['nameRu'].setValue(customerInfo.nameRu);
            this.formGroup.markAsDirty();
          }, (err) => {
            this.formGroup.controls['xin'].setErrors({ 'xin': err.message });
            this.formGroup.controls['customerId'].setValue('');
            this.formGroup.controls['nameRu'].setValue('');
          });
      });
  }

  getError(formControl: FormControl) {
    const errors = formControl.errors;
    return Object.keys(errors).map(key => {
      const value = errors[key];
      if (typeof (value) === 'boolean') {
        return 'Please enter a valid xin';
      }
      return value;
    }).join('\n');
  }

  /**
   * Подписывается на события изменения Филиала и запрашивает список служб
   *
   * @param {number} selectedDivisionId - id Филиала (divisionId)
   * @memberof UserFormComponent
   */
  onDivisionChange(selectedDivisionId: number) {
    this.dictionaryService
      .getBaseDictionary(DictionaryType.DicDepartment)
      .subscribe((dicDepartment: DicDepartment[]) => {
        this.departmentOptions = dicDepartment
          .filter(x => x.divisionId === selectedDivisionId);
      });
  }

  onDepartmentChange(selectedDepartmentId: number) {
    this.dictionaryService
      .getBaseDictionary(DictionaryType.DicPosition)
      .subscribe((dicPosition: DicPosition[]) => {
        this.positionOptions = dicPosition
          .filter(p => p.departmentId === selectedDepartmentId);
      })
  }

  /**
   * Инициализирует выподающие списки первоначальными значениями
   *
   * @memberof UserFormComponent
   */
  private initialize() {
    this.dictionaryService
      .getSelectOptions(DictionaryType.DicDivision)
      .subscribe(data => this.divisionOptions = data);
    this.dictionaryService
      .getBaseDictionary(DictionaryType.DicICGS)
      .subscribe(data => {
        this.icgsOptions = data;
        if (this.data.model) {
          this.selectedIcgsIds = this.data.model.icgsIds;
        }
      });
    this.dictionaryService.getDicIpcRoots()
      .subscribe(ipcTree => {
        this.ipcTree = ipcTree;
        this.setLabelsIpc();
      });
    this.rolesService.getSelectOptions()
      .subscribe((data: SelectOption[]) => {
        this.roleOptions = data;
        if (this.data.model) {
          const res = data.filter(r => this.data.model.roleIds.includes(r.id));
          this.checkMktu(res);
          this.selectedRoleIds = this.data.model.roleIds;
        }
      });
  }


  openDialogIpcTree() {
    const config = new MatDialogConfig();
    config.disableClose = true;
    config.width = '80vw';
    config.height = '90vh';
    const ipcIds = this.formGroup.controls['ipcIds'].value;
    config.data = {
      tree: this.ipcTree,
      ids: ipcIds
    };

    const dialogRef = this.dialog.open(TreeFormDialogIpcComponent, config);
    dialogRef.afterClosed()
      .subscribe((result: any) => {
        if (result) {
          this.formGroup.controls['ipcIds'].setValue(result);
          this.setLabelsIpc();
          this.formGroup.markAsDirty();
        }
      });
  }
  private setLabelsIpc() {
    this.labelsIpc = [];
    const ipcIds = this.formGroup.controls['ipcIds'].value;
    if (ipcIds && ipcIds.length) {
      this.initialIpcLabelsByIds(ipcIds);
    }
  }
  private initialIpcLabelsByIds(ids: number[]) {
    this.dictionaryService.getDicIpcs(ids)
      .subscribe(nodes => {
        for (const node of nodes) {
          this.labelsIpc.push(node.label);
        }
      });
  }
}
