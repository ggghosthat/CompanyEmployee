using Microsoft.EntityFrameworkCore;

namespace Entities.Models;

public class RepositoryContext : DbContext
{
	public RepositoryContext(DbContextOptions options) 
		: base(options)
	{
	}

	public DbSet<Company> Companies {get; set;}
	public DbSet<Company> Employees {get; set;}
}