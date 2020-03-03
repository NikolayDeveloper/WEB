update DicRouteStages
set ExpirationType = 2, ExpirationValue = 1
where Code = 'NMPT01.1 ';

update DicRouteStages
set ExpirationType = 2, ExpirationValue = 1
where Code = 'NMPT01.2';

update DicRouteStages
set ExpirationType = 2, ExpirationValue = 5
where Code = 'NMPT02.1';

update DicRouteStages
set ExpirationType = 2, ExpirationValue = 5
where Code = 'NMPT02.2';

update DicRouteStages
set ExpirationType = 3, ExpirationValue = 1
where Code = 'NMPT03.1';

update DicRouteStages
set ExpirationType = 3, ExpirationValue = 6
where Code = 'NMPT03.2';

update DicRouteStages
set ExpirationType = 2, ExpirationValue = 3
where Code = 'NMPT03.3.0';

update DicRouteStages
set ExpirationType = 2, ExpirationValue = 3
where Code = 'NMPT02.2.0';

update DicRouteStages
set ExpirationType = 1, ExpirationValue = 15
where Code = 'NMPT03.2.2';

update DicRouteStages
set ExpirationType = 3, ExpirationValue = 3
where Code = 'NMPT03.2.1';

update DicRouteStages
set ExpirationType = 2, ExpirationValue = 15
where Code = 'NMPT03.5';

update DicRouteStages
set ExpirationType = 2, ExpirationValue = 5
where Code = 'NMPT03.6';

update DicRouteStages
set ExpirationType = 2, ExpirationValue = 5
where Code = 'NMPT03.3.1';

update DicRouteStages
set ExpirationType = 1, ExpirationValue = 15
where Code = 'NMPT03.2.2';

update DicRouteStages
set ExpirationType = 1, ExpirationValue = 15
where Code = 'NMPT03.3.2';

update DicRouteStages
set ExpirationType = 2, ExpirationValue = 3
where Code = 'NMPT03.3.0';

update DicRouteStages
set ExpirationType = 1, ExpirationValue = 7
where Code = 'NMPT03.7';

update DicRouteStages
set ExpirationType = 1, ExpirationValue = 7
where Code = 'NMPT03.8';

update DicRouteStages
set ExpirationType = 3, ExpirationValue = 2
where Code = 'NMPT03.2.3';

update DicRouteStages
set ExpirationType = 1, ExpirationValue = 15
where Code = 'NMPT03.2.4';

update DicRouteStages set IsAuto = 1 where Code = 'NMPT03.1';

update DicRouteStages set IsAuto = 1 where Code = 'NMPT03.3';

update DicRouteStages set IsAuto = 0 where Code = 'NMPT04.1';

update DicRouteStages set IsMain = 1 where Code = 'NMPT03.2.4'

update DicRouteStages set IsMain = 1 where Code = 'NMPT03.3.4.0'

update DicRouteStages set IsLast = 0 where Code = 'NMPT03.3.4.0'

update DicRouteStages set IsLast = 0 where Code = 'NMPT03.3.6'

update DicRouteStages set IsMain = 1 where Code = 'NMPT03.7.0'

update DicRouteStages set IsMain = 1 where Code = 'NMPT02.2.1'

update DicRouteStages set IsMain = 1 where Code = 'NMPT03.3.1'

update DicRouteStages set NameRu = 'На утверждении руководства' where Code = 'NMPT03.3.0'

insert into DicRouteStages(Code, NameRu, RouteId, IsAuto, DateCreate, DateUpdate, IsFirst, IsLast, IsMultiUser, IsSystem, ExpirationType, IsMain)
values ('NMPT03.3.7', 'Отказано в регистрации', 101, 1, GETDATE(), GETDATE(), 0, 1, 0, 1, 0, 0)