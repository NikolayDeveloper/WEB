import { StatusCodes } from '../enums/status-codes.enum';

export interface IServerStatus {
  /* Сообщение */
  message: string;
  /* Код */
  code: StatusCodes;
}
