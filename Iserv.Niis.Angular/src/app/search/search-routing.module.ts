import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AdvancedSearchComponent } from './components/advanced-search/advanced-search.component';
import { SearchComponent } from './containers/search/search.component';

const routes: Routes = [
  {
    path: '',
    component: SearchComponent,
    children: [
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'advanced',
        component: AdvancedSearchComponent
      },
      { path: 'advanced', component: AdvancedSearchComponent },
      { path: 'expert', loadChildren: 'app/expert-search/expert-search.module#ExpertSearchModule' }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SearchRoutingModule { }
