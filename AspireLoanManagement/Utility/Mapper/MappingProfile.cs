using AspireLoanManagement.Business.Models;
using AspireLoanManagement.Repository;
using AutoMapper;

namespace AspireLoanManagement.Utility.Mapper
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            // <Source, Destination>
            CreateMap<LoanModelDTO, LoanModelVM>()
                .ForMember(dest => dest.Amount, source => source.MapFrom(src => src.LoanAmount))
                .ReverseMap();
            CreateMap<RepaymentModelVM, RepaymentModelDTO>()
                .ForMember(dest => dest.PendingAmount, source => source.MapFrom(src => src.RepaymentAmount));
        }
    }
}
