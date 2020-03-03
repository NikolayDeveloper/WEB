import { Component, OnInit, EventEmitter, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material';
import { Subject } from 'rxjs';
import { ModalService } from 'app/shared/services/modal.service';
import { BulletinsService } from 'app/bulletin/services/bulletins.service';

@Component({
  selector: 'app-create-bulletin-dialog',
  templateUrl: './create-bulletin-dialog.component.html',
  styleUrls: ['./create-bulletin-dialog.component.scss']
})
export class CreateBulletinDialogComponent implements OnInit, OnDestroy {
  private onDestroy = new Subject();

  public bulletinAdded: EventEmitter<any> = new EventEmitter<any>();
  public formGroup: FormGroup;

  constructor(
    private fb: FormBuilder,
    private bulletinsService: BulletinsService,
    private modalService: ModalService,
    public dialogRef: MatDialogRef<CreateBulletinDialogComponent>
  ) {
  }

  ngOnInit() {
    this.buildForm();
  }

  buildForm() {
    this.formGroup = this.fb.group({
      bulletinNumber: new FormControl('', Validators.required)
    });
  }

  onCancelClick(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    if (this.formGroup.invalid) {
      return;
    }

    /*
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
      */
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

}
