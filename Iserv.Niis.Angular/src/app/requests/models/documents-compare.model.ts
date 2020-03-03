export class DocumentsInfoForCompare {
    description: string;
    changedDescriptionDocId: number;
    changedDescription: string;
    essay: string;
    changedEssayDocId: number;
    changedEssay: string;
    formula: string;
    changedFormulaDocId: number;
    changedFormula: string;
    public constructor(init?: Partial<DocumentsInfoForCompare>) {
        Object.assign(this, init);
    }
}
