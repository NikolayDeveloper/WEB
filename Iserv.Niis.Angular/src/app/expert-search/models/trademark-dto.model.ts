import { OwnerType } from '../../shared/services/models/owner-type.enum';
import { Operators } from '../../shared/filter/operators';
import { BaseExpertSearchDto } from 'app/expert-search/models/base-expert-search-dto.model';

export class TrademarkDto extends BaseExpertSearchDto {
  previewImage: string;
  gosNumber: string;
  owner: string;
  icgs: string;
  icfems: string;
  transliteration: string;
  extensionDateTz: Date;
  disclaimerRu: string;
  disclaimerKz: string;
}

export class TrademarkSearchDto {
  id: number;
  ownerType: OwnerType;
  previewImage: string;
  regNumber: string;
  regDate: Date;
  gosNumber: string;
  gosDate: Date;
  declarantName: string;
  ownerName: string;
  icgs: string;
  disclamation: string;
  validDate: Date;
  colors: string;
  icfem: string;
}

export const OperatorFor = {
  searchStatus: Operators.in,
  name: Operators.like,
  transliteration: Operators.like,
  icgs: Operators.like,
  icfems: Operators.like,
  priorityRegNumbers: Operators.like,
  priorityRegDate: Operators.containsDateRange,
  priorityRegCountryNames: Operators.like,
  declarant: Operators.like,
  owner: Operators.like,
  requestTypeNameRu: Operators.like,
  requestStatusIds: Operators.contains,
  protectionDocStatusIds: Operators.contains,
  icgsDescriptions: Operators.like,
  icgsIds: Operators.contains,
  icfemIds: Operators.contains,
  trademarkTypeId: Operators.equal,
  trademarkKindId: Operators.equal,
  ownerCountryId: Operators.equal,
  declarantCountryId: Operators.equal,
  publishDate: Operators.containsDateRange,
  requestDate: Operators.containsDateRange,
  gosDate: Operators.containsDateRange,
  requestNumber: Operators.like,
  gosNumber: Operators.like,
  ownerName: Operators.like,
  ownerCity: Operators.like,
  ownerOblast: Operators.like,
  declarantName: Operators.like,
  declarantCity: Operators.like,
  declarantOblast: Operators.like,
  patentAttorneyName: Operators.like,
  patentAttorneyNumber: Operators.like
};
