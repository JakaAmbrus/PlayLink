using Domain.Enums;
using System.Globalization;

namespace Application.Utils
{
    public static class ValidationUtils
    {
        //Used to validate if the country is valid
        public static bool IsValidCountry(string inputCountry)
        {
            if (string.IsNullOrWhiteSpace(inputCountry))
            {
                return false;
            }

            string normalizedCountry = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(inputCountry).Replace(" ", "");

            return Enum.TryParse<Country>(normalizedCountry, out _);
        }
    }
}
