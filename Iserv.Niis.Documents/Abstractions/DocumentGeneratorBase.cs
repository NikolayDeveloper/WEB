using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Dictionaries;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Documents;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.Helpers;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.EntitiesFile;
using Iserv.Niis.FileConverter;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using TemplateEngine.Docx;

namespace Iserv.Niis.Documents.Abstractions
{
    /// <summary>
    ///     Базовый класс для генерации документа из шаблона
    /// </summary>
    public abstract class DocumentGeneratorBase : IDocumentGenerator
    {   
        protected const string UserInputFieldsParameterName = "UserInputFields";
        protected readonly IExecutor Executor;
        private readonly QrCodeHelper _qrCodeHelper;
        private readonly (int id, string code) _documentType;
        private readonly IDocxTemplateHelper _docxTemplateHelper;
        private readonly IFileConverter _fileConverter;
        protected readonly ITemplateFieldValueFactory TemplateFieldValueFactory;
        private DocumentTemplateFile _documentTemplateFile;
        private Content _valuesToFill;
        protected Dictionary<string, object> Parameters;
        protected string DefaultUserInputValue { get; set; }
        
        /// <summary>
        /// Позиция QR-кода в документе.
        /// </summary>
        public QrCodePosition QrCodePosition { get; protected set; }

        protected DocumentGeneratorBase(IExecutor executor, ITemplateFieldValueFactory templateFieldValueFactory,
            IFileConverter fileConverter, IDocxTemplateHelper docxTemplateHelper)
        {
            Executor = executor;
            TemplateFieldValueFactory = templateFieldValueFactory;
            _fileConverter = fileConverter;
            _docxTemplateHelper = docxTemplateHelper;
            //_documentType = (id: Convert.ToInt32(Parameters["DocumentId"]), code: attribute.DocumentTypeCode);
            _qrCodeHelper = new QrCodeHelper(_documentType.code, Executor);

            QrCodePosition = QrCodePosition.None;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Строит документ из шаблона
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public GeneratedDocument Process(Dictionary<string, object> parameters)
        {
            CheckRequired(parameters);

            Initialize(parameters);

           // RegisterDocNumHelper.Instance.Add(DateTime.Now);

            _valuesToFill = PrepareValue();

            return Fill();
        }

        /// <summary>
        ///     В этом методе необходимо проинициализировать переменную <see cref="_valuesToFill" />
        /// </summary>
        protected abstract Content PrepareValue();

        /// <summary>
        ///     Список обязательных параметров для генерации документа
        /// </summary>
        protected abstract IEnumerable<string> RequiredParameters();

        /// <summary>
        ///     Достает из БД шаблон для конкретного отчета в локальную переменную <see cref="_documentTemplateFile" />
        /// </summary>
        private void Initialize(Dictionary<string, object> parameters)
        {
            Parameters = parameters;
            DicDocumentType documentType;
            var code = string.Empty;
            if (Parameters.ContainsKey("DocumentId"))
            {
                var documentId = Convert.ToInt32(Parameters["DocumentId"]);
                var document = Executor.GetQuery<GetDocumentByIdQuery>().Process(q => q.Execute(documentId));
                code = document?.Type?.Code;
            }
            else
            {
                var attribute = GetType().GetCustomAttribute<DocumentGeneratorAttribute>();
                code = attribute.DocumentTypeCode;
            }

            documentType = Executor.GetQuery<GetDocumentTypesByCodeQuery>()
                .Process(q => q.Execute(code)).FirstOrDefault();


            _documentTemplateFile = documentType?.TemplateFile;
        }

        /// <summary>
        /// Устанавливает значения для шаблона документа и сохраняет в локальную переменную
        /// </summary>
        private GeneratedDocument Fill()
        {
            using (var memoryStream = new MemoryStream())
            {
                memoryStream.Write(_documentTemplateFile.File, 0, _documentTemplateFile.File.Length);
                memoryStream.Seek(0, SeekOrigin.Begin);               


                using (var outputDocument = new TemplateProcessor(memoryStream)
                    .SetRemoveContentControls(true)
                    .RemoveCommentsFromTemplate())
                {
                    outputDocument.FillContent(_valuesToFill);
                    outputDocument.SaveChanges();
                }

                Parameters.TryGetValue(UserInputFieldsParameterName, out var userInputFields);

                _docxTemplateHelper.RemoveComments(memoryStream);
                _docxTemplateHelper.FillUserInputContent(memoryStream,
                    userInputFields as List<KeyValuePair<string, string>>, DefaultUserInputValue);

                if (Parameters.ContainsKey("DocumentId") && Parameters.ContainsKey("RequestId") && QrCodePosition != QrCodePosition.None)
                {
                    InsertQrCode(memoryStream);
                }

                return _fileConverter.DocxToPdf(memoryStream, _documentTemplateFile.FileName.Replace(".docx", ".pdf"));
            }
        }

        /// <summary>
        /// Вставляет в документ QR-коды колонтитула.
        /// </summary>
        /// <param name="memoryStream">Документ.</param>
        private void InsertQrCode(MemoryStream memoryStream)
        {
            switch (QrCodePosition)
            {
                case QrCodePosition.Header:
                    _qrCodeHelper.InsertToHeader(memoryStream, Parameters);
                    break;

                case QrCodePosition.Footer:
                    _qrCodeHelper.InsertToFooter(memoryStream, Parameters);
                    break;
            }
        }


        /// <summary>
        ///  Проверяет обязательные параметры для корректной генерации отчета
        /// </summary>
        /// <param name="parameters"></param>
        private void CheckRequired(Dictionary<string, object> parameters)
        {
            foreach (var key in RequiredParameters())
                if (!parameters.ContainsKey(key))
                    throw new ArgumentNullException();
        }

        #region FieldHelpers

        protected FieldContent BuildField(TemplateFieldName templateFieldName)
        {
            return new FieldContent(templateFieldName.ToString(),
                TemplateFieldValueFactory.Create(templateFieldName).Get(Parameters));
        }

        protected ImageContent BuildImage(TemplateFieldName templateFieldName)
        {
            return new ImageContent(templateFieldName.ToString(),
                TemplateFieldValueFactory.Create(TemplateFieldName.Image).Get(Parameters));
        }

        protected ImageContent BuildQrCode(TemplateFieldName templateFieldName)
        {
            return new ImageContent(templateFieldName.ToString(),
                TemplateFieldValueFactory.Create(TemplateFieldName.QrCode).Get(Parameters));
        }
        

        #endregion
    }
}