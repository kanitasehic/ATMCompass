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
            CreateMap<AddATMRequest, ATM>();

            CreateMap<UpdateATMRequest, ATM>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ATM, GetATMResponse>()
                .ForMember(dest => dest.Lat, src => src.MapFrom(src => src.Node.Lat))
                .ForMember(dest => dest.Lon, src => src.MapFrom(src => src.Node.Lon))
                .ForMember(dest => dest.BankName, src => src.MapFrom(src => src.Bank.Name))
                .ForMember(dest => dest.BankPhone, src => src.MapFrom(src => src.Bank.Phone))
                .ForMember(dest => dest.BankWebsite, src => src.MapFrom(src => src.Bank.Website))
                .ForMember(dest => dest.BankEmail, src => src.MapFrom(src => src.Bank.Email))
                .ForMember(dest => dest.City, src => src.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.Street, src => src.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.HouseNumber, src => src.MapFrom(src => src.Address.HouseNumber))
                .ForMember(dest => dest.Postcode, src => src.MapFrom(src => src.Address.Postcode))
                .ForMember(dest => dest.CurrencyBAM, src => src.MapFrom(src => src.Currency != null ? src.Currency.BAM : null))
                .ForMember(dest => dest.CurrencyEUR, src => src.MapFrom(src => src.Currency != null ? src.Currency.EUR : null))
                .ForMember(dest => dest.CurrencyUSD, src => src.MapFrom(src => src.Currency != null ? src.Currency.USD : null));
        }
    }
}
