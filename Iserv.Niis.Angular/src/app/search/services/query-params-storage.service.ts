import { Injectable } from '@angular/core';
import { SearchType } from 'app/search/models/search-type.enum';

@Injectable()
export class QueryParamsStorageService {
  private keyPrefix = 'query_params_';

  constructor() { }

  public get(type: SearchType): any[] {
    return JSON.parse(localStorage.getItem(this.keyPrefix + type.toString()));
  }

  public set(type: SearchType, queryParams: any[]) {
    localStorage.setItem(this.keyPrefix + type.toString(), JSON.stringify(queryParams));
  }

  public clear(type: SearchType): void {
    localStorage.removeItem(this.keyPrefix + type.toString());
  }
  public prepareQueryParams(queryParams: any[]): any {
    return JSON.parse(JSON.stringify(queryParams));
  }
}
