using ATMCompass.Core.Entities;
using ATMCompass.Core.Models.ATMs.OverpassAPI;
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
            CreateMap<AddATMRequest, ATM>();
            CreateMap<ATM, AddATMResponse>();
            CreateMap<UpdateATMRequest, ATM>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<ATM, GetATMResponse>();
        }
    }
}
