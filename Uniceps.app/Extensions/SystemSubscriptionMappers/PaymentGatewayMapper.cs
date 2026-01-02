using Uniceps.app.DTOs.PayGatewayDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models;

namespace Uniceps.app.Extensions.SystemSubscriptionMappers
{
    public class PaymentGatewayMapper : IMapperExtension<PaymentGateway, PaymentGatewayDto, PaymentGatewayCreationDto>
    {
        public PaymentGateway FromCreationDto(PaymentGatewayCreationDto data)
        {
            PaymentGateway paymentGateway = new PaymentGateway()
            {
                Name = data.Name!,
                TransferInfo = data.TransferInfo
            };
            return paymentGateway;
        }

        public PaymentGatewayDto ToDto(PaymentGateway data)
        {
            PaymentGatewayDto paymentGatewayDto = new PaymentGatewayDto()
            {
                Id = data.Id,
                Name = data.Name!,
                TransferInfo = data.TransferInfo
            };
            return paymentGatewayDto;
        }
    }
}
