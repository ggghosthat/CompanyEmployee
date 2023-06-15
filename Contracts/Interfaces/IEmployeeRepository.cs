using Entities.Models;
using Entities.RequestFeatures;

using System.Threading.Tasks;
using System.Collections;

namespace Contracts.Interfaces;
//presents repository for employee instance
public interface IEmployeeRepository
{
	Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges);
	Task<Employee> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges);
	void CreateEmployeeForCompany(Guid companyId, Employee employee);
	void DeleteEmployee(Employee employee);
}
