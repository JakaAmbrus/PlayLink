using Domain.Enums;
using Microsoft.AspNetCore.Http;
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

        public static bool BeAppropriateSize(IFormFile file)
        {
            // The maximum size is 4 MB
            return file == null || file.Length <= 4 * 1024 * 1024;
        }

        public static bool BeAValidType(IFormFile file)
        {
            //Allow only JPEG and PNG files
            var allowedTypes = new[] { "image/jpeg", "image/png" };
            return file == null || allowedTypes.Contains(file.ContentType);
        }

        public static bool IsValidUrl(string photoUrl)
        {
            return Uri.TryCreate(photoUrl, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp
                || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
