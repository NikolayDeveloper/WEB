import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  QueryList,
  SimpleChanges,
  ViewChildren,
  ChangeDetectorRef,
  ViewChild
} from '@angular/core';
import { MatDialog, MatTab } from '@angular/material';
import { SubjectDto } from 'app/subjects/models/subject.model';
import { Subject } from 'rxjs/Subject';
import { IntellectualPropertyDetails } from '../../shared/models/intellectual-property-details';
import { OwnerType } from '../../shared/services/models/owner-type.enum';
import { DescriptionComponent } from './description/description.component';

@Component({
  selector: 'app-bibliographic-data',
  templateUrl: './bibliographic-data.component.html',
  styleUrls: ['./bibliographic-data.component.scss']
})
export class BibliographicDataComponent implements OnChanges {
  @Input()
  editMode: boolean;
  protectionDocTypeCode: string;
  ownerId: number;
  ownerType: OwnerType;
  counter = 1;
  tabIndex = 0;

  @Input() roleCodes = [];
  @Input() owner: IntellectualPropertyDetails;
  @Input() selectedIndex: number;
  @Input() disabled: boolean;
  @Input() isChangeMode: boolean;
  @Input() needUpdateSubjects: boolean;
  @Output() submitData: EventEmitter<any> = new EventEmitter();
  @Output() edit: EventEmitter<boolean> = new EventEmitter();
  @Output() modifiedData = new EventEmitter<IntellectualPropertyDetails>();
  @Output() subjectsChanged = new EventEmitter<SubjectDto[]>();
  @Output() subjectDeleted = new EventEmitter<SubjectDto>();
  @Output() subjectAttached = new EventEmitter<SubjectDto>();
  @Output() subjectEdited = new EventEmitter<SubjectDto>();
  @Output() descriptionChanged = new EventEmitter<string>();
  @ViewChildren(DescriptionComponent) descriptionComponents: QueryList<DescriptionComponent>;
  @ViewChild('subjectsTab') subjectsTab: MatTab;

  constructor(private dialog: MatDialog,
    private changeDetectorRef: ChangeDetectorRef) {}

  private onDestroy = new Subject();

  ngOnChanges(changes: SimpleChanges) {
    if (changes.needUpdateSubjects) {
      this.checkNeedUpdateSubjects();
    }

    if (changes.owner && changes.owner.currentValue) {
      this.protectionDocTypeCode = this.owner.protectionDocTypeCode;
      this.ownerId = this.owner.id;
      this.ownerType = this.owner.ownerType;
    }
    this.changeDetectorRef.detectChanges();
  }

  checkNeedUpdateSubjects(): void {
    if (this.needUpdateSubjects) {
      this.tabIndex = this.subjectsTab.position;
    }
  }

  onSubmitData(value: any) {
    this.submitData.emit(value);
    this.counter++;
  }

  onEdit(value: boolean) {
    this.editMode = value;
    this.edit.emit(value);
  }

  selectedIndexChange(index: number) {
    this.selectedIndex = index;
  }

  getValue(): IntellectualPropertyDetails {
    let details = null;
    this.descriptionComponents.forEach(
      d => (details = Object.assign({}, d.getValue()))
    );
    return details;
  }
}
