using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.Products;

namespace Uniceps.Entityframework.Services.ProductServices
{
    public class ReleaseDataService(AppDbContext dbContext) : IReleaseDataService
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<Release> AddReleaseAsync(Release release)
        {
            EntityEntry<Release> CreatedResult = await _dbContext.Set<Release>().AddAsync(release);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }
        public async Task<bool> DeleteReleaseAsync(int id)
        {
            Release? entity = await _dbContext.Set<Release>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            _dbContext.Set<Release>().Remove(entity!);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<Release?> GetLatestReleaseAsync(int productId, TargetOS os)
        {
            return await _dbContext.Releases
             .Where(r => r.ProductId == productId && r.TargetOS == os)
             .OrderByDescending(r => r.CreatedAt)
             .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Release>> GetLatestReleasesAsync(int productId)
        {
            return await _dbContext.Set<Release>()
          .Where(r => r.ProductId == productId)
          .GroupBy(r => r.TargetOS)
          .Select(g => g.OrderByDescending(r => r.CreatedAt).First())
          .ToListAsync();
        }

        public async Task<Release?> GetReleaseByIdAsync(int id)
        {
            Release? entity = await _dbContext.Set<Release>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<Release>> GetReleasesByProductAsync(int productId)
        {
            return await _dbContext.Set<Release>()
            .Where(r => r.ProductId == productId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(); throw new NotImplementedException();
        }

        public async Task LogDownloadAsync(int releaseId, string? ipAddress, string? userAgent)
        {
            var log = new DownloadLog
            {
                ReleaseId = releaseId,
                IPAddress = ipAddress ?? "Unknown",
                UserAgent = userAgent ?? "Unknown",
                DownloadedAt = DateTime.UtcNow
            };

            _dbContext.Set<DownloadLog>().Add(log);
            await _dbContext.SaveChangesAsync();
        }


        public async Task<Release> UpdateReleaseAsync(Release release)
        {
            _dbContext.Set<Release>().Update(release);
            await _dbContext.SaveChangesAsync();
            return release;
        }
    }
}
