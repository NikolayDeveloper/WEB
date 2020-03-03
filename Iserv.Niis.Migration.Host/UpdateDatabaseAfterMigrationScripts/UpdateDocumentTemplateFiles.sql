INSERT INTO [dbo].[DocumentTemplateFiles]
           ([DateCreate]
           ,[DateUpdate]
           ,[ExternalId]
           ,[File]
           ,[FileFingerPrint]
           ,[FileName]
           ,[FileSize]
           ,[FileType])
     VALUES
           (getdate()
           ,getdate()
           ,null
           ,null
           ,null
           ,'Справка об отсутствии секретности'
           ,-1
           ,'application/vnd.openxmlformats-officedocument.wordprocessingml.document')