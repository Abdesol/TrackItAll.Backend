using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using TrackItAll.Application.Interfaces;
using TrackItAll.Application.Services;
using TrackItAll.Functions;

[assembly: FunctionsStartup(typeof(Startup))]
namespace TrackItAll.Functions;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddScoped<IEmailService, EmailService>();
    }
}