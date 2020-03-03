import { OwnerType } from 'app/shared/services/models/owner-type.enum';

export class ConvertDto {
  ownerId: number;
  ownerType: OwnerType;
  colectiveTrademarkParticipantsInfo: string;
}

export class ConvertResponseDto {
  colectiveTrademarkParticipantsInfo: string;
  speciesTradeMarkId: number;
  speciesTrademarkCode: string;
}
