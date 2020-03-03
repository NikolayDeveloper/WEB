import {
  Component,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges
} from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import {
  ChangesComponent,
  ChangesDto
} from 'app/bibliographic-data/models/changes-dto';

@Component({
  selector: 'app-field-change',
  templateUrl: './field-change.component.html',
  styleUrls: ['./field-change.component.scss']
})
export class FieldChangeComponent
  implements OnInit, OnChanges, ChangesComponent {
  formGroup: FormGroup;
  @Input()
  changeDto: ChangesDto;
  @Input()
  isAddress: boolean;

  constructor(private fb: FormBuilder) {
    this.buildForm();
  }

  ngOnInit() {
    this.initValues();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.changeDto && changes.changeDto.currentValue) {
      this.initValues();
    }
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      oldValue: [{ value: '', disabled: true }],
      newValue: [{ value: '' }]
    });
  }

  private initValues() {
    this.formGroup.patchValue({
      oldValue: this.changeDto.oldValue,
      newValue: this.changeDto.newValue
    });
  }

  getValue(): ChangesDto {
    const newValue = this.formGroup.get('newValue').value;
    if (this.isAddress) {
      this.changeDto.newValue = newValue.address;
    } else {
      this.changeDto.newValue = newValue;
    }
    return this.changeDto;
  }
}
