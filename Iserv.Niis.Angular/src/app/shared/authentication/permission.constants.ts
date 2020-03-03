/**
 * Список прав доступа
 *
 * @export
 * @class PermissionConstants
 */
export class PermissionConstants {

  /*******************
  * Common
  *******************/

  /**
   * Доступ к модулю \"Отчеты\"
   */
  public static ReportsModule = 'reports.module';

  /**
   * Доступ к модулю \"Администрирование\"
   */
  public static AdministrationModule = 'administration.module';

  /**
   * Доступ к модулю \"Международные поисковые системы\"
   */
  public static InternationalSearchEnginesModule = 'internationalSearchEngines.module';

  /*******************
    * Actions
    *******************/

  /**
   * Генерация производственного номера
   */
  public static PoductNumberGeneration = 'action.productNumberGeneration';

  /**
   * Пометка на удаление документа или материала документа на своем этапе
   */
  public static MarkDocumentAsDeleted = 'action.markDeleted';

  /**
   * Назначение исполнителя
   */
  public static AssignExecutor = 'action.assignExecutor';

  /**
   * Редактирование Заявки/ОД
   */
  public static EditRequestOrProtectionDocument = 'action.requestProtectionDoc.edit';

  /**
   * Зачтение и списание оплат
   */
  public static PaymentAcctreptanceAndWriteOff = 'action.payment.acceptanceAndWriteOff';

  /**
   * Согласование документов (Подписание с ЭЦП)
   */
  public static SigningWithDS = 'action.signingWithDS';

  /**
   * Несогласование документов (отправка на доработку)
   */
  public static SendingForRevision = 'action.sendingForRevision';

  /*******************
  * Journal
  *******************/

  /**
   * Доступ к модулю \"Дневник\"
   */
  public static JournalModule = 'journal.module';

  /**
   * Просмотр дневника (Задачи сотрудников)
   */
  public static JournalViewStaffTasks = 'journal.staffTasks.view';

  /*******************
  * Correspndence
  *******************/

  /**
   * Доступ к модулю \"Материалы\" (корреспонденция)
   */
  public static MaterialsModule = 'correspondence.module';

  /**
   * Обработка входящей корреспонденции
   */
  public static ProcessingIncomingCorrespondence = 'correspondence.incoming.processing';

  /**
   * Обработка исходящей корреспонденции
   */
  public static ProcessingOutgoingCorrespondence = 'correspondence.outgoing.processing';

  /**
   * Обработка внутренней корреспонденции
   */
  public static ProcessingInternalCorrespondence = 'correspondence.internal.processing';

  /**
   * Создание входящей корреспонденции
   */
  public static CreatingIncomingCorrespondence = 'correspondence.incoming.create';

  /**
   * Создание исходящей корреспонденции
   */
  public static CreatingOutgoingCorrespondence = 'correspondence.outgoing.create';

  /**
   * Создание внутренней корреспонденции
   */
  public static CreatingInternalCorrespondence = 'correspondence.internal.create';

  /*******************
  * Search
  *******************/

  /**
   * Доступ к модулю \"Поиск\"
   */
  public static SearchModule = 'search.module';

  /**
   * Расширенный поиск
   */
  public static AdvancedSearch = 'search.advanced';

  /**
   * Экспертный поиск
   */
  public static ExpertSearch = 'search.expert';

  /**
   * Автораспределение
   */
  public static JournalViewAutoAllocation = 'journal.autoAllocation.view';

  /*******************
  * Bulletin
  *******************/
  /**
   * Доступ к модулю \"Бюллетень\"
   */
  public static BulletinModule = 'bulletin.module';
}
