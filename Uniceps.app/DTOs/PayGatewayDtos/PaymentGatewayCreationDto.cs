using Uniceps.Entityframework.Models;

namespace Uniceps.app.DTOs.PayGatewayDtos
{
    public class PaymentGatewayCreationDto
    {
        public string Name { get; set; } = null!;
        public string TransferInfo { get; set; } = null!;
        public string? QrCodeUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public IFormFile? QrCodeFile { get; set; }
        public PaymentGateway ToModel()
        {
            PaymentGateway paymentGateway = new PaymentGateway()
            {
                Name = Name!,
                TransferInfo = TransferInfo,
                IsActive = IsActive,
            };
            return paymentGateway;
        }
    }
}
