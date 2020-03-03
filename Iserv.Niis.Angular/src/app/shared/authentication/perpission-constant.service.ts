import { Injectable } from '@angular/core';

import { PermissionConstants } from './permission.constants';

@Injectable()
export class PermissionConstantService {

  constructor() { }


  /**
   * Возвращает значение для права доступа
   *
   * @param {string} key
   * @returns {string}
   * @memberof PerpissionService
   */
  public getByKey(keys: string[]): string[] {
    const permissions: string[] = [];
    keys.forEach(item => permissions.push(PermissionConstants[item]));
    return permissions;
  }


  /**
   * Возвращает имена свойств на основании значений прав доступа для более удобного использования в представлениях.
   * Библиотека ngx-permissions требует указания прав в виде строковых значений
   * Позволяет использовать *ngxPermissionsOnly="'JournalTasksViewActive'", а не *ngxPermissionsOnly="'journalTasks.view.active'"
   *
   * @param {string[]} permissionValues
   * @returns {string[]}
   * @memberof PermissionConstantService
   */
  public getNamesByValues(permissionValues: string[]): string[] {
    return Object.keys(PermissionConstants)
      .filter(key => permissionValues.includes(PermissionConstants[key]));
  }
}
