insert into DicContactInfoTypes (Code, DateCreate, DateUpdate, Description, NameEn, NameKz, NameRu)
values ('phone', GETDATE(), GETDATE(), 'Городской телефон', 'Phone', 'Телефон', 'Телефон'),
('mobilePhone', GETDATE(), GETDATE(), 'Мобильный телефон', 'Mobile phone', 'Ұялы телефон', 'Мобильный телефон'),
('fax', GETDATE(), GETDATE(), 'Факс', 'Fax', 'Факс', 'Факс'),
('email', GETDATE(), GETDATE(), 'Адрес электронной почты', 'Email address', 'Электрондық пошта', 'Электронная почта');