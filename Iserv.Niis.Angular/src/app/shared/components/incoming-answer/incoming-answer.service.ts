import { Injectable } from '@angular/core';
import { BaseServiceWithPagination } from 'app/shared/base-service-with-pagination';
import { MaterialTask } from 'app/materials/models/materials.model';
import { HttpClient } from '@angular/common/http';
import { ConfigService } from 'app/core';
import { ErrorHandlerService } from 'app/core/error-handler.service';
import { SnackBarHelper } from 'app/core/snack-bar-helper.service';

@Injectable()
export class IncomingAnswerService extends BaseServiceWithPagination<MaterialTask> {

  api = '/api/materials/lastStageIncoming';
  constructor(http: HttpClient,
    configService: ConfigService,
    errorHandlerService: ErrorHandlerService,
    private snackBarHelper: SnackBarHelper) {
    super(http, configService, errorHandlerService, '/api/materials/lastStageIncoming');
  }

}
