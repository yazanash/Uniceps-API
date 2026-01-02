using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.Entityframework.Services
{
    public class TelegramUserStateDataService(AppDbContext dbContext) : ITelegramUserStateDataService<TelegramUserState>
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<TelegramUserState> GetOrCreateAsync(long chatId)
        {
            TelegramUserState? entity = await _dbContext.Set<TelegramUserState>().AsNoTracking().FirstOrDefaultAsync((e) => e.ChatId == chatId);
            if (entity == null)
            {
                entity = new TelegramUserState { ChatId = chatId };
                await _dbContext.Set<TelegramUserState>().AddAsync(entity);
                await _dbContext.SaveChangesAsync();
            }
               
            return entity;
        }

        public async Task<TelegramUserState> UpdateAsync(TelegramUserState entity)
        {
            _dbContext.Set<TelegramUserState>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
