using System.ComponentModel.DataAnnotations;

namespace HotelListingsApi.Dto.Country
{
    public abstract class BaseCountryDto
    {
        [Required]
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
}
