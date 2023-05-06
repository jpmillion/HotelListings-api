using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListingsApi.Data.Configurations
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
                    new Hotel { Id = 1, Address = "midland", Name = "Sandals", CountryId = 1, Rating = 4.8 },
                    new Hotel { Id = 2, Address = "croach", Name = "Secrets", CountryId = 2, Rating = 4.9 },
                    new Hotel { Id = 3, Address = "million", Name = "Beaches", CountryId = 1, Rating = 4.2 }
                );
        }
    }
}
