import { Component, OnInit, EventEmitter, Inject, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { Subject } from 'rxjs';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { BulletinSectionsService } from 'app/bulletin/services/bulletin-sections.service';
import { ModalService } from 'app/shared/services/modal.service';
import { BulletinSectionListDto } from 'app/bulletin/models/bulletin-section-list-dto';
import { EditBulletinSectionRequestDto } from 'app/bulletin/models/edit-bulletin-section-request-dto';

@Component({
  selector: 'app-edit-bulletin-section-dialog',
  templateUrl: './edit-bulletin-section-dialog.component.html',
  styleUrls: ['./edit-bulletin-section-dialog.component.scss']
})
export class EditBulletinSectionDialogComponent implements OnInit, OnDestroy {
  private onDestroy = new Subject();
  private bulletinSectionListDto: BulletinSectionListDto;

  public formGroup: FormGroup;
  public bulletinSectionEdited: EventEmitter<any> = new EventEmitter<any>();

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<EditBulletinSectionDialogComponent>,
    @Inject(MAT_DIALOG_DATA) dialogData: any,
    private bulletinSectionsService: BulletinSectionsService,
    private modalService: ModalService
  ) {
    this.bulletinSectionListDto = dialogData.BulletinSectionListDto;
  }

  ngOnInit() {
    this.buildForm();
  }

  buildForm() {
    this.formGroup = this.fb.group({
      sectionName: new FormControl(this.bulletinSectionListDto.name, Validators.required)
    });
  }

  onCancelClick(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    if (this.formGroup.invalid) {
      return;
    }

    const sectionName = this.formGroup.get('sectionName').value;

    const requestDto = new EditBulletinSectionRequestDto();
    requestDto.name = sectionName;

    this.bulletinSectionsService.editBulletinSection(this.bulletinSectionListDto.id, requestDto)
      .takeUntil(this.onDestroy)
      .subscribe(() => {
        this.dialogRef.close();
        this.bulletinSectionEdited.emit(true);
        this.modalService
          .ok('Редактирование раздела бюллетеня.',
            'Раздел бюллетеня был успешно отредактирован.')
          .subscribe();
      });
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

}
