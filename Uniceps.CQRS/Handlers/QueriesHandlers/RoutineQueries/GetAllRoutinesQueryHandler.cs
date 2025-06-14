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
    public class GetAllRoutinesQueryHandler : IRequestHandler<GetAllRoutineQuery, IEnumerable<Routine>>
    {
        private IQueryDataService<Routine> _service;
        public GetAllRoutinesQueryHandler(IQueryDataService<Routine> service)
        {
            _service = service;
        }
        public async Task<IEnumerable<Routine>> Handle(GetAllRoutineQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Routine> routines = await _service.GetAll();
            return routines;
        }
    }
}
