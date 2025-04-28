using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DataClass.Entities;

public class UserEntity : IdentityUser
{

    [ProtectedPersonalData]
    public string? FirstName { get; set; }

    [ProtectedPersonalData]
    public string? LastName { get; set; }
    

    public virtual ICollection<ProjectEntity> Projects { get; set; } = [];
}


