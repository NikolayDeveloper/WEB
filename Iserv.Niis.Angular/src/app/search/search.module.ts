import { NgModule } from '@angular/core';
import { ColumnConfigModule } from 'app/modules/column-config';

import { UsersService } from '../administration/users.service';
import { SharedModule } from '../shared/shared.module';
import { AdvancedSearchComponent } from './components/advanced-search/advanced-search.component';
import { ContractSearchFormComponent } from './components/advanced-search/contract-search-form/contract-search-form.component';
import { ContractSearchListComponent } from './components/advanced-search/contract-search-list/contract-search-list.component';
import { DocumentSearchFormComponent } from './components/advanced-search/document-search-form/document-search-form.component';
import { DocumentSearchListComponent } from './components/advanced-search/document-search-list/document-search-list.component';
import { ProtectionDocSearchFormComponent } from './components/advanced-search/protectiondoc-search-form/protectiondoc-search-form.component';
import { ProtectionDocSearchListComponent } from './components/advanced-search/protectiondoc-search-list/protectiondoc-search-list.component';
import { RequestSearchFormComponent } from './components/advanced-search/request-search-form/request-search-form.component';
import { RequestSearchListComponent } from './components/advanced-search/request-search-list/request-search-list.component';
import { SearchComponent } from './containers/search/search.component';
import { SearchRoutingModule } from './search-routing.module';
import { ContractSearchService } from './services/contract-search.service';
import { DocumentSearchService } from './services/document-search.service';
import { ProtectionDocSearchService } from './services/protectiondoc-search.service';
import { QueryParamsStorageService } from './services/query-params-storage.service';
import { RequestSearchService } from './services/request-search.service';
import { SearchService } from './services/search.service';
import { NgxPermissionsModule } from 'ngx-permissions';

@NgModule({
    imports: [
        SharedModule,
        SearchRoutingModule,
        ColumnConfigModule,
        NgxPermissionsModule
    ],
    declarations: [
        SearchComponent,
        AdvancedSearchComponent,
        RequestSearchFormComponent,
        RequestSearchListComponent,
        ProtectionDocSearchFormComponent,
        ProtectionDocSearchListComponent,
        DocumentSearchFormComponent,
        DocumentSearchListComponent,
        ContractSearchFormComponent,
        ContractSearchListComponent,
    ],
    providers: [
        SearchService,
        RequestSearchService,
        ProtectionDocSearchService,
        DocumentSearchService,
        ContractSearchService,
        QueryParamsStorageService,
        UsersService,
    ]
})
export class SearchModule {}
