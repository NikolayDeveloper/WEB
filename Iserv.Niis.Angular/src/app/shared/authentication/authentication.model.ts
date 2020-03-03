export class LoginModel {
  public userName: string;
  public password: string;
  // Поля для входа через сертификат
  public certData: string;
  public isCertificate: boolean;
  public plainData: string;
  public signedPlainData: string;
  constructor(init?: Partial<LoginModel>) {
    Object.assign(this, init);
  }
}

export class AccessData {
  public access_token: string;
  public profile: Profile;
}

export class Profile {
  public id: number;
  public email: string;
  public name: string;
  public prm: string[];
  public roles: string[];
}
