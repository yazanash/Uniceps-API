using Stripe;
using Stripe.Checkout;
using System.Numerics;
using System.Transactions;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.AuthenticationModels;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;
using Uniceps.Entityframework.Services.ProductServices;
using Uniceps.Entityframework.Services.SystemSubscriptionServices;

namespace Uniceps.app.Services.PaymentServices
{
    public class StripeGateway : IPaymentGateway
    {
        private readonly IConfiguration _config;
        private readonly IMembershipDataService _dataService;
        private readonly IProductDataService _productDataService;
        public StripeGateway(IConfiguration config, IMembershipDataService dataService, IProductDataService productDataService)
        {
            _config = config;
            Stripe.StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
            _dataService = dataService;
            _productDataService = productDataService;
        }

        public async Task<string?> CreateSessionAsync(SystemSubscription sub, AppUser user, PlanItem planItem)
        {
            var product = await _productDataService.Get(sub.ProductId);
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = ["card"],
                Mode = "payment",
                LineItems =
            [
                new()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmount = (long)(sub.Price * 100),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = product.Name +" - " + planItem.PlanModel?.Name+" - "+planItem.DurationString
                        }
                    },
                    Quantity = 1
                }
            ],
                SuccessUrl = "https://yourdomain.com/payment-success",
                CancelUrl = "https://yourdomain.com/payment-cancelled",
                Metadata = new Dictionary<string, string>
            {
                { "subscriptionId", sub.NID.ToString() },
                { "userId", user.Id.ToString() },
                { "planItemId", planItem.Id.ToString() }
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

                    var subId = Guid.Parse(session!.Metadata["subscriptionId"]);

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
