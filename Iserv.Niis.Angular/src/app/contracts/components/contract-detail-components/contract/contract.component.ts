import { SubjectDto } from '../../../../subjects/models/subject.model';
import { ContractService } from '../../../contract.service';
import { Component, OnInit, EventEmitter, Input, Output } from '@angular/core';
import { SelectOption } from '../../../../shared/services/models/select-option';
import { ContractDetails } from '../../../models/contract-details';
import { CustomerRole } from '../../../../subjects/enums/customer-role.enum';

@Component({
  selector: 'app-contract',
  templateUrl: './contract.component.html',
  styleUrls: ['./contract.component.scss']
})
export class ContractComponent implements OnInit {
  roleCodes: CustomerRole[] = [
    CustomerRole.PatentAttorney,
    CustomerRole.Confidant,
    CustomerRole.Contact,
    CustomerRole.SideOne,
    CustomerRole.SideTwo
  ];

  additionalRules = {};

  @Input() selectOptions: SelectOption[];
  @Input() contractDetails: ContractDetails;
  @Output() submitData: EventEmitter<any> = new EventEmitter();
  @Output() register: EventEmitter<any> = new EventEmitter();
  @Output() edit: EventEmitter<boolean> = new EventEmitter();
  @Output() delete: EventEmitter<number> = new EventEmitter();
  @Output() modifiedData = new EventEmitter<ContractDetails>();

  get currentStageCode(): string {
    return this.contractService.getCurrentStageCode(this.contractDetails);
  }

  constructor(private contractService: ContractService) {}

  ngOnInit() {
    this.additionalRules = {
      [CustomerRole.SideOne]: this.currentStageCode !== 'DK02.1',
      [CustomerRole.SideTwo]: this.currentStageCode !== 'DK02.1'
    };
  }

  onSubmitData(value: any) {
    this.submitData.emit(value);
  }

  onEdit(value: boolean) {
    this.edit.emit(value);
  }

  onDelete(value: number) {
    this.delete.emit(value);
  }

  onSubjectsChanged(subjects: SubjectDto[]) {
    const otherSubjects = this.contractDetails.subjects.filter(s => !this.roleCodes.includes(s.roleCode as CustomerRole));
    this.contractDetails.subjects = otherSubjects.concat(subjects);
    this.modifiedData.emit(this.contractDetails);
  }
}
