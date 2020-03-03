export class RouteStageOrder {
    currentStageId: number;
    nextStageId: number;
    isAutomatic: boolean;
    isParallel: boolean;
    isReturn: boolean;

    public constructor(init?: Partial<RouteStageOrder>) {
        Object.assign(this, init);
    }
}
