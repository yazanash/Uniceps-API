using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Core.Services
{
    public interface IGetByTargetType<T>
    {
        public Task<IEnumerable<T>> GetAllByTarget(int target);
    }
}
