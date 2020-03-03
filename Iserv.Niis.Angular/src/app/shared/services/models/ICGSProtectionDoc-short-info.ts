export class ICGSProtectionDocItemDto {
    constructor(
        public id: number,
        public icgsId: number,
        public icgsNameRu: string,
        public icgsNameKz: string,
        public icgsNameEn: string,
        public protectionDocId: number,
        public description: string
    ) { }
}
