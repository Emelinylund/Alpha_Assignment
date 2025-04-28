using Alpha_Assignment.Models;
using Business.Dtos;
using Business.Extensions;
using DataClass.Entities;
using DataClass.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public interface IUserService
{
    Task<UserResult> GetUsersAsync();
    Task<UserResult> CreateUserAsync(RegisterFormModel formData);
}

public class UserService(IUserRepository userRepository, UserManager<UserEntity> userManager) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly UserManager<UserEntity> _userManager = userManager;

    public async Task<UserResult> GetUsersAsync()
    {
        var result = await _userRepository.GetAllAsync();
        return result.MapTo<UserResult>();
    }

    public async Task<UserResult> CreateUserAsync(RegisterFormModel formData)
    {
        if (formData == null)
            return new UserResult { Succeeded = false, StatusCode = 400, Error = "Form data is missing." };

        var user = new UserEntity
        {
            FirstName = formData.FirstName,
            LastName = formData.LastName,
            Email = formData.Email,
            UserName = formData.Email,
        };

        var result = await _userManager.CreateAsync(user, formData.Password);

        if (result.Succeeded)
        {
            return new UserResult { Succeeded = true, StatusCode = 201 };
        }

        var errorMessages = result.Errors.Select(e => e.Description).ToArray();
        return new UserResult
        {
            Succeeded = false,
            StatusCode = 400,
            Error = string.Join(", ", errorMessages)
        };
    }
}
