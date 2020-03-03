import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DocumentRequestComponent } from './containers/document-request/document-request.component';
import { DetailsComponent } from './components/details/details.component';

const routes: Routes = [
  {
    path: '', component: DocumentRequestComponent, children: [
      { path: 'create/:ownerType/:ownerId', component: DetailsComponent },
      { path: 'create/:ownerType', component: DetailsComponent },
      { path: ':id', component: DetailsComponent },
      { path: '**', redirectTo: '/404' },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DocumentRequestRoutingModule { }
