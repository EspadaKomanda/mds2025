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

      
    }
}