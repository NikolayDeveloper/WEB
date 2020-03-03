import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { ExpertSearchSimilarDto } from '../../models/expert-search-similar-dto';
import { ExpertSearchSimilarService } from '../../services/expert-search-similar.service';
import { Subject } from 'rxjs';
import { InventionDto } from '../../models/invention-dto.model';
import { OwnerType } from '../../../shared/services/models/owner-type.enum';
import { BaseExpertSearchDto } from '../../models/base-expert-search-dto.model';

@Component({
  selector: 'app-industrial-design-with-similar',
  templateUrl: './industrial-design-with-similar.component.html',
  styleUrls: ['./industrial-design-with-similar.component.scss']
})
export class IndustrialDesignWithSimilarComponent implements OnInit, OnDestroy {
  private onDestroy = new Subject();

  checkedSearchDtos: BaseExpertSearchDto[] = [];
  resultsLength: number;

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

  onCheckChanged(selectedDtos: InventionDto[]) {
    this.expertSearchSimilarService.createSimilarSearchResults(this.requestId, selectedDtos.map(i => this.mapToSimilarDto(i)))
      .subscribe(results => this.checkedSearchDtos = results);
  }

  onDelete(selectedDtos: BaseExpertSearchDto[]) {
    const i = selectedDtos.map(s => s.expertSearchSimilarId).join(';');
    this.expertSearchSimilarService.deleteSimilarSearchResults(this.requestId, i)
      .subscribe(results => this.checkedSearchDtos = results);
    this.resultsLength = this.checkedSearchDtos.length;
  }

  onSave(data: { keywords: string, checkedSearchDtos: InventionDto[] }) {
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

  private mapToSimilarDto(dto: any): ExpertSearchSimilarDto {
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
