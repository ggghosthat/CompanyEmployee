using Entities.Models;

using System.Threading.Tasks;
using System.Collections;

namespace Contracts.Interfaces;
//presents repository for employee instance
public interface IEmployeeRepository
{
	Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, bool trackChanges);
	Task<Employee> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges);
	void CreateEmployeeForCompany(Guid companyId, Employee employee);
	void DeleteEmployee(Employee employee);
}
