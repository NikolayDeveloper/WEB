using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    /// <summary>
    /// Должности
    /// </summary>
    public class DicPosition : DictionaryEntity<int>, IReference, IHaveConcurrencyToken
    {
        public bool IsMonitoring { get; set; }
        public bool? IsHead { get; set; }

        #region Relationships

        public int? DepartmentId { get; set; }
        public DicDepartment Department { get; set; }

        public int PositionTypeId { get; set; }
        public DicPositionType PositionType { get; set; }

        #endregion

        public override string ToString()
        {
            return NameRu;
		}

	    #region Public codes

	    public const int PresidentId = 771;//ToDo: Нужно актуализировать справочники и перети на коды

	    #endregion
	}
}