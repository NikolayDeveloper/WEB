import { NgModule } from '@angular/core';

import { UndoDialogComponent } from '../shared/components/undo-dialog/undo-dialog.component';
import { SharedModule } from '../shared/shared.module';
import { SubjectsModule } from 'app/subjects/subjects.module';
import { PaymentsModule } from '../payments/payments.module';
import { ProtectionDocsModule } from '../protection-docs/protection-docs.module';
import { ProtectionDocsService } from '../protection-docs/protection-docs.service';
import { DocumentFlowComponent } from './components/document-flow/document-flow.component';
import { DocumentFlowFilterComponent } from './components/document-flow-filter/document-flow-filter.component';

import { DeleteDialogComponent } from 'app/shared/components/delete-dialog/delete-dialog.component';
import { DocumentWorkflowDialogComponent } from 'app/materials/components/workflow-dialog/workflow-dialog.component';
import { SystemService } from 'app/shared/services/system.service';

@NgModule({
  imports: [
    SharedModule,
    SubjectsModule,
    PaymentsModule,
    ProtectionDocsModule
  ],
  providers: [
    ProtectionDocsService,
    SystemService
  ],
  bootstrap: [
    UndoDialogComponent,
    DeleteDialogComponent,
    DocumentWorkflowDialogComponent
  ]
})
export class DocumentFlowModule { }
