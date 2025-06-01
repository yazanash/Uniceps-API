using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.Entityframework.Commands.RoutineItemCommands
{
    public record CreateRoutineItemCommand(RoutineItem routineItem) : IRequest<RoutineItem>;
}
