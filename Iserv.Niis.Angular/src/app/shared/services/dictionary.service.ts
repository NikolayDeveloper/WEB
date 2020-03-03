import 'rxjs/add/observable/forkJoin';
import 'rxjs/add/operator/map';

import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { ConfigService } from '../../core';
import { ErrorHandlerService } from '../../core/error-handler.service';
import { BaseDictionary, DicRouteStage, DicProtectionDocSubType } from './models/base-dictionary';
import { DictionaryType } from './models/dictionary-type.enum';
import { RouteStageOrder } from './models/route-stage-order';
import { SelectOption } from './models/select-option';
import { TreeNode } from 'primeng/components/common/treenode';
import { StringWrapper } from './models/string-wrapper';

const automaticReceiveTypeCodes: string[] = ['6', '7', 'Import'];

const emptyBaseDictionary: BaseDictionary = {
  dateCreate: new Date(),
  code: '',
  dicType: null,
  id: null,
  nameRu: '',
  nameKz: '',
  description: '',
  nameEn: ''
};

@Injectable()
export class DictionaryService {
  private readonly api: string = '/api/dictionaries/';
  constructor(private http: HttpClient,
    private configService: ConfigService,
    private errorHandlerService: ErrorHandlerService) { }

  get automaticReceiveTypeCodes() { return automaticReceiveTypeCodes; }

  get emptyBaseDictionary() { return emptyBaseDictionary; }

  getSelectOptions(type: DictionaryType): Observable<SelectOption[]> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}${type}/select`)
      .map((selectOptions: any) => {
        selectOptions.forEach((option: SelectOption) => {
          option.dicType = type;
        });

        return selectOptions;
      })
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err));
  }

  getSelectOptionsByCode(type: DictionaryType, codes: string[]): Observable<SelectOption[]> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}${type}/select/bycodes`,
        { params: new HttpParams({ fromString: codes.join(',') }) })
      .map((selectOptions: any) => {
        selectOptions.forEach((option: SelectOption) => {
          option.dicType = type;
        });

        return selectOptions;
      })
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err));
  }

  getCombinedSelectOptions(types: DictionaryType[]): Observable<SelectOption[]> {
    const observables: Observable<SelectOption[]>[] = [];
    types.forEach(type => {
      const observable = this.getSelectOptions(type);
      observables.push(observable);
    });

    return Observable
      .forkJoin(observables)
      .map((selectOptionsArray: SelectOption[][]) => {
        const combinedSelectOptions: SelectOption[] = [].concat.apply([], selectOptionsArray);
        return combinedSelectOptions;
      });
  }

  /**
   * Метод для получения статусов заявки.
   */
  getDicRequestStatusForExpertSearch(): Observable<any> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}GetDicRequestStatusForExpertSearch`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: any) => data);
  }

  /**
   * Метод для получения статусов документов.
   */
  getDicProtectionDocStatusForExpertSearch(): Observable<any> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}GetDicProtectionDocStatusForExpertSearch`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: any) => data);
  }

  getBaseDictionary(type: DictionaryType): Observable<any> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}${type}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: any) => data);
  }

  getQueryBaseDictionary(type: DictionaryType, keyValuePairs: any[]): Observable<any> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}${type}`, {
        params: new HttpParams({ fromString: this.buildQueryString(keyValuePairs) })
      })
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: any) => data); 
  }

  getBaseDictionaryByCodes(type: DictionaryType, codes: string[]) {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}/${type}/bycodes`,
        { params: new HttpParams({ fromString: codes.join(',') }) })
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: BaseDictionary[]) => data);
  }

  getBaseDictionaryById(type: DictionaryType, id: number): Observable<BaseDictionary> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}/${type}/${id}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: BaseDictionary) => data);
  }

  getRouteStageOrders(): Observable<RouteStageOrder[]> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}/routestageorder`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: RouteStageOrder[]) => data);
  }

  getDetailIcgs(icgsId: number): Observable<SelectOption[]> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}/getDetailIcgs/${icgsId}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: SelectOption[]) => data);
  }
  getGetBaseTreeNode(type: DictionaryType): Observable<any> {
    return this.http
      .get(`${this.api}getBaseTreeNode/${type}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: TreeNode[]) => data);
  }
  getDicTariffs(protectionDocTypeId: number): Observable<SelectOption[]> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}/getDicTariffs/${protectionDocTypeId}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: SelectOption[]) => data);
  }
  getDicRouteStages(): Observable<DicRouteStage[]> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}/getDicRouteStages`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: DicRouteStage[]) => data);
  }

  getDicIpcChildren(parentId: number): Observable<TreeNode[]> {
    return this.http
      .get(`${this.api}getDicIpcChildren/${parentId}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: TreeNode[]) => data);
  }
  getDicIpcRoots(): Observable<TreeNode[]> {
    return this.http
      .get(`${this.api}getDicIpcRoots`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: TreeNode[]) => data);
  }
  getDicIpcs(ipcIds: number[]): Observable<TreeNode[]> {
    return this.http
      .post(`${this.api}getDicIpcs`, ipcIds)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: TreeNode[]) => data);
  }
  searchDicIpc(searchText: string): Observable<TreeNode[]> {
    const stringWrapper = new StringWrapper();
    stringWrapper.str = searchText;
    return this.http
      .post(`${this.api}searchDicIpc`, stringWrapper)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: TreeNode[]) => data);
  }
  getDicICFEMColors(): Observable<TreeNode[]> {
    return this.http
      .get(`${this.api}getDicICFEMColors`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: TreeNode[]) => data);
  }
  getDicICISs(): Observable<TreeNode[]> {
    return this.http
      .get(`${this.api}getDicICISs`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: TreeNode[]) => data);
  }

  private buildQueryString(keyValuePairs: any[] = []): string {
    return keyValuePairs
      .filter(item => item.value !== undefined && item.value.toString().length)
      .map(item => `${item.key}=${item.value}`)
      .join('&');
  }
}
