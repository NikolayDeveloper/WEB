using System.Linq;
using AutoMapper;
using Iserv.Niis.BusinessLogic.Common;
using Iserv.Niis.BusinessLogic.Dictionaries.DicDocumentType;
using Iserv.Niis.BusinessLogic.Dictionaries.DicReceiveTypes;
using Iserv.Niis.BusinessLogic.Security;
using Iserv.Niis.DI;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Documents
{
    /// <summary>
    /// Обработчик, который создает и возвращает документ из загруженного файла.
    /// </summary>
    public class CreateDocumentFromUploadedFileHandler : BaseHandler
    {
        private readonly IMapper _mapper;

        public CreateDocumentFromUploadedFileHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Выполнение обработчика.
        /// </summary>
        /// <param name="material">Загруженный файл.</param>
        /// <returns>Созданный документ.</returns>
        public Document Execute(MaterialDetailDto material)
        {
            var newDocument = _mapper.Map<MaterialDetailDto, Document>(material);

            Executor
                .GetHandler<GenerateBarcodeHandler>()
                .Process(handler => handler.Execute(newDocument));

            newDocument.ReceiveTypeId = Executor
                .GetQuery<GetReceiveTypeByCodeQuery>()
                .Process(query => query.Execute(DicReceiveType.Codes.Courier)).Id;

            var user = Executor
                .GetQuery<GetUserByIdQuery>()
                .Process(query => query.Execute(NiisAmbientContext.Current.User.Identity.UserId));

            newDocument.DepartmentId = user.DepartmentId;
            newDocument.DivisionId = user.Department.DivisionId;
            newDocument.DocumentType = Executor
                .GetQuery<GetDocumentTypeEnumByTypeIdQuery>()
                .Process(query => query.Execute(newDocument.TypeId));

            var firstRequestOwnerId = material.Owners?.FirstOrDefault()?.OwnerId;

            if (firstRequestOwnerId != null)
            {
                var request = Executor
                    .GetQuery<GetRequestByIdQuery>()
                    .Process(q => q.Execute(firstRequestOwnerId.Value));

                newDocument.AddresseeId = request.AddresseeId;
            }

            return newDocument;
        }
    }
}