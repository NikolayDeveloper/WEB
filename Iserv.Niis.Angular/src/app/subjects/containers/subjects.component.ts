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
import { MatDialog } from '@angular/material';
import { SnackBarHelper } from 'app/core/snack-bar-helper.service';
import { DictionaryService } from 'app/shared/services/dictionary.service';
import { SubjectDeleteDialogComponent } from 'app/subjects/components/subject-delete-dialog/subject-delete-dialog.component';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/operator/takeUntil';
import { Observable, Subscription } from 'rxjs/Rx';
import { Subject } from 'rxjs/Subject';
import { OwnerType } from '../../shared/services/models/owner-type.enum';
// import { SubjectFormDialogComponent } from '../components/subject-form-dialog/subject-form-dialog.component';
import { SubjectDto } from '../models/subject.model';
import { SubjectsService } from '../services/subjects.service';
import { SubjectFormMode } from 'app/subjects/enums/subject-form-mode.enum';
import { CustomerRole } from 'app/subjects/enums/customer-role.enum';
import { SubjectCreateDialogComponent } from 'app/subjects/components/subject-create-dialog/subject-create-dialog.component';

@Component({
    selector: 'app-subjects',
    templateUrl: './subjects.component.html',
    styleUrls: ['./subjects.component.css']
})
export class SubjectsComponent implements OnInit, OnChanges, OnDestroy {
    selectedSubject: SubjectDto;
    subjects: SubjectDto[] = [];
    private attachDialogAfterClosed = new Subject<Observable<any>>();
    private onDestroy = new Subject();

    rolePluralityConfig: RolePluralityConfig[] = [
        {
            roleCodes: [
                CustomerRole.PatentAttorney,
                CustomerRole.Confidant
            ],
            protectionDocTypeCode: 'TM',
            plurality: SubjectRolePlurality.Multiple,
            trademarkCodeType: ['KTM', 'TTM', 'FTM']
        },
        {
            roleCodes: [
                CustomerRole.CorrespondingRecipient
            ],
            protectionDocTypeCode: 'TM',
            plurality: SubjectRolePlurality.Single,
            trademarkCodeType: ['KTM', 'TTM', 'FTM']
        },
        {
            roleCodes: [
                CustomerRole.Declarant
            ],
            protectionDocTypeCode: 'TM',
            plurality: SubjectRolePlurality.Single,
            trademarkCodeType: ['TTM', 'FTM']
        },
        {
            roleCodes: [
                CustomerRole.Declarant
            ],
            protectionDocTypeCode: 'TM',
            plurality: SubjectRolePlurality.Multiple,
            trademarkCodeType: ['KTM']
        }
    ];
    curentCounter = 0;

    @Input() counter: number;
    @Input() ownerId: number;
    @Input() ownerType: OwnerType;
    @Input() protectionDocTypeCode: string;
    @Input() disabled: boolean;
    @Input() needUpdate: boolean;
    @Input() additionalRules = {};
    @Input() roleCodes: string[];
    @Input() hasProxy: boolean;
    @Input() isChangeMode = false;
    @Input() speciesTrademarkCode: string;
    @Output() changed = new EventEmitter<SubjectDto[]>();
    @Output() deleted = new EventEmitter<SubjectDto>();
    @Output() attached = new EventEmitter<SubjectDto>();
    @Output() edited = new EventEmitter<SubjectDto>();

    constructor(
        private subjectsService: SubjectsService,
        private dictionaryService: DictionaryService,
        private snackbarHelper: SnackBarHelper,
        public dialog: MatDialog
    ) {}

    ngOnInit() {
        let subscription: Subscription = null;
        let rawSubject = null;

        this.attachDialogAfterClosed
            .takeUntil(this.onDestroy)
            .subscribe(afterClosed => {
                // TODO: замыкание subscription используется из за бага rxjs: в данном кейсе не работают switch и switchMap
                if (subscription) {
                    subscription.unsubscribe();
                }
                subscription = afterClosed
                    .takeUntil(this.onDestroy)
                    .do(() => this.resetSelected())
                    .filter(subject => !!subject)
                    .switchMap(subject => {
                        rawSubject = subject;
                        if (this.isChangeMode) {
                            this.attached.emit(subject);
                            return Observable.of(subject);
                        }
                        return subject.mode === SubjectFormMode.InsertOnAttach
                            ? this.subjectsService.create(subject, this.ownerType)
                            : this.subjectsService.attach(subject, this.ownerType);
                    })
                    .subscribe(
                        subject => {
                            const subjects = this.subjects.slice();
                            subjects.push(subject);
                            this.subjects = subjects;
                            this.changed.emit(this.subjects);
                        },
                        err => {
                            this.snackbarHelper.error(err);
                            this.attachDialogAfterClosed.next(
                                this.dialog
                                    .open(SubjectCreateDialogComponent, {
                                        data: {
                                            subject: rawSubject,
                                            protectionDocTypeCode: this.protectionDocTypeCode,
                                            mode: SubjectFormMode.InsertOnAttach,
                                            roleCodes: this.roleCodes,
                                            hasProxy: this.hasProxy,
                                            ownerType: this.ownerType,
                                            ownerId: this.ownerId
                                        },
                                        width: '960px'
                                    }).afterClosed()
                            );
                        }
                    );
            });
    }

    ngOnChanges(changes: SimpleChanges): void {
        if (this.curentCounter === this.counter) {
            return;
        } else if (this.ownerId && this.ownerType && this.roleCodes) {
            this.subjectsService
                .get(this.ownerId, this.ownerType)
                .takeUntil(this.onDestroy)
                .do(() => this.resetSelected())
                .subscribe(subjects => {
                    this.subjects = subjects;
                    this.changed.emit(this.subjects);
                });
        }
        this.curentCounter = this.counter;
    }

    ngOnDestroy(): void {
        this.onDestroy.next();
    }

    onEdit(selectedSubject?: SubjectDto) {
        this.selectedSubject = selectedSubject || this.selectedSubject;

        const dialogRef = this.dialog.open(SubjectCreateDialogComponent, {
            data: {
                subject: Object.assign({}, this.selectedSubject),
                protectionDocTypeCode: this.protectionDocTypeCode,
                mode: SubjectFormMode.Edit,
                roleCodes: this.roleCodes,
                hasProxy: this.hasProxy,
                ownerType: this.ownerType,
                ownerId: this.ownerId
            },
            width: '960px'
        });

        dialogRef
            .afterClosed()
            .takeUntil(this.onDestroy)
            .do(() => this.resetSelected())
            .filter(subject => !!subject)
            .switchMap(subject => {
                if (this.isChangeMode) {
                    this.edited.emit(subject);
                    return Observable.of(subject);
                }
                return this.subjectsService.update(subject, this.ownerType);
            })
            .filter(updatedSubject => !!updatedSubject)
            .subscribe(
                (updatedSubject: SubjectDto) => {
                    const subjects = this.subjects.slice();
                    const oldData = subjects.filter(s => s.id === updatedSubject.id)[0];
                    const index = subjects.indexOf(oldData);
                    subjects.splice(index, 1, updatedSubject);
                    this.subjects = subjects;
                    this.changed.emit(this.subjects);
                },
                err => this.snackbarHelper.error(err)
            );
    }

    onAttach(selectedSubject?: SubjectDto) {
        this.selectedSubject = selectedSubject || this.selectedSubject;
        if (!this.selectedSubject) {
            this.resetSelected();
        }
        this.attachDialogAfterClosed.next(
            this.dialog.open(SubjectCreateDialogComponent, {
                data: {
                    subject: {
                        id: this.selectedSubject.id,
                        ownerId: this.selectedSubject.ownerId
                    },
                    protectionDocTypeCode: this.protectionDocTypeCode,
                    mode: SubjectFormMode.Insert,
                    roleCodes: this.filterRoleCodes(this.roleCodes),
                    hasProxy: this.hasProxy,
                    ownerType: this.ownerType,
                    ownerId: this.ownerId
                },
                width: '960px'
            }).afterClosed()
        );
    }

    onDelete(selectedSubject: SubjectDto) {
        const dialogRef = this.dialog.open(SubjectDeleteDialogComponent);
        dialogRef
            .afterClosed()
            .takeUntil(this.onDestroy)
            .filter(result => result === 'true')
            .switchMap(() => {
                if (this.isChangeMode) {
                    this.deleted.emit(selectedSubject);
                    return Observable.of(selectedSubject);
                }
                return this.subjectsService.delete(selectedSubject.id, this.ownerType);
            })
            .subscribe(() => {
                const subjects = this.subjects.slice();
                const index = subjects.indexOf(selectedSubject);
                subjects.splice(index, 1);
                this.subjects = subjects;
                this.changed.emit(this.subjects);
            });
    }

    private resetSelected() {
        this.selectedSubject = new SubjectDto({ ownerId: this.ownerId });
    }

    private filterRoleCodes(roleCodes: string[]): string[] {
        const singularConfigs = this.rolePluralityConfig.filter(
            rpc =>
                rpc.protectionDocTypeCode === this.protectionDocTypeCode &&
                this.speciesTrademarkCode &&
                rpc.trademarkCodeType.includes(this.speciesTrademarkCode) &&
                rpc.plurality === SubjectRolePlurality.Single
        );
        return roleCodes.filter(
            rc =>
                !(
                    singularConfigs.some(sc => sc.roleCodes.includes(rc)) &&
                    this.subjects.some(s => s.roleCode === rc)
                )
        );
    }
}

export class RolePluralityConfig {
    protectionDocTypeCode: string;
    trademarkCodeType: string[];
    roleCodes: string[];
    plurality: SubjectRolePlurality;
}

export enum SubjectRolePlurality {
    Single = 0,
    Multiple = 1
}
