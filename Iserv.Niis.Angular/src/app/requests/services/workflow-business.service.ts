import {
  ProtectionDocTypeEnum,
  ProtectionTypeAndCodes,
  Workflow,
  WorkflowSendType
} from '../../shared/services/models/workflow-model';
import { OwnerType } from '../../shared/services/models/owner-type.enum';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { DictionaryService } from '../../shared/services/dictionary.service';
import {
  DicRouteStage,
  DicTariff
} from '../../shared/services/models/base-dictionary';
import { DictionaryType } from '../../shared/services/models/dictionary-type.enum';
import { RouteStageService } from '../../shared/services/route-stage.service';
import { RequestDetails } from '../models/request-details';
import { PaymentService } from '../../payments/payment.service';
import { PaymentInvoice } from '../../payments/models/payment.model';
import { IntellectualPropertyDetails } from '../../shared/models/intellectual-property-details';
import { RouteStageCodes } from '../../shared/models/route-stage-codes';

export enum RequestPart {
  None,
  Request,
  ProtectionDoc,
  BibliographicData,
  Payments,
  Materials,
  OfficeWork
}

@Injectable()
export class WorkflowBusinessService {
  constructor(
    private dictionaryService: DictionaryService,
    private routeStageService: RouteStageService,
    private paymentService: PaymentService
  ) {}

  generateWorkflowBy(
    sendTypeSubject: Observable<WorkflowSendType>,
    requestDetails: RequestDetails,
    workflows: Workflow[]
  ): Observable<Workflow> {
    const currentWorkflow = requestDetails.currentWorkflow;

    return sendTypeSubject.switchMap((type: WorkflowSendType): Observable<
      Workflow
    > => {
      if (!currentWorkflow) {
        return Observable.of(null);
      }

      switch (type) {
        case WorkflowSendType.ToNextByRoute: {
          return this.routeStageService
            .isLast(currentWorkflow.currentStageId)
            .switchMap((isLast: boolean): Observable<Workflow> => {
              if (isLast) {
                return Observable.of(null);
              }

              return this.routeStageService
                .getNextStages(currentWorkflow.currentStageId)
                .switchMap((stages: DicRouteStage[]): Observable<Workflow> => {
                  const stage = this.getMainStage(
                    stages,
                    requestDetails.protectionDocTypeCode
                  );
                  const rawWorkflow = this.createRawWorkflow(currentWorkflow);
                  rawWorkflow.currentStageId = stage.id;
                  rawWorkflow.currentStageCode = stage.code;
                  rawWorkflow.routeId = stage.routeId;
                  rawWorkflow.isComplete = stage.isLast;
                  rawWorkflow.isMain = stage.isMain;
                  rawWorkflow.workflowSendType = type;
                  return Observable.of(rawWorkflow);
                });
            });
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
            .switchMap((isFirst: boolean): Observable<Workflow> => {
              if (isFirst) {
                return Observable.of(null);
              }

              return this.routeStageService
                .getPreviousStages(currentWorkflow.currentStageId)
                .switchMap((previousStages: DicRouteStage[]): Observable<
                  Workflow
                > => {
                  const previousByOrderWorkflow = this.getPreviousByOrderWorkflow(
                    workflows,
                    previousStages
                  );
                  const rawWorkflow = this.createRawWorkflow(currentWorkflow);
                  rawWorkflow.currentStageId =
                    previousByOrderWorkflow.currentStageId;
                  rawWorkflow.currentStageCode =
                    previousByOrderWorkflow.currentStageCode;
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

  /**
   * Предикат определения этапов предварительных экспертиз
   * Возвращает положительный результат, когда текущий этап относится к этапам предварительных экспертиз
   * @param {RequestDetails} requestDetails Заявка
   * @memberof WorkflowBusinessService
   */
  isFormalExamStage(requestDetails: IntellectualPropertyDetails) {
    return RouteStageCodes.stageCodes.formalExam
      .allCodes()
      .includes(requestDetails.currentWorkflow.currentStageCode);
  }

  // TODO: Завязать также на роли. Админу дать full access
  /**
   * Предикат доступности трансфера по этапам
   *
   * @param {RequestDetails} requestDetails заявка
   * @returns {Observable<boolean>} флаг доступности
   * @memberof WorkflowBusinessService
   */
  public availableOfTransfer(
    requestDetails: RequestDetails
  ): Observable<boolean> {
    const currentWorkflow = requestDetails.currentWorkflow;

    // Создается заявка, еще нет инициального этапа. Трансфер недоступен
    if (!currentWorkflow) {
      return Observable.of(false);
    }

    // Если платеж не зачтен, трансфер недоступен
    if (
      this.isPaymentsStage(currentWorkflow.currentStageCode) &&
      !this.areInvoicesPayed(requestDetails)
    ) {
      return Observable.of(false);
    }

    // Если текущий этап не из основного сценария, трансфер недоступен
    if (!currentWorkflow.isMain) {
      return Observable.of(false);
    }

    // Не позволять переход, если следующий этап по основному сценарию - автоматический или этап конечный
    return this.getNextWorkflow(currentWorkflow, requestDetails).switchMap(
      (nextWorkflow: Workflow): Observable<boolean> => {
        const canSendToPartial =
          nextWorkflow.currentStageCode === 'B03.2.1' &&
          RouteStageCodes.partialAutoStageCodes.includes(
            currentWorkflow.currentStageCode
          );
        return nextWorkflow.isAuto
          ? Observable.of(false)
          : // ? Observable.of(this.isPaymentsStage(currentWorkflow.currentStageCode) && this.areInvoicesPayed(requestDetails))
            Observable.of(!currentWorkflow.isComplete && !canSendToPartial);
      }
    );
  }

  // TODO: Завязать также на роли. Админу дать full access
  /**
   * Предикат доступности редактирования и других действий (применение оплат) для вкладки
   *
   * @param {RequestDetails} requestDetails Заявка
   * @param {RequestPart} part Вкладка заявки
   * @returns {Observable<boolean>} Флаг доступности
   * @memberof WorkflowBusinessService
   */
  public availableAtStage(
    requestDetails: IntellectualPropertyDetails,
    part: RequestPart
  ): Observable<boolean> {
    const currentWorkflow = requestDetails.currentWorkflow;

    // Заявка только создана, вкладка доступна
    if (!currentWorkflow) {
      return Observable.of(part === RequestPart.Request);
    }

    switch (part) {
      case RequestPart.Request: {
        return Observable.of(
          RouteStageCodes.stageCodes.initial
            .allCodes()
            .includes(currentWorkflow.currentStageCode)
        );
      }
      case RequestPart.BibliographicData: {
        return Observable.of(
          RouteStageCodes.stageCodes.formation
            .allCodes()
            .includes(currentWorkflow.currentStageCode) ||
            RouteStageCodes.stageCodes.formalExam
              .allCodes()
              .includes(currentWorkflow.currentStageCode)
        );
      }
      case RequestPart.Materials: {
        return Observable.of(true);
      }
      case RequestPart.Payments: {
        const codes = RouteStageCodes.stageCodes.formation
          .filter(
            s =>
              s.type === ProtectionDocTypeEnum.UsefulModels ||
              s.type === ProtectionDocTypeEnum.Inventions
          )
          .allCodes();
        const hasCustomers =
          requestDetails.subjects.some(s => s.roleCode === 'CORRESPONDENCE') &&
          requestDetails.subjects.some(s => s.roleCode === '1') &&
          requestDetails.subjects.some(s => s.roleCode === '2');
        const hasBiblio =
          !!requestDetails.referat &&
          !!requestDetails.requestNum &&
          !!requestDetails.requestDate &&
          (requestDetails.protectionDocTypeCode === 'U'
            ? !!requestDetails.nameRu ||
              !!requestDetails.nameKz ||
              !!requestDetails.nameEn
            : !!requestDetails.nameRu && !!requestDetails.nameKz);
        return Observable.of(
          (!this.isInitialStage(currentWorkflow.currentStageCode) &&
            !this.isFormationStage(currentWorkflow.currentStageCode)) ||
            (codes.includes(currentWorkflow.currentStageCode) &&
              hasCustomers &&
              hasBiblio)
        );
      }
      case RequestPart.OfficeWork: {
        return Observable.of(true);
      }
      default:
        return Observable.of(false);
    }
  }

  public availableAtStageByWorkflow(
    currentWorkflow: Workflow,
    part: RequestPart
  ) {
    if (!currentWorkflow) {
      return Observable.of(part === RequestPart.Request);
    }

    switch (part) {
      case RequestPart.Request: {
        return Observable.of(
          RouteStageCodes.stageCodes.initial
            .allCodes()
            .includes(currentWorkflow.currentStageCode)
        );
      }
      case RequestPart.BibliographicData: {
        return Observable.of(
          RouteStageCodes.stageCodes.formation
            .allCodes()
            .includes(currentWorkflow.currentStageCode) ||
            RouteStageCodes.stageCodes.formalExam
              .allCodes()
              .includes(currentWorkflow.currentStageCode)
        );
      }
      case RequestPart.Materials: {
        return Observable.of(true);
      }
      case RequestPart.Payments: {
        return Observable.of(
          (!this.isInitialStage(currentWorkflow.currentStageCode) &&
            !this.isFormationStage(currentWorkflow.currentStageCode)) ||
            RouteStageCodes.stageCodes.formation
              .filter(
                s =>
                  s.type === ProtectionDocTypeEnum.UsefulModels ||
                  s.type === ProtectionDocTypeEnum.Inventions
              )
              .allCodes()
              .includes(currentWorkflow.currentStageCode)
        );
      }
      case RequestPart.OfficeWork: {
        return Observable.of(true);
      }
      default:
        return Observable.of(false);
    }
  }

  afterCreateRequest(
    requestDetails: RequestDetails
  ): Observable<RequestDetails> {
    return this.doMockLogic(requestDetails);
  }

  afterUpdateRequest(
    requestDetails: RequestDetails
  ): Observable<RequestDetails> {
    return this.doMockLogic(requestDetails);
  }

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

  /**
   * Предикат проверки этапа, является ли этапом оплат
   * Возвращает положительный результат при этапах применения оплат
   * @param {string} stageCode публичный код этапа
   * @returns {boolean}
   * @memberof WorkflowBusinessService
   */
  isPaymentsStage(stageCode: string): boolean {
    return (
      RouteStageCodes.stageCodes.payment.allCodes().includes(stageCode) ||
      RouteStageCodes.stageCodes.formalExamComplete
        .allCodes()
        .includes(stageCode) ||
      RouteStageCodes.stageCodes.prepareSendToGosReestr
        .allCodes()
        .includes(stageCode)
    );
  }

  /**
   * Предикат проверки этапа, является ли этап этапом перед этапами оплат
   * Возвращает положительный результат при этапах перед применением оплат
   * @param {string} stageCode публичный код этапа
   * @returns {boolean}
   * @memberof WorkflowBusinessService
   */
  isPrePaymentStage(stageCode: string): boolean {
    return (
      RouteStageCodes.stageCodes.formation.allCodes().includes(stageCode) ||
      RouteStageCodes.stageCodes.formalExam.allCodes().includes(stageCode) ||
      RouteStageCodes.stageCodes.fullExamSuccessComplete
        .allCodes()
        .includes(stageCode)
    );
  }

  /**
   * Предикат проверки этапа, является ли этап формированием заявки
   * Возвращает положительный результат при этапах формирования заявок
   * @param {string} stageCode публичный код этапа
   * @returns {boolean}
   * @memberof WorkflowBusinessService
   */
  isFormationStage(stageCode: string): boolean {
    return RouteStageCodes.stageCodes.formation.allCodes().includes(stageCode);
  }

  /**
   * Предикат проверки этапа, является ли этап созданием заявки
   * Возвращает положительный результат при этапах создания заявок
   * @param {string} stageCode публичный код этапа
   * @returns {boolean}
   * @memberof WorkflowBusinessService
   */
  isInitialStage(stageCode: string): boolean {
    return RouteStageCodes.stageCodes.initial.allCodes().includes(stageCode);
  }

  /**
   * Предикат проверки выставленных счетов
   * Возвращает положительный результат, когда все счета оплачены
   * @param {RequestDetails} requestDetails заявка
   * @returns {boolean}
   * @memberof WorkflowBusinessService
   */
  areInvoicesPayed(requestDetails: RequestDetails): boolean {
    return (
      requestDetails.invoiceDtos.filter(i => i.statusCode === 'notpaid')
        .length === 0
    );
  }

  private getMainStage(
    stages: DicRouteStage[],
    protectionDocTypeCode: string
  ): DicRouteStage {
    let mainStages = stages.filter(s => s.isMain);

    if (mainStages.length > 1) {
      if (
        mainStages.map(s => s.code).includes('B04.1') &&
        mainStages.map(s => s.code).includes('B04.2')
      ) {
        if (protectionDocTypeCode === 'A' || protectionDocTypeCode === 'B') {
          return mainStages.filter(s => s.code === 'B04.1')[0];
        }
        if (protectionDocTypeCode === 'A4') {
          return mainStages.filter(s => s.code === 'B04.2')[0];
        }

        throw Error(`Request type code: ${protectionDocTypeCode} is uncompatible with stages:
         ${mainStages.map(s => s.code + '' + s.nameRu)}`);
      }

      mainStages = stages.filter(s =>
        RouteStageCodes.highPriorityMainStageCodes.includes(s.code)
      );

      if (mainStages.length > 1) {
        mainStages = mainStages.filter(
          s => !RouteStageCodes.lowPriorityMainStageCodes.includes(s.code)
        );
      }
    }

    return mainStages[0];
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

  private getTariff(
    previousStageCode: string,
    currentStageCode: string,
    receiveTypeId: number
  ): Observable<DicTariff> {
    if (
      this.isPrePaymentStage(previousStageCode) &&
      this.isPaymentsStage(currentStageCode)
    ) {
      return this.dictionaryService
        .getBaseDictionary(DictionaryType.DicTariff)
        .map((tariffs: DicTariff[]): DicTariff => {
          const compatibleSettingsArray = RouteStageCodes.stageTariffSettings.filter(
            s => s.stageCode === previousStageCode
          );
          if (!compatibleSettingsArray) {
            return null;
          }

          const compatibleTariffs = tariffs.filter(
            t =>
              compatibleSettingsArray.map(s => s.tariffCode).includes(t.code) &&
              (!t.receiveTypeId || t.receiveTypeId === receiveTypeId)
          );

          switch (compatibleTariffs.length) {
            case 2:

            case 1:
              return compatibleTariffs[0];
            case 0:
              return null;
            default:
              throw Error(`Found several tariffs: ${compatibleTariffs} for settings: ${compatibleSettingsArray}.
              Please do not ambigiuos settings!`);
          }
        });
    }

    return Observable.of(null);
  }

  doPaymentLogic(requestDetails: RequestDetails): Observable<RequestDetails> {
    const currentWorkflow = requestDetails.currentWorkflow;

    if (!currentWorkflow.fromStageCode) {
      throw Error('Incorrect call payment logic. Previous stage is unknown!');
    }

    return this.getTariff(
      currentWorkflow.fromStageCode,
      currentWorkflow.currentStageCode,
      requestDetails.receiveTypeId
    ).switchMap((tariff): Observable<RequestDetails> => {
      if (!tariff) {
        return Observable.of(requestDetails);
      }

      return this.dictionaryService
        .getSelectOptions(DictionaryType.DicPaymentStatus)
        .switchMap((statuses): Observable<RequestDetails> => {
          return this.paymentService
            .addInvoice(
              new PaymentInvoice({
                ownerId: requestDetails.id,
                tariffId: tariff.id,
                coefficient: this.paymentService.invoiceSettings.coefficient,
                tariffCount: this.paymentService.invoiceSettings.tariffCount,
                penaltyPercent: this.paymentService.invoiceSettings
                  .penaltyPercent,
                nds: this.paymentService.invoiceSettings.nds,
                statusId: statuses.filter(
                  s => s.code === this.paymentService.invoiceStatusCodes.notpaid
                )[0].id
              }),
              OwnerType.Request
            )
            .switchMap((invoice): Observable<RequestDetails> => {
              requestDetails.invoiceDtos.unshift(invoice);
              return Observable.of(Object.assign({}, requestDetails));
            });
        });
    });
  }

  private doMockLogic(
    requestDetails: RequestDetails
  ): Observable<RequestDetails> {
    return Observable.of(requestDetails);
  }

  private getNextWorkflow(
    currentWorkflow: Workflow,
    requestDetails: RequestDetails
  ): Observable<Workflow> {
    return this.routeStageService
      .getNextStagesByRequestId(
        currentWorkflow.currentStageId,
        requestDetails.id
      )
      .switchMap((stages: DicRouteStage[]): Observable<Workflow> => {
        let mainStageFound = true;
        let stage = this.getMainStage(
          stages,
          requestDetails.protectionDocTypeCode
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
      });
  }
}
