import { Subject } from 'rxjs/Rx';
import { AfterViewInit, Component, Input, Output, OnInit, EventEmitter, ViewChild, ChangeDetectorRef } from '@angular/core';

import { RequestDetails } from '../../../requests/models/request-details';
import { SearchType } from '../../../search/models/search-type.enum';
import { IndustrialdesignListComponent } from './industrialdesign-list/industrialdesign-list.component';
import { QueryParamsStorageService } from 'app/search/services/query-params-storage.service';
import { IndustrialdesignDto } from 'app/expert-search/models/industrialdesign-dto.model';
import { ExpertSearchSimilarDto } from 'app/expert-search/models/expert-search-similar-dto';

const expertSearchKey = 'expert_search_tm';

@Component({
  selector: 'app-industrialdesign',
  templateUrl: './industrialdesign.component.html',
  styleUrls: ['./industrialdesign.component.scss']
})
export class IndustrialdesignComponent implements AfterViewInit {
  resultsLength = new Subject<number>();

  @Input() checkable: boolean;
  @Input() checkedIndustrialDesignDtos: IndustrialdesignDto[];
  @ViewChild(IndustrialdesignListComponent) industrialdesignListComponent: IndustrialdesignListComponent;
  @Output() checkChanged = new EventEmitter<ExpertSearchSimilarDto[]>();

  constructor(private queryParamsStorageService: QueryParamsStorageService,
    private changeDetector: ChangeDetectorRef) { }

  ngAfterViewInit(): void {
    this.changeDetector.detectChanges();
  }

  onSearch(queryParams: any[]) {
    if (queryParams && queryParams.length > 0) {
      this.industrialdesignListComponent.reset(this.queryParamsStorageService.prepareQueryParams(queryParams));
    }
  }

  onReset() {
    this.industrialdesignListComponent.reset();
  }
}
