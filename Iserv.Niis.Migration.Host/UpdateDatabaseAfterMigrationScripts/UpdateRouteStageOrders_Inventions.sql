-- DELETE 


 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B02.1' and nextRoute.Code = 'B02.2'


 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B02.3' and nextRoute.Code = 'B02.2.1'


 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B02.2' and nextRoute.Code = 'B04.0.0.1'



 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B02.2.0.1' and nextRoute.Code = 'B04.0.0.1'



 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B02.2.0.2' and nextRoute.Code = 'B04.0.0.1'


 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B02.2.0.3' and nextRoute.Code = 'B04.0.0.1'


 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B02.2.0.1' and nextRoute.Code = 'B02.2.1'


 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B02.2.0.2' and nextRoute.Code = 'B02.2.1'


 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B02.2.0.3' and nextRoute.Code = 'B02.2.1'


 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B02.2' and nextRoute.Code in ('B02.3', 'B02.2.0.1', 'B02.2.0.2', 'B02.2.0.3')



 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.2.1' and nextRoute.Code='B02.2.0.3'



 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.2.1_0' and nextRoute.Code='B04.0'



 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.2.2.0' and nextRoute.Code in('B03.2.4_1', 'B03.2.4_0', 'B03.2.3', 'U02.2.5')




 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.2.3' and nextRoute.Code = 'B03.2.4'



 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.2.4' and nextRoute.Code = 'B03.3.1_1'


 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.3.1.1' and nextRoute.Code in ('B03.2.1')




 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.3.3' and nextRoute.Code = 'B03.3.4.1'



 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.3.5' and nextRoute.Code = 'B03.3.7.0'



 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code in ('B03.3.7.0', 'B03.3.7.2', 'B03.3.7.3', 'B03.3.7.4', 'B03.3.7.5') and nextRoute.Code = 'B03.3.8'



 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code ='B03.2.2.1' and nextRoute.Code in ('B03.2.4_0', 'B03.2.4_1', 'B03.2.3.0')


 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.3.4.1.0' and nextRoute.Code = 'B03.3.7.1'



 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B02.2.0' and nextRoute.Code = 'B04.0'


 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B02.2.0' and nextRoute.Code = 'B03.2.1.1'


 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.2.1' and nextRoute.Code = 'B02.2.0'


 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.3.1.1.1' and nextRoute.Code = 'B02.2.0'


 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.2.1_1' and nextRoute.Code = 'B03.2.2.0'


 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.3.4.1' and nextRoute.Code = 'B03.3.1.1.0'




 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.3.1_2' and nextRoute.Code = 'B03.3.1.1.0'

 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.2.4_1' and nextRoute.Code = 'B03.3.1.1.0'


 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.3.1' and nextRoute.Code = 'B03.3.1.1.0'

-- INSERT


INSERT INTO [dbo].[RouteStageOrders]
           ([CurrentStageId]
           ,[DateCreate]
           ,[DateUpdate]
           ,[IsAutomatic]
           ,[IsParallel]
           ,[IsReturn]
           ,[NextStageId]
           ,[ExternalId])
     VALUES
           ((select top 1 Id from DicRouteStages where Code='B02.1')
           ,getdate()
           ,getdate()
           ,0
           ,0
           ,0
           ,(select top 1 Id from DicRouteStages where Code='B02.3')
           ,null)


INSERT INTO [dbo].[RouteStageOrders]
           ([CurrentStageId]
           ,[DateCreate]
           ,[DateUpdate]
           ,[IsAutomatic]
           ,[IsParallel]
           ,[IsReturn]
           ,[NextStageId]
           ,[ExternalId])
     VALUES
           ((select top 1 Id from DicRouteStages where Code='B02.1')
           ,getdate()
           ,getdate()
           ,0
           ,0
           ,0
           ,(select top 1 Id from DicRouteStages where Code='B04.0.0.1')
           ,null)


 
INSERT INTO [dbo].[RouteStageOrders]
           ([CurrentStageId]
           ,[DateCreate]
           ,[DateUpdate]
           ,[IsAutomatic]
           ,[IsParallel]
           ,[IsReturn]
           ,[NextStageId]
           ,[ExternalId])
     VALUES
           ((select top 1 Id from DicRouteStages where Code='B03.3.1.1.1')
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,(select top 1 Id from DicRouteStages where Code='B03.2.1.1')
           ,null)



INSERT INTO [dbo].[RouteStageOrders]
           ([CurrentStageId]
           ,[DateCreate]
           ,[DateUpdate]
           ,[IsAutomatic]
           ,[IsParallel]
           ,[IsReturn]
           ,[NextStageId]
           ,[ExternalId])
     VALUES
           ((select top 1 Id from DicRouteStages where Code='B02.2.0')
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,(select top 1 Id from DicRouteStages where Code='B04.0')
           ,null)



INSERT INTO [dbo].[RouteStageOrders]
           ([CurrentStageId]
           ,[DateCreate]
           ,[DateUpdate]
           ,[IsAutomatic]
           ,[IsParallel]
           ,[IsReturn]
           ,[NextStageId]
           ,[ExternalId])
     VALUES
           ((select top 1 Id from DicRouteStages where Code='B04.0')
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,(select top 1 Id from DicRouteStages where Code='B03.3.1.1.1')
           ,null)



INSERT INTO [dbo].[RouteStageOrders]
           ([CurrentStageId]
           ,[DateCreate]
           ,[DateUpdate]
           ,[IsAutomatic]
           ,[IsParallel]
           ,[IsReturn]
           ,[NextStageId]
           ,[ExternalId])
     VALUES
           ((select top 1 Id from DicRouteStages where Code='B03.3.1.1.1')
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,(select top 1 Id from DicRouteStages where Code='B02.2.0')
           ,null)


INSERT INTO [dbo].[RouteStageOrders]
           ([CurrentStageId]
           ,[DateCreate]
           ,[DateUpdate]
           ,[IsAutomatic]
           ,[IsParallel]
           ,[IsReturn]
           ,[NextStageId]
           ,[ExternalId])
     VALUES
           ((select top 1 Id from DicRouteStages where Code='B03.3.4.1')
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,(select top 1 Id from DicRouteStages where Code='B03.3.1.1.0')
           ,null)


INSERT INTO [dbo].[RouteStageOrders]
           ([CurrentStageId]
           ,[DateCreate]
           ,[DateUpdate]
           ,[IsAutomatic]
           ,[IsParallel]
           ,[IsReturn]
           ,[NextStageId]
           ,[ExternalId])
     VALUES
           ((select top 1 Id from DicRouteStages where Code='B03.3.5')
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,(select top 1 Id from DicRouteStages where Code='B03.3.4.1')
           ,null)



 
INSERT INTO [dbo].[RouteStageOrders]
           ([CurrentStageId]
           ,[DateCreate]
           ,[DateUpdate]
           ,[IsAutomatic]
           ,[IsParallel]
           ,[IsReturn]
           ,[NextStageId]
           ,[ExternalId])
     VALUES
           ((select top 1 Id from DicRouteStages where Code='B03.3.4.1')
           ,getdate()
           ,getdate()
           ,0
           ,0
           ,0
           ,(select top 1 Id from DicRouteStages where Code='B03.3.4.1.0')
           ,null)



INSERT INTO [dbo].[RouteStageOrders]
           ([CurrentStageId]
           ,[DateCreate]
           ,[DateUpdate]
           ,[IsAutomatic]
           ,[IsParallel]
           ,[IsReturn]
           ,[NextStageId]
           ,[ExternalId])
     VALUES
           ((select top 1 Id from DicRouteStages where Code='B03.3.4.1.0')
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,(select top 1 Id from DicRouteStages where Code='B03.3.7.1')
           ,null)


INSERT INTO [dbo].[RouteStageOrders]
           ([CurrentStageId]
           ,[DateCreate]
           ,[DateUpdate]
           ,[IsAutomatic]
           ,[IsParallel]
           ,[IsReturn]
           ,[NextStageId]
           ,[ExternalId])
     VALUES
           ((select top 1 Id from DicRouteStages where Code='B03.3.5')
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,(select top 1 Id from DicRouteStages where Code='B03.3.7.1')
           ,null)


INSERT INTO [dbo].[RouteStageOrders]
           ([CurrentStageId]
           ,[DateCreate]
           ,[DateUpdate]
           ,[IsAutomatic]
           ,[IsParallel]
           ,[IsReturn]
           ,[NextStageId]
           ,[ExternalId])
     VALUES
           ((select top 1 Id from DicRouteStages where Code='B03.3.7.1')
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,(select top 1 Id from DicRouteStages where Code='B03.3.8')
           ,null)


INSERT INTO [dbo].[RouteStageOrders]
           ([CurrentStageId]
           ,[DateCreate]
           ,[DateUpdate]
           ,[IsAutomatic]
           ,[IsParallel]
           ,[IsReturn]
           ,[NextStageId]
           ,[ExternalId])
     VALUES
           ((select top 1 Id from DicRouteStages where Code='B04.0')
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,(select top 1 Id from DicRouteStages where Code='B03.3.8')
           ,null)



 
INSERT INTO [dbo].[RouteStageOrders]
           ([CurrentStageId]
           ,[DateCreate]
           ,[DateUpdate]
           ,[IsAutomatic]
           ,[IsParallel]
           ,[IsReturn]
           ,[NextStageId]
           ,[ExternalId])
     VALUES
           ((select top 1 Id from DicRouteStages where Code='B03.3.7.1')
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,(select top 1 Id from DicRouteStages where Code='B04.0')
           ,null)


 
INSERT INTO [dbo].[RouteStageOrders]
           ([CurrentStageId]
           ,[DateCreate]
           ,[DateUpdate]
           ,[IsAutomatic]
           ,[IsParallel]
           ,[IsReturn]
           ,[NextStageId]
           ,[ExternalId])
     VALUES
           ((select top 1 Id from DicRouteStages where Code='B03.2.3')
           ,getdate()
           ,getdate()
           ,0
           ,0
           ,0
           ,(select top 1 Id from DicRouteStages where Code='B03.2.2.0')
           ,null)



INSERT INTO [dbo].[RouteStageOrders]
           ([CurrentStageId]
           ,[DateCreate]
           ,[DateUpdate]
           ,[IsAutomatic]
           ,[IsParallel]
           ,[IsReturn]
           ,[NextStageId]
           ,[ExternalId])
     VALUES
           ((select top 1 Id from DicRouteStages where Code='B03.3.4.1.0')
           ,getdate()
           ,getdate()
           ,0
           ,0
           ,0
           ,(select top 1 Id from DicRouteStages where Code='B03.3.9')
           ,null)



INSERT INTO [dbo].[RouteStageOrders]
           ([CurrentStageId]
           ,[DateCreate]
           ,[DateUpdate]
           ,[IsAutomatic]
           ,[IsParallel]
           ,[IsReturn]
           ,[NextStageId]
           ,[ExternalId])
     VALUES
           ((select top 1 Id from DicRouteStages where Code='B03.3.4.1.0')
           ,getdate()
           ,getdate()
           ,0
           ,0
           ,0
           ,(select top 1 Id from DicRouteStages where Code='B03.2.4')
           ,null)


INSERT INTO [dbo].[RouteStageOrders]
           ([CurrentStageId]
           ,[DateCreate]
           ,[DateUpdate]
           ,[IsAutomatic]
           ,[IsParallel]
           ,[IsReturn]
           ,[NextStageId]
           ,[ExternalId])
     VALUES
           ((select top 1 Id from DicRouteStages where Code='B03.2.1_0')
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,(select top 1 Id from DicRouteStages where Code='B02.2.0')
           ,null)


INSERT INTO [dbo].[RouteStageOrders]
           ([CurrentStageId]
           ,[DateCreate]
           ,[DateUpdate]
           ,[IsAutomatic]
           ,[IsParallel]
           ,[IsReturn]
           ,[NextStageId]
           ,[ExternalId])
     VALUES
           ((select top 1 Id from DicRouteStages where Code='B02.2.0')
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,(select top 1 Id from DicRouteStages where Code='B03.2.1_1')
           ,null)


 
INSERT INTO [dbo].[RouteStageOrders]
           ([CurrentStageId]
           ,[DateCreate]
           ,[DateUpdate]
           ,[IsAutomatic]
           ,[IsParallel]
           ,[IsReturn]
           ,[NextStageId]
           ,[ExternalId])
     VALUES
           ((select top 1 Id from DicRouteStages where Code='B03.3.1.1.1')
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,(select top 1 Id from DicRouteStages where Code='B02.2.0')
           ,null)


INSERT INTO [dbo].[RouteStageOrders]
           ([CurrentStageId]
           ,[DateCreate]
           ,[DateUpdate]
           ,[IsAutomatic]
           ,[IsParallel]
           ,[IsReturn]
           ,[NextStageId]
           ,[ExternalId])
     VALUES
           ((select top 1 Id from DicRouteStages where Code='B03.3.1.1.1')
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,(select top 1 Id from DicRouteStages where Code='B03.3.1.1.0')
           ,null)



-- UPDATE

 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B02.2.0.0' and nextRoute.Code = 'B04.0.0.1'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B02.2' and nextRoute.Code = 'B02.2.1'



 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B02.2.0.0' and nextRoute.Code = 'B02.2.1'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B02.2' and nextRoute.Code = 'B02.2.0.0'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code in ('B03.3.1.1', 'B03.3.1.1.1', 'B03.3.1.1.0') and nextRoute.Code = 'B03.2.1'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code ='B03.2.1' and nextRoute.Code in ('B03.2.1.1', 'B03.3.1.1', 'B02.2.0')



 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code ='B02.2.0' and nextRoute.Code='B03.2.1.1'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code ='B03.3.1.1' and nextRoute.Code='B03.3.1.1.0'


 
 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code ='B03.3.1.1' and nextRoute.Code='B04.0'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code ='B04.0' and nextRoute.Code='B04.0.0'



 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code in ('B03.3.1.1', 'B03.3.1.1.1', 'B03.3.1.1.0') and nextRoute.Code='B03.2.4'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.2.4' and nextRoute.Code in ('B03.3.2', 'B03.3.1.1')


 
 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.3.1.1.0' and nextRoute.Code = 'B04.0'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.3.2' and nextRoute.Code = 'B03.3.2.1'



 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.3.2.1' and nextRoute.Code = 'B03.3.3'



 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.3.4' and nextRoute.Code = 'B03.3.5'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.3.1.1' and nextRoute.Code = 'B03.2.4'


 update RouteStageOrders
 set IsAutomatic = 0
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.3.4.1.0' and nextRoute.Code = 'B03.3.7.1'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.2.1.1' and nextRoute.Code = 'B03.2.1_0'




 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.2.1_0' and nextRoute.Code = 'B03.2.1_1'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.2.3' and nextRoute.Code in ('B03.2.2.1', 'B03.2.2.0')


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.2.2.1' and nextRoute.Code ='B03.2.4'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B02.2.1' and nextRoute.Code ='B02.2.1.0.0'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.2.1_1' and nextRoute.Code ='B03.2.3'


 
 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.3.8' and nextRoute.Code ='B04.1'



 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.2.1.1' and nextRoute.Code ='B03.2.1_0'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.2.2.0' and nextRoute.Code ='B03.2.3.0'


 update RouteStageOrders
 set IsAutomatic = 0
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.3.5' and nextRoute.Code ='B03.3.4.1'

 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.2.3' and nextRoute.Code = 'B03.2.2.1'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.2.3' and nextRoute.Code = 'B03.2.2.0'

 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'B03.2.3.0' and nextRoute.Code = 'B03.2.4'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'U03.1' and nextRoute.Code = 'U03.2'