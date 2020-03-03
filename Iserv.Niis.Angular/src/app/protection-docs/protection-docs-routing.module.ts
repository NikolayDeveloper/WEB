import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProtectionDocsComponent } from './containers/protection-docs/protection-docs.component';
import { ProtectionDocDetailsComponent } from './components/protection-doc-details/protection-doc-details.component';

const routes: Routes = [
  {
    path: '', component: ProtectionDocsComponent, children: [
      { path: 'create/:typeId', component: ProtectionDocDetailsComponent },
      { path: ':id', component: ProtectionDocDetailsComponent },
      { path: ':id/:selectedIndex', component: ProtectionDocDetailsComponent },
      { path: ':id/:selectedIndex/:selectedSubIndex', component: ProtectionDocDetailsComponent },
      { path: '**', redirectTo: '/404' },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProtectionDocsRoutingModule { }
