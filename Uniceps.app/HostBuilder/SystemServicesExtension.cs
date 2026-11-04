using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Uniceps.app.Services;
using Uniceps.app.Services.NotificationServices;
using Uniceps.app.Services.PaymentServices;
using Uniceps.app.Services.TelegramMachineState.Handlers;
using Uniceps.app.Services.TelegramMachineState.Processors;
using Uniceps.app.Services.TelegramMachineState.Renderers;
using Uniceps.app.Services.TesterServices;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.AuthenticationModels;
using Uniceps.Entityframework.Models.Billing;
using Uniceps.Entityframework.Models.StatsModels;
using Uniceps.Entityframework.Services;
using Uniceps.Entityframework.Services.ExerciseServices;
using Uniceps.Entityframework.Services.RoutineServices;
using Uniceps.Entityframework.Services.SystemSubscriptionServices;
using static Uniceps.app.Services.TelegramMachineState.State.IBotInterfaces;

namespace Uniceps.app.HostBuilder
{
    public static class SystemServicesExtension
    {
        public static IServiceCollection AddSystemServices(this IServiceCollection services)
        {

            services.AddSingleton<EmailService>();
            services.AddScoped<TelegramBotService>();
            services.AddScoped<IPaymentGateway, StripeGateway>();
            services.AddScoped<INotificationSender, FcmNotificationSender>();
            services.AddScoped<IJwtTokenService, TokenGenerationService>();
            services.AddScoped < IOTPGenerateService <OTPModel>, OTPGenerateService>();
            services.AddScoped<IStatsDataService<DashboardStats>, StatsDataService>();
            services.AddScoped<IBypassService, BypassService>();
            services.AddScoped<ExerciseImageService>();
            services.AddScoped<ILicenseGenerationService<LicenseActivation>,LicenseGenerationService>();
            services.AddScoped<IUniFileParserService, UniFileParserService>();
            services.AddScoped<ICouponDataService, SubscriptionCouponDataService>();

            services.AddScoped< PaymentConfirmedHandler>();
            services.AddScoped< ProcessCouponHandler>();
            services.AddScoped< SelectGatewayHandler>();
            services.AddScoped< SelectPlanHandler>();
            services.AddScoped< StartFlowHandler>();
            services.AddScoped< SubmitEmailHandler>();
            services.AddScoped<ProcessTransactionCodeHandler>();
            services.AddScoped<HandleConfirmCheckout>();

            services.AddScoped<PaymentFreeConfirmedHandler>();

            services.AddScoped<IStepProcessor, ChoosingGatewayStepProcessor>();
            services.AddScoped<IStepProcessor, ChoosingPlanStepProcessor>();
            services.AddScoped<IStepProcessor, EmailStepProcessor>();
            services.AddScoped<IStepProcessor, StartStepProcessor>();
            services.AddScoped<IStepProcessor, WaitingConfirmationStepProcessor>();
            services.AddScoped<IStepProcessor, WaitingCouponStepProcessor>();
            services.AddScoped<IStepProcessor, WaitingTransactionCodeStepProcessor>();
            services.AddScoped<IStepProcessor, SummeryCheckoutConfirmation>();

            services.AddScoped<IStepProcessor, WaitingFreeConfirmationProcess>();

            services.AddScoped<IStateViewRenderer, WaitingFreeConfirmationRendrer>();

            services.AddScoped<IStateViewRenderer, ChoosingGatewayViewRenderer>();
            services.AddScoped<IStateViewRenderer, ChoosingPlanViewRenderer>();
            services.AddScoped<IStateViewRenderer, WaitingConfirmationViewRenderer>();
            services.AddScoped<IStateViewRenderer, WaitingCouponViewRenderer>();
            services.AddScoped<IStateViewRenderer, WaitingEmailViewRenderer>();
            services.AddScoped<IStateViewRenderer, WaitingTransactionCodeViewRenderer>();
            services.AddScoped<IStateViewRenderer, SummeryCheckoutRenderer>();

            services.AddSingleton<ITelegramBotClient>(provider =>
            {
                var config = provider.GetRequiredService<IConfiguration>();
                string token = config["Telegram:Token"] ?? throw new InvalidOperationException("Telegram Token is missing!");
                return new TelegramBotClient(token);
            });
            return services;
        }
    }
}
