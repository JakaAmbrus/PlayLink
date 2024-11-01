namespace Social.Domain.Entities
{
    public class Friendship
    {
        public int FriendshipId { get; set; }
        public int User1Id { get; set; }
        public int User2Id { get; set; }

        public User User1 { get; set; }
        public User User2 { get; set; }

        public DateTime DateEstablished { get; set; } = DateTime.UtcNow;
    }
}
