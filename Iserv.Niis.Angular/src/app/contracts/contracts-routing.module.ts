import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ContractsComponent } from './containers/contracts/contracts.component';
import { ContractDetailComponent } from './components/contract-detail-components/contract-detail/contract-detail.component';

const routes: Routes = [
  {
    path: '',
    component: ContractsComponent,
    children: [
      { path: 'create/:typeId', component: ContractDetailComponent },
      { path: ':id', component: ContractDetailComponent },
      { path: '**', redirectTo: '/404' },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ContractsRoutingModule { }
