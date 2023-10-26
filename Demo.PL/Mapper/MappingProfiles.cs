using AutoMapper;
using Demo.DAL.Entities;
using Demo.PL.Models;

namespace Demo.PL.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<EmployeeViewModels, Employee>().ReverseMap();
            //CreateMap<DepartmentViewModels, Department>().ReverseMap();
            //CreateMap<Employee, EmployeeViewModel>();
        }
    }
}
