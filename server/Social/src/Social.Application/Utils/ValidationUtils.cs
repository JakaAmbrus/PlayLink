using Social.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace Social.Application.Utils
{
    public static class ValidationUtils
    {
        //Used to validate if the user role is valid
        public static bool IsValidRole(IEnumerable<string> roles)
        {
            if (roles == null)
            {
                return false;
            }

            var validRoles = new HashSet<string> { Role.Member.ToString(), Role.Moderator.ToString(), Role.Admin.ToString(), Role.Guest.ToString() };
            return roles.All(role => validRoles.Contains(role));
        }

        //Used to validate if the country is valid
        public static bool IsValidCountry(string inputCountry)
        {
            if (string.IsNullOrWhiteSpace(inputCountry))
            {
                return false;
            }

            string normalizedCountry = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(inputCountry.ToLowerInvariant()).Replace(" ", "");

            return Enum.GetNames(typeof(Country)).Any(name => name.Equals(normalizedCountry, StringComparison.OrdinalIgnoreCase));
        }

        //Used to validate if the file is appropriate size
        public static bool IsAppropriateSizeFile(IFormFile file, int mb)
        {
            return file == null || file.Length <= mb * 1024 * 1024;
        }

        //Used to validate if the file is appropriate type
        public static bool IsAValidTypeFile(IFormFile file)
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
