using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.RoutineModels;
using Uniceps.Entityframework.Queries.RoutineQueries;

namespace Uniceps.Entityframework.Handlers.QueriesHandlers.RoutineQueries
{
    public class GetRoutineItemQueryHandler : IRequestHandler<GetRoutineItemsQuery, IEnumerable<RoutineItem>>
    {
        private readonly IEntityQueryDataService<RoutineItem> _service;

        public GetRoutineItemQueryHandler(IEntityQueryDataService<RoutineItem> service)
        {
            _service = service;
        }

        public async Task<IEnumerable<RoutineItem>> Handle(GetRoutineItemsQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetAllById(request.id);
        }
    }
}
