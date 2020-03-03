export enum NotificationStatusType {
    None,
    PhoneNotFound,
    EmailNotFound,
    SmsSendOk,
    SmsSendFail,
    EmailSendOk,
    EmailSendFail,
    CorrespondenceNotFound,
}

export enum NotificationType {
    None,
    Sms,
    Email
}

export function getNotificationTypeName(code: string): string {

    if (code.startsWith('E')) {
        return 'Статут отправки уведомления на электронную почту';
    } else {
        return 'Статут отправки SMS-уведомления';
    }
}
