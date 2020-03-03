import { BaseDictionary } from 'app/shared/services/models/base-dictionary';

export class SubjectDto {
  id = 0;
  customerId: number;
  ownerId: number;
  roleId: number;
  roleCode: string;
  roleNameRu: string;
  countryId: number;
  nameRu: string;
  nameEn: string;
  nameKz: string;
  typeId: number;
  typeNameRu: number;
  xin: string;
  phone: string;
  mobilePhone: string;
  phoneFax: string;
  email: string;
  commonAddress: string;
  shortAddress: string;
  address: string;
  addressKz: string;
  addressEn: string;
  republic: string;
  oblast: string;
  region: string;
  city: string;
  street: string;
  isBeneficiary: boolean;
  jurRegNumber: string;
  govReg: string;
  dateBegin: Date;
  dateEnd: Date;
  powerAttorneyFullNum: string;
  isNotResident: boolean;
  isNotMention: boolean;
  contactInfos: ContactInfoDto[];
  apartment: string;
  beneficiaryTypeId: number;
  displayOrder: number;
  constructor(init?: Partial<SubjectDto>) {
    Object.assign(this, init);
  }
}

export class ContactInfoDto {
  id: number;
  info: string;
  typeId: number;
}

export class SubjectConstants {
  public static streetPartIds = [
    'G11',
    'G12',
    'G13',
    'G14',
    'G15',
    'G16',
    'G17',
    'G18',
    'G19',
    'G20',
    'G23',
    'G21',
    'G30',
    'G31',
    'G41',
    'G43',
    'G24',
    'G25',
    'G26',
    'G27',
    'G29',
    'G32',
    'G33',
    'G34',
    'G35',
    'G36',
    'G39',
    'G40',
    'G42',
    'G46',
    'G48',
    'G50',
    'G44',
    'G28',
    'G51',
    'G58',
    'G61',
    'G62',
    'G63',
    'G64',
    'G88',
    'G66',
    'G67',
    'G71',
    'G72',
    'G89',
    'G90',
    'G91',
    'G92',
    'G78',
    'G76',
    'A53',
    'G47',
    'G68',
    'G73',
    'G75',
    'G77',
    'G79',
    'G80',
    'G81',
    'G83',
    'G95',
    'G96',
    'G94',
    'G107',
    'G103',
    'G72',
    'G105',
    'G106'
  ];
  public static cityPartIds = ['A9', 'A6'];
  public static oblastPartIds = ['A7'];
  public static republicPartIds = ['A10'];
  public static regionPartIds = ['A5', 'A8'];
}

export function concatAddresseeAddress(address: any, apartment: string): string {
  const streetPart = getPart(address, SubjectConstants.streetPartIds);
  const numberPart = address.fullAddress.number;
  const cityPart = getPart(address, SubjectConstants.cityPartIds);
  const oblastPart = getPart(address, SubjectConstants.oblastPartIds);
  const republicPart = getPart(address, SubjectConstants.republicPartIds);
  const postCode = address.postcode;
  const result = [];
  if (!!streetPart) {
    result.push(streetPart);
  }
  if (!!numberPart) {
    result.push('дом ' + numberPart);
  }
  if (!!apartment) {
    result.push('кв. ' + apartment);
  }
  if (!!cityPart) {
    result.push(cityPart);
  }
  if (!!oblastPart) {
    result.push(oblastPart);
  }
  if (!!republicPart) {
    result.push(republicPart);
  }
  if (!!postCode) {
    result.push(postCode);
  }
  return result.join(', ');
}

export function concatFullAddress(address: any, apartment: string): string {
  const streetPart = getPart(address, SubjectConstants.streetPartIds);
  const numberPart = address.fullAddress.number;
  const cityPart = getPart(address, SubjectConstants.cityPartIds);
  const oblastPart = getPart(address, SubjectConstants.oblastPartIds);
  const republicPart = getPart(address, SubjectConstants.republicPartIds);
  const postCode = address.postcode;
  const result = [];
  if (!!republicPart) {
    result.push(republicPart);
  }
  if (!!oblastPart) {
    result.push(oblastPart);
  }
  if (!!cityPart) {
    result.push(cityPart);
  }
  if (!!streetPart) {
    result.push(streetPart);
  }
  if (!!numberPart) {
    result.push('дом ' + numberPart);
  }
  if (!!apartment) {
    result.push('кв. ' + apartment);
  }
  if (!!postCode) {
    result.push(postCode);
  }
  return result.join(', ');
}

export function getPart(selectedAddress: any, partIds: string[]) {
  const part = selectedAddress.fullAddress.parts.find(p =>
    partIds.includes(p.type.id)
  );
  return part ? part.nameRus : '';
}
