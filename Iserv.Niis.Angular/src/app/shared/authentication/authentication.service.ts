import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'ngx-auth';
import { NgxPermissionsService } from 'ngx-permissions';
import { Observable } from 'rxjs/Observable';

import { ConfigService } from '../../core';
import { ErrorHandlerService } from '../../core/error-handler.service';
import { AccessData, LoginModel } from './authentication.model';
import { PermissionConstantService } from './perpission-constant.service';
import { TokenStorageService } from './token-storage.service';
import { ContentType } from '@angular/http/src/enums';

@Injectable()
export class AuthenticationService implements AuthService {
  constructor(
    private http: HttpClient,
    private router: Router,
    private tokenStorageService: TokenStorageService,
    private permissionConstantService: PermissionConstantService,
    private permissionsService: NgxPermissionsService,
    private configService: ConfigService,
    private errorHandlerService: ErrorHandlerService,
  ) { }

  public getAccessToken(): Observable<string> {
    return this.tokenStorageService.getAccessToken();
  }
  public refreshToken(): Observable<any> {
    this.logout();
    this.router.navigateByUrl('/login');
    return Observable.of(null);
  }
  public refreshShouldHappen(response: HttpErrorResponse): boolean {
    return response.status === 401;
  }
  public verifyTokenRequest(url: string): boolean {
    return url.endsWith('/refresh');
  }

  private saveAccessData({ access_token, profile }: AccessData) {
    this.tokenStorageService.setAccessToken(access_token);
    this.tokenStorageService.setProfilePermissions(
      this.permissionConstantService.getNamesByValues(profile.prm)
    );
    this.tokenStorageService.setProfileOriginalPermissions(profile.prm);
    this.tokenStorageService.setProfile(profile);
  }

  public isAuthorized(): Observable<boolean> {
    return this.tokenStorageService.getAccessToken().map((token: string) => {
      return !!token;
    });
  }

  public hasOriginalProfilePermission(permission: string): boolean {
    var profilePermissions = this.tokenStorageService.getProfileOriginalPermissions();
    return profilePermissions != null && profilePermissions.indexOf(permission) > 0;
  }

  public login(loginModel: LoginModel): Observable<any> {

    return this.http
      .post(`${this.configService.apiUrl}/api/auth/login`, loginModel)
      .do((responseObject: AccessData) => {
        if (responseObject) {
          this.saveAccessData(responseObject);
          this.permissionsService.addPermission(
            this.tokenStorageService.getProfilePermissions()
          );
        }
      })
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      );
  }

  public logout(): void {
    this.http
      .post(`${this.configService.apiUrl}/api/auth/logout`, null)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .subscribe(() => {
        this.tokenStorageService.clear();
        this.router.navigateByUrl('/login');
      });
  }

  get positionTypeCode(): string {
    const name = localStorage.getItem('positionTypeCode');
    return name ? name : '';
  }

  get departmentId(): string {
    const name = localStorage.getItem('departmentId');
    return name ? name : '';
  }

  get name(): string {
    const name = localStorage.getItem('name');
    return name ? name : '';
  }

  get userId(): number {
    return +localStorage.getItem('id');
  }
}
