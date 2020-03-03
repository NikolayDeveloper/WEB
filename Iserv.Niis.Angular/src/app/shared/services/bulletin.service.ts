import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConfigService } from '../../core';
import { ErrorHandlerService } from '../../core/error-handler.service';
import { BulletinDto } from '../models/bulletin-dto';

@Injectable()
export class BulletinService {
  private readonly api: string = '/api/Bulletin/';

  constructor(
    private http: HttpClient,
    private configService: ConfigService,
    private errorHandlerService: ErrorHandlerService
  ) {
  }

  get(): Observable<BulletinDto[]> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}`)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((bulletins: BulletinDto[]) => bulletins);
  }

  getById(id: number): Observable<BulletinDto> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}getById/${id}`)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((bulletin: BulletinDto) => bulletin);
  }

  getEarliest(): Observable<BulletinDto> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}earliest`)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((bulletin: BulletinDto) => bulletin);
  }

  attachProtectionDocToBulletin(data: any) {
    return this.http
      .post(`${this.configService.apiUrl}${this.api}attach`, data)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      );
  }

  add(bulletin: BulletinDto) {
    return this.http
      .post(`${this.configService.apiUrl}${this.api}`, bulletin)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      );
  }

  update(bulletin: BulletinDto) {
    return this.http
      .put(`${this.configService.apiUrl}${this.api}`, bulletin)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      );
  }

}
