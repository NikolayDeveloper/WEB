import { Component, OnInit, Inject, OnDestroy } from '@angular/core';
import { ICGSRequestDto } from '../../models/icgs-request-dto';
import { RequestService } from '../../../requests/request.service';
import {
  MatDialogRef,
  MAT_DIALOG_DATA,
  MatSelectionListChange
} from '@angular/material';
import { Subject, Observable } from 'rxjs';
import { Router } from '@angular/router';
import { OwnerType } from '../../../shared/services/models/owner-type.enum';
import { ProtectionDocsService } from '../../../protection-docs/protection-docs.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import {DictionaryType} from '../../../shared/services/models/dictionary-type.enum';
import {DictionaryService} from '../../../shared/services/dictionary.service';

@Component({
  selector: 'app-icgs-split-dialog',
  templateUrl: './icgs-split-dialog.component.html',
  styleUrls: ['./icgs-split-dialog.component.scss']
})
export class IcgsSplitDialogComponent implements OnInit, OnDestroy {
  newIcgsRequests: ICGSRequestDto[];
  oldIcgsRequests: ICGSRequestDto[];
  icgsToSplit: ICGSRequestDto;
  ownerId: number;
  ownerType: OwnerType;
  dragIndex = -1;
  formGroup: FormGroup;
  icgs;
  private onDestroy = new Subject();

  constructor(
    private fb: FormBuilder,
    private dictionaryService: DictionaryService,
    private requestService: RequestService,
    private protectionDocService: ProtectionDocsService,
    private dialogRef: MatDialogRef<IcgsSplitDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private data: any
  ) {
    this.oldIcgsRequests = data.icgsRequests;
    this.newIcgsRequests = [];
    this.ownerId = data.ownerId;
    this.ownerType = data.ownerType;
    this.buildForm();
  }

  ngOnInit() {
    this.dictionaryService.getBaseDictionary(DictionaryType.DicICGS)
      .takeUntil(this.onDestroy)
      .subscribe(icgs => {
        this.icgs = icgs;
      });
    Observable.combineLatest(
      this.formGroup.get('newDescription').valueChanges,
      this.formGroup.get('oldDescription').valueChanges
    )
      .takeUntil(this.onDestroy)
      .subscribe(() => {
        this.onDescriptionChanged();
      });
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  dragStart(i) {
    this.dragIndex = i;
  }

  dropOld() {
    if (this.dragIndex > -1) {
      const newIcgs = this.newIcgsRequests[this.dragIndex];
      this.newIcgsRequests.splice(this.dragIndex, 1);
      this.oldIcgsRequests.push(newIcgs);
    }
  }

  dropNew() {
    if (this.dragIndex > -1) {
      const oldIcgs = this.oldIcgsRequests[this.dragIndex];
      this.oldIcgsRequests.splice(this.dragIndex, 1);
      this.newIcgsRequests.push(oldIcgs);
    }
  }

  dragEnd() {
    this.dragIndex = -1;
  }

  moveToNew(oldIcgs: any[]) {
    oldIcgs.forEach(o => {
      const index = this.oldIcgsRequests.findIndex(oi => oi.id === o.value);
      const newIcgs = this.oldIcgsRequests.find(oi => oi.id === o.value);
      if (index > -1) {
        newIcgs.isSplit = false;
        this.oldIcgsRequests.splice(index, 1);
        if (!this.newIcgsRequests.includes(newIcgs)) {
          this.newIcgsRequests.push(newIcgs);
        }
      }
    });
  }

  moveToOld(newIcgs: any[]) {
    newIcgs.forEach(n => {
      const index = this.newIcgsRequests.findIndex(ni => ni.id === n.value);
      const oldIcgs = this.newIcgsRequests.find(ni => ni.id === n.value);
      if (index > -1) {
        oldIcgs.isSplit = false;
        this.newIcgsRequests.splice(index, 1);
        if (!this.oldIcgsRequests.includes(oldIcgs)) {
          this.oldIcgsRequests.push(oldIcgs);
        }
      }
    });
  }

  isSplitDisabled(oldSelection: any[], newSelection: any[]): boolean {
    if (!oldSelection || oldSelection.length === 0) {
      if (!newSelection || newSelection.length === 0) {
        return false;
      }
      return newSelection
        .map(o => o.value)
        .some(o => this.oldIcgsRequests.some(n => n.id === o));
    } else {
      return oldSelection
        .map(o => o.value)
        .some(o => this.newIcgsRequests.some(n => n.id === o));
    }
  }

  split(oldSelection: any[], newSelection: any[]) {
    if (!oldSelection || oldSelection.length === 0) {
      if (!newSelection || newSelection.length === 0) {
        return;
      } else {
        newSelection.forEach(n => {
          const index = this.newIcgsRequests.findIndex(ni => ni.id === n.value);
          const oldIcgs = this.newIcgsRequests.find(ni => ni.id === n.value);
          if (index > -1) {
            oldIcgs.isSplit = true;
            this.oldIcgsRequests.push(oldIcgs);
            this.icgsToSplit = oldIcgs;
          }
        });
      }
    } else {
      oldSelection.forEach(o => {
        const index = this.oldIcgsRequests.findIndex(oi => oi.id === o.value);
        const newIcgs = this.oldIcgsRequests.find(oi => oi.id === o.value);
        if (index > -1) {
          newIcgs.isSplit = true;
          this.newIcgsRequests.push(newIcgs);
          this.icgsToSplit = newIcgs;
        }
      });
    }
    const icgsClass = this.icgs.find(icgs => icgs.id === this.icgsToSplit.icgsId);
    this.icgsToSplit.description += `\n${icgsClass.description || ' '}`;
    this.formGroup.get('oldDescription').setValue(this.icgsToSplit.description);
  }

  onDescriptionChanged() {
    if (!!this.icgsToSplit) {
      const newIcgsToSplit = this.newIcgsRequests.find(
        n => n.id === this.icgsToSplit.id
      );
      newIcgsToSplit.description = this.formGroup.get('newDescription').value;
      newIcgsToSplit.descriptionNew = this.formGroup.get(
        'oldDescription'
      ).value;
    }
  }

  onSelectionChange(event: MatSelectionListChange, icgs: ICGSRequestDto[]) {
    const selectedIcgsRequestId = event.option.value;
    const index = icgs.findIndex(
      n => n.id === selectedIcgsRequestId
    );
    if (index !== -1) {
      const newSplitIcgs = this.newIcgsRequests.find(n => n.id === selectedIcgsRequestId);
      this.formGroup.get('oldDescription').setValue(newSplitIcgs.descriptionNew);
      this.formGroup.get('newDescription').setValue(newSplitIcgs.description);
    }
  }

  onCancel() {
    this.dialogRef.close();
  }

  onOk() {
    switch (this.ownerType) {
      case OwnerType.Request:
        this.requestService
          .splitRequest(this.ownerId, this.newIcgsRequests)
          .takeUntil(this.onDestroy)
          .subscribe(newRequestId => {
            this.dialogRef.close(newRequestId);
          });
        break;
      case OwnerType.ProtectionDoc:
        this.protectionDocService
          .splitProtectionDoc(this.ownerId, this.newIcgsRequests)
          .takeUntil(this.onDestroy)
          .subscribe(newRequestId => {
            this.dialogRef.close(newRequestId);
          });
        break;
    }
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      newDescription: ['', Validators.required],
      oldDescription: ['', Validators.required]
    });
  }
}
