using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Infrastructure.Services
{
    public class CountryService : ICountryService
    {
        private readonly string _filePath;

        public CountryService(IConfiguration configuration)
        {
            _filePath = configuration["CountryFilePath"];
        }

        public IEnumerable<string> GetAllCountries()
        {
            var json = File.ReadAllText(_filePath);
            var countries = JsonConvert.DeserializeObject<List<string>>(json);
            return countries;
        }
    }
}
