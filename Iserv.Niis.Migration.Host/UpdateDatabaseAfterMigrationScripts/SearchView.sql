CREATE VIEW [dbo].[SearchView]
AS
SELECT DISTINCT 1 AS OwnerType, null as DocumentType, r.Id, r.Barcode, r.RequestNum AS Num, r.RequestDate AS Date, ISNULL(r.NameRu, N'') + N' ' + ISNULL(r.NameKz, N'') + N' ' + ISNULL(r.NameEn, N'') AS Description, c.Xin, c.NameRu AS Customer, c.Address, c.CountryId, country.NameRu AS CountryNameRu, r.ReceiveTypeId, rf.NameRu AS ReceiveTypeNameRu
FROM   dbo.Requests AS r LEFT OUTER JOIN
          dbo.RequestCustomers AS rc ON rc.RequestId = r.Id LEFT OUTER JOIN
          dbo.DicCustomers AS c ON c.Id = rc.CustomerId LEFT OUTER JOIN
          dbo.DicCountries AS country ON country.Id = c.CountryId LEFT OUTER JOIN
          dbo.DicReceiveTypes AS rf ON rf.Id = r.ReceiveTypeId
		  WHERE r.IsDeleted = 0
UNION ALL
SELECT DISTINCT 2 AS OwnerType, null as DocumentType, pd.Id, pd.Barcode, pd.GosNumber AS Num, pd.GosDate AS Date, ISNULL(pd.NameRu, N'') + N' ' + ISNULL(pd.NameKz, N'') + N' ' + ISNULL(pd.NameEn, N'') AS Description, c.Xin, c.NameRu AS Customer, c.Address, c.CountryId, country.NameRu AS CountryNameRu, null as ReceiveTypeId, null AS ReceiveTypeNameRu
FROM   dbo.ProtectionDocs AS pd LEFT OUTER JOIN
          dbo.ProtectionDocCustomers AS pdc ON pdc.ProtectionDocId = pd.Id LEFT OUTER JOIN
          dbo.DicCustomers AS c ON c.Id = pdc.CustomerId LEFT OUTER JOIN
          dbo.DicCountries AS country ON country.Id = c.CountryId
UNION ALL
SELECT DISTINCT 3 AS OwnerType, null as DocumentType, contract.Id, contract.Barcode, contract.ContractNum AS Num, contract.RegDate AS Date, ISNULL(contract.NameRu, N'') + N' ' + ISNULL(contract.NameKz, N'') + N' ' + ISNULL(contract.NameEn, N'') AS Description, c.Xin, c.NameRu AS Customer, c.Address, c.CountryId, country.NameRu AS CountryNameRu, contract.ReceiveTypeId, pdf.NameRu AS ReceiveTypeNameRu
FROM   dbo.Contracts AS contract LEFT OUTER JOIN
          dbo.ContractCustomers AS contractCustomer ON contractCustomer.ContractId = contract.Id LEFT OUTER JOIN
          dbo.DicCustomers AS c ON c.Id = contractCustomer.CustomerId LEFT OUTER JOIN
          dbo.DicCountries AS country ON country.Id = c.CountryId LEFT OUTER JOIN
          dbo.DicReceiveTypes AS pdf ON pdf.Id = contract.ReceiveTypeId
UNION ALL		  
SELECT DISTINCT 4 AS OwnerType, d.DocumentType, d.Id, d.Barcode, d.DocumentNum AS Num, d.DateCreate AS Date, ISNULL(d.NameRu, N'') + N' ' + ISNULL(d.NameKz, N'') + N' ' + ISNULL(d.NameEn, N'') AS Description, c.Xin, c.NameRu AS Customer, c.Address, c.CountryId, country.NameRu AS CountryNameRu, d.ReceiveTypeId, rf.NameRu AS ReceiveTypeNameRu
FROM   dbo.Documents AS d LEFT OUTER JOIN
          dbo.DicCustomers AS c ON c.Id = d.AddresseeId LEFT OUTER JOIN
          dbo.DicCountries AS country ON country.Id = c.CountryId LEFT OUTER JOIN
          dbo.DicReceiveTypes AS rf ON rf.Id = d.ReceiveTypeId
		  WHERE d.IsDeleted = 0
GO