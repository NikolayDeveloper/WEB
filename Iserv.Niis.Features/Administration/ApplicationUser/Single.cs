using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Model.Models.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Administration.ApplicationUser
{
    public class Single
    {
        public class Query : IRequest<UserDetailsDto>
        {
            public Query(int userId)
            {
                UserId = userId;
            }
            public int UserId { get; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, UserDetailsDto>
        {
            private readonly IMapper _mapper;
            private readonly NiisWebContext _context;

            public QueryHandler(IMapper mapper, NiisWebContext context)
            {
                _mapper = mapper;
                _context = context;
            }

            async Task<UserDetailsDto> IAsyncRequestHandler<Query, UserDetailsDto>.Handle(Query message)
            {
                var user = await _context.Users
                    .Include(u => u.Department)
                        .ThenInclude(d=> d.Division)
                    .Include(u => u.Icgss)
                    .SingleOrDefaultAsync(r => r.Id == message.UserId);
                
                if (user == null)
                    throw new DataNotFoundException(nameof(Domain.Entities.Request.Request),
                        DataNotFoundException.OperationType.Read, message.UserId);

                return _mapper.Map<Domain.Entities.Security.ApplicationUser, UserDetailsDto>(user,
                    options => options.Items["userRoles"] = _context.UserRoles);
            }
        }
    }
}
