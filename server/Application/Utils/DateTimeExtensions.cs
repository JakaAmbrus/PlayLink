namespace Application.Utils
{
    public static class DateTimeExtensions
    {
        //Gets the age from the users date of birth
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
