import 'rxjs/add/operator/takeUntil';

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { ConfigService } from '../../core';
import { ErrorHandlerService } from '../../core/error-handler.service';
import { DictionaryService } from './dictionary.service';
import { DicRouteStage } from './models/base-dictionary';
import { DictionaryType } from './models/dictionary-type.enum';
import { RouteStageOrder } from './models/route-stage-order';
import { WorkflowService } from './workflow.service';
import { OwnerType } from './models/owner-type.enum';

@Injectable()
export class RouteStageService {
  constructor(private http: HttpClient,
    private configService: ConfigService,
    private errorHandlerService: ErrorHandlerService,
    private dictionaryService: DictionaryService,
    private workflowService: WorkflowService,
  ) {
  }

  isFirst(stageId: number): Observable<boolean> {
    return this.getStage(stageId)
      .map((stage: DicRouteStage) => {
        return stage.isFirst;
      });
  }

  isLast(stageId: number): Observable<boolean> {
    return this.getStage(stageId)
      .map((stage: DicRouteStage) => {
        return stage.isLast;
      });
  }

  getStage(stageId: number): Observable<DicRouteStage> {
    return this.workflowService
      .getRouteStageById(stageId)
      .map((stage: DicRouteStage) => {
        if (!stage) {
          throw Error(`Stage with id ${stageId} not found!`);
        }

        return stage;
      });
  }

  getNextStages(stageId: number): Observable<DicRouteStage[]> {
    return this.isLast(stageId)
      .switchMap((last: boolean) => {
        if (last) {
          throw Error(`Stage with id ${stageId} is last!`);
        }

        return this.workflowService.getNextStages(stageId)
          .map((nextStages: DicRouteStage[]) => {
            if (nextStages.length === 0) {
              throw Error(`Stage with id ${stageId} does not have next stage!`);
            }
            return nextStages;
          });
      });
  }

  getPreviousStages(stageId: number): Observable<DicRouteStage[]> {
    return this.isFirst(stageId)
      .switchMap((first: boolean) => {
        if (first) {
          throw Error(`Stage with id ${stageId} is first!`);
        }

        return this.workflowService.getPreviousStages(stageId)
          .map((previousStages: DicRouteStage[]) => {
            if (previousStages.length === 0) {
              throw Error(`Stage with id ${stageId} does not have previous stage!`);
            }
            return previousStages;
          });
      });
  }

  getNextStagesByRequestId(stageId: number, requestid: number): Observable<DicRouteStage[]> {
    return this.isLast(stageId)
      .switchMap((last: boolean) => {
        if (last) {
          throw Error(`Stage with id ${requestid} is last!`);
        }

        return this.workflowService.getNextStagesByOwnerId(requestid, OwnerType.Request)
          .map((nextStages: DicRouteStage[]) => {
            if (nextStages.length === 0) {
              throw Error(`Stage with id ${requestid} does not have next stage!`);
            }
            return nextStages;
          });
      });
  }

  getPreviousStagesByRequestId(stageId: number, requestid: number): Observable<DicRouteStage[]> {
    return this.isFirst(stageId)
      .switchMap((first: boolean) => {
        if (first) {
          throw Error(`Stage with id ${stageId} is first!`);
        }

        return this.workflowService.getPreviousStagesByOwnerId(requestid, OwnerType.Request)
          .map((previousStages: DicRouteStage) => {
            if (!previousStages) {
              throw Error(`Stage with id ${stageId} does not have previous stage!`);
            }
            return [previousStages];
          });
      });
  }
}
