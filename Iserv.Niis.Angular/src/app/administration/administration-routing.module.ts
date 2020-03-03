import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { UsersComponent } from 'app/administration/components/users/users.component';
import { AdministrationComponent } from 'app/administration/containers/administration/administration.component';

import { RolesComponent } from './components/roles/roles.component';
import { CalendarComponent } from './components/calendar/calendar.component';

const routes = [
    {
        path: '', component: AdministrationComponent, children: [
            { path: '', pathMatch: 'full', redirectTo: 'users', component: UsersComponent },
            { path: 'users', component: UsersComponent },
            { path: 'roles', component: RolesComponent },
            { path: 'calendar', component: CalendarComponent }
        ]
    }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AdministrationRoutingModule { }
