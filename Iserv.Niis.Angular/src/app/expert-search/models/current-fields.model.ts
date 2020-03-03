import { ExpertSearchFieldEnum } from './expert-search-field.enum';
export interface CurrentFields {
    enum: ExpertSearchFieldEnum;
    canChanged: boolean;
    canDeleted: boolean;
    canSelectFirstOption: boolean;
}
