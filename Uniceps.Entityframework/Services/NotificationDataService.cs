using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models;
using Uniceps.Entityframework.Models.Measurements;

namespace Uniceps.Entityframework.Services
{
    public interface INotificationDataService
    {
        Task CreateAsync(Notification notification);
        Task RemoveAsync(int notificationId);
        Task<IEnumerable<Notification>> GetNotifications();
    }
    public class NotificationDataService(AppDbContext dbContext) : INotificationDataService
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task CreateAsync(Notification notification)
        {
            EntityEntry<Notification> CreatedResult = await _dbContext.Set<Notification>().AddAsync(notification);
            await _dbContext.SaveChangesAsync();
            return;
        }

        public async Task<IEnumerable<Notification>> GetNotifications()
        {
            IEnumerable<Notification>? entities = await _dbContext.Set<Notification>().ToListAsync();
            return entities;
        }

        public async Task RemoveAsync(int notificationId)
        {
            Notification? entity = await _dbContext.Set<Notification>().FirstOrDefaultAsync((e) => e.Id == notificationId);
            if (entity == null)
                throw new Exception();
            _dbContext.Set<Notification>().Remove(entity!);
            await _dbContext.SaveChangesAsync();
            return;
        }
    }
}
