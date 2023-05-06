using AutoMapper;
using HotelListingsApi.Data;
using HotelListingsApi.Dto.Country;
using HotelListingsApi.Dto.Hotel;
using HotelListingsApi.Dto.User;

namespace HotelListingsApi.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig() 
        {
            CreateMap<CreateCountryRequestDto, Country>().ReverseMap();
            CreateMap<Country, GetCountiesResponseDto>();
            CreateMap<Country, GetCountryByIdResponseDto>();
            CreateMap<PutCountryRequestDto, Country>();

            CreateMap<Hotel, HotelDto>().ReverseMap();
            CreateMap<AddHotelRequestDto, Hotel>();

            CreateMap<ApiUserDto, ApiUser>().ReverseMap();
        }
    }
}
