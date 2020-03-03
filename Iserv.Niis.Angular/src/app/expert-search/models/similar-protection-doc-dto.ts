export class SimilarProtectionDocDto {
    //barcode: number;
    statusNameRu: string;
    previewImage: string;
    gosNumber: string;
    gosDate: Date;
    regNumber: string;
    regDate: Date;
    nameRu: string;
    nameKz: string;
    nameEn: string;
    declarant: string;
    ownerName: string;
    //patentAttorney: string;
    //confidant: string;
    //receiveTypeNameRu: string;
    icgs: string;
    icfems: string;
    colors: string;
    //transliteration: string;
    priorityData: string;
    //numberBulletin: string;
    //publicDate: Date;
    validDate: Date;
    extensionDateTz: Date;
    disclaimerRu: string;
    disclaimerKz: string;
    gosreestr: string;
    //#region 'ExpertSearchSimilar properties'
    id: number;
    expertSearchSimilarId: number;
    imageSimilarity: number;
    phonSimilarity: number;
    semSimilarity: number;
    protectionDocCategory: string;
    //protectionDocFormula: string;
    //#endregion 'ExpertSearchSimilar properties'
}