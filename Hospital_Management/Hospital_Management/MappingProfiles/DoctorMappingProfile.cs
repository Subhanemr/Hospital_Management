using AutoMapper;
using Hospital_Management.Entities;
using Hospital_Management.ViewModels;

namespace Hospital_Management.MappingProfiles;

public class DoctorMappingProfile : Profile
{
    public DoctorMappingProfile()
    {
        CreateMap<Doctor, DoctorCreateVM>().ReverseMap();
        CreateMap<Doctor, DoctorGetVM>().ReverseMap();
        CreateMap<Doctor, DoctorIncludeVM>().ReverseMap();
        CreateMap<Doctor, DoctorUpdateVM>().ReverseMap();
    }
}
