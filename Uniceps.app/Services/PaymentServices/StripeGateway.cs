using Stripe;
using Stripe.Checkout;
using System.Transactions;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.AuthenticationModels;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;

namespace Uniceps.app.Services.PaymentServices
{
    public class StripeGateway : IPaymentGateway
    {
        private readonly IConfiguration _config;
        private readonly IDataService<SystemSubscription> _dataService;

        public StripeGateway(IConfiguration config, IDataService<SystemSubscription> dataService)
        {
            _config = config;
            Stripe.StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
            _dataService = dataService;
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

        public async Task<bool> HandleWebhookAsync(string payload, string signatureHeader)
        {
            try
            {
                Console.WriteLine("handled");
                var stripeSecret = _config["Stripe:WebhookSecret"];
                var stripeEvent = EventUtility.ConstructEvent(payload, signatureHeader, stripeSecret);

                if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;

                    var subId = int.Parse(session!.Metadata["subscriptionId"]);

                    using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

                    var sub = await _dataService.Get(subId);
                    if (sub is null) return false;

                    sub.ISPaid = true;
                    sub.IsActive = true;
                    await _dataService.Update(sub);
                    scope.Complete();

                    return true;
                }

                return false;
            }
            catch
            {
                // Log error if needed
                return false;
            }
        }
    }
}
