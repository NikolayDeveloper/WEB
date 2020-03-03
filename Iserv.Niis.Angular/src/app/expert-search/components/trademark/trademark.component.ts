import { Subject } from 'rxjs/Rx';
import { AfterViewInit, Component, Input, OnInit, ViewChild, Output, EventEmitter, ChangeDetectorRef, OnDestroy } from '@angular/core';

import { RequestDetails } from '../../../requests/models/request-details';
import { SearchType } from '../../../search/models/search-type.enum';
import { QueryParamsStorageService } from '../../../search/services/query-params-storage.service';
import { TrademarkListComponent } from './trademark-list/trademark-list.component';
import { TrademarkDto } from 'app/expert-search/models/trademark-dto.model';
import { ExpertSearchSimilarDto } from 'app/expert-search/models/expert-search-similar-dto';
import { ImageViewerComponent } from 'ng2-image-viewer/image-viewer.component';
import { RequestService } from 'app/requests/request.service';

const expertSearchKey = 'expert_search_tm';

@Component({
  selector: 'app-trademark',
  templateUrl: './trademark.component.html',
  styleUrls: ['./trademark.component.scss']
})
export class TrademarkComponent implements AfterViewInit, OnDestroy {
  resultsLength = new Subject<number>();

  @Input() checkable: boolean;
  @Input() checkedTrademarkDtos: TrademarkDto[];
  @ViewChild(TrademarkListComponent) trademarkListComponent: TrademarkListComponent;
  @Output() checkChanged = new EventEmitter<ExpertSearchSimilarDto[]>();

  showImagePreview = false;
  private onDestroy = new Subject();
  images = [];
  constructor(
    private queryParamsStorageService: QueryParamsStorageService,
    private changeDetector: ChangeDetectorRef,
    private requestService: RequestService) { }

  ngAfterViewInit(): void {
    this.changeDetector.detectChanges();
  }

  onCloseClick() {
    this.showImagePreview = false;
    this.images = [];
  }

  showImage(params: any) {
    this.requestService
      .getImage(params.ownerId, params.ownerType)
      .takeUntil(this.onDestroy)
      .subscribe(image => {
        if (image.base64) {
          this.showImagePreview = true;
          this.images = [image.base64];
        }
      });
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  onSearch(queryParams: any[]) {
    if (queryParams && queryParams.length > 0) {
      this.trademarkListComponent.reset(this.queryParamsStorageService.prepareQueryParams(queryParams));
    }
  }

  onReset() {
    this.trademarkListComponent.reset();
  }
}
