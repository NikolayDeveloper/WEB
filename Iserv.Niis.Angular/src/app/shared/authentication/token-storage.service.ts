import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { Profile } from './authentication.model';
import { UserDetails } from 'app/administration/components/users/models/user.model';

@Injectable()
export class TokenStorageService {
  public getAccessToken(): Observable<string> {
    const token: string = <string>localStorage.getItem('access_token');
    return Observable.of(token);
  }

  public setAccessToken(token: string): TokenStorageService {
    localStorage.setItem('access_token', token);

    return this;
  }

  public setUserData(data: UserDetails) {
    localStorage.setItem('positionTypeNameRu', data.positionTypeNameRu);
    localStorage.setItem('positionTypeCode', data.positionTypeCode);
    localStorage.setItem('departmentId', data.departmentId.toString());
  }

  public setProfile(profile: Profile): TokenStorageService {
    localStorage.setItem('id', profile.id.toString());
    localStorage.setItem('email', profile.email);
    localStorage.setItem('name', profile.name);
    localStorage.setItem('roles', JSON.stringify(profile.roles));

    return this;
  }

  public getCurrentUserID() {
    const id = localStorage.getItem('id');

    return id ? Number(id) : null;
  }

  public setProfilePermissions(permissions: string[]) {
    localStorage.setItem('prm', JSON.stringify(permissions));
  }

  public getProfilePermissions(): string[] {
    return JSON.parse(localStorage.getItem('prm'));
  }

  public setProfileOriginalPermissions(permissions: string[]) {
    localStorage.setItem('original_prm', JSON.stringify(permissions));
  }

  public getProfileOriginalPermissions(): string[] {
    return JSON.parse(localStorage.getItem('original_prm'));
  }

  public clear() {
    localStorage.clear();
  }

}
