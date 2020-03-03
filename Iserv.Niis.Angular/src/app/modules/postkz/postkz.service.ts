import { AddressResponse } from './postkz.model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { ConfigService } from '../../core';
import { ErrorHandlerService } from '../../core/error-handler.service';

const unreachableText = 'Адресный регистр post.kz недоступен!';

@Injectable()
export class PostKzService {
  unreachableText = unreachableText;
  constructor(private http: HttpClient,
    private configService: ConfigService,
    private errorHandlerService: ErrorHandlerService) { }

  get(pattern: string, pageNumber: number = 0): Observable<AddressResponse> {
    return this.http
      .get(`${this.configService.apiUrl}/api/kazpost/byAddress/${pattern.replace('/', ' ').replace('\\', ' ')}?from=${pageNumber}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: AddressResponse) => data);
  }
}
