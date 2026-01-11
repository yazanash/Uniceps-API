using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.AuthenticationModels;
using Uniceps.Entityframework.Models.Products;
using Uniceps.Entityframework.Models.StatsModels;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;

namespace Uniceps.Entityframework.Services
{
    public class StatsDataService(AppDbContext db) : IStatsDataService<DashboardStats>
    {
        private readonly AppDbContext _db = db;

        public async Task<DashboardStats> GetDashboardStatsAsync()
        {
            return new DashboardStats
            {
                UsersCount = await _db.Users.AsNoTracking().CountAsync(),

                ActiveUsers = await GetActiveUsers(),

                TotalDownloads = await _db.Set<DownloadLog>().AsNoTracking().CountAsync(),

                SubscriptionsByProduct = await GetSubscriptionsByProduct(),

                MonthlyNewUsers = await GetMonthlyNewUsers(),

                ActiveSubscriptions = await GetSubscriptionStats(),

                TrainingSessions = await GetTrainingSessions(),
                Revenue = await _db.Set<SystemSubscription>().AsNoTracking().Where(x=>x.ISPaid).SumAsync(x => x.Price),
                CashRequests = await _db.Set<CashPaymentRequest>().AsNoTracking().Where(x => x.Status == CashRequestStatus.Pending).CountAsync(),
                UnpaidSubscriptionCount = await _db.Set<SystemSubscription>().AsNoTracking().Where(x => !x.ISPaid).CountAsync(),
            };
        }
        private async Task<int> GetActiveUsers()
        {
            return await _db.Users
                .Where(s => s.LastLoginAt >= DateTime.Now.AddDays(-30))
                .CountAsync();
        }
        private async Task<List<MonthlyNewUsers>> GetMonthlyNewUsers()
        {
            return await _db.Users.AsNoTracking()
                .GroupBy(u => new { u.CreatedAt.Year, u.CreatedAt.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => new MonthlyNewUsers
                {
                    Month = $"{g.Key.Month}/{g.Key.Year}",
                    Users = g.Count()
                })
                .ToListAsync();
        }

        private async Task<List<ActiveSubscriptions>> GetSubscriptionStats()
        {
            return await _db.SystemSubscriptions.AsNoTracking()
                .Where(s => s.ISPaid && s.EndDate > DateTime.Now)
                .GroupBy(s => new { s.StartDate.Year, s.StartDate.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => new ActiveSubscriptions
                {
                    Month = $"{g.Key.Month}/{g.Key.Year}",
                    Active = g.Count()
                })
                .ToListAsync();
        }

        private async Task<List<TrainingSessions>> GetTrainingSessions()
        {
            return await _db.WorkoutSessions.AsNoTracking()
                .GroupBy(s => new { s.CreatedAt.Year, s.CreatedAt.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => new TrainingSessions
                {
                    Month = $"{g.Key.Month}/{g.Key.Year}",
                    Sessions = g.Count()
                })
                .ToListAsync();
        }
        private async Task<List<ProductSubscriptionStats>> GetSubscriptionsByProduct()
        {
            return await _db.SystemSubscriptions.AsNoTracking()
          .Where(s => s.ISPaid && s.EndDate > DateTime.Now)
          .Join(_db.Products,
                sub => sub.ProductId,
                prod => prod.Id,
                (sub, prod) => new { sub, prod })
          .GroupBy(x => x.prod.Name)
          .Select(g => new ProductSubscriptionStats
          {
              ProductName = g.Key,
              Count = g.Count()
          })
          .ToListAsync();
        }
    }
}
