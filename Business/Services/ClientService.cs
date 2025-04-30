using Business.Dtos;
using Business.Extensions;
using Business.Models;
using DataClass.Repositories;

namespace Business.Services;

public interface IClientService
{
    Task<dynamic> GetClientNamesAsync();
    Task<ClientResult> GetClientsAsync();
}


public class ClientService(IClientRepository clientRepository) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;

    public Task<dynamic> GetClientNamesAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ClientResult> GetClientsAsync()
    {
        var result = await _clientRepository.GetAllAsync();
        if (result.Succeeded && result.Result != null)
        {
            return new ClientResult
            {
                Succeeded = true,
                StatusCode = 200,
                Result = result.Result.Select(x => new Client
                {
                    ClientName = x.ClientName,
                    Id = x.Id
                }).ToList()



            };
        }

        return new ClientResult();

           // return result.MapTo<ClientResult>();

    }

  
}
