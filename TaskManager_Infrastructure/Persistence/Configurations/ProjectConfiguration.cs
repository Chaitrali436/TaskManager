using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager_CoreLayer.Entities;

namespace TaskManager_InfrastructureLayer.Persistence.Configurations
{
    public class ProjectConfiguration: IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Projects");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(Project.MaxNameLength);

            builder.Property(p => p.Description)
                .HasMaxLength(2000);

            builder.Property(p => p.CreatedAt)
                .IsRequired();

            // One project has many tasks. Deleting a project deletes its tasks.
            builder.HasMany(p => p.Tasks)
                .WithOne()
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // The entity exposes a read-only list; EF Core fills the private _tasks field instead.
            builder.Navigation(p => p.Tasks)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
