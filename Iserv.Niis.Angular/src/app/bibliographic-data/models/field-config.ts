export class FieldConfig {
  stageCodes: string[];
  fieldName: string;
}

export class BiblioFieldConfig {
  protectionDocTypeCodes: string[];
  type: FieldType;
}

export enum FieldType {
  Icis,
  Priority,
  Ipc,
  Icfem,
  Icgs,
  Selection,
  Product,
  Trademark,
  Color,
  Common,
  Referat,
  Image,
  Media
}

export class PriorityFieldConfig {
  protectionDocTypeCodes: string[];
  type: PriorityFieldType;
}

export enum PriorityFieldType {
  First,
  International,
  FirstCommonLabel,
  FirstTrademarkLabel,
  Earlier,
  Initial,
  Additional,
  Commission,
  Pct,
  Common,
  Special
}

export class ReferatFieldConfig {
  protectionDocTypeCodes: string[];
  type: ReferatFieldtype;
}

export enum ReferatFieldtype {
  Common,
  IndustrialDesign
}

export abstract class BiblioField {
  // костыль для геттера value
  abstract getValue();
}
