import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProtectedGuard } from 'ngx-auth';
import { NgxPermissionsGuard } from 'ngx-permissions';
import { BulletinSectionsComponent } from './components/bulletin-sections/bulletin-sections.component';
import { BulletinSectionDetailsComponent } from './components/bulletin-section-details/bulletin-section-details.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'sections',
    pathMatch: 'full',
    canActivate: [ProtectedGuard, NgxPermissionsGuard],
    data: {
      permissions: {
        only: [
          'BulletinModule'
        ],
        redirectTo: '/403'
      }
    }
  },
  { path: 'sections', component: BulletinSectionsComponent },
  { path: 'sections/:id', component: BulletinSectionDetailsComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BulletinRoutingModule { }
