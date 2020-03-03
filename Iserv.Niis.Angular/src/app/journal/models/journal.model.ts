
/**
 * Описывает статистику по задачам сотрудника
 *
 * @export
 * @interface StaffTask
 */
export interface StaffTask {
    id: number;
    fullName: string;
    incoming: number;
    executed: number;
    onJob: number;
    notOnJob: number;
    overdue: number;
    outgoing: number;
}
