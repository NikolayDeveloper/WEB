namespace Iserv.Niis.Documents.Enums
{
    public enum TemplateFieldName
    {
        DepartmentHeadName,
        DeputyDepartmentHeadName,
        DirectorName,
        DeputyDirectorName, // Имя заместителя директора
        CurrentUser,
        CurrentUserKz,
        RequestDate,
        RequestDateInWords,
        RequestNumber,
        Image,
        ExpirationDateTZ, //ToDo: в процессе
        Declarants,
        DeclarantsAndAddress,
        DeclarantsAddress,
        Mktu511, // (511) Индексы МКТУ
        QrCode,
        CorrespondenceContact,
        CorrespondenceAddress,
        Priority31WithoutCode,
        Priority32WithoutCode,
        Priority33WithoutCode,
        CustomerName,
        CustomerAddress,
        CustomerEmail,
        CustomerPhone,
        SignedUsers,
        DocumentDateCreate,
        CommitteeSolutionDate,
        CommitteeSolutionNumber,
        CurrentDate,
        DueDate, //todo: (реализация временно отложена)
        DocumentNumber,
        NSurnameLeader, //todo: (реализация временно отложена)
        Disclaimer,
        DisclaimerReason,
        Icfem,
        Icgs511,
        DicDetailICGS,
        Priority31,
        Priority32,
        Priority33,
        CurrentYear,
        ApplicantAddress,
        ApplicantAddressKz,
        FailureInfoTZ,
        PatentOwner,
        PatentOwnerKz,
        PatentOwnerCountryCodesKz,
        PatentOwnerCountryCodesRu,
        ValidDate,
        Colors,
        ColorsKz,
        ColorsEn,
        DocumentNum,
        DateTimeNow,
        GosNumber,
        GosDate,
        PatentNameRu,
        BulletinDate,
        EarlyTerminationDate,
        Authors,
        PatentAuthors,
        PatentAuthorsKz,
        AuthorsCountryCodes,
        AuthorsCountryCodesKz,
        TransferDate,
        Priority86WithoutCode,
        Priority85WithoutCode,
        PatentAttorney,
        RequestDateCreate,
        Priority86,
        IpcCodes,
        BulletinNumber,
        PatentNameKz,
        TypeOfGoods,
        PlaceOfOrigin,
        NumberApxWork,
        RecoveryTime, //todo: (реализация временно отложена)
        TransferDateWithCode,
        PatentGosNumber,
        PatentOwnerAddress,
        Mkpo51,
        Position,
        Referat,
        DeclarantsKz,
        AddressForCorrespondence,
        ExpirationDateOD,
        NotificationNumber,
        NotificationDate,
        CurrentUserPhoneNumber,
        CurrentUserMobilePhoneNumber,
        InvoiceNumber,
        ModificationDate,
        SpecialPropertiesOfGood,
        DeclarantsNew,
        ApplicantPhone,
        ApplicantPhoneFax,
        ApplicantEmail,
        CorrespondencePhone,
        CorrespondenceMobilePhone,
        CorrespondencePhoneFax,
        CorrespondenceEmail,
        InformationOnStateRegistration,
        PriorityDate,
        RequestNameRu,
        DateInWords,
        PaymentTermDate,
        DateEarlyTermination,
        President,
        PresidentKz,
        PaymentRestoreTermDate,
        CurrentUserEmafil,
        CurrentUserFax,
        AuthorAddress,
        ProtectionDocNumber,
        RequestNameKz,
        ApplicationRestorationDate,
        DateOfNotificationOfThePositiveResultOfTheFormalExamination,
        AccountRenewalYear,
        RegistrationOfTrademarkRenewalDateInWords,
        RegistrationOfTrademarkRenewalDateInWordsKz,
        RegistrationOfTrademarkRenewalYearInWordsKz,
        GosReestrChangeDateInWordsRu,
        GosReestrChangeYearInWordsKz,
        GosReestrChangeDateInWordsKz,
        ContractRegistrationDateInWordsRu,
        ContractRegistrationYearInWordsKz,
        ContractRegistrationDateInWordsKz,
        TrademarkUseRighsRu,
        TrademarkUseRighsKz,
        BulletinYear,
        PatentRestorationYearInWordKz,
        PatentRestorationDateInWordKz,
        PatentRestorationDateInWord,
        PatentRenewalDateInWord,
        PatentRenewalDateInWordKz,
        PatentRenewalYearInWordKz,
        DescriptionReferat,
        SecondPartyRightOwnerRu,
        SecondPartyRightOwnerKz,
        FirstPartyLegalNameOrFioKz,
        SecondPartyLegalNameOrFioKz,
        FirstPartyLegalNameOrFioRu,
        SecondPartyLegalNameOrFioRu,
        FirstPartyCountryCode,
        SecondPartyCountryCode,
        OpsNameKz,
        OpsTypeKz,
        AuthorNumber,

        RequestApplicantInfoRecords_Complex,
        RequestDocumentInfoRecords_Complex,
        RowIncrement,
        ApplicationQ,
        ApplicationTable,
        DeclarantShortInfo,

        RequestCount,

        MaterialNum,
        MaterialDateCreate,
        ContractNum,
        ContractType,       
        ContractTypeKz,       
        ContractGosNumber,
        ContractIncomingDate,
        ContractIncomingNumber,
        ContractAddressee,
        ContractProtectionDocType,
        ContractProtectionDocTypeKz,

        ContractStorona1NameRu,
        ContractStorona1NameKz,
        ContractStorona2NameRu,
        ContractStorona2NameKz,
        ContractStorona1Address,
        ContractStorona1AddressKz,
        ContractStorona2Address,
        ContractStorona2AddressKz,
        ContractCopyCount,
        RequestInfosOfDocument_Complex,
        RequestCountInDocumentByRequestTypeInventionAndUsefulModel,
        RequestInfosInventionOfDocument_Complex,
        RequestInfosUsefulModelOfDocument_Complex,
        InformationAboutAttachedToContract,
        DocumentUserSignature,
        // ReSharper disable once InconsistentNaming
        _RichUserInput,

        // ReSharper disable once InconsistentNaming
        _UserInput,
        IcgsIndices,
        RejectionReason,
        HeadName,
        HeadHeadName,
        ExpertName, // Имя Эксперта
        ExpertPhone,
        ExecutorName,
        ExecutorPhone,
        ExpertiseRequestData,
        Priority300,
        DivisionHeadPost,
        DivisionHeadPostKz,
        Attorneys, 

        // Terletskiy 
        RequestNumberPriority310,
        CurrentUserEmail,
        ChangedListOfDeclaredGoods,
        ApplicantAddressNew,
        /// <summary>
        ///  Заявители на английском языке.
        /// </summary>
        DeclarantsEn,

        /// <summary>
        /// Авторы патента на английском языке.
        /// </summary>
        PatentAuthorsEn,

        /// <summary>
        /// Патентообладатели на английском языке.
        /// </summary>
        PatentOwnerEn,

        /// <summary>
        /// Название заявки на английском языке.
        /// </summary>
        RequestNameEn,

        /// <summary>
        /// Наименование услуги.
        /// </summary>
        ServiceName,

        /// <summary>
        /// Код услуги.
        /// </summary>
        ServiceCode,

        /// <summary>
        /// Номер заявки, ОД или договора.
        /// </summary>
        OwnerNumber,
        
        /// <summary>
        /// Номер платежа.
        /// </summary>
        PaymentNumber,

        /// <summary>
        /// ID платежа, из которого была зачтена сумма по услуге.
        /// </summary>
        PaymentId,

        /// <summary>
        /// Дата создания записи.
        /// </summary>
        PaymentUseDateCreate,

        /// <summary>
        /// Дата зачтения платежа.
        /// </summary>
        PaymentUseDate,

        /// <summary>
        /// Плательщик
        /// </summary>
        Payer,

        /// <summary>
        /// ИИН\БИН плательщика.
        /// </summary>
        PayerXin,

        /// <summary>
        /// Дата платежа.
        /// </summary>
        PaymentDate,

        /// <summary>
        /// Номер документа 1C.
        /// </summary>
        Payment1CNumber,

        /// <summary>
        /// Сумма зачтения.
        /// </summary>
        ChargeAmount,

        /// <summary>
        /// Сумма платежа.
        /// </summary>
        PaymentAmount,

        /// <summary>
        /// Распределено.
        /// </summary>
        PaymentDisturbed,
        
        /// <summary>
        /// Остаток платежа.
        /// </summary>
        PaymentRemainder,

        /// <summary>
        /// Авансовый платеж.
        /// </summary>
        IsAdvancePayment,

        /// <summary>
        /// Назначение платежа.
        /// </summary>
        PaymentPurpose,

        /// <summary>
        /// Сотрудник выполнивший зачтение.
        /// </summary>
        ChargerUser,

        /// <summary>
        /// Примечание из материала
        /// </summary>
        DocumentDescription
    }
}