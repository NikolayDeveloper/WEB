import { Moment } from 'moment';
import { fromDateToJsonString } from './date-helpers';

export function addNumberToQueryParams(key: string, value: number, queryParams: QueryParam[]): void {
  if (value != null) {
    queryParams.push({ key: key, value: value.toString() });
  }
}

export function addDateToQueryParams(key: string, value: Moment, queryParams: QueryParam[]): void {
  if (value != null) {
    queryParams.push({ key: key, value: fromDateToJsonString(value) });
  }
}

export function addStringToQueryParams(key: string, value: string, queryParams: QueryParam[]): void {
  if (value) {
    queryParams.push({ key: key, value: value });
  }
}

export function addBooleanToQueryParams(key: string, value: boolean, queryParams: QueryParam[]): void {
  if (value != null) {
    queryParams.push({ key: key, value: value.toString() });
  }
}

export class QueryParam {
  public key: string;

  public value: string;
}
