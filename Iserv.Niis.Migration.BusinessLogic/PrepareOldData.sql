update CL_DEPARTMENT set F_ID = 227
where U_ID = 766

update CL_LOCATION set F_ID = null
where U_ID = 3890

update WT_CUSTOMER set COUNTRY_ID = 1
where U_ID = 230734


update WT_CUSTOMER set COUNTRY_ID = 9
where U_ID = 337831


update WT_CUSTOMER set COUNTRY_ID = 9
where U_ID = 346159



--Обновление справочника DicProtectionDocsubTypes
--перебьем коды по договорам

---Договор о частичной уступке исключительных прав
update SPT_PAT_SUBT set code = 'Contract_14', name_ml_ru = 'Договор уступки исключительных прав' where u_id = 20
---Договор о частичной уступке исключительных прав
update SPT_PAT_SUBT set code = 'Contract_15', name_ml_ru = 'Договор о частичной уступке исключительных прав' where u_id = 21
---Договор о частичной уступке исключительных прав
update SPT_PAT_SUBT set code = 'Contract_23', name_ml_ru = 'Договор уступки права на получение охранного документа' where u_id = 28

update SPT_PAT_SUBT set code = 'Contract_18' where u_id = 19
update SPT_PAT_SUBT set code = 'Contract_12' where u_id = 22
update SPT_PAT_SUBT set code = 'Contract_11' where u_id = 23
update SPT_PAT_SUBT set code = 'Contract_17' where u_id = 24
update SPT_PAT_SUBT set code = 'Contract_20' where u_id = 27
update SPT_PAT_SUBT set code = 'Contract_10' where u_id = 29
update SPT_PAT_SUBT set code = 'Contract_13' where u_id = 43
update SPT_PAT_SUBT set code = 'Contract_16' where u_id = 44
update SPT_PAT_SUBT set code = 'Contract_19' where u_id = 83

if not exists(select U_ID from SPT_PAT_SUBT where U_ID = 95)
begin
	insert into SPT_PAT_SUBT (u_id,TYPE_ID,NAME_ML_RU,code) values(95,72,'Договор о предоставлении неисключительной комплексной предпринимательской лицензии (франчайзинга)','Contract_21')
end

if not exists(select U_ID from SPT_PAT_SUBT where U_ID = 96)
begin
	insert into SPT_PAT_SUBT (u_id,TYPE_ID,NAME_ML_RU,code) values(96,72,'Договор комплексной предпринимательской сублицензии (франчайзинга)','Contract_22')
end

if not exists(select U_ID from SPT_PAT_SUBT where U_ID = 97)
begin
	insert into SPT_PAT_SUBT (u_id,TYPE_ID,NAME_ML_RU,code) values(97,72,'В настоящее время не используются, но должны быть','Contract_NotUsedCodes')
end
print 'для подтипов договоро обновлена таблица SPT_PAT_SUBT'




--Обновление SS_GROUP

 update SS_GROUP
 set CODE = '4.3.1.1'
 where U_ID = 15302


 update SP_TYPE_PATENT
 set CODE = 'NMPT'
 where CODE = 'PN'