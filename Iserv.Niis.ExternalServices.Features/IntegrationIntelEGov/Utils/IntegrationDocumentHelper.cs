using System;
using System.Linq;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.System;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Exceptions;
using Iserv.Niis.ExternalServices.Features.Models;
using Iserv.Niis.ExternalServices.Features.Utils;

namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Utils
{
    /// <summary>
    /// Создание материала
    /// </summary>
    public class IntegrationDocumentHelper
    {
        private readonly NiisWebContext _niisContext;
        private readonly INumberGenerator _numberGenerator;
        private readonly AppConfiguration _configuration;
        private readonly IntegrationDictionaryHelper _integrationDictionaryHelper;

        public IntegrationDocumentHelper(
            NiisWebContext niisContext, 
            INumberGenerator numberGenerator, 
            IntegrationDictionaryHelper integrationDictionaryHelper, 
            AppConfiguration configuration)
        {
            _niisContext = niisContext;
            _numberGenerator = numberGenerator;
            _integrationDictionaryHelper = integrationDictionaryHelper;
            _configuration = configuration;
        }

        /// <summary>
        /// Создание материала
        /// </summary>
        /// <param name="document">Объект</param>
        public void CreateDocument(Document document)
        {

            var user = _niisContext.Users.FirstOrDefault(d => d.Id == _configuration.AuthorAttachmentDocumentId);
            document.DepartmentId = user?.DepartmentId;
            document.DivisionId = user?.Department?.DivisionId;

            if (document.DocumentType == DocumentType.Incoming)
                GenerateDocumentIncomingNumber(document);

            _numberGenerator.GenerateBarcode(document);
            _numberGenerator.GenerateNumForRegisters(document);
            _niisContext.Documents.Add(document);
            _niisContext.SaveChanges();

            var routeId = _integrationDictionaryHelper.GetRouteIdDocumentTypeId(document.TypeId);
            if (!routeId.HasValue)
                throw new NotSupportedException("Необработанный тип объекта");
            var stage = _integrationDictionaryHelper.GetRouteStage(routeId.Value);

            var documentWorkflow = new DocumentWorkflow
            {
                OwnerId = document.Id,
                DateCreate = DateTimeOffset.Now,
                RouteId = routeId,
                CurrentStageId = stage.Id,
                CurrentUserId = _configuration.AuthorAttachmentDocumentId,
                IsComplete = stage.IsLast,
                IsCurent = true,
                IsSystem = stage.IsSystem,
                IsMain = stage.IsMain,
                DateUpdate = DateTimeOffset.Now
            };
            _niisContext.DocumentWorkflows.Add(documentWorkflow);
            _niisContext.SaveChanges();
        }

        /// <summary>
        /// Генерация входящего номера
        /// </summary>
        /// <param name="document">Материал</param>
        public void GenerateDocumentIncomingNumber(Document document)
        {
            if (!string.IsNullOrEmpty(document.IncomingNumber))
                return;

            var documentType = _niisContext.DicDocumentTypes.FirstOrDefault(d => d.Id == document.TypeId);
            if (DicDocumentTypeCodes.IgnoreGenerateIncomingNumber().Contains(documentType?.Code))
                return;

            if (document.DocumentType != DocumentType.Incoming)
                throw new Exception("Can not generate incoming number for not incoming document!");

            var commonDivision = GetDicDivisionByCode(DicDivisionCodes.RGP_NIIS);
            var division = GetDicDivisionById(document.DivisionId ?? 0);
            if (division == null)
                division = commonDivision;

            var count = GetNextCount(NumberGenerator.DocumentIncomingNumberCodePrefix + division.Code);
            document.IncomingNumber = $"{DateTime.Now.Year}-{count:D5}";
        }

        /// <summary>
        /// Получение следующего каунтера
        /// </summary>
        /// <param name="code">Ключ счетчика</param>
        /// <returns>Следующее значение</returns>
        public int GetNextCount(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            var currentCounter = GetSystemCounterByCodey(code);

            if (currentCounter != null)
            {
                currentCounter.Count = ++currentCounter.Count;
                UpdateSystemCounter(currentCounter);
            }
            else
            {
                currentCounter = new SystemCounter { Code = code, Count = 1 };
                CreateSystemCounter(currentCounter);
            }

            return currentCounter.Count;
        }

        /// <summary>
        /// Получение подразделения по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Подразделение</returns>
        public DicDivision GetDicDivisionById(int id)
        {
            var division = _niisContext.DicDivisions.FirstOrDefault(d => d.Id == id);

            return division;
        }

        /// <summary>
        /// Получение подразделения по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Подразделение</returns>
        public DicDivision GetDicDivisionByCode(string code)
        {
            var division = _niisContext.DicDivisions.FirstOrDefault(d => d.Code == code);

            return division;
        }

        /// <summary>
        /// Получить счетчик
        /// </summary>
        /// <param name="code">Ключ</param>
        /// <returns>Счетчик</returns>
        public SystemCounter GetSystemCounterByCodey(string code)
        {
            //Код счетчика должен быть уникальным
            return _niisContext.SystemCounter.AsQueryable().FirstOrDefault(c => c.Code == code);
        }

        /// <summary>
        /// Обновить счетчик
        /// </summary>
        /// <param name="counter">Счетчик</param>
        public void UpdateSystemCounter(SystemCounter counter)
        {
            _niisContext.SystemCounter.Update(counter);
            _niisContext.SaveChanges();
        }

        /// <summary>
        /// Создать счетчик
        /// </summary>
        /// <param name="counter">Счетчик</param>
        public void CreateSystemCounter(SystemCounter counter)
        {
            _niisContext.SystemCounter.Add(counter);

            _niisContext.SaveChanges();
        }
    }
}