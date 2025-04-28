using DataClass.Contexts;
using DataClass.Entities;


namespace DataClass.Repositories;


public interface IUserRepository : IBaseRepository<UserEntity>
{
}
public class UserRepository(AppDbContext context) : BaseRepository<UserEntity>(context), IUserRepository
{
}
