using Application.Utils;
using FluentAssertions;

namespace Application.Tests.Unit.Utils
{
    public class DateTimeUtilsTests
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
            var expectedAge = DateOnly.FromDateTime(DateTime.UtcNow).Year - dob.Year;

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

        [Theory]
        [InlineData(2023, 5, 1, 120)]
        [InlineData(2023, 12, 25, 358)]
        [InlineData(2023, 2, 21, 51)]
        public void CalculateDaysUntilBirthday_ShouldReturnDaysUntilBirthday_WhenBirthdayIsInTheFutureThisYear(
            int year, int month, int day, int daysUntil)
        {
            // Arrange
            var today = new DateOnly(year, 1, 1);
            var futureBirthday = new DateOnly(year, month, day);
            var expectedDays = daysUntil;

            // Act
            var result = DateTimeUtils.CalculateDaysUntilBirthday(futureBirthday, today);

            // Assert
            result.Should().Be(expectedDays);
        }

        [Fact]
        public void CalculateDaysUntilBirthday_ShouldReturnZero_WhenTheCurrentDayIsTheBirthday()
        {
            // Arrange
            var today = new DateOnly(2023, 1, 1);
            var birthdayToday = today;

            // Act
            var result = DateTimeUtils.CalculateDaysUntilBirthday(birthdayToday, today);

            // Assert
            result.Should().Be(0);
        }

        [Fact]
        public void CalculateDaysUntilBirthday_ShouldReturnDaysUntilBirthdayNextYear_WhenBirthdayHasPassedThisYear()
        {
            // Arrange
            var today = new DateOnly(2023, 12, 31);
            var birthday = new DateOnly(1999, 1, 1);
            var expectedDays = 1;

            // Act
            var result = DateTimeUtils.CalculateDaysUntilBirthday(birthday, today);

            // Assert
            result.Should().Be(expectedDays);
        }
    }
}
