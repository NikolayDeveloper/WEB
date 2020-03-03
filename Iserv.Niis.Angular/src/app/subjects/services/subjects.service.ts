import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { ConfigService } from '../../core';
import { ErrorHandlerService } from '../../core/error-handler.service';
import { SubjectDto } from '../models/subject.model';
import { OwnerType } from '../../shared/services/models/owner-type.enum';
import { BaseServiceWithPagination } from '../../shared/base-service-with-pagination';

@Injectable()
export class SubjectsService extends BaseServiceWithPagination<SubjectDto> {
  private api = `/api/subject/`;

  constructor(
    http: HttpClient,
    configService: ConfigService,
    errorHandlerService: ErrorHandlerService
  ) {
    super(http, configService, errorHandlerService, '/api/subject/');
  }

  public get(ownerId: number, ownerType: OwnerType): Observable<SubjectDto[]> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}${ownerId}/${ownerType}`)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: SubjectDto[]) => data);
  }

  public getByXinAndName(
    xin: string,
    name: string,
    isPatentAttorney?: boolean,
    regNumber?: string
  ) {
    return this.http
      .get(
        `${this.configService.apiUrl}${
          this.api
        }/byXinAndName/${xin};${name};${isPatentAttorney};${regNumber}`
      )
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: SubjectDto[]) => data);
  }

  public attach(
    subjectDto: SubjectDto,
    ownerType: OwnerType
  ): Observable<SubjectDto> {
    return this.http
      .post(`${this.configService.apiUrl}${this.api}${ownerType}`, subjectDto)
      .catch(
        (error: HttpErrorResponse | any): Observable<any> =>
          error instanceof HttpErrorResponse && error.status === 422
            ? Observable.throw(
                'Произошла ошибка! Возможно добавляемый Вами пользователь уже существует в БД АИС «НИИС».'
              )
            : this.errorHandlerService.handleError.call(
                this.errorHandlerService,
                error
              )
      )
      .map((data: SubjectDto) => data);
  }

  public create(
    subjectDto: SubjectDto,
    ownerType: OwnerType
  ): Observable<SubjectDto> {
    return this.http
      .post(
        `${this.configService.apiUrl}${this.api}/create/${ownerType}`,
        subjectDto
      )
      .catch(
        (error: HttpErrorResponse | any): Observable<any> =>
          error instanceof HttpErrorResponse && error.status === 422
            ? Observable.throw(
                'Произошла ошибка! Возможно добавляемый Вами пользователь уже существует в БД АИС «НИИС».'
              )
            : this.errorHandlerService.handleError.call(
                this.errorHandlerService,
                error
              )
      )
      .map((data: SubjectDto) => data);
  }

  public update(
    subjectDto: SubjectDto,
    ownerType: OwnerType
  ): Observable<SubjectDto> {
    return this.http
      .put(
        `${this.configService.apiUrl}${this.api}${ownerType}/${subjectDto.id}`,
        subjectDto
      )
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: SubjectDto) => data);
  }

  public delete(id: number, ownerType: OwnerType): Observable<any> {
    return this.http
      .delete(`${this.configService.apiUrl}${this.api}${ownerType}/${id}`)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      );
  }

  public updateSeveral(
    subjects: SubjectDto[],
    ownerType: OwnerType
  ): Observable<SubjectDto> {
    return this.http
      .put(
        `${this.configService.apiUrl}${this.api}several/${ownerType}`,
        subjects
      )
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: SubjectDto) => data);
  }

  public deleteSeveral(ids: number[], ownerType: OwnerType): Observable<any> {
    return this.http
      .post(`${this.configService.apiUrl}${this.api}deleteseveral/${ownerType}`, ids)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      );
  }

  public attachSeveral(
    subjects: SubjectDto[],
    ownerType: OwnerType
  ): Observable<SubjectDto> {
    return this.http
      .post(`${this.configService.apiUrl}${this.api}several/${ownerType}`, subjects)
      .catch(
        (error: HttpErrorResponse | any): Observable<any> =>
          error instanceof HttpErrorResponse && error.status === 422
            ? Observable.throw(
                'Произошла ошибка! Возможно добавляемый Вами пользователь уже существует в БД АИС «НИИС».'
              )
            : this.errorHandlerService.handleError.call(
                this.errorHandlerService,
                error
              )
      )
      .map((data: SubjectDto) => data);
  }
}
