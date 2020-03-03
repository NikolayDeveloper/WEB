using System;
using System.Text;

namespace Iserv.Niis.ExternalServices.Features.Utils
{
    public static class ExceptionHelper
    {
        public static string ExceptionFullText(Exception exception)
        {
            var sb = new StringBuilder();
            sb.AppendLine(exception.Message);
            sb.AppendLine(exception.GetType().ToString());

            if (exception.InnerException != null)
                sb.AppendLine("Has inner exception! See below.");

            sb.AppendLine(exception.StackTrace);

            if (exception.InnerException != null)
            {
                sb.AppendLine("Inner exception:");
                sb.AppendLine(ExceptionFullText(exception.InnerException));
            }

            return sb.ToString();
        }
    }
}