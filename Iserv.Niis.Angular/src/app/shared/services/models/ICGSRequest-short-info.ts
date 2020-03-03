export class ICGSRequestsShortInfo {
    constructor(
        public id: number,
        public icgsId: number,
        public icgsNameRu: string,
        public icgsNameKz: string,
        public icgsNameEn: string,
        public requestId: number,
        public description: string
    ) { }
}
