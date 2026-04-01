using Uniceps.Entityframework.Models.Billing;

namespace Uniceps.app.DTOs.LicensesDto
{
    public class LicenseResponse
    {
        private BillingLicense BillingLicense;

        public LicenseResponse(BillingLicense billingLicense)
        {
            BillingLicense = billingLicense;
        }
        public Guid Id => BillingLicense.Id;
        public int ProductId => BillingLicense.ProductId;
        public string CustomerName => BillingLicense.CustomerName;
        public LicenseType Type => BillingLicense.Type;
        public DateTime? ExpireDate => BillingLicense.ExpireDate;
        public int MaxDevices => BillingLicense.MaxDevices;
    }
}
