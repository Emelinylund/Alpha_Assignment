using DataClass.Contexts;
using DataClass.Entities;


namespace DataClass.Repositories;

public interface IProjectRepository : IBaseRepository<ProjectEntity>
{
}
public class ProjectRepository(AppDbContext context) : BaseRepository<ProjectEntity>(context), IProjectRepository
{
}
