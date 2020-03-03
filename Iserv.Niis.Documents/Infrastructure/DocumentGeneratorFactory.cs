using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Aspose.Words.Lists;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentGenerators;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.FileStorage.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.Infrastructure
{
    public class DocumentGeneratorFactory : IDocumentGeneratorFactory
    {
        private readonly IExecutor _executor;
        private readonly ITemplateFieldValueFactory _templateFieldValueFactory;
        private readonly IFileConverter _fileConverter;
        private readonly IDocxTemplateHelper _docxTemplateHelper;
        private readonly IFileStorage _fileStorage;

        public DocumentGeneratorFactory(
            IExecutor executor,
            ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter,
            IDocxTemplateHelper docxTemplateHelper,
            IFileStorage fileStorage)
        {
            _executor = executor;
            _templateFieldValueFactory = templateFieldValueFactory;
            _fileConverter = fileConverter;
            _docxTemplateHelper = docxTemplateHelper;
            _fileStorage = fileStorage;
        }
        public IDocumentGenerator Create(string templateCode)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var executingAssemblyTypes = executingAssembly.GetTypes();
            var executingAssemblyFilteredTypes = executingAssemblyTypes
                .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(DocumentGeneratorBase))).ToList();

            var specificImplementations = new List<Type>();

            foreach (var executingAssemblyFilteredType in executingAssemblyFilteredTypes)
            {
                var attribute = executingAssemblyFilteredType.GetCustomAttribute<DocumentGeneratorAttribute>();
                if (attribute == null) continue;

                if (string.IsNullOrEmpty(attribute.DocumentTypeCode))
                    continue;

                if (attribute.DocumentTypeCode == templateCode)
                {
                    specificImplementations.Add(executingAssemblyFilteredType);
                }
            }

            var specificImplementation = specificImplementations.FirstOrDefault();

            if (specificImplementation == null)
                return null;

            if (typeof(IComplexDataDocumentGenerator).IsAssignableFrom(specificImplementation))
            {
                return Activator.CreateInstance((dynamic)specificImplementation, _executor, _templateFieldValueFactory,
                    _fileConverter, _docxTemplateHelper, _fileStorage);
            }

            return Activator.CreateInstance((dynamic) specificImplementation, _executor, _templateFieldValueFactory,
                    _fileConverter, _docxTemplateHelper);
        }
    }
}