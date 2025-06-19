using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.Models.AuthenticationModels;
using Uniceps.Entityframework.Models.NotificationModels;
using Uniceps.Entityframework.Models.Profile;
using Uniceps.Entityframework.Models.RoutineModels;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;

namespace Uniceps.Entityframework.DBContext
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
        public DbSet<MuscleGroup> MuscleGroups { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Routine> Routines { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<RoutineItem> RoutineItems { get; set; }
        public DbSet<ItemSet> Sets { get; set; }
        public DbSet<OTPModel> OTPModels { get; set; }
        public DbSet<BusinessProfile> BusinessProfiles  { get; set; }
        public DbSet<NormalProfile> NormalProfiles { get; set; }
        public DbSet<PlanModel> Plans { get; set; }
        public DbSet<SystemSubscription> SystemSubscriptions { get; set; }

        public DbSet<UserDevice> UserDevices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Routine → Days (One-to-Many)
            modelBuilder.Entity<Day>()
                .HasOne(d => d.Routine)
                .WithMany(r => r.Days)
                .HasForeignKey(d => d.RoutineId);

            // Day → RoutineItems (One-to-Many)
            modelBuilder.Entity<RoutineItem>()
                .HasOne(ri => ri.Day)
                .WithMany(d => d.RoutineItems)
                .HasForeignKey(ri => ri.DayId);

            // RoutineItem → Exercise (One-to-One)
            modelBuilder.Entity<RoutineItem>()
                .HasOne(ri => ri.Exercise)
                .WithMany() // No navigation in Exercise
                .HasForeignKey(ri => ri.ExerciseId);

            // RoutineItem → Sets (One-to-Many)
            modelBuilder.Entity<ItemSet>()
                .HasOne(s => s.RoutineItem)
                .WithMany(ri => ri.Sets)
                .HasForeignKey(s => s.RoutineItemId);

            modelBuilder.Entity<MuscleGroup>().HasData(
        new MuscleGroup { Id = 1, Name = "صدر",  EngName = "Chest" },
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
        }
    }
}
