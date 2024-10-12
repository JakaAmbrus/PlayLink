namespace Shield.Api.Configurations;

public class Settings
{
    public FirebaseOptions Firebase { get; set; }
}

public class FirebaseOptions
{
    public string ProjectId { get; set; }
    public string ApiKey { get; set; }
    public string AuthDomain { get; set; }
    public string StorageBucket { get; set; }
    public string MessagingSenderId { get; set; }
    public string AppId { get; set; }
    public string ServiceAccountKeyPath { get; set; }
}