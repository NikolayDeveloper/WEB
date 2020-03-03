import { NgModule } from '@angular/core';
import { QuillModule } from 'ngx-quill';

import { DeleteDialogComponent } from 'app/materials/components/delete-dialog/delete-dialog.component';
import { SearchDialogComponent } from 'app/materials/components/search-dialog/search-dialog.component';
import { DocumentWorkflowDialogComponent } from 'app/materials/components/workflow-dialog/workflow-dialog.component';
import { DetailsComponent } from 'app/materials/internal/components/details/details.component';
import { InternalComponent } from 'app/materials/internal/containers/internal/internal.component';
import { InternalMaterialsRoutingModule } from 'app/materials/internal/internal-materials-routing.module';
import { MaterialsModule } from 'app/materials/materials.module';
import { UndoDialogComponent } from 'app/shared/components/undo-dialog/undo-dialog.component';
import { SharedModule } from 'app/shared/shared.module';
import { PaymentsModule } from '../../payments/payments.module';

@NgModule({
  imports: [
    SharedModule,
    QuillModule,
    MaterialsModule,
    InternalMaterialsRoutingModule,
    PaymentsModule,
  ],
  declarations: [
    InternalComponent,
    DetailsComponent,
  ],
  bootstrap: [
    UndoDialogComponent,
    DocumentWorkflowDialogComponent,
    SearchDialogComponent,
    DeleteDialogComponent,
  ]
})
export class InternalMaterialsModule { }
