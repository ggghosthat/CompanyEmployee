using Entities.Configuration;

using Microsoft.EntityFrameworkCore;

namespace Entities.Models;
//These special class resolve communication between sql db with EntityFramework
public class RepositoryContext : DbContext
{
	public RepositoryContext(DbContextOptions options) 
		: base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfiguration(new CompanyConfiguration());
		modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
	}

	public DbSet<Company> Companies {get; set;}
	public DbSet<Company> Employees {get; set;}
}