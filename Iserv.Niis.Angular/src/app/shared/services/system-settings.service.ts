import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ErrorHandlerService } from 'app/core/error-handler.service';
import { ConfigService } from 'app/core';
import { SettingsType } from './models/settings-type';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class SystemSettingsService {
  private api: string;

  constructor(
    private http: HttpClient,
    private errorHandlerService: ErrorHandlerService,
    configService: ConfigService
  ) {
    this.api = `${configService.apiUrl}/api`;
  }

  public getSettingsByType(type: SettingsType): Observable<string> {
    return this.http
      .get(`${this.api}/Settings/${type}`)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: string) => data);
  }
}
