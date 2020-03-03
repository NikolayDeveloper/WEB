import { NgModule } from '@angular/core';
import { MaterialsModule } from 'app/materials/materials.module';
import { PostkzModule } from 'app/modules/postkz';

import { ExpertSearchModule } from '../expert-search/expert-search.module';
import { DeleteDialogComponent } from '../shared/components/delete-dialog/delete-dialog.component';
import { UndoDialogComponent } from '../shared/components/undo-dialog/undo-dialog.component';
import { SharedModule } from '../shared/shared.module';
import { SubjectsModule } from '../subjects/subjects.module';

import { OfficeWorkComponent } from './components/request-detail-components/office-work/office-work.component';
import { RequestDetailComponent } from './components/request-detail-components/request-detail/request-detail.component';
import { RequestComponent } from './components/request-detail-components/request/request.component';
import { RequestsComponent } from './containers/requests/requests.component';
import { RequestService } from './request.service';
import { RequestsRoutingModule } from './requests-routing.module';
import { DocumentsCompareService } from './services/documents-compare.service';
import { PaymentsModule } from '../payments/payments.module';
import { BibliographicDataModule } from '../bibliographic-data/bibliographic-data.module';
import { AutoAllocationService } from './auto-allocation.service';
import { ContractsModule } from '../contracts/contracts.module';
import { WorkflowDialogComponent } from '../shared/components/workflow-dialog/workflow-dialog.component';
import { WorkflowBusinessService } from './services/workflow-business.service';
import { SystemService } from 'app/shared/services/system.service';

@NgModule({
  imports: [
    SharedModule,
    PostkzModule,
    RequestsRoutingModule,
    ExpertSearchModule,
    MaterialsModule,
    SubjectsModule,
    PaymentsModule,
    BibliographicDataModule,
    ContractsModule,
  ],
  declarations: [
    RequestsComponent,
    RequestDetailComponent,
    RequestComponent,
    OfficeWorkComponent,
  ],
  bootstrap: [
    UndoDialogComponent,
    DeleteDialogComponent,
  ],
  providers: [
    RequestService,
    WorkflowBusinessService,
    DocumentsCompareService,
    AutoAllocationService,
    SystemService
  ],
  entryComponents: [
    WorkflowDialogComponent
  ]
})
export class RequestsModule { }
