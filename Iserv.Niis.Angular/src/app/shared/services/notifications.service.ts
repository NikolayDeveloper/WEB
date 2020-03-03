import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ErrorHandlerService } from 'app/core/error-handler.service';
import { ConfigService } from 'app/core';
import { Observable } from 'rxjs/Observable';
import { NotificationStatus, errorStatusesForResent } from './models/notification-status';
import { OwnerType } from 'app/shared/services/models/owner-type.enum';
import { forEach } from '@angular/router/src/utils/collection';
import { getNotificationTypeName, NotificationStatusType, NotificationType } from 'app/shared/services/models/notification-status-type';

@Injectable()
export class NotificationsService {
  private readonly api: string = 'api/notifications/';
  constructor(private http: HttpClient,
    private errorHandlerService: ErrorHandlerService) {
  }

  getStatusList(ownerType: OwnerType, ownerId: number): Observable<NotificationStatus[]> {
    return this.http
      .get(`${this.api}/${ownerType}/${ownerId}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: NotificationStatus[]) => {
        data.forEach((status) => {
          status.isNeedResend = errorStatusesForResent.includes(status.code);
          status.typeName = getNotificationTypeName(status.code);
        });
        return data;
      });
  }

  resend(ownerType: OwnerType, ownerId: number, documentId: number): Observable<NotificationStatus[]> {
    return this.http
      .get(`${this.api}send/${ownerType}/${ownerId}/${documentId}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: any[]) => {
        data.forEach((status) => {
          status.isNeedResend = errorStatusesForResent.includes(status.code);
          status.typeName = getNotificationTypeName(status.code);
        });
        return data;
      });
  }
}
