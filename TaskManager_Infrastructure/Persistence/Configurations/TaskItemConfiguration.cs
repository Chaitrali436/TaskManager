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
    public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            builder.ToTable("Tasks");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(TaskItem.MaxTitleLength);

            builder.Property(t => t.Description)
                .HasMaxLength(2000);

            // Stored as text ("Todo", "InProgress", "Done") so the table is readable in SQL Server.
            builder.Property(t => t.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(t => t.CreatedAt)
                .IsRequired();

            // Speeds up the common query: "tasks of this project with this status".
            builder.HasIndex(t => new { t.ProjectId, t.Status });
        }
    }
}
