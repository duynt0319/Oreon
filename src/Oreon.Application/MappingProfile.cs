//using AutoMapper;
//using Oreon.Application.Common.Extensions;
//using Oreon.Application.DTOs;
//using Oreon.Domain.Messages;
//using Oreon.Domain.Users;

//namespace Oreon.Application;

//public class MappingProfile : Profile
//{
//    public MappingProfile()
//    {
//        CreateMap<AppUser, MemberDto>()
//            .ForMember(d => d.PhotoUrl, o => o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain).Url))
//            .ForMember(d => d.Age, o => o.MapFrom(s => s.DateOfBirth.CalculateAge()));

//        CreateMap<Photo, PhotoDto>();

//        CreateMap<Message, MessageDto>()
//            .ForMember(d => d.SenderPhotoUrl, o => o.MapFrom(s => s.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
//            .ForMember(d => d.RecipientPhotoUrl, o => o.MapFrom(s => s.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url));

//        CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
//        CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);
//    }
//}
