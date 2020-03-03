import { NgModule } from '@angular/core';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { CalendarModule } from 'angular-calendar';
import { AdministrationRoutingModule } from 'app/administration/administration-routing.module';
import { SharedModule } from 'app/shared/shared.module';

import { CalendarService } from './calendar.service';
import { CalendarMonthComponent } from './components/calendar/calendar-month/calendar-month.component';
import { CalendarYearComponent } from './components/calendar/calendar-year/calendar-year.component';
import { CalendarComponent } from './components/calendar/calendar.component';
import { RoleFormDialogComponent } from './components/roles/role-form-dialog/role-form-dialog.component';
import { RolesListComponent } from './components/roles/roles-list/roles-list.component';
import { RolesComponent } from './components/roles/roles.component';
import { UserFormDialogComponent } from './components/users/user-form-dialog/user-form-dialog.component';
import { UsersListComponent } from './components/users/users-list/users-list.component';
import { UsersComponent } from './components/users/users.component';
import { AdministrationComponent } from './containers/administration/administration.component';
import { RolesService } from './roles.service';
import { UsersService } from './users.service';
import { TreeFormDialogIpcComponent } from './components/users/tree-form-dialog-ipc/tree-form-dialog-ipc.component';
import { SystemService } from 'app/shared/services/system.service';

@NgModule({
  imports: [
    SharedModule,
    AdministrationRoutingModule,
    CalendarModule.forRoot(),
    MatButtonToggleModule
  ],

  declarations: [
    AdministrationComponent,
    UsersComponent,
    UsersListComponent,
    RolesComponent,
    RolesListComponent,
    CalendarComponent,
    CalendarYearComponent,
    CalendarMonthComponent,
    RoleFormDialogComponent,
    UserFormDialogComponent,
    TreeFormDialogIpcComponent
  ],
  providers: [
    UsersService,
    RolesService,
    CalendarService,
    SystemService
  ],
  entryComponents: [
    RoleFormDialogComponent,
    UserFormDialogComponent,
    TreeFormDialogIpcComponent
  ]
})
export class AdministrationModule { }
