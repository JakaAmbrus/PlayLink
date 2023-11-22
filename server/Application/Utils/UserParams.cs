namespace Application.Utils
{
    public class UserParams : PaginationParams
    {
    public string CurrentUsername { get; set; }
    public string Gender { get; set; }
    public int MinAge { get; set; } = 12;
    public int MaxAge { get; set; } = 99;
    public string OrderBy { get; set; } = "lastActive";
    }
}
