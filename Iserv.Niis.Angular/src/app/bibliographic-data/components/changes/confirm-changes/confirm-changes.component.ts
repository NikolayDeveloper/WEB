import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { BibliographicDataService } from 'app/bibliographic-data/bibliographic-data.service';
import {
  ChangesDto,
  ConfirmChangeComponent
} from 'app/bibliographic-data/models/changes-dto';
import { RequestService } from 'app/requests/request.service';
import { IntellectualPropertyDetails } from 'app/shared/models/intellectual-property-details';
import { SubjectDto } from 'app/subjects/models/subject.model';
import { SubjectsService } from 'app/subjects/services/subjects.service';
import { Observable } from 'rxjs';
import { Subject } from 'rxjs/Subject';

@Component({
  selector: 'app-confirm-changes',
  templateUrl: './confirm-changes.component.html',
  styleUrls: ['./confirm-changes.component.scss']
})
export class ConfirmChangesComponent
  implements OnInit, OnDestroy, ConfirmChangeComponent {
  @Input()
  details: IntellectualPropertyDetails;
  @Input()
  changeDtos: ChangesDto[];
  @Input()
  subjectsToAttach: SubjectDto[] = [];
  @Input()
  subjectsToEdit: SubjectDto[] = [];
  @Input()
  subjectsToDelete: SubjectDto[] = [];

  private onDestroy = new Subject();

  constructor() {}

  ngOnInit() {}

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  onSubmit(): ChangeDetails {
    return {
      details: this.details,
      changeDtos: this.changeDtos,
      subjectsToAttach: this.subjectsToAttach,
      subjectsToDelete: this.subjectsToDelete,
      subjectsToEdit: this.subjectsToEdit
    };
  }
}

export class ChangeDetails {
  details: IntellectualPropertyDetails;
  changeDtos: ChangesDto[];
  subjectsToAttach: SubjectDto[];
  subjectsToEdit: SubjectDto[];
  subjectsToDelete: SubjectDto[];
}
