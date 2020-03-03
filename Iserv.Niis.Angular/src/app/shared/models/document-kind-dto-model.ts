import { DocumentKind, getDocumentKindByType } from "./create-document-model";
import { DocumentType, getDocumentTypeName, getDocumentTypeRoute } from "../../materials/models/materials.model";

export class DocumentKindDto {
    public kind: DocumentKind;
    public displayValue: string;
    public path: string;

    constructor (type: DocumentType) {
        this.displayValue = getDocumentTypeName(type);
        this.path = getDocumentTypeRoute(type);
        this.kind = getDocumentKindByType(type);
    }
}