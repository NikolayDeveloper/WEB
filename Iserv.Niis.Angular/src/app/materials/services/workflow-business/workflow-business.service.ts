import { MaterialWorkflow, WorkflowSendType } from '../../../shared/services/models/workflow-model';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { MaterialDetail } from '../../models/materials.model';
import { DicRouteStage } from '../../../shared/services/models/base-dictionary';
import { RouteStageService } from '../../../shared/services/route-stage.service';
import { AuthenticationService } from 'app/shared/authentication/authentication.service';

/**
 * Коды из основного сценария. Коды ручных этапов высокого приоритета
 */
const highPriorityMainStageCodes: string[] = [];

const signStages: string[] = ['OUT02.1', 'OUT02.2', 'OUT02.3', 'IN01.1', 'IN01.1.1', 'IN01.1.0'];

const editStages: string[] = ['IN1.1', 'IN2.2', 'OUT01.1', 'OUT03.1', 'IN01'];

const initialStages: string[] = ['IN1.1', 'OUT01.1', 'IN01'];

const scanStages: string[] = ['OUT03.1'];

const sendStages: string[] = ['OUT03.1'];

@Injectable()
export class WorkflowBusinessService {
  constructor(
    private routeStageService: RouteStageService,
    private auth: AuthenticationService,
  ) { }

  generateWorkflowBy(
    sendTypeSubject: Observable<WorkflowSendType>,
    incomingData: MaterialDetail
  ): Observable<MaterialWorkflow> {
    const currentWorkflow = this.getCurrentWorkflow(incomingData);

    return sendTypeSubject.switchMap((type: WorkflowSendType): Observable<
      MaterialWorkflow
      > => {
      if (!currentWorkflow) {
        return Observable.of(null);
      }

      switch (type) {
        case WorkflowSendType.ToNextByRoute: {
          return this.routeStageService
            .isLast(currentWorkflow.currentStageId)
            .switchMap((isLast: boolean): Observable<MaterialWorkflow> => {
              if (isLast) {
                return Observable.of(null);
              }

              return this.routeStageService
                .getNextStages(currentWorkflow.currentStageId)
                .switchMap((stages: DicRouteStage[]): Observable<MaterialWorkflow> => {
                  const stage = this.getMainStage(stages);
                  const rawWorkflow = this.createRawWorkflow(currentWorkflow);
                  rawWorkflow.currentStageId = stage.id;
                  rawWorkflow.routeId = stage.routeId;
                  rawWorkflow.isComplete = stage.isLast;
                  rawWorkflow.isMain = stage.isMain;
                  rawWorkflow.workflowSendType = type;
                  rawWorkflow.isSigned = currentWorkflow.isSigned;
                  return Observable.of(rawWorkflow);
                });
            });
        }

        case WorkflowSendType.ToCurrentStage: {
          const rawWorkflow = this.createRawWorkflow(currentWorkflow);
          rawWorkflow.currentStageId = currentWorkflow.currentStageId;
          rawWorkflow.workflowSendType = type;
          return Observable.of(rawWorkflow);
        }

        case WorkflowSendType.ReturnByRoute: {
          return this.routeStageService
            .isFirst(currentWorkflow.currentStageId)
            .switchMap((isFirst: boolean): Observable<MaterialWorkflow> => {
              if (isFirst) {
                return Observable.of(null);
              }

              return this.routeStageService
                .getPreviousStages(currentWorkflow.currentStageId)
                .switchMap((previousStages: DicRouteStage[]): Observable<
                  MaterialWorkflow
                  > => {
                  const previousByOrderWorkflow = this.getPreviousByOrderWorkflow(
                    incomingData.workflowDtos,
                    previousStages
                  );
                  const rawWorkflow = this.createRawWorkflow(currentWorkflow);
                  rawWorkflow.currentStageId =
                    previousByOrderWorkflow.currentStageId;
                  rawWorkflow.currentUserId =
                    previousByOrderWorkflow.currentUserId;
                  rawWorkflow.routeId = previousByOrderWorkflow.routeId;
                  rawWorkflow.isComplete = previousByOrderWorkflow.isComplete;
                  rawWorkflow.isMain = previousByOrderWorkflow.isMain;
                  rawWorkflow.workflowSendType = type;
                  return Observable.of(rawWorkflow);
                });
            });
        }

        default:
          throw Error(`Unprocessable send type: ${type}`);
      }
    });
  }

  getPreviousWorkflow(incomingData: MaterialDetail): MaterialWorkflow {
    const currentWorkflow = incomingData.workflowDtos.filter(d => d.isCurent === true && d.currentUserId === this.auth.userId)[0];

    if (!incomingData.id) {
      if (currentWorkflow) {
        throw Error('New request should not contain any workflow!');
      }
      return null;
    }

    const currentWorkflowArray = incomingData.workflowDtos.filter(
      w => w.id === currentWorkflow.id
    );
    if (!currentWorkflowArray || currentWorkflowArray.length !== 1) {
      throw Error('Workflow consistency is broken!');
    }

    const previousWorkflowId = currentWorkflowArray[0].previousWorkflowId;
    const previousWorkflowArray = incomingData.workflowDtos.filter(
      w => w.id === previousWorkflowId
    );
    if (!currentWorkflowArray || currentWorkflowArray.length !== 1) {
      return null;
    }

    return previousWorkflowArray[0];
  }

  getCurrentWorkflow(incomingData: MaterialDetail): MaterialWorkflow {
    if (!incomingData || !incomingData.workflowDtos) {
      return null;
    }
    const currentWorkflow = incomingData.workflowDtos.filter(d => d.isCurent === true && d.currentUserId === this.auth.userId)[0];

    if (!incomingData.id) {
      if (currentWorkflow) {
        throw Error('New request should not contain any workflow!');
      }
      return null;
    }

    if (!currentWorkflow) {
      return null;
    }

    const currentWorkflowArray = incomingData.workflowDtos.filter(
      w => w.id === currentWorkflow.id
    );
    if (!currentWorkflowArray || currentWorkflowArray.length !== 1) {
      throw Error('Workflow consistency is broken!');
    }

    return currentWorkflowArray[0];
  }

  isSignStage(currentWorkflow: MaterialWorkflow): boolean {
    if (!currentWorkflow) {
      return false;
    }
    return currentWorkflow.currentStageIsSign;
  }

  isScanStage(currentWorkflow: MaterialWorkflow): boolean {
    if (!currentWorkflow) {
      return false;
    }
    return scanStages.includes(currentWorkflow.currentStageCode);
  }

  isEditStage(currentWorkflow: MaterialWorkflow): Observable<boolean> {
    if (!currentWorkflow) {
      return Observable.of(false);
    }
    return Observable.of(editStages.includes(currentWorkflow.currentStageCode));
  }

  isInitialStage(currentWorkflow: MaterialWorkflow): boolean {
    if (!currentWorkflow) {
      return false;
    }
    return initialStages.includes(currentWorkflow.currentStageCode);
  }

  isSendStage(currentWorkflow: MaterialWorkflow): boolean {
    if (!currentWorkflow) {
      return false;
    }
    return sendStages.includes(currentWorkflow.currentStageCode);
  }

  isNextSendStage(currentWorkflow: MaterialWorkflow): Observable<boolean> {
    if (!currentWorkflow) {
      return Observable.of(false);
    }

    return this.routeStageService
      .getStage(currentWorkflow.currentStageId)
      .switchMap((stage): Observable<boolean> => {
        const result = sendStages.includes(stage.code);
        return Observable.of(result);
      });
  }

  // TODO: Завязать также на роли. Админу дать full access
  /**
   * Предикат доступности трансфера по этапам
   *
   * @param {IncomingData} incomingData заявка
   * @returns {Observable<boolean>} флаг доступности
   * @memberof WorkflowBusinessService
   */
  public availableOfTransfer(
    incomingData: MaterialDetail
  ): Observable<boolean> {
    const currentWorkflow = this.getCurrentWorkflow(incomingData);

    // Создается заявка, еще нет инициального этапа. Трансфер недоступен
    if (!currentWorkflow) {
      return Observable.of(false);
    }

    return Observable.of(!currentWorkflow.isComplete);
  }

  // TODO: Завязать также на роли. Админу дать full access
  /**
   * Предикат доступности редактирования и других действий (применение оплат) для вкладки
   *
   * @param {IncomingData} incomingData Заявка
   * @param {RequestPart} part Вкладка заявки
   * @returns {Observable<boolean>} Флаг доступности
   * @memberof WorkflowBusinessService
   */
  public availableAtStage(incomingData: MaterialDetail): Observable<boolean> {
    const currentWorkflow = this.getCurrentWorkflow(incomingData);

    return Observable.of(true);
  }

  afterCreateIncomingData(
    incomingData: MaterialDetail
  ): Observable<MaterialDetail> {
    return this.doMockLogic(incomingData);
  }

  afterUpdateIncomingData(
    incomingData: MaterialDetail
  ): Observable<MaterialDetail> {
    return this.doMockLogic(incomingData);
  }

  afterUpdateWorkflow(
    incomingData: MaterialDetail,
    newWorkflow: MaterialWorkflow
  ): Observable<MaterialDetail> {
    incomingData.workflowDtos = [newWorkflow, ...incomingData.workflowDtos.filter(d => d.id !== newWorkflow.id)];
    return this.doMockLogic(Object.assign({}, incomingData));
  }

  afterCreateWorkflow(
    incomingData: MaterialDetail,
    newWorkflow: MaterialWorkflow
  ): Observable<MaterialDetail> {
    // incomingData.currentWorkflowId = newWorkflow.id;
    incomingData.workflowDtos.unshift(newWorkflow);
    incomingData.outgoingNumber = newWorkflow.outgoingNum;
    incomingData.documentDate = newWorkflow.documentDate;

    return this.doMockLogic(Object.assign({}, incomingData));
  }

  /**
   * Генерирует черновое тело нового этапа
   * Возвращает тело нового этапа
   * @param {MaterialWorkflow} currentWorkflow Текущий этап
   * @returns {MaterialWorkflow}
   * @memberof WorkflowBusinessService
   */
  createRawWorkflow(currentWorkflow: MaterialWorkflow): MaterialWorkflow {
    return new MaterialWorkflow({
      ownerId: currentWorkflow.ownerId,
      fromStageId: currentWorkflow.currentStageId,
      fromUserId: currentWorkflow.currentUserId,
      fromUserNameRu: currentWorkflow.currentUserNameRu
    });
  }

  private getMainStage(stages: DicRouteStage[]): DicRouteStage {
    const mainStages = stages.filter(s => s.isMain);
    return mainStages[0];
  }

  private getPreviousByOrderWorkflow(
    workflows: MaterialWorkflow[],
    previousStages: DicRouteStage[]
  ): MaterialWorkflow {
    const stageWorkflows = workflows
      .filter(w => previousStages.map(s => s.id).includes(w.currentStageId))
      .sortByDate(w => w.dateCreate, 'desc');

    if (!stageWorkflows || stageWorkflows.length === 0) {
      throw Error(`Previous workflow define error`);
    }
    return stageWorkflows[0];
  }

  private doMockLogic(
    incomingData: MaterialDetail
  ): Observable<MaterialDetail> {
    return Observable.of(incomingData);
  }
}
