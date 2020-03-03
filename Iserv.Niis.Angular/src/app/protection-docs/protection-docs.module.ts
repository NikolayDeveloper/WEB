import { NgModule } from '@angular/core';
import { ProtectionDocsComponent } from './containers/protection-docs/protection-docs.component';
import { ProtectionDocDetailsComponent } from './components/protection-doc-details/protection-doc-details.component';
import { ProtectionDocComponent } from './components/protection-doc/protection-doc.component';
import { SharedModule } from '../shared/shared.module';
import { ProtectionDocsRoutingModule } from './protection-docs-routing.module';
import { MaterialsModule } from '../materials/materials.module';
import { SubjectsModule } from '../subjects/subjects.module';
import { PaymentsModule } from '../payments/payments.module';
import { BibliographicDataModule } from '../bibliographic-data/bibliographic-data.module';
import { UndoDialogComponent } from '../shared/components/undo-dialog/undo-dialog.component';
import { ProtectionDocsService } from './protection-docs.service';
import { WorkflowBusinessService } from './services/workflow-business.service';
import { ContractsModule } from '../contracts/contracts.module';
import { WorkflowDialogComponent } from '../shared/components/workflow-dialog/workflow-dialog.component';
import { AuthorsCertificatesDialogComponent } from './components/authors-certificates-dialog/authors-certificates-dialog.component';

@NgModule({
  imports: [
    SharedModule,
    ProtectionDocsRoutingModule,
    MaterialsModule,
    SubjectsModule,
    PaymentsModule,
    BibliographicDataModule,
    ContractsModule
  ],
  declarations: [
    ProtectionDocsComponent,
    ProtectionDocDetailsComponent,
    ProtectionDocComponent,
    AuthorsCertificatesDialogComponent
  ],
  bootstrap: [
    UndoDialogComponent,
  ],
  providers: [
    ProtectionDocsService,
    WorkflowBusinessService,
  ],
  entryComponents: [
    WorkflowDialogComponent,
    AuthorsCertificatesDialogComponent
  ]
})
export class ProtectionDocsModule { }
