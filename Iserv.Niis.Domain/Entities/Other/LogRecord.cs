using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Domain.Enums;

namespace Iserv.Niis.Domain.Entities.Other
{
    /// <summary>
    /// Лог
    /// </summary>
    public class LogRecord : Entity<int>
	{
		public LogErrorType LogErrorType { get; set; }
		public LogType LogType { get; set; }
		public string Message { get; set; }
		public int? UserId { get; set; }
		public ApplicationUser User { get; set; }
	}
}