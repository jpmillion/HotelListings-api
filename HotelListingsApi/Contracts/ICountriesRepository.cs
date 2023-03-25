using HotelListingsApi.Data;

namespace HotelListingsApi.Contracts
{
    public interface ICountriesRepository : IGenericRepository<Country>
    {
        Task<Country> GetDetails(int id);
    }
}
