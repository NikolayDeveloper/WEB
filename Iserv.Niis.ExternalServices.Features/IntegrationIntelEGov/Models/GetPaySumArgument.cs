using System;
using System.Xml.Serialization;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public class GetPaySumArgument : SystemInfoMessage
    {
        public RefKey MainDocumentType { get; set; }

        public ServiceType ServiceType { get; set; }
        public RefKey DocumentType { get; set; }
        public int? Count { get; set; }

        //[Documentation]
        public ResidencyPayer ResidencyPayer { get; set; }

        public bool IsJur { get; set; }
        public bool IsFiz { get; set; }
        public bool IsFizBenefit { get; set; }
        public bool ExpiredPayment { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public enum ServiceType
    {
        MainService,
        AdditionalServiceS
    }

    [Serializable]
    [XmlType(Namespace = "http://egov.niis.kz")]
    public enum ResidencyPayer
    {
        ResidentRK, //Резидент РК
        NonresidentRK, //Не резидент РК
        ResidentOfCountryLess5000 //Резидент страны с доходом менее $5000 на душу населения //http://russian.doingbusiness.org/data/exploreeconomies/economycharacteristics/?SortColumn=name&SortDirection=asc&ajax=1
    }

    //[Serializable]
    //[System.Xml.Serialization.XmlType(Namespace = "http://egov.niis.kz")]
    //public enum PayerType { 
    //    IsJur,
    //    IsFiz,
    //    IsFizBenefit
    //}
}