import { ExpertSearchSimilarDto } from '../../models/expert-search-similar-dto';
import { Component, Input } from '@angular/core';
import { RequestDetails } from '../../../requests/models/request-details';
import { ExpertSearchSimilarService } from 'app/expert-search/services/expert-search-similar.service';
import { OnInit, OnDestroy } from '@angular/core/src/metadata/lifecycle_hooks';
import { OwnerType } from 'app/shared/services/models/owner-type.enum';
import { Subject } from 'rxjs';
import { BaseExpertSearchDto } from '../../models/base-expert-search-dto.model';

@Component({
  selector: 'app-trademark-with-similar',
  templateUrl: './trademark-with-similar.component.html',
  styleUrls: ['./trademark-with-similar.component.scss']
})
export class TrademarkWithSimilarComponent implements OnInit, OnDestroy {
  private onDestroy = new Subject();

  checkedSearchDtos: BaseExpertSearchDto[] = [];

  @Input() requestId: number;
  @Input() protectionDocsId: number[];
  @Input() imageUrl: string;

  constructor(
    private expertSearchSimilarService: ExpertSearchSimilarService) { }

  ngOnInit() {
    this.expertSearchSimilarService.getSimilarSearchResults(this.requestId)
      .subscribe(results => this.checkedSearchDtos = results);
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  onCheckChanged(selectedDtos: BaseExpertSearchDto[]) {
    this.expertSearchSimilarService.createSimilarSearchResults(this.requestId, selectedDtos.map(i => this.mapToSimilarDto(i)))
      .subscribe(results => this.checkedSearchDtos = results);
  }

  onDelete(selectedDtos: BaseExpertSearchDto[]) {
    const i = selectedDtos.map(s => s.expertSearchSimilarId).join(';');
    this.expertSearchSimilarService.deleteSimilarSearchResults(this.requestId, i)
      .subscribe(results => this.checkedSearchDtos = results);
  }

  onSave(data: { keywords: string, checkedSearchDtos: BaseExpertSearchDto[] }) {
    this.expertSearchSimilarService
      .updateSimilarSearchResults(
        this.requestId,
        OwnerType.Request,
        data.checkedSearchDtos.map(dto => this.mapToSimilarDto(dto)),
        data.keywords
      )
      .takeUntil(this.onDestroy)
      .subscribe();
  }

  private mapToSimilarDto(dto: BaseExpertSearchDto): ExpertSearchSimilarDto {
    return new ExpertSearchSimilarDto({
      id: dto.expertSearchSimilarId,
      requestId: this.requestId,
      ownerType: dto.ownerType,
      similarRequestId: dto.ownerType === OwnerType.Request ? dto.id : null,
      similarProtectionDocId: dto.ownerType === OwnerType.ProtectionDoc ? dto.id : null,
      imageSimilarity: dto.imageSimilarity,
      phonSimilarity: dto.phonSimilarity,
      semSimilarity: dto.semSimilarity,
      protectionDocCategory: dto.protectionDocCategory,
      protectionDocFormula: dto.protectionDocFormula,
    });
  }
}
