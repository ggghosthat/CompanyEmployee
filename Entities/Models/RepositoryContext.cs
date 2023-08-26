using Entities.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace Entities.Models;
//These special class resolve communication between sql db with EntityFramework
public class RepositoryContext : IdentityDbContext<User>
{
	public RepositoryContext(DbContextOptions options) 
		: base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
        base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfiguration(new CompanyConfiguration());
		modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
	}

	public DbSet<Company> Companies {get; set;}
	public DbSet<Company> Employees {get; set;}
}
