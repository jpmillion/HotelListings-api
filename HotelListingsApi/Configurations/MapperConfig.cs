using AutoMapper;
using HotelListingsApi.Data;
using HotelListingsApi.Dto.Country;
using HotelListingsApi.Dto.Hotel;

namespace HotelListingsApi.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig() 
        {
            CreateMap<CreateCountryRequestDto, Country>().ReverseMap();
            CreateMap<Country, GetCountiesResponseDto>();
            CreateMap<Country, GetCountryByIdResponseDto>();
            CreateMap<Hotel, HotelDto>();
        }
    }
}
