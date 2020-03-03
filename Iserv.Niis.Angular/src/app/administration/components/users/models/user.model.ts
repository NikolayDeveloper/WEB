
export class UserDetails {
  public id: number;
  public isLocked: boolean;
  public xin: string;
  public nameRu: string;
  // public lastName: string;
  // public patronymic: string;
  public email: string;
  public password: string;
  public divisionId: number;
  public departmentId: number;
  public positionId: number;
  public positionTypeNameRu: string;
  public positionTypeCode: string;
  public customerId: number;
  public roleIds: number[];
  // public objectType: number[];
  public icgsIds: string[];
  public ipcIds: number[];
  public certPassword: string;
  public certStoragePath: string;

  constructor(init?: Partial<UserDetails>) {
    Object.assign(this, init);
  }
}

export class User {
  public id: number;
  public nameRu: string;
  public roleNameRu: string;
  public email: string;
  public divisionNameRu: number;
  public departmentId: number;
  public departmentNameRu: string;
  public positionNameRu: string;
  public positionCode: string;

  constructor(init?: Partial<User>) {
    Object.assign(this, init);
  }
}

