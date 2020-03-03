using System;
using System.Globalization;

namespace Iserv.Niis.Documents.Helpers
{
	internal static class TemplateFieldValueExtensions
	{
		internal static string ToTemplateDateFormat(this DateTimeOffset dateTimeOffset)
		{
			return dateTimeOffset.LocalDateTime.ToString("dd.MM.yyyy");
		}

		internal static string ToTemplateDateFormat(this DateTimeOffset? dateTimeOffset)
		{
			return dateTimeOffset?.LocalDateTime.ToString("dd.MM.yyyy") ?? string.Empty;
		}

		internal static string ToTemplateDateFormatWithWordMonth(this DateTimeOffset dateTimeOffset)
		{
			return dateTimeOffset.LocalDateTime.ToString("dd MMMM yyyy", new CultureInfo("ru-RU")) ?? string.Empty;
		}

        /// <summary>
        /// Возвращает значение, которое должно быть в шаблоне.
        /// </summary>
        /// <param name="value">Логическое значение.</param>
        /// <returns>Строка с значением.</returns>
        internal static string ToTemplateFormat(this bool value)
        {
            return value ? "Да" : "Нет";
        }
	}
}