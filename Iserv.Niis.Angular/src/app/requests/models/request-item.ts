export class RequestItemDto {
    public constructor(
        public id: number,
        public incomingNumber: string,
        public requestNum: string,
        public protectionDocTypeName: string,
        public protectionDocTypeCode: string,
        public dateCreate: Date
    ) { }
}
