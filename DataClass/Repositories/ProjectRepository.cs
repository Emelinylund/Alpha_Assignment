using DataClass.Contexts;
using DataClass.Entities;
using DataClass.Models;
using Microsoft.EntityFrameworkCore;

namespace DataClass.Repositories;

public interface IProjectRepository : IBaseRepository<ProjectEntity>
{
    Task<RepositoryResult<ProjectEntity>> GetByIdAsync(string id);
}
public class ProjectRepository(AppDbContext context) : BaseRepository<ProjectEntity>(context), IProjectRepository
{
    public async Task<RepositoryResult<ProjectEntity>> GetByIdAsync(string id)
    {
        var project = await _context.Projects
            .Include(p => p.Client)
            .Include(p => p.User)
            .Include(p => p.Status)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project == null)
        {
            return new RepositoryResult<ProjectEntity>
            {
                Succeeded = false,
                StatusCode = 404,
                Error = $"Project with id '{id}' was not found."
            };
        }

        return new RepositoryResult<ProjectEntity>
        {
            Succeeded = true,
            StatusCode = 200,
            Result = project
        };
    }

}
