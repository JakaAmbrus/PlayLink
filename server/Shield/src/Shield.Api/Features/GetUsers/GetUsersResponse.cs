namespace Shield.Api.Features.GetUsers;

public class GetUsersResponse
{
    public List<UserDto> Users { get; set; }
}

public class UserDto
{
    public string UserId { get; set; }
    public string Username { get; set; }
    public bool IsModerator { get; set; }
}