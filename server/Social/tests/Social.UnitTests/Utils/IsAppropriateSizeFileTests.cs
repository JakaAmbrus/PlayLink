using Social.Application.Utils;
using Microsoft.AspNetCore.Http;

namespace Social.UnitTests.Utils
{
    public class IsAppropriateSizeFileTests
    {
        [Fact]
        public void IsAppropriateSize_ShouldReturnTrue_WhenFileIsNull()
        {
            // Arrange
            IFormFile? file = null;
            int fileSizeLimit = 5;

            // Act
            var result = ValidationUtils.IsAppropriateSizeFile(file, fileSizeLimit);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(2, 4)]
        [InlineData(5, 5)]
        public void IsAppropriateSize_ShouldReturnTrue_WhenFileSizeIsWithinSpecifiedLimit(int fileSize, int sizeLimit)
        {
            // Arrange
            var file = Substitute.For<IFormFile>();
            file.Length.Returns(fileSize * 1024 * 1024);

            // Act
            var result = ValidationUtils.IsAppropriateSizeFile(file, sizeLimit);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsAppropriateSize_ShouldReturnFalse_WhenFileSizeIsMoreThanLimit()
        {
            // Arrange
            var file = Substitute.For<IFormFile>();
            file.Length.Returns(8 * 1024 * 1024);
            int fileSizeLimit = 5;

            // Act
            var result = ValidationUtils.IsAppropriateSizeFile(file, fileSizeLimit);

            // Assert
            result.Should().BeFalse();
        }
    }
}
