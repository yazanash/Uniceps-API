using Uniceps.Entityframework.Models;

namespace Uniceps.app.DTOs.PayGatewayDtos
{
    public class PaymentGatewayDto
    {
        public PaymentGatewayDto(PaymentGateway data)
        {

            Id = data.Id;
            Name = data.Name!;
            TransferInfo = data.TransferInfo;
            QrCodeUrl = data.QrCodeUrl;
            IsActive = data.IsActive;
           
        }
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string TransferInfo { get; set; } = null!;
        public string? QrCodeUrl { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
