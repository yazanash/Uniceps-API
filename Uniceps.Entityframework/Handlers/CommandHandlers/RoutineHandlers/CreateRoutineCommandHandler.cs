using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Commands.ExerciseCommands;
using Uniceps.Entityframework.Commands.RoutineCommands;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.Entityframework.Handlers.CommandHandlers.RoutineHandlers
{
    public class CreateRoutineCommandHandler : IRequestHandler<CreateRoutineCommand, Routine>
    {
        private ICommandDataService<Routine> _service;
        public CreateRoutineCommandHandler(ICommandDataService<Routine> service)
        {
            _service = service;
        }
        public async Task<Routine> Handle(CreateRoutineCommand request, CancellationToken cancellationToken)
        {
           Routine routine= await _service.Create(request.routine);
            return routine;
        }
    }
}
