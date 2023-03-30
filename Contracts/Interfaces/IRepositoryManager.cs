namespace Contracts.Interfaces;
//presents repository manager to hold and represent existed repositories and saving logic
public interface IRepositoryManager
{
	ICompanyRepository Company {get;}
	IEmployeeRepository Employee {get;}

	void Save();
}