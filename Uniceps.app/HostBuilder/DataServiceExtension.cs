using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.RoutineModels;
using Uniceps.Entityframework.Services.ExerciseServices;
using Uniceps.Entityframework.Services.MuscleGroupServices;
using Uniceps.Entityframework.Services.RoutineServices;

namespace Uniceps.app.HostBuilder
{
    public static class DataServiceExtension
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            services.AddScoped<ICommandDataService<Exercise>, ExerciseCommandDataService>();
            services.AddScoped<IQueryDataService<Exercise>, ExerciseQueryDataService>();
            services.AddScoped<IEntityQueryDataService<Exercise>, ExerciseQueryDataService>();
            services.AddScoped<ICommandDataService<MuscleGroup>, MuscleGroupCommandDataService>();
            services.AddScoped<IQueryDataService<MuscleGroup>, MuscleGroupQueryDataService>();
            services.AddScoped<ICommandDataService<Routine>, RoutineCommandDataService>();
            services.AddScoped<IQueryDataService<Routine>, RoutineQueryDataService>();
            services.AddScoped<IEntityQueryDataService<Day>, RoutineDayQueryDataService>();
            services.AddScoped<ICommandDataService<Day>, RoutineDayCommandDataService>();
            services.AddScoped<IQueryDataService<Day>, RoutineDayQueryDataService>();
            services.AddScoped<IEntityQueryDataService<RoutineItem>, RoutineItemQueryDataService>();
            services.AddScoped<ICommandDataService<RoutineItem>, RoutineItemCommandDataService>();
            services.AddScoped<IQueryDataService<RoutineItem>, RoutineItemQueryDataService>();
            return services;
        }
    }
}
