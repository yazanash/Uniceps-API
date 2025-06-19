using Uniceps.app.Services.PaymentServices;
using Uniceps.app.Services;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.RoutineModels;
using Uniceps.Entityframework.Services.ExerciseServices;
using Uniceps.app.Services.NotificationServices;

namespace Uniceps.app.HostBuilder
{
    public static class SystemServicesExtension
    {
        public static IServiceCollection AddSystemServices(this IServiceCollection services)
        {
            services.AddSingleton<EmailService>();
            services.AddScoped<MongoDbService>();
            services.AddScoped<DataMigrationService>();
            services.AddScoped<IPaymentGateway, StripeGateway>();
            services.AddScoped<INotificationSender, FcmNotificationSender>();
            return services;
        }
    }
}
