export class ExpertInfo {
    userId: number;
    userName: string;
    ipcCodes: string;
    requestNumbers: string;
    countRequests: number;
    countCompletedRequestsCurrentYear: number;
    employmentIndexExpert: number;
    public constructor(init?: Partial<ExpertInfo>) {
        Object.assign(this, init);
    }
}
