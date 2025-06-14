using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Commands.DayCommands
{
    public record DeleteRoutineDayCommand(int id) : IRequest<bool>;
}
