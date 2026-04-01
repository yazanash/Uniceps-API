using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.Models.Billing;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.Entityframework.Services.BillingLicenseService
{
    public interface IBellingLicenseService
    {
        Task<BillingLicense> CreateAsync(BillingLicense entity);
        Task<BillingLicense> UpdateAsync(BillingLicense entity);
        Task<bool> Delete(Guid id);
        Task<BillingLicense> Get(Guid id);
        Task<IEnumerable<BillingLicense>> GetAll();
        Task<LicenseActivation> AddActivation(LicenseActivation licenseActivation);
        Task UpdateActivation(LicenseActivation licenseActivation);
        Task<LicenseActivation> GetActivation(Guid id);
        Task<IEnumerable<LicenseActivation>> GetAllActivations(Guid billingLicenseId);
    }
}
