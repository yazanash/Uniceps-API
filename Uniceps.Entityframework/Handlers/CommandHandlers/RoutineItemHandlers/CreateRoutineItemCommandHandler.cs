using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Commands.RoutineItemCommands;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.Entityframework.Handlers.CommandHandlers.RoutineItemHandlers
{
    public class CreateRoutineItemCommandHandler : IRequestHandler<CreateRoutineItemCommand, RoutineItem>
    {
        private readonly ICommandDataService<RoutineItem> _service;

        public CreateRoutineItemCommandHandler(ICommandDataService<RoutineItem> service)
        {
            _service = service;
        }

        public async Task<RoutineItem> Handle(CreateRoutineItemCommand request, CancellationToken cancellationToken)
        {
           return await _service.Create(request.routineItem);
        }
    }
}
