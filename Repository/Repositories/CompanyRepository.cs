using Repository.Base;
using Contracts.Interfaces;
using Entities.Models;

using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Threading.Tasks;

namespace Repository;
//Repository class which give us possibility to interact with "company" table
public class CompanyRepository : RepositoryBase<Company>, 
								 ICompanyRepository
{
	public CompanyRepository(RepositoryContext repositoryContext) :
			base(repositoryContext)
	{}

	public async Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges) =>
		await FindAll(trackChanges)
		.OrderBy(c => c.Name)
		.ToListAsync();

	public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) => 
		await FindByCondition(x => ids.Contains(x.Id), trackChanges)
		.ToListAsync();

	public async Task<Company> GetCompanyAsync(Guid companyId, bool trackChanges) =>
		await FindByCondition(c => c.Id.Equals(companyId), trackChanges)
		.SingleOrDefaultAsync();

	public void CreateCompany(Company company) =>
		Create(company);

	public void DeleteCompany(Company company) =>
		Delete(company);
}
