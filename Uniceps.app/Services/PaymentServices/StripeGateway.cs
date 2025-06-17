using Stripe.Checkout;
using Uniceps.Entityframework.Models.AuthenticationModels;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;

namespace Uniceps.app.Services.PaymentServices
{
    public class StripeGateway : IPaymentGateway
    {
        private readonly IConfiguration _config;

        public StripeGateway(IConfiguration config)
        {
            _config = config;
            Stripe.StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
        }

        public async Task<string?> CreateSessionAsync(SystemSubscription sub, AppUser user, PlanModel plan)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "payment",
                LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmount = (long)(sub.Price * 100),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = plan.Name
                        }
                    },
                    Quantity = 1
                }
            },
                SuccessUrl = "https://yourdomain.com/payment-success",
                CancelUrl = "https://yourdomain.com/payment-cancelled",
                Metadata = new Dictionary<string, string>
            {
                { "subscriptionId", sub.Id.ToString() },
                { "userId", user.Id.ToString() },
                { "planId", plan.Id.ToString() }
            }
            };

            var session = await new SessionService().CreateAsync(options);
            return session.Url;
        }
    }
}
