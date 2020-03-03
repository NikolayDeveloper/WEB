import { DictionaryType } from './dictionary-type.enum';

export interface SelectOption {
    dicType: DictionaryType;
    id: number;
    code: string;
    nameRu: string;
}

export interface SimpleSelectOption {
    id: number;
    nameRu: string;
}
