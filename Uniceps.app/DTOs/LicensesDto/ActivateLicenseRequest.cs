namespace Uniceps.app.DTOs.LicensesDto
{
    public class ActivateLicenseRequest
    {
        public Guid LicenseId { get; set; }
        public string MachineId { get; set; } = string.Empty;
    }
}
