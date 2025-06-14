﻿using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.Profile;
using Uniceps.Entityframework.Models.RoutineModels;
using Uniceps.Entityframework.Services.ExerciseServices;
using Uniceps.Entityframework.Services.MuscleGroupServices;
using Uniceps.Entityframework.Services.RoutineServices;
using Uniceps.Entityframework.Services.ProfileServices;
namespace Uniceps.app.HostBuilder
{
    public static class DataServiceExtension
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            services.AddScoped<IDataService<Exercise>, ExerciseDataService>();
            services.AddScoped<IEntityQueryDataService<Exercise>, ExerciseDataService>();
            services.AddScoped<IDataService<MuscleGroup>, MuscleGroupDataService>();
            services.AddScoped<IDataService<Routine>, RoutineDataService>();
            services.AddScoped<IEntityQueryDataService<Day>, RoutineDayDataService>();
            services.AddScoped<IEntityQueryDataService<RoutineItem>, RoutineItemDataService>();
            services.AddScoped<IDataService<RoutineItem>, RoutineItemDataService>();

            services.AddScoped<IDataService<NormalProfile>, NormalProfileDataService>();
            services.AddScoped<IDataService<BusinessProfile>, BusinessProfileDataService>();

            services.AddScoped<IGetByUserId<NormalProfile>, NormalProfileDataService>();
            services.AddScoped<IGetByUserId<BusinessProfile>, BusinessProfileDataService>();
            return services;
        }
    }
}
