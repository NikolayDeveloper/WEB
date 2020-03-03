insert into DicRouteStages (Code, DateCreate, DateUpdate, NameRu, OnlineRequisitionStatusId, RouteId, IsAuto, ExpirationType, ExpirationValue, IsMain, RequestStatusId, IsFirst, IsLast, IsMultiUser, IsSystem)
values ('U02.2.7.0', GETDATE(), GETDATE(), 'Внесение изменений', 838, 21, 1, 1, 15, 1, 14, 0, 0, 0, 0),
('U03.2.5', GETDATE(), GETDATE(), 'Ошибочно зарегистрированные ПМ', 867, 21, 1, 1, 15, 1, 295, 0, 0, 0, 0)

update DicRouteStages set ExpirationValue = 1, ExpirationType = 2 where Code = 'U01.1'

update DicRouteStages set ExpirationValue = 5, ExpirationType = 2 where Code = 'U02.1'

update DicRouteStages set ExpirationValue = 1, ExpirationType = 2 where Code = 'U02.1.0'

update DicRouteStages set ExpirationValue = 5, ExpirationType = 2 where Code = 'U02.2'

update DicRouteStages set ExpirationValue = 2, ExpirationType = 3 where Code = 'U02.2.7'

update DicRouteStages set ExpirationValue = 12, ExpirationType = 3 where Code = 'U03.2'

update DicRouteStages set ExpirationValue = 0, ExpirationType = 0 where Code = 'U03.3'

update DicRouteStages set ExpirationValue = 2, ExpirationType = 2 where Code = 'U03.4'

update DicRouteStages set ExpirationValue = 15, ExpirationType = 2 where Code = 'U03.5'

update DicRouteStages set ExpirationValue = 5, ExpirationType = 2 where Code = 'U03.6'

update DicRouteStages set ExpirationValue = 3, ExpirationType = 3 where Code = 'U03.7.0'

update DicRouteStages set ExpirationValue = 0, ExpirationType = 0 where Code = 'U03.7.3'

update DicRouteStages set ExpirationValue = 3, ExpirationType = 3 where Code = 'U03.2.2'

update DicRouteStages set ExpirationValue = 3, ExpirationType = 3 where Code = 'U03.2.1'

update DicRouteStages set ExpirationValue = 15, ExpirationType = 1 where Code = 'U02.2.6'

update DicRouteStages set ExpirationValue = 0, ExpirationType = 0 where Code = 'U03.9'

update DicRouteStages set ExpirationValue = 0, ExpirationType = 0 where Code = 'U02.2.5'

update DicRouteStages set ExpirationValue = 12, ExpirationType = 3 where Code = 'U03.3.1'

update DicRouteStages set IsMain = 1 where Code = 'U03.2.3'

update DicRouteStages set IsLast = 0 where Code = 'U03.4.0'

update DicRouteStages set IsMain = 1 where Code = 'U03.9'

update DicRouteStages set IsMain = 1 where Code = 'U03.3.7'

update DicRouteStages set RequestStatusId = 635 where Code = 'U03.2.4'

update DicRouteStages set RequestStatusId = 636 where Code = 'B03.3.1_1'

update DicRouteStages set NameRu = 'Признаны неподанными' where Code = 'U03.2.3'