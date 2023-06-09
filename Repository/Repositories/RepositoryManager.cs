using Contracts.Interfaces;
using Entities.Models;

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace Repository;
//This class presents existed repositories and save logic
public class RepositoryManager : IRepositoryManager
{
	private RepositoryContext _repositoryContext;
	private ICompanyRepository _companyRepository;
	private IEmployeeRepository _employeeRepository;

	public RepositoryManager(RepositoryContext repositoryContext)
	{
		_repositoryContext = repositoryContext;
	}

	public ICompanyRepository Company
	{
		get
		{
			if(_companyRepository == null)
				_companyRepository = new CompanyRepository(_repositoryContext);

			return _companyRepository;
		}
	}

	public IEmployeeRepository Employee
	{
		get 
		{
			if(_employeeRepository == null)
				_employeeRepository = new EmployeeRepository(_repositoryContext);

			return _employeeRepository;
		}
	}

	public Task SaveAsync()
	{
		return _repositoryContext.SaveChangesAsync();
	}
}
