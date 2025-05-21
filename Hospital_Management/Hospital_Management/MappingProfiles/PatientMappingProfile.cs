using AutoMapper;
using Hospital_Management.Entities;
using Hospital_Management.ViewModels;

namespace Hospital_Management.MappingProfiles;

public class PatientMappingProfile : Profile
{
    public PatientMappingProfile()
    {
        CreateMap<Patient, PatientCreateVM>().ReverseMap();
        CreateMap<Patient, PatientGetVM>().ReverseMap();
        CreateMap<Patient, PatientIncludeVM>().ReverseMap();
        CreateMap<Patient, PatientUpdateVM>().ReverseMap();
    }
}
