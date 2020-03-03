import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { ConfigService } from '../../core';
import { ErrorHandlerService } from '../../core/error-handler.service';
import { SnackBarHelper } from '../../core/snack-bar-helper.service';
import { IntellectualPropertySearchDto } from '../../search/models/intellectual-property-search-dto';
import { BaseServiceWithPagination } from '../../shared/base-service-with-pagination';

@Injectable()
export class AttachmentSearchService extends BaseServiceWithPagination<
  IntellectualPropertySearchDto
> {
  private api = '/api/MaterialOwnerSearch/';

  constructor(
    http: HttpClient,
    configService: ConfigService,
    errorHandlerService: ErrorHandlerService,
    private snackBarHelper: SnackBarHelper
  ) {
    super(http, configService, errorHandlerService, '/api/MaterialOwnerSearch/');
  }

  getSelectedOwners(
    owners: IntellectualPropertySearchDto[],
    keyValuePairs: any[]
  ): Observable<IntellectualPropertySearchDto[]> {
    const params = this.buildQueryString(keyValuePairs);
    return this.http
      .post(
        `${this.configService.apiUrl}${this.api}/getOwners`,
        owners,
        {
          params: new HttpParams({ fromString: params })
        }
      )
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: IntellectualPropertySearchDto[]) => data);
  }

  private buildQueryString(keyValuePairs: any[] = []): string {
    return keyValuePairs
      .filter(item => item.value !== undefined && item.value.toString().length)
      .map(item => `${item.key}=${item.value}`)
      .join('&');
  }
}
