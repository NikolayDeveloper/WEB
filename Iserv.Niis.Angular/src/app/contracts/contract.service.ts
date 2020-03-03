import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService } from 'app/core';
import { ErrorHandlerService } from 'app/core/error-handler.service';
import { ContractDetails } from 'app/contracts/models/contract-details';
import { Observable } from 'rxjs/Observable';
import { OwnerType } from '../shared/services/models/owner-type.enum';
import { ContractItem } from './models/contract-item';

@Injectable()
export class ContractService {
  api = '/api/contracts/';
  constructor(
    protected http: HttpClient,
    protected configService: ConfigService,
    protected errorHandlerService: ErrorHandlerService
  ) {}

  getById(id: number): Observable<ContractDetails> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}${id}`)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: ContractDetails) => {
        data.ownerType = OwnerType.Contract;
        return data;
      });
  }
  getByOwner(ownerId: number, ownerType: OwnerType): Observable<ContractItem[]> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}/byOwner/${ownerType}/${ownerId}`)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: ContractItem[]) => {
        return data;
      });
  }
  addContract(contractDetails: ContractDetails): Observable<ContractDetails> {
    return this.http
      .post(`${this.configService.apiUrl}${this.api}`, contractDetails)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: ContractDetails) => data);
  }

  updateContract(
    contractDetails: ContractDetails
  ): Observable<ContractDetails> {
    return this.http
      .put(
        `${this.configService.apiUrl}${this.api}${contractDetails.id}`,
        contractDetails
      )
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: ContractDetails) => data);
  }

  deleteContract(contractId: number): Observable<any> {
    return this.http.delete(
      `${this.configService.apiUrl}${this.api}${contractId}`
    );
  }

  createRawContract(pdTypeId: number): Observable<ContractDetails> {
    return Observable.of(
      new ContractDetails({ protectionDocTypeId: pdTypeId, receiveTypeId: 4 })
    );
  }

  registerContract(
    contractDetails: ContractDetails
  ): Observable<ContractDetails> {
    return this.http
      .put(
        `${this.configService.apiUrl}${this.api}register/${contractDetails.id}`,
        contractDetails
      )
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: ContractDetails) => data);
  }

  getCurrentStageCode(contractDetails: ContractDetails): string {
    return contractDetails.currentWorkflow
      ? contractDetails.currentWorkflow.currentStageCode
      : 'DK01.1';
  }
  generateContractNum(id: number): Observable<any> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}/generateContractNum/${id}`)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: any) => {
        return data;
      });
  }
}
