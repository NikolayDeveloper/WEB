using System.Linq;

namespace Iserv.Niis.InternalServices.Features.Utils
{
    public static class LinesHelper
    {
        public static string ConcatNotEmptyStrings(params string[] values)
        {
            return string.Join(", ", values.Where(s => !string.IsNullOrEmpty(s)));
        }
    }
}