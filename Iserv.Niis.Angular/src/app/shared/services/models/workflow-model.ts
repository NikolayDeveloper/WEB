export class Workflow {
    id: number;
    ownerId: number;
    dateCreate: Date;
    fromStageId: number;
    fromStageCode: string;
    fromStageNameRu: string;
    fromUserId: number;
    fromUserNameRu: string;
    currentStageId: number;
    currentStageCode: string;
    currentStageNameRu: string;
    currentUserId: number;
    currentUserNameRu: string;
    isComplete: boolean;
    isSystem: boolean;
    isAuto: boolean;
    isMain: boolean;
    description: string;
    routeId: number;
    routeNameRu: string;
    controlDate: Date;
    dateReceived: Date;

    workflowSendType: WorkflowSendType;

    // Extra data fields
    statusId: number;
    contractGosDate: Date;
    fullExpertiseExecutorId: number;
    applicationDateCreate: Date;

    public constructor(init?: Partial<Workflow>) {
        Object.assign(this, init);
    }
}

export class MaterialWorkflow {
    id: number;
    isCurent: boolean;
    ownerId: number;
    dateCreate: Date;
    fromStageId: number;
    fromStageCode: string;
    fromStageNameRu: string;
    fromUserId: number;
    fromUserNameRu: string;
    currentStageId: number;
    currentStageCode: string;
    currentStageIsSign: boolean;
    currentStageNameRu: string;
    currentUserId: number;
    currentUserDepartmentId: number;
    currentUserNameRu: string;
    isComplete: boolean;
    isMain: boolean;
    isSystem: boolean;
    description: string;
    routeId: number;
    routeNameRu: string;
    controlDate: Date;
    dateReceived: Date;
    workflowSendType: WorkflowSendType;
    isSend: boolean;
    outgoingNum: string;
    documentDate: Date;
    isSigned: boolean;
    previousWorkflowId: number;

    public constructor(init?: Partial<MaterialWorkflow>) {
        Object.assign(this, init);
    }
}

export enum WorkflowSendType {
    None,
    /// <summary>
    /// Далее по маршруту
    /// </summary>
    ToNextByRoute,
    /// <summary>
    /// По текущему этапу
    /// </summary>
    ToCurrentStage,
    /// <summary>
    /// Возврат по маршруту
    /// </summary>
    ReturnByRoute,
    /// <summary>
    /// Возврат отправителю
    /// </summary>
    ReturnToSender,
    /// <summary>
    /// Завершение обработки
    /// </summary>
    FinishProcessing,
    /// <summary>
    /// Восстановление (по этапам)
    /// </summary>
    RepairForStage,
    /// <summary>
    /// Восстановление
    /// </summary>
    Repair,
    /// <summary>
    /// Отправить на e-mail
    /// </summary>
    SendToEmail,
    /// <summary>
    /// Завершение обработки параллельного этапа
    /// </summary>
    FinishParallelProcessing,
}

export enum ProtectionDocTypeEnum {
    Inventions,
    UsefulModels,
    Industrialdesigns,
    SelectiveAchievements,
    Copyright,
    COO, // НМПТ
    InternationalTrademarks,
    Trademarks,
    CommercializationAgreements
}
export interface ProtectionTypeAndCodes {
    type: ProtectionDocTypeEnum,
    codes: string[],
}
