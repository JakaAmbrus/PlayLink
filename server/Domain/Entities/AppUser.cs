using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string FullName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } 
        public string ProfileDescription { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string ProfilePicturePublicId { get; set; }

        public ICollection<AppUserRole> UserRoles { get; set; }
        public ICollection<FriendRequest> SentFriendRequests { get; set; }
        public ICollection<FriendRequest> ReceivedFriendRequests { get; set; }
        public ICollection<Friendship> FriendsAsUser1 { get; set; }
        public ICollection<Friendship> FriendsAsUser2 { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<GroupChatUser> GroupChatUsers { get; set; }
        public ICollection<GroupMessage> GroupMessages { get; set; }
        public ICollection<PrivateMessage> MessagesSent { get; set; }
        public ICollection<PrivateMessage> MessagesReceived { get; set; }
    }
}
