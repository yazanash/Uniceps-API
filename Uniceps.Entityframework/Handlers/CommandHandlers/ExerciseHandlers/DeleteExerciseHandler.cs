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
    public class DeleteExerciseHandler : IRequestHandler<DeleteExerciseCommand, bool>
    {
        private ICommandDataService<Exercise> _service;
        public DeleteExerciseHandler(ICommandDataService<Exercise> service)
        {
            _service = service;
        }
        public async Task<bool> Handle(DeleteExerciseCommand request, CancellationToken cancellationToken)
        {
            bool result = await _service.Delete(request.id);
            return result;
        }
    }
}
