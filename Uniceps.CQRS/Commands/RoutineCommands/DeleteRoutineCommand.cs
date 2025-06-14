using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Commands.RoutineCommands
{
    public record DeleteRoutineCommand(int id) : IRequest<bool>;
}
