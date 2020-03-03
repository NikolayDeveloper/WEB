import { Injectable } from '@angular/core';
import {
  Workflow,
  WorkflowSendType
} from '../../shared/services/models/workflow-model';
import { Observable } from 'rxjs/Observable';
import { ProtectionDocDetails } from '../models/protection-doc-details';
import { DictionaryService } from '../../shared/services/dictionary.service';
import { RouteStageService } from '../../shared/services/route-stage.service';
import { DicRouteStage } from '../../shared/services/models/base-dictionary';
import { RequestPart } from '../../requests/services/workflow-business.service';
import { IntellectualPropertyDetails } from '../../shared/models/intellectual-property-details';

@Injectable()
export class WorkflowBusinessService {
  constructor(
    private dictionaryService: DictionaryService,
    private routeStageService: RouteStageService
  ) {}

  /**
   * Генерирует черновое тело нового этапа
   * Возвращает тело нового этапа
   * @param {Workflow} currentWorkflow Текущий этап
   * @returns {Workflow}
   * @memberof WorkflowBusinessService
   */
  createRawWorkflow(currentWorkflow: Workflow): Workflow {
    return new Workflow({
      ownerId: currentWorkflow.ownerId,
      fromStageId: currentWorkflow.currentStageId,
      fromUserId: currentWorkflow.currentUserId
    });
  }

  generateWorkflowBy(
    sendTypeSubject: Observable<WorkflowSendType>,
    protectionDocDetails: ProtectionDocDetails,
    workflows: Workflow[]
  ): Observable<Workflow> {
    const currentWorkflow = protectionDocDetails.currentWorkflow;

    return sendTypeSubject.switchMap(
      (type: WorkflowSendType): Observable<Workflow> => {
        if (!currentWorkflow) {
          return Observable.of(null);
        }

        switch (type) {
          case WorkflowSendType.ToNextByRoute: {
            return this.routeStageService
              .isLast(currentWorkflow.currentStageId)
              .switchMap(
                (isLast: boolean): Observable<Workflow> => {
                  if (isLast) {
                    return Observable.of(null);
                  }

                  return this.routeStageService
                    .getNextStages(currentWorkflow.currentStageId)
                    .switchMap(
                      (stages: DicRouteStage[]): Observable<Workflow> => {
                        const stage = this.getMainStage(
                          stages,
                          protectionDocDetails.protectionDocTypeCode
                        );
                        const rawWorkflow = this.createRawWorkflow(
                          currentWorkflow
                        );
                        rawWorkflow.currentStageId = stage.id;
                        rawWorkflow.currentStageCode = stage.code;
                        rawWorkflow.routeId = stage.routeId;
                        rawWorkflow.isComplete = stage.isLast;
                        rawWorkflow.isMain = stage.isMain;
                        rawWorkflow.workflowSendType = type;
                        return Observable.of(rawWorkflow);
                      }
                    );
                }
              );
          }

          case WorkflowSendType.ToCurrentStage: {
            const rawWorkflow = this.createRawWorkflow(currentWorkflow);
            rawWorkflow.currentStageId = currentWorkflow.currentStageId;
            rawWorkflow.currentStageCode = currentWorkflow.currentStageCode;
            rawWorkflow.workflowSendType = type;
            return Observable.of(rawWorkflow);
          }

          case WorkflowSendType.ReturnByRoute: {
            return this.routeStageService
              .isFirst(currentWorkflow.currentStageId)
              .switchMap(
                (isFirst: boolean): Observable<Workflow> => {
                  if (isFirst) {
                    return Observable.of(null);
                  }

                  return this.routeStageService
                    .getPreviousStages(currentWorkflow.currentStageId)
                    .switchMap(
                      (
                        previousStages: DicRouteStage[]
                      ): Observable<Workflow> => {
                        const previousByOrderWorkflow = this.getPreviousByOrderWorkflow(
                          workflows,
                          previousStages
                        );
                        const rawWorkflow = this.createRawWorkflow(
                          currentWorkflow
                        );
                        rawWorkflow.currentStageId =
                          previousByOrderWorkflow.currentStageId;
                        rawWorkflow.currentStageCode =
                          previousByOrderWorkflow.currentStageCode;
                        rawWorkflow.currentUserId =
                          previousByOrderWorkflow.currentUserId;
                        rawWorkflow.routeId = previousByOrderWorkflow.routeId;
                        rawWorkflow.isComplete =
                          previousByOrderWorkflow.isComplete;
                        rawWorkflow.isMain = previousByOrderWorkflow.isMain;
                        rawWorkflow.workflowSendType = type;
                        return Observable.of(rawWorkflow);
                      }
                    );
                }
              );
          }

          default:
            throw Error(`Unprocessable send type: ${type}`);
        }
      }
    );
  }

  private getPreviousByOrderWorkflow(
    workflows: Workflow[],
    previousStages: DicRouteStage[]
  ): Workflow {
    const stageWorkflows = workflows
      .filter(w => previousStages.map(s => s.id).includes(w.currentStageId))
      .sortByDate(w => w.dateCreate, 'desc');

    if (!stageWorkflows || stageWorkflows.length === 0) {
      throw Error(`Previous workflow define error`);
    }
    return stageWorkflows[0];
  }

  private getMainStage(
    stages: DicRouteStage[],
    protectionDocTypeCode: string
  ): DicRouteStage {
    const mainStages = stages.filter(s => s.isMain);
    return mainStages[0];
  }

  public availableAtStage(
    details: IntellectualPropertyDetails,
    part: RequestPart
  ): Observable<boolean> {
    switch (part) {
      case RequestPart.ProtectionDoc:
        return Observable.of(
          details.currentWorkflow.currentStageCode === 'OD01.5'
        );
      case RequestPart.Materials:
      case RequestPart.Payments:
        return Observable.of(true);
      case RequestPart.BibliographicData:
        return Observable.of(
          ['OD04.6', 'OD01.3', 'OD01.2.1', 'OD01.2.2'].includes(
            details.currentWorkflow.currentStageCode
          )
        );
    }
    return Observable.of(false);
  }

  public availableAtStageByWorkflow(
    currentWorkflow: Workflow,
    part: RequestPart
  ): Observable<boolean> {
    switch (part) {
      case RequestPart.ProtectionDoc:
        return Observable.of(false);
      case RequestPart.Materials:
      case RequestPart.Payments:
        return Observable.of(true);
      case RequestPart.BibliographicData:
        return Observable.of(
          ['OD04.6', 'OD01.3', 'OD01.2.1', 'OD01.2.2'].includes(
            currentWorkflow.currentStageCode
          )
        );
    }
    return Observable.of(false);
  }

  public availableOfTransfer(
    protectionDocDetails: ProtectionDocDetails
  ): Observable<boolean> {
    const currentWorkflow = protectionDocDetails.currentWorkflow;

    // Создается заявка, еще нет инициального этапа. Трансфер недоступен
    if (!currentWorkflow) {
      return Observable.of(false);
    }

    // Если текущий этап не из основного сценария, трансфер недоступен
    if (!currentWorkflow.isMain) {
      return Observable.of(false);
    }

    // Не позволять переход, если следующий этап по основному сценарию - автоматический или этап конечный
    return this.getNextWorkflow(
      currentWorkflow,
      protectionDocDetails
    ).switchMap(
      (nextWorkflow: Workflow): Observable<boolean> => {
        return nextWorkflow.isAuto
          ? Observable.of(false)
          : Observable.of(!currentWorkflow.isComplete);
      }
    );
  }

  private getNextWorkflow(
    currentWorkflow: Workflow,
    protectionDocDetails: ProtectionDocDetails
  ): Observable<Workflow> {
    return this.routeStageService
      .getNextStages(currentWorkflow.currentStageId)
      .switchMap(
        (stages: DicRouteStage[]): Observable<Workflow> => {
          let mainStageFound = true;
          let stage = this.getMainStage(
            stages,
            protectionDocDetails.protectionDocTypeCode
          );
          if (!stage) {
            const firstNotMainStage = stages[0];
            stage = firstNotMainStage;
            mainStageFound = false;

            console.log(
              firstNotMainStage
                ? `The next stages don\'t have a main stage. All possible next stages count is ${
                    stages.length
                  }.
                  Selected stage is (by default): ${JSON.stringify(
                    firstNotMainStage
                  )}`
                : `The next stage is not found! Please contact the administrator!`
            );
          }

          const rawWorkflow = this.createRawWorkflow(currentWorkflow);
          rawWorkflow.currentStageId = stage.id;
          rawWorkflow.currentStageCode = stage.code;
          rawWorkflow.routeId = stage.routeId;
          rawWorkflow.isComplete = stage.isLast;
          rawWorkflow.isMain = stage.isMain;
          rawWorkflow.isAuto = stage.isAuto || !mainStageFound;

          return Observable.of(rawWorkflow);
        }
      );
  }
}
