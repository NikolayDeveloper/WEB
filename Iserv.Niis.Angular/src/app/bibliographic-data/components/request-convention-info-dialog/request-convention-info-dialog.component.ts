import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatOptionSelectionChange } from '@angular/material';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { SelectOption } from '../../../shared/services/models/select-option';
import { RequestConventionInfo } from '../../models/request-convention-info';


@Component({
  selector: 'app-request-convention-info-dialog',
  templateUrl: './request-convention-info-dialog.component.html',
  styleUrls: ['./request-convention-info-dialog.component.scss']
})
export class RequestConventionInfoDialogComponent implements OnInit {
  formGroup: FormGroup;
  requestConventionInfo: RequestConventionInfo;
  countries: SelectOption[] = [];
  earlyRegTypes: SelectOption[] = [];
  editableControls: string[] = [];
  filteredDicCountries: Observable<SelectOption[]>;
  constructor(
    public dialogRef: MatDialogRef<RequestConventionInfoDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fb: FormBuilder,
  ) {
    this.buildForm();
  }

  ngOnInit() {
    this.countries = this.data.countries;
    this.earlyRegTypes = this.data.earlyRegTypes;

    if (this.data.requestConventionInfo) {
      this.formGroup.reset(this.data.requestConventionInfo);
      this.formGroup.get('countryId').setValue(this.data.requestConventionInfo.countryId);
    } else {
      this.formGroup.controls['countryId'].setValue(this.countries[0].id);
      this.formGroup.controls['earlyRegTypeId'].setValue(this.earlyRegTypes[0].id);
    }


    if (this.data.isEdit) {
      this.editableControls.forEach(c => {
        this.formGroup.controls[c].enable();
      });
    }

    /*this.filteredDicCountries = this.formGroup.get('countryId').valueChanges
      .startWith('')
      .map(
      selectOption =>
        selectOption && typeof selectOption === 'object'
          ? selectOption.nameRu
          : selectOption
      )
      .map(val => {
        return val ? this.countries.filter(s => s.nameRu && s.nameRu.toLowerCase().indexOf(val.toLowerCase()) === 0)
          : this.countries;
      });*/
  }
  private buildForm() {
    this.formGroup = this.fb.group({
      id: [{ value: '' }],
      countryId: [{ value: '', disabled: true }, Validators.required],
      earlyRegTypeId: [{ value: '', disabled: true }, Validators.required],
      dateInternationalApp: [{ value: '', disabled: true }, Validators.required],
      headIps: [{ value: '', disabled: true }],
      termNationalPhaseFirsChapter: [{ value: '', disabled: true }],
      termNationalPhaseSecondChapter: [{ value: '', disabled: true }],
      regNumberInternationalApp: [{ value: '', disabled: true }, Validators.required],
    });
    this.editableControls = ['countryId', 'earlyRegTypeId', 'dateInternationalApp',
      'termNationalPhaseFirsChapter', 'termNationalPhaseSecondChapter', 'headIps', 'regNumberInternationalApp'];
  }
  onOk(): void {
    if (this.formGroup.invalid) { return; }
    if (this.data.isEdit) {
      const value = this.formGroup.getRawValue();
      this.requestConventionInfo = new RequestConventionInfo(value);
      this.dialogRef.close(this.requestConventionInfo);
    } else {
      this.dialogRef.close(null);
    }

  }

  displayFn(option: SelectOption): string {
    return option ? option.nameRu : '';
  }
  onCancel(): void {
    this.dialogRef.close(null);
  }
}
