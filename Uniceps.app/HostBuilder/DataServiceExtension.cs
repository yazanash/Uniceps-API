using Uniceps.app.Services.PaymentServices;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models;
using Uniceps.Entityframework.Models.Measurements;
using Uniceps.Entityframework.Models.NotificationModels;
using Uniceps.Entityframework.Models.Products;
using Uniceps.Entityframework.Models.Profile;
using Uniceps.Entityframework.Models.RoutineModels;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;
using Uniceps.Entityframework.Services;
using Uniceps.Entityframework.Services.ExerciseServices;
using Uniceps.Entityframework.Services.MeasurementServices;
using Uniceps.Entityframework.Services.MuscleGroupServices;
using Uniceps.Entityframework.Services.NotificationSystemServices;
using Uniceps.Entityframework.Services.ProductServices;
using Uniceps.Entityframework.Services.ProfileServices;
using Uniceps.Entityframework.Services.SystemSubscriptionServices;
namespace Uniceps.app.HostBuilder
{
    public static class DataServiceExtension
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            services.AddScoped<IExerciseDataService, ExerciseDataService>();
            services.AddScoped<IIntDataService<MuscleGroup>, MuscleGroupDataService>();
            
            services.AddScoped<IProfileDataService, NormalProfileDataService>();
            services.AddScoped<IIntDataService<WorkoutSession>, WorkoutSessionDataService>();

            services.AddScoped<IUserQueryDataService<WorkoutSession>, WorkoutSessionDataService>();

            services.AddScoped<IPlanDataService, PlanDataService>();
            services.AddScoped<IIntDataService<PlanItem>, PlanItemDataService>();
            services.AddScoped<IMembershipDataService, SystemSubscriptionDataService>();

            services.AddScoped<IUserDeviceDataService, UserDeviceDataService>();

            services.AddScoped<IDataService<BodyMeasurement>, BodyMeasurementDataService>();
            services.AddScoped<IUserQueryDataService<BodyMeasurement>, BodyMeasurementDataService>();

            services.AddScoped<ITelegramUserStateDataService<TelegramUserState>, TelegramUserStateDataService>();
            services.AddScoped<IIntDataService<PaymentGateway>, PaymentGatewayDataService>();
            services.AddScoped<ICashRequest, CashPaymentRequestDataService>();

            services.AddScoped<IProductDataService, ProductDataService>();
            services.AddScoped<IProductRelatedDataService<FrequentlyAskedQuestion>, FAQDataService>();
            services.AddScoped<IProductRelatedDataService<ProductFeature>, ProductFeatureDataService>();
            services.AddScoped<IProductRelatedDataService<UserStep>, UserStepDataService>();
            services.AddScoped<IReleaseDataService, ReleaseDataService>();
            services.AddScoped<ISiteSettingsService, SiteSettingsDataService>();
            return services;
        }
    }
}
