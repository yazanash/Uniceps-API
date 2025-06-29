using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Core.Services
{
    public interface IEntityQueryDataService<T> where T : class
    {
        public Task<IEnumerable<T>> GetAllById(Guid entityId);
    }
    public interface IIntEntityQueryDataService<T> where T : class
    {
        public Task<IEnumerable<T>> GetAllById(int entityId);
    }
}
