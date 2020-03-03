import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, FormArray, ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

import { Fields } from '../subject-create-dialog/fields.enum';

interface Temp {
  index: number;
  value: string;
}

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: ContactsComponent,
      multi: true
    }
  ]
})
export class ContactsComponent implements OnInit, ControlValueAccessor {
  formGroup: FormGroup;
  data = [];
  temp: Temp;
  types = {
    [Fields.Tel]: 'tel',
    [Fields.MobileTel]: 'tel',
    [Fields.Fax]: 'tel',
    [Fields.Email]: 'email'
  };
  masks = {
    [Fields.Tel]: null,
    [Fields.MobileTel]: '+0 (000) 000-00-00',
    [Fields.Fax]: null,
    [Fields.Email]: null
  };
  hints = {
    tel: '+7 (700) 700-70-70',
    email: 'email@example.com'
  };
  total = 0;

  @Input() type: string;
  @Input() placeholder: string;

  isDisabled: boolean = false;

  constructor(
    private formBuilder: FormBuilder
  ) {
    this.buildForm();
  }

  ngOnInit(): void {}

  onChange(value): void {}

  onTouched(value): void {}

  writeValue(value): void {
    if (!value && this.data.length) {
      this.data = [];
    } else if (value && !this.data.length) {
      this.buildData(value);
      this.buildForm();
    }

    const outputValue = this.data.map((entry) => entry.value).filter((entry) => entry);

    this.onChange(outputValue);
  }

  registerOnChange(callback): void {
    this.onChange = callback;
  }

  registerOnTouched(callback): void {
    this.onTouched = callback;
  }

  setDisabledState(isDisabled: boolean): void {
    this.isDisabled = isDisabled;
  }

  add(): void {
    const { items } = this.formGroup.value;

    this.buildData([...items, null]);
    this.buildForm();
    this.writeValue(this.data);
  }

  update(): void {
    const { items } = this.formGroup.value;

    this.onChange(items);
  }

  delete(index: number): void {
    const { items } = this.formGroup.value;

    this.buildData(items);

    this.data.splice(index, 1);

    this.buildForm();
    this.writeValue(this.data);
  }

  isHintVisible(index: number): boolean {
    const items = this.formGroup.get('items');

    return !items.get(index.toString()).value;
  }

  private buildData(data): void {
    this.data = data.map((entry) => {
      return {
        index: this.total++,
        value: entry
      };
    });
  }

  private buildForm(): void {
    const array = [];
    for (let entry of this.data) {
      array.push(new FormControl(entry.value));
    }

    this.formGroup = this.formBuilder.group({
      items: new FormArray(array)
    });
  }
}
