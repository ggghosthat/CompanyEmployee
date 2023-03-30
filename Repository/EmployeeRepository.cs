using Repository.Base;
using Contracts.Interfaces;
using Entities.Models;

namespace Repository;
//Repository class which give us possibility to interact with "employee" table
public class EmployeeRepository : RepositoryBase<Company>, 
								 IEmployeeRepository
{
	public EmployeeRepository(RepositoryContext repositoryContext) :
			base(repositoryContext)
	{}
}