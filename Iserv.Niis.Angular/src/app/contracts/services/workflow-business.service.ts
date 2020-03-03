import { Workflow, ProtectionDocTypeEnum, ProtectionTypeAndCodes, WorkflowSendType } from '../../shared/services/models/workflow-model';
import { OwnerType } from '../../shared/services/models/owner-type.enum';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { DictionaryService } from '../../shared/services/dictionary.service';
import { DicRouteStage, DicTariff } from '../../shared/services/models/base-dictionary';
import { DictionaryType } from '../../shared/services/models/dictionary-type.enum';
import { RouteStageService } from '../../shared/services/route-stage.service';
import { ContractDetails } from '../models/contract-details';
import { PaymentService } from '../../payments/payment.service';
import { PaymentInvoice } from '../../payments/models/payment.model';
import { IntellectualPropertyDetails } from '../../shared/models/intellectual-property-details';
import { WorkflowService } from '../../shared/services/workflow.service';

export enum ContractPart {
  None,
  Contract,
  ContractSubject,
  Payments,
  Materials,
  OfficeWork,
}

/**
 * Коды из основного сценария. Коды ручных этапов высокого приоритета
 */
const highPriorityMainStageCodes: string[] = [
  'DK01.1', 'DK01.4', 'DK02.1', 'DK02.1.0', 'DK02.2', 'DK02.2.0', 'DK02.5.1', 'DK02.8', 'DK03.03', 'DK02.2.1', 'DK02.7.2'
];

const lowPriorityMainStageCodes: string[] = [];

/**
 * Коды этапов, на которые завязана бизнес-логика
 */
const stageCodes: {
  initial: ProtectionTypeAndCodes[],
  formation: ProtectionTypeAndCodes[],
  payment: ProtectionTypeAndCodes[],
  formalExam: ProtectionTypeAndCodes[],
  formalExamComplete: ProtectionTypeAndCodes[],
  fullExamSuccessComplete: ProtectionTypeAndCodes[],
  prepareSendToGosReestr: ProtectionTypeAndCodes[],
  controlling: ProtectionTypeAndCodes[],
  ministryOfJustice: ProtectionTypeAndCodes[],
  publication: ProtectionTypeAndCodes[],
  registration: ProtectionTypeAndCodes[],
} = {
    initial: [
      { type: ProtectionDocTypeEnum.CommercializationAgreements, codes: ['DK01.1'], }
    ],
    formation: [
      { type: ProtectionDocTypeEnum.CommercializationAgreements, codes: ['DK02.1'], }
    ],
    payment: [
      { type: ProtectionDocTypeEnum.CommercializationAgreements, codes: ['DK02.1.0', 'DK02.4.0', 'DK02.4.1'], }
    ],
    formalExam: [
      { type: ProtectionDocTypeEnum.CommercializationAgreements, codes: ['DK02.2', 'DK02.2.0'], }
    ],
    formalExamComplete: [
      { type: ProtectionDocTypeEnum.CommercializationAgreements, codes: ['DK02.4'], }
    ],
    fullExamSuccessComplete: [
      { type: ProtectionDocTypeEnum.CommercializationAgreements, codes: [], }
    ],
    prepareSendToGosReestr: [
      { type: ProtectionDocTypeEnum.CommercializationAgreements, codes: ['DK03.01'], }
    ],
    controlling: [
      { type: ProtectionDocTypeEnum.CommercializationAgreements, codes: ['DK02.5.1', 'DK02.5.3', 'DK02.5.4'], }
    ],
    ministryOfJustice: [
      { type: ProtectionDocTypeEnum.CommercializationAgreements, codes: ['DK02.7', 'DK02.8', 'DK02.9.2'], }
    ],
    publication: [
      { type: ProtectionDocTypeEnum.CommercializationAgreements, codes: ['DK03.00', 'DK03.03', 'DK02.9.1', 'DK03.2', 'DK02.2'], }
    ],
    registration: [
      { type: ProtectionDocTypeEnum.CommercializationAgreements, codes: ['DK02.1'], }
    ]
  };

/**
 * Матрица определения тарифов по успешно завершенному этапу
 */
const stageTariffSettings: { previousStageCode: string, currentSatgeCode: string, tariffCode: string }[] = [
  { previousStageCode: 'DK02.1', currentSatgeCode: 'DK02.1.0', tariffCode: '3_2018' }
];

/**
 * Публичные кода типов заявок
 */
const pdTypeCodes: ProtectionTypeAndCodes[] = [
  { type: ProtectionDocTypeEnum.Trademarks, codes: ['TM', 'TM', 'ITM'] },
  { type: ProtectionDocTypeEnum.COO, codes: ['PN'], },
  { type: ProtectionDocTypeEnum.Inventions, codes: ['A', 'A4', 'B'], },
  { type: ProtectionDocTypeEnum.UsefulModels, codes: ['U'], },
  { type: ProtectionDocTypeEnum.Industrialdesigns, codes: ['S1', 'S2'], },
  { type: ProtectionDocTypeEnum.SelectiveAchievements, codes: ['SA'], },
  { type: ProtectionDocTypeEnum.CommercializationAgreements, codes: ['DK'], },
];

@Injectable()

export class WorkflowBusinessService {
  get pdTypeTMCodes() { return pdTypeCodes.filter(c => c.type === ProtectionDocTypeEnum.Trademarks)[0].codes; }
  get pdTypeCOOCodes() { return pdTypeCodes.filter(c => c.type === ProtectionDocTypeEnum.COO)[0].codes; }
  get pdTypeInventionsCodes() { return pdTypeCodes.filter(c => c.type === ProtectionDocTypeEnum.Inventions)[0].codes; }
  get pdTypeUsefulModelsCodes() { return pdTypeCodes.filter(c => c.type === ProtectionDocTypeEnum.UsefulModels)[0].codes; }
  get pdTypeIndustrialdesignsCodes() { return pdTypeCodes.filter(c => c.type === ProtectionDocTypeEnum.Industrialdesigns)[0].codes; }
  get pdTypeSelectiveAchievementsCodes() {
    return pdTypeCodes.filter(c => c.type === ProtectionDocTypeEnum.SelectiveAchievements)[0]
      .codes;
  }
  get pdTypeCommercializationAgreementsCodes() {
    return pdTypeCodes.filter(c => c.type === ProtectionDocTypeEnum.CommercializationAgreements)[0]
      .codes;
  }

  constructor(
    private dictionaryService: DictionaryService,
    private routeStageService: RouteStageService,
    private paymentService: PaymentService,
    private workflowService: WorkflowService) { }

  generateWorkflowBy(sendTypeSubject: Observable<WorkflowSendType>, contractDetails: ContractDetails, workflows: Workflow[]):
    Observable<Workflow> {
    const currentWorkflow = contractDetails.currentWorkflow;

    return sendTypeSubject
      .switchMap((type: WorkflowSendType): Observable<Workflow> => {
        if (!currentWorkflow) {
          return Observable.of(null);
        }

        switch (type) {
          case WorkflowSendType.ToNextByRoute: {
            return this.routeStageService.isLast(currentWorkflow.currentStageId)
              .switchMap((isLast: boolean): Observable<Workflow> => {
                if (isLast) {
                  return Observable.of(null);
                }
                return this.workflowService.getNextStagesByContractId(contractDetails.id)
                  .switchMap(stages => {
                    const stage = stages[0];
                    if (!stage) {
                      return Observable.of(null);
                    }
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
                  .switchMap((previousStages: DicRouteStage[]): Observable<Workflow> => {
                    const previousByOrderWorkflow = this.getPreviousByOrderWorkflow(workflows, previousStages);
                    const rawWorkflow = this.createRawWorkflow(currentWorkflow);
                    rawWorkflow.currentStageId = previousByOrderWorkflow.currentStageId;
                    rawWorkflow.currentStageCode = previousByOrderWorkflow.currentStageCode;
                    rawWorkflow.currentUserId = previousByOrderWorkflow.currentUserId;
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

  // TODO: Завязать также на роли. Админу дать full access
  /**
   * Предикат доступности трансфера по этапам
   *
   * @param {ContractDetails} contractDetails Договор
   * @returns {Observable<boolean>} флаг доступности
   * @memberof WorkflowBusinessService
   */
  public availableOfTransfer(contractDetails: ContractDetails): Observable<boolean> {
    const currentWorkflow = contractDetails.currentWorkflow;

    // Создается заявка, еще нет инициального этапа. Трансфер недоступен
    if (!currentWorkflow || currentWorkflow.isComplete) {
      return Observable.of(false);
    }

    return this.workflowService.getNextStagesByContractId(contractDetails.id)
      .switchMap(stages => {
        return Observable.of(stages.length > 0);
      });
  }
  private deleteStageByCode(stages: DicRouteStage[], code: string): void {
    const index = stages.findIndex(r => r.code === code);
    if (index !== -1) {
      stages.splice(index, 1);
    }
  }
  createRawWorkflow(currentWorkflow: Workflow): Workflow {
    return new Workflow({
      ownerId: currentWorkflow.ownerId,
      fromStageId: currentWorkflow.currentStageId,
      fromUserId: currentWorkflow.currentUserId,
    });
  }

  // TODO: Завязать также на роли. Админу дать full access
  /**
   * Предикат доступности редактирования и других действий (применение оплат) для вкладки
   *
   * @param {ContractDetails} contractDetails Договор
   * @param {ContractPart} part Вкладка заявки
   * @returns {Observable<boolean>} Флаг доступности
   * @memberof WorkflowBusinessService
   */
  public availableAtStage(contractDetails: ContractDetails, part: ContractPart): Observable<boolean> {
    const currentWorkflow = contractDetails.currentWorkflow;

    if (!currentWorkflow) {
      return Observable.of(part === ContractPart.Contract);
    }

    switch (part) {
      // case ContractPart.Contract: {
      //   return Observable.of(stageCodes.initial[0].codes.includes(currentWorkflow.currentStageCode));
      // }
      // case ContractPart.ContractSubject: {
      //   return Observable.of(stageCodes.formation[0].codes.includes(currentWorkflow.currentStageCode)
      //     || stageCodes.formalExam[0].codes.includes(currentWorkflow.currentStageCode));
      // }
      // case ContractPart.Materials: {
      //   return Observable.of(true);
      // }
      case ContractPart.Payments: {
        return Observable.of(!(this.isInitialStage(currentWorkflow.currentStageCode)
          || this.isFormationStage(currentWorkflow.currentStageCode)));
      }
      // case ContractPart.OfficeWork: {
      //   return Observable.of(true);
      // }
      default:
        return Observable.of(true);
    }
  }

  /**
   * Предикат определения этапов предварительных экспертиз
   * Возвращает положительный результат, когда текущий этап относится к этапам предварительных экспертиз
   * @param {ContractDetails} contractDetails Договор
   * @memberof WorkflowBusinessService
   */
  isFormalExamStage(contractDetails: ContractDetails) {
    return stageCodes.formalExam[0].codes.includes(contractDetails.currentWorkflow.currentStageCode);
  }

  /**
   * Предикат определения этапов предварительных экспертиз
   * Возвращает положительный результат, когда текущий этап относится к этапам предварительных экспертиз
   * @param {ContractDetails} contractDetails Договор
   * @memberof WorkflowBusinessService
   */
  isFormalExamCompleteStage(contractDetails: ContractDetails) {
    return contractDetails.currentWorkflow &&
      stageCodes.formalExamComplete[0].codes.includes(contractDetails.currentWorkflow.currentStageCode);
  }

  /**
   * Предикат определения этапов контроля
   * Возвращает положительный результат, когда текущий этап относится к этапам контроля
   * @param {ContractDetails} contractDetails Договор
   * @memberof WorkflowBusinessService
   */
  isControllingStage(contractDetails: ContractDetails) {
    return contractDetails.currentWorkflow &&
      stageCodes.controlling[0].codes.includes(contractDetails.currentWorkflow.currentStageCode);
  }

  /**
   * Предикат проверки этапа, является ли этапом оплат
   * Возвращает положительный результат при этапах применения оплат
   * @param {string} stageCode публичный код этапа
   * @returns {boolean}
   * @memberof WorkflowBusinessService
   */
  isPaymentsStage(stageCode: string): boolean {
    return stageCodes.payment[0].codes.includes(stageCode);
    // || stageCodes.formalExamComplete[0].codes.includes(stageCode)
    // || stageCodes.prepareSendToGosReestr[0].codes.includes(stageCode);
  }

  /**
   * Предикат проверки этапа, является ли этап этапом перед этапами оплат
   * Возвращает положительный результат при этапах перед применением оплат
   * @param {string} stageCode публичный код этапа
   * @returns {boolean}
   * @memberof WorkflowBusinessService
   */
  isPrePaymentStage(stageCode: string): boolean {
    return stageCodes.formation[0].codes.includes(stageCode) ||
      stageCodes.formalExam[0].codes.includes(stageCode) ||
      stageCodes.fullExamSuccessComplete[0].codes.includes(stageCode);
  }

  /**
   * Предикат проверки этапа, является ли этап формированием заявки
   * Возвращает положительный результат при этапах формирования заявок
   * @param {string} stageCode публичный код этапа
   * @returns {boolean}
   * @memberof WorkflowBusinessService
   */
  isFormationStage(stageCode: string): boolean {
    return stageCodes.formation[0].codes.includes(stageCode);
  }

  isRegistrationStage(stageCode: string): boolean {
    return stageCodes.registration[0].codes.includes(stageCode);
  }

  /**
   * Возвращает положительный результат при этапах публикации договора
   * @param {string} stageCode публичный код этапа
   * @returns {boolean}
   * @memberof WorkflowBusinessService
   */
  isPublicationStage(stageCode: string): boolean {
    return stageCodes.publication[0].codes.includes(stageCode);
  }

  /**
  * Предикат проверки этапа, является ли этап созданием заявки
  * Возвращает положительный результат при этапах создания заявок
  * @param {string} stageCode публичный код этапа
  * @returns {boolean}
  * @memberof WorkflowBusinessService
  */
  isInitialStage(stageCode: string): boolean {
    return stageCodes.initial[0].codes.includes(stageCode);
  }

  /**
   * Предикат проверки выставленных счетов
   * Возвращает положительный результат, когда все счета оплачены
   * @param {ContractDetails} contractDetails Договор
   * @returns {boolean}
   * @memberof WorkflowBusinessService
   */
  areInvoicesPayed(details: IntellectualPropertyDetails): boolean {
    return details.invoiceDtos.filter(i => i.statusCode === 'notpaid').length === 0;
  }

  doPaymentLogic(contractDetails: ContractDetails): Observable<ContractDetails> {
    const currentWorkflow = contractDetails.currentWorkflow;

    if (!currentWorkflow.fromStageCode) {
      throw Error('Incorrect call payment logic. Previous stage is unknown!');
    }

    return this.getTariff(currentWorkflow.fromStageCode, currentWorkflow.currentStageCode, contractDetails.receiveTypeId)
      .switchMap((tariffs: DicTariff[]): Observable<ContractDetails> => {
        if (!tariffs || !tariffs.length) {
          return Observable.of(contractDetails);
        }

        return this.dictionaryService.getSelectOptions(DictionaryType.DicPaymentStatus)
          .switchMap((statuses): Observable<ContractDetails> => {
            const statusId = statuses.filter(s => s.code === this.paymentService.invoiceStatusCodes.notpaid)[0].id;
            const invoices = this.getPaymentInvoices(tariffs, contractDetails.id, statusId);
            return this.paymentService
              .addRangeInvoice(invoices, OwnerType.Contract)
              .switchMap((invoice): Observable<ContractDetails> => {
                contractDetails.invoiceDtos = contractDetails.invoiceDtos.concat(invoice);
                return Observable.of(Object.assign({}, contractDetails));
              });
          });
      });
  }
  getPaymentInvoices(tariffs: DicTariff[], contractId: number, statusId: number): PaymentInvoice[] {
    const invoices: PaymentInvoice[] = [];
    for (const tariff of tariffs) {
      const invoice = new PaymentInvoice({
        ownerId: contractId,
        tariffId: tariff.id,
        coefficient: this.paymentService.invoiceSettings.coefficient,
        tariffCount: this.paymentService.invoiceSettings.tariffCount,
        penaltyPercent: this.paymentService.invoiceSettings.penaltyPercent,
        nds: this.paymentService.invoiceSettings.nds,
        statusId: statusId
      });
      invoices.push(invoice);
    }
    return invoices;
  }
  public getMainStage(contractDetails: ContractDetails, stages: DicRouteStage[]): DicRouteStage[] {
    const currentWorkflow = contractDetails.currentWorkflow;
    const mainStages = stages.filter(s => s.isMain && s.isAuto === false && highPriorityMainStageCodes.includes(s.code));

    if (currentWorkflow.currentStageCode === 'DK01.4') {
      if (!contractDetails.fullExpertiseExecutorId) {
        this.deleteStageByCode(mainStages, 'DK02.1');
      }
    }

    if (this.isInitialStage(currentWorkflow.currentStageCode)) {
      const isExistCorrespondence = !!contractDetails.typeCode &&
        contractDetails.subjects.some(s => s.roleCode === 'CORRESPONDENCE');
      if (!isExistCorrespondence) {
        this.deleteStageByCode(mainStages, 'DK01.4');
      }
    }

    if (this.isFormationStage(currentWorkflow.currentStageCode)) {
      const haveBothParties = ['7', '8'].every(party => contractDetails.subjects.map(s => s.roleCode).includes(party));
      if (!haveBothParties) {
        this.deleteStageByCode(mainStages, 'DK02.1.0');
      }
    }

    if (this.isFormalExamCompleteStage(contractDetails)) {
      if (!contractDetails.gosNumber) {
        this.deleteStageByCode(mainStages, 'DK02.5.1');
      }
    }

    return mainStages;
  }

  private getPreviousByOrderWorkflow(workflows: Workflow[], previousStages: DicRouteStage[]): Workflow {
    const stageWorkflows = workflows
      .filter(w => previousStages.map(s => s.id).includes(w.currentStageId))
      .sort((e1, e2) => new Date(e1.dateCreate).getTime() - new Date(e2.dateCreate).getTime());

    if (!stageWorkflows || stageWorkflows.length === 0) {
      throw Error(`Previous workflow define error`);
    }
    return stageWorkflows[0];
  }

  private getTariff(previousStageCode: string, currentStageCode: string, receiveTypeId: number): Observable<DicTariff[]> {
    if (this.isPrePaymentStage(previousStageCode) && this.isPaymentsStage(currentStageCode)) {
      return this.dictionaryService
        .getBaseDictionary(DictionaryType.DicTariff)
        .map((tariffs: DicTariff[]): DicTariff[] => {
          const compatibleSettingsArray = stageTariffSettings.filter(s => s.previousStageCode === previousStageCode
            && s.currentSatgeCode === currentStageCode);
          if (!compatibleSettingsArray) {
            return null;
          }

          const compatibleTariffs = tariffs.filter(t => compatibleSettingsArray.map(s => s.tariffCode).includes(t.code)
            && (!t.receiveTypeId || t.receiveTypeId === receiveTypeId));
          return compatibleTariffs;
        });
    }

    return Observable.of(null);
  }

  private getNextWorkflows(currentWorkflow: Workflow, contractDetails: ContractDetails): Observable<Workflow[]> {
    return this.routeStageService
      .getNextStages(currentWorkflow.currentStageId)
      .switchMap((stages: DicRouteStage[]): Observable<Workflow[]> => {
        let mainStageFound = true;
        const mainStages = this.getMainStage(contractDetails, stages);
        if (mainStages.length < 1) {
          const firstNotMainStage = stages[0];
          mainStages.push(firstNotMainStage);
          mainStageFound = false;

          console.log(
            firstNotMainStage
              ? `The next stages don\'t have a main stage. All possible next stages count is ${stages.length}.
                  Selected stage is (by default): ${JSON.stringify(firstNotMainStage)}`
              : `The next stage is not found! Please contact the administrator!`);
        }

        const rawWorkflows = this.createRangeWorkflows(currentWorkflow, mainStages, mainStageFound);
        return Observable.of(rawWorkflows);
      });
  }

  private createRangeWorkflows(currentWorkflow: Workflow, stages: DicRouteStage[], mainStageFound: boolean) {
    const workflows: Workflow[] = [];
    for (const stage of stages) {
      const workflow = new Workflow({
        ownerId: currentWorkflow.ownerId,
        fromStageId: currentWorkflow.currentStageId,
        fromUserId: currentWorkflow.currentUserId,
      });
      workflow.currentStageId = stage.id;
      workflow.currentStageCode = stage.code;
      workflow.routeId = stage.routeId;
      workflow.isComplete = stage.isLast;
      workflow.isMain = stage.isMain;
      workflow.isAuto = stage.isAuto || !mainStageFound;
      workflows.push(workflow);
    }
    return workflows;
  }
}
