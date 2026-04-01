using Uniceps.Entityframework.Models.Billing;

namespace Uniceps.app.DTOs.LicensesDto
{
    public class CreateLicenseRequest
    {
        public string CustomerName { get; set; } = "";
        public LicenseType Type { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int MaxDevices { get; set; } = 1;
        public BillingLicense ToModel()
        {
            return new BillingLicense
            {
                CustomerName = CustomerName,
                ExpireDate = ExpireDate,
                MaxDevices = MaxDevices,
                Type = Type,
            };
        }
    }

}
