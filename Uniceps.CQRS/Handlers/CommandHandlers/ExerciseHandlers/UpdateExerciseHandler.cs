using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Commands.ExerciseCommands;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.Entityframework.Handlers.CommandHandlers.ExerciseHandlers
{
    public class UpdateExerciseHandler : IRequestHandler<UpdateExerciseCommand, Exercise>
    {
        private ICommandDataService<Exercise> _service;
        public UpdateExerciseHandler(ICommandDataService<Exercise> service)
        {
            _service = service;
        }
        public async Task<Exercise> Handle(UpdateExerciseCommand request, CancellationToken cancellationToken)
        {
            await _service.Update(request.exercise);
            return request.exercise;
        }
    }
}
