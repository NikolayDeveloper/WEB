using System;
using System.Collections.Generic;
using Iserv.Niis.Business.Helpers;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Implementations
{
    public class GetAttachedFileMetadataService : IGetAttachedFileMetadataService
    {
        private readonly Dictionary<string, int[]> _dicRequiredFiles;
        private readonly DictionaryHelper _dictionaryHelper;

        public GetAttachedFileMetadataService(DictionaryHelper dictionaryHelper)
        {
            _dicRequiredFiles = GetRequiredFiles();
            _dictionaryHelper = dictionaryHelper;
        }

        public void Handle(GetAttachedFileMetadataArgument argument,
            GetAttachedFileMetadataResult result)
        {
            var dicProtectionDocTypeCode =
                _dictionaryHelper.GetDictionaryCodeByExternalId(nameof(DicProtectionDocType), argument.MainDocumentType.UID);
            var list = new List<AttachedFileMetadata>();

            var fileTypes = GetFromDbAttachmentFileTypeUid(dicProtectionDocTypeCode);

            foreach (var fileType in fileTypes)
            {
                var required = Required(argument, fileType);
                list.Add(GetMetadata(fileType, required));
            }

            list.Sort((data1, data2) => data1.AttachedFileType.UID - data2.AttachedFileType.UID);
            result.Data = list.ToArray();
        }


        #region privateMethods

        private static int[] GetFromDbAttachmentFileTypeUid(string externalPatentType)
        {
            switch (externalPatentType)
            {
                case DicProtectionDocType.Codes.Invention:
                    return new[] {318, 292, 320, 321, 831, 832, 833, 3411, 1572, 3391, 440, 456, 469, 4132, 3471, 417};
                case DicProtectionDocType.Codes.PlaceOfOrigin:
                    return new[] {3532, 1572, 456, 469, 417};
                case DicProtectionDocType.Codes.UsefulModel:
                    return new[] {443, 442, 441, 292, 1572, 3391, 440, 831, 832, 833, 3411, 456, 469, 4132, 3471, 417};
                case DicProtectionDocType.Codes.IndustrialModel:
                    return new[] {1571, 469, 456, 455, 3391, 440, 831, 832, 833, 3411, 470, 454, 1572};
                case DicProtectionDocType.Codes.SelectiveAchievement:
                    return new[] {463, 454, 1572, 3391, 440, 456, 3472, 469, 3512, 464, 417};
                case DicProtectionDocType.Codes.Trademark:
                    return new[] {417, 412, 1334, 772, 3451, 456, 1572, 3471, 3472, 3473, 469, 1331};
                default:
                    throw new ArgumentException("Справочник не определен: " + externalPatentType);
            }
        }

        private Dictionary<string, int[]> GetRequiredFiles()
        {
            var dicRequiredFiles = new Dictionary<string, int[]>();
            dicRequiredFiles.Add(DicProtectionDocType.Codes.Trademark, new[]
            {
                1331
            });

            dicRequiredFiles.Add(DicProtectionDocType.Codes.Invention, new[]
            {
                318, 292, 320
            });

            dicRequiredFiles.Add(DicProtectionDocType.Codes.SelectiveAchievement, new[]
            {
                463, 464
            });

            dicRequiredFiles.Add(DicProtectionDocType.Codes.IndustrialModel, new[]
            {
                1571, 470, 454
            });

            dicRequiredFiles.Add(DicProtectionDocType.Codes.UsefulModel, new[]
            {
                443, 442, 292
            });

            dicRequiredFiles.Add(DicProtectionDocType.Codes.PlaceOfOrigin, new int[]
            {
            });
            return dicRequiredFiles;
        }

        private bool Required(GetAttachedFileMetadataArgument argument, int fileType)
        {
            var patentType =
                _dictionaryHelper.GetDictionaryCodeByExternalId(nameof(DicProtectionDocType), argument.MainDocumentType.UID);
            var requiredTypes = new List<int>(_dicRequiredFiles[patentType]);

            if (requiredTypes.Contains(fileType))
            {
                return true;
            }

            if (argument.NonResidentApplicantExists && fileType == 456)
            {
                return true;
            }

            if (argument.JurApplicantExists && patentType != DicProtectionDocType.Codes.IndustrialModel &&
                fileType == 417)
            {
                return true;
            }

            if (argument.JurApplicantExists && patentType != DicProtectionDocType.Codes.IndustrialModel &&
                fileType == 772)
            {
                return true;
            }


            if (patentType == DicProtectionDocType.Codes.Trademark && argument.IpApplicantExists && fileType == 412)
            {
                return true;
            }

            if (patentType == DicProtectionDocType.Codes.PlaceOfOrigin && !argument.NonResidentApplicantExists &&
                fileType == 3532)
            {
                return true;
            }

            return false;
        }

        private AttachedFileMetadata GetMetadata(int attachedFileTypeUid, bool required)
        {
            var metadata = new AttachedFileMetadata();
            metadata.AttachedFileType = new RefKey();
            metadata.AttachedFileType.UID = attachedFileTypeUid;
            metadata.Extensions = GetAttachedFileExtensions(attachedFileTypeUid);
            metadata.Required = required;
            metadata.Multiple = Multiple(attachedFileTypeUid);
            return metadata;
        }

        private string[] GetAttachedFileExtensions(int attachedFileTypeUid)
        {
            switch (attachedFileTypeUid)
            {
                case 292:
                case 318:
                case 320:
                case 442:
                case 443:
                case 831:
                case 832:
                case 833:
                case 1571:
                case 3451:
                    return new[] {".doc", ".docx", ".rtf", ".odt", ".pdf"};
                case 321:
                case 454:
                case 1331:
                    return new[] {".png"};
                default:
                    return new[] {".doc", ".docx", ".rtf", ".odt", ".pdf"};
            }
        }

        private static bool Multiple(int fileType)
        {
            switch (fileType)
            {
                case 456:
                case 417:
                case 772:
                case 412:
                case 1334:
                case 469:
                    return true;

                default:
                    return false;
            }
        }

        #endregion
    }
}