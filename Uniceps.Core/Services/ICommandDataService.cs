using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Core.Services
{
    public interface ICommandDataService<T> where T : class
    {
        public Task<T> Create(T entity);
        public Task<T> Update(T entity);
        public Task<bool> Delete(int id);
    }
}
