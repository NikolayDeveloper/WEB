import { OwnerType } from '../../shared/services/models/owner-type.enum';
import { Operators } from '../../shared/filter/operators';
import { BaseExpertSearchDto } from 'app/expert-search/models/base-expert-search-dto.model';

export class InventionDto extends BaseExpertSearchDto {
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

export class InventionSearchDto {
    id: number;
    ownerType: OwnerType;
    name: string;
    regNumber: string;
    regDate: Date;
    status: string;
    gosNumber: string;
    publishDate: Date;
    declarant: string;
    ipc: string;
    referat: string;
}

export const OperatorFor = {
    name: Operators.like,
    formula: Operators.like,
    referat: Operators.like,
    description: Operators.like,
    declarantName: Operators.like,
    declarantCountryId: Operators.equal,
    declarantOblast: Operators.like,
    declarantCity: Operators.like,
    requestStatusIds: Operators.contains,
    protectionDocStatusIds: Operators.contains,
    requestDate: Operators.containsDateRange,
    gosNumber: Operators.like,
    gosDate: Operators.containsDateRange,
    publishDate: Operators.containsDateRange,
    ipcCodes: Operators.contains,
    ipcDescriptions: Operators.like,
    author: Operators.like,
    registerDate: Operators.containsDateRange,
    ownerName: Operators.like
    /*
    searchStatus: Operators.in,
    name: Operators.like,
    requestTypeNameRu: Operators.like,
    ipcs: Operators.like,
    formula: Operators.like,
    referat: Operators.like,
    description: Operators.like,
    priorityRegNumbers: Operators.like,
    priorityRegDate: Operators.containsDateRange,
    priorityRegCountryNames: Operators.like,
    declarant: Operators.like,
    patentOwner: Operators.like,
    author: Operators.like,
    patentAttorney: Operators.like,
    gosNumber: Operators.like,
    publicDate: Operators.containsDateRange,
    */
}
