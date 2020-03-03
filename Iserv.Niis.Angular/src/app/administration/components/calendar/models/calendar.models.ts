import { CalendarEvent } from 'angular-calendar';

export class EventType {
    constructor(
        public id: number,
        public type: string,
        public color: string,
        public nameRu: string) { }
}


export class Event {
    eventType: EventType;
    constructor(
        public id: number,
        public date: Date,
        public eventTypeId: number) { }
}

export enum EventTypeEnum {
    'Publication' = 4,
    'Holiday' = 5,
    'Dayoff' = 6,
    'Unknown'
}

export class Period {
    public from: Date;
    public to: Date;

    constructor(init?: Partial<Period>) {
        Object.assign(this, init);
    }
}
