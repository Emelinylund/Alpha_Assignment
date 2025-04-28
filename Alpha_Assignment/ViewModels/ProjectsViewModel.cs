using Business.Models;

namespace Alpha_Assignment.ViewModels
{
    public class ProjectsViewModel
    {
        public IEnumerable<Project> Projects { get; set; } = [];
        public AddProjectForm AddProjectForm { get; set; } = new();
        public EditProjectForm EditProjectForm { get; set; } = new();
    }
}
