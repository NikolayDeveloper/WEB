import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DetailsComponent } from 'app/materials/internal/components/details/details.component';
import { InternalComponent } from 'app/materials/internal/containers/internal/internal.component';

const routes: Routes = [
  {
    path: '', component: InternalComponent, children: [
      { path: 'create/:ownerType/:ownerId', component: DetailsComponent },
      { path: 'create/:ownerType', component: DetailsComponent },
      { path: ':id', component: DetailsComponent },
      { path: '**', redirectTo: '/404' },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class InternalMaterialsRoutingModule { }
