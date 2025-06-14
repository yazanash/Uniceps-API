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
    public class AddExerciseHandler : IRequestHandler<AddExerciseCommand, Exercise>
    {
        private ICommandDataService<Exercise> _service;
        public AddExerciseHandler(ICommandDataService<Exercise> service)
        {
            _service = service;
        }
        public async Task<Exercise> Handle(AddExerciseCommand request, CancellationToken cancellationToken)
        {
            Exercise exercise=  await _service.Create(request.exercise);
            return exercise;
        }
    }
}
