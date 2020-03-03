CREATE VIEW [dbo].[ExpertSearchView]
AS
Select distinct
	1 as OwnerType,
	r.ProtectionDocTypeId,
	1 as SearchStatus,
	r.Id, 
	r.Barcode, 
	r.RequestTypeId,
	rt.NameRu as RequestTypeNameRu,
	r.StatusId,
	s.Code as StatusCode,
	s.NameRu as StatusNameRu,
	r.PreviewImage,
	pd.GosNumber,
	pd.GosDate,
	r.RequestNum,
	r.RequestDate,
	ISNULL(r.NameRu, N'') + N' ' + ISNULL(r.NameKz, N'') + N' ' + ISNULL(r.NameEn, N'') AS Name,
	r.NameRu,
	r.NameKz,
	r.NameEn,
	cdec.NameRu as Declarant,
	cown.NameRu as Owner,	
	cattor.NameRu as PatentAttorney,
	caddr.Address as AddressForCorrespondence,
	cconf.NameRu as Confidant,
	cauthor.NameRu as Author,
	cpowner.NameRu as PatentOwner,
	r.ReceiveTypeId,
	receiveTypes.NameRu as ReceiveTypeNameRu,
	icgsquery.Icgs,
	icfemquery.Icfems,
	icisquery.Icis,
	ipcsquery.Ipcs,
	r.Transliteration,
	prioritydataquery.PriorityRegCountryNames,
	prioritydataquery.PriorityRegNumbers,
	--PriorityDates,
	prioritydataquery.PriorityData,
	r.NumberBulletin,
	r.PublicDate,
	pd.ValidDate,
	pd.ExtensionDateTz,
	r.TransferDate,
	pd.EarlyTerminationDate,
	formulac.Content as Formula,
	ISNULL(r.Referat, N'') + N' ' + ISNULL(referatc.Content, N'') AS Referat,
	descriptionc.Content as Description,
	r.DisclaimerRu,
	r.DisclaimerKz,
	r.Id as RequestId,
	null as ProtectionDocId
from Requests r
	left join DicProtectionDocSubTypes rt on rt.Id = r.RequestTypeId
	left join DicRequestStatuses s on s.Id = r.StatusId
	left join ProtectionDocs pd on pd.Id = r.ProtectionDocId
	left join RequestCustomers rcdec on rcdec.RequestId = r.Id and rcdec.CustomerRoleId = 1
	left join DicCustomers cdec on cdec.Id = rcdec.CustomerId
	left join RequestCustomers rcown on rcown.RequestId = r.Id and rcown.CustomerRoleId = 11
	left join DicCustomers cown on cown.Id = rcown.CustomerId
	left join RequestCustomers rcattor on rcattor.RequestId = r.Id and rcattor.CustomerRoleId = 4
	left join DicCustomers cattor on cattor.Id = rcattor.CustomerId
	left join RequestCustomers rcaddr on rcaddr.RequestId = r.Id and rcaddr.CustomerRoleId = 12
	left join DicCustomers caddr on caddr.Id = rcaddr.CustomerId
	left join RequestCustomers rcconf on rcconf.RequestId = r.Id and rcconf.CustomerRoleId = 6
	left join DicCustomers cconf on cconf.Id = rcconf.CustomerId
	left join RequestCustomers rcauthor on rcauthor.RequestId = r.Id and rcauthor.CustomerRoleId = 2
	left join DicCustomers cauthor on cauthor.Id = rcauthor.CustomerId
	left join RequestCustomers rcpowner on rcpowner.RequestId = r.Id and rcpowner.CustomerRoleId = 3
	left join DicCustomers cpowner on cpowner.Id = rcpowner.CustomerId
	left join DicReceiveTypes receiveTypes on receiveTypes.Id = r.ReceiveTypeId
	left join RequestsDocuments rd on rd.RequestId = r.Id
	left join Documents formula on formula.Id = rd.DocumentId and formula.TypeId in (4289,4290,4291,4292,320,442,833,4269)
	left join DocumentContents formulac on formulac.DocumentId = formula.Id
	left join Documents referat on referat.Id = rd.DocumentId and referat.TypeId in (292,832,4125,4132,4270,4293,4294,4295,4296)
	left join DocumentContents referatc on referatc.DocumentId = referat.Id
	left join Documents description on description.Id = rd.DocumentId and description.TypeId in (318,443,831,1571,2531,2991,3011,3012,3331,3332,4253,4268)
	left join DocumentContents descriptionc on descriptionc.DocumentId = description.Id
	left join (SELECT 
				[RequestId],
				STUFF(
					CAST(
					(SELECT [text()] = ', ' + c.NameRu
					FROM RequestEarlyRegs
					inner join DicCountries c on RegCountryId = c.Id
					WHERE (RequestId = Results.RequestId) 
					FOR XML PATH(''), TYPE) AS VARCHAR(max)), 1, 2, ''
				) AS PriorityRegCountryNames,
				STUFF(
					CAST(
					(SELECT [text()] = ', ' + RegNumber
					FROM RequestEarlyRegs
					inner join DicCountries c on RegCountryId = c.Id
					WHERE (RequestId = Results.RequestId) 
					FOR XML PATH(''), TYPE) AS VARCHAR(max)), 1, 2, ''
				) AS PriorityRegNumbers,
				STUFF(
					CAST(
					(SELECT [text()] = ', ' + RegNumber	+ ' ' + c.Code + ' ' + ISNULL(CONVERT(varchar(10), dateadd(HOUR, 6, PriorityDate),104), N'') 
					FROM RequestEarlyRegs
					inner join DicCountries c on RegCountryId = c.Id
					WHERE (RequestId = Results.RequestId) 
					FOR XML PATH(''), TYPE) AS VARCHAR(max)), 1, 2, ''
				) AS PriorityData
				FROM RequestEarlyRegs Results
				GROUP BY RequestId) prioritydataquery on prioritydataquery.RequestId = r.id
				left join (SELECT 
					 [RequestId],
					 STUFF(
 						CAST(
 						(SELECT [text()] = ', ' + substring(i.Code, 1, 2)
 						FROM ICGS_Request
 						inner join DicICGSs i on IcgsId = i.Id
 						WHERE (RequestId = Results.RequestId) 
 						FOR XML PATH(''), TYPE) AS VARCHAR(max)), 1, 2, ''
					 ) AS Icgs
					FROM ICGS_Request Results
					GROUP BY RequestId
				) icgsquery on icgsquery.RequestId = r.Id
				left join (SELECT 
					 [RequestId],
					 STUFF(
						CAST(
						(SELECT [text()] = ', ' + i.Code
						FROM DicIcfem_Request
						inner join DicICFEMs i on DicIcfemId = i.Id
						WHERE (RequestId = Results.RequestId) 
						FOR XML PATH(''), TYPE) AS VARCHAR(max)), 1, 2, ''
					 ) AS Icfems
					FROM DicIcfem_Request Results
					GROUP BY RequestId) icfemquery on icfemquery.RequestId = r.Id
				left join (SELECT 
					 [RequestId],
					 STUFF(
						CAST(
						(SELECT [text()] = ', ' + i.Code
						FROM ICIS_Request
						inner join DicICISs i on IcisId = i.Id
						WHERE (RequestId = Results.RequestId) 
						FOR XML PATH(''), TYPE) AS VARCHAR(max)), 1, 2, ''
					 ) AS Icis
					FROM ICIS_Request Results
					GROUP BY RequestId) icisquery on icisquery.RequestId = r.Id
				left join (SELECT 
					 [RequestId],
					 STUFF(
						CAST(
						(SELECT [text()] = ', ' + i.Code
						FROM IPC_Request
						inner join DicIPCs i on IpcId = i.Id
						WHERE (RequestId = Results.RequestId) 
						FOR XML PATH(''), TYPE) AS VARCHAR(max)), 1, 2, ''
					 ) AS Ipcs
					FROM IPC_Request Results
					GROUP BY RequestId) ipcsquery on ipcsquery.RequestId = r.Id
where s.Code in ('03.25','03.26','W','03.49','03.9','03.51','03.52','03.56','03.57') and r.IsDeleted = 0
UNION ALL
Select distinct
	2 as OwnerType,
	pd.TypeId as ProtectionDocTypeId,
	2 as SearchStatus,
	pd.Id, 
	pd.Barcode, 
	r.RequestTypeId,
	rt.NameRu as RequestTypeNameRu,
	pd.StatusId,
	s.Code as StatusCode,
	s.NameRu as StatusNameRu,
	r.PreviewImage,
	pd.GosNumber,
	pd.GosDate,
	r.RequestNum,
	r.RequestDate,
	ISNULL(pd.NameRu, N'') + N' ' + ISNULL(pd.NameKz, N'') + N' ' + ISNULL(pd.NameEn, N'') AS Name,
	pd.NameRu,
	pd.NameKz,
	pd.NameEn,
	cdec.NameRu as Declarant,
	cown.NameRu as Owner,	
	cattor.NameRu as PatentAttorney,
	caddr.Address as AddressForCorrespondence,
	cconf.NameRu as Confidant,
	cauthor.NameRu as Author,
	cpowner.NameRu as PatentOwner,
	r.ReceiveTypeId,
	receiveTypes.NameRu as ReceiveTypeNameRu,
	icgsquery.Icgs,
	icfemquery.Icfems,
	icisquery.Icis,
	ipcsquery.Ipcs,
	pd.Transliteration,
	prioritydataquery.PriorityRegCountryNames,
	prioritydataquery.PriorityRegNumbers,
	--PriorityDates,
	prioritydataquery.PriorityData,
	pd.NumberBulletin,
	pd.PublicDate,
	pd.ValidDate,
	pd.ExtensionDateTz,
	pd.TransferDate,
	pd.EarlyTerminationDate,
	formulac.Content as Formula,
	ISNULL(pd.Referat, N'') + N' ' + ISNULL(referatc.Content, N'') AS Referat,
	descriptionc.Content as Description,
	pd.DisclaimerRu,
	pd.DisclaimerKz,
	null as RequestId,
	pd.Id as ProtectionDocId
from ProtectionDocs pd
	left join Requests r on r.ProtectionDocId = pd.Id
	left join DicProtectionDocSubTypes rt on rt.Id = pd.SubTypeId
	left join DicProtectionDocStatuses s on s.Id = pd.StatusId
	left join ProtectionDocCustomers pdcdec on pdcdec.ProtectionDocId = pd.Id and pdcdec.CustomerRoleId = 1
	left join DicCustomers cdec on cdec.Id = pdcdec.CustomerId
	left join ProtectionDocCustomers pdcown on pdcown.ProtectionDocId = pd.Id and pdcown.CustomerRoleId = 11
	left join DicCustomers cown on cown.Id = pdcown.CustomerId
	left join ProtectionDocCustomers pdcattor on pdcattor.ProtectionDocId = pd.Id and pdcattor.CustomerRoleId = 4
	left join DicCustomers cattor on cattor.Id = pdcattor.CustomerId
	left join ProtectionDocCustomers pdcaddr on pdcaddr.ProtectionDocId = pd.Id and pdcaddr.CustomerRoleId = 12
	left join DicCustomers caddr on caddr.Id = pdcaddr.CustomerId
	left join ProtectionDocCustomers pdcconf on pdcconf.ProtectionDocId = pd.Id and pdcconf.CustomerRoleId = 6
	left join DicCustomers cconf on cconf.Id = pdcconf.CustomerId
	left join ProtectionDocCustomers pdcauthor on pdcauthor.ProtectionDocId = r.Id and pdcauthor.CustomerRoleId = 2
	left join DicCustomers cauthor on cauthor.Id = pdcauthor.CustomerId
	left join ProtectionDocCustomers pdcpowner on pdcpowner.ProtectionDocId = r.Id and pdcpowner.CustomerRoleId = 3
	left join DicCustomers cpowner on cpowner.Id = pdcpowner.CustomerId
	left join DicReceiveTypes receiveTypes on receiveTypes.Id = r.ReceiveTypeId
	left join ProtectionDocDocuments pdd on pdd.ProtectionDocId = pd.Id
	left join Documents formula on formula.Id = pdd.DocumentId and formula.TypeId in (4289,4290,4291,4292,320,442,833,4269)
	left join DocumentContents formulac on formulac.DocumentId = formula.Id
	left join Documents referat on referat.Id = pdd.DocumentId and referat.TypeId in (292,832,4125,4132,4270,4293,4294,4295,4296)
	left join DocumentContents referatc on referatc.DocumentId = referat.Id
	left join Documents description on description.Id = pdd.DocumentId and description.TypeId in (318,443,831,1571,2531,2991,3011,3012,3331,3332,4253,4268)
	left join DocumentContents descriptionc on descriptionc.DocumentId = description.Id
	left join (SELECT 
				[ProtectionDocId],
				STUFF(
					CAST(
					(SELECT [text()] = ', ' + c.NameRu
					FROM ProtectionDocEarlyRegs
					inner join DicCountries c on RegCountryId = c.Id
					WHERE (ProtectionDocId = Results.ProtectionDocId) 
					FOR XML PATH(''), TYPE) AS VARCHAR(max)), 1, 2, ''
				) AS PriorityRegCountryNames,
				STUFF(
					CAST(
					(SELECT [text()] = ', ' + RegNumber
					FROM ProtectionDocEarlyRegs
					inner join DicCountries c on RegCountryId = c.Id
					WHERE (ProtectionDocId = Results.ProtectionDocId) 
					FOR XML PATH(''), TYPE) AS VARCHAR(max)), 1, 2, ''
				) AS PriorityRegNumbers,
				STUFF(
					CAST(
					(SELECT [text()] = ', ' + RegNumber	+ ' ' + c.Code + ' ' + ISNULL(CONVERT(varchar(10), dateadd(HOUR, 6, PriorityDate),104), N'') 
					FROM ProtectionDocEarlyRegs
					inner join DicCountries c on RegCountryId = c.Id
					WHERE (ProtectionDocId = Results.ProtectionDocId) 
					FOR XML PATH(''), TYPE) AS VARCHAR(max)), 1, 2, ''
				) AS PriorityData
				FROM ProtectionDocEarlyRegs Results
				GROUP BY ProtectionDocId) prioritydataquery on prioritydataquery.ProtectionDocId = pd.id
				left join (SELECT 
					 [ProtectionDocId],
					 STUFF(
 						CAST(
 						(SELECT [text()] = ', ' + substring(i.Code, 1, 2)
 						FROM ICGS_ProtectionDoc
 						inner join DicICGSs i on IcgsId = i.Id
 						WHERE (ProtectionDocId = Results.ProtectionDocId) 
 						FOR XML PATH(''), TYPE) AS VARCHAR(max)), 1, 2, ''
					 ) AS Icgs
					FROM ICGS_ProtectionDoc Results
					GROUP BY ProtectionDocId
				) icgsquery on icgsquery.ProtectionDocId = pd.Id
				left join (SELECT 
					 [ProtectionDocId],
					 STUFF(
						CAST(
						(SELECT [text()] = ', ' + i.Code
						FROM DicIcfem_ProtectionDoc
						inner join DicICFEMs i on DicIcfemId = i.Id
						WHERE (ProtectionDocId = Results.ProtectionDocId) 
						FOR XML PATH(''), TYPE) AS VARCHAR(max)), 1, 2, ''
					 ) AS Icfems
					FROM DicIcfem_ProtectionDoc Results
					GROUP BY ProtectionDocId) icfemquery on icfemquery.ProtectionDocId = pd.Id
				left join (SELECT 
					 [ProtectionDocId],
					 STUFF(
						CAST(
						(SELECT [text()] = ', ' + i.Code
						FROM ICIS_ProtectionDoc
						inner join DicICISs i on IcisId = i.Id
						WHERE (ProtectionDocId = Results.ProtectionDocId) 
						FOR XML PATH(''), TYPE) AS VARCHAR(max)), 1, 2, ''
					 ) AS Icis
					FROM ICIS_ProtectionDoc Results
					GROUP BY ProtectionDocId) icisquery on icisquery.ProtectionDocId = pd.Id
				left join (SELECT 
					 [ProtectionDocId],
					 STUFF(
						CAST(
						(SELECT [text()] = ', ' + i.Code
						FROM IPC_ProtectionDoc
						inner join DicIPCs i on IpcId = i.Id
						WHERE (ProtectionDocId = Results.ProtectionDocId) 
						FOR XML PATH(''), TYPE) AS VARCHAR(max)), 1, 2, ''
					 ) AS Ipcs
					FROM IPC_ProtectionDoc Results
					GROUP BY ProtectionDocId) ipcsquery on ipcsquery.ProtectionDocId = pd.Id
where s.Code = 'D'
UNION ALL
Select distinct
	2 as OwnerType,
	pd.TypeId as ProtectionDocTypeId,
	3 as SearchStatus,
	pd.Id, 
	pd.Barcode, 
	r.RequestTypeId,
	rt.NameRu as RequestTypeNameRu,
	pd.StatusId,
	s.Code as StatusCode,
	s.NameRu as StatusNameRu,
	r.PreviewImage,
	pd.GosNumber,
	pd.GosDate,
	r.RequestNum,
	r.RequestDate,
	ISNULL(pd.NameRu, N'') + N' ' + ISNULL(pd.NameKz, N'') + N' ' + ISNULL(pd.NameEn, N'') AS Name,
	pd.NameRu,
	pd.NameKz,
	pd.NameEn,
	cdec.NameRu as Declarant,
	cown.NameRu as Owner,	
	cattor.NameRu as PatentAttorney,
	caddr.Address as AddressForCorrespondence,
	cconf.NameRu as Confidant,
	cauthor.NameRu as Author,
	cpowner.NameRu as PatentOwner,
	r.ReceiveTypeId,
	receiveTypes.NameRu as ReceiveTypeNameRu,
	icgsquery.Icgs,
	icfemquery.Icfems,
	icisquery.Icis,
	ipcsquery.Ipcs,
	pd.Transliteration,
	prioritydataquery.PriorityRegCountryNames,
	prioritydataquery.PriorityRegNumbers,
	--PriorityDates,
	prioritydataquery.PriorityData,
	pd.NumberBulletin,
	pd.PublicDate,
	pd.ValidDate,
	pd.ExtensionDateTz,
	pd.TransferDate,
	pd.EarlyTerminationDate,
	formulac.Content as Formula,
	ISNULL(pd.Referat, N'') + N' ' + ISNULL(referatc.Content, N'') AS Referat,
	descriptionc.Content as Description,
	pd.DisclaimerRu,
	pd.DisclaimerKz,
	null as RequestId,
	pd.Id as ProtectionDocId
from ProtectionDocs pd
	left join Requests r on r.ProtectionDocId = pd.Id
	left join DicProtectionDocSubTypes rt on rt.Id = pd.SubTypeId
	left join DicProtectionDocStatuses s on s.Id = pd.StatusId
	left join ProtectionDocCustomers pdcdec on pdcdec.ProtectionDocId = pd.Id and pdcdec.CustomerRoleId = 1
	left join DicCustomers cdec on cdec.Id = pdcdec.CustomerId
	left join ProtectionDocCustomers pdcown on pdcown.ProtectionDocId = pd.Id and pdcown.CustomerRoleId = 11
	left join DicCustomers cown on cown.Id = pdcown.CustomerId
	left join ProtectionDocCustomers pdcattor on pdcattor.ProtectionDocId = pd.Id and pdcattor.CustomerRoleId = 4
	left join DicCustomers cattor on cattor.Id = pdcattor.CustomerId
	left join ProtectionDocCustomers pdcaddr on pdcaddr.ProtectionDocId = pd.Id and pdcaddr.CustomerRoleId = 12
	left join DicCustomers caddr on caddr.Id = pdcaddr.CustomerId
	left join ProtectionDocCustomers pdcconf on pdcconf.ProtectionDocId = pd.Id and pdcconf.CustomerRoleId = 6
	left join DicCustomers cconf on cconf.Id = pdcconf.CustomerId
	left join ProtectionDocCustomers pdcauthor on pdcauthor.ProtectionDocId = r.Id and pdcauthor.CustomerRoleId = 2
	left join DicCustomers cauthor on cauthor.Id = pdcauthor.CustomerId
	left join ProtectionDocCustomers pdcpowner on pdcpowner.ProtectionDocId = r.Id and pdcpowner.CustomerRoleId = 3
	left join DicCustomers cpowner on cpowner.Id = pdcpowner.CustomerId
	left join DicReceiveTypes receiveTypes on receiveTypes.Id = r.ReceiveTypeId
	left join ProtectionDocDocuments pdd on pdd.ProtectionDocId = pd.Id
	left join Documents formula on formula.Id = pdd.DocumentId and formula.TypeId in (4289,4290,4291,4292,320,442,833,4269)
	left join DocumentContents formulac on formulac.DocumentId = formula.Id
	left join Documents referat on referat.Id = pdd.DocumentId and referat.TypeId in (292,832,4125,4132,4270,4293,4294,4295,4296)
	left join DocumentContents referatc on referatc.DocumentId = referat.Id
	left join Documents description on description.Id = pdd.DocumentId and description.TypeId in (318,443,831,1571,2531,2991,3011,3012,3331,3332,4253,4268)
	left join DocumentContents descriptionc on descriptionc.DocumentId = description.Id
	left join (SELECT 
				[ProtectionDocId],
				STUFF(
					CAST(
					(SELECT [text()] = ', ' + c.NameRu
					FROM ProtectionDocEarlyRegs
					inner join DicCountries c on RegCountryId = c.Id
					WHERE (ProtectionDocId = Results.ProtectionDocId) 
					FOR XML PATH(''), TYPE) AS VARCHAR(max)), 1, 2, ''
				) AS PriorityRegCountryNames,
				STUFF(
					CAST(
					(SELECT [text()] = ', ' + RegNumber
					FROM ProtectionDocEarlyRegs
					inner join DicCountries c on RegCountryId = c.Id
					WHERE (ProtectionDocId = Results.ProtectionDocId) 
					FOR XML PATH(''), TYPE) AS VARCHAR(max)), 1, 2, ''
				) AS PriorityRegNumbers,
				STUFF(
					CAST(
					(SELECT [text()] = ', ' + RegNumber	+ ' ' + c.Code + ' ' + ISNULL(CONVERT(varchar(10), dateadd(HOUR, 6, PriorityDate),104), N'') 
					FROM ProtectionDocEarlyRegs
					inner join DicCountries c on RegCountryId = c.Id
					WHERE (ProtectionDocId = Results.ProtectionDocId) 
					FOR XML PATH(''), TYPE) AS VARCHAR(max)), 1, 2, ''
				) AS PriorityData
				FROM ProtectionDocEarlyRegs Results
				GROUP BY ProtectionDocId) prioritydataquery on prioritydataquery.ProtectionDocId = pd.id
				left join (SELECT 
					 [ProtectionDocId],
					 STUFF(
 						CAST(
 						(SELECT [text()] = ', ' + substring(i.Code, 1, 2)
 						FROM ICGS_ProtectionDoc
 						inner join DicICGSs i on IcgsId = i.Id
 						WHERE (ProtectionDocId = Results.ProtectionDocId) 
 						FOR XML PATH(''), TYPE) AS VARCHAR(max)), 1, 2, ''
					 ) AS Icgs
					FROM ICGS_ProtectionDoc Results
					GROUP BY ProtectionDocId
				) icgsquery on icgsquery.ProtectionDocId = pd.Id
				left join (SELECT 
					 [ProtectionDocId],
					 STUFF(
						CAST(
						(SELECT [text()] = ', ' + i.Code
						FROM DicIcfem_ProtectionDoc
						inner join DicICFEMs i on DicIcfemId = i.Id
						WHERE (ProtectionDocId = Results.ProtectionDocId) 
						FOR XML PATH(''), TYPE) AS VARCHAR(max)), 1, 2, ''
					 ) AS Icfems
					FROM DicIcfem_ProtectionDoc Results
					GROUP BY ProtectionDocId) icfemquery on icfemquery.ProtectionDocId = pd.Id
				left join (SELECT 
					 [ProtectionDocId],
					 STUFF(
						CAST(
						(SELECT [text()] = ', ' + i.Code
						FROM ICIS_ProtectionDoc
						inner join DicICISs i on IcisId = i.Id
						WHERE (ProtectionDocId = Results.ProtectionDocId) 
						FOR XML PATH(''), TYPE) AS VARCHAR(max)), 1, 2, ''
					 ) AS Icis
					FROM ICIS_ProtectionDoc Results
					GROUP BY ProtectionDocId) icisquery on icisquery.ProtectionDocId = pd.Id
				left join (SELECT 
					 [ProtectionDocId],
					 STUFF(
						CAST(
						(SELECT [text()] = ', ' + i.Code
						FROM IPC_ProtectionDoc
						inner join DicIPCs i on IpcId = i.Id
						WHERE (ProtectionDocId = Results.ProtectionDocId) 
						FOR XML PATH(''), TYPE) AS VARCHAR(max)), 1, 2, ''
					 ) AS Ipcs
					FROM IPC_ProtectionDoc Results
					GROUP BY ProtectionDocId) ipcsquery on ipcsquery.ProtectionDocId = pd.Id
where s.Code <> 'D'
GO