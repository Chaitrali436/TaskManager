using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager_CoreLayer.Entities;
using TaskManager_CoreLayer.Exceptions;

namespace TaskManager_UnitTests.Entities
{
    public class Taskitemtests
    {
        // A fixed "current time" so tests never depend on the real clock
        private static readonly DateTime Now = new(2026, 1, 15, 10, 0, 0, DateTimeKind.Utc);

        private static TaskItem CreateTask(DateTime? dueDate = null)
        {
            var project = new Project("Test project", Now);
            return project.AddTask("Write tests", Now, dueDate);
        }

        // ---------- Rule 3: new task starts as Todo ----------

        [Fact]
        public void NewTask_StartsAsTodo()
        {
            var task = CreateTask();

            Assert.Equal(TaskItemStatus.Todo, task.Status);
            Assert.Equal(Now, task.CreatedAt);
            Assert.Null(task.CompletedAt);
        }

        // ---------- Rule 1: title required ----------

        [Fact]
        public void NewTask_TrimsTitle()
        {
            var project = new Project("Test project", Now);

            var task = project.AddTask("  Write tests  ", Now);

            Assert.Equal("Write tests", task.Title);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void NewTask_WithoutTitle_Throws(string? title)
        {
            var project = new Project("Test project", Now);

            Assert.Throws<ArgumentException>(() => project.AddTask(title!, Now));
        }

        [Fact]
        public void NewTask_TitleTooLong_Throws()
        {
            var project = new Project("Test project", Now);
            var longTitle = new string('a', TaskItem.MaxTitleLength + 1);

            Assert.Throws<ArgumentException>(() => project.AddTask(longTitle, Now));
        }

        // ---------- Rule 2: due date not in the past ----------

        [Fact]
        public void NewTask_DueDateInPast_Throws()
        {
            Assert.Throws<ArgumentException>(() => CreateTask(Now.AddDays(-1)));
        }

        [Fact]
        public void NewTask_DueDateToday_IsAllowed()
        {
            var task = CreateTask(Now);

            Assert.Equal(Now, task.DueDate);
        }

        [Fact]
        public void NewTask_DueDateInFuture_IsAllowed()
        {
            var task = CreateTask(Now.AddDays(7));

            Assert.Equal(Now.AddDays(7), task.DueDate);
        }

        // ---------- Rule 4: status only moves forward ----------

        [Fact]
        public void ChangeStatus_TodoToInProgress_Succeeds()
        {
            var task = CreateTask();

            task.ChangeStatus(TaskItemStatus.InProgress, Now);

            Assert.Equal(TaskItemStatus.InProgress, task.Status);
            Assert.Null(task.CompletedAt);
        }

        [Theory]
        [InlineData(TaskItemStatus.Todo, TaskItemStatus.Todo)]
        [InlineData(TaskItemStatus.InProgress, TaskItemStatus.Todo)]
        [InlineData(TaskItemStatus.InProgress, TaskItemStatus.InProgress)]
        public void ChangeStatus_NotForward_ThrowsDomainException(
            TaskItemStatus current, TaskItemStatus target)
        {
            var task = CreateTask();
            if (current != TaskItemStatus.Todo)
            {
                task.ChangeStatus(current, Now);
            }

            Assert.Throws<DomainException>(() => task.ChangeStatus(target, Now));
        }

        [Fact]
        public void ChangeStatus_InvalidValue_Throws()
        {
            var task = CreateTask();

            Assert.Throws<ArgumentException>(() => task.ChangeStatus((TaskItemStatus)99, Now));
        }

        // ---------- Rule 6: completing records the time ----------

        [Fact]
        public void Complete_SetsDoneAndCompletedAt()
        {
            var task = CreateTask();
            var completedTime = Now.AddHours(3);

            task.Complete(completedTime);

            Assert.Equal(TaskItemStatus.Done, task.Status);
            Assert.Equal(completedTime, task.CompletedAt);
        }

        // ---------- Rule 5: Done task is locked ----------

        [Fact]
        public void DoneTask_CannotBeUpdated()
        {
            var task = CreateTask();
            task.Complete(Now);

            Assert.Throws<DomainException>(() => task.Update("New title", null, null));
        }

        [Fact]
        public void DoneTask_CannotChangeStatus()
        {
            var task = CreateTask();
            task.Complete(Now);

            Assert.Throws<DomainException>(() => task.ChangeStatus(TaskItemStatus.InProgress, Now));
        }

        // ---------- Update ----------

        [Fact]
        public void Update_ChangesFields()
        {
            var task = CreateTask();
            var newDue = Now.AddDays(3);

            task.Update("New title", "New description", newDue);
            Assert.Equal("New title", task.Title);
            Assert.Equal("New description", task.Description);
            Assert.Equal(newDue, task.DueDate);
        }

        [Fact]
        public void Update_EmptyTitle_Throws()
        {
            var task = CreateTask();

            Assert.Throws<ArgumentException>(() => task.Update("", null, null));
        }
    }
}
