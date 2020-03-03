using System;
using System.IO;
using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic.Requests;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.Infrastructure;
using Iserv.Niis.Utils.Abstractions;
using Iserv.Niis.Utils.Constans;
using Microsoft.AspNetCore.Http;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Attachments
{
    public class UploadMediaFileHandler: BaseHandler
    {
        private readonly IFileStorage _fileStorage;
        //private readonly IGenerateHash _generateHash;

        public UploadMediaFileHandler(IFileStorage fileStorage)//, IGenerateHash generateHash)
        {
            _fileStorage = fileStorage;
            //_generateHash = generateHash;
        }

        /// <summary>
        /// Загрузка медиа-файла на фауловое хранилище и прикрепление его к заявке
        /// </summary>
        /// <param name="requestId">Уникальный идентификатор заявки</param>
        /// <param name="file">Прикрепляемый файл из запроса</param>
        /// <returns></returns>
        public async Task ExecuteAsync(int requestId, IFormFile file)
        {
            var request = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(requestId));
            //Название папки на файловом хранилище minio
            var bucketName = $"requests";
            //var bucketName = $"request-{requestId}-media-{DateTimeOffset.Now:dd.MM.yyyy}";
            
            //var fileName = $"current/{requestId}/{file.FileName}";
            byte[] fileBytes;

            var extention = Path.GetExtension(file.FileName);
            var newFileName = $"current/{requestId}/{Guid.NewGuid().ToString()}{extention}";


            //Чтение файла, запись его содержимого в массив байтов
            using (var stream = file.OpenReadStream())
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            //Загрузка файла на minio
            await _fileStorage.AddAsync(bucketName, newFileName, fileBytes, file.ContentType);

            foreach (var mediaFile in request.MediaFiles)
            {
                mediaFile.IsDeleted = true;
                mediaFile.DeletedDate = DateTimeOffset.Now;
                await Executor.GetCommand<UpdateAttachmentCommand>().Process(c => c.ExecuteAsync(mediaFile));
            }

            //Создание сущности-прикрепления в базе
            var mediaAttachment = new Attachment
            {
                BucketName = bucketName,
                ContentType = file.ContentType,
                AuthorId = NiisAmbientContext.Current.User.Identity.UserId,
                DateCreate = DateTimeOffset.Now,
                DateUpdate = DateTimeOffset.Now,
                OriginalName = newFileName,
                Length = file.Length,
                ValidName = file.FileName.MakeValidFileName(),
                //Hash = _generateHash.GenerateFileHash(fileBytes),
                RequestId = requestId
            };
            await Executor.GetCommand<CreateAttachmentCommand>().Process(c => c.ExecuteAsync(mediaAttachment));
        }
    }
}
