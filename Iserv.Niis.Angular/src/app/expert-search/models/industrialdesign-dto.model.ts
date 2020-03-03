import { ProtectionDocSearchStatus } from './protectiondoc-search-status.enum';
import { OwnerType } from '../../shared/services/models/owner-type.enum';
import { Operators } from '../../shared/filter/operators';
import { BaseExpertSearchDto } from './base-expert-search-dto.model';

export class IndustrialdesignDto extends BaseExpertSearchDto {
    previewImage: string;
    gosNumber: string;
    patentOwner: string;
    ipcs: string;
    author: string;
    referat: string;
    earlyTerminationDate: Date;
}

export const OperatorFor = {
    searchStatus: Operators.in,
    name: Operators.like,
    requestTypeNameRu: Operators.like,
    icis: Operators.contains,
    formula: Operators.like,
    referat: Operators.like,
    description: Operators.like,
    priorityRegNumbers: Operators.like,
    priorityRegDate: Operators.containsDateRange,
    priorityRegCountryNames: Operators.like,
    declarantName: Operators.like,
    patentOwner: Operators.like,
    author: Operators.like,
    patentAttorney: Operators.like,
    gosNumber: Operators.like,
    publicDate: Operators.containsDateRange,
    priorityRegDateTo: Operators.containsDateRange,
    priorityDates: Operators.containsDateRange,
    publishDate: Operators.containsDateRange
};
