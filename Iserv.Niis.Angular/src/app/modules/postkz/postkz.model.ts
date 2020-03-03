export class AddressResponse {
  from: number;
  total: number;
  data: Address[];
}

export class Address {
  id: string;
  postcode: string;
  oldPostcode: string;
  addressRus: string;
  addressKaz: string;
}