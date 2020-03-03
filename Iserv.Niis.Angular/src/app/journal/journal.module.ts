import { NgModule } from '@angular/core';
import { MatDialogModule } from '@angular/material';
import {
  IncomingMaterialTasksComponent,
} from 'app/journal/components/incoming-material-tasks/incoming-material-tasks.component';
import { NgxPermissionsModule } from 'ngx-permissions';

import { IncomingMaterialsService } from '../materials/incoming/incoming-materials.service';
import { SharedModule } from '../shared/shared.module';
import { CreateDocumentDialogComponent } from './components/create-document-dialog/create-document-dialog.component';
import { ImportDocumentDialogComponent } from './components/import-document-dialog/import-document-dialog.component';
import { JournalAutoAllocationComponent } from './components/journal-auto-allocation/journal-auto-allocation.component';
import { JournalStaffComponent } from './components/journal-staff/journal-staff.component';
import { JournalTasksComponent } from './components/journal-tasks/journal-tasks.component';
import {
  ResultAutoAllocationDialogComponent,
} from './components/result-auto-allocation-dialog/result-auto-allocation-dialog.component';
import {
  ResultEmploymentExpertsDialogComponent,
} from './components/result-employment-experts-dialog/result-employment-experts-dialog.component';
import { SelectUserDialogComponent } from './components/select-user-dialog/select-user-dialog.component';
import { JournalComponent } from './containers/journal/journal.component';
import { JournalRoutingModule } from './journal-routing.module';
import { JournalService } from './journal.service';
import { DocumentFlowComponent } from 'app/document-flow/components/document-flow/document-flow.component';
import { DocumentFlowService } from 'app/document-flow/services/document-flow.service';
import { DocumentFlowFilterComponent } from 'app/document-flow/components/document-flow-filter/document-flow-filter.component';
import { SystemService } from 'app/shared/services/system.service';

@NgModule({
  imports: [
    SharedModule,
    JournalRoutingModule,
    NgxPermissionsModule.forChild(),
    MatDialogModule,
  ],
  declarations: [
    DocumentFlowComponent,
    JournalComponent,
    JournalStaffComponent,
    JournalTasksComponent,
    IncomingMaterialTasksComponent,
    CreateDocumentDialogComponent,
    ImportDocumentDialogComponent,
    SelectUserDialogComponent,
    JournalAutoAllocationComponent,
    ResultAutoAllocationDialogComponent,
    ResultEmploymentExpertsDialogComponent,
    DocumentFlowFilterComponent,
  ],
  bootstrap: [
    ResultAutoAllocationDialogComponent,
    ResultEmploymentExpertsDialogComponent
  ],
  providers: [
    DocumentFlowService,
    JournalService,
    IncomingMaterialsService,
    SystemService
  ],
  entryComponents: [
    CreateDocumentDialogComponent,
    ImportDocumentDialogComponent,
    SelectUserDialogComponent
  ]
})
export class JournalModule { }
