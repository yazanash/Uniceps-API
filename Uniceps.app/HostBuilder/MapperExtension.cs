using Uniceps.app.DTOs.BusinessLocalDtos;
using Uniceps.app.DTOs.BusinessLocalDtos.BusinessServicesDtos;
using Uniceps.app.DTOs.ExerciseDtos;
using Uniceps.app.DTOs.MuscleGroupDtos;
using Uniceps.app.DTOs.ProfileDtos;
using Uniceps.app.DTOs.RoutineDtos;
using Uniceps.app.DTOs.SystemSubscriptionDtos;
using Uniceps.app.DTOs.UserDeviceDto;
using Uniceps.app.Extensions.BusinessLocalMappers;
using Uniceps.app.Extensions.ProfileMappers;
using Uniceps.app.Extensions.RoutineMappers;
using Uniceps.app.Extensions.SystemSubscriptionMappers;
using Uniceps.app.Extensions.UserDeviceMappers;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.BusinessLocalModels;
using Uniceps.Entityframework.Models.NotificationModels;
using Uniceps.Entityframework.Models.Profile;
using Uniceps.Entityframework.Models.RoutineModels;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;
using Uniceps.Entityframework.Services.ExerciseServices;
using Uniceps.Entityframework.Services.MuscleGroupServices;

namespace Uniceps.app.HostBuilder
{
    public static class MapperExtension
    {
        public static IServiceCollection AddMappers(this IServiceCollection services)
        {
            services.AddScoped<IMapperExtension<Exercise, ExerciseDto, ExerciseCreateDto>, ExerciseMapperExtension>();
            services.AddScoped<IMapperExtension<MuscleGroup, MuscleGroupDto, MuscleGroupCreateDto>, MuscleGroupMapperExtension>();
            services.AddScoped<IMapperExtension<Routine, RoutineDto, RoutineCreationDto>, RoutineMapperExtension>();

            services.AddScoped<IMapperExtension<Day, DayDto, DayCreationDto>, RoutineDayMapperExtension>();
            services.AddScoped<IMapperExtension<RoutineItem, RoutineItemDto, RoutineItemCreationDto>, RoutineItemMapperExtension>();

            services.AddScoped<IMapperExtension<NormalProfile, NormalProfileDto, NormalProfileCreationDto>, NormalProfileMapper>();
            services.AddScoped<IMapperExtension<BusinessProfile, BusinessProfileDto, BusinessProfileCreationDto>, BusinessProfileMapper>();


            services.AddScoped<IMapperExtension<PlanModel, PlanDto, PlanCreationDto>, PlanMapperExension>();
            services.AddScoped<IMapperExtension<UserDevice, UserDeviceDto, UserDeviceCreationDto>, UserDeviceMapperExtension>();

            services.AddScoped<IMapperExtension<PlayerModel, PlayerModelDto, PlayerModelCreationDto>, PlayerModelMapper>();

            services.AddScoped<IMapperExtension<BusinessServiceModel, BusinessServiceDto, BusinessServiceCreationDto>, BusinessServiceMapper>();
            return services;
        }
    }
}
