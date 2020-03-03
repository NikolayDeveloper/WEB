import { HttpClient, HttpRequest, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { RequestOptions } from '@angular/http';

import { ConfigService } from '../../core';
import { ErrorHandlerService } from '../../core/error-handler.service';
import { SnackBarHelper } from '../../core/snack-bar-helper.service';
import { BaseServiceWithPagination } from '../../shared/base-service-with-pagination';
import { TrademarkDto } from '../models/trademark-dto.model';

@Injectable()
export class ImageServiceService extends BaseServiceWithPagination<TrademarkDto> {
  private api = '/api/imagesearch/';

  constructor(http: HttpClient,
    configService: ConfigService,
    errorHandlerService: ErrorHandlerService,
    snackBarHelper: SnackBarHelper) {
    super(http, configService, errorHandlerService, '/api/imagesearch/');
  }

  public searchByImage(id: number): Observable<TrademarkDto[]> {
    return this.http.get(`${this.configService.apiUrl}${this.api}${id}/searchbyimage`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: TrademarkDto[]) => data);
  }

  public searchByImageAndPhonetic(id: number): Observable<TrademarkDto[]> {
    return this.http.get(`${this.configService.apiUrl}${this.api}${id}/searchbyimageandphonetic`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: TrademarkDto[]) => data);
  }
}

