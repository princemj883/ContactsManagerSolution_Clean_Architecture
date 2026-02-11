using ContactsManager.Infrastructure.DbContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Mvc.Testing;



namespace ContractsManager.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.UseEnvironment("Test");
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                x => x.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if(descriptor != null)
                services.Remove(descriptor);
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseInMemoryDatabase("DatabaseForTesting"));
        });
    }
}