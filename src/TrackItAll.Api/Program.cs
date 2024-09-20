using TrackItAll.Api.Configuration;
using TrackItAll.Application.Interfaces;
using TrackItAll.Application.Services;
using TrackItAll.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureAuthorization();
builder.Services.AddCors(CorsConfig.CorsPolicyConfig);

builder.Services.AddControllers();

builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IReceiptService, ReceiptService>();
builder.Services.AddSingleton<IAzureAdTokenService, AzureAdTokenService>(_ => new AzureAdTokenService(
    builder.Configuration["AzureAdB2C:GraphUrl"]!,
    builder.Configuration["AzureAdB2C:TenantId"]!,
    builder.Configuration["AzureAdB2C:ClientId"]!,
    builder.Configuration["AzureAdB2C:ClientSecret"]!
));

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