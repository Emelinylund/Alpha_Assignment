using DataClass.Contexts;
using DataClass.Entities;


namespace DataClass.Repositories;

public interface IStatusRepository : IBaseRepository<StatusEntity>
{
}
public class StatusRepository(AppDbContext context) : BaseRepository<StatusEntity>(context), IStatusRepository
{
}
