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
    public class DeleteRoutineCommandHandler : IRequestHandler<DeleteRoutineCommand, bool>
    {
        private ICommandDataService<Routine> _service;
        public DeleteRoutineCommandHandler(ICommandDataService<Routine> service)
        {
            _service = service;
        }

        public async Task<bool> Handle(DeleteRoutineCommand request, CancellationToken cancellationToken)
        {
            bool result = await _service.Delete(request.id);
            return result;
        }
    }
}
