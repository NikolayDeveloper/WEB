-- =============================================
-- Author: Azamat.Syzdyqov
-- Create date: 17.05.2016
-- Description: Добавлена таблица DicBeneficiaryTypes, содержащую инфу о типах льготников, создана связь этой таблицы с таблицей Requests, изменение в маршрутах ПМ в соответствии с дополнениями от 15.05.2018
-- =============================================


insert into DicBeneficiaryTypes (Code, DateCreate, DateUpdate, NameRu)
  values ('SMB', GETDATE(), GETDATE(), 'Субъекты малого и среднего бизнеса'),
  ('VET', GETDATE(), GETDATE(), 'Участники Великой отечественной войны, инвалиды, учащиеся общеобразовательных школ и колледжей, студенты высших учебных заведений, пенсионеры по возрасту и выслуге лет')

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
  

insert into DicRouteStages (Code, DateCreate, DateUpdate, NameRu, OnlineRequisitionStatusId, RouteId, IsAuto, ExpirationType, ExpirationValue, IsMain, RequestStatusId, IsFirst, IsLast, IsMultiUser, IsSystem)
values('U03.2.5', GETDATE(), GETDATE(), 'Ошибочно зарегистрированные ПМ',
(select id from
DicOnlineRequisitionStatuses where Code = 'DP'),
(select id from DicRoutes
where Code = 'U'),
 0, 1, 15, 1, 
 (select id from
 DicRequestStatuses where Code = '03.50'), 0, 1, 0, 0)

  insert into RouteStageOrders (CurrentStageId, IsAutomatic, NextStageId, DateCreate, DateUpdate, IsParallel, IsReturn)
  values
  
  ((select id from
  DicRouteStages where Code = 'U03.7.0'), 0,
  (select id from
  DicRouteStages where Code = 'U03.3.7'), GETDATE(), GETDATE(),0,0),

  ((select id from
  DicRouteStages where Code = 'U03.7.0'), 0,
  (select id from
  DicRouteStages where Code = 'U03.8'), GETDATE(), GETDATE(),0,0),

  ((select id from
  DicRouteStages where Code = 'U03.7.0'), 1,
  (select id from
  DicRouteStages where Code = 'U03.8'), GETDATE(), GETDATE(),0,0),

((select id from
  DicRouteStages where Code = 'U03.7.3'), 0,
  (select id from
  DicRouteStages where Code = 'U03.3.7'), GETDATE(), GETDATE(),0,0),

  ((select id from
  DicRouteStages where Code = 'U03.7.3'), 0,
  (select id from
  DicRouteStages where Code = 'U03.8'), GETDATE(), GETDATE(),0,0),

  ((select id from
  DicRouteStages where Code = 'U03.7.3'), 1,
  (select id from
  DicRouteStages where Code = 'U03.8'), GETDATE(), GETDATE(),0,0),

((select top 1 id from
  DicRouteStages where Code = 'U02.2.1' order by Id), 0,
  (select id from
  DicRouteStages where Code = 'U02.2.2.2'), GETDATE(), GETDATE(),0,0),

((select top 1 id from
  DicRouteStages where Code = 'U02.2.1' order by Id), 0,
  (select id from
  DicRouteStages where Code = 'U02.2.4'), GETDATE(), GETDATE(),0,0),

((select id from
  DicRouteStages where Code = 'U02.2.2.2'), 0,
  (select id from
  DicRouteStages where Code = 'U03.1'), GETDATE(), GETDATE(),0,0),

((select id from
  DicRouteStages where Code = 'U02.2.2.2'), 0,
  (select id from
  DicRouteStages where Code = 'U02.2.4'), GETDATE(), GETDATE(),0,0),

((select top 1 id from
  DicRouteStages where Code = 'U02.1'), 0,
  (select top 1 id from
  DicRouteStages where Code = 'U03.2.5'), GETDATE(), GETDATE(),0,0)

  

update DicRouteStages set NameRu = 'Признаны неподанными' where Code = 'U03.2.3'

update DicRouteStages set NameRu = 'Утверждено руководством' where Code = 'U03.4'

insert into DicConventionTypes(Code, DateCreate, DateUpdate, NameRu)
  values('06', GETDATE(), GETDATE(), 'На основе евразийской заявки (96-97)')

update DicRouteStages set IsMain = 1 where Code = 'U02.2.4'

 update DicRouteStages set IsMain = 1 where Code = 'U02.2.2.2';

update r
   set r.RequestTypeId = 89
   from Requests r
   inner join DicProtectionDocSubTypes st on r.RequestTypeId = st.Id
    where st.Code in ('01','02','111') and st.TypeId = 2
	update pd
   set pd.SubTypeId = 89
   from ProtectionDocs pd
   inner join DicProtectionDocSubTypes st on pd.SubTypeId = st.Id
    where st.Code in ('01','02','111') and st.TypeId = 2
	update c
   set c.ContractTypeId = 89
   from Contracts c
   inner join DicProtectionDocSubTypes st on c.ContractTypeId = st.Id
    where st.Code in ('01','02','111') and st.TypeId = 2
  delete from DicProtectionDocSubTypes where Code in ('01','02','111') and TypeId = 2

update r
   set r.ConventionTypeId = 156
   from Requests r
   inner join DicConventionTypes st on r.ConventionTypeId = st.Id
    where st.Code in ('05','02')

	delete from DicConventionTypes where Code in ('05','02')

update DicConventionTypes set NameRu = 'Конвенционная (ПК)' where Code = '01'