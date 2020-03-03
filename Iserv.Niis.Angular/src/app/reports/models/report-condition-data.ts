import { Moment } from "moment";

export class ReportConditionData {
    pageNumber: number;
    itemsCountPerPage: number;
    reportCode: string;
    dateFrom: Moment;
    dateTo: Moment;
    protectionDocTypeIds: number[] = [];
}
