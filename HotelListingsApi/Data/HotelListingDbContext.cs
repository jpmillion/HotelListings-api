using Microsoft.EntityFrameworkCore;

namespace HotelListingsApi.Data
{
    public class HotelListingDbContext : DbContext
    {
        public HotelListingDbContext(DbContextOptions options) : base(options)
        { 

        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasData(

                new[]
                {
                    new Country { Id = 1, Name = "USA", ShortName = "us" },
                    new Country { Id = 2, Name = "Jamaica", ShortName = "jm"}
                }
            );

            modelBuilder.Entity<Hotel>().HasData(

                new[]
                {
                    new Hotel { Id = 1, Address = "midland", Name = "Sandals", CountryId = 1, Rating = 4.8 },
                    new Hotel { Id = 2, Address = "croach", Name = "Secrets", CountryId = 2, Rating = 4.9 },
                    new Hotel { Id = 3, Address = "million", Name = "Beaches", CountryId = 1, Rating = 4.2 }
                }
            );

        }
    }
}
