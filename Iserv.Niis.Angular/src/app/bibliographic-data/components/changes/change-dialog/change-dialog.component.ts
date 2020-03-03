import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import { IntellectualPropertyDetails } from 'app/shared/models/intellectual-property-details';
import {
  ChangeTypeOption,
  ChangeTypeChooserComponent,
  ChangeType,
  ChangesContainerComponent,
  ChangesDto,
  ConfirmChangeComponent
} from 'app/bibliographic-data/models/changes-dto';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { SubjectDto } from 'app/subjects/models/subject.model';

@Component({
  selector: 'app-change-dialog',
  templateUrl: './change-dialog.component.html',
  styleUrls: ['./change-dialog.component.scss']
})
export class ChangeDialogComponent implements OnInit {
  details: IntellectualPropertyDetails;
  newDetails: IntellectualPropertyDetails;
  changes: ChangesDto[] = [];
  fieldChangeTypes: ChangeTypeOption[] = [];
  formChangeTypes: ChangeTypeOption[] = [];
  fullChangeTypes: ChangeTypeOption[] = [];
  subjectChangeTypes: ChangeTypeOption[] = [];
  subjectsToAttach: SubjectDto[] = [];
  subjectsToEdit: SubjectDto[] = [];
  subjectsToDelete: SubjectDto[] = [];
  selectedIndex = 0;

  private choiceCount = 0;

  fieldChange = [
    ChangeType.AddresseeAddress,
    ChangeType.AddresseeAddressEn,
    ChangeType.AddresseeAddressKz,
    ChangeType.DeclarantAddress,
    ChangeType.DeclarantAddressEn,
    ChangeType.DeclarantAddressKz,
    ChangeType.DeclarantName,
    ChangeType.DeclarantNameEn,
    ChangeType.DeclarantNameKz
  ];
  formChange = [ChangeType.Image, ChangeType.Icgs];
  subjectChange = [
    ChangeType.Declarant,
    ChangeType.Addressee,
    ChangeType.PatentAttorney
  ];

  @ViewChild('choose')
  choose: ChangeTypeChooserComponent;
  @ViewChild('make')
  make: ChangesContainerComponent;
  @ViewChild('confirm')
  confirm: ConfirmChangeComponent;

  constructor(
    public dialogRef: MatDialogRef<ChangeDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.details = data.details;
  }

  ngOnInit() {}

  onBack() {
    this.selectedIndex--;
  }

  onForward() {
    switch (this.selectedIndex) {
      case 0:
        this.getChooseValues();
        break;
      case 1:
        this.getMakeValues();
        break;
    }
    this.selectedIndex++;
  }

  onOk() {
    const details = this.confirm.onSubmit();
    this.dialogRef.close(details);
  }

  isNextDisabled(): boolean {
    return this.isTabDisabled(this.selectedIndex + 1);
  }

  isTabDisabled(index: number): boolean {
    switch (index) {
      case 1:
        return this.choiceCount === 0;
      case 2:
        return false;
    }
  }

  onChoiceMade(count: number) {
    this.choiceCount = count;
  }

  private getChooseValues() {
    const changeTypes = this.choose.getValue();
    this.fieldChangeTypes = Object.assign(
      [],
      changeTypes.filter(ct => ct.types.some(t => this.fieldChange.includes(t)))
    );
    this.formChangeTypes = Object.assign(
      [],
      changeTypes.filter(ct => ct.types.some(t => this.formChange.includes(t)))
    );
    this.subjectChangeTypes = Object.assign(
      [],
      changeTypes.filter(ct =>
        ct.types.some(t => this.subjectChange.includes(t))
      )
    );
    this.fullChangeTypes = Object.assign(
      [],
      changeTypes.filter(ct => ct.types.some(t => t === ChangeType.Everything))
    );
  }

  private getMakeValues() {
    this.changes = Object.assign([], this.make.getValue());
    this.newDetails = Object.assign({}, this.make.getDetails());
    this.subjectsToAttach = Object.assign([], this.make.getSubjectsToAttach());
    this.subjectsToDelete = Object.assign([], this.make.getSubjectsToDelete());
    this.subjectsToEdit = Object.assign([], this.make.getSubjectsToEdit());
  }
}
