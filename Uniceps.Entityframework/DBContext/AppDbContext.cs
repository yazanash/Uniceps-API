using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.Configurations;
using Uniceps.Entityframework.Models;
using Uniceps.Entityframework.Models.AuthenticationModels;
using Uniceps.Entityframework.Models.Billing;
using Uniceps.Entityframework.Models.Measurements;
using Uniceps.Entityframework.Models.NotificationModels;
using Uniceps.Entityframework.Models.NutritionSystem;
using Uniceps.Entityframework.Models.Products;
using Uniceps.Entityframework.Models.Profile;
using Uniceps.Entityframework.Models.RoutineModelsV2;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;

namespace Uniceps.Entityframework.DBContext
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<OTPModel> OTPModels { get; set; }
        public DbSet<NormalProfile> NormalProfiles { get; set; }
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

        public DbSet<ExerciseV2> ExercisesV2 { get; set; }
        public DbSet<MuscleGroupV2> MuscleGroupsV2{ get; set; }
        public DbSet<MuscleHead> Heads { get; set; }
        public DbSet<Equipment> Equipment { get; set; }

        public DbSet<LicenseActivation> LicenseActivations{ get; set; }
        public DbSet<BillingLicense> BillingLicenses{ get; set; }

        public DbSet<DietLog> DietLogs{ get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<IngredientCategory> IngredientCategories { get; set; }

        public DbSet<RoutineTemplate> RoutineTemplates { get; set; }

        public DbSet<SubscriptionCoupon> subscriptionCoupons { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
       
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var idProperty = entity.FindProperty("NID");
                if (idProperty?.ClrType == typeof(Guid))
                {
                    idProperty.IsNullable = false;
                    idProperty.SetDefaultValueSql("NEWSEQUENTIALID()");
                }
            }

            modelBuilder.ApplyConfiguration(new RoutineTemplateConfiguration());
          

            modelBuilder.Entity<ExerciseV2>()
        .HasKey(e => e.ExerciseId);

            modelBuilder.Entity<ExerciseV2>()
        .HasIndex(e => e.MuscleGroupCode)
        .HasDatabaseName("Index_MuscleGroupV2");

            modelBuilder.Entity<ExerciseV2>()
                .HasIndex(e => e.EquipmentCode)
                .HasDatabaseName("Index_Equipment");

            modelBuilder.Entity<ExerciseV2>()
        .Property(e => e.Mechanism)
        .HasConversion<string>();

            modelBuilder.Entity<MuscleHead>()
                .HasOne(h => h.Group)
                .WithMany(g => g.Heads)
                .HasForeignKey(h => h.MuscleGroupCode)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExerciseV2>()
           .HasOne(e => e.MuscleGroupV2)
           .WithMany() 
           .HasForeignKey(e => e.MuscleGroupCode)
           .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<ExerciseV2>()
                .HasOne(e => e.MuscleHead)
                .WithMany()
                .HasForeignKey(e => e.MuscleHeadCode)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ExerciseV2>()
                .HasOne(e => e.Equipment)
                .WithMany()
                .HasForeignKey(e => e.EquipmentCode)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<WorkoutLog>()
        .HasIndex(l => new { l.WorkoutSessionId, l.ExerciseId, l.ExerciseIndex, l.SetIndex })
        .IsUnique();
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SubscriptionCoupon>()
              .HasIndex(e => e.Code)
              .HasDatabaseName("Index_CouponCode");

        }
    }
}
