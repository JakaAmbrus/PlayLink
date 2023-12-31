namespace Application.Utils
{
    public static class DateTimeUtils
    {
        //Gets the age from the users date of birth
        public static int CalculateAge(this DateOnly dob)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            if (dob > today)
            {
                throw new ArgumentException("Date of birth cannot be in the future.");
            }

            if (dob.Day == 29 && dob.Month == 2 && DateTime.IsLeapYear(dob.Year) && !DateTime.IsLeapYear(today.Year))
            {
                dob = dob.AddDays(-1); //this sets the date to 28th of February for non leap years
            }

            var age = today.Year - dob.Year;

            if (dob > today.AddYears(-age)) age--;
            {
                return age;
            }
        }

        //Gets the days until the users birthday
        public static int CalculateDaysUntilBirthday(DateOnly birthday, DateOnly today)
        {
            var currentYearBirthday = new DateOnly(today.Year, birthday.Month, birthday.Day);

            if (currentYearBirthday < today)
            {
                currentYearBirthday = new DateOnly(today.Year + 1, birthday.Month, birthday.Day);
            }

            return currentYearBirthday.DayNumber - today.DayNumber;
        }
    }
}
