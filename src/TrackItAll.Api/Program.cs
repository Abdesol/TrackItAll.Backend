using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using TrackItAll.Api.Configuration;
using TrackItAll.Application.Interfaces;
using TrackItAll.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    builder.Configuration.GetSection("AzureAdB2C").Bind(options);
    options.Scope.Add(options.ClientId!);
    options.SaveTokens = true;
});
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));

builder.Services.AddAuthorization(options => { options.FallbackPolicy = options.DefaultPolicy; });

builder.Services.AddControllers();

builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IReceiptService, ReceiptService>();

builder.Services.AddCors(CorsConfig.CorsPolicyConfig);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(CorsConfig.CorsPolicyName);
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();