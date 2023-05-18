using Repository.Base;
using Contracts.Interfaces;
using Entities.Models;

namespace Repository;
//Repository class which give us possibility to interact with "employee" table
public class EmployeeRepository : RepositoryBase<Employee>, 
								 IEmployeeRepository
{
	public EmployeeRepository(RepositoryContext repositoryContext) :
			base(repositoryContext)
	{}

	public IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges) =>
		FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
		.OrderBy(e => e.Name);

	public Employee GetEmployee(Guid companyId, Guid id, bool trackChanges) => 
		FindByCondition(e => e.CompanyId.Equals(companyId) & e.Id.Equals(id), trackChanges)
		.SingleOrDefault();

	public void CreateEmployeeForCompany(Guid companyId, Employee employee)
	{
		employee.CompanyId = companyId;
		Create(employee);
	}

	public void DeleteEmployee(Employee employee)
	{
		Delete(employee);
	}
}