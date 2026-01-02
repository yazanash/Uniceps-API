using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Core.Services
{
    public interface IOTPGenerateService<T>
    {
        Task<T> GenerateAsync(string email);
        Task<T?> VerifyAsync(string email,int otp);
    }
}
