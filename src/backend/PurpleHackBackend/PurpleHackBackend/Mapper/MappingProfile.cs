using AutoMapper;
using PurpleHackBackend.Models.Database;
using PurpleHackBackend.Models.DTO;

namespace PurpleHackBackend.Mapper;


public class MappingProfile : Profile
{
    public MappingProfile()
    {

        #region UserMapping
        
        CreateMap<User, UserDTO>()
            .ForMember(x => x.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(x=>x.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(x=>x.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(x=>x.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(x=>x.Role, opt => opt.MapFrom(src => src.Role));


        CreateMap<UserDTO, User>()
            .ForMember(x => x.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(x => x.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(x => x.Email, opt => opt.MapFrom(src => src.Email));
        #endregion
        
        #region RoleMapping
        
        CreateMap<Role, RoleDTO>();
    
        CreateMap<RoleDTO, Role>();
        
        #endregion

        #region  UserProfileMapping

        CreateMap<UserProfile, UserProfileDTO>()
            .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(x => x.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(x => x.Surname, opt => opt.MapFrom(src => src.Surname))
            .ForMember(x => x.Patronymic, opt => opt.MapFrom(src => src.Patronymic))
            .ForMember(x => x.Birthdate, opt => opt.MapFrom(src => src.Birthdate))
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
            .ForMember(x => x.ContactEmail, opt => opt.MapFrom(src => src.ContactEmail))
            .ForMember(x => x.ContactPhone, opt => opt.MapFrom(src => src.ContactPhone))
            .ForMember(x => x.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture));

        #endregion
      
    }
}
