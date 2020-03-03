import { Injectable } from '@angular/core';
import { BaseServiceWithPagination } from 'app/shared/base-service-with-pagination';
import { BulletinSectionListDto } from '../models/bulletin-section-list-dto';
import { HttpClient } from '@angular/common/http';
import { ConfigService } from 'app/core';
import { ErrorHandlerService } from 'app/core/error-handler.service';
import { EditBulletinSectionRequestDto } from '../models/edit-bulletin-section-request-dto';
import { CreateBulletinSectionRequestDto } from '../models/create-bulletin-section-request-dto';

const BulletinSectionsApiUrl = '/api/bulletin/sections/';

@Injectable()
export class BulletinSectionsService extends BaseServiceWithPagination<BulletinSectionListDto> {
  private readonly api: string = BulletinSectionsApiUrl;

  constructor(
    http: HttpClient,
    configService: ConfigService,
    errorHandlerService: ErrorHandlerService
  ) {
    super(http, configService, errorHandlerService, BulletinSectionsApiUrl);
  }

  createBulletinSection(requestDto: CreateBulletinSectionRequestDto) {
    return this.http
      .post(`${this.configService.apiUrl}${this.api}create`, requestDto)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      );
  }

  editBulletinSection(bulletinSectionId: number, requestDto: EditBulletinSectionRequestDto) {
    return this.http
      .post(`${this.configService.apiUrl}${this.api}${bulletinSectionId}/edit`, requestDto)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      );
  }
  
}
