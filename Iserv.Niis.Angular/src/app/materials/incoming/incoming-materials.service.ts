import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ConfigService } from 'app/core';
import { ErrorHandlerService } from 'app/core/error-handler.service';
import { SnackBarHelper } from 'app/core/snack-bar-helper.service';
import { BaseServiceWithPagination } from 'app/shared/base-service-with-pagination';
import { MaterialTask } from '../models/materials.model';

@Injectable()
export class IncomingMaterialsService extends BaseServiceWithPagination<MaterialTask> {

  api = '/api/materials/';
  constructor(http: HttpClient,
    configService: ConfigService,
    errorHandlerService: ErrorHandlerService,
    private snackBarHelper: SnackBarHelper) {
    super(http, configService, errorHandlerService, '/api/materials/');
  }

}
