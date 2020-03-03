import { getDocumentTypeRoute, DocumentType } from '../../../materials/models/materials.model';

/**
 * Тип сущности обладателя
 *
 * @export
 * @enum {number}
 */
export enum OwnerType {
    None,
    /// <summary>
    /// Заявка
    /// </summary>
    Request,
    /// <summary>
    /// Охранный документ
    /// </summary>
    ProtectionDoc,
    /// <summary>
    /// Договор коммерциализации
    /// </summary>
    Contract,
    /// <summary>
    ///     Материал
    /// </summary>
    Material
}

export function getModuleName(ownerType: OwnerType, documentType?: DocumentType): string {
    switch (ownerType) {
        case OwnerType.Request:
            return 'requests';
        case OwnerType.ProtectionDoc:
            return 'protectiondocs';
        case OwnerType.Contract:
            return 'contracts';
        case OwnerType.Material:
            return getDocumentTypeRoute(documentType);
        default:
            throw Error(`Unknown owner type: ${ownerType}`);
    }
}
