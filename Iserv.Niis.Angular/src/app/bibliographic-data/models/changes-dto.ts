import { OwnerType } from 'app/shared/services/models/owner-type.enum';
import { IntellectualPropertyDetails } from 'app/shared/models/intellectual-property-details';
import { SubjectDto } from 'app/subjects/models/subject.model';
import { ChangeDetails } from '../components/changes/confirm-changes/confirm-changes.component';

export class ChangesDto {
  id: number;
  name: string;
  ownerType: OwnerType;
  oldValue: string;
  newValue: string;
  changeType: ChangeType;
}

export enum ChangeType {
  DeclarantName = 0,
  DeclarantAddress = 1,
  AddresseeAddress = 2,
  DeclarantNameEn = 3,
  DeclarantAddressEn = 4,
  AddresseeAddressEn = 5,
  DeclarantNameKz = 6,
  DeclarantAddressKz = 7,
  AddresseeAddressKz = 8,
  Image = 9,
  Icgs = 10,
  Everything = 11,
  PatentAttorney = 12,
  Addressee = 13,
  Declarant = 14
}

export class ChangeTypeOption {
  id: number;
  code: string;
  nameRu: string;
  types: ChangeType[];
}

export abstract class ChangesComponent {
  abstract getValue(): ChangesDto;
}

export abstract class ChangeTypeChooserComponent {
  abstract getValue(): ChangeTypeOption[];
}

export abstract class ChangesContainerComponent {
  abstract getValue(): ChangesDto[];

  abstract getDetails(): IntellectualPropertyDetails;

  abstract getSubjectsToAttach(): SubjectDto[];

  abstract getSubjectsToEdit(): SubjectDto[];

  abstract getSubjectsToDelete(): SubjectDto[];

  abstract isValid(): boolean;
}

export abstract class SubjectChangesComponent {
  abstract getValue(): SubjectDto;
}

export abstract class ConfirmChangeComponent {
  abstract onSubmit(): ChangeDetails;
}
