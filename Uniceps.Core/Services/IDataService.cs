using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Core.Services
{
    public interface IDataService<T> where T : class
    {
        public Task<T> Create(T entity);
        public Task<T> Update(T entity);
        public Task<bool> Delete(Guid id);
        public Task<IEnumerable<T>> GetAll();
        public Task<T> Get(Guid id);
    }
}
