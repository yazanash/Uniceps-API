using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using models = Uniceps.Entityframework.Models;
namespace Uniceps.Entityframework.Commands.ExerciseCommands
{
    public record DeleteExerciseCommand(int id) : IRequest<bool>;
}
