//used to calculate age from date of birth, accounting for leap years 
using System.Reflection.Metadata;

namespace server.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateOnly dob)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            if (dob.Day == 29 && dob.Month == 2 && DateTime.IsLeapYear(dob.Year) && !DateTime.IsLeapYear(today.Year))
            {
                dob = dob.AddDays(-1); //this sets the date to 28th of February for non leap years
            }

            var age = today.Year - dob.Year;

            if (dob > today.AddYears(-age)) age--;

            return age;
        }
    }
}
