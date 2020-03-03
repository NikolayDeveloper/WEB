using System.Linq;
using TemplateEngine.Docx;

namespace Iserv.Niis.Documents.Helpers
{
    public static class TemplateProcessorExtensions
    {
        public static TemplateProcessor RemoveCommentsFromTemplate(this TemplateProcessor processor)
        {
            foreach (var node in processor.Document.Descendants().ToList())
                if (node.Name.LocalName.Contains("comment"))
                    node.Remove();

            return processor;
        }
    }
}