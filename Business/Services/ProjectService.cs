using Business.Dtos;
using Business.Extensions;
using Business.Models;
using DataClass.Contexts;
using DataClass.Entities;
using DataClass.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace Business.Services;

public interface IProjectService
{
    Task<ProjectResult> CreateProjectAsync(AddProjectForm formData);
    Task<ProjectResult<Project>> GetProjectAsync(string id);
    Task<ProjectResult<IEnumerable<Project>>> GetProjectsAsync();
    
    Task<ProjectResult> UpdateProjectAsync(EditProjectForm form);
    Task<ProjectResult> DeleteProjectAsync(string id);
    Task<IEnumerable<Project>> GetAllProjectsAsync();
    Task<EditProjectForm> GetProjectByIdAsync(string id);
}

public class ProjectService(IProjectRepository projectRepository, IStatusService statusService, AppDbContext context) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IStatusService _statusService = statusService;
    private readonly AppDbContext _context = context;
    private readonly IHttpContextAccessor _httpContextAccessor = new HttpContextAccessor();

    public async Task<ProjectResult> CreateProjectAsync(AddProjectForm formData)
    {
        if (formData == null)
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Not all required fields are supplied." };

        var projectEntity = formData.MapTo<ProjectEntity>();

        // Hämta inloggad användare
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return new ProjectResult { Succeeded = false, StatusCode = 401, Error = "User not logged in." };
        }

        projectEntity.UserId = userId; 

        var statusResult = await _statusService.GetStatusByIdAsync(formData.Status);

        if (!statusResult.Succeeded || statusResult.Result == null)
        {
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Invalid status" };
        }

        projectEntity.StatusId = statusResult.Result.Id;

        var result = await _projectRepository.AddAsync(projectEntity);

        return result.Succeeded
            ? new ProjectResult { Succeeded = true, StatusCode = 201 }
            : new ProjectResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }





    public async Task<ProjectResult<IEnumerable<Project>>> GetProjectsAsync()
    {
        var response = await _projectRepository.GetAllAsync();

        return response.MapTo<ProjectResult<IEnumerable<Project>>>();



    }

    public async Task<ProjectResult<Project>> GetProjectAsync(string id)
    {
        var response = await _projectRepository.GetAsync(x => x.Id == id);

        if (response.Succeeded && response.Result != null)
        {
            var project = response.Result.MapTo<Project>();




            return new ProjectResult<Project> { Succeeded = true, StatusCode = 200, Result = project };
        }
        else
        {
            return new ProjectResult<Project> { Succeeded = false, StatusCode = 404, Error = $"Project '{id}' was not found." };
        }
    }

    public async Task<ProjectResult> UpdateProjectAsync(EditProjectForm form)
    {
        if (form == null)
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Not all required fields are supplied." };

        var projectEntity = form.MapTo<ProjectEntity>();

        var result = await _projectRepository.UpdateAsync(projectEntity);

        return result.Succeeded
            ? new ProjectResult { Succeeded = true, StatusCode = 200 }
            : new ProjectResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }

    public async Task<ProjectResult> DeleteProjectAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Project ID is required." };

        var projectEntity = await _projectRepository.GetAsync(x => x.Id == id);

        if (!projectEntity.Succeeded || projectEntity.Result == null)
            return new ProjectResult { Succeeded = false, StatusCode = 404, Error = $"Project '{id}' was not found." };

        var result = await _projectRepository.DeleteAsync(projectEntity.Result);

        return result.Succeeded
            ? new ProjectResult { Succeeded = true, StatusCode = 200 }
            : new ProjectResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }

    public async Task<IEnumerable<Project>> GetAllProjectsAsync()
    {
        var result = await _context.Projects
            .Include(p => p.Client)
            .Include(p => p.User) //chat GPT suggested I should use .Include here
            .Include(p => p.Status)
            .Select(e => new Project
            {
                Id = e.Id,
                Image = e.Image,
                ProjectName = e.ProjectName,
                Description = e.Description,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Budget = e.Budget,
                Client = new Client
                {
                    Id = e.Client.Id,
                    ClientName = e.Client.ClientName
                },
                User = new User
                {
                    Id = e.User.Id,
                    FirstName = e.User.FirstName,
                    LastName = e.User.LastName
                },
                Status = new Status
                {
                    Id = e.Status.Id,
                    StatusName = e.Status.StatusName
                }
            })
            .ToListAsync(); 

        return result;
    }


    public Task<EditProjectForm> GetProjectByIdAsync(string id)
    {
        throw new NotImplementedException();
    }
}
