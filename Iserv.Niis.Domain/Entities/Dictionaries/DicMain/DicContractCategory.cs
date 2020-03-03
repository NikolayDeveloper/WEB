using System;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Dictionaries.DicMain
{
    /// <summary>
    /// 
    /// </summary> 
    public class DicContractCategory : DictionaryEntity<int>, IReference, IHaveConcurrencyToken
    {
        public static class Codes
        {
            public enum CodesEnum
            {
                None,
                NationalPartners,
                ReceivingPartyIsForeigner,
                ForeignPartners,
                ReceivingPartyIsNational

            }

            public static string GetCode(DicContractCategory.Codes.CodesEnum code)
            {
                switch (code)
                {
                    case DicContractCategory.Codes.CodesEnum.NationalPartners:
                        return NationalPartners;
                    case DicContractCategory.Codes.CodesEnum.ReceivingPartyIsForeigner:
                        return ReceivingPartyIsForeigner;
                    case DicContractCategory.Codes.CodesEnum.ForeignPartners:
                        return ForeignPartners;
                    case DicContractCategory.Codes.CodesEnum.ReceivingPartyIsNational:
                        return ReceivingPartyIsNational;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(code), code, null);
                }
            }

            #region Public codes
            /// <summary>
            /// Договор, заключенный между национальными партнерами
            /// </summary>
            public const string NationalPartners = "21";
            /// <summary>
            /// Договор, в котором передающая сторона является национальным лицом, а принимающая -  иностранным лицом
            /// </summary>
            public const string ReceivingPartyIsForeigner = "22";
            /// <summary>
            /// Договор,заключенный между иностранными партнерами
            /// </summary>
            public const string ForeignPartners = "23";
            /// <summary>
            /// Договор, в котором передающая сторона является иностранным лицом, а принимающая - национальным лицом
            /// </summary>
            public const string ReceivingPartyIsNational = "24";

            #endregion
        }
    }
}