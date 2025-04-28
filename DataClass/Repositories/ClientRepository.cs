using DataClass.Contexts;
using DataClass.Entities;


namespace DataClass.Repositories;

public interface IClientRepository : IBaseRepository<ClientEntity>
{

}

public class ClientRepository(AppDbContext context) : BaseRepository<ClientEntity>(context), IClientRepository
{
}
