using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace Application.Utils
{
    public static class ValidationUtils
    {
        //Used to validate if the user role is valid
        public static bool BeValidRole(IEnumerable<string> roles)
        {
            var validRoles = new HashSet<string> { "Member", "Moderator", "Admin", "Guest" };
            return roles.All(role => validRoles.Contains(role));
        }

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

        //Used to validate if the file is appropriate size
        public static bool BeAppropriateSize(IFormFile file)
        {
            return file == null || file.Length <= 4 * 1024 * 1024;
        }

        //Used to validate if the file is appropriate type
        public static bool BeAValidType(IFormFile file)
        {
            var allowedTypes = new[] { "image/jpeg", "image/png" };
            return file == null || allowedTypes.Contains(file.ContentType);
        }

        //Used to validate if the url is valid
        public static bool IsValidUrl(string photoUrl)
        {
            return Uri.TryCreate(photoUrl, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp
                || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
