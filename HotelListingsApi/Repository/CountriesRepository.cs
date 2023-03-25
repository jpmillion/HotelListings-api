using HotelListingsApi.Contracts;
using HotelListingsApi.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListingsApi.Repository
{
    public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
    {
        private readonly HotelListingDbContext _context;

        public CountriesRepository(HotelListingDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Country> GetDetails(int id)
        {
            return await _context.Countries
                .Include(country => country.Hotels)
                .SingleOrDefaultAsync(country => country.Id == id);
        }
    }
}
