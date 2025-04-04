using AutoMapper;
using MDSBackend.Models.Database;
using MDSBackend.Models.DTO;

namespace MDSBackend.Mapper;


public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region UserProfileMapping

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

        CreateMap<UserProfileCreateDTO, UserProfile>()
            .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(x => x.Surname, opt => opt.MapFrom(src => src.Surname))
            .ForMember(x => x.Patronymic, opt => opt.MapFrom(src => src.Patronymic))
            .ForMember(x => x.Birthdate, opt => opt.MapFrom(src => src.Birthdate))
            .ForMember(x => x.Gender, opt => opt.MapFrom(src => src.Gender))
            .ForMember(x => x.ContactEmail, opt => opt.MapFrom(src => src.ContactEmail))
            .ForMember(x => x.ContactPhone, opt => opt.MapFrom(src => src.ContactPhone))
            .ForMember(x => x.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture));

        #endregion

        #region InstructionTestMapping

        CreateMap<InstructionTestCreateDTO, InstructionTest>()
            .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(x => x.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(x => x.ScoreCalcMethod, opt => opt.MapFrom(src => src.ScoreCalcMethod))
            .ForMember(x => x.MinScore, opt => opt.MapFrom(src => src.MinScore))
            .ForMember(x => x.MaxAttempts, opt => opt.MapFrom(src => src.MaxAttempts));
        
        #endregion

        #region InstructionTestQuestionCreateMapping

        CreateMap<InstructionTestQuestionCreateDTO, InstructionTestQuestion>()
            .ForMember(x => x.Question, opt => opt.MapFrom(src => src.Question))
            .ForMember(x => x.Answers, opt => opt.MapFrom(src => src.Answers))
            .ForMember(x => x.IsMultipleAnswer, opt => opt.MapFrom(src => src.IsMultipleAnswer))
            .ForMember(x => x.CorrectAnswers, opt => opt.MapFrom(src => src.CorrectAnswers));

        #endregion

        #region InstructionTestQuestionMapping

        CreateMap<InstructionTestQuestion, InstructionTestQuestionDTO>()
            .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(x => x.Question, opt => opt.MapFrom(src => src.Question))
            .ForMember(x => x.Answers, opt => opt.MapFrom(src => src.Answers))
            .ForMember(x => x.IsMultipleAnswer, opt => opt.MapFrom(src => src.IsMultipleAnswer));

        #endregion

        #region InstructionTestResultMapping

        CreateMap<InstructionTestResult, InstructionTestResultDTO>()
            .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(x => x.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(x => x.InstructionTestId, opt => opt.MapFrom(src => src.InstructionTestId))
            .ForMember(x => x.Score, opt => opt.MapFrom(src => src.Score));
      
        #endregion
    }
}
