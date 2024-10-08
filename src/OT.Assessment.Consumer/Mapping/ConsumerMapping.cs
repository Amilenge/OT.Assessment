using AutoMapper;
using OT.Assessment.Api.Contract.Request;
using OT.Assessment.Data.Models;

namespace OT.Assessment.Consumer.Mapping
{
    public class ConsumerMapping : Profile
    {
        public ConsumerMapping()
        {
            CreateMap<CasinoWagerRequest, Wager>();

            CreateMap<CasinoWagerRequest, Transaction>();

            CreateMap<CasinoWagerRequest, Account>()
                .ForMember(dest => dest.TotalAmountSpend, opt => opt.MapFrom(src => 0.00m ));
        }
    }
}
