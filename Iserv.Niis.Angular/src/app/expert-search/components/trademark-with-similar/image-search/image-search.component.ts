import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { TrademarkDto } from 'app/expert-search/models/trademark-dto.model';

import { ImageServiceService } from '../../../services/image-service.service';
import { ImageSearchListComponent } from './image-search-list/image-search-list.component';

@Component({
  selector: 'app-image-search',
  templateUrl: './image-search.component.html',
  styleUrls: ['./image-search.component.scss']
})

export class ImageSearchComponent {
  @Input() requestId: number;
  @Input() imageUrl: string;
  @Input() protectionDocsId: number[];
  @Input() checkedTrademarkDtos: TrademarkDto[];

  @Output() checkChanged = new EventEmitter<TrademarkDto[]>();

  @ViewChild(ImageSearchListComponent) imageListComponent: ImageSearchListComponent;

  constructor(private imageService: ImageServiceService) { }

  onSearch(id: number) {
    this.imageListComponent.check ? this.imageListComponent.onSearchByNameAndImage(id) // пустой метод
      : this.imageListComponent.OnSearch(id);
  }
}
