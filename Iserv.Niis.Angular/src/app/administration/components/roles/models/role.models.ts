
export class Role {
  public id: number;
  public nameRu: string;
  public claimsTotal: number;
  public stagesTotal: number;
  public code: number;

  constructor(init?: Partial<Role>) {
    Object.assign(this, init);
  }
}

export class RoleDetails {
  public id: number;
  public nameRu: string;
  public nameKz: number;
  public nameEn: number;
  public code: string;
  public permissions: string[];
  public roleStages: number[];

  constructor(init?: Partial<RoleDetails>) {
    Object.assign(this, init);
  }
}

export interface Permission {
  fieldName: string;
  value: string;
  nameRu: string;
}
