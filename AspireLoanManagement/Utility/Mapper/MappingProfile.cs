using AspireLoanManagement.Business.Models;
using AspireLoanManagement.Repository;
using AutoMapper;

namespace AspireLoanManagement.Utility.Mapper
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            // Source => Destination
            CreateMap<LoanModelDTO, LoanModelVM>()
                .ForMember(dest => dest.Term, source => source.MapFrom(src => src.Term))
                .ReverseMap();
        }
    }
}
