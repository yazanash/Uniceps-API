using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.Models.RoutineModelsV2;

namespace Uniceps.Entityframework.Configurations
{
    public class RoutineTemplateConfiguration : IEntityTypeConfiguration<RoutineTemplate>
    {
        public void Configure(EntityTypeBuilder<RoutineTemplate> builder)
        {
            builder.ToTable("RoutineTemplates");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.Description)
                .IsRequired();

            builder.Property(t => t.PayloadJson)
                .IsRequired()
                .HasColumnType("nvarchar(max)"); 

            builder.HasIndex(t => new { t.IsActive, t.Level, t.TargetGender });
        }
    }
}
