namespace Shield.Api.Common.Models;

public class User
{
    public string UserId { get; set; }
    
    public string Username { get; set; }
    
    public int SocialId { get; set; }
    
    public List<string> Roles { get; set; }
}