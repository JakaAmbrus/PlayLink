using Social.Application.Utils;

namespace Social.Application.Tests.Unit.Utils
{
    public class IsValidRoleTests
    {
        [Theory]
        [InlineData(null, false)]
        [InlineData(new string[] { "Member", "Admin", "Guest" }, true)]
        [InlineData(new string[] { "Member", "Knight" }, false)]
        [InlineData(new string[] { }, true)]

        public void IsValidRole_ShouldReturnTrue_WhenRolesAreValid(IEnumerable<string> roles, bool expected)
        {
            // Act
            var result = ValidationUtils.IsValidRole(roles);

            // Assert
            result.Should().Be(expected);
        }
    }
}
