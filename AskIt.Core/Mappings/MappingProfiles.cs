using ATMCompass.Core.Entities;
using ATMCompass.Core.Models.ATMs.OverpassAPI;
using AutoMapper;

namespace ATMCompass.Core.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            /*
            CreateMap<QuestionCategoryUpdateRequest, QuestionCategory>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));*/

            // ATMs

            CreateMap<GetATMFromOSMItem, ATM>()
               .ForMember(dest => dest.Id, opt => opt.Ignore())
               .ForMember(dest => dest.ExternalId, src => src.MapFrom(src => src.Id))
               .ForMember(dest => dest.IsDriveThroughEnabled, src => src.MapFrom(src => src.Tags.DriveThrough == "yes" ? true : false))
               .ForMember(dest => dest.IsAccessibleUsingWheelchair, src => src.MapFrom(src => src.Tags.Wheelchair == "yes" ? true : false))
               .ForMember(dest => dest.Location, src => src.MapFrom(src => src.Tags.City))
               .ForMember(dest => dest.Address, src => src.MapFrom(src => src.Tags.Street))
               .ForMember(dest => dest.OpeningHours, src => src.MapFrom(src => src.Tags.OpeningHours))
               .ForMember(dest => dest.Website, src => src.MapFrom(src => src.Tags.Website))
               .ForMember(dest => dest.Name, opt => opt.MapFrom((src, dest) =>
               {
                   if (src.Tags.Name is not null)
                       return src.Tags.Name;
                   else if (src.Tags.Operator is not null)
                       return src.Tags.Operator;
                   else if (src.Tags.Brand is not null)
                       return src.Tags.Brand;
                   else
                       return src.Tags.Fee;
               }));
        }
    }
}
