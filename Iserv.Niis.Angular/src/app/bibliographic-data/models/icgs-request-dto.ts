export class ICGSRequestDto {
    id: number;
    icgsId: number;
    icgsName: string;
    claimedDescription: string;
    claimedDescriptionEn: string;
    description: string;
    descriptionKz: string;
    descriptionNew: string;
    negativeDescription: string;
    regNumberInternationalApp: string;
    isRefused: boolean;
    isPartialRefused: boolean;
    isSplit: boolean;
    reasonForPartialRefused: string;
    public constructor(init?: Partial<ICGSRequestDto>) {
        Object.assign(this, init);
    }
}
