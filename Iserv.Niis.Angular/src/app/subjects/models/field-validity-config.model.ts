import { CustomerRole } from '../enums/customer-role.enum';

export class FieldValidityConfig {
    roleCodes: CustomerRole[];
    fieldName: string;
    isRequired: boolean;
}
