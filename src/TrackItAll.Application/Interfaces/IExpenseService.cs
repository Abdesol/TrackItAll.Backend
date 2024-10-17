using TrackItAll.Application.Dtos;
using TrackItAll.Domain.Entities;

namespace TrackItAll.Application.Interfaces;

/// <summary>
/// Interface for managing expenses within an application, providing methods to add, update, delete, and list expenses.
/// This interface interacts with Azure Cosmos DB for expense data management and Azure Table Storage for category retrieval.
/// </summary>
public interface IExpenseService
{
    /// <summary>
    /// Adds a new expense to the system.
    /// </summary>
    /// <param name="ownerId">The object id of the owner user.</param>
    /// <param name="amount">The amount of the expense.</param>
    /// <param name="description">A description of the expense.</param>
    /// <param name="categoryId">The ID of the category to which the expense belongs.</param>
    /// <returns>A task representing the asynchronous operation, with a result of the added <see cref="AddExpenseServiceResponseDto"/> object, which contains the details of the added expense.</returns>
    Task<AddExpenseServiceResponseDto> AddExpense(string ownerId, double amount, string description, int categoryId);

    /// <summary>
    /// Updates an existing expense in the system.
    /// </summary>
    /// <param name="id">The ID of the expense to update.</param>
    /// <param name="ownerId">The object id of the owner user.</param>
    /// <param name="amount">The new amount for the expense (optional).</param>
    /// <param name="description">The new description for the expense (optional).</param>
    /// <param name="categoryId">The new category ID for the expense (optional).</param>
    /// <param name="date">The new date for the expense (optional).</param>
    /// <returns>A task representing the asynchronous operation, with a result of the updated <see cref="UpdateExpenseServiceResponseDto"/> object, which contains the details of the updated expense.</returns>
    Task<UpdateExpenseServiceResponseDto> UpdateExpense(string id, string ownerId, double? amount = null,
        string? description = null, int? categoryId = null,
        DateTime? date = null);

    /// <summary>
    /// Deletes an expense from the system.
    /// </summary>
    /// <param name="id">The ID of the expense to delete.</param>
    /// <param name="ownerId">The object id of the owner user.</param>
    /// <returns>A task representing the asynchronous operation, with a result of the <see cref="DeleteExpenseServiceResponseDto"/> object, which indicates whether the deletion was successful and any additional relevant information.</returns>
    Task<DeleteExpenseServiceResponseDto> DeleteExpense(string id, string ownerId);

    /// <summary>
    /// Lists all expenses associated with a specific owner.
    /// </summary>
    /// <param name="ownerId">The ID of the owner whose expenses are to be listed.</param>
    /// <returns>A task representing the asynchronous operation, with a result of a list of <see cref="Expense"/> objects.</returns>
    Task<List<Expense>> ListExpenses(string ownerId);

    /// <summary>
    ///
    /// </summary>
    /// <param name="id">The ID of the expense to retrieve.</param>
    /// <param name="ownerId">The object id of the owner user.</param>
    /// <returns>The <see cref="Expense"/> object result</returns>
    Task<Expense?> GetExpense(string id, string ownerId);

    /// <summary>
    /// Retrieves a list of expense categories from Azure Table Storage.
    /// </summary>
    /// <returns>A list of <see cref="Category"/> objects.</returns>
    List<Category> GetCategories();

    /// <summary>
    /// Sets the receipt ID for an expense.
    /// </summary>
    /// <param name="id">id of the receipt from azure blob storage</param>
    /// <returns></returns>
    Task<bool> SetReceiptId(string expenseId, string ownerId, string? receiptId);
}