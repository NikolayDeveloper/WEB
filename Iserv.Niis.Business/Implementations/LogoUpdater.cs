using System;
using System.Drawing;
using ImageMagick;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Domain.Abstract;
using Microsoft.AspNetCore.Http;

namespace Iserv.Niis.Business.Implementations
{
    public class LogoUpdater : ILogoUpdater
    {
        public void Update(IHaveImageAttachment entity, IFormFile file, int maxSize = 500)
        {
            if (file == null) return;

            using (MagickImage image = new MagickImage(file.OpenReadStream()))
            {
                MagickGeometry size = new MagickGeometry(maxSize, maxSize) { IgnoreAspectRatio = false };

                image.BackgroundColor = new MagickColor(Color.White);
                image.Format = MagickFormat.Png;
                image.Resize(size);

                entity.Image = image.ToByteArray();

                MagickGeometry previewSize = new MagickGeometry(100, 100) { IgnoreAspectRatio = false };
                image.Resize(previewSize);
                entity.PreviewImage = image.ToByteArray();
            }
        }

        public void Update(IHaveImageAttachment entity, int maxWidth = 500, int fontSize = 32)
        {
            if (!entity.IsImageFromName) return;

            var logoText = string.Join(Environment.NewLine, entity.NameRu, entity.NameKz, entity.NameEn);

            using (MagickImage image = new MagickImage(new MagickColor(Color.White), maxWidth, maxWidth))
            {
                image.Settings.FontPointsize = fontSize;
                image.Settings.FillColor = new MagickColor(Color.Black);
                image.Settings.TextGravity = Gravity.Center;
                image.Read("label:" + logoText);
                image.Format = MagickFormat.Png;

                entity.Image = image.ToByteArray();

                MagickGeometry previewSize = new MagickGeometry(100, 100) { IgnoreAspectRatio = false };
                image.Resize(previewSize);
                entity.PreviewImage = image.ToByteArray();
            }
        }
    }
}
