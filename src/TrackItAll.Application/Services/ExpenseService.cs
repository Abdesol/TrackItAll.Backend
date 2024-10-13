using TrackItAll.Application.Dtos;
using TrackItAll.Application.Interfaces;
using TrackItAll.Domain.Entities;

namespace TrackItAll.Application.Services;

/// <summary>
/// A service class for managing expenses within an application, providing methods to add, update, delete, and list expenses.
/// This service interacts with Azure Cosmos DB for expense data management and Azure Table Storage for category retrieval.
/// </summary>
public class ExpenseService : IExpenseService
{
    /// <inheritdoc />
    public async Task<AddExpenseServiceResponseDto> AddExpense(double amount, string description, int categoryId, DateTime date)
    {
        return new AddExpenseServiceResponseDto(true, new Expense());
    }

    /// <inheritdoc />
    public async Task<UpdateExpenseServiceResponseDto> UpdateExpense(string id, double? amount = null, string? description = null, int? categoryId = null,
        DateTime? date = null)
    {
        return new UpdateExpenseServiceResponseDto(true, new Expense());
    }

    /// <inheritdoc />
    public async Task<DeleteExpenseServiceResponseDto> DeleteExpense(string id)
    {
        return new DeleteExpenseServiceResponseDto(true);
    }

    /// <inheritdoc />
    public async Task<Expense?> GetExpense(string id)
    {
        return new Expense();
    }

    /// <inheritdoc />
    public async Task<List<Expense>> ListExpenses(string ownerId)
    {
        return [];
    }

    /// <inheritdoc />
    public async Task<List<Category>> GetCategories()
    {
        return [];
    }
}