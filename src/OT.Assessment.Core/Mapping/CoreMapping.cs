using AutoMapper;
using OT.Assessment.Core.Dtos;
using OT.Assessment.Data.Models;

namespace OT.Assessment.Consumer.Mapping
{
    public class CoreMapping : Profile
    {
        public CoreMapping()
        {
            CreateMap<Wager, WagerDto>()
                .ForMember(dest => dest.Game, opt => opt.MapFrom(src => src.Transactions.FirstOrDefault().GameName))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Transactions.FirstOrDefault().Amount));

            CreateMap<Account, AccountDto>();
        }
    }
}
