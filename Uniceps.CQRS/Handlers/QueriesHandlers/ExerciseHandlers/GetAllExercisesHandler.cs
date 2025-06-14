using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.RoutineModels;
using Uniceps.Entityframework.Queries.ExerciseQueries;

namespace Uniceps.Entityframework.Handlers.QueriesHandlers.ExerciseHandlers
{
    public class GetAllExercisesHandler : IRequestHandler<GetAllExercisesQuery, IEnumerable<Exercise>>
    {
        private IEntityQueryDataService<Exercise> _service;
        public GetAllExercisesHandler(IEntityQueryDataService<Exercise> service)
        {
            _service = service;
        }
        public async Task<IEnumerable<Exercise>> Handle(GetAllExercisesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Exercise> exercises = await _service.GetAllById(request.id);
            return exercises;
        }
    }
}
