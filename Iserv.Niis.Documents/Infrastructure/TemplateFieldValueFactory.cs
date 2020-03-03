using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.Infrastructure
{
    public class TemplateFieldValueFactory : ITemplateFieldValueFactory
    {
        private readonly IExecutor _executor;

        public TemplateFieldValueFactory(IExecutor executor)
        {
            _executor = executor;
        }

        public ITemplateFieldValue Create(TemplateFieldName templateFieldName)
        {
            return CreateSome(templateFieldName).First();
        }

        public IEnumerable<ITemplateFieldValue> CreateSome(params TemplateFieldName[] templateFieldNames)
        {
				var specificImplementations = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.IsClass
                            && !x.IsAbstract
                            && x.IsSubclassOf(typeof(TemplateFieldValueBase))
                            && templateFieldNames.Contains(x.GetCustomAttribute<TemplateFieldNameAttribute>()
                                .Name));

            return specificImplementations
                .Select(t => (ITemplateFieldValue)Activator.CreateInstance(t, _executor));
        }
    }
}