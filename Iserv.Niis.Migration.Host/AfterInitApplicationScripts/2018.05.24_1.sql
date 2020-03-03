begin transaction

-- =============================================
-- Author: Azamat.Syzdyqov
-- Create date: 24.05.2016
-- Description: Дополнен справочник Подтипов ОД, добавлены шаблоны патентов ОД, изменены названия и суммы новых тарифов, изменения в маршрутах ОД
-- =============================================


  insert into DicProtectionDocSubTypes(code, DateCreate, DateUpdate, NameRu, TypeId)
  values ('Sort', GETDATE(), GETDATE(), 'Сорта винограда, древесных декоративных, плодовых и лесных культур, в том числе их подвоев', 6)

  update DicRouteStages set Code = 'OD04.6' where NameRu = 'Внесение изменений в ОД'

  update DicDocumentTypes set RouteId = 2 where Code in ('SD_PAT', 'SD_PAT_2', 'PAT', 'OD_PAT_IZ_RU', 'SVID_NMPT', 'SVID_TZ', 'PATPM')

  update DicDocumentTypes set RouteId = 3, ClassificationId = 3 where Code in ('PAT_AVT_PO', 'PAT_AVT_IZ', 'PAT_AVT_PM', 'PAT_AVT_CD_RASTENIE', 'PAT_AVT_CD_ZHIVOD')
  
  INSERT INTO [dbo].[DocumentTemplateFiles] ([DateCreate],[DateUpdate],[FileType],[FileName],[FileSize],[FileFingerPrint]) VALUES
(getdate(),getdate(), 'application/vnd.openxmlformats-officedocument.wordprocessingml.document' ,'Патент на изобретение.docx',-1,null),
(getdate(),getdate(), 'application/vnd.openxmlformats-officedocument.wordprocessingml.document' ,'Патент на Полезную Модель.docx',-1,null),
(getdate(),getdate(), 'application/vnd.openxmlformats-officedocument.wordprocessingml.document' ,'Патент на Промышленный образец.docx',-1,null),
(getdate(),getdate(), 'application/vnd.openxmlformats-officedocument.wordprocessingml.document' ,'Патент на СД  (животноводство).docx',-1,null),
(getdate(),getdate(), 'application/vnd.openxmlformats-officedocument.wordprocessingml.document' ,'Патент на СД  (растениеводство).docx',-1,null),
(getdate(),getdate(), 'application/vnd.openxmlformats-officedocument.wordprocessingml.document' ,'Свидетельство на НМПТ.docx',-1,null),
(getdate(),getdate(), 'application/vnd.openxmlformats-officedocument.wordprocessingml.document' ,'Свидетельство на ТЗ.docx',-1,null),

(getdate(),getdate(), 'application/vnd.openxmlformats-officedocument.wordprocessingml.document' ,'Удостоверение ИЗ.docx',-1,null),
(getdate(),getdate(), 'application/vnd.openxmlformats-officedocument.wordprocessingml.document' ,'Удостоверение ПМ.docx',-1,null),
(getdate(),getdate(), 'application/vnd.openxmlformats-officedocument.wordprocessingml.document' ,'Удостоверение ПО.docx',-1,null),
(getdate(),getdate(), 'application/vnd.openxmlformats-officedocument.wordprocessingml.document' ,'Удостоверение СДЖ.docx',-1,null),
(getdate(),getdate(), 'application/vnd.openxmlformats-officedocument.wordprocessingml.document' ,'Удостоверение СДР.docx',-1,null)

  update DicDocumentTypes set TemplateFileId = 5148 where Code = 'OD_PAT_IZ_RU'
  update DicDocumentTypes set TemplateFileId = 5149 where Code = 'PATPM'
  update DicDocumentTypes set TemplateFileId = 5150 where Code = 'PAT'
  update DicDocumentTypes set TemplateFileId = 5151 where Code = 'SD_PAT_2'
  update DicDocumentTypes set TemplateFileId = 5152 where Code = 'SD_PAT'
  update DicDocumentTypes set TemplateFileId = 5153 where Code = 'SVID_NMPT'
  update DicDocumentTypes set TemplateFileId = 5154 where Code = 'SVID_TZ'

  update DicDocumentTypes set TemplateFileId = 5157 where Code = 'PAT_AVT_PO'
  update DicDocumentTypes set TemplateFileId = 5155 where Code = 'PAT_AVT_IZ'
  update DicDocumentTypes set TemplateFileId = 5156 where Code = 'PAT_AVT_PM'
  update DicDocumentTypes set TemplateFileId = 5159 where Code = 'PAT_AVT_CD_RASTENIE'
  update DicDocumentTypes set TemplateFileId = 5158 where Code = 'PAT_AVT_CD_ZHIVOD'

  update DicDocumentTypes set TemplateFileId = 1 where Code = 'OP_PAT'
  update DicDocumentTypes set TemplateFileId = 1 where Code = 'OP_PAT_PM'
  update DicDocumentTypes set TemplateFileId = 1 where Code = 'OP_PAT_SA'
  update DicDocumentTypes set TemplateFileId = 1 where Code = 'OP_PAT_KZ'
  update DicDocumentTypes set TemplateFileId = 1 where Code = 'OP_PAT_PM_KZ'

   insert into RouteStageOrders(CurrentStageId, DateCreate, DateUpdate, IsAutomatic, IsParallel, IsReturn, NextStageId) values
  (1573, GETDATE(), GETDATE(), 0,0,0,2959)

  delete ro
  from RouteStageOrders ro
  inner join DicRouteStages ds on ro.CurrentStageId = ds.Id
  where ds.Code = 'OD01.2.1'

  delete ro
  from RouteStageOrders ro
  inner join DicRouteStages ds on ro.CurrentStageId = ds.Id
  where ds.Code = 'OD01.3'

  delete ro
  from RouteStageOrders ro
  inner join DicRouteStages ds on ro.CurrentStageId = ds.Id
  where ds.Code = 'OD01.5'

  delete ro
  from RouteStageOrders ro
  inner join DicRouteStages ds on ro.CurrentStageId = ds.Id
  where ds.Code = 'OD04.2'

  update DicRouteStages set IsAuto = 1 where Code = 'OD04.2'

  delete ro
  from RouteStageOrders ro
  inner join DicRouteStages ds on ro.CurrentStageId = ds.Id
  where ds.Code = 'OD04.1'

  update DicRouteStages set IsAuto = 1 where Code = 'OD04.1'

  delete ro
  from RouteStageOrders ro
  inner join DicRouteStages ds on ro.CurrentStageId = ds.Id
  where ds.Code = 'OD05.02'

  delete ro
  from RouteStageOrders ro
  inner join DicRouteStages ds on ro.CurrentStageId = ds.Id
  where ds.Code = 'OD04.6'

  delete ro
  from RouteStageOrders ro
  inner join DicRouteStages ds on ro.CurrentStageId = ds.Id
  where ds.Code = 'OD04.4'

  update DicRouteStages set IsAuto = 1 where Code = 'OD05.02'

  delete ro
  from RouteStageOrders ro
  inner join DicRouteStages ds on ro.CurrentStageId = ds.Id
  where ds.Code = 'OD05.03'

  update DicRouteStages set IsAuto = 1 where Code = 'OD05.03'

  insert into RouteStageOrders (CurrentStageId, DateCreate, DateUpdate, IsAutomatic, IsParallel, IsReturn, NextStageId)
  values (2962, GETUTCDATE(), GETUTCDATE(), 1, 0,0,8455),
  (2962, GETUTCDATE(), GETUTCDATE(), 1, 0,0,12910)

  update DicRouteStages set IsAuto = 0 where code = 'OD01.5.1'

  delete ro
  from RouteStageOrders ro
  inner join DicRouteStages drs on ro.CurrentStageId = drs.Id
  inner join DicRouteStages ns on ro.NextStageId = ns.Id
  where drs.Code = 'OD05' and ns.Code not in ('OD04.6')

  delete ro
  from RouteStageOrders ro
  inner join DicRouteStages drs on ro.CurrentStageId = drs.Id
  inner join DicRouteStages ns on ro.NextStageId = ns.Id
  where drs.Code = 'OD05.01' and ns.Code not in ('OD04.4','OD04.6')
  
  insert into DicDocumentTypes(ClassificationId, Code, DateCreate, DateUpdate, NameRu, RouteId)
  values(1, '006.02.01', GETDATE(), GETDATE(), 'Судебные документы',1),
  (1, 'PetitionForInvalidation', GETDATE(), GETDATE(), 'Ходатайство о прекращении срока действия',1)

    update DicDocumentTypes set RouteId = 3, ClassificationId = 3 where Code = 'POL_1'

	update DicDocumentTypes set RouteId = 2, ClassificationId = 2 where Code = 'POL_2'

	update DicDocumentTypes set TemplateFileId = 366 where code = 'POL_2'

	update DicRouteStages set ProtectionDocStatusId = 661 where code = 'OD03.2'
		
	insert into RouteStageOrders (CurrentStageId, DateCreate, DateUpdate, IsAutomatic, IsParallel, IsReturn, NextStageId)
  values (5694, GETDATE(), GETDATE(), 0, 0, 0, 805),
  (2962, GETDATE(), GETDATE(), 1, 0, 0, 8455),
  (2962, GETDATE(), GETDATE(), 1, 0, 0, 12910),
  (15449, GETDATE(), GETDATE(), 1, 0, 0, 8455),
  (15449, GETDATE(), GETDATE(), 1, 0, 0, 12910)

	update DicTariffs set PriceBeneficiary = ROUND(PriceBeneficiary/112*100, 0), PriceBusiness = ROUND(PriceBusiness/112*100, 0), PriceFl = ROUND(PriceFl/112*100, 0), PriceUl = ROUND(PriceUl/112*100, 0) where Code != 'NEW_GOS_18' and Code like 'new_%'

	  update DicTariffs set NameRu = N'За поддержание патента на изобретение (Первый-Третий)', NameKz = N'Өнертабыс патентін күшінде сақтау үшін (Бірінші-Үшінші)' where Code = 'NEW_108';
  update DicTariffs set NameRu = N'За поддержание патента на изобретение (Четвертый-Пятый)', NameKz = N'Өнертабыс патентін күшінде сақтау үшін (Төртінші-Бесінші)' where Code = 'NEW_109';
  update DicTariffs set NameRu = N'За поддержание патента на изобретение (Шестой-Седьмой)', NameKz = N'Өнертабыс патентін күшінде сақтау үшін (Алтыншы-Жетінші)' where Code = 'NEW_110';
  update DicTariffs set NameRu = N'За поддержание патента на изобретение (Восьмой-Десятый)', NameKz = N'Өнертабыс патентін күшінде сақтау үшін (Сегізінші-Оныншы)' where Code = 'NEW_111';
  update DicTariffs set NameRu = N'За поддержание патента на изобретение (Одиннадцатый-Двенадцатый)', NameKz = N'Өнертабыс патентін күшінде сақтау үшін (Он бірінші-Он екінші)' where Code = 'NEW_112';
  update DicTariffs set NameRu = N'За поддержание патента на изобретение (Тринадцатый-Пятнадцатый)', NameKz = N'Өнертабыс патентін күшінде сақтау үшін (Он үшінші-Он бесінші)' where Code = 'NEW_113';
  update DicTariffs set NameRu = N'За поддержание патента на изобретение (Шестнадцатый-Восемнадцатый)', NameKz = N'Өнертабыс патентін күшінде сақтау үшін (Он алтыншы-Он сегізінші)' where Code = 'NEW_114';
  update DicTariffs set NameRu = N'За поддержание патента на изобретение (Девятнадцатый-Двадцать пятый)', NameKz = N'Өнертабыс патентін күшінде сақтау үшін (Он тоғызыншы-Жиырма бесінші)' where Code = 'NEW_115';

  update DicTariffs set NameRu = N'За поддержание патента на изобретение (Первый-Третий, после установленного срока, но не позднее шести месяцев со дня его истечения)', NameKz = N'Өнертабыс патентін күшінде сақтау үшін (Бірінші-Үшінші, бекітілген мерзімінен кейін, бірақ оның өтуінен алты айдан кеш емес)' where Code = 'NEW_116';
  update DicTariffs set NameRu = N'За поддержание патента на изобретение (Четвертый-Пятый, после установленного срока, но не позднее шести месяцев со дня его истечения)', NameKz = N'Өнертабыс патентін күшінде сақтау үшін (Төртінші-Бесінші, бекітілген мерзімінен кейін, бірақ оның өтуінен алты айдан кеш емес)' where Code = 'NEW_117';
  update DicTariffs set NameRu = N'За поддержание патента на изобретение (Шестой-Седьмой, после установленного срока, но не позднее шести месяцев со дня его истечения)', NameKz = N'Өнертабыс патентін күшінде сақтау үшін (Алтыншы-Жетінші, бекітілген мерзімінен кейін, бірақ оның өтуінен алты айдан кеш емес)' where Code = 'NEW_118';
  update DicTariffs set NameRu = N'За поддержание патента на изобретение (Восьмой-Десятый, после установленного срока, но не позднее шести месяцев со дня его истечения)', NameKz = N'Өнертабыс патентін күшінде сақтау үшін (Сегізінші-Оныншы), бекітілген мерзімінен кейін, бірақ оның өтуінен алты айдан кеш емес' where Code = 'NEW_119';
  update DicTariffs set NameRu = N'За поддержание патента на изобретение (Одиннадцатый-Двенадцатый, после установленного срока, но не позднее шести месяцев со дня его истечения)', NameKz = N'Өнертабыс патентін күшінде сақтау үшін (Он бірінші-Он екінші), бекітілген мерзімінен кейін, бірақ оның өтуінен алты айдан кеш емес' where Code = 'NEW_120';
  update DicTariffs set NameRu = N'За поддержание патента на изобретение (Тринадцатый-Пятнадцатый, после установленного срока, но не позднее шести месяцев со дня его истечения)', NameKz = N'Өнертабыс патентін күшінде сақтау үшін (Он үшінші-Он бесінші), бекітілген мерзімінен кейін, бірақ оның өтуінен алты айдан кеш емес' where Code = 'NEW_121';
  update DicTariffs set NameRu = N'За поддержание патента на изобретение (Шестнадцатый-Восемнадцатый, после установленного срока, но не позднее шести месяцев со дня его истечения)', NameKz = N'Өнертабыс патентін күшінде сақтау үшін (Он алтыншы-Он сегізінші), бекітілген мерзімінен кейін, бірақ оның өтуінен алты айдан кеш емес' where Code = 'NEW_122';
  update DicTariffs set NameRu = N'За поддержание патента на изобретение (Девятнадцатый-Двадцать пятый, после установленного срока, но не позднее шести месяцев со дня его истечения)', NameKz = N'Өнертабыс патентін күшінде сақтау үшін (Он тоғызыншы-Жиырма бесінші), бекітілген мерзімінен кейін, бірақ оның өтуінен алты айдан кеш емес' where Code = 'NEW_123';

  update DicTariffs set NameRu = N'За поддержание патента на полезную модель (Первый-Третий)', NameKz = N'Пайдалы модель патентін күшінде сақтау үшін (Бірінші-Үшінші)' where Code = 'NEW_124';
  update DicTariffs set NameRu = N'За поддержание патента на полезную модель (Четвертый-Восьмой)', NameKz = N'Пайдалы модель патентін күшінде сақтау үшін (Төртінші-Сегізінші)' where Code = 'NEW_125';

  update DicTariffs set NameRu = N'За поддержание патента на полезную модель (Первый-Третий, после установленного срока, но не позднее шести месяцев со дня его истечения)', NameKz = N'Пайдалы модель патентін күшінде сақтау үшін (Бірінші-Үшінші, бекітілген мерзімінен кейін, бірақ оның өтуінен алты айдан кеш емес)' where Code = 'NEW_126';
  update DicTariffs set NameRu = N'За поддержание патента на полезную модель (Четвертый-Восьмой, после установленного срока, но не позднее шести месяцев со дня его истечения)', NameKz = N'Пайдалы модель патентін күшінде сақтау үшін (Төртінші-Сегізінші, бекітілген мерзімінен кейін, бірақ оның өтуінен алты айдан кеш емес)' where Code = 'NEW_127';

  update DicTariffs set NameRu = N'За поддержание патента на промышленный образец (Первый-Третий)', NameKz = N'Өнеркәсіп үлгісі патентін күшінде сақтау үшін (Бірінші-Үшінші)' where Code = 'NEW_128';
  update DicTariffs set NameRu = N'За поддержание патента на промышленный образец (Четвертый-Пятый)', NameKz = N'Өнеркәсіп үлгісі патентін күшінде сақтау үшін (Төртінші-Бесінші)' where Code = 'NEW_129';
  update DicTariffs set NameRu = N'За поддержание патента на промышленный образец (Шестой-Седьмой)', NameKz = N'Өнеркәсіп үлгісі патентін күшінде сақтау үшін (Алтыншы-Жетінші)' where Code = 'NEW_130';
  update DicTariffs set NameRu = N'За поддержание патента на промышленный образец (Восьмой-Девятый)', NameKz = N'Өнеркәсіп үлгісі патентін күшінде сақтау үшін (Сегізінші-Тоғызыншы)' where Code = 'NEW_131';
  update DicTariffs set NameRu = N'За поддержание патента на промышленный образец (Десятый)', NameKz = N'Өнеркәсіп үлгісі патентін күшінде сақтау үшін (Оныншы)' where Code = 'NEW_132';
  update DicTariffs set NameRu = N'За поддержание патента на промышленный образец (Одиннадцатый-Пятнадцатый)', NameKz = N'Өнеркәсіп үлгісі патентін күшінде сақтау үшін (Он бірінші-Он бесінші)' where Code = 'NEW_133';
  update DicTariffs set NameRu = N'За поддержание патента на промышленный образец (Шестнадцатый-Двадцатый)', NameKz = N'Өнеркәсіп үлгісі патентін күшінде сақтау үшін (Он алтыншы-Жиырмасыншы)' where Code = 'NEW_134';

  update DicTariffs set NameRu = N'За поддержание патента на промышленный образец (Первый-Третий, после установленного срока, но не позднее шести месяцев со дня его истечения)', NameKz = N'Өнеркәсіп үлгісі патентін күшінде сақтау үшін (Бірінші-Үшінші, бекітілген мерзімінен кейін, бірақ оның өтуінен алты айдан кеш емес)' where Code = 'NEW_135';
  update DicTariffs set NameRu = N'За поддержание патента на промышленный образец (Четвертый-Пятый, после установленного срока, но не позднее шести месяцев со дня его истечения)', NameKz = N'Өнеркәсіп үлгісі патентін күшінде сақтау үшін (Төртінші-Бесінші, бекітілген мерзімінен кейін, бірақ оның өтуінен алты айдан кеш емес)' where Code = 'NEW_136';
  update DicTariffs set NameRu = N'За поддержание патента на промышленный образец (Шестой-Седьмой, после установленного срока, но не позднее шести месяцев со дня его истечения)', NameKz = N'Өнеркәсіп үлгісі патентін күшінде сақтау үшін (Алтыншы-Жетінші, бекітілген мерзімінен кейін, бірақ оның өтуінен алты айдан кеш емес)' where Code = 'NEW_137';
  update DicTariffs set NameRu = N'За поддержание патента на промышленный образец (Восьмой-Девятый, после установленного срока, но не позднее шести месяцев со дня его истечения)', NameKz = N'Өнеркәсіп үлгісі патентін күшінде сақтау үшін (Сегізінші-Тоғызыншы), бекітілген мерзімінен кейін, бірақ оның өтуінен алты айдан кеш емес' where Code = 'NEW_138';
  update DicTariffs set NameRu = N'За поддержание патента на промышленный образец (Десятый, после установленного срока, но не позднее шести месяцев со дня его истечения)', NameKz = N'Өнеркәсіп үлгісі патентін күшінде сақтау үшін (Оныншы), бекітілген мерзімінен кейін, бірақ оның өтуінен алты айдан кеш емес' where Code = 'NEW_139';
  update DicTariffs set NameRu = N'За поддержание патента на промышленный образец (Одиннадцатый-Пятнадцатый, после установленного срока, но не позднее шести месяцев со дня его истечения)', NameKz = N'Өнеркәсіп үлгісі патентін күшінде сақтау үшін (Он бірінші-Он бесінші), бекітілген мерзімінен кейін, бірақ оның өтуінен алты айдан кеш емес' where Code = 'NEW_140';
  update DicTariffs set NameRu = N'За поддержание патента на промышленный образец (Шестнадцатый-Двадцатый, после установленного срока, но не позднее шести месяцев со дня его истечения)', NameKz = N'Өнеркәсіп үлгісі патентін күшінде сақтау үшін (Он алтыншы-Жиырмасыншы), бекітілген мерзімінен кейін, бірақ оның өтуінен алты айдан кеш емес' where Code = 'NEW_141';

  update DicTariffs set NameRu = N'За поддержание патента на селекционное достижение (Первый-Третий)', NameKz = N'Селекциялік жетістігі патентін күшінде сақтау үшін (Бірінші-Үшінші)' where Code = 'NEW_142';
  update DicTariffs set NameRu = N'За поддержание патента на селекционное достижение (Четвертый-Пятый)', NameKz = N'Селекциялік жетістігі патентін күшінде сақтау үшін (Төртінші-Бесінші)' where Code = 'NEW_143';
  update DicTariffs set NameRu = N'За поддержание патента на селекционное достижение (Шестой-Седьмой)', NameKz = N'Селекциялік жетістігі патентін күшінде сақтау үшін (Алтыншы-Жетінші)' where Code = 'NEW_144';
  update DicTariffs set NameRu = N'За поддержание патента на селекционное достижение (Восьмой-Десятый)', NameKz = N'Селекциялік жетістігі патентін күшінде сақтау үшін (Сегізінші-Оныншы)' where Code = 'NEW_145';
  update DicTariffs set NameRu = N'За поддержание патента на селекционное достижение (Одиннадцатый-Двенадцатый)', NameKz = N'Селекциялік жетістігі патентін күшінде сақтау үшін (Он бірінші-Он екінші)' where Code = 'NEW_146';
  update DicTariffs set NameRu = N'За поддержание патента на селекционное достижение (Тринадцатый-Пятнадцатый)', NameKz = N'Селекциялік жетістігі патентін күшінде сақтау үшін (Он үшінші-Он бесінші)' where Code = 'NEW_147';
  update DicTariffs set NameRu = N'За поддержание патента на селекционное достижение (Шестнадцатый-Восемнадцатый)', NameKz = N'Селекциялік жетістігі патентін күшінде сақтау үшін (Он алтыншы-Он сегізінші)' where Code = 'NEW_148';
  update DicTariffs set NameRu = N'За поддержание патента на селекционное достижение (Девятнадцатый-Двадцать пятый)', NameKz = N'Селекциялік жетістігі патентін күшінде сақтау үшін (Он тоғызыншы-Жиырма бесінші)' where Code = 'NEW_149';
  update DicTariffs set NameRu = N'За поддержание патента на селекционное достижение (Двадцать шестой-Тридцатый)', NameKz = N'Селекциялік жетістігі патентін күшінде сақтау үшін (Жиырма алтыншы-Отызыншы)' where Code = 'NEW_150';
  update DicTariffs set NameRu = N'За поддержание патента на селекционное достижение (Тридцать первый-Тридцать пятый)', NameKz = N'Селекциялік жетістігі патентін күшінде сақтау үшін (Отыз бірінші-Отыз бесінші)' where Code = 'NEW_151';
  update DicTariffs set NameRu = N'За поддержание патента на селекционное достижение (Тридцать шестой-Сороковой)', NameKz = N'Селекциялік жетістігі патентін күшінде сақтау үшін (Отыз алтыншы-Қырықыншы)' where Code = 'NEW_152';
  update DicTariffs set NameRu = N'За поддержание патента на селекционное достижение (Сорок первый-Сорок пятый)', NameKz = N'Селекциялік жетістігі патентін күшінде сақтау үшін (Қырық бірінші-Қырық бесінші)' where Code = 'NEW_153';

  update DicTariffs set NameRu = N'За поддержание патента на селекционное достижение (Первый-Третий, после установленного срока, но не позднее шести месяцев со дня его истечения)', NameKz = N'Селекциялік жетістігі патентін күшінде сақтау үшін (Бірінші-Үшінші, бекітілген мерзімінен кейін, бірақ оның өтуінен алты айдан кеш емес)' where Code = 'NEW_154';
  update DicTariffs set NameRu = N'За поддержание патента на селекционное достижение (Четвертый-Пятый, после установленного срока, но не позднее шести месяцев со дня его истечения)', NameKz = N'Селекциялік жетістігі патентін күшінде сақтау үшін (Төртінші-Бесінші, бекітілген мерзімінен кейін, бірақ оның өтуінен алты айдан кеш емес)' where Code = 'NEW_155';
  update DicTariffs set NameRu = N'За поддержание патента на селекционное достижение (Шестой-Седьмой, после установленного срока, но не позднее шести месяцев со дня его истечения)', NameKz = N'Селекциялік жетістігі патентін күшінде сақтау үшін (Алтыншы-Жетінші, бекітілген мерзімінен кейін, бірақ оның өтуінен алты айдан кеш емес)' where Code = 'NEW_156';
  update DicTariffs set NameRu = N'За поддержание патента на селекционное достижение (Восьмой-Десятый, после установленного срока, но не позднее шести месяцев со дня его истечения)', NameKz = N'Селекциялік жетістігі патентін күшінде сақтау үшін (Сегізінші-Оныншы), бекітілген мерзімінен кейін, бірақ оның өтуінен алты айдан кеш емес' where Code = 'NEW_157';
  update DicTariffs set NameRu = N'За поддержание патента на селекционное достижение (Одиннадцатый-Двенадцатый, после установленного срока, но не позднее шести месяцев со дня его истечения)', NameKz = N'Селекциялік жетістігі патентін күшінде сақтау үшін (Он бірінші-Он екінші), бекітілген мерзімінен кейін, бірақ оның өтуінен алты айдан кеш емес' where Code = 'NEW_158';
  update DicTariffs set NameRu = N'За поддержание патента на селекционное достижение (Тринадцатый-Пятнадцатый, после установленного срока, но не позднее шести месяцев со дня его истечения)', NameKz = N'Селекциялік жетістігі патентін күшінде сақтау үшін (Он үшінші-Он бесінші), бекітілген мерзімінен кейін, бірақ оның өтуінен алты айдан кеш емес' where Code = 'NEW_159';
  update DicTariffs set NameRu = N'За поддержание патента на селекционное достижение (Шестнадцатый-Восемнадцатый, после установленного срока, но не позднее шести месяцев со дня его истечения)', NameKz = N'Селекциялік жетістігі патентін күшінде сақтау үшін (Он алтыншы-Он сегізінші, бекітілген мерзімінен кейін, бірақ оның өтуінен алты айдан кеш емес)' where Code = 'NEW_160';
  update DicTariffs set NameRu = N'За поддержание патента на селекционное достижение (Девятнадцатый-Двадцать пятый, после установленного срока, но не позднее шести месяцев со дня его истечения)', NameKz = N'Селекциялік жетістігі патентін күшінде сақтау үшін (Он тоғызыншы-Жиырма бесінші, бекітілген мерзімінен кейін, бірақ оның өтуінен алты айдан кеш емес)' where Code = 'NEW_161';
  update DicTariffs set NameRu = N'За поддержание патента на селекционное достижение (Двадцать шестой-Тридцатый, после установленного срока, но не позднее шести месяцев со дня его истечения)', NameKz = N'Селекциялік жетістігі патентін күшінде сақтау үшін (Жиырма алтыншы-Отызыншы, бекітілген мерзімінен кейін, бірақ оның өтуінен алты айдан кеш емес)' where Code = 'NEW_162';
  update DicTariffs set NameRu = N'За поддержание патента на селекционное достижение (Тридцать первый-Тридцать пятый, после установленного срока, но не позднее шести месяцев со дня его истечения)', NameKz = N'Селекциялік жетістігі патентін күшінде сақтау үшін (Отыз бірінші-Отыз бесінші), бекітілген мерзімінен кейін, бірақ оның өтуінен алты айдан кеш емес' where Code = 'NEW_163';
  update DicTariffs set NameRu = N'За поддержание патента на селекционное достижение (Тридцать шестой-Сороковой, после установленного срока, но не позднее шести месяцев со дня его истечения)', NameKz = N'Селекциялік жетістігі патентін күшінде сақтау үшін (Отыз алтыншы-Қырықыншы), бекітілген мерзімінен кейін, бірақ оның өтуінен алты айдан кеш емес' where Code = 'NEW_164';
  update DicTariffs set NameRu = N'За поддержание патента на селекционное достижение (Сорок первый-Сорок пятый, после установленного срока, но не позднее шести месяцев со дня его истечения)', NameKz = N'Селекциялік жетістігі патентін күшінде сақтау үшін (Қырық бірінші-Қырық бесінші), бекітілген мерзімінен кейін, бірақ оның өтуінен алты айдан кеш емес' where Code = 'NEW_165';

rollback transaction