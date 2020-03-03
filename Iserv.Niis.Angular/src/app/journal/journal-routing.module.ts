import {
  IncomingMaterialTasksComponent,
} from './components/incoming-material-tasks/incoming-material-tasks.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProtectedGuard } from 'ngx-auth';
import { NgxPermissionsGuard } from 'ngx-permissions';

import { AuthenticationService } from '../shared/authentication/authentication.service';
import { PermissionConstantService } from '../shared/authentication/perpission-constant.service';
import { JournalStaffComponent } from './components/journal-staff/journal-staff.component';
import { JournalTasksComponent } from './components/journal-tasks/journal-tasks.component';
import { JournalComponent } from './containers/journal/journal.component';
import { JournalAutoAllocationComponent } from './components/journal-auto-allocation/journal-auto-allocation.component';
import { DocumentFlowComponent } from 'app/document-flow/components/document-flow/document-flow.component';


const routes: Routes = [
  {
    path: '',
    component: JournalComponent,
    canActivate: [ProtectedGuard, NgxPermissionsGuard],
    data: {
      permissions: {
        only: [
          'JournalModule'
        ],
        redirectTo: '/403'
      }
    },
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'document-flow', component: DocumentFlowComponent },
      { path: 'tasks/incoming', component: IncomingMaterialTasksComponent },
      { path: 'tasks/:id', component: JournalTasksComponent },
      { path: 'staff', component: JournalStaffComponent },
      { path: 'document-flow', component: DocumentFlowComponent },
      { path: 'autoallocation', component: JournalAutoAllocationComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class JournalRoutingModule {
  constructor(
    authenticationService: AuthenticationService,
    permissionConstantsService: PermissionConstantService
  ) { }
}
