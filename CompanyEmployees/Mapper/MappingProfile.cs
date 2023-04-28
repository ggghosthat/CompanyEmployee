using Entities.DTO;
using Entities.Models;

using AutoMapper;
namespace CompanyEmployees.Mapper;
//Here we resolve our mapping logic
public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<Company, CompanyDto>()
				.ForMember(c => c.FullAddress,
					opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));

		CreateMap<Employee, EmployeeDto>();
		CreateMap<CompanyForCreationDto, Company>();
		CreateMap<EmployeeForCreationDto, Employee>();
	}
}