import { OwnerType } from '../../shared/services/models/owner-type.enum';

export interface Request {
  id: number;
  barcode: number;
  dateCreate: Date;
  protectionDocTypeValue: string;
  currentStageValue: string;
  canGenerateGosNumber: boolean;
  reviewDaysAll: Date;
  reviewDaysStage: Date;
  isRead: boolean;
  isComplete: boolean;
  taskType: string;
  nameRu: string;
  regNumber: string;
  requestNum: string;
  priority: TaskPriority;
  canDownload: boolean;
  ownerType: OwnerType;
  isIndustrial: boolean;
  ipcCodes: string;
  pageCount: number;
  expertId: number;
  countIndependentItems: number;
  coefficientComplexity: number;
  isActiveProtectionDocument: boolean;
}

export enum TaskPriority {
  /// <summary>
  /// Непросроченные задачи (не подсвечивать)
  /// </summary>
  Normal = 0,

  /// <summary>
  /// Просрочено менее чем на 5 календарных дней (подсвечивать жёлтым)
  /// </summary>
  Yellow = 1,

  /// <summary>
  /// Просрочено на 5 или более календарных дней (подсвечивать красным)
  /// </summary>
  Red = 2,

  /// <summary>
  /// Экспертиза не оплачена
  /// </summary>
  Orange = 3
}
