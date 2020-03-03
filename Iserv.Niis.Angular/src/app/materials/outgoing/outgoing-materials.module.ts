import { NgModule } from '@angular/core';
import { QuillModule } from 'ngx-quill';

import { ContractService } from '../../contracts/contract.service';
import { UndoDialogComponent } from '../../shared/components/undo-dialog/undo-dialog.component';
import { SharedModule } from '../../shared/shared.module';
import { NotificationsService } from '../../shared/services/notifications.service';
import { SubjectsService } from '../../subjects/services/subjects.service';
import { SearchDialogComponent } from '../components/search-dialog/search-dialog.component';
import { DocumentWorkflowDialogComponent } from '../components/workflow-dialog/workflow-dialog.component';
import { MaterialsModule } from '../materials.module';
import { WorkflowBusinessService } from '../services/workflow-business/workflow-business.service';
import { DetailsComponent } from './components/details/details.component';
import { OutgoingMaterialsComponent } from './containers/outgoing-materials/outgoing-materials.component';
import { OutgoingMaterialsRoutingModule } from './outgoing-materials-routing.module';
import { SubjectsModule } from 'app/subjects/subjects.module';
import { PaymentsModule } from '../../payments/payments.module';
import { ProtectionDocsModule } from '../../protection-docs/protection-docs.module';
import { ProtectionDocsService } from '../../protection-docs/protection-docs.service';
import { AttachToOwnerFormComponent } from '../components/attach-to-owner-form/attach-to-owner-form.component';
import { SystemService } from 'app/shared/services/system.service';

@NgModule({
  imports: [
    SharedModule,
    OutgoingMaterialsRoutingModule,
    MaterialsModule,
    QuillModule,
    SubjectsModule,
    PaymentsModule,
    ProtectionDocsModule,
  ],
  declarations: [
    DetailsComponent,
    OutgoingMaterialsComponent,
  ],
  providers: [
    WorkflowBusinessService,
    ContractService,
    SubjectsService,
    NotificationsService,
    ProtectionDocsService,
    SystemService
  ],
  bootstrap: [
    UndoDialogComponent,
    DocumentWorkflowDialogComponent,
    SearchDialogComponent
  ]
})
export class OutgoingMaterialsModule { }
