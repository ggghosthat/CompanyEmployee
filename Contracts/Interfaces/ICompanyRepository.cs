using Entities.Models;

using System.Collections;
namespace Contracts.Interfaces;
//presents repository for company instance
public interface ICompanyRepository
{
	IEnumerable<Company> GetAllCompanies(bool trackChanges);
	Company GetCompany(Guid companyId, bool trackChanges);
	void CreateCompany(Company company);
}