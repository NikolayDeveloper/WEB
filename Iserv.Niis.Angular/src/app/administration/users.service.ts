import { HttpClient, HttpErrorResponse  } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User, UserDetails } from 'app/administration/components/users/models/user.model';
import { ConfigService } from 'app/core';
import { ErrorHandlerService } from 'app/core/error-handler.service';
import { BaseServiceWithPagination } from 'app/shared/base-service-with-pagination';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class UsersService extends BaseServiceWithPagination<User> {
  private readonly api: string = '/api/users/';
  private readonly apiUrl: string;
  constructor(http: HttpClient,
    configService: ConfigService,
    errorHandlerService: ErrorHandlerService) {
    super(http, configService, errorHandlerService, '/api/users/');
    this.apiUrl = `${this.configService.apiUrl}${this.api}`;
  }

  getCurrent(): Observable<UserDetails> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}getCurrent`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: UserDetails) => data);
  }

  get(): Observable<User[]> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}list`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: User[]) => data);
  }

  getById(id: number): Observable<UserDetails> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}${id}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: UserDetails) => data);
  }

  addUser(newUser: UserDetails): Observable<UserDetails> {
    newUser.id = 0;
    return this.http
      .post(this.apiUrl, newUser)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: UserDetails) => data);
  }

  updateUser(existingUser: UserDetails): Observable<any> {
    return this.http
      .put(`${this.apiUrl}/${existingUser.id}`, existingUser)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err));
  }
}
