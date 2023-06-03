using ATMCompass.Core.Entities;
using ATMCompass.Core.Models.ATMs.Requests;
using ATMCompass.Core.Models.ATMs.Responses;
using AutoMapper;

namespace ATMCompass.Core.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // ATMs
            CreateMap<UpdateATMRequest, ATM>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ATM, GetATMResponse>()
                .ForMember(dest => dest.Lat, opt => opt.MapFrom(src => src.Node.Lat))
                .ForMember(dest => dest.Lon, opt => opt.MapFrom(src => src.Node.Lon))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.HouseNumber, opt => opt.MapFrom(src => src.Address.HouseNumber));
        }
    }
}
