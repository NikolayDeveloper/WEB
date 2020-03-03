export class EditPaymentUseResponseDto {
    amountIsGreaterThanPaymentInvoiceReminder: boolean;
    paymentInvoiceReminder: number;
    amountIsGreaterThanPaymentReminder: boolean;
    paymentReminder: number;
    paymentInvoiceNewReminderIsGreaterThan100KZT: boolean;
    success: boolean;
}
