namespace Shield.Api.Common.Configurations;

public class Settings
{
    public string[] AllowedOrigins { get; set; }
    public string SocialUrl { get; set; }
    public FirebaseOptions Firebase { get; set; }
    public GuestsOptions Guests { get; set; }
}

public class FirebaseOptions
{
    public string ProjectId { get; set; }
    public string ServiceAccountKeyPath { get; set; }
    public string UserEmailDomain { get; set; }
    public string ApiKey { get; set; }
    public FirestoreOptions Firestore { get; set; }
}

public class FirestoreOptions
{
    public string Collection { get; set; }
    public FirestoreFields Fields { get; set; }
}

public class FirestoreFields
{
    public string UserId { get; set; }
    public string SocialId { get; set; }
    public string Username { get; set; }
    public string RolesField { get; set; }
}

public class GuestsOptions
{
    public List<GuestAccount> Members { get; set; }
    public List<GuestAccount> Moderators { get; set; }
    public List<GuestAccount> Admins { get; set; }
}

public class GuestAccount
{
    public string Email { get; set; }
    public string Password { get; set; }
}