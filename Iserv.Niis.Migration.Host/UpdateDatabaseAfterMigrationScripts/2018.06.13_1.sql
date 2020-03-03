-- =============================================
-- Author: Azamat.Syzdyqov
-- Create date: 24.05.2016
-- Description: Права доступа на работу с бюллетенями и поддержкой ОД, пополнение спавочника Типов отправки, изменение названий для заявок, пополнен справочник типов ОД
-- =============================================

  insert into AspNetClaimConstants(DateCreate, DateUpdate, FieldName, NameEn, NameKz, NameRu, Value)
  values (GETUTCDATE(), GETUTCDATE(), 'BulletinModule', 'Доступ к модулю "Бюллетени"', 'Доступ к модулю "Бюллетени"','Доступ к модулю "Бюллетени"','bulletin.module'),
  (GETUTCDATE(), GETUTCDATE(), 'ProtectionDocSupport', 'Поддержка охранных документов', 'Поддержка охранных документов','Поддержка охранных документов','protection.docs.support')

  insert into DicSendTypes(Code, DateCreate, DateUpdate, NameRu)
  values
  ('1-1', GETUTCDATE(), GETUTCDATE(), 'Нарочно'),
  ('1-2', GETUTCDATE(), GETUTCDATE(), 'Простое почтовое отправление'),
  ('1-3', GETUTCDATE(), GETUTCDATE(), 'Заказное почтовое отправление')

  update DicRouteStages set IsLast = 0 where Code = 'OD01.2.2'

  update DicProtectionDocTypes set NameRu = 'Заявка на Товарный Знак' where code = 'TM';
  update DicProtectionDocTypes set NameRu = 'Заявка на Наименование Мест Происхождения Товаров' where code = 'NMPT';

  insert into DicProtectionDocTypes (code, DateCreate, DateUpdate, NameRu, RouteId)
  values
  ('B_PD', GETUTCDATE(), GETUTCDATE(), 'Патент на изобретение', 22),
  ('U_PD', GETUTCDATE(), GETUTCDATE(), 'Патент на Полезную Модель', 22),
  ('S2_PD', GETUTCDATE(), GETUTCDATE(), 'Патент на Промышленный Образец', 22),
  ('TM_PD', GETUTCDATE(), GETUTCDATE(), 'Свидетельство на Товарный Знак', 22),
  ('NMPT_PD', GETUTCDATE(), GETUTCDATE(), 'Свидетельство на Наименование Мест Происхождения Товаров', 22),
  ('SA_PD', GETUTCDATE(), GETUTCDATE(), 'Патент на селекционное достижение', 22)

  insert into DicProtectionDocSubTypes (code, DateCreate, DateUpdate, NameRu, TypeId)
  values
  ('01_PD', GETUTCDATE(), GETUTCDATE(), 'Евразийская заявка', (select top 1 id from DicProtectionDocTypes where Code = 'B_PD')),
  ('02_PD', GETUTCDATE(), GETUTCDATE(), 'международный', (select top 1 id from DicProtectionDocTypes where Code = 'B_PD')),
  ('03_PD', GETUTCDATE(), GETUTCDATE(), 'Ближнее зарубежъе', (select top 1 id from DicProtectionDocTypes where Code = 'B_PD')),
  ('04_PD', GETUTCDATE(), GETUTCDATE(), 'Иностранная', (select top 1 id from DicProtectionDocTypes where Code = 'B_PD')),
  ('05_PD', GETUTCDATE(), GETUTCDATE(), 'Национальная', (select top 1 id from DicProtectionDocTypes where Code = 'B_PD')),
  ('111_PD', GETUTCDATE(), GETUTCDATE(), 'Евразийский патент', (select top 1 id from DicProtectionDocTypes where Code = 'B_PD')),
  
  ('03_UsefulModel_PD', GETUTCDATE(), GETUTCDATE(), 'Ближнее зарубежъе', (select top 1 id from DicProtectionDocTypes where Code = 'U_PD')),
  ('04_UsefulModel_PD', GETUTCDATE(), GETUTCDATE(), 'Иностранная', (select top 1 id from DicProtectionDocTypes where Code = 'U_PD')),
  ('05_UsefulModel_PD', GETUTCDATE(), GETUTCDATE(), 'Национальная', (select top 1 id from DicProtectionDocTypes where Code = 'U_PD')),
  
  ('03_Industrial_Sample_PD', GETUTCDATE(), GETUTCDATE(), 'Ближнее зарубежъе', (select top 1 id from DicProtectionDocTypes where Code = 'S2_PD')),
  ('04_Industrial_Sample_PD', GETUTCDATE(), GETUTCDATE(), 'Иностранная', (select top 1 id from DicProtectionDocTypes where Code = 'S2_PD')),
  ('05_Industrial_Sample_PD', GETUTCDATE(), GETUTCDATE(), 'Национальная', (select top 1 id from DicProtectionDocTypes where Code = 'S2_PD')),
  
  ('03_Trade_Mark_PD', GETUTCDATE(), GETUTCDATE(), 'Ближнее зарубежъе', (select top 1 id from DicProtectionDocTypes where Code = 'TM_PD')),
  ('04_Trade_Mark_PD', GETUTCDATE(), GETUTCDATE(), 'Иностранная', (select top 1 id from DicProtectionDocTypes where Code = 'TM_PD')),
  ('KTM_PD', GETUTCDATE(), GETUTCDATE(), 'Коллективный товарный знак', (select top 1 id from DicProtectionDocTypes where Code = 'TM_PD')),
  ('ZTM_PD', GETUTCDATE(), GETUTCDATE(), 'Звуковой товарный знак', (select top 1 id from DicProtectionDocTypes where Code = 'TM_PD')),
  ('FTM_PD', GETUTCDATE(), GETUTCDATE(), 'Общеизвестный товарный знак', (select top 1 id from DicProtectionDocTypes where Code = 'TM_PD')),
  ('TM_PD', GETUTCDATE(), GETUTCDATE(), 'Национальный', (select top 1 id from DicProtectionDocTypes where Code = 'TM_PD')),
  
  ('03_Name_of_Origin_PD', GETUTCDATE(), GETUTCDATE(), 'Ближнее зарубежъе', (select top 1 id from DicProtectionDocTypes where Code = 'NMPT_PD')),
  ('04_Name_of_Origin_PD', GETUTCDATE(), GETUTCDATE(), 'Иностранная', (select top 1 id from DicProtectionDocTypes where Code = 'NMPT_PD')),
  ('05_Name_of_Origin_PD', GETUTCDATE(), GETUTCDATE(), 'Национальная', (select top 1 id from DicProtectionDocTypes where Code = 'NMPT_PD')),
  
  ('03_Selection_Achieve_PD', GETUTCDATE(), GETUTCDATE(), 'Ближнее зарубежъе', (select top 1 id from DicProtectionDocTypes where Code = 'SA_PD')),
  ('04_Selection_Achieve_PD', GETUTCDATE(), GETUTCDATE(), 'Иностранная', (select top 1 id from DicProtectionDocTypes where Code = 'SA_PD')),
  ('05_Selection_Achieve_PD', GETUTCDATE(), GETUTCDATE(), 'Национальная', (select top 1 id from DicProtectionDocTypes where Code = 'SA_PD')),
  ('Sort_PD', GETUTCDATE(), GETUTCDATE(), 'Сорта винограда, древесных декоративных, плодовых и лесных культур, в том числе их подвоев', (select top 1 id from DicProtectionDocTypes where Code = 'SA_PD')),
  ('06_PD', GETUTCDATE(), GETUTCDATE(), 'Растениеводство', (select top 1 id from DicProtectionDocTypes where Code = 'SA_PD')),
  ('07_PD', GETUTCDATE(), GETUTCDATE(), 'Животноводство', (select top 1 id from DicProtectionDocTypes where Code = 'SA_PD'))

  delete rso from RouteStageOrders rso
  inner join DicRouteStages drs on rso.CurrentStageId = drs.Id
  where Code in ('OD05','OD05.01') and IsAutomatic = 1

  insert into RouteStageOrders (CurrentStageId, DateCreate, DateUpdate, IsAutomatic, IsParallel, IsReturn, NextStageId)
  values
  ((select id
  from DicRouteStages
  where Code = 'OD05'), GETUTCDATE(), GETUTCDATE(), 0,0,0, (select id
  from DicRouteStages
  where Code = 'OD03.2')),
  ((select id
  from DicRouteStages
  where Code = 'OD05'), GETUTCDATE(), GETUTCDATE(), 0,0,0, (select id
  from DicRouteStages
  where Code = 'OD04.5')),
  ((select id
  from DicRouteStages
  where Code = 'OD05.01'), GETUTCDATE(), GETUTCDATE(), 0,0,0, (select id
  from DicRouteStages
  where Code = 'OD03.2')),
  ((select id
  from DicRouteStages
  where Code = 'OD05.01'), GETUTCDATE(), GETUTCDATE(), 0,0,0, (select id
  from DicRouteStages
  where Code = 'OD04.5')),
  ((select id
  from DicRouteStages
  where Code = 'OUT02.1'), GETUTCDATE(), GETUTCDATE(), 0,0,0, (select id
  from DicRouteStages
  where Code = 'OUT02.3')),
  ((select id
  from DicRouteStages
  where Code = 'OD01.2.1'), GETUTCDATE(), GETUTCDATE(), 0,0,0, (select id
  from DicRouteStages
  where Code = 'OD01.3'))

  update DicRouteStages set ProtectionDocStatusId = null where Code = 'OD03.2'
  update DicRouteStages set ProtectionDocStatusId = (select id
  from DicProtectionDocStatuses
  where Code = '03.38')
  where Code = 'OD04.3'
  update DicRouteStages set ProtectionDocStatusId = (select id
  from DicProtectionDocStatuses
  where Code = '03.37')
  where Code = 'OD04.2'
  update DicRouteStages set ProtectionDocStatusId = (select id
  from DicProtectionDocStatuses
  where Code = 'D')
  where Code in ('OD05', 'OD05.01')
  update DicRouteStages set IsAuto = 0 where Code = 'OD01.3'

  update DicDocumentTypes set NameRu = 'Уведомление УВО-5' where code = 'UVO-5'

    update DicPositions set NameRu = 'Заместитель министра юстиции Республики Казахстан' where Id = 771

	  insert into DocumentTemplateFiles(DateCreate, DateUpdate, FileName, FileType, FileSize)
  values
  (GETUTCDATE(), GETUTCDATE(), 'Приложение 366.docx', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document', -1)

  update DicDocumentTypes set TemplateFileId = 5163 where Code = '006.014.3'

  update DicTariffs set ProtectionDocTypeId = 125 where Code in('NEW_108','NEW_109','NEW_110','NEW_111','NEW_112','NEW_113','NEW_114','NEW_115','NEW_116','NEW_117','NEW_118','NEW_119','NEW_120','NEW_121','NEW_122','NEW_123')
  update DicTariffs set ProtectionDocTypeId = 126 where Code in('NEW_124','NEW_125','NEW_126','NEW_127')
  update DicTariffs set ProtectionDocTypeId = 127 where Code in('NEW_128','NEW_129','NEW_130','NEW_131','NEW_132','NEW_133','NEW_134','NEW_135','NEW_136','NEW_137','NEW_138','NEW_139','NEW_140','NEW_141')
  update DicTariffs set ProtectionDocTypeId = 130 where Code in('NEW_142','NEW_143','NEW_144','NEW_145','NEW_146','NEW_147','NEW_148','NEW_149','NEW_150','NEW_151','NEW_152','NEW_153','NEW_154','NEW_155','NEW_156','NEW_157','NEW_158','NEW_159','NEW_160','NEW_161','NEW_162','NEW_163','NEW_164','NEW_165')