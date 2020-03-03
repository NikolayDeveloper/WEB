import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges,
  ViewEncapsulation,
} from '@angular/core';
import { CalendarDateFormatter, CalendarEvent, CalendarMonthViewDay, DAYS_OF_WEEK } from 'angular-calendar';
import { CustomDateFormatter } from 'app/administration/components/calendar/custom-date-formatter.provider';
import { Subject } from 'rxjs/Subject';

import { Period } from '../../../components/calendar/models/calendar.models';

@Component({
  selector: 'app-calendar-month',
  templateUrl: './calendar-month.component.html',
  styleUrls: ['../calendar.component.scss', './calendar-month.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None,
  providers: [
    {
      provide: CalendarDateFormatter,
      useClass: CustomDateFormatter
    }
  ]
})
export class CalendarMonthComponent implements OnInit, OnChanges {
  @Input() events: CalendarEvent[];
  @Input() selectedDays: CalendarMonthViewDay[];
  @Input() refresh: Subject<any>;
  @Input() today: Date;
  @Output() changeTitle = new EventEmitter();
  @Output() periodChange = new EventEmitter<Period>();

  viewDate: Date;
  locale = 'en';
  formatter = new CustomDateFormatter();
  weekStartsOn: number = DAYS_OF_WEEK.MONDAY;
  weekendDays: number[] = [];
  clickedDate: Date;

  constructor() { }

  ngOnInit() { }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.today && changes.today.currentValue) {
      this.viewDate = this.today;
      this.onViewDateChange();
    }
  }

  onViewDateChange() {
    const period = new Period({
      from: new Date(this.viewDate.getFullYear(), this.viewDate.getMonth(), 1),
      to: new Date(this.viewDate.getFullYear(), this.viewDate.getMonth() + 1, 0)
    });
    this.periodChange.emit(period);
    this.changeTitle.emit(this.formatter.monthViewTitle({ date: this.viewDate, locale: this.locale }));
  }

  onDayClicked(day: CalendarMonthViewDay): void {
    const existSelectedDayIndex = this.selectedDays.findIndex(findIndexOfDay);
    if (existSelectedDayIndex > -1) {
      delete this.selectedDays[existSelectedDayIndex].cssClass;
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
            selectedDay && day.date.getTime() === selectedDay.date.getTime()
          ) {
            day.cssClass = 'cal-day-selected';
          }
        });
      }
    });
  }

  dateIsValid(date: Date): boolean {
    date.setHours(0, 0, 0, 0);
    const today = this.today;
    today.setHours(0, 0, 0, 0);
    return date >= today;
  }
}
