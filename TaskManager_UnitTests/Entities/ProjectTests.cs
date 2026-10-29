using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager_CoreLayer.Entities;

namespace TaskManager_UnitTests.Entities
{
    public class ProjectTests
    {
        private static readonly DateTime Now = new(2026, 1, 15, 10, 0, 0, DateTimeKind.Utc);

        // ---------- Rule 8: project name required ----------

        [Fact]
        public void NewProject_SetsNameAndCreatedAt()
        {
            var project = new Project("  Learn CI/CD  ", Now, "My description");
            Assert.Equal("Learn CI/CD", project.Name);
            Assert.Equal("My description", project.Description);
            Assert.Equal(Now, project.CreatedAt);
            Assert.Empty(project.Tasks);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void NewProject_WithoutName_Throws(string? name)
        {
            Assert.Throws<ArgumentException>(() => new Project(name!, Now));
        }

        [Fact]
        public void NewProject_NameTooLong_Throws()
        {
            var longName = new string('a', Project.MaxNameLength + 1);

            Assert.Throws<ArgumentException>(() => new Project(longName, Now));
        }

        [Fact]
        public void Update_ChangesNameAndDescription()
        {
            var project = new Project("Old name", Now);

            project.Update("New name", "New description");

            Assert.Equal("New name", project.Name);
            Assert.Equal("New description", project.Description);
        }

        [Fact]
        public void Update_EmptyName_Throws()
        {
            var project = new Project("Old name", Now);

            Assert.Throws<ArgumentException>(() => project.Update("", null));
        }

        // ---------- Adding tasks ----------

        [Fact]
        public void AddTask_AddsTaskToProject()
        {
            var project = new Project("Learn CI/CD", Now);

            var task = project.AddTask("Write first pipeline", Now);

            Assert.Single(project.Tasks);
            Assert.Contains(task, project.Tasks);
        }
    }
}
