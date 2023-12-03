namespace WebAPI.SignalR
{
    public class PresenceTracker
    {
        private static readonly Dictionary<int, List<string>> OnlineUsers = new();

        public Task UserConnected(int userId, string connectionId)
        {
            lock (OnlineUsers)
            {
                if (OnlineUsers.ContainsKey(userId))
                {
                    OnlineUsers[userId].Add(connectionId);
                }
                else
                {
                    OnlineUsers.Add(userId, new List<string> { connectionId });
                }

                return Task.CompletedTask;
            }
        }

        public Task UserDisconnected(int userId, string connectionId)
        {
            lock (OnlineUsers)
            {
                if (!OnlineUsers.ContainsKey(userId)) return Task.CompletedTask;

                OnlineUsers[userId].Remove(connectionId);

                if (OnlineUsers[userId].Count == 0)
                {
                    OnlineUsers.Remove(userId);
                }

                return Task.CompletedTask;
            }
        }

        public Task<string[]> GetOnlineUsers()
        {
            lock (OnlineUsers)
            {
                return Task.FromResult(OnlineUsers
                    .OrderBy(k => k.Key)
                    .Select(k => k.Key.ToString())
                    .ToArray());
            }
        }
    }
}
