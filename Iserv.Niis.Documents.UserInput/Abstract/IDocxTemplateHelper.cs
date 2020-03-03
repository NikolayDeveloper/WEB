using System.Collections.Generic;
using System.IO;

namespace Iserv.Niis.Documents.UserInput.Abstract
{
    public interface IDocxTemplateHelper
    {
        void FillUserInputContent(MemoryStream stream, List<KeyValuePair<string, string>> userInputFields, string defaultValue);
        void RemoveComments(MemoryStream stream);
        Dictionary<string, string> GetUserInputFieldNames(byte[] buffer);
        //List<string> GetDocumentTags(byte[] buffer);
    }
}