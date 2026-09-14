using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.Models;

namespace Uniceps.Entityframework.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OwnedBy<T>(this IQueryable<T> query, string userId) where T : IOwnable
        {
            return query.Where(e => e.UserId == userId);
        }
    }
}
