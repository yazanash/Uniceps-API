namespace Uniceps.app.DTOs.PayGatewayDtos
{
    public class PaymentGatewayDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string TransferInfo { get; set; } = null!;
        public string? QrCodeUrl { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
