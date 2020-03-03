import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {
  IncomingMaterialsComponent,
} from 'app/materials/incoming/containers/incoming-materials/incoming-materials.component';

import { IncomingMaterialDetailsComponent } from './components/details/details.component';

const routes: Routes = [
  {
    path: '', component: IncomingMaterialsComponent, children: [
      { path: 'create/:ownerType/:ownerId', component: IncomingMaterialDetailsComponent},
      { path: 'create/:ownerType', component: IncomingMaterialDetailsComponent},
      { path: ':id', component: IncomingMaterialDetailsComponent},
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
export class IncomingMaterialsRoutingModule { }
