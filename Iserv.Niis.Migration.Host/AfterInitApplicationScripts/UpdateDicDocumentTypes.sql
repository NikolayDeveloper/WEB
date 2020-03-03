-- update 
  update DicDocumentTypes
  set TemplateFileId = 1 -- было 5145
  where Code = '006.014.1.DK'

  update DicDocumentTypes
  set TemplateFileId = 1 -- было NULL
  where Code = 'DK_SEND_CONTRACT_RECALL'


  update DicDocumentTypes
  set TemplateFileId = 1 -- было NULL
  where Code = 'DK_SEND_CONTRACT_DELAY'


update DicDocumentTypes
set ClassificationId = (select top 1 Id from DicDocumentClassifications where Code = '03'),
RouteId = (select top 1 Id from DicRoutes where Code = 'DK')
where Code='001.006'

update DicDocumentTypes
set TemplateFileId = 1 -- было 204
where code='IZ_POISK'


update DicDocumentTypes
set ClassificationId = (select top 1 Id from DicDocumentClassifications where Code = '03.01')
where Code='IZ_POISK'


-- insert


INSERT INTO [dbo].[DicDocumentTypes]
           ([ClassificationId]
           ,[Code]
           ,[ConServiceTypeCode]
           ,[DateCreate]
           ,[DateUpdate]
           ,[Description]
           ,[Interval]
           ,[IsRequireSigning]
           ,[IsSendByEmail]
           ,[IsUnique]
           ,[NameEn]
           ,[NameKz]
           ,[NameRu]
           ,[Order]
           ,[RouteId]
           ,[TemplateFileId]
           ,[ExternalId])
     VALUES
           ((select top 1 Id from DicDocumentClassifications where Code = '03')
           ,'DK_Registry_Transfer'
           ,null
           ,getdate()
           ,getdate()
           ,null
           ,null
           ,null
           ,null
           ,0
           ,null
           ,null
           ,'Реестр для передачи сведений по договорам'
           ,1000
           ,(select top 1 Id from DicRoutes where Code = 'DK')
           ,null -- TODO: TemplateFileId - позже добавить 
           ,null)



INSERT INTO [dbo].[DicDocumentTypes]
           ([ClassificationId]
           ,[Code]
           ,[ConServiceTypeCode]
           ,[DateCreate]
           ,[DateUpdate]
           ,[Description]
           ,[Interval]
           ,[IsRequireSigning]
           ,[IsSendByEmail]
           ,[IsUnique]
           ,[NameEn]
           ,[NameKz]
           ,[NameRu]
           ,[Order]
           ,[RouteId]
           ,[TemplateFileId]
           ,[ExternalId])
     VALUES
           ((select top 1 Id from DicDocumentClassifications where Code = '02')
           ,'Сertificate_Of_Confidentiality'
           ,null
           ,getdate()
           ,getdate()
           ,null
           ,null
           ,null
           ,null
           ,0
           ,null
           ,null
           ,'Справка об отсутствии секретности'
           ,1000
           ,(select top 1 Id from DicRoutes where Code = 'OUT')
           ,(select top 1 Id from DocumentTemplateFiles where [FileName] ='Справка об отсутствии секретности.docx')
           ,null)

INSERT INTO [dbo].[DicDocumentTypes]
           ([ClassificationId]
           ,[Code]
           ,[ConServiceTypeCode]
           ,[DateCreate]
           ,[DateUpdate]
           ,[Description]
           ,[Interval]
           ,[IsRequireSigning]
           ,[IsSendByEmail]
           ,[IsUnique]
           ,[NameEn]
           ,[NameKz]
           ,[NameRu]
           ,[Order]
           ,[RouteId]
           ,[TemplateFileId]
           ,[ExternalId])
     VALUES
           ((select top 1 Id from DicDocumentClassifications where Code = '03')
           ,'Result_Distribution_Requests'
           ,null
           ,getdate()
           ,getdate()
           ,null
           ,null
           ,null
           ,null
           ,0
           ,null
           ,null
           ,'Результат распределения заявок'
           ,1000
           ,(select top 1 Id from DicRoutes where Code = 'W')
           ,null -- TODO: TemplateFileId - позже добавить 
           ,null)



update DicDocumentTypes set NameRu = 'Ходатайство о преобразовании ИЗ в ПМ или ПМ в ИЗ' where Code = '001.004G_1'
