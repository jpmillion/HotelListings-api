using HotelListingsApi.Dto.Hotel;

namespace HotelListingsApi.Dto.Country
{
    public class GetCountryByIdResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public List<HotelDto> Hotels { get; set; }
    }
}
