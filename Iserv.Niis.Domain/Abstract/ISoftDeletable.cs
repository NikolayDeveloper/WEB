using System;

namespace Iserv.Niis.Domain.Abstract
{
	/// <summary>
	/// Представляет возможность мягкого удаления сущности
	/// </summary>
	public interface ISoftDeletable
	{
		/// <summary>
		/// Флаг удалена ли сущность
		/// </summary>
		bool IsDeleted { get; set; }

		/// <summary>
		/// Дата удаления сущности
		/// </summary>
		DateTimeOffset? DeletedDate { get; set; }
	}
}