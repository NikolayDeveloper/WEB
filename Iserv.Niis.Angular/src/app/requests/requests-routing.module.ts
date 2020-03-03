import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RequestsComponent } from './containers/requests/requests.component';
import { RequestDetailComponent } from './components/request-detail-components/request-detail/request-detail.component';

const routes: Routes = [
  {
    path: '', component: RequestsComponent, children: [
      { path: 'create/:typeId', component: RequestDetailComponent },
      { path: ':id', component: RequestDetailComponent },
      { path: ':id/:selectedIndex', component: RequestDetailComponent },
      { path: ':id/:selectedIndex/:selectedSubIndex', component: RequestDetailComponent },
      { path: '**', redirectTo: '/404' },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RequestsRoutingModule { }
