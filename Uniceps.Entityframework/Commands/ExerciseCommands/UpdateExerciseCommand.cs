using MediatR;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.Entityframework.Commands.ExerciseCommands
{
    public record UpdateExerciseCommand(Exercise exercise) : IRequest<Exercise>;
}
