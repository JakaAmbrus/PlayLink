using Social.Application.Utils;

namespace Social.Application.Tests.Unit.Utils
{
    public class IsValidCountryTests
    {
        [Theory]
        [InlineData("", false)]
        [InlineData(" ", false)]
        [InlineData(null, false)]
        [InlineData("Narnia", false)]
        [InlineData("United States", true)]
        [InlineData("Slovenia", true)]
        [InlineData("slovenia", true)]
        [InlineData("UnitedStates", true)]
        public void IsValidCountry_ShouldReturnTrue_WhenTheInputCountryMatchesCountriesEnum(string inputCountry, bool expected)
        {
            // Act
            var result = ValidationUtils.IsValidCountry(inputCountry);

            // Assert
            result.Should().Be(expected);
        }
    }
}
