using Social.Application.Utils;

namespace Social.Application.Tests.Unit.Utils
{
    public class CalculateAgeTests
    {
        [Fact]
        public void CalculateAge_ShouldThrowArgumentException_WhenDateOfBirthIsInFuture()
        {
            // Arrange
            var tomorrow = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));
            var dobInFuture = new DateOnly(tomorrow.Year, tomorrow.Month, tomorrow.Day);

            // Act
            var age = () => dobInFuture.CalculateAge();

            // Assert
            age.Should().Throw<ArgumentException>()
               .WithMessage("Date of birth cannot be in the future.");
        }

        [Theory]
        [InlineData(1999, 5, 16)]
        [InlineData(2000, 12, 25)]
        [InlineData(2020, 2, 21)]
        [InlineData(1900, 3, 28)]
        public void CalculateAge_ShouldReturnCorrectAge_WhenGivenValidDateOfBirth(
            int year, int month, int day)
        {
            // Arrange
            var dob = new DateOnly(year, month, day);
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var expectedAge = today.Year - dob.Year;

            if (dob.Month > today.Month || (dob.Month == today.Month && dob.Day > today.Day))
            {
                expectedAge--;
            }

            // Act
            var result = dob.CalculateAge();

            // Assert
            result.Should().Be(expectedAge);
        }

        [Fact]
        public void CalculateAge_ShouldReturnCorrectAge_WhenGivenValidDateOfBirthWithLeapYearsConsidered()
        {
            // Arrange
            var dob = new DateOnly(2000, 2, 29);
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var expectedAge = today.Year - dob.Year;
            if (dob > today.AddYears(-expectedAge)) expectedAge--;

            // Act
            var result = dob.CalculateAge();

            // Assert
            result.Should().Be(expectedAge);
        }

        [Fact]
        public void CalculateAge_ShouldReturnCorrectAge_WhenBirthdayIsYetToOccurThisYear()
        {
            // Arrange
            var dob = new DateOnly(2000, 12, 31);
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var expectedAge = today.Year - dob.Year;

            if (dob > today.AddYears(-expectedAge))
            {
                expectedAge--;
            }

            // Act
            var result = dob.CalculateAge();

            // Assert
            result.Should().Be(expectedAge);
        }

        [Fact]
        public void CalculateAge_ShouldReturnCorrectAge_WhenBirthdayAlreadyOccurredThisYear()
        {
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var dob = new DateOnly(today.Year - 20, 1, 1);

            // Act
            var result = dob.CalculateAge();

            // Assert
            result.Should().Be(20);
        }

        [Theory]
        [InlineData(24)]
        [InlineData(12)]
        [InlineData(99)]
        public void CalculateAge_ShouldReturnCorrectAge_WhenBirthdayIsTheCurrentDay(int year)
        {
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var dob = new DateOnly(today.Year - year, today.Month, today.Day);

            // Act
            var result = dob.CalculateAge();

            // Assert
            result.Should().Be(year);
        }
    }
}
