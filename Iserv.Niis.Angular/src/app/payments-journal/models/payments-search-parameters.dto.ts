import { Moment } from 'moment';
import {
  addBooleanToQueryParams,
  addDateToQueryParams,
  addNumberToQueryParams,
  addStringToQueryParams,
  QueryParam
} from '../helpers/query-param';

export class PaymentsSearchParametersDto {
  public id: number;

  public dateCreateFrom: Moment;

  public dateCreateTo: Moment;

  public payerName: string;

  public payerXin: string;

  public payerRnn: string;

  public paymentDateFrom: Moment;

  public paymentDateTo: Moment;

  public amount: number;

  public remainder: number;

  public distributed: number;

  public paymentPurpose: string;

  public paymentNumber: string;

  public paymentDocumentNumber: string;

  public paymentStatusId: number;

  public isForeignCurrency: boolean;

  public currencyCode: string;

  constructor() {
    this.isForeignCurrency = false;
  }

  public getQueryParams(): QueryParam[] {
    const result = [];

    addNumberToQueryParams('id', this.id, result);
    addDateToQueryParams('dateCreateFrom', this.dateCreateFrom, result);
    addDateToQueryParams('dateCreateTo', this.dateCreateTo, result);
    addStringToQueryParams('payerName', this.payerName, result);
    addStringToQueryParams('payerXin', this.payerXin, result);
    addStringToQueryParams('payerRnn', this.payerRnn, result);
    addDateToQueryParams('paymentDateFrom', this.paymentDateFrom, result);
    addDateToQueryParams('paymentDateTo', this.paymentDateTo, result);
    addNumberToQueryParams('amount', this.amount, result);
    addNumberToQueryParams('remainder', this.remainder, result);
    addNumberToQueryParams('distributed', this.distributed, result);
    addStringToQueryParams('paymentPurpose', this.paymentPurpose, result);
    addStringToQueryParams('paymentNumber', this.paymentNumber, result);
    addStringToQueryParams('paymentDocumentNumber', this.paymentDocumentNumber, result);
    addNumberToQueryParams('paymentStatusId', this.paymentStatusId, result);
    addBooleanToQueryParams('isForeignCurrency', this.isForeignCurrency, result);
    addStringToQueryParams('currencyCode', this.currencyCode, result);

    return result;
  }
}
