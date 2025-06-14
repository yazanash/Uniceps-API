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
    public class UpdateRoutineCommandHandler : IRequestHandler<UpdateRoutineCommand, Routine>
    {
        private ICommandDataService<Routine> _service;
        public UpdateRoutineCommandHandler(ICommandDataService<Routine> service)
        {
            _service = service;
        }

        public async Task<Routine> Handle(UpdateRoutineCommand request, CancellationToken cancellationToken)
        {
            await _service.Update(request.routine);
            return request.routine;
        }
    }
}
