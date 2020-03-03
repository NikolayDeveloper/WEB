import {
  ProtectionTypeAndCodes,
  ProtectionDocTypeEnum
} from '../services/models/workflow-model';

export class RouteStageCodes {
  /**
   * Коды этапов, на которые завязана бизнес-логика
   */
  public static readonly stageCodes: {
    initial: ProtectionTypeAndCodes[];
    formation: ProtectionTypeAndCodes[];
    payment: ProtectionTypeAndCodes[];
    formalExam: ProtectionTypeAndCodes[];
    formalExamComplete: ProtectionTypeAndCodes[];
    fullExamSuccessComplete: ProtectionTypeAndCodes[];
    prepareSendToGosReestr: ProtectionTypeAndCodes[];
    fullExpertise: ProtectionTypeAndCodes[];
    makingChanges: ProtectionTypeAndCodes[];
    requestSeparation: ProtectionTypeAndCodes[];
    patentExam: ProtectionTypeAndCodes[];
  } = {
    initial: [
      { type: ProtectionDocTypeEnum.Trademarks, codes: ['TM01.1', 'TMI01.1'] },
      { type: ProtectionDocTypeEnum.COO, codes: ['NMPT01.1'] },
      { type: ProtectionDocTypeEnum.Inventions, codes: ['B01.1', 'B01.1_IN'] },
      { type: ProtectionDocTypeEnum.UsefulModels, codes: ['U01.1'] },
      { type: ProtectionDocTypeEnum.Industrialdesigns, codes: ['PO01.1'] },
      { type: ProtectionDocTypeEnum.Copyright, codes: ['AP01'] },
      { type: ProtectionDocTypeEnum.SelectiveAchievements, codes: ['SA01.1'] }
    ],
    formation: [
      { type: ProtectionDocTypeEnum.Trademarks, codes: ['TM02.1', 'TMIRequestDataFormation'] },
      { type: ProtectionDocTypeEnum.COO, codes: ['NMPT02.1', 'NMPT03.2.4'] },
      { type: ProtectionDocTypeEnum.Inventions, codes: ['B02.1', 'B02.1_IN'] },
      {
        type: ProtectionDocTypeEnum.UsefulModels,
        codes: ['U02.1', 'U02.2.7.0']
      },
      { type: ProtectionDocTypeEnum.Industrialdesigns, codes: ['PO02.1'] },
      { type: ProtectionDocTypeEnum.SelectiveAchievements, codes: ['SA02.1'] },
      { type: ProtectionDocTypeEnum.Copyright, codes: ['AP01.1'] },
      { type: ProtectionDocTypeEnum.InternationalTrademarks, codes: ['TMI02.1'] }
    ],
    payment: [
      {
        type: ProtectionDocTypeEnum.Trademarks,
        codes: ['TM02.2', 'TM02.2.2', 'TM03.2.2.1']
      },
      { type: ProtectionDocTypeEnum.COO, codes: ['NMPT02.2'] },
      {
        type: ProtectionDocTypeEnum.Inventions,
        codes: ['B02.2', 'B02.2_IN', 'B02.2.0.0']
      },
      { type: ProtectionDocTypeEnum.UsefulModels, codes: ['U02.2'] },
      { type: ProtectionDocTypeEnum.Industrialdesigns, codes: ['PO02.2'] },
      { type: ProtectionDocTypeEnum.SelectiveAchievements, codes: ['SA02.2'] }
    ],
    formalExam: [
      { type: ProtectionDocTypeEnum.Trademarks, codes: ['TM03.2.2'] },
      {
        type: ProtectionDocTypeEnum.Inventions,
        codes: ['B03.2.1', 'B03.2.1_IN', 'B02.2.1.0.0']
      },
      { type: ProtectionDocTypeEnum.Industrialdesigns, codes: ['PO03.2'] },
      { type: ProtectionDocTypeEnum.COO, codes: ['NMPT03.2'] }
    ],
    formalExamComplete: [
      { type: ProtectionDocTypeEnum.Trademarks, codes: ['TM03.2.2.2'] },
      {
        type: ProtectionDocTypeEnum.Inventions,
        codes: ['B03.2.1.1', 'B03.4.2_IN', 'B03.2.1']
      },
      { type: ProtectionDocTypeEnum.Industrialdesigns, codes: ['PO03.2.01'] }
    ],
    fullExamSuccessComplete: [
      { type: ProtectionDocTypeEnum.Trademarks, codes: ['TM03.3.6'] },
      { type: ProtectionDocTypeEnum.COO, codes: ['NMPT03.6.EDIT'] },
      { type: ProtectionDocTypeEnum.Inventions, codes: ['B03.3.5.EDIT'] },
      { type: ProtectionDocTypeEnum.Industrialdesigns, codes: ['PO03.7'] },
      { type: ProtectionDocTypeEnum.UsefulModels, codes: ['U03.6.EDIT'] },
      { type: ProtectionDocTypeEnum.SelectiveAchievements, codes: ['SA03.3'] }
    ],
    prepareSendToGosReestr: [
      { type: ProtectionDocTypeEnum.Trademarks, codes: ['TM03.3.7'] },
      { type: ProtectionDocTypeEnum.COO, codes: ['NMPT03.7'] },
      { type: ProtectionDocTypeEnum.Inventions, codes: ['B03.3.7.1'] },
      { type: ProtectionDocTypeEnum.Industrialdesigns, codes: ['PO03.8'] },
      { type: ProtectionDocTypeEnum.UsefulModels, codes: ['U03.7.1'] },
      { type: ProtectionDocTypeEnum.SelectiveAchievements, codes: ['SA03.4'] }
    ],
    fullExpertise: [
      {
        type: ProtectionDocTypeEnum.Trademarks,
        codes: ['TM03.3.2', 'B03.2.4', 'U03.1', 'TM03.3.2_2', 'TM03.3.2_1']
      },
      {
        type: ProtectionDocTypeEnum.Inventions,
        codes: ['B03.2.4']
      },
      {
        type: ProtectionDocTypeEnum.Industrialdesigns,
        codes: ['PO03.3']
      }
    ],
    makingChanges: [
      {
        type: ProtectionDocTypeEnum.Inventions,
        codes: ['B03.9']
      },
      {
        type: ProtectionDocTypeEnum.Trademarks,
        codes: ['TM06']
      },
      {
        type: ProtectionDocTypeEnum.COO,
        codes: ['NMPT03.2.4']
      },
      {
        type: ProtectionDocTypeEnum.UsefulModels,
        codes: ['U02.2.7.0']
      }
    ],
    requestSeparation: [
      {
        type: ProtectionDocTypeEnum.Trademarks,
        codes: ['TM03.3.2.2']
      }
    ],
    patentExam: [
      {
        type: ProtectionDocTypeEnum.Inventions,
        codes: ['B03.3.1', 'B03.2.4_1', 'B03.3.1_2', 'B03.2.4_0']
      },
      {
        type: ProtectionDocTypeEnum.UsefulModels,
        codes: ['U03.2', 'U02.2.5']
      }
    ]
  };

  /**
   * Коды не из основного сценария. Коды автоматически генерируемых этапов. Недоступно для ручного трансфера
   */
  public static readonly automaticStageCodes: string[] = [
    'TM03.3.9',
    'TM02.2.1',
    'TM02.2.2',
    'TM02.2.3',
    'TM03.2.2.1',
    'TM03.3.7.1',
    'TM03.3.9.0',
    'TM02.2.1.0',
    'TM03.3.4.1',
    'TM03.3.2.1',
    'TM03.3.2.2',
    'TM03.3.4.2',
    'TM03.3.7.0',
    'TM03.3.4.3',
    'TM03.3.9.1',
    'TM03.3.4.4.0',
    'TM03.3.4.4',
    'TM03.3.9.1',
    'TM03.3.2.0.0',

    'TMI03.3.3',
    'TMI03.3.3.1',
    'TMI03.3.4.5.1',
    'TMI03.3.5',
    'TMI03.3.8',
    'TMI03.3.9',
    'TMI03.3.2.0',
    'TMI03.3.4',
    'TMI03.3.4.2',
    'TMI03.3.4.3',
    'TMI03.3.4.5',

    'NMPT02.2.1',
    'NMPT03.2.1',
    'NMPT02.2.1',
    'NMPT03.3.1',
    'NMPT03.5',
    'NMPT03.6',
    'B04.0.0.1',
    'B02.3',
    'B04.0',
    'B04.0.0.0',
    'B03.3.1.1',
    'B03.2.2',
    'B03.2.2.1',
    'B02.2.0.3',
    'B03.2.4_1',
    'B03.3.1_1',
    'B03.3.4',
    'B03.3.5',
    'B03.3.4.1',
    'B03.3.7.3',
    'B03.3.7.2',
    'B03.1',
    'B03.3.7.0',
    'U01.2',
    'OUT03.1',
    'U02.2.3',
    'U02.2.2.2',
    'U03.2.3',
    'U02.2.0',
    'U03.2.2',
    'U03.2.1',
    'U03.7.2',
    'U03.7.3',
    'U03.5',
    'U03.6',
    'U03.7.0',
    'U02.2.4',
    'U02.3',
    'U02.2.7',
    'PO02.3',
    'PO02.2.0',
    'PO03.2.1',
    'PO03.2.02',
    'PO03.5.1',
    'PO03.3.0',
    'PO03.3.1',
    'PO03.8.0',
    'PO03.5.0',
    'PO03.8.0',
    'SA02.2.1',
    'SA02.3',
    'SA02.1.1', // SA02.1.1
    'SA03.3.1', // SA03.3.1
    'SA03.3.9' // SA03.3.9
  ];

  /**
   * Коды из основного сценария. Коды ручных этапов высокого приоритета
   */
  public static readonly highPriorityMainStageCodes: string[] = [
    'TMI03.3.3',
    'TMI03.3.4',
    'TMI03.3.4.4',
    'B03.2.1.1',
    'B03.2.1',
    'B03.2.3',
    'U02.2',
    'TM03.3.5',
    'TM03.3.7',
    'PO03.0',
    'PO03.1',
    'PO03.2',
    'PO03.2.0',
    'NMPT03.7',
    'NMPT04.1',
    'NMPT03.2',
    'NMPT03.3.5',
    'B03.3.7.1',
    'B03.2.2.1',
    'B02.3',
    'NMPT02.2',
    'B03.2.4',
    'U04',
    'U02.2.5',
    'U02.1.0'
  ];

  public static readonly lowPriorityMainStageCodes: string[] = [
    'TMI03.3.3',
    'TMI03.3.4',
    'TMI03.3.4.4',
    'B02.2.0',
    'B03.2.1',
    'B03.2.3',
    'U02.2',
    'PO03.1',
    'TM03.3.7',
    'TM03.2.1'
  ];

  public static readonly partialAutoStageCodes: string[] = [
    'B03.3.1.1',
    'B03.3.1.1.0',
    'B03.3.1.1.1'
  ];

  /**
   * Матрица определения тарифов по успешно завершенному этапу
   */
  public static readonly stageTariffSettings: {
    stageCode: string;
    tariffCode: string;
  }[] = [
    // Генерация во время ввода оплат, после успешного формирования
    { stageCode: 'NMPT02.1', tariffCode: 'NEW_078' },
    { stageCode: 'PO02.1', tariffCode: 'NEW_013' },
    { stageCode: 'PO02.1', tariffCode: 'NEW_014' },
    { stageCode: 'B02.1_IN', tariffCode: '100' },
    { stageCode: 'B02.1_IN', tariffCode: '101' },
    { stageCode: 'U02.1', tariffCode: 'NEW_011' },
    { stageCode: 'U02.1', tariffCode: 'NEW_012' },
    { stageCode: 'SA02.1', tariffCode: 'NEW_054' },
    { stageCode: 'SA02.1', tariffCode: 'NEW_055' },
    // Генерация во время передачи на полную, после успешной формальной экспертизы (по существу)
    { stageCode: 'TM03.2.2', tariffCode: 'NEW_076' },
    { stageCode: 'PO03.2', tariffCode: 'NEW_015' },
    { stageCode: 'B03.2.1_IN', tariffCode: '300' },
    // Генерация во время подготовки для передачи в госреестр, после успешного завершения полной экспертизы с положительным ответом
    { stageCode: 'TM03.3.6', tariffCode: 'NEW_081' },
    { stageCode: 'NMPT03.6.EDIT', tariffCode: 'NEW_081' },
    { stageCode: 'PO03.7', tariffCode: '9907' },
    { stageCode: 'B03.3.5.EDIT', tariffCode: '9907' },
    { stageCode: 'B03.3.2.3', tariffCode: '9907' },
    { stageCode: 'U03.6.EDIT', tariffCode: '9907' },
    { stageCode: 'SA03.3', tariffCode: 'NEW_057' }
  ];

  /**
   * Публичные кода типов заявок
   */
  public static readonly pdTypeCodes: ProtectionTypeAndCodes[] = [
    { type: ProtectionDocTypeEnum.Trademarks, codes: ['TM', 'TM', 'ITM', 'TM_PD'] },
    { type: ProtectionDocTypeEnum.COO, codes: ['PN', 'PN_PD'] },
    { type: ProtectionDocTypeEnum.Inventions, codes: ['A', 'A4', 'B', 'B_PD'] },
    { type: ProtectionDocTypeEnum.UsefulModels, codes: ['U', 'U_PD'] },
    { type: ProtectionDocTypeEnum.Industrialdesigns, codes: ['S1', 'S2', 'S2_PD'] },
    { type: ProtectionDocTypeEnum.SelectiveAchievements, codes: ['SA', 'SA_PD'] }
  ];

  public static get pdTypeTMCodes() {
    return this.pdTypeCodes.filter(
      c => c.type === ProtectionDocTypeEnum.Trademarks
    )[0].codes;
  }
  public static get pdTypeCOOCodes() {
    return this.pdTypeCodes.filter(c => c.type === ProtectionDocTypeEnum.COO)[0]
      .codes;
  }
  public static get pdTypeInventionsCodes() {
    return this.pdTypeCodes.filter(
      c => c.type === ProtectionDocTypeEnum.Inventions
    )[0].codes;
  }
  public static get pdTypeUsefulModelsCodes() {
    return this.pdTypeCodes.filter(
      c => c.type === ProtectionDocTypeEnum.UsefulModels
    )[0].codes;
  }
  public static get pdTypeIndustrialdesignsCodes() {
    return this.pdTypeCodes.filter(
      c => c.type === ProtectionDocTypeEnum.Industrialdesigns
    )[0].codes;
  }
  public static get pdTypeSelectiveAchievementsCodes() {
    return this.pdTypeCodes.filter(
      c => c.type === ProtectionDocTypeEnum.SelectiveAchievements
    )[0].codes;
  }
  public static get stagesinitial() {
    return this.stageCodes.initial.allCodes();
  }
  public static get stagesFormationAppData() {
    return this.stageCodes.formation.allCodes();
  }
  public static get stagesformalExam() {
    return this.stageCodes.formalExam.allCodes();
  }
  public static get stagesFullExpertise() {
    return this.stageCodes.fullExpertise.allCodes();
  }
  public static get stagesPayment() {
    return this.stageCodes.payment.allCodes();
  }
  public static get stagesMakingChanges() {
    return this.stageCodes.makingChanges.allCodes();
  }
  public static get stagesRequestSeparation() {
    return this.stageCodes.requestSeparation.allCodes();
  }
  public static get stagesPatentExam() {
    return this.stageCodes.patentExam.allCodes();
  }
}
