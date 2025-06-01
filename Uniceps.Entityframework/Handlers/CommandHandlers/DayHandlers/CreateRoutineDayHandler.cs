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
    public class CreateRoutineDayHandler:IRequestHandler<CreateRoutineDayCommand,Day>
    {
        private readonly ICommandDataService<Day> _service;

        public CreateRoutineDayHandler(ICommandDataService<Day> service)
        {
            _service = service;
        }

        public async Task<Day> Handle(CreateRoutineDayCommand request, CancellationToken cancellationToken)
        {
            return await _service.Create(request.day);
        }
    }
}
