using AutoMapper;
using Iserv.Niis.BusinessLogic.AutoRouteStages;
using Iserv.Niis.BusinessLogic.Dictionaries.DicDocumentStatusQuery;
using Iserv.Niis.BusinessLogic.Documents;
using Iserv.Niis.BusinessLogic.Documents.Numbers;
using Iserv.Niis.BusinessLogic.Workflows.Documents;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.Services.Interfaces;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Iserv.Niis.Services.Implementations
{
    /// <summary>
    /// Сервис, который отвечает за загрузку документов и их вложений.
    /// </summary>
    public class UploadService : IUploadService
    {
        private readonly IAutoRouteStageHelper _autoRouteStageHelper;
        private readonly IExecutor _executor;
        private readonly IMapper _mapper;

        public UploadService(IAutoRouteStageHelper autoRouteStageHelper, IExecutor executor, IMapper mapper)
        {
            _autoRouteStageHelper = autoRouteStageHelper ?? throw new ArgumentNullException(nameof(autoRouteStageHelper));
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Кладет вложения в материалы.
        /// </summary>
        /// <param name="materials">Материалы с вложениями.</param>
        /// <returns>Материалы.</returns>
        public async Task<List<MaterialDetailDto>> AddAttachmentsToMaterials(IEnumerable<MaterialDetailDto> materials)
        {
            List<MaterialDetailDto> responseMaterials = new List<MaterialDetailDto>();

            foreach(MaterialDetailDto material in materials)
            {
                Document documentWithAttachment = await _executor
                    .GetHandler<AddMainAttachToDocumentHandler>()
                    .Process(handler => handler.Execute(material));

                MaterialDetailDto materialWithAttachment = _mapper.Map<MaterialDetailDto>(documentWithAttachment);

                //IList<MaterialWorkflowDto> materialWorkflows = await _autoRouteStageHelper.StartAutuRouteStage(documentWithAttachment.Id);

                //if (materialWorkflows != null)
                //{
                //    materialWithAttachment.WorkflowDtos = materialWorkflows.ToArray();
                //}

                responseMaterials.Add(materialWithAttachment);
            }

            return responseMaterials;
        }

        /// <summary>
        /// Создает дочерние документы и прикрепляет их к родительскому документу.
        /// <para></para>
        /// Дочерние документы добавляются в материлы заявок родительского документа.
        /// </summary>
        /// <param name="attachDto">Модель для прикрепления дочерних документов к родительскому документов.</param>
        /// <returns>Асинхронная операция.</returns>
        public async Task CreateMaterialsAndAttachThemToParent(AttachMaterialDto attachDto)
        {
            await AddAttachmentsToMaterials(new[] { attachDto.Parent });

            IEnumerable<MaterialDetailDto> childMaterials = attachDto.Children;

            foreach (MaterialDetailDto childMaterial in childMaterials)
            {
                childMaterial.Id = await CreateDocumentAndGetId(childMaterial);
            }
            
            await AddAttachmentsToMaterials(childMaterials);

            List<int> childMaterialIds = childMaterials
                .Select(material => material.Id.Value)
                .ToList();

            await _executor
                .GetHandler<LinkDocumentWithParentRequestsHandler>()
                .Process(handler => handler.ExecuteAsync(attachDto.Parent.Id ?? 0, childMaterialIds));
        }

        #region Private methods

        /// <summary>
        /// Создает документ и возвращает его идентификатор.
        /// </summary>
        /// <param name="material">Материал.</param>
        /// <returns>Идентификатор созданного документа.</returns>
        private async Task<int> CreateDocumentAndGetId(MaterialDetailDto material)
        {
            Document newDocument = _executor
                .GetHandler<CreateDocumentFromUploadedFileHandler>()
                .Process(handler => handler.Execute(material));

            DicDocumentStatus workStatus = _executor
                .GetQuery<GetDocumentStatusByCodeQuery>()
                .Process(query => query.Execute(DicDocumentStatusCodes.InWork));

            newDocument.StatusId = workStatus.Id;

            int documentId = await _executor
                .GetCommand<CreateDocumentCommand>()
                .Process(command => command.ExecuteAsync(newDocument));

            DocumentWorkflow initialWorkflow = await _executor
                .GetQuery<GetInitialDocumentWorkflowQuery>()
                .Process(query => query.ExecuteAsync(documentId, NiisAmbientContext.Current.User.Identity.UserId));

            await _executor
                .GetCommand<ApplyDocumentWorkflowCommand>()
                .Process(command => command.ExecuteAsync(initialWorkflow));

            if (newDocument.DocumentType == DocumentType.Incoming)
            {
                await _executor
                    .GetHandler<GenerateDocumentIncomingNumberHandler>()
                    .Process(c => c.ExecuteAsync(documentId));
            }             

            return documentId;
        }  

        #endregion
    }
}
