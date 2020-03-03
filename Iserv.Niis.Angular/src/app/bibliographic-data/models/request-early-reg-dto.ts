export class RequestEarlyRegDto {
    id: number;
    earlyRegTypeId: number;
    regCountryId: number;
    countryNameRu: string;
    regNumber: string;
    regDate: Date;
    nameSD: string;
    stageSD: string;
    public constructor(init?: Partial<RequestEarlyRegDto>) {
        Object.assign(this, init);
    }
}
