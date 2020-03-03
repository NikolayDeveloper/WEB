import { Workflow } from './models/workflow-model';
import { OwnerType } from './models/owner-type.enum';
import { SnackBarHelper } from '../../core/snack-bar-helper.service';
import { ConfigService } from '../../core/index';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SelectOption } from 'app/shared/services/models/select-option';
import { Observable } from 'rxjs/Observable';

import { ErrorHandlerService } from '../../core/error-handler.service';
import { BaseDictionary, DicRouteStage } from './models/base-dictionary';

@Injectable()
export class WorkflowService {
  private api: string;
  constructor(
    private http: HttpClient,
    private configService: ConfigService,
    private errorHandlerService: ErrorHandlerService,
    private snackBarHelper: SnackBarHelper
  ) {
    this.api = `${this.configService.apiUrl}/api/workflow/`;
  }

  get(ownerId: number, ownerType: OwnerType): Observable<Workflow[]> {
    return this.http
      .get(`${this.api}${ownerId}/${ownerType}`)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: Workflow[]) => data);
  }

  add(requestWorkflow: Workflow, ownerType: OwnerType): Observable<Workflow> {
    return this.http
      .post(`${this.api}${ownerType}`, requestWorkflow)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: Workflow) => data);
  }

  getStageUsers(stageId: number): Observable<SelectOption[]> {
    return this.http
      .get(`${this.api}stageUsers/${stageId}`)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: SelectOption[]) => data);
  }

  getBulletinUsers(): Observable<SelectOption[]> {
    return this.http
      .get(`${this.api}getBulletinUsers`)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: SelectOption[]) => data);
  }

  getSupportUsers(): Observable<SelectOption[]> {
    return this.http
      .get(`${this.api}getSupportUsers`)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: SelectOption[]) => data);
  }

  getRequestStages(requestId: number): Observable<DicRouteStage[]> {
    return this.http
      .get(`${this.api}requestStages/${requestId}`)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((stages: DicRouteStage[]) => stages);
  }

  getRouteStageById(stageId: number): Observable<DicRouteStage> {
    return this.http
      .get(
        `${this.configService.apiUrl}${this.api}getRouteStageById/${stageId}`
      )
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((stage: DicRouteStage) => stage);
  }

  getRouteStageByCode(stageCode: string): Observable<DicRouteStage> {
    return this.http
      .get(
        `${this.configService.apiUrl}${
          this.api
        }getRouteStageByCode/${stageCode}`
      )
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((stage: DicRouteStage) => stage);
  }

  getNextStagesByWorkflow(workflowId: number): Observable<DicRouteStage[]> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}getNextStagesByWorkflow/${workflowId}`)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((nextStageIds: DicRouteStage[]) => nextStageIds);
  }

  getNextStages(stageId: number): Observable<DicRouteStage[]> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}getNextStages/${stageId}`)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((nextStageIds: DicRouteStage[]) => nextStageIds);
  }

  getPreviousStages(stageId: number): Observable<DicRouteStage[]> {
    return this.http
      .get(
        `${this.configService.apiUrl}${this.api}getPreviousStages/${stageId}`
      )
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((nextStageIds: DicRouteStage[]) => nextStageIds);
  }

  getNextStagesByOwnerId(
    ownerId: number,
    ownerType: OwnerType
  ): Observable<DicRouteStage[]> {
    return this.http
      .get(
        `${this.configService.apiUrl}${
          this.api
        }getNextStagesByOwner/${ownerType}/${ownerId}`
      )
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((nextStageIds: DicRouteStage[]) => nextStageIds);
  }

  getPreviousStagesByOwnerId(
    ownerId: number,
    ownerType: OwnerType
  ): Observable<DicRouteStage> {
    return this.http
      .get(
        `${this.configService.apiUrl}${
          this.api
        }getPreviousStagesByOwner/${ownerType}/${ownerId}`
      )
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((nextStageIds: DicRouteStage) => nextStageIds);
  }
  getNextStagesByContractId(contractId: number): Observable<DicRouteStage[]> {
    return this.http
      .get(
        `${this.configService.apiUrl}${
          this.api
        }getNextStagesByContractId/${contractId}`
      )
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((nextStageIds: DicRouteStage[]) => nextStageIds);
  }

  finishParallelWorkflow(id: number){
    return this.http
      .get(
        `${this.api}finishParallelWorkflow/${id}`
      )
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
  }
  
  isParallelWorkflow(id: number){
    return this.http
      .get(
        `${this.api}isParallelWorkflow/${id}`
      )
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
  }
}
