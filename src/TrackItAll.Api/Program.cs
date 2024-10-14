using Microsoft.Azure.Cosmos;
using TrackItAll.Api.Configuration;
using TrackItAll.Application.Interfaces;
using TrackItAll.Application.Services;
using TrackItAll.Infrastructure.Authentication;
using TrackItAll.Infrastructure.Persistence;
using TrackItAll.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureAuthorization();
builder.Services.AddCors(CorsConfig.CorsPolicyConfig);

builder.Services.AddControllers();

builder.Services.AddSingleton<CosmosClient>(_ =>
    new CosmosClient(builder.Configuration["AzureCosmos:Endpoint"], 
        builder.Configuration["AzureCosmos:Key"]));

builder.Services.AddSingleton<IContainerFactory, ContainerFactory>();

builder.Services.AddScoped<IExpenseService, ExpenseService>(provider =>
{
    var containerFactory = provider.GetRequiredService<IContainerFactory>();
    var container = containerFactory.GetContainer(
        builder.Configuration["AzureCosmos:DatabaseName"]!,
        builder.Configuration["AzureCosmos:ContainerName"]!);
    
    return new ExpenseService(container);
});
    
builder.Services.AddScoped<IReceiptService, ReceiptService>();

builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddSingleton<IAzureAdTokenService, AzureAdTokenService>(_ => new AzureAdTokenService(
    builder.Configuration["AzureAdB2C:GraphUrl"]!,
    builder.Configuration["AzureAdB2C:TenantId"]!,
    builder.Configuration["AzureAdB2C:ClientId"]!,
    builder.Configuration["AzureAdB2C:ClientSecret"]!
));
builder.Services.AddScoped<AzureAdB2CHelper>();

builder.Services.AddSingleton<IQueueService, QueueService>(_ =>
    new QueueService(builder.Configuration["AzureStorage:ConnectionString"]!));

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