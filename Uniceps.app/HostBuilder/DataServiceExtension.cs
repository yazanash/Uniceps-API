using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.Profile;
using Uniceps.Entityframework.Models.RoutineModels;
using Uniceps.Entityframework.Services.ExerciseServices;
using Uniceps.Entityframework.Services.MuscleGroupServices;
using Uniceps.Entityframework.Services.RoutineServices;
using Uniceps.Entityframework.Services.ProfileServices;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;
using Uniceps.Entityframework.Services.SystemSubscriptionServices;
using Uniceps.Entityframework.Models.NotificationModels;
using Uniceps.Entityframework.Services.NotificationSystemServices;
using Uniceps.Entityframework.Models.BusinessLocalModels;
using Uniceps.Entityframework.Services.BusinessLocalServices;
using Uniceps.Entityframework.Models.Measurements;
using Uniceps.Entityframework.Services.MeasurementServices;
namespace Uniceps.app.HostBuilder
{
    public static class DataServiceExtension
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            services.AddScoped<IIntDataService<Exercise>, ExerciseDataService>();
            services.AddScoped<IIntEntityQueryDataService<Exercise>, ExerciseDataService>();
            services.AddScoped<IIntDataService<MuscleGroup>, MuscleGroupDataService>();
            services.AddScoped<IDataService<Routine>, RoutineDataService>();
            services.AddScoped<IEntityQueryDataService<Day>, RoutineDayDataService>();
            services.AddScoped<IEntityQueryDataService<RoutineItem>, RoutineItemDataService>();
            services.AddScoped<IDataService<RoutineItem>, RoutineItemDataService>();

            services.AddScoped<IDataService<NormalProfile>, NormalProfileDataService>();
            services.AddScoped<IDataService<BusinessProfile>, BusinessProfileDataService>();

            services.AddScoped<IGetByUserId<NormalProfile>, NormalProfileDataService>();
            services.AddScoped<IGetByUserId<BusinessProfile>, BusinessProfileDataService>();

            services.AddScoped<IDataService<PlanModel>, PlanDataService>();
            services.AddScoped<IDataService<SystemSubscription>, SystemSubscriptionDataService>();
            services.AddScoped<IGetByUserId<SystemSubscription>, SystemSubscriptionDataService>();
            services.AddScoped<IGetByTargetType<PlanModel>, PlanDataService>();

            services.AddScoped<IDataService<UserDevice>, UserDeviceDataService>();
            services.AddScoped<IUserQueryDataService<UserDevice>, UserDeviceDataService>();

            services.AddScoped<IDataService<PlayerModel>, PlayerModelDataService>();
            services.AddScoped<IUserQueryDataService<PlayerModel>, PlayerModelDataService>();
            services.AddScoped<IDataService<BusinessServiceModel>, BusinessServiceModelDataService>();
            services.AddScoped<IUserQueryDataService<BusinessServiceModel>, BusinessServiceModelDataService>();

            services.AddScoped<IDataService<BusinessSubscriptionModel>, BusinessSubscriptionModelDataService>();


            services.AddScoped<IDataService<BodyMeasurement>, BodyMeasurementDataService>();
            return services;
        }
    }
}
