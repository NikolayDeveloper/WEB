import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';

import { SnackBarHelper } from './snack-bar-helper.service';


@Injectable()
export class ErrorHandlerService {

  constructor(
    private snackBarHelper: SnackBarHelper,
    private router: Router) { }

  handleError(error: HttpErrorResponse | any): Observable<any> {
    let errMsg: string;
    if (error instanceof HttpErrorResponse) {
      const rootError = this.getRootError(error);
      errMsg = rootError.message ? rootError.message : rootError.toString();

      switch (error.status) {
        case 400:
          break;
        case 401:
          this.router.navigate(['login']);
          break;
        case 403:
          this.router.navigate(['403']);
          break;
        case 404:
          this.router.navigate(['404']);
          break;
        case 412:
          errMsg = 'Обновите пожалуйста страницу, данные устарели: ' + errMsg;
          break;
        case 422:
          break;
        default:
          break;
      }
    } else {
      errMsg = error.message ? error.message : error.toString();
    }
    this.snackBarHelper.error(errMsg);

    return Observable.throw(errMsg);
  }

  private getRootError(error: any): any {
    if (error.error) {
      return this.getRootError(error.error);
    }
    return error || {};
  }
}
