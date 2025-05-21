using AutoMapper;
using Hospital_Management.Entities;
using Hospital_Management.ViewModels;

namespace Hospital_Management.MappingProfiles;

public class MedicalCardMappingProfile : Profile
{
    public MedicalCardMappingProfile()
    {
        CreateMap<MedicalCard, MedicalCardCreateVM>().ReverseMap();
        CreateMap<MedicalCard, MedicalCardGetVM>().ReverseMap();
        CreateMap<MedicalCard, MedicalCardIncludeVM>().ReverseMap();
        CreateMap<MedicalCard, MedicalCardUpdateVM>().ReverseMap();
    }
}
