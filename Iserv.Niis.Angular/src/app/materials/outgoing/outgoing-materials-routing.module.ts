import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DetailsComponent } from './components/details/details.component';
import { OutgoingMaterialsComponent } from 'app/materials/outgoing/containers/outgoing-materials/outgoing-materials.component';

const routes: Routes = [
  {
    path: '', component: OutgoingMaterialsComponent, children: [
      { path: 'create/:ownerType/:ownerId', component: DetailsComponent },
      { path: 'create/:ownerType', component: DetailsComponent },
      { path: ':id', component: DetailsComponent },
      { path: '**', redirectTo: '/404' },
    ]
  },
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ],
})
export class OutgoingMaterialsRoutingModule { }
