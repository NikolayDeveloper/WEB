import { Component, OnInit, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { MatDialogRef } from '@angular/material';
import { IntegrationWith1cService } from 'app/shared/services/integration-with-1c.service';
import { ImportPaymentsRequestDto } from 'app/shared/models/import-payments-request-dto';
import { Subject } from 'rxjs';
import { ImportPaymentsResponseDto } from 'app/shared/models/import-payments-response-dto';
import { ImportPaymentsErrorType } from 'app/shared/models/import-payments-error-type.enum';
import { ModalService } from 'app/shared/services/modal.service';
import { fromDateToJsonString } from 'app/payments-journal/helpers/date-helpers';

@Component({
  selector: 'app-import-payments-dialog',
  templateUrl: './import-payments-dialog.component.html',
  styleUrls: ['./import-payments-dialog.component.scss']
})
export class ImportPaymentsDialogComponent implements OnInit {
  formGroup: FormGroup;
  private onDestroy = new Subject();

  public paymentsImported: EventEmitter<number> = new EventEmitter<number>();

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<ImportPaymentsDialogComponent>,
    private integrationWith1CService: IntegrationWith1cService,
    private modalService: ModalService) {
  }

  ngOnInit() {
    this.buildForm();
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      paymentsDate: new FormControl(new Date(), Validators.required)
    });
  }

  onCancelClick(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    if (this.formGroup.invalid) {
      return;
    }

    const paymentsDate = this.formGroup.get('paymentsDate').value;
    const requestDto = new ImportPaymentsRequestDto();
    requestDto.paymentsDate = fromDateToJsonString(paymentsDate);

    this.integrationWith1CService.importPayments(requestDto)
      .takeUntil(this.onDestroy)
      .subscribe((response: ImportPaymentsResponseDto) => {
        if (!response.error) {
          this.dialogRef.close();
          this.modalService.ok('Импорт платежей', `Загружено ${response.importedNumber} платежей.`)
            .subscribe();
          this.paymentsImported.emit(response.importedNumber);
        }
        else {
          switch (response.errorType) {
            case ImportPaymentsErrorType.CannotCreateComConnectorInstance:
              this.modalService.ok('Импорт платежей', 'Не удалось создать экземпляр com connector.')
                .subscribe();
              break;
            case ImportPaymentsErrorType.CannotConnectTo1CDatabase:
              this.modalService.ok('Импорт платежей', 'Ошибка подключения к БД 1С.')
                .subscribe();
              break;
            case ImportPaymentsErrorType.UnknownComError:
              this.modalService.ok('Импорт платежей', 'Произошла неизвестная ошибка при работе с com.')
                .subscribe();
              break;
          }
        }
      });
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

}
