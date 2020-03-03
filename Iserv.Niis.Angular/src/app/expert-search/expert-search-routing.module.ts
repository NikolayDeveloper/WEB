import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { IndustrialdesignComponent } from './components/industrialdesign/industrialdesign.component';
import { InventionComponent } from './components/invention/invention.component';
import { TrademarkComponent } from './components/trademark/trademark.component';
import { UsefulmodelComponent } from './components/usefulmodel/usefulmodel.component';
import { ExpertSearchComponent } from './containers/expert-search/expert-search.component';

const routes: Routes = [{
    path: '',
    component: ExpertSearchComponent,
    children: [
        { path: '', pathMatch: 'full', redirectTo: 'trademark', component: TrademarkComponent },
        { path: 'invention', component: InventionComponent },
        { path: 'trademark', component: TrademarkComponent },
        { path: 'usefulmodel', component: UsefulmodelComponent },
        { path: 'industrialdesign', component: IndustrialdesignComponent },
    ]
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ExpertSearchRoutingModule {

}
