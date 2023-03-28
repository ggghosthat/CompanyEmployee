using Repository.Base;
using Contracts.Interfaces;
using Entities.Models;

namespace Repository;
//Repository class which give us possibility to interact with "company" table
public class CompanyRepository : RepositoryBase<Company>, 
								 ICompanyRepository
{
	public CompanyRepository(RepositoryContext repositoryContext) :
			base(repositoryContext)
	{}
}