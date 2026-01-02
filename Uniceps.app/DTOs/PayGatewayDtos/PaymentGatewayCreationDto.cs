namespace Uniceps.app.DTOs.PayGatewayDtos
{
    public class PaymentGatewayCreationDto
    {
        public string Name { get; set; } = null!;
        public string TransferInfo { get; set; } = null!;
        public string? QrCodeUrl { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
