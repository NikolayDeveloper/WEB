namespace Iserv.Niis.Integration.Romarin.BL.Interfaces
{
    public interface IAdditionalDocTag
    {
        // DESAG DESAG { get; set; }
        string REGRDAT { get; set; }

        string NOTDATE { get; set; }
        /// <summary>
        /// (891) Дата распространения на РК
        /// </summary>
        string REGEDAT { get; set; }

        string GAZNO { get; set; }

        string PUBDATE { get; set; }

        string INTOFF { get; set; }
    }
}