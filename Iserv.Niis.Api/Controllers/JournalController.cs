using Iserv.Niis.BusinessLogic.ProtectionDocs;
using Iserv.Niis.BusinessLogic.Contracts;
using Iserv.Niis.BusinessLogic.Requests;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.Infrastructure.Extensions.Filter;
using Iserv.Niis.Infrastructure.Pagination;
using Iserv.Niis.Model.Models.Journal;
using Iserv.Niis.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Exceptions;
using Iserv.Niis.FileConverter.Abstract;

namespace Iserv.Niis.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    //[Authorize(Policy = KeyFor.Policy.HasAccessToJournal)]
    public class JournalController : Controller
    {
        private readonly IExecutor _executor;
        private readonly IFileStorage _fileStorage;
        private readonly NiisWebContext _context;
        private readonly IFileConverter _fileConverter;

        public JournalController(IExecutor executor, IFileStorage fileStorage, NiisWebContext context, IFileConverter fileConverter)
        {
            _executor = executor;
            _fileStorage = fileStorage;
            _context = context;
            _fileConverter = fileConverter;
        }

        [HttpGet("getAttachment/{ownerType}/{ownerId}")]
        public async Task<IActionResult> Get(Owner.Type ownerType, int ownerId)
        {
            Attachment attachment = null;

            switch (ownerType)
            {
                case Owner.Type.Request:
                    var request = await _executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(ownerId));
                    attachment = request?.MainAttachment;
                    break;
                case Owner.Type.Contract:
                    var contract = await _executor.GetQuery<GetContractByIdQuery>().Process(q => q.ExecuteAsync(ownerId));
                    attachment = contract?.MainAttachment;
                    break;
                case Owner.Type.ProtectionDoc:
                    var protectionDoc = await _executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.ExecuteAsync(ownerId));
                    attachment = protectionDoc?.MainAttachment;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ownerType), ownerType, null);
            }

            if (attachment == null)
            {
                var documentName = ownerType == Owner.Type.Request ? nameof(Request) : nameof(Document);
                throw new DataNotFoundException(documentName, DataNotFoundException.OperationType.Read,
                    ownerId);
            }

            var fileContent = await _fileStorage.GetAsync(attachment.BucketName, attachment.OriginalName);

            var extention = Path.GetExtension(attachment.OriginalName);
            if (extention != null && extention.ToLower().Contains("odt"))
            {
                using (var memoryStream = new MemoryStream())
                {
                    memoryStream.Write(fileContent, 0, fileContent.Length);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    var contentType = attachment.ContentType;

                    var newName = attachment.ValidName.Replace(".odt", ".pdf");
                    var pdf = _fileConverter.DocxToPdf(memoryStream, newName);
                    return File(pdf.File, contentType, newName);
                }
            }

            return File(fileContent, attachment.ContentType, attachment.ValidName);
        }

        //[Authorize(Policy = KeyFor.Policy.HasAccessToViewStaffTasks)]
        [HttpGet("staff")]
        public async Task<IActionResult> Staff()
        {
            //TODO: заменить когда будут данные
            var mockData = Enumerable.Range(0, 200)
                .Select(x => new StaffTaskDto()
                {
                    Id = x,
                    FullName = "user " + x,
                    Incoming = new Random(x).Next(5, 50),
                    Executed = new Random(x + 2).Next(5, 50),
                    OnJob = new Random(x + 3).Next(5, 50),
                    NotOnJob = new Random(x + 4).Next(5, 50),
                    Overdue = new Random(x + 5).Next(5, 50),
                    Outgoing = new Random(x + 6).Next(5, 50),
                }).AsQueryable();

            return mockData
                .Filter(Request.Query)
                .Sort(Request.Query)
                .ToPagedList(Request.GetPaginationParams())
                .AsOkObjectResult(Response);
        }

        [HttpGet("userstaskscounts")]
        public IActionResult GetUsersTasksCounts()
        {
            List<int> userIds = new List<int>();
            DateTimeOffset now = DateTimeOffset.Now;
            DateTimeOffset? fromDate = null;
            DateTimeOffset? toDate = null;

            if (Request.Query.ContainsKey("users"))
            {
                if (Request.Query.TryGetValue("users", out var usersValue))
                {
                    var usersValueStr = usersValue.ToString();

                    if (!string.IsNullOrWhiteSpace(usersValueStr))
                    {
                        userIds = usersValueStr.Split(',').Select(id => Convert.ToInt32(id)).ToList();
                    }
                }
            }

            if (Request.Query.ContainsKey("fromDate"))
            {
                if (Request.Query.TryGetValue("fromDate", out var fromDateValue))
                {
                    var fromDateValueStr = fromDateValue.First();

                    if (!string.IsNullOrWhiteSpace(fromDateValueStr))
                    {
                        if (DateTimeOffset.TryParse(fromDateValueStr, out DateTimeOffset outFromDate))
                        {
                            fromDate = outFromDate;
                        }
                    }
                }
            }

            if (Request.Query.ContainsKey("toDate"))
            {
                if (Request.Query.TryGetValue("toDate", out var toDateValue))
                {
                    var toDateValueStr = toDateValue.First();

                    if (!string.IsNullOrWhiteSpace(toDateValueStr))
                    {
                        if (DateTimeOffset.TryParse(toDateValueStr, out DateTimeOffset outToDate))
                        {
                            toDate = outToDate;
                        }
                    }
                }
            }

            var usersTvpBuilder = new TableValuedParameterBuilder("UserTableType", new SqlMetaData("Id", SqlDbType.Int));
            foreach (var userId in userIds)
            {
                usersTvpBuilder.AddRow(userId);
            }

            var usersParam = usersTvpBuilder.CreateParameter("Users");

            var fromDateParam = new SqlParameter()
            {
                ParameterName = "FromDate",
                SqlDbType = SqlDbType.DateTimeOffset,
                Direction = ParameterDirection.Input,
                IsNullable = true
            };

            if (fromDate != null)
            {
                fromDateParam.Value = fromDate.Value;
            }
            else
            {
                fromDateParam.Value = DBNull.Value;
            }

            var toDateParam = new SqlParameter()
            {
                ParameterName = "ToDate",
                SqlDbType = SqlDbType.DateTimeOffset,
                Direction = ParameterDirection.Input,
                IsNullable = true
            };

            if (toDate != null)
            {
                toDateParam.Value = toDate.Value;
            }
            else
            {
                toDateParam.Value = DBNull.Value;
            }

            var nowParam = new SqlParameter()
            {
                ParameterName = "Now",
                SqlDbType = SqlDbType.DateTimeOffset,
                Direction = ParameterDirection.Input,
                IsNullable = false,
                Value = now
            };

            var countRows = _context.UsersTasksCountsEntities
                .FromSql("usp_GetUsersTasksCounts @Users, @FromDate, @ToDate, @Now", usersParam, fromDateParam, toDateParam, nowParam)
                .ToList();

            var countDtos = countRows.Select(r => new UsersTasksCountsDto
            {
                Id = r.Id,
                ActiveTasks = r.ActiveRequests + r.ActiveProtectionDocs + r.ActiveContracts,
                CompletedTasks = r.CompletedRequests + r.CompletedProtectionDocs + r.CompletedContracts,
                ExpiredTasks = r.ExpiredRequests + r.ExpiredProtectionDocs + r.ExpiredContracts,
                Documents = r.Documents,
                CompletedDocuments = r.CompletedDocuments,
                ExpiredDocuments = r.ExpiredDocuments
            });

            return Ok(countDtos);
        }
    }
}