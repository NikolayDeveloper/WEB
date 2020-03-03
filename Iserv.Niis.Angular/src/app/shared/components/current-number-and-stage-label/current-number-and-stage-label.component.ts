import { Component, OnInit, Input } from '@angular/core';
import { OwnerType } from 'app/shared/services/models/owner-type.enum';

@Component({
  selector: 'app-current-number-and-stage-label',
  templateUrl: './current-number-and-stage-label.component.html',
  styleUrls: ['./current-number-and-stage-label.component.scss']
})
export class CurrentNumberAndStageLabelComponent implements OnInit {
  @Input() currentStageNameRu: string;
  @Input() number: string;
  @Input() ownerType: OwnerType;

  constructor() {}

  ngOnInit() {}

  getTypeName(): string {
    switch (this.ownerType) {
      case OwnerType.Request:
        return 'Заявка';
      case OwnerType.ProtectionDoc:
        return 'Охранный документ';
      case OwnerType.Contract:
        return 'Договор';
      default:
        return '';
    }
  }

  getLabel(): string {
    const result = [];
    const type = this.getTypeName();
    if (type) {
      if (this.number) {
        result.push(`${type} №${this.number}`);
      } else {
        result.push(`${type} без номера`);
      }
    }
    if (this.currentStageNameRu) {
      result.push(`Текущий этап ${this.currentStageNameRu}`);
    }
    return result.join(', ');
  }
}
