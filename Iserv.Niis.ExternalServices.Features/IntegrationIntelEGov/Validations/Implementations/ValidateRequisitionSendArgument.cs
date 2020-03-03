using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iserv.Niis.Business.Helpers;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Utils;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Validations.Abstractions;
using Serilog;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Validations.Implementations
{
    public class ValidateRequisitionSendArgument : IValidateRequisitionSendArgument
    {
        private readonly IGetAttachedFileMetadataService _attachedFileMetadata;
        private readonly IntegrationAttachFileHelper _attachFileHelper;
        private readonly DictionaryHelper _dictionaryHelper;
        private readonly IntegrationValidationHelper _validationHelper;

        public ValidateRequisitionSendArgument(
            IGetAttachedFileMetadataService attachedFile,
            IntegrationValidationHelper validationHelper,
            IntegrationAttachFileHelper attachFileHelper,
            DictionaryHelper dictionaryHelper)
        {
            _attachedFileMetadata = attachedFile;
            _validationHelper = validationHelper;
            _attachFileHelper = attachFileHelper;
            _dictionaryHelper = dictionaryHelper;
        }

        public string GetValidationErrors(RequisitionSendArgument argument)
        {
            if (argument.PatentType == null)
            {
                return "Не указан ExternalPatentType!";
            }

            var errors = new List<string>();

            if (string.IsNullOrEmpty(argument.AdrCustomerName))
            {
                errors.Add("AdrCustomerName не указан!");
            }

            if (argument.AdrCountry == null)
            {
                errors.Add("AdrCountry не указан!");
            }

            if (argument.AdrStreet == null)
            {
                errors.Add("AdrStreet не указан!");
            }

            if (argument.AdrEmail == null)
            {
                errors.Add("AdrEmail не указан!");
            }

            var applicants = 0;
            var attorney = 0;
            var authors = 0;
            var errRefKey = false;
            var errCusType = false;

            foreach (var customer in argument.BlockCustomer)
            {
                if (customer.PatentLinkType == null)
                {
                    errRefKey = true;
                }
                else
                {
                    switch (customer.PatentLinkType.UID)
                    {
                        case 1:
                            applicants++;
                            break;
                        case 479:
                        case 4:
                            attorney++;
                            break;
                        case 2:
                            authors++;
                            break;
                    }
                }

                if (customer.Type == null)
                {
                    errCusType = true;
                }
            }

            if (errRefKey)
            {
                errors.Add("BlockCustomer: Customer.PatentLinkType не указан!");
            }

            if (errCusType)
            {
                errors.Add("BlockCustomer: Customer.Type не указан!");
            }

            if (applicants <= 0)
            {
                errors.Add("Нужно добавить как минимум одного заявителя!");
            }
            else
            {
                var cusType = GetErrApplicatTypeForPatentType(argument.PatentType.UID, argument.BlockCustomer);
                if (!string.IsNullOrEmpty(cusType))
                {
                    errors.Add("Для заявки ExternalPatentType.UID = " + argument.PatentType.UID +
                               ", заявитель не может быть BlockCustomer: Customer.Type.UID = " + cusType);
                }
            }


            var dicProtectionDocTypeCode =
                _dictionaryHelper.GetDictionaryCodeByExternalId(nameof(DicProtectionDocType), argument.PatentType.UID);

            if (dicProtectionDocTypeCode == DicProtectionDocType.Codes.Trademark)
            {
                if (argument.BlockClassification.Length <= 0)
                {
                    errors.Add("Нужно указать как минимум один МКТУ!");
                }
            }

            if (authors <= 0 && (
                    dicProtectionDocTypeCode == DicProtectionDocType.Codes.Invention
                    || dicProtectionDocTypeCode == DicProtectionDocType.Codes.IndustrialModel
                    || dicProtectionDocTypeCode == DicProtectionDocType.Codes.UsefulModel
                    || dicProtectionDocTypeCode == DicProtectionDocType.Codes.SelectiveAchievement
                ))
            {
                errors.Add("Нужно добавить как минимум одного автора!");
            }

            var patentLinkType = false;
            var customerType = false;
            var errCustomerName = false;

            foreach (var customer in argument.BlockCustomer)
            {
                if (customer.PatentLinkType == null)
                {
                    patentLinkType = true;
                }

                if (customer.Type == null)
                {
                    customerType = true;
                }

                if (string.IsNullOrEmpty(customer.NameRu))
                {
                    errCustomerName = true;
                }
            }

            if (errCustomerName)
            {
                errors.Add("Не указан ФИО/Полное наименование на русском языке BlockCustomer: Customer.NameRu");
            }

            if (patentLinkType)
            {
                errors.Add("Не указан тип связки контрагента BlockCustomer: Customer.PatentLinkType");
            }

            if (customerType)
            {
                errors.Add("Не указан тип контрагента BlockCustomer: Customer.Type");
            }

            if (!patentLinkType && !customerType)
            {
                errors.AddRange(GetArgumentErrorRequisitionSendAttachedFiles(argument, argument));
            }

            if (!_validationHelper.SenderIsPep(argument.SystemInfo.Sender) &&
                _validationHelper.FileIsEmpty(argument.RequisitionFile, argument))
            {
                errors.Add("RequisitionFile. Заявление не может быть пустым");
            }

            if (_validationHelper.SenderIsPep(argument.SystemInfo.Sender) && argument.RequisitionFile != null)
            {
                errors.Add("ПЭП должен отправлять RequisitionFile = null");
            }

            if (_validationHelper.SenderIsPep(argument.SystemInfo.Sender) && argument.RequisitionFile == null)
            {
                argument.RequisitionFile = new File {Name = argument.SystemInfo.ChainId + ".pdf"};

                try
                {
                    argument.RequisitionFile.Content =
                        _attachFileHelper.DownloadPepRequisitionFile(argument.SystemInfo.ChainId, argument.Xin);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex.Message);
                    errors.Add("Не удалось получить pdf файл заявки. Заявка " + argument.SystemInfo.ChainId);
                    errors.Add(ex.Message);
                }
            }

            if (argument.RequisitionFile != null && argument.RequisitionFile.Content != null)
            {
                if (argument.RequisitionFile.Content.Length < 10)
                {
                    errors.Add("Файл заявки пуст. Заявка " + argument.SystemInfo.ChainId);
                }
                else
                {
                    const string textPdf = "%PDF";
                    var textFile = Encoding.UTF8.GetString(argument.RequisitionFile.Content, 0, 4);

                    if (textFile != textPdf)
                    {
                        errors.Add("Файл заявки не является PDF-документом. Заявка " + argument.SystemInfo.ChainId);
                    }
                }
            }

            if (errors.Count == 0)
            {
                return null;
            }

            return string.Join(Environment.NewLine, errors.ToArray());
        }

        #region PrivateMethods

        private string GetErrApplicatTypeForPatentType(int patentType, Customer[] blockCustomer)
        {
            foreach (var customer in blockCustomer)
            {
                switch (customer.PatentLinkType.UID)
                {
                    case 1:
                        if (patentType == 5)
                        {
                            if (customer.Type.UID != 19 && customer.Type.UID != 775 && customer.Type.UID != 777)
                            {
                                return customer.Type.UID + string.Empty;
                            }
                        }

                        if (patentType == 4)
                        {
                            if (customer.Type.UID != 20 && customer.Type.UID != 19 && customer.Type.UID != 775 &&
                                customer.Type.UID != 777)
                            {
                                return customer.Type.UID + string.Empty;
                            }
                        }

                        break;
                }
            }

            return null;
        }

        private string[] GetArgumentErrorRequisitionSendAttachedFiles(RequisitionSendArgument argument,
            SystemInfoMessage infoMessage)
        {
            var errors = new List<string>();

            var arg = new GetAttachedFileMetadataArgument();
            var result = new GetAttachedFileMetadataResult();

            arg.MainDocumentType = argument.PatentType;
            arg.JurApplicantExists = ExistsInApplicants(DicCustomerTypeCodes.Juridical, argument);
            arg.IpApplicantExists = ExistsInApplicants(DicCustomerTypeCodes.SoloEntrepreneur, argument);
            //todo Поменялся механизм по которому определяется яввляется ли контрагент резидентом или нет. Не понятно как разруливать с интеграцией
            arg.NonResidentApplicantExists = ExistsInApplicants(DicCustomerTypeCodes.Nonresident, argument);
            _attachedFileMetadata.Handle(arg, result);
            var addedFileTypes = new List<int>();

            if (argument.BlockFile != null)
            {
                foreach (var attachedFile in argument.BlockFile)
                {
                    if (attachedFile == null)
                    {
                        errors.Add("BlockFile AttachedFile == null");
                        continue;
                    }

                    if (attachedFile.Type == null || attachedFile.Type.UID == 0)
                    {
                        errors.Add("Не указан тип документа BlockFile: AttachedFile.Type");
                        continue;
                    }

                    //var metaData = GetMetaData(attachedFile.Type, result.Data);

                    //if (metaData == null)
                    //{
                    //    errors.Add("BlockFile AttachedFile.Type.UID = " + attachedFile.Type.UID +
                    //               ". Данный документ не относится к этому типу патента.");
                    //    continue;
                    //}

                    if (_validationHelper.FileIsEmpty(attachedFile.File, infoMessage))
                    {
                        errors.Add("Не добавлен файл для документа BlockFile: AttachedFile.Type.UID = " +
                                   attachedFile.Type.UID);
                        continue;
                    }

                    if (!_validationHelper.FileNameIsCorrect(attachedFile.File, infoMessage))
                    {
                        errors.Add(
                            "Не указано имя файла или некорректное имя файла BlockFile: AttachedFile.File, Type.UID = " +
                            attachedFile.Type.UID);
                        continue;
                    }

                    //if (!_validationHelper.CheckFileExtension(metaData.Extensions, attachedFile.File, infoMessage))
                    //{
                    //    errors.Add(
                    //        "Неправильный формат файла для данного типа документа BlockFile: AttachedFile.Type.UID = "
                    //        + attachedFile.Type.UID + ", файл должен быть в формате\"" +
                    //        string.Join(", ", metaData.Extensions) + "\"");
                    //    continue;
                    //}

                    addedFileTypes.Add(attachedFile.Type.UID);
                }
            }

            //foreach (var metadata in result.Data)
            //{
            //    if (metadata.Required && !addedFileTypes.Contains(metadata.AttachedFileType.UID))
            //    {
            //        errors.Add("Не добавлен обязательный документ BlockFile: AttachedFile.Type.UID = " +
            //                   metadata.AttachedFileType.UID);
            //    }
            //}

            return errors.ToArray();
        }

        private bool ExistsInApplicants(string dicCustomerTypeCode, RequisitionSendArgument argument)
        {
            var dicCustomerTypeId =
                _dictionaryHelper.GetDictionaryIdByCode(nameof(DicCustomerType), dicCustomerTypeCode);
            foreach (var customer in argument.BlockCustomer)
            {
                if (customer.PatentLinkType.UID == 1 && customer.Type.UID == dicCustomerTypeId)
                {
                    return true;
                }
            }

            return false;
        }

        public static AttachedFileMetadata GetMetaData(RefKey fileType, AttachedFileMetadata[] data)
        {
            foreach (var metadata in data)
            {
                if (metadata.AttachedFileType.UID == fileType.UID)
                {
                    return metadata;
                }
            }

            return null;
        }

        #endregion
    }
}