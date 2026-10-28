using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager_CoreLayer.Entities
{
    public class Project
    {
        public const int MaxNameLength = 100;

        private readonly List<TaskItem> _tasks = new();

        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public DateTime CreatedAt { get; private set; }

        // Read-only view: outside code can't add to the list directly
        public IReadOnlyCollection<TaskItem> Tasks => _tasks;

        // Needed later by the database library (EF Core)
        private Project()
        {
        }

        public Project(string name, DateTime now, string? description = null)
        {
            ValidateName(name);

            Name = name.Trim();
            Description = description;
            CreatedAt = now;
        }

        public void Update(string name, string? description)
        {
            ValidateName(name);

            Name = name.Trim();
            Description = description;
        }

        public TaskItem AddTask(string title, DateTime now, DateTime? dueDate = null, string? description = null)
        {
            var task = new TaskItem(title, description, dueDate, now);
            _tasks.Add(task);
            return task;
        }

        private static void ValidateName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Project name is required.", nameof(name));
            }

            if (name.Trim().Length > MaxNameLength)
            {
                throw new ArgumentException(
                    $"Project name cannot exceed {MaxNameLength} characters.", nameof(name));
            }
        }
    }
}
