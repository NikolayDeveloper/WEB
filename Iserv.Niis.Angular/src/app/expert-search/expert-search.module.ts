import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ColumnConfigModule } from 'app/modules/column-config/column-config.module';
import { DiffMatchPatchModule } from 'ng-diff-match-patch';

import { QueryParamsStorageService } from '../search/services/query-params-storage.service';
import { SharedModule } from '../shared/shared.module';
import {
    IndustrialdesignFormComponent,
} from './components/industrialdesign/industrialdesign-form/industrialdesign-form.component';
import {
    IndustrialdesignListComponent,
} from './components/industrialdesign/industrialdesign-list/industrialdesign-list.component';
import { IndustrialdesignComponent } from './components/industrialdesign/industrialdesign.component';
import { InventionFormComponent } from './components/invention/invention-form/invention-form.component';
import { InventionListComponent } from './components/invention/invention-list/invention-list.component';
import { InventionComponent } from './components/invention/invention.component';
import { TrademarkFormComponent } from './components/trademark/trademark-form/trademark-form.component';
import { TrademarkListComponent } from './components/trademark/trademark-list/trademark-list.component';
import { TrademarkComponent } from './components/trademark/trademark.component';
import { UsefulmodelFormComponent } from './components/usefulmodel/usefulmodel-form/usefulmodel-form.component';
import { UsefulmodelListComponent } from './components/usefulmodel/usefulmodel-list/usefulmodel-list.component';
import { UsefulmodelComponent } from './components/usefulmodel/usefulmodel.component';
import { RequestExpertSearchComponent } from './containers/request-expert-search/request-expert-search.component';
import { DynamicFieldComponent } from './dynamic-field/dynamic-field.component';
import { IndustrialdesignSearchService } from './services/industrialdesign-search.service';
import { InventionSearchService } from './services/invention-search.service';
import { TrademarkSearchService } from './services/trademark-search.service';
import { UsefulmodelSearchService } from './services/usefulmodel-search.service';
import { ImageSearchComponent } from './components/trademark-with-similar/image-search/image-search.component';
import { ImageSearchListComponent } from './components/trademark-with-similar/image-search/image-search-list/image-search-list.component';
import { ImageSearchFormComponent } from './components/trademark-with-similar/image-search/image-search-form/image-search-form.component';
import { ImageServiceService } from './services/image-service.service';
import { TrademarkWithSimilarComponent } from './components/trademark-with-similar/trademark-with-similar.component';
import { ImageDialog } from './image-dialog';
import { SimilarProtectionDocsComponent } from './components/similar-protection-docs/similar-protection-docs.component';
import { ExpertSearchSimilarService } from './services/expert-search-similar.service';
import { NavigateOnSelectService } from './services/navigate-on-select.service';
import { DocumentsCompareComponent } from 'app/expert-search/components/documents-compare/documents-compare.component';
import { ViewCompareComponent } from 'app/expert-search/components/view-compare/view-compare.component';
import { ExpertSearchComponent } from './containers/expert-search/expert-search.component';
import { ExpertSearchRoutingModule } from './expert-search-routing.module';
import { InventionWithSimilarComponent } from './components/invention-with-similar/invention-with-similar.component';
import { UsefulModelWithSimilarComponent } from './components/useful-model-with-similar/useful-model-with-similar.component';
import { IndustrialDesignWithSimilarComponent } from './components/industrial-design-with-similar/industrial-design-with-similar.component';
// import { BrowserModule } from '@angular/platform-browser';
import { ImageViewerModule } from 'ng2-image-viewer';
import { FilterByParamsPipe } from './pipes/filter-by-params';
import { DetailsPopupComponent } from './components/details-popup/details-popup.component';
import { TreeFormDialogComponent } from 'app/shared/components/tree-form-dialog/tree-form-dialog.component';
import { AdvancedSelectComponent } from 'app/shared/components/advanced-select/advanced-select.component';

@NgModule({
    imports: [
        // BrowserModule,
        ImageViewerModule,
        CommonModule,
        SharedModule,
        ColumnConfigModule,
        DiffMatchPatchModule,
        ExpertSearchRoutingModule
    ],
    exports: [
        RequestExpertSearchComponent
    ],
    declarations: [
        TrademarkComponent,
        TrademarkListComponent,
        TrademarkFormComponent,
        InventionComponent,
        InventionFormComponent,
        InventionListComponent,
        UsefulmodelComponent,
        UsefulmodelFormComponent,
        UsefulmodelListComponent,
        IndustrialdesignComponent,
        IndustrialdesignFormComponent,
        IndustrialdesignListComponent,
        DynamicFieldComponent,
        RequestExpertSearchComponent,
        ImageSearchComponent,
        ImageSearchListComponent,
        ImageSearchFormComponent,
        SimilarProtectionDocsComponent,
        ImageDialog,
        DocumentsCompareComponent,
        ViewCompareComponent,
        ExpertSearchComponent,
        TrademarkWithSimilarComponent,
        InventionWithSimilarComponent,
        UsefulModelWithSimilarComponent,
        IndustrialDesignWithSimilarComponent,
        FilterByParamsPipe,
        DetailsPopupComponent,
        AdvancedSelectComponent
    ],
    providers: [
        TrademarkSearchService,
        InventionSearchService,
        UsefulmodelSearchService,
        IndustrialdesignSearchService,
        QueryParamsStorageService,
        ImageServiceService,
        ExpertSearchSimilarService,
        NavigateOnSelectService
    ],
    entryComponents: [
        ImageDialog,
        DetailsPopupComponent,
        TreeFormDialogComponent
    ]
})
export class ExpertSearchModule { }
