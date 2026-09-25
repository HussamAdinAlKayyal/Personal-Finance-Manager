using Microsoft.AspNetCore.Identity;

namespace Finance.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public DateOnly DateOfBirth { get; private set; }

    public ApplicationUser(string firstName, string lastName, DateOnly dateOfBirth, string email)
    {
        SetFirstName(firstName);
        SetLastName(lastName);
        SetDateOfBirth(dateOfBirth);
        Email = UserName = email;
    }

    public void SetFirstName(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException("First name cannot be null or empty.", nameof(firstName));
        }
        FirstName = firstName;
    }

    public void SetLastName(string lastName)
    {

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Last name cannot be null or empty.", nameof(lastName));
        }
        LastName = lastName;
    }

    public void SetDateOfBirth(DateOnly dateOfBirth)
    {
        DateTime dateTimeOfBirth = new(dateOfBirth.Year, dateOfBirth.Month, dateOfBirth.Day),
                 now = DateTime.Now;
        if (dateTimeOfBirth > now)
        {
            throw new ArgumentException("Date of birth cannot be in the future.", nameof(dateOfBirth));
        }
        DateOfBirth = dateOfBirth;
    }
}
