using System.Collections.Generic;
using Iserv.Niis.FileConverter;

namespace Iserv.Niis.Documents.Abstractions
{
    public interface IDocumentGenerator
    {
        /// <summary>
        ///     Строит документ из шаблона
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        GeneratedDocument Process(Dictionary<string, object> parameters);
    }
}