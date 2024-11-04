using Social.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Shared.Core.Security;

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

            var validRoles = new HashSet<string> { Roles.Member, Roles.Moderator, Roles.Admin, Roles.Guest };
            return roles.All(role => validRoles.Contains(role));
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
