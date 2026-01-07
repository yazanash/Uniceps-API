using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.Models;
using Uniceps.Entityframework.Models.AuthenticationModels;
using Uniceps.Entityframework.Models.Measurements;
using Uniceps.Entityframework.Models.NotificationModels;
using Uniceps.Entityframework.Models.Products;
using Uniceps.Entityframework.Models.Profile;
using Uniceps.Entityframework.Models.RoutineModels;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;

namespace Uniceps.Entityframework.DBContext
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<MuscleGroup> MuscleGroups { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<OTPModel> OTPModels { get; set; }
        public DbSet<NormalProfile> NormalProfiles { get; set; }
        public DbSet<PlanModel> Plans { get; set; }
        public DbSet<PlanItem> PlanItems { get; set; }
        public DbSet<SystemSubscription> SystemSubscriptions { get; set; }
        public DbSet<UserDevice> UserDevices { get; set; }
        public DbSet<BodyMeasurement> BodyMeasurements { get; set; }
        public DbSet<WorkoutSession>  WorkoutSessions { get; set; }
        public DbSet<WorkoutLog> WorkoutLogs { get; set; }
        public DbSet<TelegramUserState> TelegramUserStates { get; set; }
        public DbSet<PaymentGateway> paymentGateways{ get; set; }
        public DbSet<CashPaymentRequest> CashPaymentRequests { get; set; }
        public DbSet<Product>  Products{ get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }
        public DbSet<UserStep> UserSteps { get; set; }
        public DbSet<Release> Releases{ get; set; }
        public DbSet<FrequentlyAskedQuestion> FrequentlyAskedQuestions{ get; set; }
        public DbSet<DownloadLog> DownloadLogs { get; set; }
        public DbSet<SiteSettings> SiteSettings{  get; set;}
        public DbSet<Notification> Notifications{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
       
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var idProperty = entity.FindProperty("NID");
                if (idProperty?.ClrType == typeof(Guid))
                {
                    idProperty.IsNullable = false;

                    // Use database-generated GUIDs (SQL Server only)
                    idProperty.SetDefaultValueSql("NEWSEQUENTIALID()");
                }
            }

          
            modelBuilder.Entity<MuscleGroup>().HasData
                (
                new MuscleGroup { Id = 1, Name = "صدر", EngName = "Chest" },
                new MuscleGroup { Id = 2, Name = "اكتاف", EngName = "Shoulders" },
                new MuscleGroup { Id = 3, Name = "ظهر", EngName = "Back" },
                new MuscleGroup { Id = 4, Name = "ارجل", EngName = "Legs" },
                new MuscleGroup { Id = 5, Name = "بايسيبس", EngName = "Biceps" },
                new MuscleGroup { Id = 6, Name = "ترايسيبس", EngName = "Triceps" },
                new MuscleGroup { Id = 7, Name = "بطات الارجل", EngName = "Calves" },
                new MuscleGroup { Id = 8, Name = "معدة", EngName = "ABS" },
                new MuscleGroup { Id = 9, Name = "سواعد", EngName = "ForeArms" },
                new MuscleGroup { Id = 10, Name = "تربيز", EngName = "Shrug" }
                );

            modelBuilder.Entity<PlanModel>()
             .HasMany(plan => plan.PlanItems)
             .WithOne(planItem => planItem.PlanModel) 
             .HasForeignKey(ri => ri.PlanNID);
            base.OnModelCreating(modelBuilder);

        }
    }
}
