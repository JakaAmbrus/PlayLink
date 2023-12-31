using Application.Utils;
using Microsoft.AspNetCore.Http;

namespace Application.Tests.Unit.Utils
{
    public class IsAValidTypeFileTests
    {
        [Fact]
        public void IsAValidTypeFile_ShouldReturnTrue_WhenFileIsNull()
        {
            // Arrange
            IFormFile? file = null;

            // Act
            var result = ValidationUtils.IsAValidTypeFile(file);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("image/jpeg")]
        [InlineData("image/png")]
        public void IsAValidTypeFile_ShouldReturnTrue_WhenFileTypeIsValid(string fileType)
        {
            // Arrange
            var file = Substitute.For<IFormFile>();
            file.ContentType.Returns(fileType);

            // Act
            var result = ValidationUtils.IsAValidTypeFile(file);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsAValidTypeFile_ShouldReturnFalse_WhenFileTypeIsNotValid()
        {
            // Arrange
            var file = Substitute.For<IFormFile>();
            file.ContentType.Returns("image/svg");

            // Act
            var result = ValidationUtils.IsAValidTypeFile(file);

            // Assert
            result.Should().BeFalse();
        }
    }
}
