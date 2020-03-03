
--update

update DicRouteStages
set ExpirationValue = 15, ExpirationType = 2
where code='TMI03.3.4.2'


update DicRouteStages
set ExpirationValue = 4, ExpirationType = 3
where code='PO02.2.0'



update DicRouteStages
set ExpirationValue = 3, ExpirationType = 3
where code='PO03.2.01'



update DicRouteStages
set ExpirationValue = 3, ExpirationType = 3
where code='PO03.2.1.0'



update DicRouteStages
set ExpirationValue = 6, ExpirationType = 3
where code='PO03.7.1'




update DicRouteStages
set ExpirationValue = 6, ExpirationType = 3
where code='PO03.2.02'


update DicRouteStages
set ExpirationValue = 10, ExpirationType = 2
where code='PO03.6'



update DicRouteStages
set ExpirationValue = 10, ExpirationType = 2
where code='PO03.7'




update DicRouteStages
set ExpirationValue = 3, ExpirationType = 3
where code='PO03.8'



update DicRouteStages
set ExpirationValue = 2, ExpirationType = 3
where code='PO03.7.0.3'


update DicRouteStages
set ExpirationValue = 12, ExpirationType = 3
where code='PO03.9'




update DicRouteStages
set ExpirationValue = 3, ExpirationType = 3
where code='PO03.8.0'


update DicRouteStages
set ExpirationValue = 6, ExpirationType = 3
where code='U02.2.6'


update DicRouteStages
set ExpirationValue = 2, ExpirationType = 3
where code='U03.9'


 update DicRouteStages
 set IsAuto = 0, IsMain = 1
 where Code='DK02.4'

 update DicRouteStages
 set IsAuto = 0, IsMain = 1, IsLast = 0
 where Code='DK02.9.2'


 update DicRouteStages
 set IsLast = 0
 where Code='DK03.03'

 update DicRouteStages
 set NameRu = 'Создание заявления на регистрацию договора'
 where Code='DK01.1'

update DicRouteStages
set NameRu = 'Контроль начальника УРЗиУО'
where Code = 'NMPT01.2'

--insert 


INSERT INTO [dbo].[DicRouteStages]
           ([Code]
           ,[DateCreate]
           ,[DateUpdate]
           ,[Description]
           ,[FinishConServiceStatusId]
           ,[Interval]
           ,[IsFirst]
           ,[IsLast]
           ,[IsMultiUser]
           ,[IsReturnable]
           ,[IsSystem]
           ,[NameEn]
           ,[NameKz]
           ,[NameRu]
           ,[OnlineRequisitionStatusId]
           ,[RouteId]
           ,[StartConServiceStatusId]
           ,[ExternalId]
           ,[IsAuto]
           ,[ExpirationType]
           ,[ExpirationValue]
           ,[IsMain]
           ,[ContractStatusId]
           ,[ProtectionDocStatusId]
           ,[RequestStatusId])
     VALUES
           ('DK02.9'
           ,getdate()
           ,getdate()
           ,null
           ,null
           ,0
           ,0
           ,0
           ,0
           ,1
           ,0
           ,null
           ,null
           ,'Возвращено с МЮ РК'
           ,838
           ,165
           ,null
           ,null
           ,1
           ,0
           ,null
           ,1
           ,null
           ,null
           ,null)

GO



INSERT INTO [dbo].[DicRouteStages]
           ([Code]
           ,[DateCreate]
           ,[DateUpdate]
           ,[Description]
           ,[FinishConServiceStatusId]
           ,[Interval]
           ,[IsFirst]
           ,[IsLast]
           ,[IsMultiUser]
           ,[IsReturnable]
           ,[IsSystem]
           ,[NameEn]
           ,[NameKz]
           ,[NameRu]
           ,[OnlineRequisitionStatusId]
           ,[RouteId]
           ,[StartConServiceStatusId]
           ,[ExternalId]
           ,[IsAuto]
           ,[ExpirationType]
           ,[ExpirationValue]
           ,[IsMain]
           ,[ContractStatusId]
           ,[ProtectionDocStatusId]
           ,[RequestStatusId])
     VALUES
           ('DK03.01'
           ,getdate()
           ,getdate()
           ,null
           ,null
           ,0
           ,0
           ,0
           ,0
           ,1
           ,0
           ,null
           ,null
           ,'Подготовка для публикации'
           ,838
           ,165
           ,null
           ,null
           ,1
           ,0
           ,null
           ,1
           ,null
           ,null
           ,null)

GO