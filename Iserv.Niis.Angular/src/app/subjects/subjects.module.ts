import { NgModule } from '@angular/core';
import { PostkzModule } from 'app/modules/postkz';
import { IMaskModule } from 'angular-imask';

import { WorkflowBusinessService } from '../requests/services/workflow-business.service';
import { SharedModule } from '../shared/shared.module';
import { SubjectFormDialogComponent } from './components/subject-form-dialog/subject-form-dialog.component';
import { SubjectsListComponent } from './components/subjects-list/subjects-list.component';
import { SubjectsComponent } from './containers/subjects.component';
import { SubjectsService } from './services/subjects.service';
import { SubjectsSearchDialogComponent } from './components/subjects-search-dialog/subjects-search-dialog.component';
import { SubjectsSearchFormComponent } from './components/subjects-search-form/subjects-search-form.component';
import { SubjectDeleteDialogComponent } from 'app/subjects/components/subject-delete-dialog/subject-delete-dialog.component';
import { SubjectsSearchComponent } from 'app/subjects/components/subjects-search/subjects-search.component';
import { SystemService } from 'app/shared/services/system.service';
import { SubjectCreateDialogComponent } from 'app/subjects/components/subject-create-dialog/subject-create-dialog.component';
import { ContactsComponent } from 'app/subjects/components/contacts/contacts.component';

@NgModule({
  exports: [
    SubjectsComponent,
    SubjectsSearchDialogComponent,
    SubjectFormDialogComponent,
    SubjectsSearchFormComponent,
    SubjectDeleteDialogComponent,
    SubjectsSearchComponent
  ],
  imports: [
    SharedModule,
    PostkzModule,
    IMaskModule
  ],
  declarations: [
    SubjectsComponent,
    SubjectsListComponent,
    SubjectFormDialogComponent,
    SubjectsSearchDialogComponent,
    SubjectsSearchFormComponent,
    SubjectDeleteDialogComponent,
    SubjectsSearchComponent,
    SubjectCreateDialogComponent,
    ContactsComponent
  ],
  providers: [
    SubjectsService,
    WorkflowBusinessService,
    SystemService
  ],
  entryComponents: [
    SubjectFormDialogComponent,
    SubjectsSearchDialogComponent,
    SubjectDeleteDialogComponent,
    SubjectsSearchComponent,
    SubjectCreateDialogComponent
  ]
})
export class SubjectsModule { }
