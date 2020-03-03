using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Administration.ApplicationRole
{
    public class GetRouteStagesTree
    {
        public class Query : IRequest<object>
        {
        }

        public class QueryValidator : AbstractValidator<Query>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, object>
        {
            private readonly NiisWebContext _context;

            public QueryHandler(NiisWebContext context)
            {
                _context = context;
            }

            public async Task<object> Handle(Query message)
            {
                // TODO: после релиза Material Tree изменить формирование дерева. Перенести в универсальный метод построения дерева на UI который рекомендовал Данияр

                var docTypes = await _context.DicProtectionDocTypes.ToListAsync();

                var protectionDocTypes = await _context.DicRoutes
                    .Select(r =>
                        new TreeNode
                        {
                            Data = 0,
                            Label = r.NameRu + (docTypes.Any(dt => dt.RouteId == r.Id)
                                        ? $" ({string.Join(", ", docTypes.Where(dt => dt.RouteId == r.Id).Select(t => t.NameRu))})"
                                        : string.Empty),
                            Selectable = true,
                            Children = r.RouteStages.Select(rs => new TreeNode
                            {
                                Data = rs.Id,
                                Label = rs.NameRu,
                                Selectable = true
                            })
                        })
                    .OrderBy(t => t.Label)
                    .ToListAsync();

                return protectionDocTypes;
            }

            class TreeNode
            {
                public object Data { get; set; }

                public string Label { get; set; }

                public bool Selectable { get; set; }

                public IEnumerable<TreeNode> Children { get; set; }
            }
        }
    }
}