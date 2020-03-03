import { Priority } from './priority.enum';
import { ControlType } from './control-type.enum';
import { ProtectionDocTypes } from 'app/shared/enums/protection-doc-types.enum';

class Field {
  type: ControlType;
  name: string;
  placeholder: string;
  group: string;
  disabled: boolean;
  required: boolean;

  constructor(type, name, placeholder) {
    this.type = type;
    this.name = name;
    this.placeholder = placeholder;
    this.group = Field.DEFAULT_GROUP;
    this.disabled = false;
    this.required = false;
  }

  static get DEFAULT_GROUP() {
    return 'default';
  }

  setGroup(group) {
    this.group = group;

    return this;
  }

  setDisabled(value = true) {
    this.disabled = value;

    return this;
  }

  setRequired(value = true) {
    this.required = value;

    return this;
  }
}

class Configuration {
  name: string;
  title: string;
  fields: Field[];
  values: any;
  default: any;
  available: any;

  constructor(name, title) {
    this.name = name;
    this.title = title;
    this.fields = [];
    this.values = {};
    this.default = {};
    this.available = null;
  }

  setFields(...data) {
    this.fields = data;

    return this;
  }

  setValues(data) {
    this.values = data;

    return this;
  }

  setDefault(data) {
    this.default = data;

    return this;
  }

  setAvailable(callback) {
    this.available = callback;

    return this;
  }
}

export const PCTConfiguration = [
  new Configuration(Priority.InternationalApplication, 'Международная заявка')
    .setFields(
      new Field(ControlType.Input, 'regNumber', 'Номер').setGroup('line_1').setRequired(),
      new Field(ControlType.Select, 'regCountryId', 'Страна').setGroup('line_1').setRequired(),
      new Field(ControlType.Date, 'regDate', 'Дата').setGroup('line_1').setRequired(),
      new Field(ControlType.Input, 'chapterOne', 'Глава 1').setGroup('line_2').setRequired(),
      new Field(ControlType.Date, 'dateOfChapterOne', 'Дата главы 1').setGroup('line_2').setRequired(),
      new Field(ControlType.Input, 'chapterTwo', 'Глава 2').setGroup('line_3').setRequired(),
      new Field(ControlType.Date, 'dateOfChapterTwo', 'Дата главы 2').setGroup('line_3').setRequired(),
    ).setDefault({
      country: 1,
      date: new Date()
    }),
  new Configuration(Priority.PublicationInternationalApplication, 'Публикация международной заявки')
    .setFields(
      new Field(ControlType.Input, 'regNumber', 'Номер').setGroup('line_1').setRequired(),
      new Field(ControlType.Select, 'regCountryId', 'Страна').setGroup('line_1').setRequired(),
      new Field(ControlType.Date, 'regDate', 'Дата').setGroup('line_1').setRequired(),
    ).setDefault({
      country: 1,
      date: new Date()
    })
];

export const EurasionApplicationConfiguration = [
  new Configuration(Priority.EurasianApplication, 'Евразийская заявка')
    .setFields(
      new Field(ControlType.Input, 'regNumber', 'Номер').setRequired(),
      new Field(ControlType.Select, 'regCountryId', 'Страна').setRequired(),
      new Field(ControlType.Date, 'regDate', 'Дата').setRequired()
    )
    .setDefault({
      country: 250,
      date: new Date()
    }),
  new Configuration(Priority.PublicationEurasianApplication, 'Публикация евразийской заявки')
    .setFields(
      new Field(ControlType.Input, 'regNumber', 'Номер').setRequired(),
      new Field(ControlType.Select, 'regCountryId', 'Страна').setRequired(),
      new Field(ControlType.Date, 'regDate', 'Дата').setRequired()
    )
    .setDefault({
      country: 250,
      date: new Date()
    })
];

export const PriorityDataConfiguration = [
  new Configuration(Priority.PriorityNextPendingNationalApplication, 'Приоритет следующей предшествующей заявки (национальная заявка)')
    .setFields(
      new Field(ControlType.Input, 'regNumber', '(31) № первой, более ранней, первоначальной заявки').setRequired(),
      new Field(ControlType.Date, 'regDate', '(32) Дата испрашиваемого приоритета').setRequired(),
      new Field(ControlType.Select, 'regCountryId', '(33) Код страны подачи по ST.3 (при испрашивании конвенционного приоритета)').setRequired()
    )
    .setDefault({
      date: new Date()
    })
    .setAvailable((applicationType) => {
      // Заявка на изобретение (РСТ)
      return false;
    }),
  new Configuration(Priority.PriorityNextPendingRegionalApplication, 'Приоритет следующей предшествующей заявки (региональная заявка)')
    .setFields(
      new Field(ControlType.Input, 'regNumber', '(31) № первой, более ранней, первоначальной заявки').setRequired(),
      new Field(ControlType.Date, 'regDate', '(32) Дата испрашиваемого приоритета').setRequired(),
      new Field(ControlType.Select, 'regCountryId', '(33) Код страны подачи по ST.3 (при испрашивании конвенционного приоритета)').setRequired()
    )
    .setDefault({
      date: new Date()
    })
    .setAvailable((applicationType) => {
      // Заявка на изобретение (РСТ)
      return false;
    }),
  new Configuration(Priority.PriorityNextPendingInternationalApplication, 'Приоритет следующей предшествующей заявки (международная заявка)')
    .setFields(
      new Field(ControlType.Input, 'regNumber', '(31) № первой, более ранней, первоначальной заявки').setRequired(),
      new Field(ControlType.Date, 'regDate', '(32) Дата испрашиваемого приоритета').setRequired(),
      new Field(ControlType.Select, 'regCountryId', '(33) Код страны подачи по ST.3 (при испрашивании конвенционного приоритета)').setRequired()
    )
    .setDefault({
      date: new Date()
    })
    .setAvailable((applicationType) => {
      // Заявка на изобретение (РСТ)
      return false;
    }),
  new Configuration(Priority.FirstApplicationParisConvention, 'Первой(ых) заявки(ок) в государстве-участнике Парижской конвенции (п. 2 ст. 20 Закона)')
    .setFields(
      new Field(ControlType.Input, 'regNumber', '(31) № первой, более ранней, первоначальной заявки').setRequired(),
      new Field(ControlType.Date, 'regDate', '(32) Дата испрашиваемого приоритета').setRequired(),
      new Field(ControlType.Select, 'regCountryId', '(33) Код страны подачи по ST.3 (при испрашивании конвенционного приоритета)').setRequired()
    )
    .setDefault({
      date: new Date()
    })
    .setAvailable((applicationType) => {
      // Заявка на изобретение (ЕАПО)
      return (applicationType === ProtectionDocTypes.Invention || applicationType === ProtectionDocTypes.IndustrialDesign || applicationType === ProtectionDocTypes.UsefulModel);
    }),
  new Configuration(Priority.EqualsParagraph3Article20, 'Поступления дополнительных материалов к более ранней заявки в соответствии с п. 3 ст. 20 Закона')
    .setFields(
      new Field(ControlType.Input, 'regNumber', '(31) № первой, более ранней, первоначальной заявки').setRequired(),
      new Field(ControlType.Date, 'regDate', '(32) Дата испрашиваемого приоритета').setRequired(),
      new Field(ControlType.Select, 'regCountryId', '(33) Код страны подачи по ST.3 (при испрашивании конвенционного приоритета)').setRequired()
    )
    .setDefault({
      date: new Date()
    })
    .setAvailable((applicationType) => {
      return (applicationType === ProtectionDocTypes.Invention || applicationType === ProtectionDocTypes.IndustrialDesign || applicationType === ProtectionDocTypes.UsefulModel);
    }),
  new Configuration(Priority.EqualsParagraph4Article20, 'Более ранней заявки в РГП "НИИС" в соответствии с п. 4 ст. 20 Закона')
    .setFields(
      new Field(ControlType.Input, 'regNumber', '(31) № первой, более ранней, первоначальной заявки').setRequired(),
      new Field(ControlType.Date, 'regDate', '(32) Дата испрашиваемого приоритета').setRequired(),
      new Field(ControlType.Select, 'regCountryId', '(33) Код страны подачи по ST.3 (при испрашивании конвенционного приоритета)').setRequired()
    )
    .setDefault({
      date: new Date()
    })
    .setAvailable((applicationType) => {
      return (applicationType === ProtectionDocTypes.Invention || applicationType === ProtectionDocTypes.IndustrialDesign || applicationType === ProtectionDocTypes.UsefulModel);
    }),
  new Configuration(Priority.EqualsParagraph5Article20, 'Первоначальной заявки в РГП "НИИС" в соответствии с п. 5 ст. 20 Закона')
    .setFields(
      new Field(ControlType.Input, 'regNumber', '(31) № первой, более ранней, первоначальной заявки').setRequired(),
      new Field(ControlType.Date, 'regDate', '(32) Дата испрашиваемого приоритета').setRequired(),
      new Field(ControlType.Select, 'regCountryId', '(33) Код страны подачи по ST.3 (при испрашивании конвенционного приоритета)').setRequired()
    )
    .setDefault({
      date: new Date()
    })
    .setAvailable((applicationType) => {
      return (applicationType === ProtectionDocTypes.Invention || applicationType === ProtectionDocTypes.IndustrialDesign || applicationType === ProtectionDocTypes.UsefulModel);
    }),
  new Configuration(Priority.EqualsParagraph6Article20, 'В соответствии с п. 6 ст. 20 Закона')
    .setFields(
      new Field(ControlType.Input, 'regNumber', 'Номер').setRequired(),
      new Field(ControlType.Select, 'regCountryId', 'Страна').setRequired(),
      new Field(ControlType.Date, 'regDate', 'Дата').setRequired()
    )
    .setDefault({
      country: 1,
      date: new Date()
    }),
  new Configuration(Priority.PriorityData, 'Приоритетные данные')
    .setFields(
      new Field(ControlType.Input, 'regNumber', 'Номер первой заявки').setRequired(),
      new Field(ControlType.Date, 'regDate', 'Дата испрашиваемого приоритета').setRequired(),
      new Field(ControlType.Select, 'regCountryId', 'Код страны подачи по ST.3').setRequired()
    )
    .setDefault({
      date: new Date()
    })
    .setAvailable((applicationType) => {
      return true;
      // return applicationType === ProtectionDocTypes.Trademark;
    }),
  new Configuration(Priority.ExhibitionPriority, 'Выставочный приоритет')
    .setFields(
      new Field(ControlType.Date, 'regDate', 'Дата приоритета').setRequired()
    )
    .setDefault({
      date: new Date()
    })
    .setAvailable((applicationType) => {
      return applicationType === ProtectionDocTypes.Trademark;
    }),
  new Configuration(Priority.ClaimedDesignationHasEarlierRegistration, 'Заявляемое обозначение имеет более раннюю регистрацию в Казахстане')
    .setFields(
      new Field(ControlType.Input, 'regNumber', 'Номер первой заявки').setRequired(),
      new Field(ControlType.Date, 'regDate', 'Дата испрашиваемого приоритета').setRequired(),
      new Field(ControlType.Select, 'regCountryId', 'Страна').setRequired()
    )
    .setDefault({
      country: 1,
      date: new Date()
    })
    .setAvailable((applicationType) => {
      return applicationType === ProtectionDocTypes.Trademark;
    }),
  new Configuration(Priority.Rule6_2, 'Поступление дополнительных материалов (правило 6 (2) Инструкции)')
    .setFields(
      new Field(ControlType.Input, 'regNumber', '(31) № первой, более ранней, первоначальной заявки ').setRequired(),
      new Field(ControlType.Date, 'regDate', '(32) Дата испрашиваемого приоритета').setRequired(),
      new Field(ControlType.Select, 'regCountryId', '(33) Код страны подачи по ST.3 (при испрашивании конвенционного приоритета)').setRequired()
    )
    .setDefault({
      date: new Date()
    })
    .setAvailable((applicationType) => {
      // Заявка на изобретение (ЕАПО)
      return false;
    }),
  new Configuration(Priority.Rule6_3, 'Подача предшествующей евразийской заявки того же заявителя (правило 6 (3) Инструкции)')
    .setFields(
      new Field(ControlType.Input, 'regNumber', '(31) № первой, более ранней, первоначальной заявки ').setRequired(),
      new Field(ControlType.Date, 'regDate', '(32) Дата испрашиваемого приоритета').setRequired(),
      new Field(ControlType.Select, 'regCountryId', '(33) Код страны подачи по ST.3 (при испрашивании конвенционного приоритета)').setRequired()
    )
    .setDefault({
      date: new Date()
    })
    .setAvailable((applicationType) => {
      // Заявка на изобретение (ЕАПО)
      return false;
    }),
  new Configuration(Priority.Rule6_5, 'Начала открытого показа объекта, содержащего указанное выше изобретение на официальной или официально признанной международной выставке (правило 6 (5) Инструкции)')
    .setFields(
      new Field(ControlType.Input, 'regNumber', '(31) № первой, более ранней, первоначальной заявки ').setRequired(),
      new Field(ControlType.Date, 'regDate', '(32) Дата испрашиваемого приоритета').setRequired(),
      new Field(ControlType.Select, 'regCountryId', '(33) Код страны подачи по ST.3 (при испрашивании конвенционного приоритета)').setRequired()
    )
    .setDefault({
      date: new Date()
    })
    .setAvailable((applicationType) => {
      // Заявка на изобретение (ЕАПО)
      return false;
    }),
  new Configuration(Priority.ApplicationOrCertificateRelationDocumentAdditional, 'Заявка или свидетельство по отношению к которому настоящий документ является дополнительным')
    .setFields(
      new Field(ControlType.Input, 'regNumber', 'Номер').setRequired(),
      new Field(ControlType.Select, 'regCountryId', 'Страна').setRequired(),
      new Field(ControlType.Date, 'regDate', 'Дата').setRequired()
    )
    .setDefault({
      country: 1,
      date: new Date()
    }),
  new Configuration(Priority.EarlierApplicationDocumentAllocated, 'Более ранняя заявка, из которой выделен настоящий документ')
    .setFields(
      new Field(ControlType.Input, 'regNumber', 'Номер').setRequired(),
      new Field(ControlType.Select, 'regCountryId', 'Страна').setRequired(),
      new Field(ControlType.Date, 'regDate', 'Дата').setRequired()
    )
    .setDefault({
      country: 1,
      date: new Date()
    }),
  new Configuration(Priority.EarlierApplicationPatentDocumentContinuation, 'Более ранняя заявка, по отношению к которой настоящий патентный документ является продолжением')
    .setFields(
      new Field(ControlType.Input, 'regNumber', 'Номер').setRequired(),
      new Field(ControlType.Select, 'regCountryId', 'Страна').setRequired(),
      new Field(ControlType.Date, 'regDate', 'Дата').setRequired()
    )
    .setDefault({
      country: 1,
      date: new Date()
    }),
  new Configuration(Priority.PublishedPatentDocumentRelatingApplication, 'Опубликованный патентный документ, касающийся данной заявки')
    .setFields(
      new Field(ControlType.Input, 'regNumber', 'Номер').setRequired(),
      new Field(ControlType.Select, 'regCountryId', 'Страна').setRequired(),
      new Field(ControlType.Date, 'regDate', 'Дата').setRequired()
    )
    .setDefault({
      country: 1,
      date: new Date()
    }),
  new Configuration(Priority.EarlierApplicationReplacementPresentDocument, 'Более ранняя заявка, заменой которой является настоящий документ, то есть, если более поздняя заявка подана после отказа по более ранней заявке на то же самое изобретение')
    .setFields(
      new Field(ControlType.Input, 'regNumber', 'Номер').setRequired(),
      new Field(ControlType.Select, 'regCountryId', 'Страна').setRequired(),
      new Field(ControlType.Date, 'regDate', 'Дата').setRequired()
    )
    .setDefault({
      country: 1,
      date: new Date()
    }),
  new Configuration(Priority.ReceiptApplicationAdmissionStateCommission, 'Поступления заявки на допуск к использованию в госкомиссию (п. 2 ст. 4 Закона)')
    .setFields(
      new Field(ControlType.Input, 'regNumber', '№ первой заявки или заявки на допуск к использованию').setGroup('line_1').setRequired(),
      new Field(ControlType.Date, 'regDate', 'Дата испрашиваемого приоритета').setGroup('line_1').setRequired(),
      new Field(ControlType.Select, 'regCountryId', 'Код страны подачи по ST.3').setGroup('line_1').setRequired(),
      new Field(ControlType.Input, 'stageSD', 'Стадия рассмотрения заявки').setGroup('line_2').setRequired(),
      new Field(ControlType.Input, 'nameSD', ' Под каким названием зарегистрировано селекционное достижение').setGroup('line_2').setRequired()
    )
    .setDefault({
      date: new Date()
    })
    .setAvailable((applicationType) => {
      return applicationType === ProtectionDocTypes.SelectionAchievement;
    }),
  new Configuration(Priority.FilingApplicationCountryInternationalConvention, 'Подачи первой заявки в стране-участнице Международной конвенции (п. 2 ст. 7 Закона)')
    .setFields(
      new Field(ControlType.Input, 'regNumber', '№ первой заявки или заявки на допуск к использованию').setGroup('line_1').setRequired(),
      new Field(ControlType.Date, 'regDate', 'Дата испрашиваемого приоритета').setGroup('line_1').setRequired(),
      new Field(ControlType.Select, 'regCountryId', 'Код страны подачи по ST.3').setGroup('line_1').setRequired(),
      new Field(ControlType.Input, 'stageSD', 'Стадия рассмотрения заявки').setGroup('line_2').setRequired(),
      new Field(ControlType.Input, 'nameSD', ' Под каким названием зарегистрировано селекционное достижение').setGroup('line_2').setRequired()
    )
    .setDefault({
      date: new Date()
    })
    .setAvailable((applicationType) => {
      return applicationType === ProtectionDocTypes.SelectionAchievement;
    }),
  new Configuration(Priority.WhetherOfferedRepublic, 'Предлагался ли сорт (порода) к продаже или продавался в РК')
    .setFields(
      new Field(ControlType.Date, 'regDate', 'Дата').setRequired(),
      new Field(ControlType.Input, 'nameSD', 'Название').setRequired(),
    )
    .setDefault({
      date: new Date()
    })
    .setAvailable((applicationType) => {
      return applicationType === ProtectionDocTypes.SelectionAchievement;
    }),
  new Configuration(Priority.WhetherOfferedOther, 'Предлагался ли сорт (порода) к продаже или продавался в других странах')
    .setFields(
      new Field(ControlType.Date, 'regDate', 'Дата').setRequired(),
      new Field(ControlType.Select, 'regCountryId', 'Страна').setRequired(),
      new Field(ControlType.Input, 'nameSD', 'Название').setRequired(),
    )
    .setDefault({
      date: new Date()
    })
    .setAvailable((applicationType) => {
      return applicationType === ProtectionDocTypes.SelectionAchievement;
    }),
  new Configuration(Priority.InternationalEurasianApplication, 'Международная (Евразийская) Заявка')
    .setFields(
      new Field(ControlType.Input, 'regNumber', 'Номер').setRequired(),
      new Field(ControlType.Select, 'regCountryId', 'Страна').setRequired(),
      new Field(ControlType.Date, 'regDate', 'Дата').setRequired()
    )
    .setDefault({
      country: 1,
      date: new Date()
    })
];
