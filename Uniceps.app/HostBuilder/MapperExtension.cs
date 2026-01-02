using Uniceps.app.DTOs.ExerciseDtos;
using Uniceps.app.DTOs.MeasurementDtos;
using Uniceps.app.DTOs.MuscleGroupDtos;
using Uniceps.app.DTOs.PayGatewayDtos;
using Uniceps.app.DTOs.ProductDtos;
using Uniceps.app.DTOs.ProductDtos.FAQDtos;
using Uniceps.app.DTOs.ProductDtos.ProductFeatureDtos;
using Uniceps.app.DTOs.ProductDtos.UserStepDtos;
using Uniceps.app.DTOs.ProfileDtos;
using Uniceps.app.DTOs.ReleaseDtos;
using Uniceps.app.DTOs.SystemSubscriptionDtos;
using Uniceps.app.DTOs.UserDeviceDto;
using Uniceps.app.Extensions.MeasurementMappers;
using Uniceps.app.Extensions.ProductMappers;
using Uniceps.app.Extensions.ProfileMappers;
using Uniceps.app.Extensions.RoutineMappers;
using Uniceps.app.Extensions.SystemSubscriptionMappers;
using Uniceps.app.Extensions.UserDeviceMappers;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models;
using Uniceps.Entityframework.Models.Measurements;
using Uniceps.Entityframework.Models.NotificationModels;
using Uniceps.Entityframework.Models.Products;
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
            services.AddScoped<IMapperExtension<WorkoutSession, WorkoutSessionDto, WorkoutSessionCreationDto>, WorkoutSessionMapperExtension>();

            services.AddScoped<IMapperExtension<NormalProfile, NormalProfileDto, NormalProfileCreationDto>, NormalProfileMapper>();
        
            services.AddScoped<IMapperExtension<PlanModel, PlanDto, PlanCreationDto>, PlanMapperExension>();
            services.AddScoped<IMapperExtension<PlanItem, PlanItemDto, PlanItemCreationDto>, PlanItemMapperExension>();
            services.AddScoped<IMapperExtension<UserDevice, UserDeviceDto, UserDeviceCreationDto>, UserDeviceMapperExtension>();

          
            services.AddScoped<IMapperExtension<BodyMeasurement, BodyMeasurementDto, BodyMeasurementCreationDto>, BodyMeasurementMapper>();
            services.AddScoped<IMapperExtension<PaymentGateway, PaymentGatewayDto, PaymentGatewayCreationDto>, PaymentGatewayMapper>();

            services.AddScoped<IMapperExtension<Product, ProductDto, ProductCreationDto>, ProductMapperExtension>();

            services.AddScoped<IMapperExtension<UserStep, UserStepDto, UserStepCreationDro>, UserStepMapperExension>();
            services.AddScoped<IMapperExtension<ProductFeature, ProductFeatureDto, ProductFeatureCreationDto>, ProductFeatureMapperExension>();
            services.AddScoped<IMapperExtension<Release, ReleaseDto, ReleaseCreationDto>, ReleaseMapperExtension>();
            services.AddScoped<IMapperExtension<FrequentlyAskedQuestion, FrequentlyAskedQuestionDto, FrequentlyAskedQuestionCreationDto>, FAQMapperExension>();

            return services;
        }
    }
}
