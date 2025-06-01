using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.Entityframework.Commands.DayCommands
{
    public record UpdateRoutineDayCommand(Day day) : IRequest<Day>;
}
