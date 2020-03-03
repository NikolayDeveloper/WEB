import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import {
  HttpClient,
  HttpParams
} from '@angular/common/http';

import { DictionaryService } from '../shared/services/dictionary.service';
import { DictionaryType } from '../shared/services/models/dictionary-type.enum';
import { EventType, EventTypeEnum, Event } from './components/calendar/models/calendar.models';
import { SelectOption } from '../shared/services/models/select-option';
import { CalendarEvent } from 'angular-calendar';
import { ConfigService } from 'app/core';
import { ErrorHandlerService } from '../core/error-handler.service';

@Injectable()
export class CalendarService {
  private readonly api: string = '/api/calendar/';
  private readonly apiUrl: string;
  eventTypeColors: EventType[] = [];
  constructor(
    private http: HttpClient,
    private dictionaryService: DictionaryService,
    private errorHandlerService: ErrorHandlerService,
    configService: ConfigService) {
    this.apiUrl = `${configService.apiUrl}${this.api}`;
  }

  getEventTypes(): Observable<EventType[]> {
    return this.dictionaryService
      .getSelectOptions(DictionaryType.DicEventType)
      .map(selectOptions => selectOptions.map(so => new EventType(so.id, so.code, this.getEventColor(so.id), so.nameRu)));
  }

  getDateNow(): Observable<any> {
    return this.http
      .get(`${this.apiUrl}getDateNow`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err));
  }

  getEvents(fromDate: Date, toDate: Date): Observable<CalendarEvent[]> {
    return this.http
      .get(`${this.apiUrl}${fromDate.toDateString()}/${toDate.toDateString()}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map(opts => this.mapEvents(opts));
  }

  getByEventType(fromDate: Date, toDate: Date, eventType: EventTypeEnum): Observable<CalendarEvent[]> {
    return this.http
      .get(`${this.apiUrl}GetByEventType/${fromDate.toDateString()}/${toDate.toDateString()}/${eventType}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map(opts => this.mapEvents(opts));
  }

  addEvents(calendarEvents: CalendarEvent[]): Observable<any> {
    const events = calendarEvents.map(e => new Event(e.meta, e.start, this.getEventTypeId(e.color.primary)));

    return this.http
      .post(this.apiUrl, events)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err));
  }

  removeEvent(calendarEventId: number): Observable<any> {
    return this.http
      .delete(`${this.apiUrl}/${calendarEventId}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err));
  }

  mapEvents(opts: any): CalendarEvent[] {
    return opts.map((event: Event) => <CalendarEvent>{
      start: new Date(event.date),
      title: '',
      color: {
        primary: this.getEventColor(event.eventTypeId),
        secondary: this.getEventColor(event.eventTypeId)
      },
      meta: event.id
    });
  }

  getEventColor(eventName: EventTypeEnum): string {
    switch (eventName) {
      case EventTypeEnum.Holiday: {
        return 'red';
      }
      case EventTypeEnum.Dayoff: {
        return 'darkred';
      }
      case EventTypeEnum.Publication: {
        return 'green';
      }
      default: {
        return 'blue';
      }
    }
  }

  private getEventTypeId(eventTypeColor: string): number {
    switch (eventTypeColor) {
      case 'red': {
        return EventTypeEnum.Holiday;
      }
      case 'darkred': {
        return EventTypeEnum.Dayoff;
      }
      case 'green': {
        return EventTypeEnum.Publication;
      }
      case 'blue': {
        return EventTypeEnum.Unknown;
      }
    }
  }
}
