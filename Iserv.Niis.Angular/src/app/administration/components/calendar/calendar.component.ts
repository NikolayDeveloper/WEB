import { ChangeDetectionStrategy, Component, OnInit, ViewEncapsulation, OnDestroy } from '@angular/core';
import { CalendarEvent, CalendarMonthViewDay } from 'angular-calendar';
import { SnackBarHelper } from 'app/core/snack-bar-helper.service';
import { Subject } from 'rxjs/Subject';

import { CalendarService } from '../../calendar.service';
import { EventType, EventTypeEnum, Period } from './models/calendar.models';

@Component({
  selector: 'app-calendar',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CalendarComponent implements OnInit, OnDestroy {
  private onDestroy = new Subject();
  mainTitle = 'Календарь';
  events: CalendarEvent[] = [];
  selectedDays: CalendarMonthViewDay[] = [];
  eventTypeColors: EventType[];
  refresh = new Subject<any>();
  today: Date = new Date();
  period = new Period();
  dayoffCount: number;
  holidayCount: number;
  publicationCount: number;

  constructor(
    private calendarService: CalendarService,
    private snackBarHelper: SnackBarHelper) { }

  ngOnInit() {
    this.calendarService
      .getDateNow()
      .takeUntil(this.onDestroy)
      .map(dateNowString => new Date(dateNowString))
      .subscribe(dateNow => {
        this.today = dateNow;
      });
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  onAddEvents(color: string, title: string): void {
    this.selectedDays.forEach(selectedDay => {
      this.createOrUpdateEvent(selectedDay.date, color, title);
    });
    this.selectedDays = [];
    this.saveEvents();
  }

  onRemoveEvents() {
    const removingEventIds: number[] = this.getRemovingEventIds();
    removingEventIds.forEach(id => {
      this.calendarService
        .removeEvent(id)
        .takeUntil(this.onDestroy)
        .subscribe(() => {
          this.fetchEvents(this.period);
        });
      this.snackBarHelper.success('Events removed');
    });
  }

  onFillEvents() {
    const year = this.period.from.getFullYear();
    const monthStart = this.period.from.getMonth();
    const monthEnd = this.period.to.getMonth();

    this.fillDayOffEvents(year, monthStart, monthEnd);
    this.fillHolidayEvents(year - 1, year, monthStart, monthEnd);
  }

  fetchEvents(period: Period) {
    this.calendarService
      .getEvents(period.from, period.to)
      .takeUntil(this.onDestroy)
      .subscribe(events => {
        this.events = events;
        this.setCountOfEvent();
        this.refresh.next();
      });
  }

  onPeriodChange(period: Period) {
    this.period = period;
    this.fetchEvents(period);
  }

  fillDayOffEvents(year: number, monthStart: number, monthEnd: number) {
    for (let month = monthStart; month <= monthEnd; month++) {
      let dayStart = 1;
      const dayEnd = new Date(year, month + 1, 0).getDate();
      if (new Date(year, month, 1) < new Date(this.today.getFullYear(), this.today.getMonth(), 1)) {
        continue;
      } else {
        dayStart = this.today.getDate();
      }

      for (let day = dayStart; day <= dayEnd; day++) {
        const newDate = new Date(year, month, day);
        const weekDay = newDate.getDay();
        if (newDate.getDay() === 6 || newDate.getDay() === 0) {
          const newColor = this.calendarService.getEventColor(EventTypeEnum.Dayoff);
          this.createOrUpdateEvent(newDate, newColor, 'Выходной');
        }
      }
    }
  }

  fillHolidayEvents(getFromYear: number, year: number, monthStart: number, monthEnd: number) {
    this.calendarService
      .getByEventType(
        new Date(getFromYear, monthStart, 1),
        new Date(getFromYear, monthEnd + 1, 0),
        EventTypeEnum.Holiday)
      .takeUntil(this.onDestroy)
      .subscribe(data => {
        data.forEach(event => {
          event.start.setFullYear(year);
          this.createOrUpdateEvent(event.start, event.color.primary, 'Праздничный');
        });
        this.saveEvents();
      });
  }

  createOrUpdateEvent(checkDate: Date, color: string, title: string) {
    const existEventOnDay = this.events.find(findIndexOfDay);
    if (existEventOnDay) {
      existEventOnDay.color = {
        primary: color,
        secondary: color
      };
      existEventOnDay.title = title;
    } else {
      this.events.push({
        start: checkDate,
        title: title,
        color: {
          primary: color,
          secondary: color
        }
      });
    }

    function findIndexOfDay(element, index, array) {
      return element.start.toString() === checkDate.toString();
    }
  }

  saveEvents() {
    this.calendarService
      .addEvents(this.events)
      .takeUntil(this.onDestroy)
      .subscribe(events => {
        this.snackBarHelper.success('Events saved');
        this.events = this.calendarService.mapEvents(events);
        this.setCountOfEvent();
        this.refresh.next();
      });
  }

  getRemovingEventIds(): number[] {
    const removingEventIds: number[] = [];
    this.selectedDays.forEach(selectedDay => {
      const existEventOnDayIndex = this.events.findIndex(findIndexOfDay);
      if (existEventOnDayIndex > -1) {
        removingEventIds.push(this.events[existEventOnDayIndex].meta);
        this.events.slice(existEventOnDayIndex);
      }

      function findIndexOfDay(element, index, array) {
        return element.start.toString() === selectedDay.date.toString();
      }
    });
    this.selectedDays = [];
    return removingEventIds;
  }

  private setCountOfEvent() {
    if (!this.events || !this.events.length) {
      this.dayoffCount = this.holidayCount = this.publicationCount = 0;
      return;
    }

    this.dayoffCount = this.events.filter(e => e.color.primary === 'darkred').length;
    this.holidayCount = this.events.filter(e => e.color.primary === 'red').length;
    this.publicationCount = this.events.filter(e => e.color.primary === 'green').length;
  }
}

