using System.ComponentModel.DataAnnotations;

namespace WebAPI.Attributes
{
    //validator for the fullname property
    public class FullNameAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string fullName = value as string;

            if (string.IsNullOrWhiteSpace(fullName))
            {
                return false;
            }
            return fullName.Trim().Contains(" ");
        }
    }
}
