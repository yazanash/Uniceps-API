﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.Entityframework.Services.RoutineServices
{
    public class RoutineDataService : IDataService<Routine>
    {
        private readonly AppDbContext _contextFactory;

        public RoutineDataService(AppDbContext contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<Routine> Create(Routine entity)
        {
            EntityEntry<Routine> CreatedResult = await _contextFactory.Set<Routine>().AddAsync(entity);
            await _contextFactory.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            Routine? entity = await _contextFactory.Set<Routine>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            _contextFactory.Set<Routine>().Remove(entity!);
            await _contextFactory.SaveChangesAsync();
            return true;
        }

        public async Task<Routine> Update(Routine entity)
        {
            _contextFactory.Set<Routine>().Update(entity);
            await _contextFactory.SaveChangesAsync();
            return entity;
        }
        public async Task<Routine> Get(int id)
        {
            Routine? entity = await _contextFactory.Set<Routine>().AsNoTracking()
                .Include(x => x.Days)
                .ThenInclude(d => d.RoutineItems)
                .ThenInclude(x => x.Exercise)
                .Include(x => x.Days)
                .ThenInclude(d => d.RoutineItems)
                .ThenInclude(i => i.Sets).FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<Routine>> GetAll()
        {
            IEnumerable<Routine>? entities = await _contextFactory.Set<Routine>()
                .Include(x => x.Days)
                .ThenInclude(d => d.RoutineItems)
                .ThenInclude(x => x.Exercise)
                .Include(x => x.Days)
                .ThenInclude(d => d.RoutineItems)
                .ThenInclude(i => i.Sets).ToListAsync();
            return entities;

        }
    }
}
