namespace Uniceps.app.Services
{
    public interface ILicenseGenerationService<T>
    {
        Task<T> Activate(Guid billingLicenseId, string machineId);
        string GenerateRSASignature(string fileData);
        Task<(byte[] FileBytes, string FileName)> DownloadLicenseFileAsync(Guid licenseId);
    }
}
