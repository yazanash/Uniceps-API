using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.Models.Profile;

namespace Uniceps.Entityframework.Services.ProfileServices
{
    public interface IProfileDataService
    {
        Task<NormalProfile> Create(NormalProfile entity);
        Task<bool> Delete(Guid id);
        Task<NormalProfile> Update(NormalProfile entity);
        Task<NormalProfile> Get(Guid id);
        Task<NormalProfile> GetByUserId(string userid);
    }
}
