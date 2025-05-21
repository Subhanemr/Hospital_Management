using AutoMapper;
using Hospital_Management.Entities;
using Hospital_Management.ViewModels;

namespace Hospital_Management.MappingProfiles;

public class AppUserMappingProfile : Profile
{
    public AppUserMappingProfile()
    {
        CreateMap<AppUser, LoginVM>().ReverseMap();
        CreateMap<AppUser, RegisterVM>().ReverseMap();

        CreateMap<AppUser, AppUserCreateVM>().ReverseMap();
        CreateMap<AppUser, AppUserGetVM>().ReverseMap();
        CreateMap<AppUser, AppUserIncludeVM>().ReverseMap();
        CreateMap<AppUser, AppUserUpdateVM>().ReverseMap();
    }
}
