update DicProtectionDocTypes set Code = 'NMPT' where Code = 'PN'

INSERT [dbo].[DicProtectionDocSubTypes] ([DateCreate],[DateUpdate],[TypeId],[NameRu],[NameKz],[Code],[S1],[S2],[S1Kz],[S2Kz]) VALUES 

(getdate(),getdate(),(select top 1 Id from DicProtectionDocTypes where Code = 'S2'),N'Ближнее зарубежъе',NULL,N'03_Industrial_Sample',NULL,NULL,NULL,NULL),
(getdate(),getdate(),(select top 1 Id from DicProtectionDocTypes where Code = 'S2'),N'Иностранная',NULL,N'04_Industrial_Sample',NULL,NULL,NULL,NULL),
(getdate(),getdate(),(select top 1 Id from DicProtectionDocTypes where Code = 'S2'),N'Национальная',NULL,N'05_Industrial_Sample',NULL,NULL,NULL,NULL),

(getdate(),getdate(),(select top 1 Id from DicProtectionDocTypes where Code = 'TM'),N'Ближнее зарубежъе',NULL,N'03_Trade_Mark',NULL,NULL,NULL,NULL),
(getdate(),getdate(),(select top 1 Id from DicProtectionDocTypes where Code = 'TM'),N'Иностранная',NULL,N'04_Trade_Mark',NULL,NULL,NULL,NULL),


(getdate(),getdate(),(select top 1 Id from DicProtectionDocTypes where Code = 'NMPT'),N'Ближнее зарубежъе',NULL,N'03_Name_of_Origin',NULL,NULL,NULL,NULL),
(getdate(),getdate(),(select top 1 Id from DicProtectionDocTypes where Code = 'NMPT'),N'Иностранная',NULL,N'04_Name_of_Origin',NULL,NULL,NULL,NULL),
(getdate(),getdate(),(select top 1 Id from DicProtectionDocTypes where Code = 'NMPT'),N'Национальная',NULL,N'05_Name_of_Origin',NULL,NULL,NULL,NULL),

(getdate(),getdate(),(select top 1 Id from DicProtectionDocTypes where Code = 'SA'),N'Ближнее зарубежъе',NULL,N'03_Selection_Achieve',NULL,NULL,NULL,NULL),
(getdate(),getdate(),(select top 1 Id from DicProtectionDocTypes where Code = 'SA'),N'Иностранная',NULL,N'04_Selection_Achieve',NULL,NULL,NULL,NULL),
(getdate(),getdate(),(select top 1 Id from DicProtectionDocTypes where Code = 'SA'),N'Национальная',NULL,N'05_Selection_Achieve',NULL,NULL,NULL,NULL)