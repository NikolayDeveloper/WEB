import { NgModule } from '@angular/core';

import { UndoDialogComponent } from '../../shared/components/undo-dialog/undo-dialog.component';
import { SharedModule } from '../../shared/shared.module';
import { DeleteDialogComponent } from '../components/delete-dialog/delete-dialog.component';
import { DocumentWorkflowDialogComponent } from '../components/workflow-dialog/workflow-dialog.component';
import { MaterialsModule } from '../materials.module';
import { WorkflowBusinessService } from '../services/workflow-business/workflow-business.service';
import { IncomingMaterialDetailsComponent } from './components/details/details.component';
import { IncomingMaterialsComponent } from './containers/incoming-materials/incoming-materials.component';
import { IncomingMaterialsRoutingModule } from './incoming-materials-routing.module';
import { SubjectsModule } from 'app/subjects/subjects.module';
import { PaymentsModule } from '../../payments/payments.module';
import { ProtectionDocsModule } from '../../protection-docs/protection-docs.module';
import { ProtectionDocsService } from '../../protection-docs/protection-docs.service';

@NgModule({
  imports: [
    SharedModule,
    IncomingMaterialsRoutingModule,
    MaterialsModule,
    SubjectsModule,
    PaymentsModule,
    ProtectionDocsModule,
  ],
  declarations: [
    IncomingMaterialDetailsComponent,
    IncomingMaterialsComponent
  ],
  providers: [
    WorkflowBusinessService,
    ProtectionDocsService,
  ],
  bootstrap: [
    UndoDialogComponent,
    DeleteDialogComponent,
    DocumentWorkflowDialogComponent
  ]
})
export class IncomingMaterialsModule { }
