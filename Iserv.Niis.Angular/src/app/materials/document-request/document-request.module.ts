import { NgModule } from '@angular/core';
import { QuillModule } from 'ngx-quill';

import { DeleteDialogComponent } from 'app/materials/components/delete-dialog/delete-dialog.component';
import { SearchDialogComponent } from 'app/materials/components/search-dialog/search-dialog.component';
import { DocumentWorkflowDialogComponent } from 'app/materials/components/workflow-dialog/workflow-dialog.component';

import { DocumentRequestRoutingModule } from 'app/materials/document-request/document-request-routing.module';
import { DetailsComponent } from 'app/materials/document-request/components/details/details.component';
import { DocumentRequestComponent } from 'app/materials/document-request/containers/document-request/document-request.component';

import { MaterialsModule } from 'app/materials/materials.module';
import { UndoDialogComponent } from 'app/shared/components/undo-dialog/undo-dialog.component';
import { SharedModule } from 'app/shared/shared.module';
import { PaymentsModule } from '../../payments/payments.module';

@NgModule({
  imports: [
    SharedModule,
    QuillModule,
    MaterialsModule,
    DocumentRequestRoutingModule,
    PaymentsModule,
  ],
  declarations: [
    DocumentRequestComponent,
    DetailsComponent
  ],
  bootstrap: [
    UndoDialogComponent,
    DocumentWorkflowDialogComponent,
    SearchDialogComponent,
    DeleteDialogComponent,
  ]
})
export class DocumentRequestModule { }
