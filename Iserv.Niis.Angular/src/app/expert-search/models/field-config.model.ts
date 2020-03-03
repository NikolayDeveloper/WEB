import { ExpertSearchFieldEnum } from './expert-search-field.enum';
import { BaseDictionary } from 'app/shared/services/models/base-dictionary';
export interface FieldConfig {
    enum: ExpertSearchFieldEnum;
    name: string;
    label: string;
}
