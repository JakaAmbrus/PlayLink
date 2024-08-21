using Social.Api.SignalR;

namespace Social.Api.Extensions
{
    public static class SignalRExtensions
    {
        public static void AddSignalRExtensions(this IServiceCollection services)
        {
            services.AddSignalR();

            services.AddSingleton<PresenceTracker>();
        }
    }
}
