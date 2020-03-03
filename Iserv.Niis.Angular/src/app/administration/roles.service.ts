import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ConfigService } from 'app/core';
import { ErrorHandlerService } from 'app/core/error-handler.service';
import { Observable } from 'rxjs/Observable';

import { BaseServiceWithPagination } from '../shared/base-service-with-pagination';
import { SelectOption } from '../shared/services/models/select-option';
import { Permission, Role, RoleDetails } from './components/roles/models/role.models';

@Injectable()
export class RolesService extends BaseServiceWithPagination<Role> {
  private readonly api: string = '/api/roles/';
  private readonly apiUrl: string;

  constructor(http: HttpClient,
    configService: ConfigService,
    errorHandlerService: ErrorHandlerService) {
    super(http, configService, errorHandlerService, '/api/roles/');
    this.apiUrl = `${this.configService.apiUrl}${this.api}`;
  }

  getById(id: number): Observable<RoleDetails> {
    return this.http
      .get(`${this.apiUrl}${id}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: RoleDetails) => data);
  }

  addRole(newRole: RoleDetails): Observable<any> {
    return this.http
      .post(this.apiUrl, newRole)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err));
  }

  updateRole(existingRole: RoleDetails): Observable<any> {
    return this.http
      .put(`${this.apiUrl}${existingRole.id}`, existingRole)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err));
  }

  /**
   * Возвращает список ролей для выпадающего списка
   *
   * @returns {(Observable<SelectOption[]>)}
   * @memberof RolesService
   */
  getSelectOptions(): Observable<SelectOption[]> {
    return this.http
      .get(`${this.apiUrl}select`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: SelectOption[]) => data);
  }

  getPermissions(): Observable<Permission[]> {
    return this.http
      .get(`${this.apiUrl}prm`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: Permission[]) => data);
  }

  getStagesTree(): Observable<any> {
    return this.http.get(`${this.apiUrl}stagesTree`);
  }
}
