 --МТЗ

 -- delete
 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'TMI03.3.4.1.1' and nextRoute.Code = 'TMI03.3.4.2'


 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'TMI03.3.2.0' and nextRoute.Code = 'TMI03.3.8'




 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'TMI03.3.4.5.1' and nextRoute.Code = 'TMI03.3.8'



 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'TMI03.3.9' and nextRoute.Code = 'TMI03.3.4.2'



 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'TMI03.3.4.1' and nextRoute.Code = 'TMI03.3.4.2'



 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'TMI03.3.4.4' and nextRoute.Code = 'TMI03.3.4.5'


-- insert
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
           (8876
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,8875
           ,null)
GO




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
           (15467
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,8882
           ,null)
GO




-- update

 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'TMI03.3.1'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'TMI03.3.3'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'TMI03.3.3.1'



 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'TMI03.3.4.1.0'



 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'TMI03.3.4.5.1'



 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'TMI03.3.5'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'TMI03.3.4.1.1'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'TMI03.3.9'



 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'TMI03.3.2.0'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'TMI03.3.4'

 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'TMI03.3.8'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'TMI03.3.4.2'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'TMI03.3.4.3'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'TMI03.3.4.5'


-- ПО

--delete 

 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'PO02.1' and nextRoute.Code = 'PO02.2.0'

 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'PO03.7.0' and nextRoute.Code = 'PO03.7.1'

 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'PO03.7.0.1' and nextRoute.Code = 'PO03.7.1'


 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'PO03.2' and nextRoute.Code = 'PO03.2.02'



 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'PO03.7.0.3' and nextRoute.Code = 'PO03.4'



 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'PO03.2' and nextRoute.Code = 'PO03.2.1'


 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'PO03.7.0.2' and nextRoute.Code = 'PO03.7.0.3'


 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'PO03.8.1' and nextRoute.Code = 'PO03.9'

--insert
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
           (1155
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,15514
           ,null)
GO




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
           (15432
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,15549
           ,null)
GO

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
           (15517
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,15549
           ,null)
GO







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
           (15549
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,15575
           ,null)
GO



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
           (15516
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,15575
           ,null)
GO

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
           (1161
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,15450
           ,null)
GO



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
           (15564
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,5853
           ,null)
GO


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
           (15562
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,15564
           ,null)
GO




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
           (15562
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,15575
           ,null)
GO

--update



 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'PO03.0'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'PO03.0'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'PO03.2.01'

 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'PO03.2.1'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'PO03.2.1.0'



 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'PO03.8.2'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'PO03.2.03'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'PO03.2.02'



 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'PO03.2.03'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'PO03.4'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'PO03.2.1'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'PO03.2.1.0'

 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'PO03.7.1'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'PO03.5'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'PO03.6'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'PO03.7'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'PO03.8'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'PO03.9'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'PO03.8.0'






 -- ПМ


 -- delete 

 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'U02.1' and nextRoute.Code = 'U02.2.7'


 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'U02.2.5' and nextRoute.Code = 'U03.3'



 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'U02.2.5' and nextRoute.Code = 'U02.2.6'

 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'U03.2.1' and nextRoute.Code = 'U03.2.2'

 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'U02.2.7' and nextRoute.Code = 'U03.2.2'


 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'U02.2.6' and nextRoute.Code = 'U03.2.2'



 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'U03.7.0' and nextRoute.Code = 'U03.2.2'




 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'U03.7.2' and nextRoute.Code = 'U03.2.2'



 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'U03.7.3' and nextRoute.Code = 'U03.2.2'





 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'U03.4' and nextRoute.Code = 'U03.4.0'


 -- insert


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
           (15505
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,15465
           ,null)
GO

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
           (8190
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,15511
           ,null)
GO



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
           ((select top 1 id from DicRouteStages where Code = 'U03.9')
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,5821
           ,null)
GO




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
           (15489
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,(select top 1 id from DicRouteStages where Code = 'U03.9')
           ,null)
GO


 -- update

 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'U02.2.7'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'U02.2.0'

 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'U03.3'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'U03.2.1'

 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'U03.3.1'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'U03.4'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'U03.5'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'U03.6'

 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'U03.7.1'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'U03.4.0'



-- document 

-- delete 

 delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'OUT02.1' and nextRoute.Code = 'OUT02.3'




 -- Contracts


 --insert 



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
           ((select top 1 Id from DicRouteStages where Code='DK02.9')
           ,getdate()
           ,getdate()
           ,0
           ,0
           ,0
           ,(select top 1 Id from DicRouteStages where Code='DK02.9.2')
           ,null)
GO


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
           ((select top 1 Id from DicRouteStages where Code='DK02.4.0')
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,(select top 1 Id from DicRouteStages where Code='DK02.2')
           ,null)
GO


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
           ((select top 1 Id from DicRouteStages where Code='DK02.2')
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,(select top 1 Id from DicRouteStages where Code='DK02.4')
           ,null)
GO

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
           ((select top 1 Id from DicRouteStages where Code='DK02.4')
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,(select top 1 Id from DicRouteStages where Code='DK02.2')
           ,null)
GO


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
           ((select top 1 Id from DicRouteStages where Code='DK02.5.3')
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,(select top 1 Id from DicRouteStages where Code='DK01.3')
           ,null)
GO


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
           ((select top 1 Id from DicRouteStages where Code='DK01.3')
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,(select top 1 Id from DicRouteStages where Code='DK02.7')
           ,null)
GO


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
           ((select top 1 Id from DicRouteStages where Code='DK02.8')
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,(select top 1 Id from DicRouteStages where Code='DK02.9')
           ,null)
GO



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
           ((select top 1 Id from DicRouteStages where Code='DK02.9.2')
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,(select top 1 Id from DicRouteStages where Code='DK03.00')
           ,null)
GO



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
           ((select top 1 Id from DicRouteStages where Code='DK02.9.2')
           ,getdate()
           ,getdate()
           ,1
           ,0
           ,0
           ,(select top 1 Id from DicRouteStages where Code='DK02.9.3')
           ,null)
GO

-- update 


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'DK02.4'


  update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'DK02.5.3'

 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'DK02.8'



 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where nextRoute.Code = 'DK02.9.1'


  update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages as routeStageCurrent on ro.CurrentStageId = routeStageCurrent.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where routeStageCurrent.Code='DK02.1.0' and nextRoute.Code = 'DK02.2'

  update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages as routeStageCurrent on ro.CurrentStageId = routeStageCurrent.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where routeStageCurrent.Code='DK02.1.0' and nextRoute.Code = 'DK02.4.0'


 update RouteStageOrders
 set IsAutomatic = 1
 from RouteStageOrders ro
 inner join DicRouteStages as routeStageCurrent on ro.CurrentStageId = routeStageCurrent.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where routeStageCurrent.Code='DK02.4' and nextRoute.Code = 'DK02.4.2'

-- delete 


  delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'DK02.5.3' and nextRoute.Code = 'DK02.5.4'


   delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'DK03.00' and nextRoute.Code = 'DK03.03'




  delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'DK02.2' and nextRoute.Code = 'DK02.2.0' 

   delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'DK02.8' and nextRoute.Code = 'DK02.9.2' 



   delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'DK02.2.0' 


   delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'DK02.4.0' and nextRoute.Code = 'DK02.4' 


   delete RouteStageOrders
 from RouteStageOrders ro
 inner join DicRouteStages currentRoute on ro.CurrentStageId = currentRoute.Id
 inner join DicRouteStages nextRoute on ro.NextStageId = nextRoute.Id
 where currentRoute.Code = 'DK02.2.1' or nextRoute.Code = 'DK02.2.1' 
