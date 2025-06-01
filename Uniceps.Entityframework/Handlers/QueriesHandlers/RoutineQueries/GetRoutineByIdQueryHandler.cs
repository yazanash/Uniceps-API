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
    public class GetRoutineByIdQueryHandler : IRequestHandler<GetRoutineByIdQuery, Routine>
    {
        private readonly IQueryDataService<Routine> _service;

        public GetRoutineByIdQueryHandler(IQueryDataService<Routine> service)
        {
            _service = service;
        }

        public async Task<Routine> Handle(GetRoutineByIdQuery request, CancellationToken cancellationToken)
        {
            Routine routine = await _service.Get(request.id);
            return routine;
        }
    }
}
