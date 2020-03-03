import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { DictionaryService } from '../../../shared/services/dictionary.service';
import { Subject } from 'rxjs';
import { DictionaryType } from '../../../shared/services/models/dictionary-type.enum';
import { BaseDictionary } from '../../../shared/services/models/base-dictionary';
import { ReportConditionData } from '../../models/report-condition-data';
import { moment } from '../../../shared/shared.module';

@Component({
  selector: 'app-report-filter',
  templateUrl: './report-filter.component.html',
  styleUrls: ['./report-filter.component.scss']
})
export class ReportFilterComponent implements OnInit, OnDestroy {
  formGroup: FormGroup;
  dicPdTypes: BaseDictionary[];
  selectedProtectionDocTypes: BaseDictionary[] = [];
  private onDestroy = new Subject();

  @Input() reportCode: string;

  constructor(
    private fb: FormBuilder,
    private dictionaryService: DictionaryService,
  ) {
    this.buildForm();
  }

  ngOnInit() {
    this.dictionaryService.getBaseDictionary(DictionaryType.DicProtectionDocType)
      .takeUntil(this.onDestroy)
      .subscribe(pdTypes => this.dicPdTypes = pdTypes);
  }

  ngOnDestroy(): void {
    this.formGroup.reset();
    this.onDestroy.next();
  }

  buildForm() {
    this.formGroup = this.fb.group({
      reportCode: [''],
      dateFrom: [''],
      dateTo: [''],
      pdTypeSearchText: ['']
    });
  }

  getConditions(): ReportConditionData {
    this.formGroup.get('reportCode').setValue(this.reportCode);
    const condition = new ReportConditionData();
    const formValue = this.formGroup.getRawValue();
    delete formValue.pdTypeSearchText;
    Object.assign(condition, formValue);
    if (condition.dateTo && condition.dateTo.hours() !== 23) {
      condition.dateTo.add('hours', 23);
      condition.dateTo.add('minutes', 59);
      condition.dateTo.add('seconds', 59);
      condition.dateTo.add('milliseconds', 59);
    }
    condition.protectionDocTypeIds = this.selectedProtectionDocTypes.map(pdt => pdt.id);
    return condition;
  }

  onAddPdType(event: any): void {
    const value: BaseDictionary = event.option.value;
    if (value) {
      this.selectedProtectionDocTypes.push(value);
      this.formGroup.markAsDirty();
    }
  }

  onRemovePdType(pdType: any): void {
    const index = this.selectedProtectionDocTypes.indexOf(pdType);

    if (index >= 0) {
      this.selectedProtectionDocTypes.splice(index, 1);
      this.formGroup.markAsDirty();
    }
  }
}
