using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListingsApi.Data.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(
                    new Country 
                    { 
                        Id = 1, 
                        Name = "USA", 
                        ShortName = "us" 
                    },
                    new Country 
                    { 
                        Id = 2, 
                        Name = "Jamaica", 
                        ShortName = "jm" 
                    }
                );
        }
    }
}
