import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DocumentFlowComponent } from './components/document-flow/document-flow.component';

const routes: Routes = [
  {
    path: '', component: DocumentFlowComponent, children: [      
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
