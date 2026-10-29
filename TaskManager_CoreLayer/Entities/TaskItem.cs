using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager_CoreLayer.Exceptions;

namespace TaskManager_CoreLayer.Entities
{
    public class TaskItem
    {
        public const int MaxTitleLength = 200;

        public int Id { get; private set; }
        public int ProjectId { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public TaskItemStatus Status { get; private set; }
        public DateTime? DueDate { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        // Needed later by the database library (EF Core)
        private TaskItem()
        {
        }

        // internal: only code inside TaskManager.Core (the Project class) can create tasks
        internal TaskItem(string title, string? description, DateTime? dueDate, DateTime now)
        {
            ValidateTitle(title);

            if (dueDate.HasValue && dueDate.Value.Date < now.Date)
            {
                throw new ArgumentException("Due date cannot be in the past.", nameof(dueDate));
            }

            Title = title.Trim();
            Description = description;
            DueDate = dueDate;
            Status = TaskItemStatus.Todo;
            CreatedAt = now;
        }

        public void Update(string title, string? description, DateTime? dueDate)
        {
            EnsureNotDone();
            ValidateTitle(title);

            Title = title.Trim();
            Description = description;
            DueDate = dueDate;
        }

        public void ChangeStatus(TaskItemStatus newStatus, DateTime now)
        {
            EnsureNotDone();

            if (!Enum.IsDefined(newStatus))
            {
                throw new ArgumentException("Invalid status.", nameof(newStatus));
            }

            if (newStatus < Status)
            {
                throw new DomainException(
                    $"Status can only move forward. Current status is {Status}.");
            }

            Status = newStatus;

            if (newStatus == TaskItemStatus.Done)
            {
                CompletedAt = now;
            }
        }

        public void Complete(DateTime now) => ChangeStatus(TaskItemStatus.Done, now);

        private void EnsureNotDone()
        {
            if (Status == TaskItemStatus.Done)
            {
                throw new DomainException("A completed task cannot be changed.");
            }
        }

        private static void ValidateTitle(string? title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Task title is required.", nameof(title));
            }

            if (title.Trim().Length > MaxTitleLength)
            {
                throw new ArgumentException(
                    $"Task title cannot exceed {MaxTitleLength} characters.", nameof(title));
            }
        }
    }
}
