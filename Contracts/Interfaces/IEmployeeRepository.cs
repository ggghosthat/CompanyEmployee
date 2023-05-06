using Entities.Models;

using System.Collections;
namespace Contracts.Interfaces;
//presents repository for employee instance
public interface IEmployeeRepository
{
	IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges);
	Employee GetEmployee(Guid companyId, Guid id, bool trackChanges);
	void CreateEmployeeForCompany(Guid companyId, Employee employee);
	void DeleteEmployee(Employee employee);
}