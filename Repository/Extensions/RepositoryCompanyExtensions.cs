using Entities.Models;
using Repository.Extensions.Utilities;

using System.Reflection;
using System.Linq.Dynamic.Core;
using System.Text;

namespace Repository.Extensions;

public static class RepositoryCompanyExtensions
{
    public static IQueryable<Company> FilterCompanies(this IQueryable<Company> companies,
                                                      string country) =>
        companies.Where(c => (c.Country == country));                                      

    public static IQueryable<Company> Search(this IQueryable<Company> companies, string searchTerm)
    {
        if(string.IsNullOrEmpty(searchTerm))
            return companies;

        var lowerCase = searchTerm.Trim().ToLower();
        return companies.Where(c => c.Name.ToLower().Contains(lowerCase));
    }

    public static IQueryable<Company> Sort(this IQueryable<Company> companies,
                                           string orderByQueryString)
    {
        if(string.IsNullOrWhiteSpace(orderByQueryString))
            return companies.OrderBy(c => c.Name);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<Company>(orderByQueryString);

        if(string.IsNullOrWhiteSpace(orderQuery))
            return companies.OrderBy(c => c.Name);

        return companies.OrderBy(orderQuery);
    }
}