using System.ComponentModel;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Uniceps.Entityframework.Models.Billing;
using Uniceps.Entityframework.Services.BillingLicenseService;

namespace Uniceps.app.Services
{
    public class LicenseGenerationService : ILicenseGenerationService<LicenseActivation>
    {
        private readonly IBellingLicenseService _billingLicenseService;
        private readonly IConfiguration _configuration;
        public LicenseGenerationService(IBellingLicenseService billingLicenseService, IConfiguration configuration)
        {
            _billingLicenseService = billingLicenseService;
            _configuration = configuration;
        }

        public async Task<LicenseActivation> Activate(Guid billingLicenseId, string machineId)
        {
            BillingLicense? license = await _billingLicenseService.Get(billingLicenseId);
            if (license == null)
                throw new Exception("License not found");

            var activations = await _billingLicenseService.GetAllActivations(billingLicenseId);

            var existingActivation = activations.FirstOrDefault(a => a.MachineId == machineId);

            if (existingActivation != null)
            {
                existingActivation.ActivatedAt = DateTime.UtcNow;
                existingActivation.ActivationToken = GenerateToken(existingActivation.LicenseId, machineId, existingActivation.ActivatedAt);

                await _billingLicenseService.UpdateActivation(existingActivation);

                return existingActivation;
            }

            if (activations.Count() >= license.MaxDevices)
                throw new Exception("Max devices reached. Please contact support.");

            var activation = new LicenseActivation
            {
                Id = Guid.NewGuid(),
                LicenseId = billingLicenseId,
                MachineId = machineId,
                ActivatedAt = DateTime.UtcNow,
                 ExpiredAt = license.ExpireDate,
                IsUsed = true 
            };

            activation.ActivationToken = GenerateToken(activation.LicenseId, activation.MachineId, activation.ActivatedAt);

            await _billingLicenseService.AddActivation(activation);

            return activation;
        }
        public string GenerateToken(Guid licenseId,string machineId,DateTime activatedAt)
        {
            string rawData = $"{licenseId}|{machineId}|{activatedAt.Ticks}";
            byte[] dataToSign = Encoding.UTF8.GetBytes(rawData);

            using (var rsa = RSA.Create())
            {
                var privateKey = _configuration["RSA:PrivateKey"];
                rsa.FromXmlString(privateKey!);

                byte[] signature = rsa.SignData(dataToSign, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                return Convert.ToBase64String(signature);
            }
        }
        public async Task<(byte[] FileBytes, string FileName)> DownloadLicenseFileAsync(Guid licenseId)
        {
            var license = await _billingLicenseService.Get(licenseId);

            var jsonString = JsonSerializer.Serialize(license, new JsonSerializerOptions { WriteIndented = true });
            var fileBytes = Encoding.UTF8.GetBytes(jsonString);
            var fileName = $"Uniceps_{license.CustomerName.Replace(" ", "_")}.unxlic";

            return (fileBytes, fileName);
        }

        public string GenerateRSASignature(string fileData)
        {
            var privateKeyXml = _configuration["RSA:PrivateKey"];

            if (string.IsNullOrEmpty(privateKeyXml))
                throw new Exception("Private Key is missing in server configuration!");

            using (var rsa = RSA.Create())
            {
                rsa.FromXmlString(privateKeyXml);

                byte[] dataBytes = Encoding.UTF8.GetBytes(fileData);

                byte[] signatureBytes = rsa.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                return Convert.ToBase64String(signatureBytes);
            }
        }
    }
}
