using Iserv.Niis.Model.Models.Material;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Iserv.Niis.Services.Interfaces
{
    /// <summary>
    /// Сервис, который отвечает за загрузку документов и их вложений.
    /// </summary>
    public interface IUploadService
    {
        /// <summary>
        /// Кладет вложения в материалы.
        /// </summary>
        /// <param name="materials">Материалы с вложениями.</param>
        /// <returns>Материалы.</returns>
        Task<List<MaterialDetailDto>> AddAttachmentsToMaterials(IEnumerable<MaterialDetailDto> materials);

        /// <summary>
        /// Создает дочерние документы и прикрепляет их к родительскому документу.
        /// <para></para>
        /// Дочерние документы добавляются в материлы заявок родительского документа.
        /// </summary>
        /// <param name="attachDto">Модель для прикрепления дочерних документов к родительскому документов.</param>
        /// <returns>Асинхронная операция.</returns>
        Task CreateMaterialsAndAttachThemToParent(AttachMaterialDto attachDto);
    }
}
