using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Dictionaries;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.Domain.EntitiesFile;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.Infrastructure
{
    public class TemplateUserInputChecker : ITemplateUserInputChecker
    {
        private readonly IExecutor _executor;
        private readonly IDocxTemplateHelper _docxTemplateHelper;

        public TemplateUserInputChecker(IExecutor executor, IDocxTemplateHelper docxTemplateHelper)
        {
            _executor = executor;
            _docxTemplateHelper = docxTemplateHelper;
        }

        public bool GetConfig(string templateCode, out UserInputConfigDto fieldsConfig)
        {
            var documentType = _executor.GetQuery<GetDocumentTypesByCodeQuery>().Process(q => q.Execute(templateCode))
                .FirstOrDefault();

            if (documentType?.TemplateFileId == null)
            {
                fieldsConfig = null;
                return false;
            }

            var documentTemplateFile = documentType.TemplateFile;

            if (documentTemplateFile?.File == null)
            {
                fieldsConfig = null;
                return false;
            }

            var customFieldNames = _docxTemplateHelper.GetUserInputFieldNames(documentTemplateFile.File);

            fieldsConfig = BuildDynamicFormConfig(customFieldNames);

            return fieldsConfig.RequireUserInput;
        }

        private static UserInputConfigDto BuildDynamicFormConfig(Dictionary<string, string> customFieldNames)
        {
            return new UserInputConfigDto
            {
                FieldsConfig = customFieldNames.Select(x =>
                        new UserInputConfigDto.UserInputFieldConfig
                        {
                            Key = x.Key,
                            Label = x.Value,
                            Value = string.Empty,
                            RichInput = x.Key.EndsWith(TemplateFieldName._RichUserInput.ToString()),
                            Required = false
                        })
                    .ToList()
            };
        }
    }
}