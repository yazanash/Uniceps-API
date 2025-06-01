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
    public class DeleteRoutineItemCommandHandler : IRequestHandler<DeleteRoutineItemCommand, bool>
    {
        private readonly ICommandDataService<RoutineItem> _service;

        public DeleteRoutineItemCommandHandler(ICommandDataService<RoutineItem> service)
        {
            _service = service;
        }

        public Task<bool> Handle(DeleteRoutineItemCommand request, CancellationToken cancellationToken)
        {
            return _service.Delete(request.id);
        }
    }
}
