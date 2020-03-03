using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Integration
{
    /// <summary> Статус интеграционного пакета ЦОН </summary>
    public class IntegrationConPackageState : Entity<int>, ISimpleReference
    {
        #region Properties

        /// <summary> Код </summary>
        public int Code { get; set; }

        /// <summary> Название на русском языке </summary>
        public string NameRu { get; set; }

        /// <summary> Название на казахском языке </summary>
        public string NameKz { get; set; }

        /// <summary> Название на английском языке </summary>
        public string NameEn { get; set; }

        #endregion

        #region Methods

        /// <summary> Получить строковое представление </summary>
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(NameRu))
                return NameRu;
            if (!string.IsNullOrEmpty(NameKz))
                return NameKz;
            if (!string.IsNullOrEmpty(NameEn))
                return NameEn;
            return Id.ToString();
        }

        #endregion
    }
}