namespace DistroRepo;

public class Recipient
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string DisplayName => $"{FirstName} {LastName}";
    public bool EmailIsValid()
    {
        return EmailAddress.Contains("@");
    }
}