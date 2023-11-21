namespace Infrastructure.Interfaces
{
    public interface ICountryService
    {
        IEnumerable<string> GetAllCountries();
    }
}
