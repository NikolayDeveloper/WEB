import { Component, OnInit, Input, Output, ViewChild, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatTable } from '@angular/material';

import { ControlType } from '../priority/control-type.enum';

@Component({
  selector: 'app-priority-container',
  templateUrl: './priority-container.component.html',
  styleUrls: ['./priority-container.component.scss']
})
export class PriorityContainerComponent implements OnInit {
  @Input() configuration: any;
  @Input() dataSource: any;
  @Input() isChecked = false;
  @Input() canEdited = false;
  @Input() edit: EventEmitter<any>;
  @Input() needSubmit: EventEmitter<any>;
  @Output() toggle: EventEmitter<any> = new EventEmitter();
  @Output() update: EventEmitter<any> = new EventEmitter();

  formGroup: FormGroup;
  groups: Map<string, any> = new Map();
  displayedColumns = ['buttons'];
  controlType = ControlType;
  @ViewChild(MatTable) table: MatTable<any>;

  constructor(
    private changeDetectionRef: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    const { fields } = this.configuration;

    for (let field of fields) {
      const group = this.groups.has(field.group) ? this.groups.get(field.group) : [];

      group.push(field);
      this.groups.set(field.group, group);
      this.displayedColumns.splice(this.displayedColumns.length - 1, 0, field.name);
    }

    this.edit.subscribe((isEditMode) => {
      if (!isEditMode) {
        this.resetForm();
      }
    });

    this.needSubmit.subscribe(() => {
      if (!this.isEmpty()) {
        this.add();
      }
    });

    this.buildForm();
  }

  add(): void {
    this.dataSource.push(this.formGroup.value);
    this.changeDetectionRef.detectChanges();

    if (this.table) {
      this.table.renderRows();
    }

    this.resetForm();
  }

  remove(index): void {
    this.dataSource.splice(index, 1);
    this.changeDetectionRef.detectChanges();

    if (this.table) {
      this.table.renderRows();
    }
  }

  onToggle(): void {
    this.toggle.emit({
      name: this.configuration.name,
      data: this.isChecked
    });
    this.changeDetectionRef.detectChanges();
  }

  toDate(value): string {
    return new Date(Date.parse(value)).toLocaleString();
  }

  getValue(key, value): string {
    if (this.configuration.values.hasOwnProperty(key)) {
      const entry = this.configuration.values[key].find((entry) => (entry.id === value));

      return entry.value;
    } else {
      return value;
    }
  }

  get fieldGroups(): any[] {
    return Array.from(this.groups.values());
  }

  isEmpty(): boolean {
    const values = Object.values(this.formGroup.value);

    return values.every(value => !value);
  }

  isDisabled(): boolean {
    this.update.emit(this.isEmpty() || !this.formGroup.invalid);

    return this.formGroup.invalid;
  }

  private buildForm(): void {
    const group = {};

    for (let field of this.configuration.fields) {
      const control = new FormControl();

      if (this.configuration.default.hasOwnProperty(field.name)) {
        control.setValue(this.configuration.default[field.name]);
      } else {
        control.reset();
      }

      if (field.required) {
        control.setValidators(Validators.required);
      }

      group[field.name] = control;
    }

    this.formGroup = new FormGroup(group);
  }

  private resetForm(): void {
    for (let field of this.configuration.fields) {
      const control = this.formGroup.get(field.name);

      if (this.configuration.default.hasOwnProperty(field.name)) {
        control.setValue(this.configuration.default[field.name]);
      } else {
        control.reset();
      }
    }
  }
}
