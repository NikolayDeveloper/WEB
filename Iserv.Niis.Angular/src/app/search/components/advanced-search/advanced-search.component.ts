import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Subject } from 'rxjs/Rx';

import { ResetableComponent } from '../../models/resetable-component';
import { SearchType } from '../../models/search-type.enum';
import { QueryParamsStorageService } from '../../services/query-params-storage.service';
import { RequestSearchService } from '../../services/request-search.service';
import { ContractSearchListComponent } from './contract-search-list/contract-search-list.component';
import { DocumentSearchListComponent } from './document-search-list/document-search-list.component';
import { ProtectionDocSearchListComponent } from './protectiondoc-search-list/protectiondoc-search-list.component';
import { RequestSearchListComponent } from './request-search-list/request-search-list.component';

export const selectedSearchTypeKey = 'selected_search_type';

@Component({
  selector: 'app-advanced-search',
  templateUrl: './advanced-search.component.html',
  styleUrls: ['./advanced-search.component.scss']
})
export class AdvancedSearchComponent implements OnInit, OnDestroy, AfterViewInit {
  resultsLength = new Subject<number>();
  selectedSearchType: SearchType;
  searchType = SearchType;

  private onDestroy = new Subject();

  @ViewChild(RequestSearchListComponent) requestSearchListComponent: RequestSearchListComponent;
  @ViewChild(ProtectionDocSearchListComponent) protectionDocSearchListComponent: ProtectionDocSearchListComponent;
  @ViewChild(ContractSearchListComponent) contractSearchListComponent: ContractSearchListComponent;
  @ViewChild(DocumentSearchListComponent) documentSearchListComponent: DocumentSearchListComponent;

  constructor(private requestSearchService: RequestSearchService,
    private queryParamsStorageService: QueryParamsStorageService) { }

  ngOnInit() {

    this.queryParamsStorageService.clear(SearchType.AdvancedRequest);
    this.queryParamsStorageService.clear(SearchType.AdvancedContract);
    this.queryParamsStorageService.clear(SearchType.AdvancedDocument);
    this.queryParamsStorageService.clear(SearchType.AdvancedProtectionDoc);

    this.selectedSearchType = this.getSelectedSearchType();
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  ngAfterViewInit(): void {
    this.applySearch(this.selectedSearchType);
  }

  onSearch(queryParams: any[]) {
    this.queryParamsStorageService.set(this.selectedSearchType, queryParams);
    this.applySearch(this.selectedSearchType);
  }

  onReset() {
    this.queryParamsStorageService.clear(this.selectedSearchType);
    this.getResetableComponent(this.selectedSearchType).reset();
  }

  onSelectedSearchType(type: SearchType) {
    this.selectedSearchType = type;
    localStorage.setItem(selectedSearchTypeKey, JSON.stringify(type));
    setTimeout(() => {
      this.applySearch(type);
    }, 200);
  }

  private applySearch(type: SearchType): void {
    const queryParams = this.queryParamsStorageService.get(type);
    const resetableComponent = this.getResetableComponent(type);
    if (queryParams && queryParams.length > 0 && resetableComponent) {
      resetableComponent.reset(queryParams);
    }
  }

  private getSelectedSearchType(): SearchType {
    const selectedType = JSON.parse(localStorage.getItem(selectedSearchTypeKey));

    const type = selectedType
      ? selectedType
      : SearchType.AdvancedRequest;

    localStorage.setItem(selectedSearchTypeKey, JSON.stringify(type));

    return type;
  }

  private getResetableComponent(type: SearchType): ResetableComponent {
    switch (type) {
      case SearchType.AdvancedRequest:
        return this.requestSearchListComponent;
      case SearchType.AdvancedProtectionDoc:
        return this.protectionDocSearchListComponent;
      case SearchType.AdvancedContract:
        return this.contractSearchListComponent;
      case SearchType.AdvancedDocument:
        return this.documentSearchListComponent;
      default:
        throw Error(`Selected type: ${type} have not list component!`);
    }
  }
}
