using DataClass.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataClass.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<UserEntity>(options)
{
    public virtual required DbSet<ClientEntity> Clients { get; set; }

    public virtual required DbSet<StatusEntity> Statuses { get; set; }

    public virtual required DbSet<ProjectEntity> Projects { get; set; }

   
}
