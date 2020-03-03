import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ConfigService } from 'app/core';
import { ErrorHandlerService } from 'app/core/error-handler.service';
import { SnackBarHelper } from 'app/core/snack-bar-helper.service';
import { BaseServiceWithPagination } from 'app/shared/base-service-with-pagination';
import { Subject } from 'rxjs/Subject';

@Injectable()
export class DocumentFlowService extends BaseServiceWithPagination<Request> {
  private api  = 'api/requests/';
  private readonly apiUrl: string;
  public searchFields: Subject<any> = new Subject();
  public searchMatFields: Subject<any> = new Subject();
  public tableStates: Subject<any> = new Subject();

  constructor(http: HttpClient,
    configService: ConfigService,
    errorHandlerService: ErrorHandlerService,
    private snackBarHelper: SnackBarHelper) {
    super(http, configService, errorHandlerService, 'api/requests/');
  }
}
