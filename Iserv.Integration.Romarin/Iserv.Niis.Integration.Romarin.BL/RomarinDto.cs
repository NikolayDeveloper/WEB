using System.Collections.Generic;
using System.Xml.Serialization;
using Iserv.Niis.Integration.Romarin.BL.Interfaces;

namespace Iserv.Niis.Integration.Romarin.BL
{
    [XmlRoot(ElementName = "NAME")]
    public class NAME
    {
        [XmlElement(ElementName = "NAMEL")]
        public List<string> NAMEL { get; set; }
    }

    [XmlRoot(ElementName = "ADDRESS")]
    public class ADDRESS
    {
        [XmlElement(ElementName = "ADDRL")]
        public List<string> ADDRL { get; set; }

        [XmlElement(ElementName = "COUNTRY")]
        public string COUNTRY { get; set; }
    }

    /// <summary>
    /// Патентообладатель
    /// </summary>
    [XmlRoot(ElementName = "HOLGR")]
    public class HOLGR
    {
        [XmlElement(ElementName = "NAME")]
        public NAME NAME { get; set; }

        [XmlElement(ElementName = "ADDRESS")]
        public ADDRESS ADDRESS { get; set; }

        [XmlElement(ElementName = "ENTNATL")]
        public string ENTNATL { get; set; }

        [XmlElement(ElementName = "NATLTY")]
        public string NATLTY { get; set; }

        [XmlElement(ElementName = "CORRIND")]
        public string CORRIND { get; set; }

        [XmlAttribute(AttributeName = "CLID")]
        public string CLID { get; set; }

        [XmlAttribute(AttributeName = "NOTLANG")]
        public string NOTLANG { get; set; }
    }

    [XmlRoot(ElementName = "REPGR")]
    public class REPGR
    {
        [XmlElement(ElementName = "NAME")]
        public NAME NAME { get; set; }

        [XmlElement(ElementName = "ADDRESS")]
        public ADDRESS ADDRESS { get; set; }

        [XmlAttribute(AttributeName = "CLID")]
        public string CLID { get; set; }
    }

    [XmlRoot(ElementName = "PHOLGR")]
    public class PHOLGR
    {
        [XmlElement(ElementName = "NAME")]
        public NAME NAME { get; set; }

        [XmlElement(ElementName = "ADDRESS")]
        public ADDRESS ADDRESS { get; set; }

        [XmlAttribute(AttributeName = "CLID")]
        public string CLID { get; set; }
    }

    /// <summary>
    /// Изображение
    /// </summary>
    [XmlRoot(ElementName = "IMAGE")]
    public class IMAGE
    {
        [XmlAttribute(AttributeName = "COLOUR")]
        public string COLOUR { get; set; }

        [XmlAttribute(AttributeName = "TYPE")]
        public string TYPE { get; set; }

        /// <summary>
        /// (54) Название на иностранном языке
        /// </summary>
        [XmlAttribute(AttributeName = "NAME")]
        public string NAME { get; set; }

        [XmlAttribute(AttributeName = "TEXT")]
        public string TEXT { get; set; }
    }
    
    /// <summary>
    /// (526) Дискламация
    /// </summary>
    [XmlRoot(ElementName = "DISCLAIMGR")]
    public class DISCLAIMGR
    {
        [XmlElement(ElementName = "DISCLAIMEREN")]
        public string DISCLAIMEREN { get; set; }
        [XmlElement(ElementName = "DISCLAIMERFR")]
        public string DISCLAIMERFR { get; set; }
        [XmlElement(ElementName = "DISCLAIMERES")]
        public string DISCLAIMERES { get; set; }
    }

    [XmlRoot(ElementName = "VIENNAGR")]
    public class VIENNAGR
    {
        [XmlElement(ElementName = "VIECLAI")]
        public List<string> VIECLAI { get; set; }

        [XmlElement(ElementName = "VIECLA3")]
        public List<string> VIECLA3 { get; set; }

        [XmlAttribute(AttributeName = "VIENVER")]
        public string VIENVER { get; set; }
    }

    [XmlRoot(ElementName = "COLCLAGR")]
    public class COLCLAGR
    {
        [XmlElement(ElementName = "COLCLAEN")]
        public List<string> COLCLAEN { get; set; }
        [XmlElement(ElementName = "COLCLAFR")]
        public List<string> COLCLAFR { get; set; }
        [XmlElement(ElementName = "COLCLAES")]
        public List<string> COLCLAES { get; set; }
    }

    [XmlRoot(ElementName = "PREREGG")]
    public class PREREGG
    {
        [XmlElement(ElementName = "PREREGD")]
        public string PREREGD { get; set; }

        [XmlElement(ElementName = "PREREGN")]
        public string PREREGN { get; set; }
    }

    [XmlRoot(ElementName = "GSGR")]
    public class GSGR
    {
        [XmlElement(ElementName = "GSTERMEN")]
        public string GSTERMEN { get; set; }

        [XmlElement(ElementName = "GSTERMFR")]
        public string GSTERMFR { get; set; }

        [XmlElement(ElementName = "GSTERMES")]
        public string GSTERMES { get; set; }

        [XmlAttribute(AttributeName = "NICCLAI")]
        public string NICCLAI { get; set; }
    }

    /// <summary>
    /// МКТУ
    /// </summary>
    [XmlRoot(ElementName = "BASICGS")]
    public class BASICGS
    {
        [XmlElement(ElementName = "GSGR")]
        public List<GSGR> GSGRs { get; set; }

        [XmlAttribute(AttributeName = "NICEVER")]
        public string NICEVER { get; set; }
    }

    [XmlRoot(ElementName = "BASREGGR")]
    public class BASREGGR
    {
        [XmlElement(ElementName = "BASREGD")]
        public string BASREGD { get; set; }

        [XmlElement(ElementName = "BASREGN")]
        public string BASREGN { get; set; }
    }

    [XmlRoot(ElementName = "BASGR")]
    public class BASGR
    {
        [XmlElement(ElementName = "BASREGGR")]
        public BASREGGR BASREGGR { get; set; }
    }

    /// <summary>
    /// Приоритетные данные
    /// </summary>
    [XmlRoot(ElementName = "PRIGR")]
    public class PRIGR
    {
        [XmlElement(ElementName = "PRICP")]
        public string PRICP { get; set; }
        [XmlElement(ElementName = "PRIAPPD")]
        public string PRIAPPD { get; set; }
        [XmlElement(ElementName = "PRIAPPN")]
        public string PRIAPPN { get; set; }
        [XmlElement(ElementName = "TEXTEN")]
        public string TEXTEN { get; set; }
        [XmlElement(ElementName = "TEXTFR")]
        public string TEXTFR { get; set; }
        [XmlElement(ElementName = "TEXTES")]
        public string TEXTES { get; set; }
    }

    [XmlRoot(ElementName = "DESPG")]
    public class DESPG
    {
        [XmlElement(ElementName = "DCPCD")]
        public List<string> DCPCD { get; set; }
    }

    [XmlRoot(ElementName = "DESPG2")]
    public class DESPG2
    {
        [XmlElement(ElementName = "DCPCD")]
        public List<string> DCPCD { get; set; }
    }

    [XmlRoot(ElementName = "INTENTG")]
    public class INTENTG
    {
        [XmlElement(ElementName = "CPCD")]
        public List<string> CPCD { get; set; }
    }

    [XmlRoot(ElementName = "LIMGR")]
    public class LIMGR
    {
        [XmlElement(ElementName = "DCPCD")]
        public List<string> DCPCD { get; set; }
        [XmlElement(ElementName = "LIMTO")]
        public List<LIMTO> LIMTO { get; set; }
    }

    [XmlRoot(ElementName = "CURRENT")]
    public class CURRENT
    {
        [XmlElement(ElementName = "HOLGR")]
        public HOLGR HOLGR { get; set; }

        [XmlElement(ElementName = "REPGR")]
        public REPGR REPGR { get; set; }

        [XmlElement(ElementName = "PHOLGR")]
        public PHOLGR PHOLGR { get; set; }

        [XmlElement(ElementName = "IMAGE")]
        public IMAGE IMAGE { get; set; }

        /// <summary>
        /// Транслитирация
        /// </summary>
        [XmlElement(ElementName = "MARTRAN")]
        public string MARTRAN { get; set; }

        /// <summary>
        /// МКИЭТЗ
        /// </summary>
        [XmlElement(ElementName = "MARDUR")]
        public string MARDUR { get; set; }

        /// <summary>
        /// (526) Дискламация
        /// </summary>
        [XmlElement(ElementName = "DISCLAIMGR")]
        public DISCLAIMGR DISCLAIMGR { get; set; }

        /// <summary>
        /// МКИЭТЗ
        /// </summary>
        [XmlElement(ElementName = "VIENNAGR")]
        public VIENNAGR VIENNAGR { get; set; }

        /// <summary>
        /// (591) Цвета
        /// </summary>
        [XmlElement(ElementName = "COLCLAGR")]
        public COLCLAGR COLCLAGR { get; set; }

        [XmlElement(ElementName = "PREREGG")]
        public PREREGG PREREGG { get; set; }

        /// <summary>
        /// МКТУ
        /// </summary>
        [XmlElement(ElementName = "BASICGS")]
        public BASICGS BASICGS { get; set; }

        [XmlElement(ElementName = "BASGR")]
        public BASGR BASGR { get; set; }

        /// <summary>
        /// Приоритетные данные
        /// </summary>
        [XmlElement(ElementName = "PRIGR")]
        public PRIGR PRIGR { get; set; }

        [XmlElement(ElementName = "DESPG")]
        public DESPG DESPG { get; set; }

        [XmlElement(ElementName = "DESPG2")]
        public DESPG2 DESPG2 { get; set; }

        [XmlElement(ElementName = "INTENTG")]
        public INTENTG INTENTG { get; set; }
    }

    [XmlRoot(ElementName = "DESAG")]
    public class DESAG
    {
        [XmlElement(ElementName = "DCPCD")]
        public List<string> DCPCD { get; set; }
    }

    /// <summary>
    /// Registration
    /// </summary>
    [XmlRoot(ElementName = "ENN")]
    public class ENN : IAdditionalDocTag
    {
        [XmlElement(ElementName = "DESAG")]
        public DESAG DESAG { get; set; }

        [XmlAttribute(AttributeName = "REGRDAT")]
        public string REGRDAT { get; set; }

        [XmlAttribute(AttributeName = "NOTDATE")]
        public string NOTDATE { get; set; }

        /// <summary>
        /// (891) Дата распространения на РК
        /// </summary>
        [XmlAttribute(AttributeName = "REGEDAT")]
        public string REGEDAT { get; set; }

        [XmlAttribute(AttributeName = "GAZNO")]
        public string GAZNO { get; set; }

        [XmlAttribute(AttributeName = "PUBDATE")]
        public string PUBDATE { get; set; }

        [XmlAttribute(AttributeName = "INTOFF")]
        public string INTOFF { get; set; }
    }

    /// <summary>
    /// Continuation Of effect
    /// </summary>
    [XmlRoot(ElementName = "CEN")]
    public class CEN : IAdditionalDocTag 
    {
        [XmlElement(ElementName = "DCPCD")]
        public List<string> DCPCD { get; set; }
        [XmlAttribute(AttributeName = "REGRDAT")]
        public string REGRDAT { get; set; }
        [XmlAttribute(AttributeName = "NOTDATE")]
        public string NOTDATE { get; set; }
        [XmlAttribute(AttributeName = "REGEDAT")]
        public string REGEDAT { get; set; }
        [XmlAttribute(AttributeName = "GAZNO")]
        public string GAZNO { get; set; }
        [XmlAttribute(AttributeName = "PUBDATE")]
        public string PUBDATE { get; set; }
        public string INTOFF { get; set; }
    }

    /// <summary>
    /// Statement indicating that the mark is protected 
    /// for all the goofs and services requested
    /// </summary>
    [XmlRoot(ElementName = "FINV")]
    public class FINV : IAdditionalDocTag
    {
        [XmlAttribute(AttributeName = "REGRDAT")]
        public string REGRDAT { get; set; }

        [XmlAttribute(AttributeName = "NOTDATE")]
        public string NOTDATE { get; set; }

        [XmlAttribute(AttributeName = "REGEDAT")]
        public string REGEDAT { get; set; }

        [XmlAttribute(AttributeName = "INTOFF")]
        public string INTOFF { get; set; }

        [XmlAttribute(AttributeName = "PUBDATE")]
        public string PUBDATE { get; set; }

        [XmlAttribute(AttributeName = "GAZNO")]
        public string GAZNO { get; set; }
    }

    /// <summary>
    /// Further statement under Rule 18ter(4) indicating
    /// that protection of the mark is granted for all the goods
    /// and services requested
    /// </summary>
    [XmlRoot(ElementName = "FDNV")]
    public class FDNV: IAdditionalDocTag
    {
        [XmlAttribute(AttributeName = "REGRDAT")]
        public string REGRDAT { get; set; }
        [XmlAttribute(AttributeName = "NOTDATE")]
        public string NOTDATE { get; set; }
        [XmlAttribute(AttributeName = "DOCID")]
        public string DOCID { get; set; }
        [XmlAttribute(AttributeName = "REGEDAT")]
        public string REGEDAT { get; set; }
        [XmlAttribute(AttributeName = "GAZNO")]
        public string GAZNO { get; set; }
        [XmlAttribute(AttributeName = "PUBDATE")]
        public string PUBDATE { get; set; }
        [XmlAttribute(AttributeName = "INTOFF")]
        public string INTOFF { get; set; }
    }

    /// <summary>
    /// Goods and Services limited to
    /// </summary>
    [XmlRoot(ElementName = "LIMTO")]
    public class LIMTO
    {
        [XmlElement(ElementName = "GSTERMEN")]
        public string GSTERMEN { get; set; }
        [XmlElement(ElementName = "GSTERMFR")]
        public string GSTERMFR { get; set; }
        [XmlElement(ElementName = "GSTERMES")]
        public string GSTERMES { get; set; }
        [XmlAttribute(AttributeName = "NICCLAI")]
        public string NICCLAI { get; set; }
    }

    /// <summary>
    /// Limitation
    /// </summary>
    [XmlRoot(ElementName = "LIN")]
    public class LIN : IAdditionalDocTag
    {
        [XmlElement(ElementName = "DCPCD")]
        public List<string> DCPCD { get; set; }

        /// <summary>
        /// (511) МКТУ - Описание отказа по классу
        /// </summary>
        [XmlElement(ElementName = "LIMTO")]
        public List<LIMTO> LIMTO { get; set; }
        [XmlElement(ElementName = "GSFOOTEN")]
        public string GSFOOTEN { get; set; }
        [XmlElement(ElementName = "GSFOOTFR")]
        public string GSFOOTFR { get; set; }
        [XmlElement(ElementName = "GSFOOTES")]
        public string GSFOOTES { get; set; }
        [XmlAttribute(AttributeName = "REGRDAT")]
        public string REGRDAT { get; set; }
        [XmlAttribute(AttributeName = "NOTDATE")]
        public string NOTDATE { get; set; }
        [XmlAttribute(AttributeName = "REGEDAT")]
        public string REGEDAT { get; set; }
        [XmlAttribute(AttributeName = "GAZNO")]
        public string GAZNO { get; set; }
        [XmlAttribute(AttributeName = "PUBDATE")]
        public string PUBDATE { get; set; }

        public string INTOFF { get; set; }
    }

    /// <summary>
    /// Renewal
    /// </summary>
    [XmlRoot(ElementName = "REN")]
    public class REN : IAdditionalDocTag
    {
        [XmlElement(ElementName = "DESAG")]
        public DESAG DESAG { get; set; }

        [XmlAttribute(AttributeName = "REGRDAT")]
        public string REGRDAT { get; set; }

        [XmlAttribute(AttributeName = "NOTDATE")]
        public string NOTDATE { get; set; }

        [XmlAttribute(AttributeName = "REGEDAT")]
        public string REGEDAT { get; set; }

        [XmlAttribute(AttributeName = "GAZNO")]
        public string GAZNO { get; set; }

        [XmlAttribute(AttributeName = "PUBDATE")]
        public string PUBDATE { get; set; }

        public string INTOFF { get; set; }

        [XmlElement(ElementName = "DESPG2")]
        public DESPG2 DESPG2 { get; set; }
    }

    [XmlRoot(ElementName = "REN3")]
    public class REN3
    {
        [XmlElement(ElementName = "DCPCD")]
        public List<string> DCPCD { get; set; }
    }

    /// <summary>
    /// Subsequent designation
    /// </summary>
    [XmlRoot(ElementName = "EXN")]
    public class EXN : IAdditionalDocTag
    {
        [XmlElement(ElementName = "DESPG")]
        public DESPG DESPG { get; set; }

        [XmlElement(ElementName = "DESPG2")]
        public DESPG2 DESPG2 { get; set; }

        [XmlElement(ElementName = "INTENTG")]
        public INTENTG INTENTG { get; set; }

        [XmlElement(ElementName = "LIMGR")]
        public LIMGR LIMGR { get; set; }

        [XmlAttribute(AttributeName = "REGRDAT")]
        public string REGRDAT { get; set; }

        [XmlAttribute(AttributeName = "NOTDATE")]
        public string NOTDATE { get; set; }

        [XmlAttribute(AttributeName = "REGEDAT")]
        public string REGEDAT { get; set; }

        [XmlAttribute(AttributeName = "GAZNO")]
        public string GAZNO { get; set; }

        [XmlAttribute(AttributeName = "PUBDATE")]
        public string PUBDATE { get; set; }

        [XmlAttribute(AttributeName = "INTOFF")]
        public string INTOFF { get; set; }
    }

    /// <summary>
    /// Partial provisional refusal of protection
    /// </summary>
    [XmlRoot(ElementName = "RFNP")]
    public class RFNP: IAdditionalDocTag
    {
        [XmlElement(ElementName = "PRF")]
        public PRF PRF { get; set; }
        [XmlAttribute(AttributeName = "FINAL")]
        public string FINAL { get; set; }
        [XmlAttribute(AttributeName = "REGRDAT")]
        public string REGRDAT { get; set; }
        [XmlAttribute(AttributeName = "NOTDATE")]
        public string NOTDATE { get; set; }
        [XmlAttribute(AttributeName = "DOCID")]
        public string DOCID { get; set; }
        [XmlAttribute(AttributeName = "REGEDAT")]
        public string REGEDAT { get; set; }
        [XmlAttribute(AttributeName = "GAZNO")]
        public string GAZNO { get; set; }
        [XmlAttribute(AttributeName = "PUBDATE")]
        public string PUBDATE { get; set; }
        [XmlAttribute(AttributeName = "INTOFF")]
        public string INTOFF { get; set; }
    }

    /// <summary>
    /// Ex officio examination completed but opposition 
    /// or observations by third parties still possible, under Rule 18bis(1)
    /// </summary>
    [XmlRoot(ElementName = "ISN")]
    public class ISN : IAdditionalDocTag
    {
        [XmlAttribute(AttributeName = "REGRDAT")]
        public string REGRDAT { get; set; }

        [XmlAttribute(AttributeName = "NOTDATE")]
        public string NOTDATE { get; set; }

        [XmlAttribute(AttributeName = "DOCID")]
        public string DOCID { get; set; }

        [XmlAttribute(AttributeName = "REGEDAT")]
        public string REGEDAT { get; set; }

        [XmlAttribute(AttributeName = "GAZNO")]
        public string GAZNO { get; set; }

        [XmlAttribute(AttributeName = "PUBDATE")]
        public string PUBDATE { get; set; }

        [XmlAttribute(AttributeName = "OPPERE")]
        public string OPPERE { get; set; }

        [XmlAttribute(AttributeName = "INTOFF")]
        public string INTOFF { get; set; }
    }

    /// <summary>
    /// Statement of grant of protection made under Rule 18ter(1)
    /// </summary>
    [XmlRoot(ElementName = "GP18N")]
    public class GP18N : IAdditionalDocTag
    {
        [XmlAttribute(AttributeName = "REGRDAT")]
        public string REGRDAT { get; set; }

        [XmlAttribute(AttributeName = "NOTDATE")]
        public string NOTDATE { get; set; }

        [XmlAttribute(AttributeName = "DOCID")]
        public string DOCID { get; set; }

        [XmlAttribute(AttributeName = "REGEDAT")]
        public string REGEDAT { get; set; }

        [XmlAttribute(AttributeName = "GAZNO")]
        public string GAZNO { get; set; }

        [XmlAttribute(AttributeName = "PUBDATE")]
        public string PUBDATE { get; set; }

        [XmlAttribute(AttributeName = "INTOFF")]
        public string INTOFF { get; set; }
    }

    /// <summary>
    /// Statement indicating the goods and services for 
    /// which protection of the mark is granted under Rule 18ter(2)(ii)
    /// </summary>
    [XmlRoot(ElementName = "R18NP")]
    public class R18NP: IAdditionalDocTag
    {
        [XmlElement(ElementName = "PRF")]
        public PRF PRF { get; set; }
        [XmlAttribute(AttributeName = "REGRDAT")]
        public string REGRDAT { get; set; }
        [XmlAttribute(AttributeName = "NOTDATE")]
        public string NOTDATE { get; set; }
        [XmlAttribute(AttributeName = "DOCID")]
        public string DOCID { get; set; }
        [XmlAttribute(AttributeName = "REGEDAT")]
        public string REGEDAT { get; set; }
        [XmlAttribute(AttributeName = "GAZNO")]
        public string GAZNO { get; set; }
        [XmlAttribute(AttributeName = "PUBDATE")]
        public string PUBDATE { get; set; }
        [XmlAttribute(AttributeName = "INTOFF")]
        public string INTOFF { get; set; }
    }

    /// <summary>
    /// Confirmation of total provisional refusal under Rule 18ter(3)
    /// </summary>
    [XmlRoot(ElementName = "R18NT")]
    public class R18NT: IAdditionalDocTag
    {
        [XmlAttribute(AttributeName = "REGRDAT")]
        public string REGRDAT { get; set; }
        [XmlAttribute(AttributeName = "NOTDATE")]
        public string NOTDATE { get; set; }
        [XmlAttribute(AttributeName = "DOCID")]
        public string DOCID { get; set; }
        [XmlAttribute(AttributeName = "REGEDAT")]
        public string REGEDAT { get; set; }
        [XmlAttribute(AttributeName = "GAZNO")]
        public string GAZNO { get; set; }
        [XmlAttribute(AttributeName = "PUBDATE")]
        public string PUBDATE { get; set; }
        [XmlAttribute(AttributeName = "INTOFF")]
        public string INTOFF { get; set; }
    }

    [XmlRoot(ElementName = "PRF")]
    public class PRF
    {
        [XmlElement(ElementName = "LIMTO")]
        public List<LIMTO> LIMTO { get; set; }
        [XmlElement(ElementName = "GSFOOTEN")]
        public string GSFOOTEN { get; set; }
        [XmlElement(ElementName = "GSFOOTFR")]
        public string GSFOOTFR { get; set; }
        [XmlElement(ElementName = "GSFOOTES")]
        public string GSFOOTES { get; set; }
        [XmlAttribute(AttributeName = "ORIGLAN")]
        public string ORIGLAN { get; set; }
    }

    /// <summary>
    /// Partial Invalidation
    /// </summary>
    [XmlRoot(ElementName = "INNP")]
    public class INNP: IAdditionalDocTag
    {
        [XmlElement(ElementName = "PRF")]
        public PRF PRF { get; set; }
        [XmlAttribute(AttributeName = "REGRDAT")]
        public string REGRDAT { get; set; }
        [XmlAttribute(AttributeName = "NOTDATE")]
        public string NOTDATE { get; set; }
        [XmlAttribute(AttributeName = "DOCID")]
        public string DOCID { get; set; }
        [XmlAttribute(AttributeName = "REGEDAT")]
        public string REGEDAT { get; set; }
        [XmlAttribute(AttributeName = "GAZNO")]
        public string GAZNO { get; set; }
        [XmlAttribute(AttributeName = "PUBDATE")]
        public string PUBDATE { get; set; }
        [XmlAttribute(AttributeName = "INTOFF")]
        public string INTOFF { get; set; }
    }

    /// <summary>
    /// Opposition possible after 18 months time limit
    /// </summary>
    [XmlRoot(ElementName = "OPN")]
    public class OPN : IAdditionalDocTag
    {
        [XmlAttribute(AttributeName = "REGRDAT")]
        public string REGRDAT { get; set; }

        [XmlAttribute(AttributeName = "NOTDATE")]
        public string NOTDATE { get; set; }

        [XmlAttribute(AttributeName = "REGEDAT")]
        public string REGEDAT { get; set; }

        [XmlAttribute(AttributeName = "GAZNO")]
        public string GAZNO { get; set; }

        [XmlAttribute(AttributeName = "PUBDATE")]
        public string PUBDATE { get; set; }

        [XmlAttribute(AttributeName = "INTOFF")]
        public string INTOFF { get; set; }

        [XmlAttribute(AttributeName = "OPPERS")]
        public string OPPERS { get; set; }

        [XmlAttribute(AttributeName = "OPPERE")]
        public string OPPERE { get; set; }
    }

    /// <summary>
    /// Total provisional refusal of protection
    /// </summary>
    [XmlRoot(ElementName = "RFNT")]
    public class RFNT : IAdditionalDocTag
    {
        [XmlAttribute(AttributeName = "REGRDAT")]
        public string REGRDAT { get; set; }

        [XmlAttribute(AttributeName = "NOTDATE")]
        public string NOTDATE { get; set; }

        [XmlAttribute(AttributeName = "DOCID")]
        public string DOCID { get; set; }

        [XmlAttribute(AttributeName = "REGEDAT")]
        public string REGEDAT { get; set; }

        [XmlAttribute(AttributeName = "GAZNO")]
        public string GAZNO { get; set; }

        [XmlAttribute(AttributeName = "PUBDATE")]
        public string PUBDATE { get; set; }

        [XmlAttribute(AttributeName = "INTOFF")]
        public string INTOFF { get; set; }
    }

    /// <summary>
    /// Statement of grant of protection following 
    /// a provisional refusal under Rule 18ter (2)(i)
    /// </summary>
    [XmlRoot(ElementName = "R18NV")]
    public class R18NV: IAdditionalDocTag
    {
        [XmlAttribute(AttributeName = "REGRDAT")]
        public string REGRDAT { get; set; }

        [XmlAttribute(AttributeName = "NOTDATE")]
        public string NOTDATE { get; set; }

        [XmlAttribute(AttributeName = "DOCID")]
        public string DOCID { get; set; }

        [XmlAttribute(AttributeName = "REGEDAT")]
        public string REGEDAT { get; set; }

        [XmlAttribute(AttributeName = "GAZNO")]
        public string GAZNO { get; set; }

        [XmlAttribute(AttributeName = "PUBDATE")]
        public string PUBDATE { get; set; }

        [XmlAttribute(AttributeName = "INTOFF")]
        public string INTOFF { get; set; }
    }

    [XmlRoot(ElementName = "MARKGR")]
    public class MARKGR
    {
        [XmlElement(ElementName = "CURRENT")]
        public CURRENT CURRENT { get; set; }

        /// <summary>
        /// 17.1 Registration
        /// </summary>
        [XmlElement(ElementName = "ENN")]
        public List<ENN> ENN { get; set; }

        /// <summary>
        /// 17.2 Subsequent designation
        /// </summary>
        [XmlElement(ElementName = "EXN")]
        public List<EXN> EXN { get; set; }

        /// <summary>
        /// 17.3 Continuation of effect
        /// </summary>
        [XmlElement(ElementName = "CEN")]
        public List<CEN> CENS { get; set; }

        /// <summary>
        /// 17.4 Total provisional refusal of protection
        /// </summary>
        [XmlElement(ElementName = "RFNT")]
        public List<RFNT> RFNT { get; set; }

        /// <summary>
        /// 17.5 Statement indicating that the mark is protected 
        /// for all the goofs and services requested
        /// </summary>
        [XmlElement(ElementName = "FINV")]
        public List<FINV> FINV { get; set; }

        /// <summary>
        /// 17.6 Renewal
        /// </summary>
        [XmlElement(ElementName = "REN")]
        public List<REN> REN { get; set; }

        [XmlElement(ElementName = "REN3")]
        public List<REN3> REN3 { get; set; }

        /// <summary>
        /// 17.7 Limitation 
        /// </summary>
        [XmlElement(ElementName = "LIN")]
        public List<LIN> LIN { get; set; }

        /// <summary>
        /// 17.8 Statement of grant of protection made under Rule 18ter(1)
        /// </summary>
        [XmlElement(ElementName = "GP18N")]
        public List<GP18N> GP18N { get; set; }

        /// <summary>
        /// 17.9 Ex officio examination completed but opposition 
        /// or observations by third parties still possible, under Rule 18bis(1)
        /// </summary>
        [XmlElement(ElementName = "ISN")]
        public List<ISN> ISN { get; set; }

        /// <summary>
        /// 17.10 Final decision under rule 18 refusing all of the goods and services 
        /// </summary>
        [XmlElement(ElementName = "R18NT")]
        public List<R18NT> R18NT { get; set; }

        /// <summary>
        /// 17.11 Partial Invalidation
        /// </summary>
        [XmlElement(ElementName = "INNP")]
        public List<INNP> INNP { get; set; }

        /// <summary>
        /// 17.12 Statement indicating the goods and services for 
        /// which protection of the mark is granted under Rule 18ter(2)(ii)
        /// </summary>
        [XmlElement(ElementName = "R18NP")]
        public List<R18NP> R18NP { get; set; }

        /// <summary>
        /// 17.13 Further statement under Rule 18ter(4) indicating
        /// that protection of the mark is granted for all the goods
        /// and services requested
        /// </summary>
        [XmlElement(ElementName = "FDNV")]
        public List<FDNV> FDNV { get; set; }

        /// <summary>
        /// 17.14 Opposition possible after 18 months time limit
        /// </summary>
        [XmlElement(ElementName = "OPN")]
        public List<OPN> OPN { get; set; }

        /// <summary>
        /// 17.15 Statement of grant of protection following 
        /// a provisional refusal under Rule 18ter (2)(i)
        /// </summary>
        [XmlElement(ElementName = "R18NV")]
        public List<R18NV> R18NV { get; set; }

        /// <summary>
        /// 17.16 Partial provisional refusal of protection
        /// </summary>
        [XmlElement(ElementName = "RFNP")]
        public List<RFNP> RFNP { get; set; }







        /// <summary>
        /// (21) Регистрационный номер заявки
        /// (11) Номер охранного документа
        /// </summary>
        [XmlAttribute(AttributeName = "INTREGN")]
        public string INTREGN { get; set; }

        [XmlAttribute(AttributeName = "BILING")]
        public string BILING { get; set; }

        [XmlAttribute(AttributeName = "OOCD")]
        public string OOCD { get; set; }

        /// <summary>
        /// (22) Дата подачи заявки
        /// (151) Дата охранного документа
        /// </summary>
        [XmlAttribute(AttributeName = "INTREGD")]
        public string INTREGD { get; set; }

        /// <summary>
        /// (180) Срок действия ОД
        /// </summary>
        [XmlAttribute(AttributeName = "EXPDATE")]
        public string EXPDATE { get; set; }

        [XmlAttribute(AttributeName = "ORIGLAN")]
        public string ORIGLAN { get; set; }
    }

    [XmlRoot(ElementName = "ROMARIN")]
    public class RomarinDto
    {
        [XmlElement(ElementName = "MARKGR")]
        public List<MARKGR> MARKGRS { get; set; }

        [XmlAttribute(AttributeName = "EXTRDATE")]
        public string EXTRDATE { get; set; }
    }
}
