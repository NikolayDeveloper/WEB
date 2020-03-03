import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  ViewEncapsulation,
  Input,
  Output,
  EventEmitter,
  OnChanges,
  SimpleChanges
} from '@angular/core';
import {
  CalendarEvent,
  DAYS_OF_WEEK,
  CalendarDateFormatter,
  CalendarMonthViewDay
} from 'angular-calendar';
import { CustomDateFormatter } from '../custom-date-formatter.provider';
import { Subject } from 'rxjs/Subject';
import { Period } from '../../../components/calendar/models/calendar.models';

@Component({
  selector: 'app-calendar-year',
  templateUrl: './calendar-year.component.html',
  styleUrls: ['../calendar.component.scss', './calendar-year.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None,
  providers: [
    {
      provide: CalendarDateFormatter,
      useClass: CustomDateFormatter
    }
  ]
})

export class CalendarYearComponent implements OnInit, OnChanges {
  @Input() events: CalendarEvent[];
  @Input() selectedDays: CalendarMonthViewDay[];
  @Input() refresh: Subject<any>;
  @Input() today: Date;
  @Output() changeTitle = new EventEmitter<string>();
  @Output() periodChange = new EventEmitter<Period>();

  year: number;
  locale = 'en';
  weekStartsOn: number = DAYS_OF_WEEK.MONDAY;
  weekendDays: number[] = [];
  clickedDate: Date;

  constructor() { }

  ngOnInit() { }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.today && changes.today.currentValue) {
      this.year = this.today.getFullYear();
      this.onViewYearChange();
    }
  }

  onChangeYear(addToYear: number) {
    if (addToYear === 0) {
      this.year = this.today.getFullYear();
    }
    this.year += addToYear;
    this.onViewYearChange();
  }

  onDayClicked(day: CalendarMonthViewDay): void {
    const existSelectedDayIndex = this.selectedDays.findIndex(findIndexOfDay);
    if (existSelectedDayIndex > -1) {
      this.selectedDays.splice(existSelectedDayIndex, 1);
    } else {
      day.cssClass = 'cal-day-selected';
      this.selectedDays.push(day);
    }

    function findIndexOfDay(element, index, array) {
      return element.date.toString() === day.date.toString();
    }
  }

  onBeforeMonthViewRender({ body }: { body: CalendarMonthViewDay[] }): void {
    body.forEach(day => {
      if (!this.dateIsValid(day.date)) {
        day.cssClass = 'cal-disabled';
      }
      if (day.cssClass === undefined) {
        this.selectedDays.forEach(selectedDay => {
          if (
            selectedDay &&
            day.date.getTime() === selectedDay.date.getTime()
          ) {
            day.cssClass = 'cal-day-selected';
          }
        });
      }
    });
  }

  onViewYearChange() {
    const period = new Period({
      from: new Date(this.year, 0, 1),
      to: new Date(this.year, 11, 31)
    });

    this.periodChange.emit(period);
    this.changeTitle.emit(`${this.year} год`);
  }

  viewDate(number): Date {
    return this.getDateForMonth(number);
  }

  getDateForMonth(month: number): Date {
    const rightMonth = Number(month);
    const newDate = new Date(this.year, rightMonth);
    return newDate;
  }

  dateIsValid(date: Date): boolean {
    date.setHours(0, 0, 0, 0);
    const today = this.today;
    today.setHours(0, 0, 0, 0);
    return date >= today;
  }
}
