using Social.Application.Utils;

namespace Social.UnitTests.Utils
{
    public class IsValidUrlTests
    {
        [Theory]
        [InlineData("", false)]
        [InlineData(null, false)]
        [InlineData("http://www.fakesite.com", true)]
        [InlineData("https://www.fakesite.com", true)]
        [InlineData("ftp://www.fakesite.com", false)]
        [InlineData("www.fakesite.com", false)]
        [InlineData("http://localhost", true)]
        [InlineData("https://192.168.0.1", true)]
        [InlineData("http://exa mple.com", false)]
        [InlineData("http:///fakesite.com", false)]
        public void IsValidUrl_ShouldReturnTrue_WhenTheUrlIsValid(string url, bool expected)
        {
            // Act
            var result = ValidationUtils.IsValidUrl(url);

            // Assert
            result.Should().Be(expected);
        }
    }
}
