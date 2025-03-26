using AutoMapper;
using EmployeeManagementAPI.Entities.Models;
using EmployeeManagementAPI.SharedResources.DTO;

namespace EmployeeManagementAPI.Host;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Company, CompanyDto>()
            .ForMember(c => c.FullAddress,
            opt => opt.MapFrom(x => string.Join(' ', new string?[] { x.Address, x.Country })));

        CreateMap<Employee, EmployeeDto>();

        CreateMap<CompanyForCreationDto, Company>();

        CreateMap<EmployeeForCreationDto, Employee>();

        CreateMap<EmployeeForUpdateDto, Employee>();

        CreateMap<CompanyForUpdateDto, Company>();

        CreateMap<UserForRegistrationDto, User>();
    }
}
