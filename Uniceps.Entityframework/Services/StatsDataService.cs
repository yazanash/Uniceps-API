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
using Uniceps.Entityframework.Models.StatsModels;

namespace Uniceps.Entityframework.Services
{
    public class StatsDataService(AppDbContext db) : IStatsDataService<DashboardStats>
    {
        private readonly AppDbContext _db = db;

        public async Task<DashboardStats> GetDashboardStatsAsync()
        {
            return new DashboardStats
            {
                UsersCount = await _db.Users.CountAsync(),

                TotalBusinessUsers = await _db.Users
                 .Where(u => u.UserType == UserType.Business)
                 .CountAsync(),

                ActiveUsers = await GetActiveUsers(),

                MonthlyNewUsers = await GetMonthlyNewUsers(),

                ActiveSubscriptions = await GetSubscriptionStats(),

                TrainingSessions = await GetTrainingSessions(),
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
            return await _db.Users
                .GroupBy(u => new { u.CreatedAt.Year, u.CreatedAt.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => new MonthlyNewUsers
                {
                    Month = $"{g.Key.Month}/{g.Key.Year}",
                    NormalUsers = g.Count(u => u.UserType == UserType.Normal),
                    BusinessUsers = g.Count(u => u.UserType == UserType.Business)
                })
                .ToListAsync();
        }

        private async Task<List<ActiveSubscriptions>> GetSubscriptionStats()
        {
            return await _db.SystemSubscriptions
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
            return await _db.WorkoutSessions
                .GroupBy(s => new { s.CreatedAt.Year, s.CreatedAt.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => new TrainingSessions
                {
                    Month = $"{g.Key.Month}/{g.Key.Year}",
                    Sessions = g.Count()
                })
                .ToListAsync();
        }
    }
}
