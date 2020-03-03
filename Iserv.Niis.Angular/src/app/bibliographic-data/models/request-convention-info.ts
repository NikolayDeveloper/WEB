export class RequestConventionInfo {
    id: number;
    requestId: number;
    countryId: number;
    earlyRegTypeId: number;
    internationalAppToNationalPhaseTransferDate: Date;
    dateInternationalApp: Date;
    regNumberInternationalApp: string;
    publishDateInternationalApp: Date;
    publishRegNumberInternationalApp: string;
    dateEurasianApp: Date;
    regNumberEurasianApp: string;
    publishDateEurasianApp: Date;
    publishRegNumberEurasianApp: string;
    headIps: string;
    termNationalPhaseFirsChapter: Date;
    termNationalPhaseSecondChapter: Date;
    public constructor(init?: Partial<RequestConventionInfo>) {
        Object.assign(this, init);
    }
}
