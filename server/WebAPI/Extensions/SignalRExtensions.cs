namespace WebAPI.Extensions
{
    public static class SignalRExtensions
    {
        public static void AddSignalRExtensions(this IServiceCollection services)
        {
            services.AddSignalR();
        }
    }
}
