INSERT INTO [dbo].[Role_RouteStage]
           ([RoleId]
           ,[StageId])
     VALUES
           ((select top 1 Id from AspNetRoles where Code='CONTRACT_EXPERT')
           ,(select top 1 Id from DicRouteStages where Code='DK02.9'))
GO



INSERT INTO [dbo].[Role_RouteStage]
           ([RoleId]
           ,[StageId])
     VALUES
           ((select top 1 Id from AspNetRoles where Code='CONTRACT_EXPERT')
           ,(select top 1 Id from DicRouteStages where Code='DK03.01'))
GO