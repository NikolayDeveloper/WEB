import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { JournalModule } from 'app/journal/journal.module';
import { PostkzModule } from 'app/modules/postkz';
import { SearchService } from 'app/search/services/search.service';
import { QuillModule } from 'ngx-quill';
import { DialogForPasswordComponent } from '../login/dialog/dialog-for-password.component';
import { LoginModule } from '../login/login.module';
import { UndoDialogComponent } from '../shared/components/undo-dialog/undo-dialog.component';
import { SharedModule } from '../shared/shared.module';
import { SubjectsService } from '../subjects/services/subjects.service';
import { AttachToOwnerFormComponent } from './components/attach-to-owner-form/attach-to-owner-form.component';
import { DeleteDialogComponent } from './components/delete-dialog/delete-dialog.component';
import { MaterialsListComponent } from './components/materials-list/materials-list.component';
import { MaterialsComponent } from './components/materials/materials.component';
import { SearchDialogComponent } from './components/search-dialog/search-dialog.component';
import { DocumentWorkflowDialogComponent } from './components/workflow-dialog/workflow-dialog.component';
import { AttachmentSearchService } from './services/attachment-search.service';
import { WorkflowBusinessService } from './services/workflow-business/workflow-business.service';
import { ChangeExecutorDialogComponent } from './components/change-executor-dialog/change-executor-dialog.component';
import { IncomingAnswerService } from 'app/shared/components/incoming-answer/incoming-answer.service';
import { DocumenLinkComponent } from './components/documen-link/documen-link.component';
import { MaterialListComponent } from './components/material-list/material-list.component';
import { MaterialListService } from './components/material-list/material-list.service';
import { SystemService } from 'app/shared/services/system.service';
import { AttachmentsComponent } from './components/attachments/attachments.component';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    PostkzModule,
    QuillModule,
    JournalModule,
    LoginModule
  ],
  exports: [
    MaterialsComponent,
    SearchDialogComponent,
    DeleteDialogComponent,
    DocumentWorkflowDialogComponent,
    AttachToOwnerFormComponent,
    ChangeExecutorDialogComponent,
    DocumenLinkComponent,
    MaterialListComponent,
    AttachmentsComponent
  ],
  declarations: [
    MaterialsComponent,
    SearchDialogComponent,
    DeleteDialogComponent,
    DocumentWorkflowDialogComponent,
    MaterialsListComponent,
    AttachToOwnerFormComponent,
    ChangeExecutorDialogComponent,
    DocumenLinkComponent,
    MaterialListComponent,
    AttachmentsComponent
  ],
  providers: [
    WorkflowBusinessService,
    IncomingAnswerService,
    MaterialListService,
    SubjectsService,
    SubjectsService,
    SearchService,
    AttachmentSearchService,
    SystemService
  ],
  bootstrap: [
    UndoDialogComponent,
    DeleteDialogComponent,
    DocumentWorkflowDialogComponent,
    SearchDialogComponent,
    ChangeExecutorDialogComponent,
    DocumenLinkComponent,
    MaterialListComponent,
  ],
  entryComponents: [
    DocumentWorkflowDialogComponent,
    DialogForPasswordComponent,
    ChangeExecutorDialogComponent,
    DocumenLinkComponent,
    MaterialListComponent]
})
export class MaterialsModule {}
