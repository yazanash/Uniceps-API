using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.Entityframework.Queries.RoutineQueries
{
    public record GetRoutineItemsQuery(int id):IRequest<IEnumerable<RoutineItem>>;
}
