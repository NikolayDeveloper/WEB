import {
  getDocumentTypeRoute,
  DocumentType
} from '../../../materials/models/materials.model';
import 'rxjs/add/operator/takeUntil';

import { Component, OnDestroy, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Router } from '@angular/router';
import { Subject } from 'rxjs/Subject';

import { DocumentKind } from '../../../shared/models/create-document-model';
import { DictionaryService } from '../../../shared/services/dictionary.service';
import { DictionaryType } from '../../../shared/services/models/dictionary-type.enum';
import { SelectOption } from '../../../shared/services/models/select-option';
import { OwnerType } from 'app/shared/services/models/owner-type.enum';
import { DocumentKindDto } from '../../../shared/models/document-kind-dto-model';
import { AccessService } from '../../../shared/services/access.service';

@Component({
  selector: 'app-create-document-dialog',
  templateUrl: './create-document-dialog.component.html',
  styleUrls: ['./create-document-dialog.component.scss']
})
export class CreateDocumentDialogComponent implements OnInit, OnDestroy {
  formGroup: FormGroup;
  documentKindDic: DocumentKindDto[];
  documentKind = DocumentKind;
  selectedKind = DocumentKind.Request;
  dicPdTypes: SelectOption[];
  dicContractCategories: SelectOption[];

  ownerId: number;
  ownerType: OwnerType;

  unavailableProtectionDocTypes = [
    'DK',
    'A4',
    'A',
    'S1',
    'PR',
    'AP',
    'B_PD',
    'U_PD',
    'S2_PD',
    'TM_PD',
    'PN_PD',
    'SA_PD'
  ];

  private onDestroy = new Subject();

  constructor(
    private fb: FormBuilder,
    private accessService: AccessService,
    private router: Router,
    private dialogRef: MatDialogRef<CreateDocumentDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private data: any
  ) {
    this.ownerId = this.data.ownerId;
    this.ownerType = this.data.ownerType;
  }

  ngOnInit() {
    this.buildForm();

    this.accessService
      .getProtectionDocTypesSelectOption(DictionaryType.DicProtectionDocType)
      .takeUntil(this.onDestroy)
      .subscribe(pdTypes => {
        console.log(pdTypes);
        this.dicPdTypes = pdTypes.filter(p => {
          // 4 - это id товарного знака
          // Этот финт ушами, нужен что бы переименовать с "Товарный знак"
          if (p.id === 4) {
            p.nameRu = 'Заявка на Товарный знак';
          }
          return !this.unavailableProtectionDocTypes.includes(p.code);
        });
        console.log(this.dicPdTypes);
      });

      this.accessService
        .getDocumentKinds(this.ownerId)
        .takeUntil(this.onDestroy)
        .subscribe(data => this.documentKindDic = data);
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  onSubmit() {
    if (this.formGroup.invalid) {
      return;
    }
    this.formGroup.markAsPristine();
    this.dialogRef.close();

    const model = this.formGroup.value;
    const docKindComponentPath = this.documentKindDic.filter(
      d => d.kind === model.kind
    )[0].path;
    if (this.ownerType) {
      this.router.navigate([
        docKindComponentPath,
        'create',
        this.ownerType,
        this.ownerId
      ]);
    } else {
      if (model.typeId) {
        this.router.navigate([docKindComponentPath, 'create', model.typeId]);
      } else {
        this.router.navigate([docKindComponentPath, 'create', OwnerType.None]);
      }
    }
  }

  onCancel() {
    this.dialogRef.close();
  }

  onKindChange(value: DocumentKind) {
    this.selectedKind = value;
    this.formGroup.controls.typeId.reset();
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      kind: [this.selectedKind, Validators.required],
      typeId: []
    });
  }
}
