import { DocumentType } from "../../materials/models/materials.model";

export enum DocumentKind {
  None,
  Request,
  ProtectionDoc,
  Contract,
  IncomingMaterial,
  OutgoingMaterial,
  Internalmaterial,
  DocumentRequest
}

export function getDocumentKindByType(type: DocumentType) {
  switch (type) {
    case DocumentType.Incoming:
      return DocumentKind.IncomingMaterial;
    case DocumentType.Outgoing:
      return DocumentKind.OutgoingMaterial;
    case DocumentType.Internal:
      return DocumentKind.Internalmaterial;
    case DocumentType.DocumentRequest:
      return DocumentKind.DocumentRequest;
    case DocumentType.Request:
      return DocumentKind.Request;
    case DocumentType.Contract:
      return DocumentKind.Contract;
    default:
      return DocumentKind.None;
  }
}
