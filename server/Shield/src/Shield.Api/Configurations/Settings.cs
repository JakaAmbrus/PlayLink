namespace Shield.Api.Configurations;

public class Settings
{
    public FirebaseOptions Firebase { get; set; }
}

public class FirebaseOptions
{
    public string ProjectId { get; set; }
    public string ServiceAccountKeyPath { get; set; }
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
    public string Username { get; set; }
    public string RolesField { get; set; }
}