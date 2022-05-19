using Restaurant.Data;

namespace Restaurant.Api.Extensions
{
    public static class HostExtensions
    {
        public static void InitializeDbContext(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var databaseInitializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();

                try
                {
                    Task.Run(async () =>
                    {
                        await databaseInitializer.InitializeAsync();
                    }).Wait();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
