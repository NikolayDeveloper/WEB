  insert into RouteStageOrders(CurrentStageId, DateCreate, DateUpdate, IsAutomatic, IsParallel, IsReturn, NextStageId)
  values 
((select top 1 id from DicRouteStages where Code = 'NMPT03.6'), GETDATE(),GETDATE(), 0, 0, 0, (select top 1 id from DicRouteStages where Code = 'NMPT03.7.0')),
((select top 1 id from DicRouteStages where Code = 'NMPT02.2.0'), GETDATE(), GETDATE(), 1, 0, 0, 15477),
((select top 1 id from DicRouteStages where Code = 'NMPT03.7.0'), GETDATE(), GETDATE(), 0, 0, 0, 8310),
(8310, GETDATE(), GETDATE(), 1, 0, 0, 15477),
(8307, GETDATE(), GETDATE(), 1, 0, 0, 8335),
(15532, GETDATE(), GETDATE(), 1, 0, 0, 8335),
(8308, GETDATE(), GETDATE(), 1, 0, 0, 8335),
(15533, GETDATE(), GETDATE(), 1, 0, 0, 8335),
(15427, GETDATE(), GETDATE(), 1, 0, 0, 8335),
(8309, GETDATE(), GETDATE(), 1, 0, 0, 8335),
(8312, GETDATE(), GETDATE(), 1, 0, 0, 8335),
(15429, GETDATE(), GETDATE(), 1, 0, 0, (select top 1 id from DicRouteStages where Code = 'NMPT03.3.7')),
(15535, GETDATE(), GETDATE(), 1, 0, 0, (select top 1 id from DicRouteStages where Code = 'NMPT03.3.7')),
(15536, GETDATE(), GETDATE(), 1, 0, 0, 8335),
((select top 1 id from DicRouteStages where Code = 'NMPT03.7.0'), GETDATE(), GETDATE(), 1, 0, 0, 8335),
(8310, GETDATE(), GETDATE(), 1, 0, 0, 8335),
(8311, GETDATE(), GETDATE(), 1, 0, 0, 8335),
(15547, GETDATE(), GETDATE(), 1, 0, 0, 8335),
(15539, GETDATE(), GETDATE(), 1, 0, 0, 8335),
(15540, GETDATE(), GETDATE(), 1, 0, 0, 8335),
(15538, GETDATE(), GETDATE(), 1, 0, 0, 8335),
(15540, GETDATE(), GETDATE(), 0,0,0,15538)

update RouteStageOrders
   set IsAutomatic = 1
   from RouteStageOrders ro
   inner join DicRouteStages fs on ro.CurrentStageId = fs.Id
   inner join DicRouteStages ts on ro.NextStageId = ts.Id
   where fs.Code = 'NMPT03.2.1' and ts.Code = 'NMPT02.2.1'

  update RouteStageOrders set NextStageId = 8295 where CurrentStageId = 8290

  update RouteStageOrders set NextStageId = 8297 where CurrentStageId = 8295


  delete from RouteStageOrders
  from RouteStageOrders ro
   inner join DicRouteStages fs on ro.CurrentStageId = fs.Id
   inner join DicRouteStages ts on ro.NextStageId = ts.Id
where fs.Code = 'NMPT03.3.6' and ts.Code in ('NMPT03.3.6.0', 'NMPT03.3.0')

  delete from RouteStageOrders
  from RouteStageOrders ro
   inner join DicRouteStages fs on ro.CurrentStageId = fs.Id
   inner join DicRouteStages ts on ro.NextStageId = ts.Id
where fs.Code = 'NMPT03.6' and ts.Code in ('NMPT03.3.4.0', 'NMPT03.3.1')

  delete from RouteStageOrders
  from RouteStageOrders ro
   inner join DicRouteStages fs on ro.CurrentStageId = fs.Id
   inner join DicRouteStages ts on ro.NextStageId = ts.Id
where fs.Code = 'NMPT03.3.4.0' and ts.Code = 'NMPT03.3.5'

delete from RouteStageOrders
from RouteStageOrders ro
   inner join DicRouteStages fs on ro.CurrentStageId = fs.Id
   inner join DicRouteStages ts on ro.NextStageId = ts.Id
  where fs.Code = 'NMPT03.2.2' and ts.Code in ('NMPT03.2','NMPT02.2.1','NMPT03.2.3','NMPT03.3.2')

delete from RouteStageOrders
  where CurrentStageId = 15427 and NextStageId in (8335, 15533) and IsAutomatic = 0