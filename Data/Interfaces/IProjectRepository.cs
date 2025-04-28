using Alpha_Assignment.Entities;

namespace Data.Interfaces;

internal interface IProjectRepository
{
    bool Create(ProjectEntity projectEntity);

    Task<bool> DeleteAsync(ProjectEntity projectEntity);
    Task<bool> UpdateAsync(ProjectEntity projectEntity);
    IEnumerable<ProjectEntity> GetAll();
}
