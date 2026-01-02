using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.Products;

namespace Uniceps.Entityframework.Services.ProductServices
{
    public interface IReleaseDataService
    {
        Task<Release> AddReleaseAsync(Release release);
        Task<Release> UpdateReleaseAsync(Release release);
        Task<bool> DeleteReleaseAsync(int id);
        Task<IEnumerable<Release>> GetReleasesByProductAsync(int productId);
        Task<Release?> GetLatestReleaseAsync(int productId, TargetOS os);
        Task<IEnumerable<Release>> GetLatestReleasesAsync(int productId);
        Task<Release?> GetReleaseByIdAsync(int id);
        Task LogDownloadAsync(int releaseId, string? ipAddress, string? userAgent);
    }
}
