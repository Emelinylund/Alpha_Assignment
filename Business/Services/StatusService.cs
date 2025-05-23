﻿using Business.Dtos;
using Business.Models;
using DataClass.Repositories;
using DataClass.Models;

namespace Business.Services;

public interface IStatusService
{
    Task<StatusResult<Status>> GetStatusByIdAsync(int id);
    Task<StatusResult<Status>> GetStatusByNameAsync(string statusName);
    Task<StatusResult<IEnumerable<Status>>> GetStatusesAsync();
    Task<dynamic> GetStatusNamesAsync();
}

public class StatusService(IStatusRepository statusRepository) : IStatusService
{
    private readonly IStatusRepository _statusRepository = statusRepository;

    public async Task<StatusResult<IEnumerable<Status>>> GetStatusesAsync()
    {
        var result = await _statusRepository.GetAllAsync();
        return result.Succeeded && result.Result != null
            ? new StatusResult<IEnumerable<Status>> { Succeeded = true, StatusCode = 200, Result = result.Result.Select(e => new Status { Id = e.Id, StatusName = e.StatusName }) }
            : new StatusResult<IEnumerable<Status>> { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }

    public async Task<StatusResult<Status>> GetStatusByNameAsync(string statusName)
    {
        var result = await _statusRepository.GetAsync(x => x.StatusName == statusName);
        return result.Succeeded && result.Result != null
           ? new StatusResult<Status> { Succeeded = true, StatusCode = 200, Result = new Status { Id = result.Result.Id, StatusName = result.Result.StatusName } }
           : new StatusResult<Status> { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }

    public async Task<StatusResult<Status>> GetStatusByIdAsync(int id)
    {
        var result = await _statusRepository.GetAsync(x => x.Id == id);
        return result.Succeeded && result.Result != null
           ? new StatusResult<Status> { Succeeded = true, StatusCode = 200, Result = new Status { Id = result.Result.Id, StatusName = result.Result.StatusName } }
           : new StatusResult<Status> { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }

    public Task<dynamic> GetStatusNamesAsync()
    {
        throw new NotImplementedException();
    }
}

