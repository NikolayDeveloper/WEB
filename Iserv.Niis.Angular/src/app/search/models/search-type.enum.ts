import { SimpleSelectOption } from '../../shared/services/models/select-option';

export enum SearchType {
    None,
    Simple,
    AdvancedRequest,
    AdvancedProtectionDoc,
    AdvancedContract,
    AdvancedDocument,
    ExpertTrademark,
    ExpertInvention,
    ExpertUsefulmodel,
    ExpertIndustrialdesign,
    ExpertImage
}

export const SearchTypeSelectOptions: SimpleSelectOption[] = [
    { id: SearchType.AdvancedRequest, nameRu: getName(SearchType.AdvancedRequest) },
    { id: SearchType.AdvancedProtectionDoc, nameRu: getName(SearchType.AdvancedProtectionDoc) },
    { id: SearchType.AdvancedContract, nameRu: getName(SearchType.AdvancedContract) },
    { id: SearchType.AdvancedDocument, nameRu: getName(SearchType.AdvancedDocument) },
];

function getName(type: SearchType): string {
    switch (type) {
        case SearchType.AdvancedRequest:
            return 'Заявки на ОД';
        case SearchType.AdvancedProtectionDoc:
            return 'Охранные документы';
        case SearchType.AdvancedContract:
            return 'Договоры';
        case SearchType.AdvancedDocument:
            return 'Материалы';
        default:
            throw Error(`Unknown search type: ${type}`);
    }
}