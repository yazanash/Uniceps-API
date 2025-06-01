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
    public class GetRoutineDaysQueryHandler : IRequestHandler<GetRoutineDaysQuery, IEnumerable<Day>>
    {
        private readonly IEntityQueryDataService<Day> _service;

        public GetRoutineDaysQueryHandler(IEntityQueryDataService<Day> service)
        {
            _service = service;
        }

        public async Task<IEnumerable<Day>> Handle(GetRoutineDaysQuery request, CancellationToken cancellationToken)
        {
           IEnumerable<Day> days = await _service.GetAllById(request.id);
            return days;
        }
    }
}
