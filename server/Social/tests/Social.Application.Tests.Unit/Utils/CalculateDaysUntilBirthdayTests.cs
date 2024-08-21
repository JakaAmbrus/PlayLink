using Social.Application.Utils;
using FluentAssertions;

namespace Social.Application.Tests.Unit.Utils
{
    public class CalculateDaysUntilBirthdayTests
    {
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
