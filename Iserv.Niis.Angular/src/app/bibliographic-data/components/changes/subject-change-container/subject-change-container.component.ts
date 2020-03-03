import { Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import {
    ChangeType,
    SubjectChangesComponent
} from 'app/bibliographic-data/models/changes-dto';
import { IntellectualPropertyDetails } from 'app/shared/models/intellectual-property-details';
import { DictionaryService } from 'app/shared/services/dictionary.service';
import { BaseDictionary } from 'app/shared/services/models/base-dictionary';
import { DictionaryType } from 'app/shared/services/models/dictionary-type.enum';
import { SubjectDto } from 'app/subjects/models/subject.model';
import { Subject } from 'rxjs/Subject';
import { SubjectChangeComponent } from '../subject-change/subject-change.component';
import { CustomerRole } from 'app/subjects/enums/customer-role.enum';

@Component({
    selector: 'app-subject-change-container',
    templateUrl: './subject-change-container.component.html',
    styleUrls: ['./subject-change-container.component.scss']
})
export class SubjectChangeContainerComponent
    implements OnInit, OnDestroy, SubjectChangesComponent {
    @Input()
    changeType: ChangeType;
    label: string;
    oldSubject: SubjectDto;
    newSubject: SubjectDto;
    @Input()
    details: IntellectualPropertyDetails;
    @ViewChild('newSubject')
    newSubjectControl: SubjectChangeComponent;

    private onDestroy = new Subject();

    constructor(private dictionaryService: DictionaryService) {}

    ngOnInit() {
        let roleCode = '';
        switch (this.changeType) {
            case ChangeType.Declarant:
                roleCode = CustomerRole.Declarant;
                break;
            case ChangeType.Addressee:
                roleCode = CustomerRole.CorrespondingRecipient;
                break;
            case ChangeType.PatentAttorney:
                roleCode = CustomerRole.PatentAttorney;
                break;
        }
        this.oldSubject = Object.assign(
            {},
            this.details.subjects.find(s => s.roleCode === roleCode)
        );
        this.dictionaryService
            .getBaseDictionary(DictionaryType.DicCustomerRole)
            .takeUntil(this.onDestroy)
            .subscribe((data: BaseDictionary[]) => {
                const role = data.find(d => d.code === roleCode);
                this.label = `Изменение контрагента заявки с ролью ${role.nameRu}`;
            });
    }

    ngOnDestroy(): void {
        this.onDestroy.next();
    }

    onCopySubject() {
        this.newSubjectControl.setValue(this.oldSubject);
    }

    getValue(): SubjectDto {
        this.newSubject = this.newSubjectControl.getValue();
        return this.newSubject;
    }
}
