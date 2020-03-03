update DicTariffs set ProtectionDocTypeId = null where code = 'NEW_058'

insert into DicTariffs (Code, DateCreate, DateUpdate, NameRu, PriceBeneficiary, PriceBusiness, PriceFl, PriceUl)
  values ('NEW_GOS_18', GETDATE(), GETDATE(), 'Госпошлина', 2405,2405,2405,2405)