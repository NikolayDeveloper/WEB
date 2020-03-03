import { moment } from '../../shared/shared.module';
import { Moment } from 'moment';

export function fromJsonStringToDate(value: string) {
  return moment(value).utc().add(moment(value).utcOffset(), 'm').toDate();
}

export function fromDateToJsonString(value: Date | Moment) {
  return moment(value).add(moment(value).utcOffset(), 'minutes').toISOString();
}

export function toShortDateString(value: Date) {
  return value ? moment(value).format('DD.MM.YYYY') : '';
}

export function toFullDateString(value: Date) {
  return value ? moment(value).format('DD.MM.YYYY HH:mm') : '';
}

export function FromUTCDateToFullDateString(value: Date) {
  return value ? moment(value).utc().format('DD.MM.YYYY HH:mm') : '';
}

export function FromUTCDateToShortDateString(value: Date) {
  return value ? moment(value).utc().format('DD.MM.YYYY') : '';
}
