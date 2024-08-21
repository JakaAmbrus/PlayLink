namespace Social.Api.SignalR
{
    public class PresenceTracker
    {
        private static readonly Dictionary<int, List<string>> OnlineUsers = new();

        public Task<bool> UserConnected(int userId, string connectionId)
        {
            bool isOnline = false;

            lock (OnlineUsers)
            {
                if (OnlineUsers.ContainsKey(userId))
                {
                    OnlineUsers[userId].Add(connectionId);
                }
                else
                {
                    OnlineUsers.Add(userId, new List<string> { connectionId });
                    isOnline = true;
                }

                return Task.FromResult(isOnline);
            }
        }

        public Task<bool> UserDisconnected(int userId, string connectionId)
        {
            bool isOffline = false;

            lock (OnlineUsers)
            {
                if (!OnlineUsers.ContainsKey(userId)) return Task.FromResult(isOffline);

                OnlineUsers[userId].Remove(connectionId);

                if (OnlineUsers[userId].Count == 0)
                {
                    OnlineUsers.Remove(userId);
                    isOffline = true;
                }

                return Task.FromResult(isOffline);
            }
        }

        public Task<int[]> GetOnlineUsers()
        {
            lock (OnlineUsers)
            {
                return Task.FromResult(OnlineUsers
                    .OrderBy(k => k.Key)
                    .Select(k => k.Key)
                    .ToArray());
            }
        }

        public static Task<List<string>> GetConnectionsForUser(int userId)
        {
            List<string> connectionIds;

            lock (OnlineUsers)
            {
                connectionIds = OnlineUsers.GetValueOrDefault(userId);
            }

            return Task.FromResult(connectionIds);
        }
    }
}
