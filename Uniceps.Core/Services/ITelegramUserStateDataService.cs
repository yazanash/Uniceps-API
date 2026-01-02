using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Core.Services
{
    public interface ITelegramUserStateDataService<T>
    {
        public Task<T> GetOrCreateAsync(long chatId);
        public Task<T> UpdateAsync(T entity);
    }
}
