using System;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Dictionaries.DicMain
{
    public class DicCustomerRole : DictionaryEntity<int>, IReference, IHaveConcurrencyToken
    {
        public static class Codes
        {
            public enum CodesEnum
            {
                None,
                Declarant,
                Author,
                PatentOwner,
                PatentAttorney,
                Contact98,
                Confidant,
                Storona1,
                Correspondence,
                Storona2,
                Addressee
            }

            public static string GetCode(CodesEnum code)
            {
                switch (code)
                {
                    case CodesEnum.Declarant:
                        return Declarant;
                    case CodesEnum.Addressee:
                        return Addressee;
                    case CodesEnum.Author:
                        return Author;
                    case CodesEnum.PatentOwner:
                        return PatentOwner;
                    case CodesEnum.PatentAttorney:
                        return PatentAttorney;
                    case CodesEnum.Storona1:
                        return Storona1;
                    case CodesEnum.Contact98:
                        return Contact98;
                    case CodesEnum.Correspondence:
                        return Correspondence;
                    case CodesEnum.Confidant:
                        return Confidant;
                    case CodesEnum.Storona2:
                        return Storona2;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(code), code, null);
                }
            }

            #region Public codes

            public const string Declarant = "1";
            public const string Author = "2";
            public const string PatentOwner = "3";
            public const string PatentAttorney = "4";
            public const string Contact98 = "5";
            public const string Confidant = "6";
            public const string Storona1 = "7";
            public const string Correspondence = "CORRESPONDENCE";
            public const string Storona2 = "8";
            public const string Addressee = "Addressee";

            #endregion
        }
    }
}