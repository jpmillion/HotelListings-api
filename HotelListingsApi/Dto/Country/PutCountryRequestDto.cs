using System.ComponentModel.DataAnnotations;

namespace HotelListingsApi.Dto.Country
{
    public class PutCountryRequestDto : BaseCountryDto
    {
        [Required]
        public int Id { get; set; }
    }
}
