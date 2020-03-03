import { AfterViewInit, Component, Input, OnInit, ViewChild, ChangeDetectorRef, EventEmitter, Output } from '@angular/core';
import { QueryParamsStorageService } from 'app/search/services/query-params-storage.service';
import { Subject } from 'rxjs/Rx';

import { UsefulmodelListComponent } from './usefulmodel-list/usefulmodel-list.component';
import { ExpertSearchSimilarDto } from 'app/expert-search/models/expert-search-similar-dto';
import { UsefulmodelDto } from '../../models/usefulmodel-dto.model';

const expertSearchKey = 'expert_search_tm';

@Component({
  selector: 'app-usefulmodel',
  templateUrl: './usefulmodel.component.html',
  styleUrls: ['./usefulmodel.component.scss']
})
export class UsefulmodelComponent implements AfterViewInit {
  resultsLength = new Subject<number>();

  @Input() checkable: boolean;
  @Input() checkedUsefulModelDtos: UsefulmodelDto[];
  @Output() checkChanged = new EventEmitter<ExpertSearchSimilarDto[]>();
  @ViewChild(UsefulmodelListComponent) usefulmodelListComponent: UsefulmodelListComponent;

  constructor(
    private queryParamsStorageService: QueryParamsStorageService,
    private changeDetector: ChangeDetectorRef
  ) { }

  ngAfterViewInit(): void {
    this.changeDetector.detectChanges();
  }

  onSearch(queryParams: any[]) {
    if (queryParams && queryParams.length > 0) {
      this.usefulmodelListComponent.reset(this.queryParamsStorageService.prepareQueryParams(queryParams));
    }
  }

  onReset() {
    this.usefulmodelListComponent.reset();
  }
}
