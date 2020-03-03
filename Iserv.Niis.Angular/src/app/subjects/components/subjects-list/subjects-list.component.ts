import {
    Component,
    EventEmitter,
    Input,
    OnChanges,
    OnDestroy,
    OnInit,
    Output,
    SimpleChanges
} from '@angular/core';
import { Config } from '../../../shared/components/table/config.model';
import { OwnerType } from '../../../shared/services/models/owner-type.enum';
import { SubjectDto } from '../../models/subject.model';
import { SubjectsService } from 'app/subjects/services/subjects.service';
import {DictionaryType} from '../../../shared/services/models/dictionary-type.enum';
import {DictionaryService} from '../../../shared/services/dictionary.service';
import {Subject} from 'rxjs/Subject';
import { CustomerRole } from 'app/subjects/enums/customer-role.enum';
import { CustomerType } from 'app/subjects/enums/customer-type.enum';

@Component({
    selector: 'app-subjects-list',
    templateUrl: './subjects-list.component.html',
    styleUrls: ['./subjects-list.component.scss']
})
export class SubjectsListComponent implements OnInit, OnChanges, OnDestroy {
    subjectDtos: any[];
    tableName: string;
    columns: Config[] = [
        new Config({ columnDef: 'roleNameRu', header: 'Роль', class: 'width-100' }),
        new Config({ columnDef: 'typeNameRu', header: 'Тип', class: 'width-100' }),
        new Config({ columnDef: 'xin', header: 'ИИН/БИН', class: 'width-100' }),
        new Config({ columnDef: 'nameRu', header: 'ФИО / Наименование Рус.', class: 'width-350' }),
        new Config({ columnDef: 'nameKz', header: 'ФИО / Наименование Каз.', class: 'width-350' }),
        new Config({ columnDef: 'nameEn', header: 'ФИО / Наименование Англ.', class: 'width-350' }),
        new Config({ columnDef: 'commonAddress', header: 'Адрес', class: 'width-200' }),
        new Config({ columnDef: 'phone', header: 'Телефон', class: 'width-200' }),
        new Config({ columnDef: 'mobilePhone', header: 'Мобильный телефон', class: 'width-200' }),
        new Config({ columnDef: 'email', header: 'Email', class: 'width-150' }),
        new Config({ columnDef: 'residence', header: 'Резидент', class: 'width-150'}),
        new Config({ columnDef: 'countryCode', header: 'Код страны', class: 'width-100' }),
        new Config({
            columnDef: 'edit',
            header: 'Редактировать',
            class: 'width-25 sticky',
            icon: 'edit',
            click: row => this.edit.emit(row),
            disable: row => {
              // if (this.needUpdate) {
              //   return !this.brokenSubjects.includes(row.id);
              // } else if (this.additionalRules.hasOwnProperty(row.roleCode)) {
              //   return this.additionalRules[row.roleCode];
              // } else {
              //   return this.disabled || !this.roleCodes.includes(row.roleCode);
              // }
              return this.disabled;
            }
        }),
        new Config({
            columnDef: 'delete',
            header: 'Удалить',
            class: 'width-25 sticky',
            icon: 'delete',
            click: row => this.delete.emit(row),
            disable: row => {
              // if (this.additionalRules.hasOwnProperty(row.roleCode)) {
              //   return this.additionalRules[row.roleCode];
              // } else {
              //   return this.disabled;
              // }
              return this.disabled;
            }
        })
    ];
    residencyStatuses: any[] = [
        { code: 'res', nameRu: 'Резидент' },
        { code: 'nrs', nameRu: 'Не резидент' }
    ];
    hiddenRoles = [
      CustomerRole.Addressee
    ];
    brokenSubjects = [];
    @Input() subjects: SubjectDto[];
    @Input() disabled: boolean;
    @Input() needUpdate: boolean;
    @Input() additionalRules = {};
    @Input() roleCodes: string;
    @Input() ownerType: OwnerType;
    @Input() ownerId: number;
    @Output() edit = new EventEmitter<SubjectDto>();
    @Output() delete = new EventEmitter<SubjectDto>();
    private onDestroy: Subject<any> = new Subject<any>();
    constructor(private subjectsService: SubjectsService, private dictionaryService: DictionaryService,
    ) {}

    isNotResident(value): boolean {
        return value !== 'KZ';
    }
    getCountryCodes() {
        return this.dictionaryService.getSelectOptions(DictionaryType.DicCountry);

    }
    getResidencyValue() {
        this.getCountryCodes()
            .takeUntil(this.onDestroy)
            .subscribe(data => this.getCountiesForRows(data));
    }
    getCountiesForRows(countryCodes) {
        this.subjectDtos.forEach(subject => {
            const id = countryCodes.find(ids => ids.id === subject.countryId);
            subject.countryCode = id && id.code;
            subject.residence = this.isNotResident(id && id.code)
                ? this.residencyStatuses.find(rs => rs.code === 'nrs').nameRu
                : this.residencyStatuses.find(rs => rs.code === 'res').nameRu;
        });
    }

    ngOnInit() {
        this.tableName = this.ownerType === OwnerType.ProtectionDoc ? 'protectionDocSubjectBibTable' : 'requestsSubjectBibTable';
        if (this.ownerType === OwnerType.ProtectionDoc) {
            this.columns = this.columns.filter(c => c.columnDef !== 'delete');
            const editColumn = this.columns.find(c => c.columnDef === 'edit');
            if (editColumn) {
                editColumn.disable = row => {
                    return (
                        !['CORRESPONDENCE', 'OWNER', '3'].includes(row.roleCode) ||
                        this.disabled
                    );
                };
            }
        }
    }

    ngOnChanges(changes: SimpleChanges): void {
        if (this.ownerType === OwnerType.Contract && this.ownerId && this.roleCodes) {
            this.subjectsService
                .get(this.ownerId, this.ownerType)
                .subscribe(result => {
                    this.subjectDtos = result.filter(s => s.roleCode === null || this.roleCodes.includes(s.roleCode));
                });
        }

        if (this.subjects && this.subjects.length && this.roleCodes && this.ownerType !== OwnerType.Contract) {
            this.subjectDtos = this.subjects
              // .filter((subject) => (this.roleCodes.length === 0 || this.roleCodes.includes(subject.roleCode)))
              .filter((subject) => !this.hiddenRoles.includes(subject.roleCode as CustomerRole))
              .sort((a, b) => (a.displayOrder - b.displayOrder));
        }
        if (this.subjectDtos) {
            this.getResidencyValue();
        }
        if (this.needUpdate) {
            const declarants = this.subjects.filter(subject => (subject.roleCode === CustomerRole.Declarant));
            this.brokenSubjects = [];
            for (let subject of declarants) {
                if (subject.typeId !== CustomerType.Individual && subject.typeId !== CustomerType.LegalEntity) {
                    this.brokenSubjects.push(subject.id);
                }
            }
        }
    }
    ngOnDestroy() {
        this.onDestroy.next(true);
    }

}
