import { IServerStatus } from './server-status.interface';

export interface IGenerateNumberResponse {
  /* Номер заявки */
  number: string;
  /* Статус */
  status: IServerStatus;
}
