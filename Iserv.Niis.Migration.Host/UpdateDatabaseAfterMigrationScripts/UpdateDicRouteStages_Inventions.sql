
-- UPDATE

update DicRouteStages
set  ExpirationType = 3, ExpirationValue = 2
where code='B02.3'

update DicRouteStages
set  ExpirationType = 3, ExpirationValue = 2
where code='B02.2.0.0'


update DicRouteStages
set  ExpirationType = 3, ExpirationValue = 1
where code='B03.2.1'



update DicRouteStages
set  ExpirationType = 3, ExpirationValue = 12
where code='B04.0'


update DicRouteStages
set  ExpirationType = 2, ExpirationValue = 1
where code='B03.2.1_1'





update DicRouteStages
set  ExpirationType = 2, ExpirationValue = 1
where code='B03.2.3'





update DicRouteStages
set  ExpirationType = 2, ExpirationValue = 1
where code='B03.2.2.1'



update DicRouteStages
set  ExpirationType = 2, ExpirationValue = 1
where code='B03.2.3.0'




update DicRouteStages
set  ExpirationType = 2, ExpirationValue = 10
where code='B03.3.5'




update DicRouteStages
set  ExpirationType = 3, ExpirationValue = 3
where code='B03.3.7.1'




update DicRouteStages
set  IsMain = 1
where code='B02.3'


update DicRouteStages
set IsAuto = 0
where Code in ('B03.3.7.1', 'B03.3.9')


update DicRouteStages
set IsMain = 1
where code='B03.2.2.1'


update DicRouteStages 
set IsMain = 1
where code='B04.0.0.1'


update DicRouteStages 
set NameRu='На утверждении руководства'
where code='B03.3.2.1'



update DicRouteStages 
set NameRu='Утверждено руководством'
where code='B03.3.3'


update DicRouteStages
set ExpirationType = 3, ExpirationValue = 2
where code='B02.2'

update DicRouteStages
set IsMain = 1, IsAuto = 0
where code='B03.3.4.1'


update DicRouteStages
set IsMain = 1
where code='B03.3.5'