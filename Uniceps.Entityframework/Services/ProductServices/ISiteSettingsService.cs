using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.Models;

namespace Uniceps.Entityframework.Services.ProductServices
{
    public interface ISiteSettingsService
    {
        Task<SiteSettings> CreateOrUpdate(SiteSettings entity);
        Task<SiteSettings> Get();
    }
}
