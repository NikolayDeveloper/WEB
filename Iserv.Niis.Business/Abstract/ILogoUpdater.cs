using Iserv.Niis.Domain.Abstract;
using Microsoft.AspNetCore.Http;

namespace Iserv.Niis.Business.Abstract
{
    public interface ILogoUpdater
    {
        void Update(IHaveImageAttachment entity, IFormFile file, int maxSize = 500);
        void Update(IHaveImageAttachment entity, int maxWidth = 500, int fontSize = 32);
    }
}