import {
    Component,
    OnInit,
    Input,
    OnChanges,
    SimpleChanges,
    ViewChildren,
    QueryList,
    Output,
    EventEmitter,
    AfterViewInit,
    ViewChild
} from '@angular/core';
import {
    ChangesDto,
    ChangeTypeOption,
    ChangeType,
    ChangesComponent,
    ChangesContainerComponent,
    SubjectChangesComponent
} from 'app/bibliographic-data/models/changes-dto';
import { IntellectualPropertyDetails } from 'app/shared/models/intellectual-property-details';
import { SubjectDto } from 'app/subjects/models/subject.model';
import { BiblioField } from 'app/bibliographic-data/models/field-config';
import { FormGroup, FormBuilder } from '@angular/forms';
import { FieldChangeComponent } from '../field-change/field-change.component';
import { SubjectChangeContainerComponent } from '../subject-change-container/subject-change-container.component';
import { BibliographicDataComponent } from '../../bibliographic-data.component';
import { CustomerRole } from 'app/subjects/enums/customer-role.enum';

@Component({
    selector: 'app-make-changes',
    templateUrl: './make-changes.component.html',
    styleUrls: ['./make-changes.component.scss']
})
export class MakeChangesComponent
    implements OnInit, OnChanges, ChangesContainerComponent {
    changeDtos: ChangesDto[] = [];
    roleCodes = ['1', '2', '4', '6', 'OWNER', 'CORRESPONDENCE'];
    formGroup: FormGroup;

    subjectsToAttach: SubjectDto[] = [];
    subjectsToEdit: SubjectDto[] = [];
    subjectsToDelete: SubjectDto[] = [];

    @Input()
    fieldChangeTypes: ChangeTypeOption[];
    @Input()
    formChangeTypes: ChangeTypeOption[];
    @Input()
    fullChangeTypes: ChangeTypeOption[];
    @Input()
    subjectChangeTypes: ChangeTypeOption[];
    @Input()
    details: IntellectualPropertyDetails;
    @ViewChildren(FieldChangeComponent)
    changeControls!: QueryList<FieldChangeComponent>;
    @ViewChildren(BiblioField)
    fields!: QueryList<BiblioField>;
    @ViewChildren(SubjectChangeContainerComponent)
    subjects!: QueryList<SubjectChangeContainerComponent>;
    @ViewChildren(BibliographicDataComponent)
    biblios!: QueryList<BibliographicDataComponent>;

    private addressChangeTypes = [
        ChangeType.AddresseeAddress,
        ChangeType.AddresseeAddressEn,
        ChangeType.AddresseeAddressKz,
        ChangeType.DeclarantAddress,
        ChangeType.DeclarantAddressEn,
        ChangeType.DeclarantAddressKz
    ];

    constructor(private fb: FormBuilder) {
        this.buildForm();
    }

    ngOnInit() {}

    ngOnChanges(changes: SimpleChanges): void {
        if (changes.fieldChangeTypes && changes.fieldChangeTypes.currentValue) {
            this.initChangeDtos();
        }
        if (changes.formChangeTypes && changes.formChangeTypes.currentValue) {
            this.initFormChangeValues();
        }
    }

    private initChangeDtos() {
        this.changeDtos = [];
        this.fieldChangeTypes.forEach(ct => {
            ct.types.forEach(t => {
                const changeDto = new ChangesDto();
                changeDto.changeType = t;
                changeDto.id = this.details.id;
                changeDto.ownerType = this.details.ownerType;
                switch (t) {
                    case ChangeType.AddresseeAddress:
                        changeDto.oldValue = this.details.subjects.find(
                            s =>
                                s.roleCode ===
                                CustomerRole.CorrespondingRecipient
                        ).address;
                        changeDto.name = 'Адрес адресата для переписки на русском';
                        break;
                    case ChangeType.AddresseeAddressEn:
                        changeDto.oldValue = this.details.subjects.find(
                            s =>
                                s.roleCode ===
                                CustomerRole.CorrespondingRecipient
                        ).addressEn;
                        changeDto.name = 'Адрес адресата для переписки на английском';
                        break;
                    case ChangeType.AddresseeAddressKz:
                        changeDto.oldValue = this.details.subjects.find(
                            s =>
                                s.roleCode ===
                                CustomerRole.CorrespondingRecipient
                        ).addressKz;
                        changeDto.name = 'Адрес адресата для переписки на казахском';
                        break;
                    case ChangeType.DeclarantAddress:
                        changeDto.oldValue = this.details.subjects.find(
                            s =>
                                s.roleCode ===
                                CustomerRole.Declarant
                        ).address;
                        changeDto.name = 'Адрес заявителя на русском';
                        break;
                    case ChangeType.DeclarantAddressEn:
                        changeDto.oldValue = this.details.subjects.find(
                            s =>
                                s.roleCode ===
                                CustomerRole.Declarant
                        ).addressEn;
                        changeDto.name = 'Адрес заявителя на английском';
                        break;
                    case ChangeType.DeclarantAddressKz:
                        changeDto.oldValue = this.details.subjects.find(
                            s =>
                                s.roleCode ===
                                CustomerRole.Declarant
                        ).addressKz;
                        changeDto.name = 'Адрес заявителя на казахском';
                        break;
                    case ChangeType.DeclarantName:
                        changeDto.oldValue = this.details.subjects.find(
                            s =>
                                s.roleCode ===
                                CustomerRole.Declarant
                        ).nameRu;
                        changeDto.name = 'Наименование заявителя на русском';
                        break;
                    case ChangeType.DeclarantNameEn:
                        changeDto.oldValue = this.details.subjects.find(
                            s =>
                                s.roleCode ===
                                CustomerRole.Declarant
                        ).nameEn;
                        changeDto.name = 'Наименование заявителя на английском';
                        break;
                    case ChangeType.DeclarantNameKz:
                        changeDto.oldValue = this.details.subjects.find(
                            s =>
                                s.roleCode ===
                                CustomerRole.Declarant
                        ).nameKz;
                        changeDto.name = 'Наименование заявителя на казахском';
                        break;
                }
                this.changeDtos.push(changeDto);
            });
        });
    }

    private initFormChangeValues() {
        if (this.details) {
            if (['PN', 'TM'].includes(this.details.protectionDocTypeCode)) {
                if (this.details.isHasMaterialExpertOpinionWithOugoingNumber) {
                    this.roleCodes = ['1', '4', '6', 'CORRESPONDENCE', 'OWNER'];
                } else {
                    this.roleCodes = ['1', '4', '6', 'CORRESPONDENCE'];
                }
            } else if (this.details.protectionDocTypeCode === 'TM') {
                if (this.details.currentWorkflow.currentStageCode !== 'TM02.1') {
                    this.roleCodes = ['1', '4', '6', 'CORRESPONDENCE', 'OWNER'];
                } else {
                    this.roleCodes = ['1', '4', '6', 'CORRESPONDENCE'];
                }
            } else if (
                ['U', 'B', 'S2'].includes(this.details.protectionDocTypeCode)
            ) {
                if (this.details.currentWorkflow.currentStageCode !== 'TM02.1') {
                    this.roleCodes = ['1', '2', '4', '6', 'CORRESPONDENCE', 'OWNER'];
                } else {
                    this.roleCodes = ['1', '2', '4', '6', 'CORRESPONDENCE'];
                }
            } else if (this.details.protectionDocTypeCode === 'SA') {
                this.roleCodes = ['1', '2', '4', '6', 'CORRESPONDENCE'];
            }
            this.formGroup.get('icgsFields').patchValue(this.details.icgsRequestDtos);
            this.formGroup.get('imageFields').patchValue(this.details);
        }
    }

    getValue(): ChangesDto[] {
        const result = [];
        if (!!this.changeControls && this.changeControls.length > 0) {
            this.changeControls.forEach(c => result.push(c.getValue()));
        }
        return result;
    }

    getDetails(): IntellectualPropertyDetails {
        this.details.subjects = [];
        this.biblios.forEach(
            b => (this.details = Object.assign(this.details, b.getValue()))
        );
        this.subjects.forEach(s => this.subjectsToEdit.push(s.getValue()));
        this.fields.forEach(f => Object.assign(this.details, f.getValue()));
        return this.details;
    }

    getSubjectsToAttach(): SubjectDto[] {
        return this.subjectsToAttach;
    }

    getSubjectsToEdit(): SubjectDto[] {
        return this.subjectsToEdit;
    }

    getSubjectsToDelete(): SubjectDto[] {
        return this.subjectsToDelete;
    }

    isImagesActive(): boolean {
        return this.formChangeTypes.some(t => t.types.includes(ChangeType.Image));
    }

    isIcgsActive(): boolean {
        return this.formChangeTypes.some(t => t.types.includes(ChangeType.Icgs));
    }

    isFullFormActive(): boolean {
        return this.fullChangeTypes.some(t =>
            t.types.includes(ChangeType.Everything)
        );
    }

    assignDetails(value: any) {
        Object.assign(this.details, value);
    }

    onSubjectAttached(subject: SubjectDto) {
        this.subjectsToAttach.push(subject);
    }

    onSubjectEdited(subject: SubjectDto) {
        this.subjectsToEdit.push(subject);
    }

    onSubjectDeleted(subject: SubjectDto) {
        this.subjectsToDelete.push(subject);
    }

    isValid(): boolean {
        const changeValues = this.getValue();
        return (
            this.formGroup.valid &&
            changeValues.every(v => !!v.newValue && v.newValue !== '')
        );
    }

    isAddress(changeDto: ChangesDto): boolean {
        return this.addressChangeTypes.includes(changeDto.changeType);
    }

    private buildForm() {
        this.formGroup = this.fb.group({
            imageFields: [{ value: '' }],
            icgsFields: [{ value: '' }]
        });
    }
}
