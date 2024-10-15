using System.Net;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using TrackItAll.Application.Dtos;
using TrackItAll.Application.Interfaces;
using TrackItAll.Domain.Entities;
using TrackItAll.Shared.Utils;

namespace TrackItAll.Application.Services;

/// <summary>
/// A service class for managing expenses within an application, providing methods to add, update, delete, and list expenses.
/// This service interacts with Azure Cosmos DB for expense data management and Azure Table Storage for category retrieval.
/// </summary>
/// <param name="container">The Cosmos DB container for expenses.</param>
public class ExpenseService(Container container) : IExpenseService
{
    /// <inheritdoc />
    public async Task<AddExpenseServiceResponseDto> AddExpense(string ownerId, double amount, string description, int categoryId)
    {
        var categories = await GetCategories();
        var categoryIdExist = categories.Select(c => c.Id).Contains(categoryId);
        if (!categoryIdExist)
            return new AddExpenseServiceResponseDto(false, ErrorMessage: "Category id doesn't exist");
        
        var expense = new Expense()
        {
            Id = UniqueIdGenerator.Generate(),
            OwnerId = ownerId,
            Date = DateTime.Now,
            Amount = amount,
            Description = description,
            CategoryId = categoryId,
            Category = categories.FirstOrDefault(c => c.Id == categoryId),
            ReceiptId = null
        };
        ItemResponse<Expense> response;
        try
        {
            response = await container.CreateItemAsync(expense, new PartitionKey(ownerId));
        }
        catch (CosmosException)
        {
            return new AddExpenseServiceResponseDto(false,
                ErrorMessage: "Unexpected error has occured while processing your request.");
        }

        return response.StatusCode == HttpStatusCode.Created
            ? new AddExpenseServiceResponseDto(true, expense)
            : new AddExpenseServiceResponseDto(false, ErrorMessage: "Failed to create expense. Please try again.");
    }

    /// <inheritdoc />
    public async Task<UpdateExpenseServiceResponseDto> UpdateExpense(string id, string ownerId, double? amount = null,
        string? description = null, int? categoryId = null,
        DateTime? date = null)
    {
        if (categoryId.HasValue)
        {
            var categoryIdExist = (await GetCategories()).Select(c => c.Id).Contains(categoryId.Value);
            if (!categoryIdExist)
                return new UpdateExpenseServiceResponseDto(false, ErrorMessage: "Category id doesn't exist");
        }
        
        try
        {
            var response = await container.ReadItemAsync<Expense>(id, new PartitionKey(ownerId));
            var expenseToUpdate = response.Resource;

            if (amount.HasValue) expenseToUpdate.Amount = amount.Value;
            if (!string.IsNullOrWhiteSpace(description)) expenseToUpdate.Description = description;
            if (categoryId.HasValue) expenseToUpdate.CategoryId = categoryId.Value;
            if (date.HasValue) expenseToUpdate.Date = date.Value;

            await container.ReplaceItemAsync(expenseToUpdate, id, new PartitionKey(ownerId));
            expenseToUpdate.Category = (await GetCategories()).FirstOrDefault(c => c.Id == expenseToUpdate.CategoryId);
            return new UpdateExpenseServiceResponseDto(true, expenseToUpdate);
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return new UpdateExpenseServiceResponseDto(false, ErrorMessage: "Expense not found.");
        }
        catch (CosmosException)
        {
            return new UpdateExpenseServiceResponseDto(false,
                ErrorMessage: "Unexpected error has occurred while processing your request.");
        }
    }

    /// <inheritdoc />
    public async Task<DeleteExpenseServiceResponseDto> DeleteExpense(string id, string ownerId)
    {
        try
        {
            await container.DeleteItemAsync<Expense>(id, new PartitionKey(ownerId));
            return new DeleteExpenseServiceResponseDto(true);
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return new DeleteExpenseServiceResponseDto(false, ErrorMessage: "Expense not found.");
        }
        catch (CosmosException)
        {
            return new DeleteExpenseServiceResponseDto(false, ErrorMessage: "Unexpected error has occurred while processing your request.");
        }
    }

    /// <inheritdoc />
    public async Task<Expense?> GetExpense(string id, string ownerId)
    {
        try
        {
            var response = await container.ReadItemAsync<Expense>(id, new PartitionKey(ownerId));
            var expense = response.Resource;
            expense.Category = (await GetCategories()).FirstOrDefault(c => c.Id == expense.CategoryId);
            return expense;
        }
        catch (CosmosException)
        {
            return null;
        }
    }

    /// <inheritdoc />
    public async Task<List<Expense>> ListExpenses(string ownerId)
    {
        var expenses = new List<Expense>();
        var query = $"SELECT * FROM c WHERE c.OwnerId = '{ownerId}'";

        var iterator = container.GetItemQueryIterator<Expense>(new QueryDefinition(query));
    
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            expenses.AddRange(response);
        }
        
        var categories = await GetCategories();
        expenses.ForEach(e => e.Category = categories.FirstOrDefault(c => c.Id == e.CategoryId));

        return expenses;
    }

    /// <inheritdoc />
    public async Task<List<Category>> GetCategories()
    {
        var categories = new List<Category>();
        for (var i = 0; i <= 5; i++)
        {
            categories.Add(new Category()
            {
                Id = i,
                Name = $"Category {i}"
            });
        }
        return categories;
    }
    
    /// <inheritdoc />
    public async Task<bool> SetReceiptId(string expenseId, string ownerId, string? receiptId)
    {
        try
        {
            var response = await container.ReadItemAsync<Expense>(expenseId, new PartitionKey(ownerId));
            var expenseToUpdate = response.Resource;

            expenseToUpdate.ReceiptId = receiptId;

            await container.ReplaceItemAsync(expenseToUpdate, expenseId, new PartitionKey(ownerId));

            return true;
        }
        catch (CosmosException)
        {
            return false;
        }
    }
}