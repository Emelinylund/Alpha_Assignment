using Business.Models;

namespace Alpha_Assignment.ViewModels
{
    public class ProjectsViewModel
    {
        public IEnumerable<Project> Projects { get; set; } = [];
        public AddProjectForm AddProjectForm { get; set; } = new();
        public EditProjectForm EditProjectForm { get; set; } = new();

    }

    public class EditProjectViewModel
    {
        public EditProjectForm Form { get; set; } = null!;
        public List<string> Statuses { get; set; } = [];
        public List<string> Clients { get; set; } = [];
    }
}
