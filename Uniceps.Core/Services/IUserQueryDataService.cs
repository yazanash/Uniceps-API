using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Uniceps.Core.Services
{
    public interface IUserQueryDataService<T>where T : class
    {
        public Task<IEnumerable<T>> GetAllByUser(string? userid );
    }
}
