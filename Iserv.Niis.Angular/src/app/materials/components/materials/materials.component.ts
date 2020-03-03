import { Component, Input } from '@angular/core';
import { OnDestroy } from '@angular/core/src/metadata/lifecycle_hooks';
import { MatDialog } from '@angular/material';
import { Router } from '@angular/router';
import { CreateDocumentDialogComponent } from 'app/journal/components/create-document-dialog/create-document-dialog.component';
import { MaterialsService } from 'app/materials/services/materials.service';
import { IntellectualPropertyDetails } from 'app/shared/models/intellectual-property-details';
import { DocumentsService } from 'app/shared/services/documents.service';
import { saveAs } from 'file-saver';
import { Subject } from 'rxjs/Subject';

@Component({
  selector: 'app-materials',
  templateUrl: './materials.component.html',
  styleUrls: ['./materials.component.scss']
})
export class MaterialsComponent implements OnDestroy {
  editMode: boolean;

  @Input() owner: IntellectualPropertyDetails;
  @Input() disabled: boolean;
  private onDestroy = new Subject();

  constructor(
    private materialsService: MaterialsService,
    private documentsService: DocumentsService,
    private router: Router,
    public dialog: MatDialog
  ) {}

  ngOnDestroy() {
    this.onDestroy.next();
  }

  download(attachmentData) {
    this.documentsService
      .getDocumentPdf(attachmentData.id, !attachmentData.hasTemplate, true)
      .takeUntil(this.onDestroy)
      .subscribe(blob => {
        if (attachmentData.isDownload) {
          saveAs(blob, (blob as any).name || attachmentData.name);
        } else {
          window.open(window.URL.createObjectURL(blob, { oneTimeOnly: true }));
        }
      });
  }
  onCreateMaterialClick() {
    this.dialog.open(CreateDocumentDialogComponent, {
      data: {
        ownerType: this.owner.ownerType,
        ownerId: this.owner.id
      },
      width: '500px'
    });
  }

  onGenerateInternalRegisterClick() {
    this.materialsService
      .generateInternalRegister(this.owner.id, this.owner.ownerType)
      .takeUntil(this.onDestroy)
      .subscribe(blob => {
        window.open(window.URL.createObjectURL(blob, { oneTimeOnly: true }));
      });
  }
}
