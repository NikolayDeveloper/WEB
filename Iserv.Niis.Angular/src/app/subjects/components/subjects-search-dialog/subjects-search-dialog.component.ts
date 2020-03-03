import { Component, Inject } from '@angular/core';
import { OnDestroy } from '@angular/core/src/metadata/lifecycle_hooks';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { Config } from '../../../shared/components/table/config.model';
import { DictionaryService } from '../../../shared/services/dictionary.service';
import { DictionaryType } from '../../../shared/services/models/dictionary-type.enum';
import { SelectOption } from '../../../shared/services/models/select-option';
import { SubjectDto } from '../../models/subject.model';
import { SubjectsService } from '../../services/subjects.service';

const availableCustomerTypeCodes = ['1', '2', 'NR'];

@Component({
  selector: 'app-subjects-search-dialog',
  templateUrl: './subjects-search-dialog.component.html',
  styleUrls: ['./subjects-search-dialog.component.scss']
})
export class SubjectsSearchDialogComponent implements OnDestroy {
  subjects: SubjectDto[];
  dicCustomerTypes: SelectOption[];
  columns: Config[] = [
    new Config({ columnDef: 'id', header: 'ID', class: 'width-75' }),
    new Config({ columnDef: 'xin', header: 'ИИН/БИН', class: 'width-100' }),
    new Config({ columnDef: 'nameRu', header: 'Наименование на русском', class: 'width-300' }),
    new Config({ columnDef: 'nameKz', header: 'Наименование на казахском', class: 'width-300' }),
    new Config({ columnDef: 'nameEn', header: 'Наименование на английском', class: 'width-300' }),
    new Config({ columnDef: 'address', header: 'Адрес', class: 'width-250' }),
    new Config({ columnDef: 'typeNameRu', header: 'Тип контрагента', class: 'width-150' })
  ];

  canCreate = false;
  formGroup: FormGroup;
  queryData = void 0;
  isPatentAttorney = false;
  selectedCustomer: SubjectDto;
  get source() {
    return this.subjectsService;
  }
  reset = new Subject<any>();
  query: any;

  private customers: SubjectDto[] = [];
  private onDestroy = new Subject();

  constructor(
    private fb: FormBuilder,
    private subjectsService: SubjectsService,
    private dialogRef: MatDialogRef<SubjectsSearchDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private data: any,
    private dictionaryService: DictionaryService
  ) {
    this.buildForm();
    if (data.isPatentAttorney) {
      this.isPatentAttorney = data.isPatentAttorney;
    }

    if (this.isPatentAttorney) {
      this.columns = [
        ...this.columns,
        new Config({
          columnDef: 'powerAttorneyFullNum',
          header: 'Номер патентный поверенного'
        })
      ];
    }
    this.initSelectOptions();
  }
  private initSelectOptions() {
    Observable.combineLatest(
      this.dictionaryService.getSelectOptions(DictionaryType.DicCustomerType)
    )
      .takeUntil(this.onDestroy)
      .subscribe(([customerTypes]) => {
        this.dicCustomerTypes = customerTypes.filter(ct =>
          availableCustomerTypeCodes.includes(ct.code)
        );
      });
  }
  ngOnDestroy(): void {
    this.customers = [];
    this.onDestroy.next();
  }

  onSubmit() {
    this.dialogRef.close(this.selectedCustomer);
  }

  onSearchCustomersClick() {
    this.queryData = [
      {
        key: 'id',
        value: this.formGroup.get('inputId').value
      },
      {
        key: 'xin',
        value: this.formGroup.get('inputXin').value
      },
      {
        key: 'name',
        value: this.formGroup.get('inputName').value
      },
      {
        key: 'powerAttorneyFullNum',
        value: this.formGroup.get('inputPowerAttorneyNumber').value
      },
      {
        key: 'isPatentAttorney',
        value: this.isPatentAttorney
      },
      {
        key: 'customerTypeId',
        value: this.formGroup.get('customerTypeId').value
      }
    ];
    this.reset.next(this.queryData);
  }

  onCustomerSelect(row: SubjectDto) {
    this.selectedCustomer = row;
  }

  onCreateCustomerClick() {
    this.dialogRef.close('create');
  }

  onCancel() {
    this.dialogRef.close();
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      inputId: [''],
      inputXin: [''],
      inputName: [''],
      inputPowerAttorneyNumber: [''],
      customerTypeId: ['']
    });
  }
}
