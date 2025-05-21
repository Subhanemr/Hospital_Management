using AutoMapper;
using Hospital_Management.Entities;
using Hospital_Management.Validations.Appointments;
using Hospital_Management.ViewModels;

namespace Hospital_Management.MappingProfiles;

public class AppointmentMappingProfile : Profile
{
    public AppointmentMappingProfile()
    {
        CreateMap<Appointment, AppointmentCreateVM>().ReverseMap();
        CreateMap<Appointment, AppointmentGetVM>().ReverseMap();
        CreateMap<Appointment, AppointmentIncludeVM>().ReverseMap();
        CreateMap<Appointment, AppointmentUpdateVM>().ReverseMap();
    }
}
