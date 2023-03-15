using Microsoft.EntityFrameworkCore;

namespace HotelListingsApi.Data
{
    public class HotelListingDbContext : DbContext
    {
        public HotelListingDbContext(DbContextOptions options) : base(options)
        { 

        }
    }
}
