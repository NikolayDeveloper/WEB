import { ProtectionDocSearchStatus } from './protectiondoc-search-status.enum';
import { OwnerType } from '../../shared/services/models/owner-type.enum';
import { Operators } from '../../shared/filter/operators';
import { BaseExpertSearchDto } from './base-expert-search-dto.model';

export class UsefulmodelDto extends BaseExpertSearchDto {
    gosNumber: string;
    patentOwner: string;
    ipcs: string;
    author: string;
    earlyTerminationDate: Date;
    transferDate: Date;
    formula: string;
    referat: string;
    description: string;
}

export const OperatorFor = {
    searchStatus: Operators.in,
    name: Operators.like,
    requestTypeNameRu: Operators.like,
    icis: Operators.contains,
    referat: Operators.like,
    priorityRegNumbers: Operators.like,
    priorityRegDate: Operators.containsDateRange,
    priorityRegCountryNames: Operators.like,
    declarantName: Operators.like,
    patentOwner: Operators.like,
    author: Operators.like,
    patentAttorney: Operators.like,
    gosNumber: Operators.like,
    publicDate: Operators.containsDateRange,
    publishDate: Operators.containsDateRange,
    formula: Operators.like,
    description: Operators.like,
    ipcCodes: Operators.contains,
    ipcDescriptions: Operators.like
};
