import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseExpertSearchDto } from 'app/expert-search/models/base-expert-search-dto.model';
import { ExpertSearchSimilarDto } from 'app/expert-search/models/expert-search-similar-dto';
import { OwnerType } from 'app/shared/services/models/owner-type.enum';
import { Observable } from 'rxjs/Observable';

import { ConfigService } from '../../core';
import { ErrorHandlerService } from '../../core/error-handler.service';
import { BaseServiceWithPagination } from '../../shared/base-service-with-pagination';

@Injectable()
export class ExpertSearchSimilarService extends BaseServiceWithPagination<
ExpertSearchSimilarDto
> {
  api = 'api/Search/expert/';
  constructor(
    http: HttpClient,
    configService: ConfigService,
    errorHandlerService: ErrorHandlerService
  ) {
    super(http, configService, errorHandlerService, 'api/Search/expert/');
  }

  createSimilarSearchResults(ownerId: number, similarResults: ExpertSearchSimilarDto[]): Observable<BaseExpertSearchDto[]> {
    const url = `${this.configService.apiUrl}${this.api}/${ownerId}/similarResults`;
    return this.http
      .post(url, similarResults)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: BaseExpertSearchDto[]) => data);
  }

  updateSimilarSearchResults(ownerId: number, ownerType: OwnerType, similarResults: ExpertSearchSimilarDto[], keywords: string):
    Observable<BaseExpertSearchDto[]> {
    const url = `${this.configService.apiUrl}${this.api}/${ownerType}/${ownerId}/similarResults/${keywords}`;
    return this.http
      .put(url, similarResults)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: BaseExpertSearchDto[]) => data);
  }

  getSimilarSearchResults(requestId: number): Observable<BaseExpertSearchDto[]> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}${requestId}/similarResults`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: BaseExpertSearchDto[]) => data);
  }

  deleteSimilarSearchResults(requestId: number, similarityIds: string): Observable<BaseExpertSearchDto[]> {
    const url = `${this.configService.apiUrl}${this.api}${requestId}/similarResults/${similarityIds}`;
    return this.http
      .delete(url)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: BaseExpertSearchDto[]) => data);
  }
}
