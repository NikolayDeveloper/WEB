import { Injectable } from '@angular/core';
import { ErrorHandlerService } from '../core/error-handler.service';
import { ConfigService } from '../core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { OwnerType } from '../shared/services/models/owner-type.enum';
import { IntellectualPropertyDetails } from '../shared/models/intellectual-property-details';
import { Observable } from 'rxjs/Observable';
import { ChangesDto, ChangeTypeOption } from './models/changes-dto';
import { ICGSRequestDto } from './models/icgs-request-dto';
import { RequestEarlyRegDto } from './models/request-early-reg-dto';

@Injectable()
export class BibliographicDataService {
  api = '/api/BibliographicData/';
  constructor(
    protected http: HttpClient,
    protected configService: ConfigService,
    protected errorHandlerService: ErrorHandlerService
  ) {}

  get(
    ownerId: number,
    ownerType: OwnerType
  ): Observable<IntellectualPropertyDetails> {
    return this.http
      .get(`${this.api}${ownerType}/${ownerId}`)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: IntellectualPropertyDetails) => data);
  }

  change(changes: ChangesDto[]): Observable<any> {
    return this.http
      .put(`${this.api}change`, changes)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map(data => data);
  }

  getChangeTypeOptions(
    ownerType: OwnerType,
    ownerId: number
  ): Observable<ChangeTypeOption[]> {
    const params = this.buildQueryString([
      { key: 'ownerType', value: ownerType },
      { key: 'ownerId', value: ownerId }
    ]);
    return this.http
      .get(`${this.api}changeTypeOptions`, {
        params: new HttpParams({ fromString: params })
      })
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: ChangeTypeOption[]) => data);
  }

  updateBibliographicData(details: IntellectualPropertyDetails) {
    return this.http
      .put(`${this.api}`, details)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map(data => data);
  }

  updateIcgs(ownerId: number, ownerType: OwnerType, icgs: ICGSRequestDto[]) {
    return this.http
      .put(`${this.api}icgs/${ownerType}/${ownerId}`, icgs)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map(data => data);
  }

  updateColors(ownerId: number, ownerType: OwnerType, colorTzs: number[]) {
    return this.http
      .put(`${this.api}colorTzs/${ownerType}/${ownerId}`, colorTzs)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map(data => data);
  }

  updateEarlyRegs(
    ownerId: number,
    ownerType: OwnerType,
    earlyRegs: RequestEarlyRegDto[]
  ) {
    return this.http
      .put(`${this.api}earlyRegs/${ownerType}/${ownerId}`, earlyRegs)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map(data => data);
  }

  updateIcfems(ownerId: number, ownerType: OwnerType, icfems: number[]) {
    return this.http
      .put(`${this.api}icfem/${ownerType}/${ownerId}`, icfems)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map(data => data);
  }

  changeWorkflow(ownerId: number, ownerType: OwnerType): Observable<any> {
    return this.http
      .post(`${this.api}changeworkflow/${ownerType}`, ownerId)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map(data => data);
  }

  private buildQueryString(keyValuePairs: any[] = []): string {
    return keyValuePairs
      .filter(item => item.value !== undefined && item.value.toString().length)
      .map(item => `${item.key}=${item.value}`)
      .join('&');
  }
}
