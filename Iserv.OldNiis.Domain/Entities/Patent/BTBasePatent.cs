using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIIS.DBConverter.Entities.Patent {
    [Table("BT_BASE_PATENT")]
    public class BtBasePatent {
        [Column("U_ID")]
        public int Id { get; set; }

        [Column("TYPE_ID")]
        public int TypeId { get; set; }

        [Column("STATUS_ID")]
        public int? StatusId { get; set; }

        [Column("GOS_NUMBER_11")]
        public string GosNumber11 { get; set; }

        [Column("GOS_DATE_11")]
        public DateTime? GosDate11 { get; set; }

        [Column("REQ_NUMBER_21")]
        public string ReqNumber21 { get; set; }

        [Column("REQ_DATE_22")]
        public DateTime? ReqDate22 { get; set; }

        [Column("NAME_540_EN")]
        public string Name540En { get; set; }

        [Column("NAME_540_RU")]
        public string Name540Ru { get; set; }

        [Column("NAME_540_KZ")]
        public string Name540Kz { get; set; }

        [Column("CONSID_ID")]
        public int? ConsidId { get; set; }

        [Column("TYPE_REQUEST")]
        public int? TypeRequest { get; set; }

        [Column("TRASLITERATION")]
        public string Transliteration { get; set; }

        [Column("SELECTION_NAME_OFFER")]
        public string SelectionNameOffer { get; set; }

        [Column("SELECTION_NUMBER")]
        public string SelectionNumber { get; set; }

        [Column("DECLARANT_EMPLOYER")]
        public string DeclarantEmployer { get; set; }

        [Column("COPYRIGHT_EMPLOYER")]
        public string CopyrightEmployer { get; set; }

        [Column("COPYRIGHT_AUTOR")]
        public string CopyrightAuthor { get; set; }

        [Column("INHERIT")]
        public string Inherit { get; set; }

        [Column("PRIZA")]
        public int? Priza { get; set; }

        [Column("OTKAZ")]
        public int? Otkaz { get; set; }

        [Column("KODTIP")]
        public int? KodTip { get; set; }

        [Column("JZ")]
        public int? Jz { get; set; }

        [Column("KODETAP")]
        public int? KodEtap { get; set; }

        [Column("WAIT_FE")]
        public int? WaitFe { get; set; }

        [Column("IZ3PER")]
        public int? Iz3Per { get; set; }

        [Column("DPOS")]
        public DateTime? DPos { get; set; }

        [Column("DPOP")]
        public DateTime? DPop { get; set; }

        [Column("PROCH")]
        public string Proch { get; set; }

        [Column("DDOK")]
        public string DDok { get; set; }

        [Column("TIP_PAT")]
        public int? TipPat { get; set; }

        [Column("KODETAP1")]
        public int? KodEtap1 { get; set; }

        [Column("GO_ORG")]
        public DateTime? GoOrg { get; set; }

        [Column("GO_OEI")]
        public DateTime? GoOei { get; set; }

        [Column("KOD_OEI")]
        public string KodOei { get; set; }

        [Column("KOD_EXP")]
        public string KodExp { get; set; }

        [Column("DVIDOD")]
        public DateTime? DviDod { get; set; }

        [Column("DATA_POL")]
        public DateTime? DataPol { get; set; }

        [Column("DATA_END")]
        public DateTime? DataEnd { get; set; }

        [Column("MOSKOW")]
        public int? Moskow { get; set; }

        [Column("IN_DATE")]
        public DateTime? InDate { get; set; }

        [Column("OLD_RESH")]
        public string OldResh { get; set; }

        [Column("WAIT_ANSW")]
        public int? WaitAnsw { get; set; }

        [Column("DVIDODPP")]
        public DateTime? DviDodPp { get; set; }

        [Column("OUT_DATE")]
        public DateTime? OutDate { get; set; }

        [Column("NPCTIPEA")]
        public int? NpcTipea { get; set; }

        [Column("NPSTISA")]
        public int? NpsTisa { get; set; }

        [Column("NKPPYB")]
        public int? NkpPyb { get; set; }

        [Column("DATGRP")]
        public DateTime? DatGrp { get; set; }

        [Column("DATGRE")]
        public DateTime? DatGre { get; set; }

        [Column("DATPDPAT")]
        public DateTime? DatPdpat { get; set; }

        [Column("KODPDPAT")]
        public int? KodPdPat { get; set; }

        [Column("LICENZ")]
        public string Licenz { get; set; }

        [Column("LICENZ1")]
        public string Licenz1 { get; set; }

        [Column("LICOPEN")]
        public int? LicOpen { get; set; }

        [Column("NAC")]
        public string Nac { get; set; }

        [Column("DVPP")]
        public string Dvpp { get; set; }

        [Column("DPRINT")]
        public DateTime? DPrint { get; set; }

        [Column("DFIRST")]
        public DateTime? DFirst { get; set; }

        [Column("DQUEST")]
        public DateTime? DQuest { get; set; }

        [Column("DOTZIV")]
        public DateTime? DOtzyv { get; set; }

        [Column("DMOSKOW")]
        public DateTime? DMoskow { get; set; }

        [Column("DMOSKOW2")]
        public DateTime? DMoskow2 { get; set; }

        [Column("DPVZ")]
        public DateTime? DPvz { get; set; }

        [Column("DHODVOST")]
        public DateTime? DHodvost { get; set; }

        [Column("GOD_PROD")]
        public int? GodProd { get; set; }

        [Column("DATHOD")]
        public DateTime? DatHod { get; set; }

        [Column("DES")]
        public DateTime? Des { get; set; }

        [Column("DATA_END2")]
        public DateTime? DataEnd2 { get; set; }

        [Column("DSDAK")]
        public DateTime? DSdak { get; set; }

        [Column("DATGRPA")]
        public DateTime? DatGrpa { get; set; }

        [Column("CPF")]
        public int? Cpf { get; set; }

        [Column("PREDS")]
        public string Preds { get; set; }

        [Column("IN_DATEF")]
        public DateTime? InDatef { get; set; }

        [Column("DQUESTF")]
        public DateTime? DQuestF { get; set; }

        [Column("OUT_DATEF")]
        public DateTime? OutDateF { get; set; }

        [Column("DOTZIVF")]
        public DateTime? DOtzyvF { get; set; }

        [Column("DOTVETDO")]
        public DateTime? DOtvetDo { get; set; }

        [Column("OSN4")]
        public int? Osn4 { get; set; }

        [Column("DOSPYB")]
        public int? Dospyb { get; set; }

        [Column("PARO")]
        public string Paro { get; set; }

        [Column("DATUVO4")]
        public DateTime? DatUvo4 { get; set; }

        [Column("DHODPROD")]
        public DateTime? DHodProd { get; set; }

        [Column("PROM")]
        public string Prom { get; set; }

        [Column("DATAZAKPP")]
        public DateTime? DataZakPp { get; set; }

        [Column("DATAZAKP")]
        public DateTime? DataZakP { get; set; }

        [Column("SUBTYPE_ID")]
        public int? SubTypeId { get; set; }

        [Column("SYS_IMAGE")]
        public string SysImage { get; set; }

        [Column("IMAGE")]
        public byte[] Image { get; set; }

        [Column("DATE_85")]
        public DateTime? Date85 { get; set; }

        [Column("TYPEII_ID")]
        public int? TypeIiId { get; set; }

        [Column("NBY")]
        public string NBy { get; set; }

        [Column("DBY")]
        public DateTime? DBy { get; set; }

        [Column("GOSP")]
        public string Gosp { get; set; }

        [Column("REF_57")]
        public string Ref57 { get; set; }

        [Column("STZ17")]
        public DateTime? Stz17 { get; set; }

        [Column("STZ176")]
        public DateTime? Stz176 { get; set; }

        [Column("STZ156")]
        public DateTime? Stz156 { get; set; }

        [Column("DISCLAM_RU")]
        public string DisclamRu { get; set; }

        [Column("DISCLAM_KZ")]
        public string DisclamKz { get; set; }

        [Column("stamp")]
        public DateTime? Stamp { get; set; }

        [Column("date_create")]
        public DateTime? DateCreate { get; set; }
       

        [Column("flAddress")]
        public int? FlAddress { get; set; }

        [Column("NM60")]
        public string Nm60 { get; set; }

        [Column("SELECTION_FAMILY")]
        public string SelectionFamily { get; set; }

        [Column("SYS_ImageSmall")]
        public byte[] SysImageSmall { get; set; }






        [Column("TO_PM")]
        public int? ToPm { get; set; }
        [Column("INF_P")]
        public int? InfP { get; set; }
        [Column("DIZYAT")]
        public DateTime? DIzyat { get; set; }
        [Column("KPPP")]
        public int? Kppp { get; set; }
        [Column("RESHFIPS")]
        public int? ReshFips { get; set; }
        [Column("DVIDPI")]
        public DateTime? DviDpi { get; set; }



        [Column("storona1")]
        public int? Storona1 { get; set; }
        [Column("storona2")]
        public int? Storona2 { get; set; }
    }

}