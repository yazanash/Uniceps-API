using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.Models;

namespace Uniceps.CQRS.Queries
{
    public record GetAllGroupsQuery:IRequest<IEnumerable<MuscleGroup>>;
}
