import { AfterViewInit, ChangeDetectorRef, Component, Input, ViewChild, Output, EventEmitter } from '@angular/core';
import { QueryParamsStorageService } from 'app/search/services/query-params-storage.service';
import { Subject } from 'rxjs/Rx';

import { InventionListComponent } from './invention-list/invention-list.component';
import { ExpertSearchSimilarDto } from '../../models/expert-search-similar-dto';
import { InventionDto } from '../../models/invention-dto.model';

const expertSearchKey = 'expert_search_tm';

@Component({
  selector: 'app-invention',
  templateUrl: './invention.component.html',
  styleUrls: ['./invention.component.scss'],
})
export class InventionComponent implements AfterViewInit {
  resultsLength = new Subject<number>();

  @Input() checkable: boolean;
  @Input() checkedInventionDtos: InventionDto[];
  @Output() checkChanged = new EventEmitter<ExpertSearchSimilarDto[]>();
  @ViewChild(InventionListComponent) inventionListComponent: InventionListComponent;

  constructor(
    private queryParamsStorageService: QueryParamsStorageService,
    private changeDetector: ChangeDetectorRef) { }

  ngAfterViewInit(): void {
    this.changeDetector.detectChanges();
  }

  onSearch(queryParams: any[]) {
    if (queryParams && queryParams.length > 0) {
      this.inventionListComponent.reset(this.queryParamsStorageService.prepareQueryParams(queryParams));
    }
  }

  onReset() {
    this.inventionListComponent.reset();
  }
}
