import { Observable } from 'rxjs/Rx';
import { SubjectDto } from '../../../../subjects/models/subject.model';
import { ContractService } from '../../../contract.service';
import { Component, OnInit, Input, SimpleChanges, EventEmitter, Output, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs/Subject';
import { ContractDetails } from 'app/contracts/models/contract-details';
import { SelectOption } from 'app/shared/services/models/select-option';

@Component({
  selector: 'app-contract-subject',
  templateUrl: './contract-subject.component.html',
  styleUrls: ['./contract-subject.component.scss']
})
export class ContractSubjectComponent implements OnInit, OnDestroy {
  editMode: boolean;
  roleCodes: string[] = ['7', '8'];

  @Input() contractDetails: ContractDetails;
  @Input() selectOptions: SelectOption[];
  @Output() edit: EventEmitter<boolean> = new EventEmitter();
  @Output() submitData: EventEmitter<any> = new EventEmitter();
  @Output() modifiedData = new EventEmitter<ContractDetails>();
  get currentStageCode(): string { return this.contractService.getCurrentStageCode(this.contractDetails); }

  private onDestroy = new Subject();

  constructor(private contractService: ContractService) { }

  ngOnInit() {
  }

  onSubmitData(value: any) {
    this.submitData.emit(value);
  }

  onEdit(value: boolean) {
    this.editMode = value;
    this.edit.emit(value);
  }

  onSubjectsChanged(subjects: SubjectDto[]) {
    const haveBothParties = this.roleCodes.every(party => subjects.map(s => s.roleCode).includes(party));
    const oldPartiesSubjects = this.contractDetails.subjects.filter(s => this.roleCodes.includes(s.roleCode));
    const notPartiesSubjects = this.contractDetails.subjects.filter(s => !this.roleCodes.includes(s.roleCode));
    this.contractDetails.subjects = notPartiesSubjects.concat(subjects);

    (haveBothParties && oldPartiesSubjects.length > 0
      ? this.contractService.getById(this.contractDetails.id)
      : Observable.of(this.contractDetails))
      .takeUntil(this.onDestroy)
      .subscribe(contractDetails => this.modifiedData.emit(contractDetails));
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }
}
