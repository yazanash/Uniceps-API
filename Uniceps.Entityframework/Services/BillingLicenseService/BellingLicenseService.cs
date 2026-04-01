using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.Billing;
using Uniceps.Entityframework.Models.Profile;

namespace Uniceps.Entityframework.Services.BillingLicenseService
{
    public class BellingLicenseService(AppDbContext dbContext) : IBellingLicenseService
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<LicenseActivation> AddActivation(LicenseActivation licenseActivation)
        {
            EntityEntry<LicenseActivation> CreatedResult = await _dbContext.Set<LicenseActivation>().AddAsync(licenseActivation);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }
        public async Task UpdateActivation(LicenseActivation licenseActivation)
        {
            _dbContext.Set<LicenseActivation>().Update(licenseActivation);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<BillingLicense> CreateAsync(BillingLicense entity)
        {
            EntityEntry<BillingLicense> CreatedResult = await _dbContext.Set<BillingLicense>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(Guid id)
        {
            BillingLicense? entity = await _dbContext.Set<BillingLicense>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            _dbContext.Set<BillingLicense>().Remove(entity!);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<BillingLicense> Get(Guid id)
        {
            BillingLicense? entity = await _dbContext.Set<BillingLicense>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<LicenseActivation> GetActivation(Guid id)
        {
            LicenseActivation? entity = await _dbContext.Set<LicenseActivation>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<BillingLicense>> GetAll()
        {
            IEnumerable<BillingLicense>? entities = await _dbContext.Set<BillingLicense>().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<LicenseActivation>> GetAllActivations(Guid billingLicenseId)
        {
            IEnumerable<LicenseActivation>? entities = await _dbContext.Set<LicenseActivation>().Where(x=>x.LicenseId == billingLicenseId).ToListAsync();
            return entities;
        }

        public async Task<BillingLicense> UpdateAsync(BillingLicense entity)
        {
            _dbContext.Set<BillingLicense>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

    }
}
