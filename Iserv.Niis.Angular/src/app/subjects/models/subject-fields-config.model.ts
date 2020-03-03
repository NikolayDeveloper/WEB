import { CustomerRole } from '../enums/customer-role.enum';
import { SubjectSpecialFields } from '../enums/subject-special-fields.enum';

export class SubjectFieldsConfig {
    roleCodes: CustomerRole[];
    specialField: SubjectSpecialFields;
}
