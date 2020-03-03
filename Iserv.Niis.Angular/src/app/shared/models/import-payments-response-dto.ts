import { ImportPaymentsErrorType } from "./import-payments-error-type.enum";

export class ImportPaymentsResponseDto {
    importedNumber: number;
    error: boolean;
    errorType: ImportPaymentsErrorType;
}
