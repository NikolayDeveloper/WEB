import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators
} from '../../../../../node_modules/@angular/forms';
import {
  MatDialogRef,
  MatSelectionListChange,
  MAT_DIALOG_DATA
} from '../../../../../node_modules/@angular/material';
import { Subject } from '../../../../../node_modules/rxjs/Subject';
import { DictionaryService } from '../../../shared/services/dictionary.service';
import { DictionaryType } from '../../../shared/services/models/dictionary-type.enum';
import { SelectOption } from '../../../shared/services/models/select-option';
import { BaseDictionary } from '../../../shared/services/models/base-dictionary';

@Component({
  selector: 'app-icis-selection-dialog',
  templateUrl: './icis-selection-dialog.component.html',
  styleUrls: ['./icis-selection-dialog.component.scss']
})
export class IcisSelectionDialogComponent implements OnInit, OnDestroy {
  dicIcis: BaseDictionary[];
  formGroup: FormGroup;
  selectedIds: number[];
  private onDestroy = new Subject();

  constructor(
    private dialogRef: MatDialogRef<IcisSelectionDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private data: any,
    private dictionaryService: DictionaryService,
    private fb: FormBuilder
  ) {
    this.buildForm();
    this.selectedIds = data.icisIds;
  }

  ngOnInit() {
    this.dictionaryService
      .getBaseDictionary(DictionaryType.DicICIS)
      .takeUntil(this.onDestroy)
      .subscribe(icis => {
        this.dicIcis = icis;
      });
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  onSubmit() {
    this.dialogRef.close(this.selectedIds);
  }

  onCancel() {
    this.dialogRef.close(null);
  }

  onIcisSelectionChanged(event: MatSelectionListChange) {
    event.option.selected
      ? this.selectedIds.push(event.option.value)
      : this.selectedIds.splice(
          this.selectedIds.findIndex(s => s === event.option.value),
          1
        );
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      icis: ['', Validators.required]
    });
  }
}
