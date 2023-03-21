using HotelListingsApi.Contracts;
using HotelListingsApi.Data;

namespace HotelListingsApi.Repository
{
    public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
    {
        public CountriesRepository(HotelListingDbContext context) : base(context)
        {
        }
    }
}
