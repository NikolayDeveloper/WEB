INSERT [dbo].[AspNetRoles] ([DateCreate],[DateUpdate], [NameKz], [NameRu], [NameEn]) VALUES 
(getdate(),getdate(),N'Директор', N'Директор', N'Director'),
(getdate(),getdate(),N'директор орынбасары', N'Заместитель директора', N'Deputy Director'),
(getdate(),getdate(),N'Бастық', N'Начальник ', N'Supervisor'),
(getdate(),getdate(),N'Кіріс хат-қызметкері', N'Сотрудник входящей корреспонденции', N'Incoming mail employee'),
(getdate(),getdate(),N'Шығыс хат-қызметкері', N'Сотрудник исходящей корреспонденции', N'Outgoing mail employee'),
(getdate(),getdate(),N'Іс жүргізуші', N'Делопроизводитель', N'Clerk'),
(getdate(),getdate(),N'Төлеу жөніндегі сарапшы', N'Эксперт по оплатам', N'Expert on payments'),
(getdate(),getdate(),N'Алдын ала / формалды сараптама сарапшы', N'Эксперт предварительной/формальной экспертизы', N'Expert of preliminary / formal expertise'),
(getdate(),getdate(),N'Толық / мәнi бойынша сараптама сарапшы', N'Эксперт полной/ по существу экспертизы', N'Expert of full / in essence expertise'),
(getdate(),getdate(),N'Мемлекеттік тізілімі сарапшысы ', N'Эскперт Госреестра', N'Expert of the State Register'),
(getdate(),getdate(),N'Шарт бойынша сарапшысы', N'Эксперт по Договорам', N'Contract expert'),
(getdate(),getdate(),N'Бюллетень сарапшы', N'Эксперт бюллетеня', N'Ballot expert')

GO