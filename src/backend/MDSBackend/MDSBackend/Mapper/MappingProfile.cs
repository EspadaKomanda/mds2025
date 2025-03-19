using AutoMapper;
using MDSBackend.Models.Database;
using MDSBackend.Models.DTO;

namespace MDSBackend.Mapper;


public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region  UserProfileMapping

        CreateMap<UserProfile, UserProfileDTO>()
            .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(x => x.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(x => x.Surname, opt => opt.MapFrom(src => src.Surname))
            .ForMember(x => x.Patronymic, opt => opt.MapFrom(src => src.Patronymic))
            .ForMember(x => x.Birthdate, opt => opt.MapFrom(src => src.Birthdate))
            .ForMember(x => x.Gender, opt => opt.MapFrom(src => src.Gender))
            .ForMember(x => x.ContactEmail, opt => opt.MapFrom(src => src.ContactEmail))
            .ForMember(x => x.ContactPhone, opt => opt.MapFrom(src => src.ContactPhone))
            .ForMember(x => x.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture));

        CreateMap<UserProfileDTO, UserProfile>()
            .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(x => x.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(x => x.Surname, opt => opt.MapFrom(src => src.Surname))
            .ForMember(x => x.Patronymic, opt => opt.MapFrom(src => src.Patronymic))
            .ForMember(x => x.Birthdate, opt => opt.MapFrom(src => src.Birthdate))
            .ForMember(x => x.Gender, opt => opt.MapFrom(src => src.Gender))
            .ForMember(x => x.ContactEmail, opt => opt.MapFrom(src => src.ContactEmail))
            .ForMember(x => x.ContactPhone, opt => opt.MapFrom(src => src.ContactPhone))
            .ForMember(x => x.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture));

        #endregion
      
    }
}
