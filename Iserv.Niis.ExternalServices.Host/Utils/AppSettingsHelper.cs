using Iserv.Niis.Domain.Entities.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Iserv.Niis.ExternalServices.Host.Utils
{
    public static class AppSettingsHelper
    {
        private static Dictionary<string, string> _codes;

        static AppSettingsHelper()
        {
            _codes = new Dictionary<string, string>
            {
                { nameof(DicProtectionDocType.Codes.Trademark), DicProtectionDocType.Codes.Trademark },
                { nameof(DicProtectionDocType.Codes.PlaceOfOrigin), DicProtectionDocType.Codes.PlaceOfOrigin },
                { nameof(DicProtectionDocType.Codes.Invention), DicProtectionDocType.Codes.Invention },
                { nameof(DicProtectionDocType.Codes.UsefulModel), DicProtectionDocType.Codes.UsefulModel },
                { nameof(DicProtectionDocType.Codes.SelectiveAchievement), DicProtectionDocType.Codes.SelectiveAchievement },
                { nameof(DicProtectionDocType.Codes.IndustrialModel), DicProtectionDocType.Codes.IndustrialModel }
            };
        }

        public static Dictionary<string, int> GetMainExecutorIds(string setting)
        {
            var executors = setting.Split(';').ToDictionary(k => _codes[k.Split('=').First()], v => Convert.ToInt32(v.Split('=').Last()));

            return executors;
        }
    }
}