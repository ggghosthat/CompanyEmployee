using Entities.Models;
using Repository.Extensions.Utilities;

using System.Reflection;
using System.Linq.Dynamic.Core;
using System.Text;

namespace Repository.Extensions;

public static class RepositoryEmployeeExtensions
{
    public static IQueryable<Employee> FilterEmployees(this IQueryable<Employee> employees,
                                                        uint minAge, 
                                                        uint maxAge) => 
        employees.Where(e => (e.Age >= minAge && e.Age <= maxAge));

    public static IQueryable<Employee> Search(this IQueryable<Employee> employees, string searchTerm)
    {
        if(string.IsNullOrEmpty(searchTerm))
            return employees;

        var lowerCase = searchTerm.Trim().ToLower();
        return employees.Where(e => e.Name.ToLower().Contains(lowerCase));
    }

    public static IQueryable<Employee> Sort(this IQueryable<Employee> employees,
                                            string orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return employees.OrderBy(e => e.Name);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<Employee>(orderByQueryString);

        if(string.IsNullOrWhiteSpace(orderQuery))
            return employees.OrderBy(e => e.Name);

        return employees.OrderBy(orderQuery);
    }
}
