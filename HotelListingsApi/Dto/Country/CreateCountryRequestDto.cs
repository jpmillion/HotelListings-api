using System.ComponentModel.DataAnnotations;

namespace HotelListingsApi.Dto.Country
{
    public class CreateCountryRequestDto
    {
        [Required]
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
}
