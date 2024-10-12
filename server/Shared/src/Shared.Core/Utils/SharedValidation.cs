using System.Globalization;
using Shared.Core.Enums;

namespace Shared.Core.Utils;

public static class SharedValidation
{
    /// <summary>
    /// Used to validate if the country is valid
    /// </summary>
    public static bool IsValidCountry(string inputCountry)
    {
        if (string.IsNullOrWhiteSpace(inputCountry))
        {
            return false;
        }

        string normalizedCountry = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(inputCountry.ToLowerInvariant()).Replace(" ", "");

        return Enum.GetNames(typeof(Country)).Any(name => name.Equals(normalizedCountry, StringComparison.OrdinalIgnoreCase));
    }
}