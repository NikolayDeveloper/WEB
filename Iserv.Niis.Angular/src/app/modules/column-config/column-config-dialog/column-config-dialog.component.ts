import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, FormArray } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

import { ColumnConfig } from '../column-config.model';
import { ColumnConfigService } from '../column-config.service';
import { ColumnConfigModule } from '..';

@Component({
  selector: 'app-column-config-dialog',
  templateUrl: './column-config-dialog.component.html',
})
export class ColumnConfigDialogComponent {
  formGroup: FormGroup;
  columnsConfig: ColumnConfig[];
  dragIndex = -1;
  dropIndex = -1;

  constructor(
    private dialogRef: MatDialogRef<ColumnConfigDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private data: any,
    private columnConfigService: ColumnConfigService,
    private formBuilder: FormBuilder) {
    this.init();
  }

  get items(): FormArray {
    return this.formGroup.get('items') as FormArray;
  }

  onCancel(): void {
    this.dialogRef.close(this.columnConfigService.get(this.data.configKey, this.columnsConfig)
      .filter(cc => cc.enabled)
      .map(cc => cc.field));
  }

  onSave(): void {
    let index = 0;
    this.columnsConfig.forEach(cc => {
      cc.enabled = this.items.get(`${index++}`).value.value;
    });
    const dispayedColumns = this.columnsConfig
      .filter(i => i.enabled)
      .map(i => i.field);
    this.columnConfigService.save(this.data.configKey, this.columnsConfig);
    this.dialogRef.close(dispayedColumns);
  }

  private init() {
    this.formGroup = this.formBuilder.group({
      items: this.formBuilder.array
    });

    this.columnsConfig = this.columnConfigService.get(this.data.configKey, this.data.defaultConfig);

    const formGroups = this.columnsConfig.map(config => {
      return this.formBuilder.group({
        field: config.field,
        value: config.enabled
      });
    });
    this.formGroup.setControl('items', this.formBuilder.array(formGroups));
  }

  dragStart(event, i: number) {
    this.dragIndex = i;
  }

  dragEnter(event, i: number) {
    this.dropIndex = i;
  }

  dragEnd(event) {
    this.dragIndex = -1;
    this.dropIndex = -1;
  }

  drop(event, i: number) {
    if (this.dragIndex > -1) {
      const dragConfig = this.columnsConfig[this.dragIndex];
      this.columnsConfig.splice(this.dragIndex, 1);
      this.columnsConfig.splice(i, 0, dragConfig);

      const dragItem = this.items.controls[this.dragIndex];
      this.items.controls.splice(this.dragIndex, 1);
      this.items.insert(i, dragItem);
      this.formGroup.markAsDirty();
    }
  }
}
