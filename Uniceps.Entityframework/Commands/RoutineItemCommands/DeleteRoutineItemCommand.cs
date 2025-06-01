using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Commands.RoutineItemCommands
{
    public record DeleteRoutineItemCommand(int id):IRequest<bool>;
    
}
