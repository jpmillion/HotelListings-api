namespace HotelListingsApi.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name, object key) : base($"{name} and {key} not found")
        {
            
        }
    }
}
