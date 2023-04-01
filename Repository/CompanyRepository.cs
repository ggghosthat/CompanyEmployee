using Repository.Base;
using Contracts.Interfaces;
using Entities.Models;

using System.Linq;
using System.Collections;

namespace Repository;
//Repository class which give us possibility to interact with "company" table
public class CompanyRepository : RepositoryBase<Company>, 
								 ICompanyRepository
{
	public CompanyRepository(RepositoryContext repositoryContext) :
			base(repositoryContext)
	{}

	public IEnumerable<Company> GetAllCompanies(bool trackChanges) =>
		FindAll(trackChanges)
		.OrderBy(c => c.Name)
		.ToList();
}