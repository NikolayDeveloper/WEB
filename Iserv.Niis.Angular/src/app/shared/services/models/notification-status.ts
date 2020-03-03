import { OwnerType } from './owner-type.enum';
import { NotificationStatusType, NotificationType } from './notification-status-type';


export const errorStatusCodes: string[] = [
    'ENF', 'ESF', 'ECNF', 'PNF', 'SSF', 'SCNF'
];

export const errorStatusesForResent: string[] = [
    'ENF', 'ESF', 'ECNF',
];

export class NotificationStatus {
    nameEn: string;
    nameRu: string;
    nameKz: string;
    code: string;

    isNeedResend: boolean;
    typeName: string;
}
