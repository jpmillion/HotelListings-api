using HotelListingsApi.Dto.Hotel;

namespace HotelListingsApi.Dto.Country
{
    public class GetCountryByIdResponseDto : BaseCountryDto
    {
        public int Id { get; set; }
        public List<HotelDto> Hotels { get; set; }
    }
}
