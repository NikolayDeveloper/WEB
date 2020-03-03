using System;

namespace Iserv.Niis.Domain.Entities.Integration
{
    /// <summary> Интеграционный пакет ЦОН </summary>
    public class IntegrationConPackage
    {
        #region Properties

        /// <summary> ИД </summary>
        public long Id { get; set; }

        public DateTimeOffset DateCreate { get; set; }
        public DateTimeOffset? DateProcess { get; set; }
        public int DocumentId { get; set; }
        public int TypeId { get; set; }
        public IntegrationConPackageType Type { get; set; }
        public int StateId { get; set; }
        public IntegrationConPackageState State { get; set; }
        public string PackageData { get; set; }
        public string ProcessError { get; set; }

        #endregion
    }
}