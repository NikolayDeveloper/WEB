import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProtectedGuard, PublicGuard } from 'ngx-auth';
import { NgxPermissionsGuard } from 'ngx-permissions';
import { AccessDeniedComponent } from './access-denied.component';
import { PageNotFoundComponent } from './page-not-found.component';
import { PaymentsJournalComponent } from './payments-journal/components/payments-journal/payments-journal.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'journal',
    canActivate: [ProtectedGuard]
  },
  {
    path: 'login',
    loadChildren: 'app/login/login.module#LoginModule',
    canActivate: [PublicGuard]
  },
  {
    path: 'journal',
    loadChildren: 'app/journal/journal.module#JournalModule',
    canActivate: [ProtectedGuard, NgxPermissionsGuard],
    data: {
      permissions: {
        only: ['JournalModule'],
        redirectTo: '/403'
      }
    }
  },
  {
    path: 'requests',
    loadChildren: 'app/requests/requests.module#RequestsModule',
    canActivate: [ProtectedGuard]
  },
  {
    path: 'contracts',
    loadChildren: 'app/contracts/contracts.module#ContractsModule',
    canActivate: [ProtectedGuard]
  },
  {
    path: 'protectiondocs',
    loadChildren: 'app/protection-docs/protection-docs.module#ProtectionDocsModule',
    canActivate: [ProtectedGuard]
  },
  {
    path: 'search',
    loadChildren: 'app/search/search.module#SearchModule',
    canActivate: [ProtectedGuard, NgxPermissionsGuard],
    data: {
      permissions: {
        only: ['SearchModule'],
        redirectTo: '/403'
      }
    }
  },
  {
    path: 'bulletin',
    loadChildren: 'app/bulletin/bulletin.module#BulletinModule',
    canActivate: [ProtectedGuard, NgxPermissionsGuard],
    data: {
      permissions: {
        only: ['BulletinModule'],
        redirectTo: '/403'
      }
    }
  },
  {
    path: 'reports',
    loadChildren: 'app/reports/reports.module#ReportsModule',
  },
  {
    path: 'admin',
    loadChildren: 'app/administration/administration.module#AdministrationModule',
    canActivate: [ProtectedGuard, NgxPermissionsGuard],
    data: {
      permissions: {
        only: ['AdministrationModule'],
        redirectTo: '/403'
      }
    }
  },
  {
    path: 'materials/incoming',
    loadChildren: 'app/materials/incoming/incoming-materials.module#IncomingMaterialsModule',
    canActivate: [ProtectedGuard, NgxPermissionsGuard],
    data: {
      permissions: {
        only: ['MaterialsModule'],
        redirectTo: '/403'
      }
    }
  },
  {
    path: 'materials/outgoing',
    loadChildren: 'app/materials/outgoing/outgoing-materials.module#OutgoingMaterialsModule',
    canActivate: [ProtectedGuard, NgxPermissionsGuard],
    data: {
      permissions: {
        only: ['MaterialsModule'],
        redirectTo: '/403'
      }
    }
  },
  {
    path: 'materials/internal',
    loadChildren: 'app/materials/internal/internal-materials.module#InternalMaterialsModule',
    canActivate: [ProtectedGuard, NgxPermissionsGuard],
    data: {
      permissions: {
        only: ['MaterialsModule'],
        redirectTo: '/403'
      }
    }
  },
  {
    path: 'materials/document-request',
    loadChildren: 'app/materials/document-request/document-request.module#DocumentRequestModule',
    canActivate: [ProtectedGuard, NgxPermissionsGuard],
    data: {
      permissions: {
        only: ['MaterialsModule'],
        redirectTo: '/403'
      }
    }
  },
  {
    path: 'paymentsJournal',
    component: PaymentsJournalComponent,
  },
  {
    path: '403',
    component: AccessDeniedComponent
  },
  {
    path: '404',
    component: PageNotFoundComponent
  },
  {
    path: '**',
    redirectTo: '/404'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
