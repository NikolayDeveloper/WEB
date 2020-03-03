using System.IO;
using Iserv.Niis.Domain.EntitiesFile;
using Iserv.Niis.Exceptions;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Documents.TemplateFiles
{
    public class UpdateTemplateFilesHandler: BaseHandler
    {
        public void Execute(string path)
        {
            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path);
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);
                    var bytes = File.ReadAllBytes(file);
                    var templateFile = Executor.GetQuery<GetDocumentTemplateFileByFileNameQuery>()
                        .Process(q => q.Execute(fileName));
                    if (templateFile == null)
                        throw new DataNotFoundException(nameof(DocumentTemplateFile),
                            DataNotFoundException.OperationType.Read, fileName);
                    templateFile.File = bytes;
                    Executor.GetCommand<UpdateDocumentTemplateFileCommand>().Process(c => c.Execute(templateFile));
                }
            }
        }
    }
}
