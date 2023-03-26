using HotelListingsApi.Contracts;
using HotelListingsApi.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListingsApi.Repository
{
    public class HotelsRepository : GenericRepository<Hotel>, IHotelsRepository
    {

        public HotelsRepository(HotelListingDbContext context) : base(context)
        {
   
        }

    }
}
