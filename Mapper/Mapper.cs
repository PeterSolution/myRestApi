using AutoMapper;
using ServerApi.Models;
namespace ServerApi.Mapper
{
    public class Mapper : Profile
    {

        public Mapper() 
        {
            CreateMap<DataDbModel, DataUserDbModel>().ReverseMap();
            CreateMap<UserDbModel,UserForUserDbModel>().ReverseMap();
            CreateMap<NotificationDbModel, NotificationForUserDbModel>().ReverseMap();
            CreateMap<ChatDbModel,ChatUserDbModel>().ReverseMap();
            CreateMap<ChatForWho,UserChatForWho>().ReverseMap();
            CreateMap<UserDbModel,UserForShowDbModel>().ReverseMap();
        }
        
    }
}
