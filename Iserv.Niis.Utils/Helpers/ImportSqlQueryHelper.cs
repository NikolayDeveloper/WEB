namespace Iserv.Niis.Utils.Helpers
{
    public static class ImportSqlQueryHelper
    {
        #region RequestSqlQueries

        public static string RequestsByDateSqlQuery => @"SELECT 
	                                                        ddd.DOCUM_NUM
                                                        FROM dbo.DD_DOCUMENT ddd
                                                        LEFT JOIN dbo.CL_DOCUMENT cld ON cld.U_ID = ddd.DOCTYPE_ID
                                                        WHERE 
	                                                        cld.WORKTYPE_ID IN ({0}) AND ddd.DOCUM_NUM IS NOT NULL AND ddd.INOUT_NUM IS NOT NULL AND ddd.DOCUM_NUM <> ''
	                                                        AND DATEADD(dd, DATEDIFF(dd, 0, ddd.date_create), 0) = CONVERT(DATETIME, '{1}')
                                                        ORDER BY ddd.date_create DESC";

        public static string RequestSqlQuery => @"SELECT 
                                                        ddd.U_ID as Id,
	                                                    ddd.DOCTYPE_ID as DoctypeId,
	                                                    ddd.date_create as DateCreate,
	                                                    ddd.stamp as Stamp,
	                                                    ddd.CUSTOMER_ID as CustomerId,
	                                                    ddd.COPY_COUNT as CopyCount,
	                                                    ddd.PAGE_COUNT as PageCount,
	                                                    ddd.DEPARTMENT_ID as DepartmentId,
	                                                    ddd.DOCUM_NUM as DocumNum,
	                                                    ddd.DOCUM_DATE as DocumDate,
	                                                    ddd.OUTNUM as Outnum,
	                                                    ddd.INOUT_NUM as InoutNum,
	                                                    ddd.DESCRIPTION_ML_RU as DescriptionMlRu,
	                                                    ddd.DESCRIPTION_ML_EN as DescriptionMlEn,
	                                                    ddd.DESCRIPTION_ML_KZ as DescriptionMlKz,
	                                                    ddd.DIVISION_ID as DivisionId,
	                                                    ddd.flDivisionId as FlDivisionId,
	                                                    ddd.USER_ID as UserId,
	                                                    ddd.SENDTYPE as SendType,
	                                                    ddd.INNUM_ADD as InnumAdd,
	                                                    ddd.IS_COMPLETE as IsComplete,
	                                                    btbp.STATUS_ID as StatusId,
	                                                    btbp.TYPEII_ID as TypeiiId,
	                                                    btbp.SELECTION_FAMILY as SelectionFamily,
	                                                    btbp.REF_57 as REF_57,
	                                                    btbp.TRASLITERATION as Trasliteration,
	                                                    btbp.NBY as Nby,
	                                                    btbp.DISCLAM_RU as DisclamRu,
	                                                    btbp.DISCLAM_KZ as DisclamKz,
	                                                    btbp.DATE_85 as Date85,
	                                                    btbp.IMAGE as Image,
	                                                    btbp.SYS_ImageSmall as SysImagesmall,
	                                                    btbp.SUBTYPE_ID as SubtypeId,
                                                        btbp.DBY as Dby,
										                btbp.PUBLICATION_DATE as PublicationDate
                                                    FROM 
	                                                    dbo.DD_DOCUMENT ddd
                                                    LEFT JOIN dbo.BT_BASE_PATENT btbp ON btbp.U_ID = ddd.DOC_ID
                                                    LEFT JOIN dbo.CL_DOCUMENT cld ON cld.U_ID = ddd.DOCTYPE_ID
                                                    WHERE 
	                                                    cld.WORKTYPE_ID IN ({0}) AND (ddd.INOUT_NUM = '{1}' OR ddd.DOCUM_NUM = '{1}') AND ddd.DOCUM_NUM IS NOT NULL AND ddd.INOUT_NUM IS NOT NULL AND ddd.DOCUM_NUM <> ''
                                                    ORDER BY 
                                                        btbp.U_ID";

        public static string WfSqlQuery => @"SELECT 
                                                wf.IS_COMPLETE as IsComplete,
                                                wf.DOCUMENT_ID as DocumentId,
                                                wf.CONTROL_DATE as ControlDate,
                                                wf.date_create as DateCreate,
                                                wf.stamp as Stamp,
                                                wf.TO_STAGE_ID as ToStageId,
                                                wf.TO_USER_ID as ToUserId,
                                                wf.FROM_STAGE_ID as FromStageId,
                                                wf.FROM_USER_ID as FromUserId,
                                                wf.DESCRIPTION as Description,
                                                wf.IS_SYSTEM as IsSystem,
                                                wf.TYPE_ID as TypeId,
	                                            wf.U_ID as Id
                                            FROM 
	                                            dbo.WT_PT_WORKOFFICE wf
                                            LEFT JOIN dbo.DD_DOCUMENT ddd ON ddd.U_ID = wf.DOCUMENT_ID
                                            LEFT JOIN dbo.CL_DOCUMENT cld ON cld.U_ID = ddd.DOCTYPE_ID
                                            WHERE 
	                                            cld.WORKTYPE_ID IN ({0}) AND ddd.INOUT_NUM = '{1}'";

        public static string InfoSqlQuery => @"SELECT 
	                                                ddi.U_ID as Id,
                                                    ddi.date_create as DateCreate,
                                                    ddi.stamp as Stamp,
                                                    ddi.FLAG_NINE as FlagNine,
                                                    ddi.FLAG_TTH as FlagTth,
                                                    ddi.FLAG_TTW as FlagTtw,
                                                    ddi.FLAG_TPT as FlagTpt,
                                                    ddi.FLAG_TAT as FlagTat,
                                                    ddi.FLAG_TN as FlagTn,
                                                    ddi.COL_TZ as ColTz,
                                                    ddi.AWARD_TZ as AwardTz,
                                                    ddi.FONT_TZ as FontTz,
                                                    ddi.D3_TZ as D3Tz,
                                                    ddi.COLOR_TZ as ColorTz,
                                                    ddi.REG_TZ as RegTz,
                                                    ddi.TM_TRANSLIT as TmTranslit,
                                                    ddi.TM_TRANSLATE as TmTranslate,
                                                    ddi.TM_PRIORITET as TmPrioritet,
                                                    ddi.SEL_NOMER as SelNomer,
                                                    ddi.SEL_ROOT as SelRoot,
                                                    ddi.SEL_FAMILY as SelFamily,
                                                    ddi.PN_GOODS as PnGoods,
                                                    ddi.PN_DSC as PnDsc,
                                                    ddi.PN_PLACE as PnPlace,
                                                    ddi.flBreedCountry as FlBreedCountry,
                                                    ddi.flProductSpecialProp as FlProductSpecialProp,
                                                    ddi.flProductPalce as FlProductPalce
                                                FROM 
	                                                dbo.DD_INFO ddi
                                                LEFT JOIN dbo.DD_DOCUMENT ddd ON ddd.U_ID = ddi.U_ID
                                                LEFT JOIN dbo.CL_DOCUMENT cld ON cld.U_ID = ddd.DOCTYPE_ID
                                                WHERE 
	                                                cld.WORKTYPE_ID IN ({0}) AND ddd.INOUT_NUM = '{1}'";

        public static string ColorSqlQuery => @"SELECT DISTINCT
                                                    ddd.U_ID as DocId,
                                                    color.LCFEM_ID as LcfemId
                                                FROM 
	                                                dbo.RF_TM_ICOLOR_TM color
	                                            LEFT JOIN dbo.DD_DOCUMENT ddd ON ddd.DOC_ID = color.DOC_ID
                                                LEFT JOIN dbo.CL_DOCUMENT cld ON cld.U_ID = ddd.DOCTYPE_ID
                                                WHERE 
	                                                cld.WORKTYPE_ID IN ({0}) AND color.U_ID > 0 AND ddd.DOC_ID IS NOT NULL AND ddd.INOUT_NUM = '{1}'";

        public static string DicCustomersSqlQuery => @"WITH Locations(GroupId, F_ID, LocationPathRu, LocationPathEn, LocationPathKz, City, Oblast, Republic, Region) AS   
                                                        (  
	                                                        SELECT 
		                                                        cll.U_ID AS GroupId,
		                                                        cll.F_ID,
		                                                        LocationPathRu = cll.NAME_ML_RU,
		                                                        LocationPathEn = cll.NAME_ML_EN,
		                                                        LocationPathKz = cll.NAME_ML_KZ,
		                                                        City = CASE WHEN CLL.TYPE_ID in (8, 28) THEN cll.NAME_ML_RU END,
		                                                        Oblast = CASE WHEN CLL.TYPE_ID = 175 THEN cll.NAME_ML_RU END,
		                                                        Republic = CASE WHEN CLL.TYPE_ID = 6 THEN cll.NAME_ML_RU END,
		                                                        Region = CASE WHEN CLL.TYPE_ID = 7 THEN cll.NAME_ML_RU END
	                                                        FROM 
		                                                        dbo.CL_LOCATION cll

	                                                        UNION ALL

	                                                        SELECT 
		                                                        l.GroupId,
		                                                        cll.F_ID,
		                                                        LocationPathRu = cll.NAME_ML_RU + ', ' + l.LocationPathRu,
		                                                        LocationPathEn = cll.NAME_ML_En + ', ' + l.LocationPathEn,
		                                                        LocationPathKz = cll.NAME_ML_KZ + ', ' + l.LocationPathKz,
		                                                        City = CASE WHEN CLL.TYPE_ID in (8, 28) THEN cll.NAME_ML_RU ELSE City END,
		                                                        Oblast = CASE WHEN CLL.TYPE_ID = 175 THEN cll.NAME_ML_RU ELSE Oblast END,
		                                                        Republic = CASE WHEN CLL.TYPE_ID = 6 THEN cll.NAME_ML_RU ELSE Republic END,
		                                                        Region = CASE WHEN CLL.TYPE_ID = 7 THEN cll.NAME_ML_RU ELSE Region END
	                                                        FROM 
		                                                        dbo.CL_LOCATION cll
		                                                        INNER JOIN Locations l ON cll.U_ID = l.F_ID
                                                        )    

                                                        SELECT 
	                                                        wtc.U_ID as Id,
	                                                        wtc.flApplicantsInfo as FlApplicantsInfo,
	                                                        wtc.flCertificateNumber as FlCertificateNumber,
	                                                        wtc.flCertificateSeries as FlCertificateSeries,
	                                                        wtc.CONTACT_FACE as ContactFace,
	                                                        wtc.COUNTRY_ID as CountryId, 
	                                                        wtc.date_create as DateCreate,
	                                                        wtc.EMAIL as Email,
	                                                        wtc.flIsSMB as FlIsSmb,
	                                                        wtc.JUR_REG_NUMBER as JurRegNumber,
	                                                        wtc.LOGIN as Login,
	                                                        wtc.CUS_NAME_ML_EN as CusNameMlEn,
	                                                        wtc.CUS_NAME_ML_EN_long as CusNameMlEnLong,
	                                                        wtc.CUS_NAME_ML_KZ as CusNameMlKz,
	                                                        wtc.CUS_NAME_ML_KZ_long as CusNameMlKzLong,
	                                                        wtc.CUS_NAME_ML_RU as CusNameMlRu,
	                                                        wtc.CUS_NAME_ML_RU_long as CusNameMlRuLong,
	                                                        wtc.flNotaryName as FlNotaryName,
	                                                        wtc.flOpf as FlOpf,
	                                                        wtc.PASSWORD_ as Password,
	                                                        wtc.PHONE as Phone,
	                                                        wtc.FAX as Fax,
	                                                        wtc.flPowerAttorneyDateIssue as FlPowerAttorneyDateIssue,
	                                                        wtc.flPowerAttorneyFullNum as FlPowerAttorneyFullNum,
	                                                        wtc.flRegDate as FlRegDate,
	                                                        wtc.RTN as Rtn,
	                                                        wtc.flShortDocContent as FlShortDocContent,
	                                                        wtc.SUBSCRIPT as Subscript,
	                                                        wtc.TYPE_ID as TypeId,
	                                                        wtc.flXIN as Xin,
                                                            wtc.ATT_DATE_BEGIN_STOP as AttDateBeginStop,
	                                                        wtc.ATT_DATE_CARD as AttDateCard,
	                                                        wtc.ATT_DATE_DISCARD as AttDateDiscard,
	                                                        wtc.ATT_DATE_END_STOP as AttDateEndStop,
	                                                        wtc.ATT_EDUCATION as AttEducation,
	                                                        wtc.ATT_SPHERE_WORK as AttSphereWork,
	                                                        wtc.ATT_SPHERE_KNOW as AttSphereKnow,
	                                                        wtc.ATT_STAT_REG as AttStatReg,
	                                                        wtc.ATT_STAT_REG_DATE as AttStatRegDate,
	                                                        wtc.ATT_LANG as AttLang,
	                                                        wtc.ATT_PLATPOR as AttPlatpor,
	                                                        wtc.ATT_PUBLIC_REDEFINE as AttPublicRedefine,
	                                                        wtc.ATT_REDEFINE as AttRedefine,
	                                                        wtc.ATT_CODE as AttCode,
	                                                        wtc.ATT_SOME_DATE as AttSomeDate,
	                                                        wtc.ATT_WORK_PLACE as AttWorkPlace,

	                                                        cll.LocationPathRu + ', ' + wta.ADDRESS_ML_RU AS AddresNameRu,
	                                                        cll.LocationPathKz + ', ' + wta.ADDRESS_ML_KZ AS AddresNameKz,
	                                                        cll.LocationPathEn + ', ' + wta.ADDRESS_ML_EN AS AddresNameEn,
	                                                        cll.City,
	                                                        cll.Oblast,
	                                                        cll.Republic,
	                                                        cll.Region
                                                        FROM 
	                                                        dbo.WT_CUSTOMER wtc
	                                                        LEFT JOIN dbo.WT_ADDRESS wta ON wta.U_ID = wtc.ADDRESS_ID
	                                                        LEFT JOIN Locations cll ON cll.GroupId = wta.LOCATION_ID AND cll.F_ID IS NULL
                                                        WHERE wtc.U_ID = {0}
                                                        ORDER BY
	                                                        wtc.U_ID";

        public static string CustomersSqlQuery => @"WITH Locations(GroupId, F_ID, LocationPathRu, LocationPathEn, LocationPathKz) AS   
                                                        (  
	                                                        SELECT 
		                                                        cll.U_ID AS GroupId,
		                                                        cll.F_ID,
		                                                        LocationPathRu = cll.NAME_ML_RU,
		                                                        LocationPathEn = cll.NAME_ML_EN,
		                                                        LocationPathKz = cll.NAME_ML_KZ
	                                                        FROM 
		                                                        dbo.CL_LOCATION cll

	                                                        UNION ALL

	                                                        SELECT 
		                                                        l.GroupId,
		                                                        cll.F_ID,
		                                                        LocationPathRu = cll.NAME_ML_RU + ', ' + l.LocationPathRu,
		                                                        LocationPathEn = cll.NAME_ML_En + ', ' + l.LocationPathEn,
		                                                        LocationPathKz = cll.NAME_ML_KZ + ', ' + l.LocationPathKz
	                                                        FROM 
		                                                        dbo.CL_LOCATION cll
		                                                        INNER JOIN Locations l ON cll.U_ID = l.F_ID
                                                        ) 

                                                        SELECT 
                                                            ddd.U_ID AS DocId,
                                                            cust.C_TYPE as CType,
                                                            cust.U_ID as Id,
                                                            cust.CUSTOMER_ID as CustomerId,
                                                            cust.MENTION as Mention,
                                                            cust.DATE_BEGIN as DateBegin,
                                                            cust.DATE_END as DateEnd,
                                                            cust.stamp as Stamp,
                                                            cust.date_create as DateCreate,

	                                                        cll.LocationPathRu + ', ' + wta.ADDRESS_ML_RU AS AddresNameRu,
	                                                        cll.LocationPathEn + ', ' + wta.ADDRESS_ML_EN AS AddresNameEn,
	                                                        cll.LocationPathKz + ', ' + wta.ADDRESS_ML_KZ AS AddresNameKz
                                                        FROM 
	                                                        dbo.RF_CUSTOMER cust
                                                        LEFT JOIN dbo.DD_DOCUMENT ddd ON ddd.DOC_ID = cust.DOC_ID
                                                        LEFT JOIN dbo.CL_DOCUMENT cld ON cld.U_ID = ddd.DOCTYPE_ID
                                                        LEFT JOIN dbo.WT_ADDRESS wta ON wta.U_ID = cust.ADDRESS_ID
                                                        LEFT JOIN Locations cll ON cll.GroupId = wta.LOCATION_ID AND cll.F_ID IS NULL

                                                        WHERE 
	                                                        cld.WORKTYPE_ID IN ({0}) AND ddd.INOUT_NUM = '{1}'";

        public static string IcfemsSqlQuery => @"SELECT DISTINCT
                                                    ddd.U_ID AS DOC_ID,
                                                    icfem.LCFEM_ID as LcfemId
                                                FROM 
	                                                dbo.RF_TM_ICFEM icfem
                                                LEFT JOIN dbo.DD_DOCUMENT ddd ON ddd.DOC_ID = icfem.DOC_ID
                                                LEFT JOIN dbo.CL_DOCUMENT cld ON cld.U_ID = ddd.DOCTYPE_ID
                                                WHERE 
	                                                cld.WORKTYPE_ID IN ({0}) AND icfem.LCFEM_ID IS NOT NULL AND ddd.INOUT_NUM = '{1}'";

        public static string IcgsSqlQuery => @"SELECT 
                                                    ddd.U_ID AS DocId,
                                                    icgs.U_ID as Id,
	                                                icgs.ICPS_ID as IcpsId,
	                                                icgs.IS_NEGATIVE as IsNegative,
	                                                icgs.flDscStarted as FlDscStarted,
	                                                icgs.DSC as Dsc,
	                                                icgs.DSC_KZ as DscKz,
	                                                icgs.flIsNegativePartial as FlIsNegativePartial,
	                                                icgs.date_create as DateCreate,
	                                                icgs.stamp as Stamp
                                                FROM 
	                                                dbo.RF_TM_ICGS icgs
                                                LEFT JOIN dbo.DD_DOCUMENT ddd ON ddd.DOC_ID = icgs.DOC_ID
                                                LEFT JOIN dbo.CL_DOCUMENT cld ON cld.U_ID = ddd.DOCTYPE_ID
                                                WHERE 
	                                                cld.WORKTYPE_ID IN ({0}) AND icgs.ICPS_ID IS NOT NULL AND ddd.INOUT_NUM = '{1}'";

        public static string IcisSqlQuery => @"SELECT 
                                                    ddd.U_ID AS PatentId,
                                                    icis.U_ID as Id,
	                                                icis.date_create as DateCreate,
	                                                icis.TYPE_ID as TypeId
                                                FROM 
	                                                dbo.RF_ICIS icis
                                                LEFT JOIN dbo.DD_DOCUMENT ddd ON ddd.DOC_ID = icis.PATENT_ID
                                                LEFT JOIN dbo.CL_DOCUMENT cld ON cld.U_ID = ddd.DOCTYPE_ID
                                                WHERE 
	                                                cld.WORKTYPE_ID IN ({0}) AND icis.TYPE_ID IS NOT NULL AND ddd.INOUT_NUM = '{1}'";

        public static string IpcSqlQuery => @"SELECT 
                                                    ddd.U_ID AS PatentId,
                                                    ipc.U_ID as Id,
	                                                ipc.flIsMain as FlIsMain,
	                                                ipc.stamp as Stamp,
	                                                ipc.date_create as DateCreate,
	                                                ipc.TYPE_ID as TypeId
                                                FROM 
	                                                dbo.RF_IPC ipc
                                                LEFT JOIN dbo.DD_DOCUMENT ddd ON ddd.DOC_ID = ipc.PATENT_ID
                                                LEFT JOIN dbo.CL_DOCUMENT cld ON cld.U_ID = ddd.DOCTYPE_ID
                                                WHERE 
	                                                cld.WORKTYPE_ID IN ({0}) AND ddd.INOUT_NUM = '{1}'";

        public static string EarlyRegsSqlQuery => @"SELECT 
                                                        ddd.U_ID AS DocId,
                                                        early.U_ID as Id,
	                                                    early.ETYPE_ID as EtypeId,
	                                                    early.REQ_COUNTRY as ReqCountry,
	                                                    early.date_create as DateCreate,
	                                                    early.REQ_NUMBER as ReqNumber,
                                                        early.REQ_DATE as ReqDate,
	                                                    early.DESCRIPTION as Description,
	                                                    early.SA_NAME as SaName
                                                    FROM 
	                                                    dbo.WT_PT_EARLYREG early
                                                    LEFT JOIN dbo.DD_DOCUMENT ddd ON ddd.DOC_ID = early.DOC_ID
                                                    LEFT JOIN dbo.CL_DOCUMENT cld ON cld.U_ID = ddd.DOCTYPE_ID
                                                    WHERE 
	                                                    cld.WORKTYPE_ID IN ({0}) AND early.ETYPE_ID IS NOT NULL AND ddd.INOUT_NUM = '{1}'";

        public static string BeneficiarySqlQuery => @"BEGIN TRY
	                                                        BEGIN TRANSACTION

	                                                        UPDATE req 
	                                                        SET req.BeneficiaryTypeId = (SELECT TOP(1) dbo.DicBeneficiaryTypes.Id FROM dbo.DicBeneficiaryTypes WHERE dbo.DicBeneficiaryTypes.Code = 'SMB')
	                                                        FROM dbo.Requests req
	                                                        INNER JOIN dbo.RequestCustomers rc
		                                                        ON rc.RequestId = req.Id AND rc.CustomerRoleId = (SELECT TOP(1) dbo.DicCustomerRoles.Id FROM dbo.DicCustomerRoles WHERE dbo.DicCustomerRoles.Code = '1')
	                                                        INNER JOIN dbo.DicCustomers dc
		                                                        ON dc.Id = rc.CustomerId AND dc.IsSMB = 0
	                                                        WHERE req.IncomingNumber = '{0}'
	                                                        COMMIT
                                                        END TRY
                                                        BEGIN CATCH
                                                            ROLLBACK
                                                        END CATCH";

        public static string CurrentWorkflowSqlQuery => @"BEGIN TRY
	                                                            BEGIN TRANSACTION

	                                                            UPDATE req 
	                                                            SET req.CurrentWorkflowId = 
		                                                            (SELECT TOP (1)
			                                                            t.Id
		                                                            FROM (SELECT 
			                                                            OwnerId, 
			                                                            MAX(DateCreate) AS maxDate
		                                                            FROM 
			                                                            dbo.RequestWorkflows
		                                                            WHERE 
			                                                            req.Id = OwnerId
		                                                            GROUP BY 
			                                                            OwnerId) as m
		                                                            INNER JOIN dbo.RequestWorkflows as t ON t.OwnerId = m.OwnerId AND t.DateCreate = m.maxDate AND t.OwnerId = req.Id)
	                                                            FROM dbo.Requests req
														        WHERE req.Id = {0}
	                                                            COMMIT
                                                            END TRY
                                                            BEGIN CATCH
                                                                ROLLBACK
                                                            END CATCH";

        public static string AttachmentsSqlQuery => @"SELECT 
	                                                        [SYS_ANY_DATA] AS [FileName],
	                                                        [ANY_DATA] AS [File]
                                                        FROM 
	                                                        [DD_DOCUMENT_DATA]
                                                        WHERE 
												            [U_ID] IN ({0}) AND
	                                                        [ANY_DATA] IS NOT NULL AND
												            DATALENGTH([TEMPLATE_DATA]) > 0

                                                        UNION ALL

                                                        SELECT 
                                                            [SYS_TEMPLATE_DATA] AS [FileName],
                                                            [TEMPLATE_DATA] AS [File]
                                                        FROM 
	                                                        [DD_DOCUMENT_DATA]
                                                        WHERE 
												            [U_ID] IN ({0}) AND
												            [SYS_TEMPLATE_DATA] IS NOT NULL AND 
												            DATALENGTH([TEMPLATE_DATA]) > 0";

        public static string SetRequestMainAttachmentSqlQuery => @"BEGIN TRY
                                                                        BEGIN TRANSACTION 

	                                                                    DECLARE @externalId int = (SELECT TOP(1) r.ExternalId FROM dbo.Requests r WHERE r.Id = {0})

	                                                                    DECLARE @attachmentId int
	                                                                    IF (SELECT MAX(a.id) FROM dbo.Attachments a WHERE a.ExternalId = @externalId AND a.ValidName LIKE N'%.pdf') IS NOT NULL
		                                                                    set @attachmentId = (SELECT MAX(a.id) FROM dbo.Attachments a WHERE a.ExternalId = @externalId AND a.ValidName LIKE N'%.pdf')
	                                                                    ELSE IF (SELECT MAX(a.id) FROM dbo.Attachments a WHERE a.ExternalId = @externalId AND a.ValidName LIKE N'%.docx') IS NOT NULL
		                                                                    set @attachmentId = (SELECT MAX(a.id) FROM dbo.Attachments a WHERE a.ExternalId = @externalId AND a.ValidName LIKE N'%.docx')
	                                                                    ELSE IF (SELECT MAX(a.id) FROM dbo.Attachments a WHERE a.ExternalId = @externalId AND a.ValidName LIKE N'%.doc') IS NOT NULL
		                                                                    set @attachmentId = (SELECT MAX(a.id) FROM dbo.Attachments a WHERE a.ExternalId = @externalId AND a.ValidName LIKE N'%.doc')
	                                                                    ELSE IF (SELECT MAX(a.id) FROM dbo.Attachments a WHERE a.ExternalId = @externalId AND a.ValidName LIKE N'%.odt') IS NOT NULL
		                                                                    set @attachmentId = (SELECT MAX(a.id) FROM dbo.Attachments a WHERE a.ExternalId = @externalId AND a.ValidName LIKE N'%.odt')
	                                                                    ELSE IF (SELECT MAX(a.id) FROM dbo.Attachments a WHERE a.ExternalId = @externalId)  IS NOT NULL
		                                                                    set @attachmentId = (SELECT MAX(a.id) FROM dbo.Attachments a WHERE a.ExternalId = @externalId)

                                                                        UPDATE r 
		                                                                    SET r.MainAttachmentId = @attachmentId
	                                                                    FROM 
		                                                                    dbo.Requests r
		                                                                    WHERE r.Id = {0}
	                                                                        COMMIT
                                                                        END TRY
                                                                    BEGIN CATCH
                                                                        ROLLBACK
                                                                    END CATCH";

        #endregion

        #region PaymentsSqlQuery

        public static string PaymentInvoicesSqlQuery => @"SELECT 
                                                                fp.U_ID as Id,
                                                                fp.TARIFF_ID as TariffId,
                                                                fp.FINE_PERCENT as FinePercent,
                                                                fp.VAT_PERCENT as VatPercent,
                                                                ddd.U_ID as AppId,
                                                                fp.date_create as DateCreate,
                                                                fp.stamp as Stamp,
                                                                fp.PENI_PERCENT as PeniPercent,
                                                                fp.TARIFF_COUNT as TariffCount,
                                                                fp.IS_COMPLETE as IsComplete,
                                                                fp.DATE_LIMIT as DateLimit,
                                                                fp.DATE_FACT as DateFact,
	                                                            dbo.GET_FIXPAYREMAINDER(fp.U_ID) AS FixPayRemainder,
                                                                fp.DATE_COMPLETE as DateComplete,
                                                                fp.flCreateUserId as FlCreateUserId
                                                            FROM 
                                                                dbo.WT_PL_FIXPAYMENT fp
                                                            LEFT JOIN dbo.DD_DOCUMENT ddd ON fp.APP_ID = ddd.DOC_ID
		                                                    LEFT JOIN dbo.CL_DOCUMENT cld ON cld.U_ID = ddd.DOCTYPE_ID
                                                            WHERE 
                                                                fp.U_ID > 0 AND cld.WORKTYPE_ID IN ({0}) AND ddd.INOUT_NUM = '{1}'";

        public static string PaymentsSqlQuery => @"SELECT 
	                                                    p.U_ID as Id,
                                                        p.date_create as DateCreate,
	                                                    p.CUSTOMER_ID AS CustomerId,
	                                                    p.PAYMENT_TYPE AS PaymentType,
	                                                    p.PAYMENT_AMOUNT AS PaymentAmount,
	                                                    p.PAYMENT_DATE AS PaymentDate,
	                                                    p.PAYMENT_NUMB AS PaymentNumb,
	                                                    p.IS_AVANS AS IsAvans,
	                                                    p.DSC AS Dsc,
	                                                    p.USE_DSC AS UseDsc,
	                                                    p.flValSum AS FlValSum,
	                                                    p.flExchangeRate AS FlExchangeRate,
	                                                    p.flValType AS FlValType
                                                    FROM 
                                                        dbo.WT_PL_PAYMENT p
                                                    LEFT JOIN dbo.WT_PL_PAYMENT_USE u ON u.PAYMENT_ID = p.U_ID
                                                    LEFT JOIN dbo.WT_PL_FIXPAYMENT fp ON fp.U_ID = u.FIX_ID
                                                    LEFT JOIN dbo.DD_DOCUMENT ddd ON fp.APP_ID = ddd.DOC_ID
                                                    LEFT JOIN dbo.CL_DOCUMENT cld ON cld.U_ID = ddd.DOCTYPE_ID
                                                    WHERE
                                                        p.U_ID > 0 AND cld.WORKTYPE_ID IN ({0}) AND ddd.INOUT_NUM = '{1}'";

        public static string PaymentUses => @"SELECT 
                                                    u.U_ID AS Id,
	                                                u.PAYMENT_ID AS PaymentId,
	                                                u.FIX_ID AS FixId,
	                                                u.AMOUNT AS Amount,
	                                                u.date_create AS DateCreate,
	                                                u.DSC AS Dsc
                                                FROM 
                                                    dbo.WT_PL_PAYMENT_USE u
                                                    LEFT JOIN dbo.WT_PL_FIXPAYMENT fp ON fp.U_ID = u.FIX_ID
                                                    LEFT JOIN dbo.DD_DOCUMENT ddd ON fp.APP_ID = ddd.DOC_ID
                                                    LEFT JOIN dbo.CL_DOCUMENT cld ON cld.U_ID = ddd.DOCTYPE_ID
                                                WHERE 
                                                    u.U_ID > 0 AND u.PAYMENT_ID > 0 AND cld.WORKTYPE_ID IN ({0}) AND ddd.INOUT_NUM = '{1}'";

        #endregion

        #region ContractSqlQuery

        public static string ContractRefSqlQuery => @"SELECT 
                                                            rfd.U_ID as Id,
                                                            rfd.date_create as DateCreate,
                                                            rfd.DOCUMENT_ID as DocumentId,
                                                            rfd.REFDOCUMENT_ID as RefdocumentId
                                                        FROM 
                                                            dbo.RF_MESSAGE_DOCUMENT rfd
                                                            LEFT JOIN dbo.DD_DOCUMENT ddd ON ddd.U_ID = rfd.DOCUMENT_ID
                                                            LEFT JOIN dbo.CL_DOCUMENT clDoc ON clDoc.U_ID = ddd.DOCTYPE_ID
                                                            LEFT JOIN dbo.DD_DOCUMENT dddRef ON dddRef.U_ID = rfd.REFDOCUMENT_ID
                                                            LEFT JOIN dbo.CL_DOCUMENT clDocRef ON clDocRef.U_ID = dddRef.DOCTYPE_ID
                                                        WHERE 
	                                                        rfd.DOCUMENT_ID IS NOT NULL AND rfd.REFDOCUMENT_ID IS NOT NULL AND dddRef.DOC_ID IS NOT NULL AND
                                                            clDocRef.WORKTYPE_ID IN ({1}) AND 
	                                                        clDoc.WORKTYPE_ID IN ({0}) AND ddd.INOUT_NUM = '{2}'";

        public static string ContractSqlQuery => @"SELECT 
                                                        ddd.U_ID AS Id,
	                                                    ddd.DOCTYPE_ID AS DoctypeId,
                                                        clDoc.CODE as DoctypeCode,
	                                                    ddd.DESCRIPTION_ML_RU AS DescriptionMlRu,
	                                                    ddd.DESCRIPTION_ML_EN AS DescriptionMlEn,
	                                                    ddd.DESCRIPTION_ML_KZ AS DescriptionMlKz,
														ddd.CUSTOMER_ID AS CustomerId,
	                                                    ddd.date_create AS DateCreate,
	                                                    ddd.stamp AS Stamp,
	                                                    ddd.SENDTYPE AS SendType,
	                                                    ddd.OUTNUM AS Outnum,
	                                                    ddd.COPY_COUNT AS CopyCount,
	                                                    ddd.PAGE_COUNT AS PageCount,
	                                                    ddd.DEPARTMENT_ID AS DepartmentId,
	                                                    ddd.DIVISION_ID AS DivisionId,
	                                                    btbp.DBY AS Dby,
	                                                    btbp.NBY AS Nby,
	                                                    btbp.GOS_DATE_11 AS GosDate11,
	                                                    btbp.GOS_NUMBER_11 AS GosNumber11,
	                                                    btbp.REQ_DATE_22 AS ReqDate22,
	                                                    btbp.STATUS_ID AS StatusId,
	                                                    btbp.STZ17 AS Stz17,
	                                                    btbp.REQ_NUMBER_21 AS ReqNumber21,
                                                        btbp.SUBTYPE_ID 
                                                    FROM 
	                                                    dbo.DD_DOCUMENT ddd
                                                    LEFT JOIN dbo.BT_BASE_PATENT btbp ON btbp.U_ID = ddd.DOC_ID
                                                    LEFT JOIN dbo.CL_DOCUMENT clDoc ON clDoc.U_ID = ddd.DOCTYPE_ID
                                                    WHERE 
	                                                     clDoc.WORKTYPE_ID IN ({0}) AND ddd.U_ID = {1}";

        public static string ContractsWfSqlQuery => @"SELECT
	                                                        wpw.IS_COMPLETE as IsComplete,
                                                            wpw.DOCUMENT_ID as DocumentId,
                                                            wpw.CONTROL_DATE as ControlDate,
                                                            wpw.date_create as DateCreate,
                                                            wpw.stamp as Stamp,
                                                            wpw.TO_STAGE_ID as ToStageId,
                                                            wpw.TO_USER_ID as ToUserId,
                                                            wpw.FROM_STAGE_ID as FromStageId,
                                                            wpw.FROM_USER_ID as FromUserId,
                                                            wpw.DESCRIPTION as Description,
                                                            wpw.IS_SYSTEM as IsSystem,
                                                            wpw.TYPE_ID as TypeId,
	                                                        wpw.U_ID as Id
                                                        FROM 
	                                                        dbo.WT_PT_WORKOFFICE wpw
                                                        LEFT JOIN dbo.DD_DOCUMENT ddd ON ddd.U_ID = wpw.DOCUMENT_ID
                                                        LEFT JOIN dbo.CL_DOCUMENT clDoc ON clDoc.U_ID = ddd.DOCTYPE_ID
                                                        WHERE 
	                                                        clDoc.WORKTYPE_ID IN ({0}) AND wpw.DOCUMENT_ID = {1}";

        public static string ContractCustomersSqlQuery => @"WITH Locations(GroupId, F_ID, LocationPathRu, LocationPathEn, LocationPathKz) AS   
                                                                (  
	                                                                SELECT 
		                                                                cll.U_ID AS GroupId,
		                                                                cll.F_ID,
		                                                                LocationPathRu = cll.NAME_ML_RU,
		                                                                LocationPathEn = cll.NAME_ML_EN,
		                                                                LocationPathKz = cll.NAME_ML_KZ
	                                                                FROM 
		                                                                dbo.CL_LOCATION cll

	                                                                UNION ALL

	                                                                SELECT 
		                                                                l.GroupId,
		                                                                cll.F_ID,
		                                                                LocationPathRu = cll.NAME_ML_RU + ', ' + l.LocationPathRu,
		                                                                LocationPathEn = cll.NAME_ML_En + ', ' + l.LocationPathEn,
		                                                                LocationPathKz = cll.NAME_ML_KZ + ', ' + l.LocationPathKz
	                                                                FROM 
		                                                                dbo.CL_LOCATION cll
		                                                                INNER JOIN Locations l ON cll.U_ID = l.F_ID
                                                                ) 

                                                                SELECT 
                                                                     ddd.U_ID AS DocId,
                                                                    cust.C_TYPE as CType,
                                                                    cust.U_ID as Id,
                                                                    cust.CUSTOMER_ID as CustomerId,
                                                                    cust.MENTION as Mention,
                                                                    cust.DATE_BEGIN as DateBegin,
                                                                    cust.DATE_END as DateEnd,
                                                                    cust.stamp as Stamp,
                                                                    cust.date_create as DateCreate,

	                                                                cll.LocationPathRu + ', ' + wta.ADDRESS_ML_RU AS AddresNameRu,
	                                                                cll.LocationPathEn + ', ' + wta.ADDRESS_ML_EN AS AddresNameEn,
	                                                                cll.LocationPathKz + ', ' + wta.ADDRESS_ML_KZ AS AddresNameKz
                                                                FROM 
	                                                                dbo.RF_CUSTOMER cust
                                                                LEFT JOIN dbo.DD_DOCUMENT ddd ON ddd.DOC_ID = cust.DOC_ID
                                                                LEFT JOIN dbo.CL_DOCUMENT cld ON cld.U_ID = ddd.DOCTYPE_ID
                                                                LEFT JOIN dbo.WT_ADDRESS wta ON wta.U_ID = cust.ADDRESS_ID
                                                                LEFT JOIN Locations cll ON cll.GroupId = wta.LOCATION_ID AND cll.F_ID IS NULL
                                                                WHERE 
	                                                                cld.WORKTYPE_ID IN ({0}) AND ddd.U_ID = {1}";

        public static string ContractsCurrentWorkflowSqlQuery => @"BEGIN TRY
	                                                                    BEGIN TRANSACTION

	                                                                    UPDATE req 
	                                                                    SET req.CurrentWorkflowId = 
		                                                                    (SELECT TOP (1)
			                                                                    t.Id
		                                                                    FROM (SELECT 
			                                                                    OwnerId, 
			                                                                    MAX(DateCreate) AS maxDate
		                                                                    FROM 
			                                                                    dbo.ContractWorkflows
		                                                                    WHERE 
			                                                                    req.Id = OwnerId
		                                                                    GROUP BY 
			                                                                    OwnerId) as m
		                                                                    INNER JOIN dbo.ContractWorkflows as t ON t.OwnerId = m.OwnerId AND t.DateCreate = m.maxDate AND t.OwnerId = req.Id)
	                                                                    FROM dbo.Contracts req
	                                                                    WHERE req.Id = {0}
	                                                                    COMMIT
                                                                    END TRY
                                                                    BEGIN CATCH
                                                                        ROLLBACK
                                                                    END CATCH";

        public static string SetContractsMainAttachmentSqlQuery => @"BEGIN TRY
                                                                        BEGIN TRANSACTION 

	                                                                    DECLARE @externalId int = (SELECT TOP(1) c.ExternalId FROM dbo.Contracts c WHERE c.Id = {0})

	                                                                    DECLARE @attachmentId int
	                                                                    IF (SELECT MAX(a.id) FROM dbo.Attachments a WHERE a.ExternalId = @externalId AND a.ValidName LIKE N'%.pdf') IS NOT NULL
		                                                                    set @attachmentId = (SELECT MAX(a.id) FROM dbo.Attachments a WHERE a.ExternalId = @externalId AND a.ValidName LIKE N'%.pdf')
	                                                                    ELSE IF (SELECT MAX(a.id) FROM dbo.Attachments a WHERE a.ExternalId = @externalId AND a.ValidName LIKE N'%.docx') IS NOT NULL
		                                                                    set @attachmentId = (SELECT MAX(a.id) FROM dbo.Attachments a WHERE a.ExternalId = @externalId AND a.ValidName LIKE N'%.docx')
	                                                                    ELSE IF (SELECT MAX(a.id) FROM dbo.Attachments a WHERE a.ExternalId = @externalId AND a.ValidName LIKE N'%.doc') IS NOT NULL
		                                                                    set @attachmentId = (SELECT MAX(a.id) FROM dbo.Attachments a WHERE a.ExternalId = @externalId AND a.ValidName LIKE N'%.doc')
	                                                                    ELSE IF (SELECT MAX(a.id) FROM dbo.Attachments a WHERE a.ExternalId = @externalId AND a.ValidName LIKE N'%.odt') IS NOT NULL
		                                                                    set @attachmentId = (SELECT MAX(a.id) FROM dbo.Attachments a WHERE a.ExternalId = @externalId AND a.ValidName LIKE N'%.odt')
	                                                                    ELSE IF (SELECT MAX(a.id) FROM dbo.Attachments a WHERE a.ExternalId = @externalId)  IS NOT NULL
		                                                                    set @attachmentId = (SELECT MAX(a.id) FROM dbo.Attachments a WHERE a.ExternalId = @externalId)

                                                                        UPDATE c
		                                                                    SET c.MainAttachmentId = @attachmentId
	                                                                    FROM 
		                                                                    dbo.Contracts c
		                                                                    WHERE c.Id = {0}
	                                                                        COMMIT
                                                                        END TRY
                                                                    BEGIN CATCH
                                                                        ROLLBACK
                                                                    END CATCH";

        #endregion

        #region DocumentSqlQuery

        public static string DocumentSqlQuery => @"SELECT 
                                                        ddd.U_ID as Id,
                                                        ddd.DOCTYPE_ID as DoctypeId,
                                                        clDoc.CODE as DoctypeCode,
                                                        ddd.date_create as DateCreate,
                                                        ddd.stamp as Stamp,
                                                        ddd.CUSTOMER_ID as CustomerId,
                                                        ddd.DEPARTMENT_ID as DepartmentId,
                                                        ddd.DOCUM_NUM as DocumNum,
                                                        ddd.OUTNUM as Outnum,
                                                        ddd.INOUT_NUM as InoutNum,
                                                        ddd.DOCUM_DATE as DocumDate,
                                                        ddd.PAGE_COUNT as PageCount,
                                                        ddd.DESCRIPTION_ML_RU as DescriptionMlRu,
                                                        ddd.DESCRIPTION_ML_EN as DescriptionMlEn,
                                                        ddd.DESCRIPTION_ML_KZ as DescriptionMlKz,
                                                        ddd.DIVISION_ID as DivisionId,
                                                        ddd.SENDTYPE as SendType,
                                                        ddd.INNUM_ADD as InnumAdd,
                                                        ddd.IS_COMPLETE as IsComplete,
	                                                    wt.CODE AS WorkTypeCode
                                                    FROM dbo.DD_DOCUMENT ddd
                                                    LEFT JOIN dbo.CL_DOCUMENT clDoc ON clDoc.U_ID = ddd.DOCTYPE_ID
                                                    LEFT JOIN dbo.DD_WORKTYPE wt ON wt.U_ID = clDoc.WORKTYPE_ID
                                                    WHERE 
                                                        clDoc.WORKTYPE_ID IN ({0}) AND ddd.U_ID = {1}";

        public static string RefSqlQuery => @"SELECT DISTINCT
                                                    rfd.U_ID as Id,
                                                    rfd.date_create as DateCreate,
                                                    rfd.DOCUMENT_ID as DocumentId,
                                                    rfd.REFDOCUMENT_ID as RefdocumentId
                                                FROM 
                                                    dbo.RF_MESSAGE_DOCUMENT rfd
                                                    LEFT JOIN dbo.DD_DOCUMENT ddd ON ddd.U_ID = rfd.DOCUMENT_ID
                                                    LEFT JOIN dbo.CL_DOCUMENT clDoc ON clDoc.U_ID = ddd.DOCTYPE_ID
                                                    LEFT JOIN dbo.DD_DOCUMENT dddRef ON dddRef.U_ID = rfd.REFDOCUMENT_ID
                                                    LEFT JOIN dbo.CL_DOCUMENT clDocRef ON clDocRef.U_ID = dddRef.DOCTYPE_ID
                                                WHERE 
	                                                rfd.DOCUMENT_ID IS NOT NULL AND rfd.REFDOCUMENT_ID IS NOT NULL AND
                                                    clDoc.WORKTYPE_ID IN ({0}) AND ddd.INOUT_NUM = '{2}' AND
	                                                clDocRef.WORKTYPE_ID IN ({1})";

        public static string ContractDocRefSqlQuery => @"SELECT DISTINCT
                                                            rfd.U_ID as Id,
                                                            rfd.date_create as DateCreate,
                                                            rfd.DOCUMENT_ID as DocumentId,
                                                            rfd.REFDOCUMENT_ID as RefdocumentId
                                                        FROM 
                                                            dbo.RF_MESSAGE_DOCUMENT rfd
                                                            LEFT JOIN dbo.DD_DOCUMENT ddd ON ddd.U_ID = rfd.DOCUMENT_ID
                                                            LEFT JOIN dbo.CL_DOCUMENT clDoc ON clDoc.U_ID = ddd.DOCTYPE_ID
                                                            LEFT JOIN dbo.DD_DOCUMENT dddRef ON dddRef.U_ID = rfd.REFDOCUMENT_ID
                                                            LEFT JOIN dbo.CL_DOCUMENT clDocRef ON clDocRef.U_ID = dddRef.DOCTYPE_ID
                                                        WHERE 
	                                                        rfd.DOCUMENT_ID IS NOT NULL AND rfd.REFDOCUMENT_ID IS NOT NULL AND
                                                            clDoc.WORKTYPE_ID IN ({0}) AND ddd.U_ID = {2} AND
	                                                        clDocRef.WORKTYPE_ID IN ({1})";

        public static string DocumentsWfSqlQuery => @"SELECT 
                                                            wf.IS_COMPLETE as IsComplete,
                                                            wf.DOCUMENT_ID as DocumentId,
                                                            wf.CONTROL_DATE as ControlDate,
                                                            wf.date_create as DateCreate,
                                                            wf.stamp as Stamp,
                                                            wf.TO_STAGE_ID as ToStageId,
                                                            wf.TO_USER_ID as ToUserId,
                                                            wf.FROM_STAGE_ID as FromStageId,
                                                            wf.FROM_USER_ID as FromUserId,
                                                            wf.DESCRIPTION as Description,
                                                            wf.IS_SYSTEM as IsSystem,
                                                            wf.TYPE_ID as TypeId,
	                                                        wf.U_ID as Id
                                                        FROM 
	                                                        dbo.WT_PT_WORKOFFICE wf
                                                        LEFT JOIN dbo.DD_DOCUMENT ddd ON ddd.U_ID = wf.DOCUMENT_ID
	                                                    LEFT JOIN dbo.CL_DOCUMENT clDoc ON clDoc.U_ID = ddd.DOCTYPE_ID
                                                        WHERE 
	                                                       clDoc.WORKTYPE_ID IN ({0}) AND wf.DOCUMENT_ID = {1}";

        public static string UserSignatureSqlQuery => @"SELECT 
	                                                        userSign.flUserId as FlUserId,
                                                            userSign.U_ID as Id,
                                                            userSign.flFingerPrint as FlFingerPrint,
                                                            userSign.flSignedData as FlSignedData,
                                                            userSign.flSignerCertificate as FlSignerCertificate,
                                                            userSign.flSignDate as FlSignDate,
	                                                        wff.U_ID AS FlDocUId 
                                                        FROM 
	                                                        dbo.tbDocumentUsersSignature userSign
	                                                        INNER JOIN 
                                                        (SELECT 
	                                                        wf.U_ID,
	                                                        wf.TO_USER_ID,
	                                                        wf.DOCUMENT_ID
                                                        FROM 
	                                                        dbo.tbDocumentUsersSignature userSign
	                                                        LEFT JOIN dbo.WT_PT_WORKOFFICE wf ON wf.TO_USER_ID = userSign.flUserId AND wf.DOCUMENT_ID = userSign.flDocUId
                                                        GROUP BY 
	                                                        wf.U_ID, wf.TO_USER_ID, wf.DOCUMENT_ID
                                                        HAVING 
	                                                        COUNT(userSign.U_ID) <= 1) AS wff ON wff.TO_USER_ID = userSign.flUserId AND wff.DOCUMENT_ID = userSign.flDocUId
	            
                                                            LEFT JOIN dbo.DD_DOCUMENT ddd ON ddd.U_ID = wff.DOCUMENT_ID
                                                            LEFT JOIN dbo.CL_DOCUMENT clDoc ON clDoc.U_ID = ddd.DOCTYPE_ID
                                                            WHERE 
	                                                            clDoc.WORKTYPE_ID IN ({0}) AND ddd.U_ID = {1}";

        public static string DocumentsCurrentWorkflowSqlQuery => @"BEGIN TRY
	                                                                    BEGIN TRANSACTION

	                                                                    UPDATE req 
	                                                                    SET req.IsCurent = 1
	                                                                    FROM dbo.DocumentWorkflows req
	                                                                    WHERE id =
		                                                                    (SELECT TOP (1)
			                                                                    t.Id
		                                                                    FROM (SELECT 
			                                                                    OwnerId, 
			                                                                    MAX(DateCreate) AS maxDate
		                                                                    FROM 
			                                                                    dbo.DocumentWorkflows
		                                                                    WHERE 
			                                                                    req.OwnerId = OwnerId
		                                                                    GROUP BY 
			                                                                    OwnerId) as m
		                                                                    INNER JOIN dbo.DocumentWorkflows as t ON t.OwnerId = m.OwnerId AND t.DateCreate = m.maxDate AND t.OwnerId = req.OwnerId) AND OwnerId = {0}

	                                                                    COMMIT
                                                                    END TRY
                                                                    BEGIN CATCH
                                                                        ROLLBACK
                                                                    END CATCH
                                                                    ";

        public static string DocumentsSetStatusSqlQuery => @"BEGIN TRY
	                                                            BEGIN TRANSACTION
	                                                            UPDATE d

	                                                            SET d.StatusId = (SELECT TOP(1) Id FROM dbo.DicDocumentStatuses WHERE Code = '1')
	                                                            FROM
		                                                            dbo.Documents d
		                                                            LEFT JOIN dbo.DocumentWorkflows dwf ON dwf.OwnerId = d.Id AND dwf.IsCurent = 1
		                                                            WHERE d.Id = {0} and d.StatusId = (SELECT TOP(1) Id FROM dbo.DicDocumentStatuses WHERE Code = '2') AND dwf.Id IS NOT NULL AND d.ExternalId IS NOT NULL AND dwf.ExternalId IS NOT NULL
	                                                            COMMIT
		                                                            END TRY
		                                                            BEGIN CATCH
		                                                            ROLLBACK
                                                            END CATCH";

        public static string DocumentsAttachmentsSqlQuery => @"SELECT 
	                                                                [SYS_ANY_DATA] AS [FileName],
	                                                                [ANY_DATA] AS [File]
                                                                FROM 
	                                                                [DD_DOCUMENT_DATA]
                                                                WHERE 
												                    [U_ID] IN ({0}) AND
	                                                                [ANY_DATA] IS NOT NULL AND
												                    DATALENGTH([ANY_DATA]) > 0

                                                                UNION ALL

                                                                SELECT 
                                                                    [SYS_TEMPLATE_DATA] AS [FileName],
                                                                    [TEMPLATE_DATA] AS [File]
                                                                FROM 
	                                                                [DD_DOCUMENT_DATA]
                                                                WHERE 
												                    [U_ID] IN ({0}) AND
												                    [SYS_TEMPLATE_DATA] IS NOT NULL AND 
												                    DATALENGTH([TEMPLATE_DATA]) > 0";

        public static string SetDocumentsMainAttachmentSqlQuery => @"BEGIN TRY
                                                                        BEGIN TRANSACTION 

	                                                                    DECLARE @externalId int = (SELECT d.ExternalId FROM dbo.Documents d WHERE d.Id = {0})

	                                                                    DECLARE @attachmentId int
	                                                                    IF (SELECT MAX(a.id) FROM dbo.Attachments a WHERE a.ExternalId = @externalId	AND a.ValidName LIKE N'%.pdf') IS NOT NULL
		                                                                    set @attachmentId = (SELECT MAX(a.id) FROM dbo.Attachments a WHERE a.ExternalId = @externalId AND a.ValidName LIKE N'%.pdf')
	                                                                    ELSE IF (SELECT MAX(a.id) FROM dbo.Attachments a WHERE a.ExternalId = @externalId AND a.ValidName LIKE N'%.docx') IS NOT NULL
		                                                                    set @attachmentId = (SELECT MAX(a.id) FROM dbo.Attachments a WHERE a.ExternalId = @externalId AND a.ValidName LIKE N'%.docx')
	                                                                    ELSE IF (SELECT MAX(a.id) FROM dbo.Attachments a WHERE a.ExternalId = @externalId AND a.ValidName LIKE N'%.doc') IS NOT NULL
		                                                                    set @attachmentId = (SELECT MAX(a.id) FROM dbo.Attachments a WHERE a.ExternalId = @externalId AND a.ValidName LIKE N'%.doc')
	                                                                    ELSE IF (SELECT MAX(a.id) FROM dbo.Attachments a WHERE a.ExternalId = @externalId AND a.ValidName LIKE N'%.odt') IS NOT NULL
		                                                                    set @attachmentId = (SELECT MAX(a.id) FROM dbo.Attachments a WHERE a.ExternalId = @externalId AND a.ValidName LIKE N'%.odt')
	                                                                    ELSE IF (SELECT MAX(a.id) FROM dbo.Attachments a WHERE a.ExternalId = @externalId)  IS NOT NULL
		                                                                    set @attachmentId = (SELECT MAX(a.id) FROM dbo.Attachments a WHERE a.ExternalId = @externalId)

                                                                        UPDATE d 
		                                                                    SET d.MainAttachmentId = @attachmentId
	                                                                    FROM 
		                                                                    dbo.Documents d
		                                                                    WHERE d.Id = {0}
	                                                                        COMMIT
                                                                        END TRY
                                                                    BEGIN CATCH
                                                                        ROLLBACK
                                                                    END CATCH";

        #endregion
    }
}