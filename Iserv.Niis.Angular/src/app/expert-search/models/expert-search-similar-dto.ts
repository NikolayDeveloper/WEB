import { OwnerType } from '../../shared/services/models/owner-type.enum';
export class ExpertSearchSimilarDto {
  id: number;
  dateCreate: string;
  requestId: number;
  ownerType: OwnerType;
  similarRequestId: number;
  similarProtectionDocId: number;
  imageSimilarity: string;
  phonSimilarity: string;
  semSimilarity: string;

  constructor(init?: Partial<ExpertSearchSimilarDto>) {
    Object.assign(this, init);
  }
  protectionDocFormula: string;
  protectionDocCategory: string;
}



