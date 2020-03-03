using System;
using System.IO;
using Iserv.Niis.ExternalServices.Features.Constants;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;
using File = Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models.File;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Utils
{
    public class IntegrationValidationHelper
    {
        public bool SenderIsPep(string sender)
        {
            return sender.Equals(CommonConstants.SenderPep,
                StringComparison.InvariantCultureIgnoreCase);
        }

        public bool FileIsEmpty(File file, SystemInfoMessage infoMessage)
        {
            if (file == null)
                return true;

            if (SenderIsPep(infoMessage.SystemInfo.Sender))
            {
                if (string.IsNullOrEmpty(file.ShepFile?.ID))
                    return true;
            }
            else
            {
                if (file.Content == null || file.Content.Length == 0)
                    return true;
            }

            return false;
        }

        public bool FileNameIsCorrect(File file, SystemInfoMessage infoMessage)
        {
            if (file == null)
                return false;

            if (SenderIsPep(infoMessage.SystemInfo.Sender))
            {
                if (file.ShepFile == null || !CheckFileNameCorrect(file.ShepFile.Name))
                    return false;
            }
            else
            {
                if (file.Content == null || !CheckFileNameCorrect(file.Name))
                    return false;
            }

            return true;
        }

        public bool CheckFileExtension(string[] extensions, File file, SystemInfoMessage infoMessage)
        {
            string fileExtension = null;

            if (SenderIsPep(infoMessage.SystemInfo.Sender))
                fileExtension = Path.GetExtension(file.ShepFile.Name);
            else
                fileExtension = Path.GetExtension(file.Name);

            if (fileExtension == null)
                return false;

            foreach (var extension in extensions)
                if (fileExtension.Equals(extension, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            return false;
        }

        #region PrivateMethods

        private bool CheckFileNameCorrect(string name)
        {
            return !string.IsNullOrEmpty(name) && name.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
        }

        #endregion
    }
}