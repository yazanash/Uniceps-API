using Microsoft.Extensions.DependencyInjection;
using Uniceps.app.Services;
using Uniceps.app.Services.NotificationServices;
using Uniceps.app.Services.PaymentServices;
using Uniceps.app.Services.TesterServices;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.AuthenticationModels;
using Uniceps.Entityframework.Models.RoutineModels;
using Uniceps.Entityframework.Models.StatsModels;
using Uniceps.Entityframework.Services;
using Uniceps.Entityframework.Services.ExerciseServices;

namespace Uniceps.app.HostBuilder
{
    public static class SystemServicesExtension
    {
        public static IServiceCollection AddSystemServices(this IServiceCollection services)
        {

            services.AddSingleton<EmailService>();
            services.AddScoped<MongoDbService>();
            services.AddScoped<DataMigrationService>();
            services.AddScoped<TelegramBotService>();
            services.AddScoped<IPaymentGateway, StripeGateway>();
            services.AddScoped<INotificationSender, FcmNotificationSender>();
            services.AddScoped<IJwtTokenService, TokenGenerationService>();
            services.AddScoped < IOTPGenerateService <OTPModel>, OTPGenerateService>();
            services.AddScoped<IStatsDataService<DashboardStats>, StatsDataService>();
            services.AddScoped<IBypassService, BypassService>();
            return services;
        }
    }
}
