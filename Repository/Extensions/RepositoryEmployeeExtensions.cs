using Entities.Models;

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

        var orderParams = orderByQueryString.Trim().Split(',');
        var propertyInfos = typeof(Employee).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var orderQueryBuilder = new StringBuilder();

        foreach (var param in orderParams)
        {
            if(string.IsNullOrWhiteSpace(param))
                continue;

            var properetyFromQueryName = param.Split(" ")[0];
            var objectProperty = propertyInfos.FirstOrDefault(pi => 
                pi.Name.Equals(properetyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

            if(objectProperty == null)
                continue;

            var direction = param.EndsWith(" desc") ? "descending" : "ascending";
            orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction},");
        }

        var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

        if(string.IsNullOrWhiteSpace(orderQuery))
            return employees.OrderBy(e => e.Name);

        return employees.OrderBy(orderQuery);
    }
}
