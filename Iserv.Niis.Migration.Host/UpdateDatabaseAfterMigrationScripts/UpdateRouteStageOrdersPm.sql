delete from RouteStageOrders where CurrentStageId = 1199;

delete from RouteStageOrders where CurrentStageId = 5820;

delete from RouteStageOrders where CurrentStageId = 20001;

delete from RouteStageOrders where CurrentStageId = 13274;

insert into RouteStageOrders (CurrentStageId, IsAutomatic, NextStageId, DateCreate, DateUpdate, IsParallel, IsReturn)
values
(15505, 1, 15464, GETDATE(), GETDATE(),0,0),
(5822, 1, 1226, GETDATE(), GETDATE(),0,0),
(8190, 1, 8217, GETDATE(), GETDATE(),0,0),
(15510, 1, 8217, GETDATE(), GETDATE(),0,0),
(15511, 1, 8217, GETDATE(), GETDATE(),0,0),
(1199, 0, 15573, GETDATE(), GETDATE(),0,0),
(15489, 0, (select top 1 id from DicRouteStages where Code = 'U03.9'), GETDATE(), GETDATE(),0,0), --Нет NextStageId в DicRouteStages
(5822, 0, 15512, GETDATE(), GETDATE(),0,0),
((select top 1 id from DicRouteStages where Code = 'U02.2.7.0'), 0, 1220, GETDATE(), GETDATE(),0,0),
((select top 1 id from DicRouteStages where Code = 'U03.9'), 0, 15489, GETDATE(), GETDATE(), 0, 0),
((select top 1 id from DicRouteStages where Code = 'U03.9'), 0, 15509, GETDATE(), GETDATE(), 0, 0),
(13274, 0, 15512, GETDATE(), GETDATE(),0,0),
  (13274, 0, 5822, GETDATE(), GETDATE(),0,0),
  (13274, 1, 5822, GETDATE(), GETDATE(),0,0),
(15557, 0, 15512, GETDATE(), GETDATE(),0,0),
  (15557, 0, 5822, GETDATE(), GETDATE(),0,0),
  (15557, 1, 5822, GETDATE(), GETDATE(),0,0),
(15452, 0, 15506, GETDATE(), GETDATE(),0,0),
(15452, 0, 15508, GETDATE(), GETDATE(),0,0),
(15506, 0, 1215, GETDATE(), GETDATE(),0,0),
(15506, 0, 15508, GETDATE(), GETDATE(),0,0)

delete ro from RouteStageOrders ro
  inner join DicRouteStages ds on ro.CurrentStageId = ds.Id
  where ds.Code = 'U02.2.3'

delete ro from RouteStageOrders ro
  inner join DicRouteStages ds on ro.NextStageId = ds.Id
  where ds.Code = 'U02.2.3'

delete ro from RouteStageOrders ro
  inner join DicRouteStages ds on ro.CurrentStageId = ds.Id
  inner join DicRouteStages ds2 on ro.NextStageId = ds2.Id
  where ds.Code = 'U03.8' and ds2.Code = 'U03.3.7'

delete ro from RouteStageOrders ro
  inner join DicRouteStages ds on ro.CurrentStageId = ds.Id
  where ds.Code = 'U03.8' and ro.IsAutomatic = 1

delete ro from RouteStageOrders ro
  inner join DicRouteStages ds on ro.CurrentStageId = ds.Id
  where ds.Code = 'U03.7.0'

delete ro from RouteStageOrders ro
  inner join DicRouteStages ds on ro.CurrentStageId = ds.Id
  where ds.Code = 'U03.7.3'