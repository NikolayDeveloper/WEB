import { Component, OnInit, OnDestroy, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { MatDialogRef } from '@angular/material';
import { moment } from 'app/shared/shared.module';
import { BulletinSectionsService } from 'app/bulletin/services/bulletin-sections.service';
import { CreateBulletinSectionRequestDto } from 'app/bulletin/models/create-bulletin-section-request-dto';
import { ModalService } from 'app/shared/services/modal.service';

@Component({
  selector: 'app-create-bulletin-section-dialog',
  templateUrl: './create-bulletin-section-dialog.component.html',
  styleUrls: ['./create-bulletin-section-dialog.component.scss']
})
export class CreateBulletinSectionDialogComponent implements OnInit, OnDestroy {
  private onDestroy = new Subject();

  public bulletinSectionAdded: EventEmitter<any> = new EventEmitter<any>();
  public formGroup: FormGroup;

  constructor(
    private fb: FormBuilder,
    private bulletinSectionsService: BulletinSectionsService,
    private modalService: ModalService,
    public dialogRef: MatDialogRef<CreateBulletinSectionDialogComponent>
  ) {
  }

  ngOnInit() {
    this.buildForm();
  }

  buildForm() {
    const currentYear = moment(new Date()).format('YYYY');

    this.formGroup = this.fb.group({
      sectionName: new FormControl(currentYear, Validators.required)
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

    const requestDto = new CreateBulletinSectionRequestDto();
    requestDto.name = sectionName;

    this.bulletinSectionsService.createBulletinSection(requestDto)
      .takeUntil(this.onDestroy)
      .subscribe(() => {
        this.dialogRef.close();
        this.bulletinSectionAdded.emit(true);
        this.modalService
          .ok('Создание раздела бюллетеня.',
            'Раздел бюллетеня был успешно создан.')
          .subscribe();
      });
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

}
