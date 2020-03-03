namespace Iserv.Niis.Common.Codes
{
    public static class RouteStageCodes
    {
        /// <summary>
        /// Заявка отозвана
        /// </summary>
        public const string RequestCanceled = "RequestCanceled";

        /// <summary>
        /// Просрочено
        /// </summary>
        public const string Overdue = "X01";

        #region Коды этапов для заявки МТЗ (Международные товарные знаки)

        /// <summary>
        /// Создание - Регистрация документа
        /// </summary>
        public const string ITZ_01_1 = "TMI01.1";
        /// <summary>
        /// Формирование данных заявки
        /// </summary>
        public const string ITZ_02_1 = "TMI02.1";
        /// <summary>
        /// Ожидание срока рассмотрения заявки
        /// </summary>
        public const string ITZ_03_1 = "TMI03.1";
        /// <summary>
        /// Выбор исполнителя полной экспертизы МТЗ
        /// </summary>
        public const string ITZ_03_3_1 = "TMI03.3.1";
        /// <summary>
        /// Полная экспертиза МТЗ
        /// </summary>
        public const string ITZ_03_3_2 = "TMI03.3.2";
        /// <summary>
        /// Вынесено экспертное заключение об охране
        /// </summary>
        public const string ITZ_03_3_3 = "TMI03.3.3";
        /// <summary>
        /// Вынесено экспертное заключение об отказе
        /// </summary>
        public const string ITZ_03_3_3_1 = "TMI03.3.3.1";
        /// <summary>
        /// Утверждено директором
        /// </summary>
        public const string ITZ_03_3_4 = "TMI03.3.4";
        /// <summary>
        /// На утверждение директору окончательных решений
        /// </summary>
        public const string ITZ_03_3_9 = "TMI03.3.9";
        /// <summary>
        /// Архив
        /// </summary>
        public const string ITZ_05 = "TMI05";
        /// <summary>
        /// Передача прав
        /// </summary>
        public const string ITZ_05_1 = "TMI05.1";
        /// <summary>
        /// Внесение изменений
        /// </summary>
        public const string ITZ_06 = "TMI06";
        /// <summary>
        /// Вынесение окончательного решения
        /// </summary>
        public const string ITZ_03_3_8 = "TMI03.3.8";
        /// <summary>
        /// Возражение на предварительный или частичный отказ
        /// </summary>
        public const string ITZ_03_3_5 = "TMI03.3.5";
        /// <summary>
        /// Утверждено директором окончательное решение
        /// </summary>
        public const string ITZ_03_3_9_1 = "TMI03.3.9.1";
        /// <summary>
        /// Положительное решение
        /// </summary>
        public const string ITZ_03_3_4_1 = "TMI03.3.4.1";
        /// <summary>
        /// Публикация заключения об отказе в ВОИС
        /// </summary>
        public const string ITZ_03_3_4_1_0 = "TMI03.3.4.1.0";
        /// <summary>
        /// Заключение окончательное по истечении срока
        /// </summary>
        public const string ITZ_03_3_4_1_1 = "TMI03.3.4.1.1";
        /// <summary>
        /// Передано в МЮ РК
        /// </summary>
        public const string ITZ_03_3_4_2 = "TMI03.3.4.2";
        /// <summary>
        /// Возвращено с МЮ РК
        /// </summary>
        public const string ITZ_03_3_4_3 = "TMI03.3.4.3";
        /// <summary>
        /// Решение об охране (окончательное заключение) в ВОИС
        /// </summary>
        public const string ITZ_03_3_4_4 = "TMI03.3.4.4";
        /// <summary>
        /// Публикация в ВОИС
        /// </summary>
        public const string ITZ_03_3_4_5 = "TMI03.3.4.5";
        /// <summary>
        /// Делопроизводство приостановлено
        /// </summary>
        public const string ITZ_03_3_2_1 = "TMI03.3.2.1";
        /// <summary>
        /// Направлен запрос
        /// </summary>
        public const string ITZ_03_3_2_0 = "TMI03.3.2.0";
        /// <summary>
        /// Продление срока
        /// </summary>
        public const string ITZ_03_3_4_5_1 = "TMI03.3.4.5.1";
        #endregion

        #region  Коды этапов для заявки ИЗ (Изобретения)

        /// <summary>
        /// Создание Заявки
        /// </summary>
        public const string I_01_1 = "B01.1";
        /// <summary>
        /// Формирование данных заявки
        /// </summary>
        public const string I_02_1 = "B02.1";
        /// <summary>
        /// Формальная экспертиза изобретения
        /// </summary>
        public const string I_03_2_1 = "B03.2.1";
        /// <summary>
        /// Создание ОД на патент
        /// </summary>
        public const string I_04_1 = "B04.1";
        /// <summary>
        /// Экспертиза по существу
        /// </summary>
        public const string I_03_2_4 = "B03.2.4";
        /// <summary>
        /// Экспертиза заявки на выдачу инновационного патента
        /// </summary>
        public const string I_03_3_1 = "B03.3.1";
        /// <summary>
        /// Проведение поиска (ФИПС)
        /// </summary>
        public const string I_03_2_2 = "B03.2.2";
        /// <summary>
        /// Внутренняя регистрация. Распределение по экспертам
        /// </summary>
        public const string I_03_1 = "B03.1";
        /// <summary>
        /// Отправка в ЕАПВ, ФИПС, ЕПВ или ВОИС
        /// </summary>
        public const string I_03_4_2 = "B03.4.2";
        /// <summary>
        /// Обработка евразийской или международной заявки
        /// </summary>
        public const string I_03_4_1 = "B03.4.1";
        /// <summary>
        /// Внесение изменений
        /// </summary>
        public const string I_03_9 = "B03.9";
        /// <summary>
        /// Архив
        /// </summary>
        public const string I_05 = "B05";
        /// <summary>
        /// Контроль  директора
        /// </summary>
        public const string I_01_2 = "B01.2";
        /// <summary>
        /// Контроль  заместителя
        /// </summary>
        public const string I_01_3 = "B01.3";
        /// <summary>
        /// Распределение
        /// </summary>
        public const string I_01_4 = "B01.4";
        /// <summary>
        /// Ввод оплат
        /// </summary>
        public const string I_02_2 = "B02.2";
        /// <summary>
        /// Распределение эксперту на проведение экспертизы по существу
        /// </summary>
        public const string I_03_2_3 = "B03.2.3";
        /// <summary>
        /// Создание ОД на инновационный патент
        /// </summary>
        public const string I_04_2 = "B04.2";
        /// <summary>
        /// Секретка
        /// </summary>
        public const string I_02_3 = "B02.3";
        /// <summary>
        /// Создание ОД на предварительный патент
        /// </summary>
        public const string I_04_3 = "B04.3";
        /// <summary>
        /// Подготовка к экспертизе по существу
        /// </summary>
        public const string I_03_2_1_0 = "B03.2.1_0";
        /// <summary>
        /// Вынесено решение и заключение
        /// </summary>
        public const string I_03_3_2 = "B03.3.2";
        /// <summary>
        /// Утверждено руководством 
        /// </summary>
        public const string I_03_3_3 = "B03.3.3";
        /// <summary>
        /// Передано в МЮ РК
        /// </summary>
        public const string I_03_3_4 = "B03.3.4";
        /// <summary>
        /// Возвращено с МЮ РК
        /// </summary>
        public const string I_03_3_5 = "B03.3.5";
        /// <summary>
        /// Подготовка для передачи в Госреестр
        /// </summary>
        public const string I_03_3_7_1 = "B03.3.7.1";
        /// <summary>
        /// Запрос экспертизы 
        /// </summary>
        public const string I_03_3_1_1 = "B03.3.1.1";
        /// <summary>
        /// Отозванные
        /// </summary>
        public const string I_04_0 = "B04.0";
        /// <summary>
        /// Готовые для передачи в Госреестр
        /// </summary>
        public const string I_03_3_8 = "B03.3.8";
        /// <summary>
        /// Готовые для передачи на экспертизу по существу
        /// </summary>
        public const string I_03_2_1_1 = "B03.2.1.1";
        /// <summary>
        /// Передача прав
        /// </summary>
        public const string I_05_2_1_1 = "B05.2.1.1";
        /// <summary>
        /// Поиск по Алмате УПИ
        /// </summary>
        public const string I_03_2_2_0 = "B03.2.2.0";
        /// <summary>
        /// Подготовка к поиску
        /// </summary>
        public const string I_03_2_1__1 = "B03.2.1_1";
        /// <summary>
        /// Поиск по Астане УПЭФиМ
        /// </summary>
        public const string I_03_2_2_1 = "B03.2.2.1";
        /// <summary>
        /// Поиск по Астане УПЭХБиМ
        /// </summary>
        public const string I_03_2_2_2 = "B03.2.2.2";
        /// <summary>
        /// Направлено уведомление о принятии решения
        /// </summary>
        public const string I_03_3_7_0 = "B03.3.7.0";
        /// <summary>
        /// Окончательно отозванные
        /// </summary>
        public const string I_04_0_0 = "B04.0.0";
        /// <summary>
        /// Признаны неохраноспособными
        /// </summary>
        public const string I_04_0_0_0 = "B04.0.0.0";
        /// <summary>
        /// Преобразование заявки на полезную модель
        /// </summary>
        public const string I_03_3_1__1 = "B03.3.1_1";
        /// <summary>
        /// Экспертиза заявки на выдачу патента (повторное по решению Апелляционного совета)
        /// </summary>
        public const string I_03_2_4_1 = "B03.2.4_1";
        /// <summary>
        /// Экспертиза заявки на выдачу инновационного патента (повторное по решению Апелляционного совета)
        /// </summary>
        public const string I_03_3_1_2 = "B03.3.1_2";
        /// <summary>
        /// Признаны неподанными или ошибочно зарегистрированные
        /// </summary>
        public const string I_04_0_0_1 = "B04.0.0.1";
        /// <summary>
        /// Отказано в выдаче охранного документа
        /// </summary>
        public const string I_03_3_4_1 = "B03.3.4.1";
        /// <summary>
        /// Готовые для передачи на формальную экспертизу
        /// </summary>
        public const string I_02_2_1 = "B02.2.1";
        /// <summary>
        /// Готовые для передачи в архив
        /// </summary>
        public const string I_02_2_1_0 = "B02.2.1.0";
        /// <summary>
        /// Ожидает оплату за подачу
        /// </summary>
        public const string I_02_2_0_0 = "B02.2.0.0";
        /// <summary>
        /// Ожидает заверенную копию конв.заявки
        /// </summary>
        public const string I_02_2_0_1 = "B02.2.0.1";
        /// <summary>
        /// Ожидает срок перевода на нац.фазу
        /// </summary>
        public const string I_02_2_0_2 = "B02.2.0.2";
        /// <summary>
        /// Ожидает перевод материалов заявки
        /// </summary>
        public const string I_02_2_0_3 = "B02.2.0.3";
        /// <summary>
        /// Продление срока 
        /// </summary>
        public const string I_03_3_1_1_0 = "B03.3.1.1.0";
        /// <summary>
        /// Направлен запрос
        /// </summary>
        public const string I_03_3_1_1_1 = "B03.3.1.1.1";
        /// <summary>
        /// Экспертиза заявки на выдачу патента (на основе ин.патента)
        /// </summary>
        public const string I_03_2_4_0 = "B03.2.4_0";
        /// <summary>
        /// Распределение эксперту на проведение экспертизы по существу (после поиска)
        /// </summary>
        public const string I_03_2_3_0 = "B03.2.3.0";
        /// <summary>
        /// Отсутствует ходатайство о досрочной публикации
        /// </summary>
        public const string I_03_3_7_2 = "B03.3.7.2";
        /// <summary>
        /// Отсутствует оплата (до внесения изменений в Патентный закон РК)
        /// </summary>
        public const string I_03_3_7_3 = "B03.3.7.3";
        /// <summary>
        /// Выбор исполнителя формальной экспертизы
        /// </summary>
        public const string I_02_2_1_0_0 = "B02.2.1.0.0";
        /// <summary>
        /// Отсутствует оплата государственной пошлины
        /// </summary>
        public const string I_03_3_7_4 = "B03.3.7.4";
        /// <summary>
        /// Отсутствует оплата, срок с правом восстановления
        /// </summary>
        public const string I_03_3_7_5 = "B03.3.7.5";
        /// <summary>
        /// Ожидает оплату за экспертизу по существу
        /// </summary>
        public const string I_02_2_0 = "B02.2.0";
        /// <summary>
        /// На утверждении руководства 
        /// </summary>
        public const string I_03_3_2_1 = "B03.3.2.1";
        /// <summary>
        /// Делопроизводство приостановлено
        /// </summary>
        public const string I_03_3_9_0 = "B03.3.9.0";
        /// <summary>
        /// Делопроизводство прекращено
        /// </summary>
        public const string I_03_3_9 = "B03.3.9";
        /// <summary>
        /// Рассмотрение возражения на Апелляционном совете
        /// </summary>
        public const string I_03_3_4_1_0 = "B03.3.4.1.0";

        /// <summary>
        /// Передано на подготовку уведомления
        /// </summary>
        public const string I_03_3_2_2 = "B03.3.5.0";

        /// <summary>
        /// Направлено решение заявителю
        /// </summary>
        public const string I_03_3_2_3 = "B03.3.2.3";

        /// <summary>
        /// Формирование данных заявки (выбор исполнителя)
        /// </summary>
        public const string BFormationPerformerChoosing = "BFormationPerformerChoosing";

        #endregion

        #region  Коды этапов для заявки ТЗ (Товарный знак)

        /// <summary>
        /// Создание Заявки
        /// </summary>
        public const string TZ_01_1 = "TM01.1";
        /// <summary>
        /// Формирование данных заявки
        /// </summary>
        public const string TZ_02_1 = "TM02.1";
        /// <summary>
        /// Формирование данных заявки (выбор исполнителя)
        /// </summary>
        public const string TZFormationPerformerChoosing = "TZFormationPerformerChoosing";
        /// <summary>
        /// Выбор исполнителя предварительной экспертизы
        /// </summary>
        public const string TZ_03_2_1 = "TM03.2.1";
        /// <summary>
        /// Предварительная экспертиза
        /// </summary>
        public const string TZ_03_2_2 = "TM03.2.2";

        /// <summary>
        /// Предварительная экспертиза
        /// </summary>
        public const string SA_032 = "TM03.2.2";
        /// <summary>
        /// Ожидает оплату за полную экспертизу
        /// </summary>
        public const string TZAwaitingFullExpertizePayment = "TZAwaitingFullExpertizePayment";
        /// <summary>
        /// Выбор исполнителя полной экспертизы
        /// </summary>
        public const string TZ_03_3_1 = "TM03.3.1";
        /// <summary>
        /// Выбор исполнителя 1ого этапа полной экспертизы
        /// </summary>
        public const string TZFirstFullExpertizePerformerChoosing = "TM03.3.1-1";
        /// <summary>
        /// Полная экспертиза
        /// </summary>
        public const string TZ_03_3_2 = "TM03.3.2";
        /// <summary>
        /// 1ый этап полной экспертизы
        /// </summary>
        public const string TZFirstFullExpertize = "TM03.3.2_1";
        /// <summary>
        /// Выбор исполнителя 2-ого этапа полной экспертизы
        /// </summary>
        public const string TZSecondFullExpertizePerformerChoosing = "TM03.3.1-2";
        /// <summary>
        /// 2ой этап полной экспертизы
        /// </summary>
        public const string TZSecondFullExpertize = "TM03.3.2_2";
        /// <summary>
        /// Передано на подготовку уведомления
        /// </summary>
        public const string TZNotificationPreparation = "TM03.3.6.0";
        /// <summary>
        /// Направлено уведомление заявителю
        /// </summary>
        public const string TZNotificationSentToDeclarant = "TZNotificationSentToDeclarant";
        /// <summary>
        /// Ожидания возражения заявителя
        /// </summary>
        public const string TZAwaitingDeclarantObjection = "TZAwaitingDeclarantObjection";
        /// <summary>
        /// Ожидание восстановления срока подачи возражения
        /// </summary>
        public const string TZAwaitingObjectionSubmissionTermRestoration =
            "TZAwaitingObjectionSubmissionTermRestoration";
        /// <summary>
        /// Рассмотрение возражения
        /// </summary>
        public const string TZObjectionConsideration = "TZObjectionConsideration";
        /// <summary>
        /// Вынесение решения о частичной регистрации
        /// </summary>
        public const string TZPartialRegistrationDecision = "TZPartialRegistrationDecision";
        /// <summary>
        /// Вынесение решения об отказе
        /// </summary>
        public const string TZRejectionDecision = "TZRejectionDecision";
        /// <summary>
        /// Ожидание восстановления  срока оплаты за  регистрацию
        /// </summary>
        public const string TZAwaitingRegistrationTermRestoration = "TZAwaitingRegistrationTermRestoration";
        /// <summary>
        /// Создание охранного документа
        /// </summary>
        public const string TZ_04 = "TM04";
        /// <summary>
        /// Проверка материалов поступивших заявок
        /// </summary>
        public const string TZ_03_1 = "TM03.1";
        /// <summary>
        /// Контроль Директора
        /// </summary>
        public const string TZ_01_2 = "TM01.2";
        /// <summary>
        /// Контроль заместителя
        /// </summary>
        public const string TZ_01_3 = "TM01.3";
        /// <summary>
        /// Распределение
        /// </summary>
        public const string TZ_01_4 = "TM01.4";
        /// <summary>
        /// Ввод оплаты
        /// </summary>
        public const string TZ_02_2 = "TM02.2";
        /// <summary>
        /// Архив
        /// </summary>
        public const string TZ_05 = "TM05";
        /// <summary>
        /// Вынесено экспертное заключение
        /// </summary>
        public const string TZ_03_3_3 = "TM03.3.3";
        /// <summary>
        /// Утверждено директором
        /// </summary>
        public const string TZ_03_3_4 = "TM03.3.4";
        /// <summary>
        /// Передано в МЮ РК
        /// </summary>
        public const string TZ_03_3_5 = "TM03.3.5";
        /// <summary>
        /// Возвращено с МЮ РК
        /// </summary>
        public const string TZ_03_3_6 = "TM03.3.6";
        /// <summary>
        /// Направлено решение заявителю
        /// </summary>
        public const string TZ_03_3_7 = "TM03.3.7";
        /// <summary>
        /// Готовые для передачи в Госреестр
        /// </summary>
        public const string TZ_03_3_8 = "TM03.3.8";
        /// <summary>
        /// На утверждение директору
        /// </summary>
        public const string TZ_03_3_3_1 = "TM03.3.3.1";
        /// <summary>
        /// Внесение изменений
        /// </summary>
        public const string TZ_06 = "TM06";
        /// <summary>
        /// Делопроизводство прекращено
        /// </summary>
        public const string TZ_03_3_9 = "TM03.3.9";
        /// <summary>
        /// Передача прав
        /// </summary>
        public const string TZ_05_1 = "TM05.1";
        /// <summary>
        /// Направлено заявителю заключение об окончательном отказе
        /// </summary>
        public const string TZ_03_3_9_1 = "TM03.3.9.1";
        /// <summary>
        /// Апелляционный Совет
        /// </summary>
        public const string TZ_03_3_9_2 = "TM03.3.9.2";
        /// <summary>
        /// Признаны неподанными и ошибочно зарегистрированные
        /// </summary>
        public const string TZ_02_2_1 = "TM02.2.1";
        /// <summary>
        /// Направлен запрос
        /// </summary>
        public const string TZ_03_3_7_1 = "TM03.3.7.1";
        /// <summary>
        /// Направлен запрос об оплате за выдачу ОД
        /// </summary>
        public const string TZ_03_3_7_2 = "TM03.3.7.2";
        /// <summary>
        /// Делопроизводство приостановлено
        /// </summary>
        public const string TZ_03_3_9_0 = "TM03.3.9.0";
        /// <summary>
        /// Отозваны
        /// </summary>
        public const string TZ_02_2_3 = "TM02.2.3";
        /// <summary>
        /// Заключение об отказе в регистрации
        /// </summary>
        public const string TZ_03_3_4_1 = "TM03.3.4.1";
        /// <summary>
        /// Готовые для передачи на полную экспертизу
        /// </summary>
        public const string TZ_03_2_2_1 = "TM03.2.2.2";
        /// <summary>
        /// Оформление на межд. регистрацию
        /// </summary>
        public const string TZ_03_3_2_1 = "TM03.3.2.1";
        /// <summary>
        /// Заключение о регистрации с частичным отказом или дискламацией
        /// </summary>
        public const string TZ_03_3_4_2 = "TM03.3.4.2";
        /// <summary>
        /// Заключение о регистрации
        /// </summary>
        public const string TZ_03_3_4_3 = "TM03.3.4.3";
        /// <summary>
        /// Направлено заявителю пред.заключение о част.рег.
        /// </summary>
        public const string TZ_03_3_4_4 = "TM03.3.4.4";
        /// <summary>
        /// Направлено эксперту на исправление
        /// </summary>
        public const string TZ_03_3_7_0 = "TM03.3.7.0";
        /// <summary>
        /// Ожидание восстановления пропущенного срока
        /// </summary>
        public const string TZ_03_3_7_4 = "TM03.3.7.4";
        /// <summary>
        /// Восстановление срока
        /// </summary>
        public const string TZTermRestart = "TZTermRestart";
        /// <summary>
        /// Восстановление срока (предварительная)
        /// </summary>
        public const string TZTermRestartFormal = "TZTermRestartFormal";
        /// <summary>
        /// Продление срока (предварительная)
        /// </summary>
        public const string TZTermProlongationFormal = "TZTermProlongationFormal";
        /// <summary>
        /// Восстановление срока (полная)
        /// </summary>
        public const string TZTermRestartFull = "TZTermRestartFull";
        /// <summary>
        /// Продление срока (полная)
        /// </summary>
        public const string TZTermProlongationFull = "TZTermProlongationFull";
        /// <summary>
        /// Восстановление срока (возражение)
        /// </summary>
        public const string TZTermRestartObjection = "TZTermRestartObjection";
        /// <summary>
        /// Продление срока (возражение)
        /// </summary>
        public const string TZTermProlongationObjection = "TZTermProlongationObjection";
        /// <summary>
        /// Направлен запрос (предварительная)
        /// </summary>
        public const string TZRequestFormal = "TZRequestFormal";
        /// <summary>
        /// Направлен запрос (полная)
        /// </summary>
        public const string TZRequestFull = "TZRequestFull";
        /// <summary>
        /// Восстановление срока (преобразование)
        /// </summary>
        public const string TZTermRestartConvert = "TZTermRestartConvert";
        /// <summary>
        /// Продление срока (преобразование)
        /// </summary>
        public const string TZTermProlongationConvert = "TZTermProlongationConvert";
        /// <summary>
        /// Направлен запрос (преобразование)
        /// </summary>
        public const string TZRequestConvert = "TZRequestConvert";
        /// <summary>
        /// Восстановление срока (разделение)
        /// </summary>
        public const string TZTermRestartSplit = "TZTermRestartSplit";
        /// <summary>
        /// Продление срока (разделение)
        /// </summary>
        public const string TZTermProlongationSplit = "TZTermProlongationSplit";
        /// <summary>
        /// Направлен запрос (разделение)
        /// </summary>
        public const string TZRequestSplit = "TZRequestSplit";
        /// <summary>
        /// Восстановление срока (внесение изменений)
        /// </summary>
        public const string TZTermRestartChange = "TZTermRestartChange";
        /// <summary>
        /// Продление срока (внесение изменений)
        /// </summary>
        public const string TZTermProlongationChange = "TZTermProlongationChange";
        /// <summary>
        /// Направлен запрос (внесение изменений)
        /// </summary>
        public const string TZRequestChange = "TZRequestChange";
        /// <summary>
        /// Продление срока
        /// </summary>
        public const string TZ_03_3_7_3 = "TM03.3.7.3";
        /// <summary>
        /// Ожидаемые оплату за полную экспертизу
        /// </summary>
        public const string TZ_03_2_2_0 = "TM03.2.2.1";
        /// <summary>
        /// Возражение на предварительный или частичный отказ
        /// </summary>
        public const string TZ_03_3_2_0 = "TM03.3.2.0";
        /// <summary>
        /// Окончательное экспертное заключение
        /// </summary>
        public const string TZ_03_3_3_0 = "TM03.3.3.0";
        /// <summary>
        /// Вынесение окончательного экспертного заключения (с согласием/без согласия/по решению МЮ)
        /// </summary>
        public const string TZ_03_3_2_0_0 = "TM03.3.2.0.0";
        /// <summary>
        /// Направлено заявителю пред.заключение об отказе
        /// </summary>
        public const string TZ_03_3_4_4_0 = "TM03.3.4.4.0";
        /// <summary>
        /// Решение Апелляционного Совета
        /// </summary>
        public const string TZ_03_3_9_2_0 = "TM03.3.9.2.0";
        /// <summary>
        /// Подготовлено уведомление о регистрации
        /// </summary>
        public const string TZ_03_3_9_2_1 = "TM03.3.9.2.1";
        /// <summary>
        /// Окончательный отказ в связи с отсутствием возражения
        /// </summary>
        public const string TZ_03_3_9_1_0 = "TM03.3.9.1.0";
        /// <summary>
        /// Ожидаемые оплату за прием и проведение предварительной экспертизы
        /// </summary>
        public const string TZ_02_2_2 = "TM02.2.2";
        /// <summary>
        /// Разделение заявки на ТЗ
        /// </summary>
        public const string TZ_03_3_2_2 = "TM03.3.2.2";
        /// <summary>
        /// Рассмотрение заявки на экспертном совете
        /// </summary>
        public const string TZ_03_3_4_5 = "TM03.3.4.5";
        /// <summary>
        /// Готовые на передачу на предварительную экспертизу
        /// </summary>
        public const string TZ_02_2_0 = "TM02.2.0";
        /// <summary>
        /// Окончательный отказ
        /// </summary>
        public const string TZ_03_3_9_2_2 = "TM03.3.9.2.2";
        /// <summary>
        /// Преобразование заявки
        /// </summary>
        public const string TZConvert = "TMConvert";
        /// <summary>
        /// Регистрация ТЗ
        /// </summary>
        public const string TZRegistration = "TMRegistration";
        #endregion

        #region  Коды этапов для заявки НМПТ (Наименование места происхождения товара)

        /// <summary>
        /// Создание Заявки
        /// </summary>
        public const string NMPT_01_1 = "NMPT01.1";
        /// <summary>
        /// Контроль начальника УРЗиУО
        /// </summary>
        public const string NMPT_01_2 = "NMPT01.2";
        /// <summary>
        /// Контроль заместителя директора
        /// </summary>
        public const string NMPT_01_3 = "NMPT01.3";
        /// <summary>
        /// Формирование данных заявки
        /// </summary>
        public const string NMPT_02_1 = "NMPT02.1";
        /// <summary>
        /// Ввод оплат
        /// </summary>
        public const string NMPT_02_2 = "NMPT02.2";
        /// <summary>
        /// Ожидает оплату за приём и проведение экспертизы
        /// </summary>
        public const string NMPT_02_2_0 = "NMPT02.2.0";
        /// <summary>
        /// Выбор исполнителя полной экспертизы
        /// </summary>
        public const string NMPT_03_1 = "NMPT03.1";
        /// <summary>
        /// Вынесено экспертное заключение
        /// </summary>
        public const string NMPT_03_3 = "NMPT03.3";
        /// <summary>
        /// Утверждено директором
        /// </summary>
        public const string NMPT_03_4 = "NMPT03.4";
        /// <summary>
        /// Передано в МЮ РК
        /// </summary>
        public const string NMPT_03_5 = "NMPT03.5";
        /// <summary>
        /// Подготовка для передачи в ГР
        /// </summary>
        public const string NMPT_03_7 = "NMPT03.7";
        /// <summary>
        /// Направлено эксперту на исправление
        /// </summary>
        public const string NMPT_03_7_0 = "NMPT03.7.0";
        /// <summary>
        /// Готовые для передачи в ГР
        /// </summary>
        public const string NMPT_03_8 = "NMPT03.8";
        /// <summary>
        /// Возвращено с МЮ РК
        /// </summary>
        public const string NMPT_03_6 = "NMPT03.6";
        /// <summary>
        /// Создание ОД
        /// </summary>
        public const string NMPT_04_1 = "NMPT04.1";
        /// <summary>
        /// Экспертиза
        /// </summary>
        public const string NMPT_03_2 = "NMPT03.2";
        /// <summary>
        /// Направлен запрос
        /// </summary>
        public const string NMPT_03_2_1 = "NMPT03.2.1";
        /// <summary>
        /// Направлено заявителю заключение об отказе
        /// </summary>
        public const string NMPT_03_3_1 = "NMPT03.3.1";
        /// <summary>
        /// Делопроизводство прекращено
        /// </summary>
        public const string NMPT_02_2_1 = "NMPT02.2.1";
        /// <summary>
        /// На утверждение директору
        /// </summary>
        public const string NMPT_03_3_0 = "NMPT03.3.0";
        /// <summary>
        /// Продление срока
        /// </summary>
        public const string NMPT_03_2_2 = "NMPT03.2.2";
        /// <summary>
        /// Ожидание восстановления пропущенного срока
        /// </summary>
        public const string NMPT_03_2_3 = "NMPT03.2.3";
        /// <summary>
        /// Возражение на предварительный или частичный отказ
        /// </summary>
        public const string NMPT_03_3_2 = "NMPT03.3.2";
        /// <summary>
        /// Вынесено окончательное экспертное заключение
        /// </summary>
        public const string NMPT_03_3_3 = "NMPT03.3.3";
        // TODO: одинаковый этап
        /// <summary>
        /// Окончательный отказ (в связи с отсутствием возражения)
        /// </summary>
        public const string NMPT_03_3_32 = "NMPT03.3.3";
        /// <summary>
        /// Окончательный отказ
        /// </summary>
        public const string NMPT_03_3_4 = "NMPT03.3.4";
        /// <summary>
        /// Апелляционный совет
        /// </summary>
        public const string NMPT_03_3_5 = "NMPT03.3.5";
        /// <summary>
        /// Решение Апелляционного совета
        /// </summary>
        public const string NMPT_03_3_6 = "NMPT03.3.6";
        /// <summary>
        /// Отказано в регистрации
        /// </summary>
        public const string NMPT_03_3_7 = "NMPT03.3.7";
        /// <summary>
        /// направлено заявителю заключение об окончательном отказе
        /// </summary>
        public const string NMPT_03_3_4_0 = "NMPT03.3.4.0";
        /// <summary>
        /// подготовка уведомления о регистрации/ Передано на подготовку уведомления NMPT03.4.0
        /// </summary>
        public const string NMPT_03_3_6_0 = "NMPT03.4.0";
        /// <summary>
        /// Внесение изменений
        /// </summary>
        public const string NMPT_03_2_4 = "NMPT03.2.4";
        ///// <summary>
        ///// Признаны неподанными или ошибочно зарегистрированные
        ///// </summary>
        //public const string NMPT_02_3 = "NMPT02.3";
        #endregion

        #region  Коды этапов для заявки ПМ (Полезные модели)

        /// <summary>
        /// Создание Заявки
        /// </summary>
        public const string UM_01_1 = "U01.1";
        /// <summary>
        /// Формирование данных заявки (выбор исполнителя)
        /// </summary>
        public const string UMFormationPerformerChoosing = "UFormationPerformerChoosing";
        /// <summary>
        /// Формирование данных заявки
        /// </summary>
        public const string UM_02_1 = "U02.1";

        /// <summary>
        /// Ввод оплат
        /// </summary>
        public const string UM_02_2 = "U02.2";
        /// <summary>
        /// Распределение на проведение экспертизы на ПМ
        /// </summary>
        public const string UM_03_1 = "U03.1";
        /// <summary>
        /// Экспертиза заявки на выдачу патента на ПМ
        /// </summary>
        public const string UM_03_2 = "U03.2";
        /// <summary>
        /// Создание охранного документа
        /// </summary>
        public const string UM_04 = "U04";
        /// <summary>
        /// Контроль Директора
        /// </summary>
        public const string UM_01_2 = "U01.2";
        /// <summary>
        /// Контроль заместителя
        /// </summary>
        public const string UM_01_3 = "U01.3";
        /// <summary>
        /// Распределение
        /// </summary>
        public const string UM_01_4 = "U01.4";
        /// <summary>
        /// Архив
        /// </summary>
        public const string UM_05 = "U05";
        /// <summary>
        /// Вынесено экспертное заключение
        /// </summary>
        public const string UM_03_3 = "U03.3";
        /// <summary>
        /// Утверждено руководством
        /// </summary>
        public const string UM_03_4 = "U03.4";
        /// <summary>
        /// Передано в МЮ РК
        /// </summary>
        public const string UM_03_5 = "U03.5";
        /// <summary>
        /// Возвращено с МЮ РК
        /// </summary>
        public const string UM_03_6 = "U03.6";
        /// <summary>
        /// Подготовка для передачи в Госреестр
        /// </summary>
        public const string UM_03_7_1 = "U03.7.1";
        /// <summary>
        /// Готовые для передачи в Госреестр
        /// </summary>
        public const string UM_03_8 = "U03.8";
        /// <summary>
        /// Направлен запрос
        /// </summary>
        public const string UM_03_2_1 = "U03.2.1";
        /// <summary>
        /// Отозванные
        /// </summary>
        public const string UM_03_2_2 = "U03.2.2";
        /// <summary>
        /// Направлено уведомление о принятии решения
        /// </summary>
        public const string UM_03_7_0 = "U03.7.0";
        /// <summary>
        /// Преобразование заявки на изобретение
        /// </summary>
        public const string UM_03_2_4 = "U03.2.4";
        /// <summary>
        /// Готовые для передачи на экспертизу
        /// </summary>
        public const string UM_02_2_1 = "U02.2.1";
        /// <summary>
        /// Признаны неподанными
        /// </summary>
        public const string UM_03_2_3 = "U03.2.3";
        /// <summary>
        /// Ошибочно зарегистрированные ПМ
        /// </summary>
        public const string UM_03_2_5 = "U03.2.5";
        /// <summary>
        /// Делопроизводство прекращено
        /// </summary>
        public const string UM_02_2_0 = "U02.2.0";
        /// <summary>
        /// Готовые для передачи в архив
        /// </summary>
        public const string UM_02_2_3 = "U02.2.3";
        /// <summary>
        /// Отказанов в выдаче патента
        /// </summary>
        public const string UM_03_4_0 = "U03.4.0";
        /// <summary>
        /// Ожидает оплату за подачу ПМ
        /// </summary>
        public const string UM_02_2_7 = "U02.2.7";
        /// <summary>
        /// Ожидает заверенную копию конв.зачвки ПМ
        /// </summary>
        public const string UM_02_2_2_2 = "U02.2.2.2";
        // TODO: одиинаковый код этапа
        /// <summary>
        /// Ожидает срок перевода на нац.фазу ПМ
        /// </summary>
        public const string UM_02_2_32 = "U02.2.3";
        /// <summary>
        /// Ожидает перевод материалов заявки ПМ
        /// </summary>
        public const string UM_02_2_4 = "U02.2.4";
        /// <summary>
        /// Экспертиза заявки на выдачу патента (повторное по решению Апелляционного совета) ПМ
        /// </summary>
        public const string UM_02_2_5 = "U02.2.5";
        /// <summary>
        /// Продление срока 
        /// </summary>
        public const string UM_02_2_6 = "U02.2.6";
        /// <summary>
        /// Внесение изменений
        /// </summary>
        public const string UM_02_2_7_0 = "U02.2.7.0";
        /// <summary>
        /// Восстановление срока
        /// </summary>
        public const string UM_03_3_1 = "U03.3.1";
        /// <summary>
        /// Отсутствует ходатайство о досрочной публикации ПМ
        /// </summary>
        public const string UM_03_3_7 = "U03.3.7";
        /// <summary>
        /// Отсутствует оплата (до внесения изменений в Патентный закон РК)
        /// </summary>
        public const string UM_03_3_8 = "U03.3.8";
        /// <summary>
        /// Отсутствует оплата государственной пошлины
        /// </summary>
        public const string UM_03_7_2 = "U03.7.2";
        /// <summary>
        /// Отсутствует оплата, срок с правом восстановления
        /// </summary>
        public const string UM_03_7_3 = "U03.7.3";
        /// <summary>
        /// Секретка
        /// </summary>
        public const string UM_02_1_0 = "U02.1.0";
        ///// <summary>
        ///// Рассмотрение возражения на Апелляционном совете
        ///// </summary>
        //public const string UM_03_9 = "U03.9";
        /// <summary>
        /// Передано на подготовку уведомления
        /// </summary>
        public const string UM_03_6_0 = "U03.6.0";
        #endregion

        #region  Коды этапов для заявки ПО (Промышленные образцы)

        /// <summary>
        /// Создание Заявки
        /// </summary>
        public const string PO_01_1 = "PO01.1";
        /// <summary>
        /// Формирование данных заявки (выбор исполнителя)
        /// </summary>
        public const string POFormationPerformerChoosing = "POFormationPerformerChoosing";
        /// <summary>
        /// Формирование данных заявки
        /// </summary>
        public const string PO_02_1 = "PO02.1";
        /// <summary>
        /// Секретка PO02.1.0
        /// </summary>
        public const string PoSecret = "PO02.1.0";
        /// <summary>
        /// Ввод оплат
        /// </summary>
        public const string PO_02_2 = "PO02.2";
        /// <summary>
        /// Выбор исполнителя формальной экспертизы
        /// </summary>
        public const string PO_03_1 = "PO03.1";
        /// <summary>
        /// Формальная экспертиза
        /// </summary>
        public const string PO_03_2 = "PO03.2";
        /// <summary>
        /// Экспертиза по существу
        /// </summary>
        public const string PO_03_3 = "PO03.3";
        /// <summary>
        /// Создание охранного документа
        /// </summary>
        public const string PO_04 = "PO04";
        /// <summary>
        /// Контроль директора
        /// </summary>
        public const string PO_01_2 = "PO01.2";
        /// <summary>
        /// Контроль заместителя
        /// </summary>
        public const string PO_01_3 = "PO01.3";
        /// <summary>
        /// Распределение
        /// </summary>
        public const string PO_01_4 = "PO01.4";
        /// <summary>
        /// Архив
        /// </summary>
        public const string PO_05 = "PO05";
        /// <summary>
        /// Вынесено экспертное заключение
        /// </summary>
        public const string PO_03_4 = "PO03.4";
        /// <summary>
        /// Утверждено директором
        /// </summary>
        public const string PO_03_5 = "PO03.5";
        /// <summary>
        /// Передано в МЮ РК
        /// </summary>
        public const string PO_03_6 = "PO03.6";
        /// <summary>
        /// Возвращено с МЮ РК
        /// </summary>
        public const string PO_03_7 = "PO03.7";
        /// <summary>
        /// Подготовка для передачи в Госреестр
        /// </summary>
        public const string PO_03_8 = "PO03.8";
        /// <summary>
        /// Готовые для передачи в Госреестр
        /// </summary>
        public const string PO_03_9 = "PO03.9";
        /// <summary>
        /// Отозванные
        /// </summary>
        public const string PO_03_5_1 = "PO03.5.1";
        /// <summary>
        /// Признаны неподанными
        /// </summary>
        public const string PO_02_3 = "PO02.3";
        /// <summary>
        /// Направлен запрос
        /// </summary>
        public const string PO_03_2_1 = "PO03.2.1";
        /// <summary>
        /// Проведение поиска (г.Алматы) PO03.2.05
        /// </summary>
        public const string PO_03_2_04 = "PO03.2.05";
        /// <summary>
        /// Выбор исполнителя экспертизы по существу
        /// </summary>
        public const string PO_03_2_2 = "PO03.2.2";
        /// <summary>
        /// Готовые на экспертизу по существу
        /// </summary>
        public const string PO_03_2_03 = "PO03.2.03";
        /// <summary>
        /// Отсутствует оплата за выдачу
        /// </summary>
        public const string PO_03_8_0 = "PO03.8.0";
        /// <summary>
        /// Приостановление делопроизводства
        /// </summary>
        public const string PO_03_3_0 = "PO03.3.0";
        /// <summary>
        /// Частичный отказ в выдаче
        /// </summary>
        public const string PO_03_5_0 = "PO03.5.0";
        /// <summary>
        /// Подготовка на экспертизу по существу
        /// </summary>
        public const string PO_03_2_01 = "PO03.2.01";
        /// <summary>
        /// Решение об отказе в выдаче патента на ПО
        /// </summary>
        public const string PO_03_7_0 = "PO03.7.0";
        /// <summary>
        /// Отсутствует оплата за выдачу (до внесения изменений в Патентный закон РК)
        /// </summary>
        public const string PO_03_8_1 = "PO03.8.1";
        /// <summary>
        /// Ожидает оплату за подачу
        /// </summary>
        public const string PO_02_2_0 = "PO02.2.0";
        /// <summary>
        /// Ожидает заверенную копию конвенционной заявки
        /// </summary>
        public const string PO_03_3_1 = "PO03.3.1";
        /// <summary>
        /// Ожидает оплату за экспертизу по существу
        /// </summary>
        public const string PO_03_2_02 = "PO03.2.02";
        /// <summary>
        /// Продление срока
        /// </summary>
        public const string PO_03_2_1_0 = "PO03.2.1.0";
        /// <summary>
        /// Восстановление
        /// </summary>
        public const string PO_03_2_1_1 = "PO03.2.1.1";
        /// <summary>
        /// Ожидание востановления
        /// </summary>
        public const string PO_03_7_1 = "PO03.7.1";
        /// <summary>
        /// Направление заявителю экспертного заключения об отказе с выявленным аналогом
        /// </summary>
        public const string PO_03_7_0_1 = "PO03.7.0.1";
        /// <summary>
        /// Подача возражения в Апелляционный совет
        /// </summary>
        public const string PO_03_7_0_2 = "PO03.7.0.2";
        /// <summary>
        /// Рассмотрение Апелляционным совтом
        /// </summary>
        public const string PO_03_7_0_3 = "PO03.7.0.3";
        /// <summary>
        /// Готовые на передачу на формальную экспертизу
        /// </summary>
        public const string PO_03_0 = "PO03.0";
        /// <summary>
        /// Делопроизводство прекращено
        /// </summary>
        public const string PO_03_8_2 = "PO03.8.2";



        #endregion
        //todo: Удалить/переименовать/добавить коды, чтобы соответствовали последним кодам из базы
        #region  Коды этапов для заявки СД (Селекционные достижения)

        /// <summary>
        /// Создание Заявки
        /// </summary>
        public const string SA_01_1 = "SA01.1";
        /// <summary>
        /// Формирование данных заявки
        /// </summary>
        public const string SA_02_1 = "SA02.1";
        /// <summary>
        /// Формирование данных заявки (выбор исполнителя)
        /// </summary>
        public const string SAFormationPerformerChoosing = "SAFormationPerformerChoosing";
        /// <summary>
        /// Ввод оплаты
        /// </summary>
        public const string SA_02_2 = "SA02.2";
        /// <summary>
        /// Экспертиза на патентоспособность
        /// </summary>
        public const string SA_03_3 = "SA03.3";
        /// <summary>
        /// Создание охранного документа
        /// </summary>
        public const string SA_04 = "SA04";
        /// <summary>
        /// Контроль Директора
        /// </summary>
        public const string SA_01_2 = "SA01.2";
        /// <summary>
        /// Контроль заместителя
        /// </summary>
        public const string SA_01_3 = "SA01.3";
        /// <summary>
        /// Выбор исполнителя предварительной экспертизы
        /// </summary>
        public const string SA_03_1 = "SA03.1";
        /// <summary>
        /// Предварительная экспертиза SA03.2.0
        /// </summary>
        public const string SA_03_2 = "SA03.2.0";
        /// <summary>
        /// Распределение
        /// </summary>
        public const string SA_01_4 = "SA01.4";
        /// <summary>
        /// Архив
        /// </summary>
        public const string SA_05 = "SA05";
        /// <summary>
        /// Подготовка для передачи в гос.реестр
        /// </summary>
        public const string SA_03_4 = "SA03.4";
        /// <summary>
        /// Готовые для передачи в гос.реестр
        /// </summary>
        public const string SA_03_5 = "SA03.5";
        /// <summary>
        /// Признаны неподанными
        /// </summary>
        public const string SA_02_3 = "SA02.3";
        /// <summary>
        /// Запрос предварительной экспертизы SA03.3.1
        /// </summary>
        public const string SA_03_2_1 = "SA03.3.1";
        /// <summary>
        /// Готовые для передачи на предвар.экспертизу
        /// </summary>
        public const string SA_02_2_0 = "SA02.2.0";
        /// <summary>
        /// Готовые для передачи в архив
        /// </summary>
        public const string SA_02_2_2 = "SA02.2.2";
        /// <summary>
        /// Ошибочно зарегистрированы SA02.1.1
        /// </summary>
        public const string SA_02_3_1 = "SA02.1.1";
        /// <summary>
        /// Отказано в выдаче патента SA03.3.9
        /// </summary>
        public const string SA_03_4_1 = "SA03.3.9";
        /// <summary>
        /// Ожидает оплату за подачу
        /// </summary>
        public const string SA_02_2_1 = "SA02.2.1";
        /// <summary>
        /// Направлен запрос
        /// </summary>
        public const string SA_03_3_1 = "SA03.2.1";
        /// <summary>
        /// Возвращено с Госкомиссии после наименования SA03.2.2
        /// </summary>
        public const string SA_03_3_2 = "SA03.2.2";
        /// <summary>
        /// Вынесено экспертное заключение предварительной экспертизы SA03.2.3
        /// </summary>
        public const string SA_03_3_3 = "SA03.2.3";
        /// <summary>
        /// Утверждено директором SA03.2.4
        /// </summary>
        public const string SA_03_3_4 = "SA03.2.4";
        /// <summary>
        /// Передано в МЮ РК SA03.2.5
        /// </summary>
        public const string SA_03_3_5 = "SA03.2.5";
        // TODO: одиинаковый код этапа
        /// <summary>
        /// Отказано в дальнейшем рассмотрении SA03.2.5
        /// </summary>
        public const string SA_03_3_52 = "SA03.2.5";
        /// <summary>
        /// Возвращено с МЮ РК
        /// </summary>
        public const string SA_03_3_6 = "SA03.3.6";
        /// <summary>
        /// Возвращено с Госкомиссии после патентоспособности SA03.3.3
        /// </summary>
        public const string SA_03_3_7_0 = "SA03.3.3";
        /// <summary>
        /// Направлено в Госкомиссию (патентоспособность) SA03.3.2
        /// </summary>
        public const string SA_03_3_7 = "SA03.3.2";
        /// <summary>
        /// Передано в МЮ РК (заключения Госкомиссии) SA03.3.5
        /// </summary>
        public const string SA_03_3_8 = "SA03.3.5";
        /// <summary>
        /// Направлено уведомление о принятии решения SA03.3.7
        /// </summary>
        public const string SA_03_3_3_8 = "SA03.3.7";
        /// <summary>
        /// Внесение изменений SA03.3.9.0
        /// </summary>
        public const string SA_03_3_9 = "SA03.3.9.0";
        /// <summary>
        /// Отсутствует оплата (до внесения изменений в Закон РК Об охране СД) SA03.3.8
        /// </summary>
        public const string SA_03_3_10 = "SA03.3.8";
        #endregion

        #region Коды этапов для документов 

        /// <summary>
        /// Создание исходящего документа
        /// </summary>
        public const string DocumentOutgoing_01_1 = "OUT01.1";
        /// <summary>
        /// Отправка (создание почтового конверта)
        /// </summary>
        public const string DocumentOutgoing_03_1 = "OUT03.1";
        /// <summary>
        /// Контроль руководителя
        /// </summary>
        public const string DocumentOutgoing_02_1 = "OUT02.1";
        /// <summary>
        /// Контроль Начальника управления
        /// </summary>
        public const string DocumentOutgoing_02_1_1 = "OUT02.1.1";
        /// <summary>
        /// Контроль Начальника департамента
        /// </summary>
        public const string DocumentOutgoing_02_1_2 = "OUT02.1.2";
        /// <summary>
        /// Контроль заместителя
        /// </summary>
        public const string DocumentOutgoing_02_2 = "OUT02.2";

        /// <summary>
        /// Контроль заместителя
        /// </summary>
        public const string DocumentInternal_IN01_1_0 = "IN01.1.0";
        /// <summary>
        /// Контроль директора
        /// </summary>
        public const string DocumentOutgoing_02_3 = "OUT02.3";
        /// <summary>
        /// Возврат с почты
        /// </summary>
        public const string DocumentOutgoing_04_1 = "OUT04.1";
        /// <summary>
        /// Заместитель Начальника департамента
        /// </summary>
        public const string OUT_DeputyHeadOfDepartment = "OUT_DeputyHeadOfDepartment";

        /// <summary>
        /// Создание входящего документа
        /// </summary>
        public const string DocumentIncoming_1_1 = "IN1.1";
        /// <summary>
        /// Определение исполнителя
        /// </summary>
        public const string DocumentIncoming_2_1 = "IN2.1";
        /// <summary>
        /// Обработка документа
        /// </summary>
        public const string DocumentIncoming_2_2 = "IN2.2";
        /// <summary>
        /// Контроль директора
        /// </summary>
        public const string DocumentIncoming_1_2_1 = "IN1.2.1";
        /// <summary>
        /// Контроль заместителя
        /// </summary>
        public const string DocumentIncoming_1_2_2 = "IN1.2.2";
        /// <summary>
        /// Распределение по службам
        /// </summary>
        public const string DocumentIncoming_1_3 = "IN1.3";
        /// <summary>
        /// Архив
        /// </summary>
        public const string DocumentIncoming_3 = "IN3";

        /// <summary>
        /// Создание внутреннего документа
        /// </summary>
        public const string DocumentInternal_0_1 = "IN01";

        /// <summary>
        /// Контроль руководителя 
        /// </summary>
        public const string DocumentInternal_1_1 = "IN01.1";

        /// <summary>
        /// Контроль Начальника управления
        /// </summary>
        public const string DocumentInternal_1_1_4 = "IN01.1.4";

        /// <summary>
        /// Контроль Начальника департамента
        /// </summary>
        public const string DocumentInternal_1_1_5 = "IN01.1.5";

        /// <summary>
        /// Контроль директора
        /// </summary>
        public const string DocumentInternalIN01_1_1 = "IN01.1.1";

        /// <summary>
        /// Обработка документа
        /// </summary>
        public const string DocumentInternalIN01_1_3 = "IN01.1.3";

        #endregion

        #region Коды этапов для ОД

        /// <summary>
        /// Присвоение номера охранного документа
        /// </summary>
        public const string OD01_1 = "OD01.1";


        /// <summary>
        /// Параллельный этап для ОД (возможена обработка одновременно нескольких из этапов <see cref="RouteStageCodes.OD01_3"/>, <see cref="RouteStageCodes.OD01_2_2"/>, <see cref="RouteStageCodes.OD03_1"/>) 
        /// </summary>
        public const string ODParallel = "ODParallel";

        /// <summary>
        /// Печать охранного документа
        /// </summary>
        public const string OD01_3 = "OD01.3";

        /// <summary>
        /// Публикация охранного документа
        /// </summary>
        public const string OD01_6 = "OD01.6";


        /// <summary>
        /// Действующие ТЗ
        /// </summary>
        public const string OD05_01 = "OD05.01";

        /// <summary>
        /// Контроль  начальника управления
        /// </summary>
        public const string OD01_3_0 = "OD01.3.0";

        /// <summary>
        /// Контроль  директора
        /// </summary>
        public const string OD01_3_1 = "OD01.3.1";

        /// <summary>
        /// Передача патентообладателю
        /// </summary>
        public const string OD01_5 = "OD01.5";

        /// <summary>
        /// Подготовка печатного вида ОД
        /// </summary>
        public const string OD01_2_1 = "OD01.2.1";

        /// <summary>
        /// Подготовка описаний для ОД
        /// </summary>
        public const string OD01_2_2 = "OD01.2.2";

        /// <summary>
        /// Действующие ОД
        /// </summary>
        public const string OD05 = "OD05";

        /// <summary>
        /// Срок действия ОД истек
        /// </summary>
        public const string OD03_3 = "OD03.3";

        /// <summary>
        /// Аннулирование ОД
        /// </summary>
        public const string OD03_2 = "OD03.2";

        /// <summary>
        /// Продление срока действия ОД
        /// </summary>
        public const string OD04_1 = "OD04.1";

        /// <summary>
        /// Подготовка дубликата ОД 
        /// </summary>
        public const string OD04_5 = "OD04.5";

        /// <summary>
        /// Внесение изменений в ОД
        /// </summary>
        public const string OD04_6 = "OD04.6";

        /// <summary>
        /// Восстановление срока действия  ОД
        /// </summary>
        public const string OD04_2 = "OD04.2";

        /// <summary>
        /// Прекращение срока действия ОД с правомвостановления 
        /// </summary>
        public const string OD04_3 = "OD04.3";

        /// <summary>
        /// Выделение ОД
        /// </summary>
        public const string OD04_4 = "OD04.4";

        /// <summary>
        /// Подготовка и отправка уведомление формы УВО-4
        /// </summary>
        public const string OD05_02 = "OD05.02";

        /// <summary>
        /// Направлено уведомление формы УВО-4
        /// </summary>
        public const string OD05_03 = "OD05.03";

        /// <summary>
        /// Оформление на межд.регистрацию
        /// </summary>
        public const string OD08 = "OD08";

        /// <summary>
        /// Поддержание ОД
        /// </summary>
        public const string OD03_1 = "OD03.1";



        #endregion

        //todo: Добавить больше кодов, чтобы соответствовали последним кодам из базы
        #region Коды этапов для ОД свидетельства на НМПТ

        /// <summary>
        /// Присвоение номера регистрации НМПТ
        /// </summary>
        public const string PD_NMPT_AssignmentRegistrationNumber = "PD_NMPT_AssignmentRegistrationNumber";

        #endregion

        //todo: Добавить больше кодов, чтобы соответствовали последним кодам из базы
        #region Коды этапов для ОД свидетельства на ТЗ

        /// <summary>
        /// Присвоение номера регистрации ТЗ
        /// </summary>
        public const string PD_TM_AssignmentRegistrationNumber = "PD_TM_AssignmentRegistrationNumber";

        /// <summary>
        /// Печать свидетельства ТЗ
        /// </summary>
        public const string PD_TM_PrintingCertificate = "PD_TM_PrintingCertificate";

        /// <summary>
        /// Действующие ТЗ
        /// </summary>
        public const string PD_TM_Active = "PD_TM_Active";

        /// <summary>
        /// Продление срока действия
        /// </summary>
        public const string PD_TM_ActivityTermExtension = "PD_TM_ActivityTermExtension";

        /// <summary>
        /// Срок действия истек (с правом восстановления)
        /// </summary>
        public const string PD_TM_ActivityTermExpiredWithRightToRestore = "PD_TM_ActivityTermExpiredWithRightToRestore";

        /// <summary>
        /// Срок действия истек
        /// </summary>
        public const string PD_TM_ActivityTermExpired = "PD_TM_ActivityTermExpired";

        /// <summary>
        /// Внесение изменений
        /// </summary>
        public const string PD_TM_MakingChanges = "PD_TM_MakingChanges";

        /// <summary>
        /// Аннулирование
        /// </summary>
        public const string PD_TM_Nullification = "PD_TM_Nullification";

        /// <summary>
        /// Аннулирован
        /// </summary>
        public const string PD_TM_Nullified = "PD_TM_Nullified";

        /// <summary>
        /// Выдача дубликата
        /// </summary>
        public const string PD_TM_IssueDuplicate = "PD_TM_IssueDuplicate";

        /// <summary>
        /// Предоставление выписки
        /// </summary>
        public const string PD_TM_ProvisionOfWritingOut = "PD_TM_ProvisionOfWritingOut";

        /// <summary>
        /// Внесение поправок
        /// </summary>
        public const string PD_TM_MakingAmendments = "PD_TM_MakingAmendments";

        #endregion

        #region Коды для контрактов

        /// <summary>
        /// Формирование данных заявления
        /// </summary>
        public const string DK02_1 = "DK02.1";

        /// <summary>
        /// Создание заявления на регистрацию договора
        /// </summary>
        public const string DK01_1 = "DK01.1";

        /// <summary>
        /// Контроль начальника отдела
        /// </summary>
        public const string DK01_4 = "DK01.4";

        /// <summary>
        /// Ожидается выписка
        /// </summary>
        public const string DK02_2_0 = "DK02.2.0";

        /// <summary>
        /// Экспертиза по существу
        /// </summary>
        public const string DK02_4 = "DK02.4";

        /// <summary>
        /// Зачтение оплаты за экспертизу
        /// </summary>
        public const string DK02_1_0 = "DK02.1.0";

        /// <summary>
        /// Ожидается оплата экспертизы
        /// </summary>
        public const string DK02_4_0 = "DK02.4.0";

        /// <summary>
        /// Ожидается ответ на запрос
        /// </summary>
        public const string DK02_4_2 = "DK02.4.2";

        /// <summary>
        /// Контроль начальника управления
        /// </summary>
        public const string DK02_5_1 = "DK02.5.1";

        /// <summary>
        /// Контроль начальника департамента
        /// </summary>
        public const string DK02_5_3 = "DK02.5.3";

        /// <summary>
        /// Контроль заместителя директора
        /// </summary>
        public const string DK02_5_4 = "DK02.5.4";

        /// <summary>
        /// Подготовка для передачи в МЮ
        /// </summary>
        public const string DK02_7 = "DK02.7";

        /// <summary>
        /// Ожидается решение МЮ
        /// </summary>
        public const string DK02_8 = "DK02.8";

        /// <summary>
        /// Уведомление заявителя о решении
        /// </summary>
        public const string DK02_9_2 = "DK02.9.2";

        /// <summary>
        /// Зарегистрировано
        /// </summary>
        public const string DK02_9_1 = "DK02.9.1";

        /// <summary>
        /// Отказано в регистрации
        /// </summary>
        public const string DK02_9_3 = "DK02.9.3";

        /// <summary>
        /// Регистрация договора
        /// </summary>
        public const string DK02_2 = "DK02.2";

        /// <summary>
        /// Делопроизводство прекращено по просьбе заявителя
        /// </summary>
        public const string DK02_7_2 = "DK02.7.2";

        /// <summary>
        /// Делопроизводство приостановлено по просьбе заявителя
        /// </summary>
        public const string DK02_2_1 = "DK02.2.1";

        /// <summary>
        /// Контроль заместителя
        /// </summary>
        public const string DK01_3 = "DK01.3";

        /// <summary>
        /// Распределение
        /// </summary>
        public const string DK01_2 = "DK01.2";

        /// <summary>
        /// Возвращено с МЮ РК
        /// </summary>
        public const string DK02_9 = "DK02.9";

        /// <summary>
        /// Внесение данных о регистрации
        /// </summary>
        public const string DK03_00 = "DK03.00";

        /// <summary>
        /// Подготовка для публикации
        /// </summary>
        public const string DK03_01 = "DK03.01";

        /// <summary>
        /// Публикация
        /// </summary>
        public const string DK03_03 = "DK03.03";

        /// <summary>
        /// Признан не поданным
        /// </summary>
        public const string DK05_4 = "DK05.4";

        /// <summary>
        /// Зачтение оплаты за регистрацию договора
        /// </summary>
        public const string DK02_1_1 = "DK02.1.1";

        /// <summary>
        /// Внесение данных в Госреестр
        /// </summary>
        public const string DK03_2 = "DK03.2";

        /// <summary>
        /// Аннулирование
        /// </summary>
        public const string DK05_2 = "DK05.2";

        /// <summary>
        /// Расторжение
        /// </summary>
        public const string DK05_3 = "DK05.3";

        #endregion
    }
}