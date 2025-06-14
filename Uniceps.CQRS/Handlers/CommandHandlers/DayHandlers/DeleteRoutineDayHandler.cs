using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Commands.DayCommands;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.Entityframework.Handlers.CommandHandlers.DayHandlers
{
    public class DeleteRoutineDayHandler : IRequestHandler<DeleteRoutineDayCommand, bool>
    {
        private readonly ICommandDataService<Day> _service;

        public DeleteRoutineDayHandler(ICommandDataService<Day> service)
        {
            _service = service;
        }

        public Task<bool> Handle(DeleteRoutineDayCommand request, CancellationToken cancellationToken)
        {
            return _service.Delete(request.id);
        }
    }
}
