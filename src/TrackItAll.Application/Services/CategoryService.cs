using Azure.Data.Tables;
using Microsoft.Extensions.Hosting;
using TrackItAll.Application.Interfaces;
using TrackItAll.Domain.Entities;

namespace TrackItAll.Application.Services;

/// <summary>
/// A service class for managing expense categories within an application, providing methods to retrieve categories from Azure Table Storage.
/// </summary>
public class CategoryService(ICacheService cacheService, TableClient tableClient) : IHostedService
{
    public const string CacheKey = "ExpenseCategories";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(24);
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(() => RefreshCategoriesEvery24Hours(cancellationToken), cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task RefreshCategoriesEvery24Hours(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await LoadCategories();
            await Task.Delay(CacheDuration, stoppingToken);
        }
    }

    private async Task LoadCategories()
    {
        var categories = new List<Category>();
        await foreach (var category in tableClient.QueryAsync<Category>())
        {
            categories.Add(category);
        }
        
        cacheService.Set(CacheKey, categories, CacheDuration + TimeSpan.FromHours(1));
    }
}