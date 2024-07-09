using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShortcutService.Services;

namespace ShortcutService.IntegrationTests;

[ExcludeFromCodeCoverage]
public class TestStartup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    public virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IShortcutService, ShortcutService.Services.ShortcutService>();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
    }

    public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}